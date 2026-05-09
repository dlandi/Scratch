//! Loads the shared seed-data.json fixture into a fresh database.
//!
//! Both the .NET InMemoryClientService and this module read the same
//! file (embedded as `seed-data.json` from the repo's `src/`). The
//! fixture is unchanging across runs except for reservation
//! timestamps, which are reconstituted at seed time from
//! `(dayOffset, startHour, endHour)` anchored to local midnight, so a
//! `9` in the JSON renders as 09:00 wall-clock in whatever zone the
//! seeding host runs in.
//!
//! `seed_if_empty` is a no-op when any seedable table already has
//! rows, so re-running the binary against a populated database does
//! not duplicate data. Seeding is gated by the `SEED_DEMO_DATA` env
//! var in `main.rs`; production deployments should leave it unset.

use chrono::{DateTime, Local, TimeZone, Utc};
use serde::Deserialize;
use sqlx::SqlitePool;
use uuid::Uuid;

use crate::error::ServiceResult;
use crate::models::enums::{DeviceGroupStatus, DeviceStatus, ReservationStatus};

const SEED_JSON: &str = include_str!("../../seed-data.json");

pub async fn seed_if_empty(pool: &SqlitePool) -> ServiceResult<()> {
    if has_any_data(pool).await? {
        tracing::info!("seed: skipped (database already has data)");
        return Ok(());
    }
    tracing::info!("seed: applying demo fixture from seed-data.json");
    let data: SeedData = serde_json::from_str(SEED_JSON)
        .map_err(|e| anyhow::anyhow!("invalid seed-data.json: {e}"))?;
    apply(pool, &data).await?;
    tracing::info!(
        buildings = data.buildings.len(),
        devices = data.devices.len(),
        device_groups = data.device_groups.len(),
        people = data.people.len(),
        test_groups = data.test_groups.len(),
        reservations = data.reservations.len(),
        "seed: applied"
    );
    Ok(())
}

async fn has_any_data(pool: &SqlitePool) -> ServiceResult<bool> {
    let total: i64 = sqlx::query_scalar(
        "SELECT (SELECT COUNT(*) FROM buildings) \
              + (SELECT COUNT(*) FROM devices) \
              + (SELECT COUNT(*) FROM device_groups) \
              + (SELECT COUNT(*) FROM people) \
              + (SELECT COUNT(*) FROM test_groups) \
              + (SELECT COUNT(*) FROM reservations)",
    )
    .fetch_one(pool)
    .await?;
    Ok(total > 0)
}

async fn apply(pool: &SqlitePool, data: &SeedData) -> ServiceResult<()> {
    let mut tx = pool.begin().await?;

    for b in &data.buildings {
        sqlx::query(
            "INSERT INTO buildings (building_id, name, address, version) VALUES (?, ?, ?, 1)",
        )
        .bind(b.building_id)
        .bind(&b.name)
        .bind(&b.address)
        .execute(&mut *tx)
        .await?;
    }

    for d in &data.devices {
        sqlx::query(
            "INSERT INTO devices \
             (device_id, name, status, building_id, assigned_device_group_id, version) \
             VALUES (?, ?, ?, ?, NULL, 1)",
        )
        .bind(d.device_id)
        .bind(&d.name)
        .bind(d.status)
        .bind(d.building_id)
        .execute(&mut *tx)
        .await?;
    }

    for p in &data.people {
        sqlx::query("INSERT INTO people (person_id, name, email) VALUES (?, ?, ?)")
            .bind(p.person_id)
            .bind(&p.name)
            .bind(&p.email)
            .execute(&mut *tx)
            .await?;
    }

    for t in &data.test_groups {
        sqlx::query("INSERT INTO test_groups (test_group_id, name, version) VALUES (?, ?, 1)")
            .bind(t.test_group_id)
            .bind(&t.name)
            .execute(&mut *tx)
            .await?;
        for (ordinal, person_id) in t.member_ids.iter().enumerate() {
            sqlx::query(
                "INSERT INTO test_group_members (test_group_id, person_id, ordinal) \
                 VALUES (?, ?, ?)",
            )
            .bind(t.test_group_id)
            .bind(person_id)
            .bind(ordinal as i32)
            .execute(&mut *tx)
            .await?;
        }
    }

    for g in &data.device_groups {
        sqlx::query(
            "INSERT INTO device_groups (device_group_id, name, status, version) \
             VALUES (?, ?, ?, 1)",
        )
        .bind(g.device_group_id)
        .bind(&g.name)
        .bind(g.status)
        .execute(&mut *tx)
        .await?;

        for (ordinal, device_id) in g.device_ids.iter().enumerate() {
            sqlx::query(
                "INSERT INTO device_group_members (device_group_id, device_id, ordinal) \
                 VALUES (?, ?, ?)",
            )
            .bind(g.device_group_id)
            .bind(device_id)
            .bind(ordinal as i32)
            .execute(&mut *tx)
            .await?;
        }

        for c in &g.connections {
            sqlx::query(
                "INSERT INTO device_group_connections \
                 (connection_id, device_group_id, from_device_id, to_device_id, label) \
                 VALUES (?, ?, ?, ?, ?)",
            )
            .bind(c.connection_id)
            .bind(g.device_group_id)
            .bind(c.from_device_id)
            .bind(c.to_device_id)
            .bind(&c.label)
            .execute(&mut *tx)
            .await?;
        }

        for l in &g.layout {
            sqlx::query(
                "INSERT INTO device_group_layout (device_group_id, device_id, x, y) \
                 VALUES (?, ?, ?, ?)",
            )
            .bind(g.device_group_id)
            .bind(l.device_id)
            .bind(l.x)
            .bind(l.y)
            .execute(&mut *tx)
            .await?;
        }

        // Reflect the convenience pointer on each member device for
        // active groups, mirroring ActivateDeviceGroupAsync.
        if g.status == DeviceGroupStatus::Active {
            sqlx::query(
                "UPDATE devices SET assigned_device_group_id = ? \
                 WHERE device_id IN \
                   (SELECT device_id FROM device_group_members WHERE device_group_id = ?)",
            )
            .bind(g.device_group_id)
            .bind(g.device_group_id)
            .execute(&mut *tx)
            .await?;
        }
    }

    // Compute today's local midnight, in UTC. AddHours(N) on this value
    // lands on local N:00 today after the UI's ToLocal round-trip,
    // independent of the host's UTC offset. Same pattern as the .NET
    // seeder so reservations look identical across backends.
    let local_now = Local::now();
    let local_midnight_naive = local_now
        .date_naive()
        .and_hms_opt(0, 0, 0)
        .expect("00:00:00 is a valid time");
    let today_local = Local
        .from_local_datetime(&local_midnight_naive)
        .single()
        .expect("midnight is unambiguous in any IANA zone");
    let today_utc: DateTime<Utc> = today_local.with_timezone(&Utc);

    for r in &data.reservations {
        let day_base = today_utc + chrono::Duration::days(r.day_offset);
        let start = day_base + chrono::Duration::hours(r.start_hour);
        let end = day_base + chrono::Duration::hours(r.end_hour);
        let id = Uuid::new_v4();
        sqlx::query(
            "INSERT INTO reservations \
             (reservation_id, device_group_id, test_group_id, \
              start_utc, end_utc, status, notes, version) \
             VALUES (?, ?, ?, ?, ?, ?, ?, 1)",
        )
        .bind(id)
        .bind(r.device_group_id)
        .bind(r.test_group_id)
        .bind(start)
        .bind(end)
        .bind(r.status)
        .bind(&r.notes)
        .execute(&mut *tx)
        .await?;
    }

    tx.commit().await?;
    Ok(())
}

// ---- wire shapes for seed-data.json ----

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedData {
    buildings: Vec<SeedBuilding>,
    devices: Vec<SeedDevice>,
    device_groups: Vec<SeedDeviceGroup>,
    people: Vec<SeedPerson>,
    test_groups: Vec<SeedTestGroup>,
    reservations: Vec<SeedReservation>,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedBuilding {
    building_id: Uuid,
    name: String,
    address: String,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedDevice {
    device_id: Uuid,
    name: String,
    status: DeviceStatus,
    building_id: Uuid,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedDeviceGroup {
    device_group_id: Uuid,
    name: String,
    status: DeviceGroupStatus,
    device_ids: Vec<Uuid>,
    connections: Vec<SeedConnection>,
    layout: Vec<SeedLayoutEntry>,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedConnection {
    connection_id: Uuid,
    from_device_id: Uuid,
    to_device_id: Uuid,
    label: String,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedLayoutEntry {
    device_id: Uuid,
    x: f64,
    y: f64,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedPerson {
    person_id: Uuid,
    name: String,
    email: Option<String>,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedTestGroup {
    test_group_id: Uuid,
    name: String,
    member_ids: Vec<Uuid>,
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct SeedReservation {
    device_group_id: Uuid,
    test_group_id: Uuid,
    day_offset: i64,
    start_hour: i64,
    end_hour: i64,
    status: ReservationStatus,
    notes: Option<String>,
}
