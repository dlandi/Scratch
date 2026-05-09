use sqlx::SqlitePool;
use uuid::Uuid;

use crate::error::{ServiceError, ServiceResult};
use crate::models::buildings::{BuildingCreate, BuildingDto, BuildingUpdate};

pub async fn list(pool: &SqlitePool) -> ServiceResult<Vec<BuildingDto>> {
    let rows = sqlx::query_as::<_, BuildingDto>(
        "SELECT building_id, name, address, version FROM buildings ORDER BY name",
    )
    .fetch_all(pool)
    .await?;
    Ok(rows)
}

pub async fn get(pool: &SqlitePool, id: Uuid) -> ServiceResult<Option<BuildingDto>> {
    let row = sqlx::query_as::<_, BuildingDto>(
        "SELECT building_id, name, address, version FROM buildings WHERE building_id = ?",
    )
    .bind(id)
    .fetch_optional(pool)
    .await?;
    Ok(row)
}

pub async fn create(pool: &SqlitePool, input: BuildingCreate) -> ServiceResult<BuildingDto> {
    let id = Uuid::new_v4();
    let version: i32 = 1;
    sqlx::query("INSERT INTO buildings (building_id, name, address, version) VALUES (?, ?, ?, ?)")
        .bind(id)
        .bind(&input.name)
        .bind(&input.address)
        .bind(version)
        .execute(pool)
        .await?;
    Ok(BuildingDto {
        building_id: id,
        name: input.name,
        address: input.address,
        version,
    })
}

pub async fn update(
    pool: &SqlitePool,
    id: Uuid,
    input: BuildingUpdate,
    expected_version: i32,
) -> ServiceResult<BuildingDto> {
    // Fetch first so 404 and 409 can be distinguished. The C# client
    // maps 404 to NotFoundException and 409 to ConflictException, so the
    // server must return the correct one.
    let current = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }

    let new_version = current.version + 1;
    sqlx::query(
        "UPDATE buildings \
         SET name = ?, address = ?, version = ? \
         WHERE building_id = ? AND version = ?",
    )
    .bind(&input.name)
    .bind(&input.address)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(pool)
    .await?;

    Ok(BuildingDto {
        building_id: id,
        name: input.name,
        address: input.address,
        version: new_version,
    })
}

pub async fn delete(pool: &SqlitePool, id: Uuid, expected_version: i32) -> ServiceResult<()> {
    let current = get(pool, id).await?.ok_or(ServiceError::NotFound)?;
    if current.version != expected_version {
        return Err(ServiceError::Conflict);
    }
    // R15: Building cannot be deleted while any Device references it.
    if crate::store::devices::any_in_building(pool, id).await? {
        return Err(ServiceError::validation(
            "R15",
            format!(
                "Building '{}' has devices assigned. Reassign or delete those devices first.",
                current.name
            ),
        ));
    }
    sqlx::query("DELETE FROM buildings WHERE building_id = ? AND version = ?")
        .bind(id)
        .bind(expected_version)
        .execute(pool)
        .await?;
    Ok(())
}
