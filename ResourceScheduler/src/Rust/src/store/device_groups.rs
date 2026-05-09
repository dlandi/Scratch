use sqlx::{Row as _, SqlitePool};
use uuid::Uuid;

use crate::error::{ServiceError, ServiceResult};
use crate::models::device_groups::{
    DeviceConnectionDto, DeviceGroupCreate, DeviceGroupDto, DeviceGroupUpdate, DeviceLayoutEntry,
};
use crate::models::enums::DeviceGroupStatus;

pub async fn list(pool: &SqlitePool) -> ServiceResult<Vec<DeviceGroupDto>> {
    let roots = sqlx::query(
        "SELECT device_group_id, name, status, version FROM device_groups ORDER BY name",
    )
    .fetch_all(pool)
    .await?;
    let mut out = Vec::with_capacity(roots.len());
    for r in roots {
        let id: Uuid = r.get("device_group_id");
        let name: String = r.get("name");
        let status: DeviceGroupStatus = r.get("status");
        let version: i32 = r.get("version");
        out.push(assemble(pool, id, name, status, version).await?);
    }
    Ok(out)
}

pub async fn get(pool: &SqlitePool, id: Uuid) -> ServiceResult<Option<DeviceGroupDto>> {
    let row = sqlx::query(
        "SELECT device_group_id, name, status, version FROM device_groups WHERE device_group_id = ?",
    )
    .bind(id)
    .fetch_optional(pool)
    .await?;
    let Some(row) = row else {
        return Ok(None);
    };
    let name: String = row.get("name");
    let status: DeviceGroupStatus = row.get("status");
    let version: i32 = row.get("version");
    Ok(Some(assemble(pool, id, name, status, version).await?))
}

pub async fn create(pool: &SqlitePool, input: DeviceGroupCreate) -> ServiceResult<DeviceGroupDto> {
    validate_connection_membership(&input.name, &input.device_ids, &input.connections)?;

    let id = Uuid::new_v4();
    let version: i32 = 1;
    // Materialize connection ids once so the DB rows and the response
    // body agree on which freshly-minted UUIDs were assigned.
    let connections: Vec<DeviceConnectionDto> = input
        .connections
        .into_iter()
        .map(materialize_connection_id)
        .collect();

    let mut tx = pool.begin().await?;

    sqlx::query(
        "INSERT INTO device_groups (device_group_id, name, status, version) VALUES (?, ?, ?, ?)",
    )
    .bind(id)
    .bind(&input.name)
    .bind(DeviceGroupStatus::Inactive)
    .bind(version)
    .execute(&mut *tx)
    .await?;

    insert_children(&mut tx, id, &input.device_ids, &connections, &input.layout).await?;

    tx.commit().await?;

    Ok(DeviceGroupDto {
        device_group_id: id,
        name: input.name,
        status: DeviceGroupStatus::Inactive,
        device_ids: input.device_ids,
        connections,
        layout: input.layout,
        version,
    })
}

pub async fn update(
    pool: &SqlitePool,
    id: Uuid,
    input: DeviceGroupUpdate,
    expected_version: i32,
) -> ServiceResult<DeviceGroupDto> {
    let current =
        sqlx::query("SELECT name, status, version FROM device_groups WHERE device_group_id = ?")
            .bind(id)
            .fetch_optional(pool)
            .await?
            .ok_or(ServiceError::NotFound)?;
    let current_version: i32 = current.get("version");
    if current_version != expected_version {
        return Err(ServiceError::Conflict);
    }
    let current_status: DeviceGroupStatus = current.get("status");

    validate_connection_membership(&input.name, &input.device_ids, &input.connections)?;

    let new_version = current_version + 1;
    let connections: Vec<DeviceConnectionDto> = input
        .connections
        .into_iter()
        .map(materialize_connection_id)
        .collect();

    let mut tx = pool.begin().await?;

    sqlx::query(
        "UPDATE device_groups SET name = ?, version = ? WHERE device_group_id = ? AND version = ?",
    )
    .bind(&input.name)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(&mut *tx)
    .await?;

    delete_children(&mut tx, id).await?;
    insert_children(&mut tx, id, &input.device_ids, &connections, &input.layout).await?;

    tx.commit().await?;

    Ok(DeviceGroupDto {
        device_group_id: id,
        name: input.name,
        status: current_status,
        device_ids: input.device_ids,
        connections,
        layout: input.layout,
        version: new_version,
    })
}

pub async fn activate(
    pool: &SqlitePool,
    id: Uuid,
    expected_version: i32,
) -> ServiceResult<DeviceGroupDto> {
    let mut tx = pool.begin().await?;

    let current =
        sqlx::query("SELECT name, status, version FROM device_groups WHERE device_group_id = ?")
            .bind(id)
            .fetch_optional(&mut *tx)
            .await?
            .ok_or(ServiceError::NotFound)?;
    let current_version: i32 = current.get("version");
    if current_version != expected_version {
        return Err(ServiceError::Conflict);
    }
    let group_name: String = current.get("name");

    // R7: must have at least one member.
    let member_count: i32 =
        sqlx::query_scalar("SELECT COUNT(*) FROM device_group_members WHERE device_group_id = ?")
            .bind(id)
            .fetch_one(&mut *tx)
            .await?;
    if member_count == 0 {
        return Err(ServiceError::validation(
            "R7",
            format!("Device-Group '{group_name}' has no members and cannot be activated."),
        ));
    }

    // R5: no member may be Offline / Maintenance / Retired.
    let bad = sqlx::query(
        "SELECT d.name AS name, d.status AS status \
         FROM device_group_members m \
         JOIN devices d ON d.device_id = m.device_id \
         WHERE m.device_group_id = ? \
           AND d.status IN ('Offline', 'Maintenance', 'Retired') \
         LIMIT 1",
    )
    .bind(id)
    .fetch_optional(&mut *tx)
    .await?;
    if let Some(row) = bad {
        let dev_name: String = row.get("name");
        let dev_status: String = row.get("status");
        return Err(ServiceError::validation(
            "R5",
            format!(
                "Device-Group '{group_name}' cannot be activated: member device '{dev_name}' is {dev_status}.",
            ),
        ));
    }

    // R3: no member device may be in another currently-Active group.
    let clash = sqlx::query(
        "SELECT d.name AS device_name, g_other.name AS other_group \
         FROM device_group_members m_self \
         JOIN device_group_members m_other \
           ON m_other.device_id = m_self.device_id \
          AND m_other.device_group_id != m_self.device_group_id \
         JOIN device_groups g_other \
           ON g_other.device_group_id = m_other.device_group_id \
         JOIN devices d ON d.device_id = m_self.device_id \
         WHERE m_self.device_group_id = ? AND g_other.status = 'Active' \
         LIMIT 1",
    )
    .bind(id)
    .fetch_optional(&mut *tx)
    .await?;
    if let Some(row) = clash {
        let dev_name: String = row.get("device_name");
        let other_name: String = row.get("other_group");
        return Err(ServiceError::validation(
            "R3",
            format!(
                "Device-Group '{group_name}' cannot be activated: device '{dev_name}' is already deployed in active group '{other_name}'.",
            ),
        ));
    }

    let new_version = current_version + 1;
    sqlx::query(
        "UPDATE device_groups SET status = ?, version = ? \
         WHERE device_group_id = ? AND version = ?",
    )
    .bind(DeviceGroupStatus::Active)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(&mut *tx)
    .await?;

    // Refresh the convenience pointer on each member device.
    sqlx::query(
        "UPDATE devices SET assigned_device_group_id = ? \
         WHERE device_id IN (SELECT device_id FROM device_group_members WHERE device_group_id = ?)",
    )
    .bind(id)
    .bind(id)
    .execute(&mut *tx)
    .await?;

    tx.commit().await?;

    let group = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    Ok(group)
}

pub async fn deactivate(
    pool: &SqlitePool,
    id: Uuid,
    expected_version: i32,
) -> ServiceResult<DeviceGroupDto> {
    let mut tx = pool.begin().await?;

    let current = sqlx::query("SELECT version FROM device_groups WHERE device_group_id = ?")
        .bind(id)
        .fetch_optional(&mut *tx)
        .await?
        .ok_or(ServiceError::NotFound)?;
    let current_version: i32 = current.get("version");
    if current_version != expected_version {
        return Err(ServiceError::Conflict);
    }

    let new_version = current_version + 1;
    sqlx::query(
        "UPDATE device_groups SET status = ?, version = ? \
         WHERE device_group_id = ? AND version = ?",
    )
    .bind(DeviceGroupStatus::Inactive)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(&mut *tx)
    .await?;

    tx.commit().await?;

    let group = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    Ok(group)
}

pub async fn delete(pool: &SqlitePool, id: Uuid, expected_version: i32) -> ServiceResult<()> {
    let mut tx = pool.begin().await?;

    let current = sqlx::query("SELECT version FROM device_groups WHERE device_group_id = ?")
        .bind(id)
        .fetch_optional(&mut *tx)
        .await?
        .ok_or(ServiceError::NotFound)?;
    let current_version: i32 = current.get("version");
    if current_version != expected_version {
        return Err(ServiceError::Conflict);
    }

    delete_children(&mut tx, id).await?;
    sqlx::query("DELETE FROM device_groups WHERE device_group_id = ? AND version = ?")
        .bind(id)
        .bind(expected_version)
        .execute(&mut *tx)
        .await?;

    tx.commit().await?;
    Ok(())
}

// ---- helpers ----

async fn assemble(
    pool: &SqlitePool,
    id: Uuid,
    name: String,
    status: DeviceGroupStatus,
    version: i32,
) -> ServiceResult<DeviceGroupDto> {
    let device_ids: Vec<Uuid> = sqlx::query_scalar(
        "SELECT device_id FROM device_group_members WHERE device_group_id = ? ORDER BY ordinal",
    )
    .bind(id)
    .fetch_all(pool)
    .await?;

    let conn_rows = sqlx::query(
        "SELECT connection_id, from_device_id, to_device_id, label \
         FROM device_group_connections WHERE device_group_id = ?",
    )
    .bind(id)
    .fetch_all(pool)
    .await?;
    let connections = conn_rows
        .into_iter()
        .map(|r| DeviceConnectionDto {
            connection_id: r.get("connection_id"),
            from_device_id: r.get("from_device_id"),
            to_device_id: r.get("to_device_id"),
            label: r.get("label"),
        })
        .collect();

    let layout_rows =
        sqlx::query("SELECT device_id, x, y FROM device_group_layout WHERE device_group_id = ?")
            .bind(id)
            .fetch_all(pool)
            .await?;
    let layout = layout_rows
        .into_iter()
        .map(|r| DeviceLayoutEntry {
            device_id: r.get("device_id"),
            x: r.get("x"),
            y: r.get("y"),
        })
        .collect();

    Ok(DeviceGroupDto {
        device_group_id: id,
        name,
        status,
        device_ids,
        connections,
        layout,
        version,
    })
}

async fn insert_children(
    tx: &mut sqlx::Transaction<'_, sqlx::Sqlite>,
    group_id: Uuid,
    device_ids: &[Uuid],
    connections: &[DeviceConnectionDto],
    layout: &[DeviceLayoutEntry],
) -> ServiceResult<()> {
    for (ordinal, device_id) in device_ids.iter().enumerate() {
        sqlx::query(
            "INSERT INTO device_group_members (device_group_id, device_id, ordinal) VALUES (?, ?, ?)",
        )
        .bind(group_id)
        .bind(device_id)
        .bind(ordinal as i32)
        .execute(&mut **tx)
        .await?;
    }

    for c in connections {
        let connection_id = if c.connection_id == Uuid::nil() {
            Uuid::new_v4()
        } else {
            c.connection_id
        };
        sqlx::query(
            "INSERT INTO device_group_connections \
             (connection_id, device_group_id, from_device_id, to_device_id, label) \
             VALUES (?, ?, ?, ?, ?)",
        )
        .bind(connection_id)
        .bind(group_id)
        .bind(c.from_device_id)
        .bind(c.to_device_id)
        .bind(&c.label)
        .execute(&mut **tx)
        .await?;
    }

    for entry in layout {
        sqlx::query(
            "INSERT INTO device_group_layout (device_group_id, device_id, x, y) VALUES (?, ?, ?, ?)",
        )
        .bind(group_id)
        .bind(entry.device_id)
        .bind(entry.x)
        .bind(entry.y)
        .execute(&mut **tx)
        .await?;
    }

    Ok(())
}

async fn delete_children(
    tx: &mut sqlx::Transaction<'_, sqlx::Sqlite>,
    group_id: Uuid,
) -> ServiceResult<()> {
    sqlx::query("DELETE FROM device_group_members WHERE device_group_id = ?")
        .bind(group_id)
        .execute(&mut **tx)
        .await?;
    sqlx::query("DELETE FROM device_group_connections WHERE device_group_id = ?")
        .bind(group_id)
        .execute(&mut **tx)
        .await?;
    sqlx::query("DELETE FROM device_group_layout WHERE device_group_id = ?")
        .bind(group_id)
        .execute(&mut **tx)
        .await?;
    Ok(())
}

fn validate_connection_membership(
    group_name: &str,
    device_ids: &[Uuid],
    connections: &[DeviceConnectionDto],
) -> ServiceResult<()> {
    let members: std::collections::HashSet<Uuid> = device_ids.iter().copied().collect();
    for c in connections {
        if !members.contains(&c.from_device_id) {
            return Err(ServiceError::validation(
                "R6",
                format!(
                    "Device-Group '{group_name}' has a connection whose FromDeviceId {} is not a member of the group.",
                    c.from_device_id
                ),
            ));
        }
        if !members.contains(&c.to_device_id) {
            return Err(ServiceError::validation(
                "R6",
                format!(
                    "Device-Group '{group_name}' has a connection whose ToDeviceId {} is not a member of the group.",
                    c.to_device_id
                ),
            ));
        }
    }
    Ok(())
}

fn materialize_connection_id(c: DeviceConnectionDto) -> DeviceConnectionDto {
    if c.connection_id == Uuid::nil() {
        DeviceConnectionDto {
            connection_id: Uuid::new_v4(),
            ..c
        }
    } else {
        c
    }
}
