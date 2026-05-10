use sqlx::SqlitePool;
use uuid::Uuid;

use crate::error::{ServiceError, ServiceResult};
use crate::models::devices::{DeviceCreate, DeviceDto, DeviceUpdate};

// `assigned_device_group_id` on the row is a denormalized pointer that
// only the activate path keeps fresh; remove/update/deactivate/delete
// don't touch it, so reading it directly produces stale or missing
// values. Compute it on read instead from the canonical membership +
// status join. R1 guarantees at most one active group per device, so
// LIMIT 1 is exact.
const SELECT_COLS: &str = "d.device_id, d.name, d.status, d.building_id, \
                           (SELECT m.device_group_id FROM device_group_members m \
                            JOIN device_groups g ON g.device_group_id = m.device_group_id \
                            WHERE m.device_id = d.device_id AND g.status = 'Active' \
                            LIMIT 1) AS assigned_device_group_id, \
                           d.version";

pub async fn list(pool: &SqlitePool) -> ServiceResult<Vec<DeviceDto>> {
    let rows = sqlx::query_as::<_, DeviceDto>(&format!(
        "SELECT {SELECT_COLS} FROM devices d ORDER BY d.name"
    ))
    .fetch_all(pool)
    .await?;
    Ok(rows)
}

pub async fn get(pool: &SqlitePool, id: Uuid) -> ServiceResult<Option<DeviceDto>> {
    let row = sqlx::query_as::<_, DeviceDto>(&format!(
        "SELECT {SELECT_COLS} FROM devices d WHERE d.device_id = ?"
    ))
    .bind(id)
    .fetch_optional(pool)
    .await?;
    Ok(row)
}

pub async fn create(pool: &SqlitePool, input: DeviceCreate) -> ServiceResult<DeviceDto> {
    if !building_exists(pool, input.building_id).await? {
        return Err(ServiceError::validation(
            "R14",
            "Device must reference an existing Building.",
        ));
    }
    let id = Uuid::new_v4();
    let version: i32 = 1;
    sqlx::query(
        "INSERT INTO devices \
         (device_id, name, status, building_id, assigned_device_group_id, version) \
         VALUES (?, ?, ?, ?, NULL, ?)",
    )
    .bind(id)
    .bind(&input.name)
    .bind(input.status)
    .bind(input.building_id)
    .bind(version)
    .execute(pool)
    .await?;
    Ok(DeviceDto {
        device_id: id,
        name: input.name,
        status: input.status,
        building_id: input.building_id,
        assigned_device_group_id: None,
        version,
    })
}

pub async fn update(
    pool: &SqlitePool,
    id: Uuid,
    input: DeviceUpdate,
    expected_version: i32,
) -> ServiceResult<DeviceDto> {
    let current = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }
    if !building_exists(pool, input.building_id).await? {
        return Err(ServiceError::validation(
            "R14",
            "Device must reference an existing Building.",
        ));
    }
    let new_version = current.version + 1;
    sqlx::query(
        "UPDATE devices \
         SET name = ?, status = ?, building_id = ?, version = ? \
         WHERE device_id = ? AND version = ?",
    )
    .bind(&input.name)
    .bind(input.status)
    .bind(input.building_id)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(pool)
    .await?;
    Ok(DeviceDto {
        device_id: id,
        name: input.name,
        status: input.status,
        building_id: input.building_id,
        assigned_device_group_id: current.assigned_device_group_id,
        version: new_version,
    })
}

pub async fn delete(pool: &SqlitePool, id: Uuid, expected_version: i32) -> ServiceResult<()> {
    let current = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }
    sqlx::query("DELETE FROM devices WHERE device_id = ? AND version = ?")
        .bind(id)
        .bind(expected_version)
        .execute(pool)
        .await?;
    Ok(())
}

async fn building_exists(pool: &SqlitePool, building_id: Uuid) -> ServiceResult<bool> {
    let exists: bool =
        sqlx::query_scalar("SELECT EXISTS(SELECT 1 FROM buildings WHERE building_id = ?)")
            .bind(building_id)
            .fetch_one(pool)
            .await?;
    Ok(exists)
}

/// Used by the building-delete handler (R15) once Devices exist.
pub async fn any_in_building(pool: &SqlitePool, building_id: Uuid) -> ServiceResult<bool> {
    let exists: bool =
        sqlx::query_scalar("SELECT EXISTS(SELECT 1 FROM devices WHERE building_id = ?)")
            .bind(building_id)
            .fetch_one(pool)
            .await?;
    Ok(exists)
}
