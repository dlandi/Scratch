use chrono::{DateTime, Utc};
use sqlx::SqlitePool;
use uuid::Uuid;

use crate::error::{ServiceError, ServiceResult};
use crate::models::enums::{DeviceGroupStatus, ReservationStatus};
use crate::models::reservations::{ReservationCreate, ReservationDto, ReservationFilter};

const SELECT_COLS: &str =
    "reservation_id, device_group_id, test_group_id, start_utc, end_utc, status, notes, version";

/// Canonical text form for `DateTime<Utc>` columns. Always nine
/// fractional digits and a trailing `Z` so every row has identical
/// width, which lets SQLite `<` / `>` / `ORDER BY` on the raw TEXT
/// column agree with chronological order. Without this, sqlx-sqlite's
/// default RFC 3339 encoder picks variable precision (no fractional
/// for whole seconds, otherwise milli/micro/nano) and lex order
/// disagrees with time order at the `.` vs `Z` byte position.
pub(crate) fn fmt_utc(dt: DateTime<Utc>) -> String {
    dt.format("%Y-%m-%dT%H:%M:%S%.9fZ").to_string()
}

/// Render a UTC timestamp the way C# `DateTime.ToString("u")` does
/// (`yyyy-MM-dd HH:mm:ssZ`, fixed seconds resolution). Used inside R10
/// and R11 message text so the in-memory and Rust backends produce
/// byte-identical violation strings.
fn fmt_msg_utc(dt: DateTime<Utc>) -> String {
    dt.format("%Y-%m-%d %H:%M:%SZ").to_string()
}

pub async fn list(
    pool: &SqlitePool,
    filter: &ReservationFilter,
) -> ServiceResult<Vec<ReservationDto>> {
    // Build the query dynamically based on which filter fields are set.
    // Comparisons run on the raw TEXT column rather than via SQLite's
    // datetime() function so sub-second precision is preserved; see
    // fmt_utc for why every value uses a fixed nine-digit form.
    let mut sql = format!("SELECT {SELECT_COLS} FROM reservations WHERE 1 = 1");
    if filter.device_group_id.is_some() {
        sql.push_str(" AND device_group_id = ?");
    }
    if filter.test_group_id.is_some() {
        sql.push_str(" AND test_group_id = ?");
    }
    if filter.from_utc.is_some() {
        sql.push_str(" AND end_utc > ?");
    }
    if filter.to_utc.is_some() {
        sql.push_str(" AND start_utc < ?");
    }
    if !filter.status_in.is_empty() {
        sql.push_str(" AND status IN (");
        for i in 0..filter.status_in.len() {
            if i > 0 {
                sql.push_str(", ");
            }
            sql.push('?');
        }
        sql.push(')');
    }
    sql.push_str(" ORDER BY start_utc");

    let mut q = sqlx::query_as::<_, ReservationDto>(&sql);
    if let Some(id) = filter.device_group_id {
        q = q.bind(id);
    }
    if let Some(id) = filter.test_group_id {
        q = q.bind(id);
    }
    if let Some(t) = filter.from_utc {
        q = q.bind(fmt_utc(t));
    }
    if let Some(t) = filter.to_utc {
        q = q.bind(fmt_utc(t));
    }
    for s in &filter.status_in {
        q = q.bind(*s);
    }
    let rows = q.fetch_all(pool).await?;
    Ok(rows)
}

pub async fn create(pool: &SqlitePool, input: ReservationCreate) -> ServiceResult<ReservationDto> {
    // R13: end must be strictly after start.
    if input.end_utc <= input.start_utc {
        return Err(ServiceError::validation(
            "R13",
            "Reservation end must be after start.",
        ));
    }
    // R8: device-group and test-group must exist.
    let dg_exists: bool =
        sqlx::query_scalar("SELECT EXISTS(SELECT 1 FROM device_groups WHERE device_group_id = ?)")
            .bind(input.device_group_id)
            .fetch_one(pool)
            .await?;
    if !dg_exists {
        return Err(ServiceError::validation(
            "R8",
            "Reservation references unknown Device-Group.",
        ));
    }
    let tg_exists: bool =
        sqlx::query_scalar("SELECT EXISTS(SELECT 1 FROM test_groups WHERE test_group_id = ?)")
            .bind(input.test_group_id)
            .fetch_one(pool)
            .await?;
    if !tg_exists {
        return Err(ServiceError::validation(
            "R8",
            "Reservation references unknown Test-Group.",
        ));
    }

    let id = Uuid::new_v4();
    let version: i32 = 1;
    let start_utc = input.start_utc.with_timezone(&Utc);
    let end_utc = input.end_utc.with_timezone(&Utc);
    let status = ReservationStatus::Pending;

    sqlx::query(
        "INSERT INTO reservations \
         (reservation_id, device_group_id, test_group_id, start_utc, end_utc, status, notes, version) \
         VALUES (?, ?, ?, ?, ?, ?, ?, ?)",
    )
    .bind(id)
    .bind(input.device_group_id)
    .bind(input.test_group_id)
    .bind(fmt_utc(start_utc))
    .bind(fmt_utc(end_utc))
    .bind(status)
    .bind(&input.notes)
    .bind(version)
    .execute(pool)
    .await?;

    Ok(ReservationDto {
        reservation_id: id,
        device_group_id: input.device_group_id,
        test_group_id: input.test_group_id,
        start_utc,
        end_utc,
        status,
        notes: input.notes,
        version,
    })
}

pub async fn confirm(
    pool: &SqlitePool,
    id: Uuid,
    expected_version: i32,
) -> ServiceResult<ReservationDto> {
    let current = get_internal(pool, id)
        .await?
        .ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }

    // R9: target Device-Group must be Active.
    let dg_row = sqlx::query_as::<_, (String, DeviceGroupStatus)>(
        "SELECT name, status FROM device_groups WHERE device_group_id = ?",
    )
    .bind(current.device_group_id)
    .fetch_optional(pool)
    .await?;
    let Some((dg_name, dg_status)) = dg_row else {
        return Err(ServiceError::validation(
            "R9",
            "Reservation references unknown Device-Group.",
        ));
    };
    if dg_status != DeviceGroupStatus::Active {
        return Err(ServiceError::validation(
            "R9",
            format!(
                "Reservation cannot be confirmed: Device-Group '{dg_name}' is {}.",
                match dg_status {
                    DeviceGroupStatus::Active => "Active",
                    DeviceGroupStatus::Inactive => "Inactive",
                }
            ),
        ));
    }

    // R10: no overlap with another Confirmed reservation on the same
    // Device-Group. Overlap test: candidate.start < existing.end AND
    // candidate.end > existing.start.
    let r10_clash: Option<(chrono::DateTime<chrono::Utc>, chrono::DateTime<chrono::Utc>)> =
        sqlx::query_as(
            "SELECT start_utc, end_utc FROM reservations \
             WHERE reservation_id != ? \
               AND device_group_id = ? \
               AND status = 'Confirmed' \
               AND ? < end_utc \
               AND ? > start_utc \
             LIMIT 1",
        )
        .bind(id)
        .bind(current.device_group_id)
        .bind(fmt_utc(current.start_utc))
        .bind(fmt_utc(current.end_utc))
        .fetch_optional(pool)
        .await?;
    if let Some((existing_start, existing_end)) = r10_clash {
        return Err(ServiceError::validation(
            "R10",
            format!(
                "Reservation cannot be confirmed: overlaps a confirmed booking on Device-Group '{dg_name}' from {} to {}.",
                fmt_msg_utc(existing_start),
                fmt_msg_utc(existing_end),
            ),
        ));
    }

    // R11: no overlap with another Confirmed reservation on the same
    // Test-Group.
    let tg_name: Option<String> =
        sqlx::query_scalar("SELECT name FROM test_groups WHERE test_group_id = ?")
            .bind(current.test_group_id)
            .fetch_optional(pool)
            .await?;
    let tg_name = tg_name.unwrap_or_else(|| current.test_group_id.to_string());

    let r11_clash: Option<(chrono::DateTime<chrono::Utc>, chrono::DateTime<chrono::Utc>)> =
        sqlx::query_as(
            "SELECT start_utc, end_utc FROM reservations \
             WHERE reservation_id != ? \
               AND test_group_id = ? \
               AND status = 'Confirmed' \
               AND ? < end_utc \
               AND ? > start_utc \
             LIMIT 1",
        )
        .bind(id)
        .bind(current.test_group_id)
        .bind(fmt_utc(current.start_utc))
        .bind(fmt_utc(current.end_utc))
        .fetch_optional(pool)
        .await?;
    if let Some((existing_start, existing_end)) = r11_clash {
        return Err(ServiceError::validation(
            "R11",
            format!(
                "Reservation cannot be confirmed: Test-Group '{tg_name}' already has a confirmed booking from {} to {}.",
                fmt_msg_utc(existing_start),
                fmt_msg_utc(existing_end),
            ),
        ));
    }

    let new_version = current.version + 1;
    sqlx::query(
        "UPDATE reservations SET status = ?, version = ? \
         WHERE reservation_id = ? AND version = ?",
    )
    .bind(ReservationStatus::Confirmed)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(pool)
    .await?;

    Ok(ReservationDto {
        status: ReservationStatus::Confirmed,
        version: new_version,
        ..current
    })
}

pub async fn cancel(
    pool: &SqlitePool,
    id: Uuid,
    expected_version: i32,
) -> ServiceResult<ReservationDto> {
    let current = get_internal(pool, id)
        .await?
        .ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }
    let new_version = current.version + 1;
    sqlx::query(
        "UPDATE reservations SET status = ?, version = ? \
         WHERE reservation_id = ? AND version = ?",
    )
    .bind(ReservationStatus::Cancelled)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(pool)
    .await?;
    Ok(ReservationDto {
        status: ReservationStatus::Cancelled,
        version: new_version,
        ..current
    })
}

async fn get_internal(pool: &SqlitePool, id: Uuid) -> ServiceResult<Option<ReservationDto>> {
    let row = sqlx::query_as::<_, ReservationDto>(&format!(
        "SELECT {SELECT_COLS} FROM reservations WHERE reservation_id = ?"
    ))
    .bind(id)
    .fetch_optional(pool)
    .await?;
    Ok(row)
}
