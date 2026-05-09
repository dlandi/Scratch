use sqlx::{Row as _, SqlitePool};
use uuid::Uuid;

use crate::error::{ServiceError, ServiceResult};
use crate::models::test_groups::{TestGroupCreate, TestGroupDto, TestGroupUpdate};

pub async fn list(pool: &SqlitePool) -> ServiceResult<Vec<TestGroupDto>> {
    let roots = sqlx::query("SELECT test_group_id, name, version FROM test_groups ORDER BY name")
        .fetch_all(pool)
        .await?;
    let mut out = Vec::with_capacity(roots.len());
    for r in roots {
        let id: Uuid = r.get("test_group_id");
        let name: String = r.get("name");
        let version: i32 = r.get("version");
        let member_ids = load_members(pool, id).await?;
        out.push(TestGroupDto {
            test_group_id: id,
            name,
            member_ids,
            version,
        });
    }
    Ok(out)
}

pub async fn create(pool: &SqlitePool, input: TestGroupCreate) -> ServiceResult<TestGroupDto> {
    let id = Uuid::new_v4();
    let version: i32 = 1;
    let mut tx = pool.begin().await?;
    sqlx::query("INSERT INTO test_groups (test_group_id, name, version) VALUES (?, ?, ?)")
        .bind(id)
        .bind(&input.name)
        .bind(version)
        .execute(&mut *tx)
        .await?;
    insert_members(&mut tx, id, &input.member_ids).await?;
    tx.commit().await?;
    Ok(TestGroupDto {
        test_group_id: id,
        name: input.name,
        member_ids: input.member_ids,
        version,
    })
}

pub async fn update(
    pool: &SqlitePool,
    id: Uuid,
    input: TestGroupUpdate,
    expected_version: i32,
) -> ServiceResult<TestGroupDto> {
    let current = sqlx::query("SELECT version FROM test_groups WHERE test_group_id = ?")
        .bind(id)
        .fetch_optional(pool)
        .await?
        .ok_or(ServiceError::NotFound)?;
    let current_version: i32 = current.get("version");
    if current_version != expected_version {
        return Err(ServiceError::Conflict);
    }
    let new_version = current_version + 1;
    let mut tx = pool.begin().await?;
    sqlx::query(
        "UPDATE test_groups SET name = ?, version = ? WHERE test_group_id = ? AND version = ?",
    )
    .bind(&input.name)
    .bind(new_version)
    .bind(id)
    .bind(expected_version)
    .execute(&mut *tx)
    .await?;
    sqlx::query("DELETE FROM test_group_members WHERE test_group_id = ?")
        .bind(id)
        .execute(&mut *tx)
        .await?;
    insert_members(&mut tx, id, &input.member_ids).await?;
    tx.commit().await?;
    Ok(TestGroupDto {
        test_group_id: id,
        name: input.name,
        member_ids: input.member_ids,
        version: new_version,
    })
}

pub async fn delete(pool: &SqlitePool, id: Uuid, expected_version: i32) -> ServiceResult<()> {
    let current = sqlx::query("SELECT version FROM test_groups WHERE test_group_id = ?")
        .bind(id)
        .fetch_optional(pool)
        .await?
        .ok_or(ServiceError::NotFound)?;
    let current_version: i32 = current.get("version");
    if current_version != expected_version {
        return Err(ServiceError::Conflict);
    }
    let mut tx = pool.begin().await?;
    sqlx::query("DELETE FROM test_group_members WHERE test_group_id = ?")
        .bind(id)
        .execute(&mut *tx)
        .await?;
    sqlx::query("DELETE FROM test_groups WHERE test_group_id = ? AND version = ?")
        .bind(id)
        .bind(expected_version)
        .execute(&mut *tx)
        .await?;
    tx.commit().await?;
    Ok(())
}

pub async fn exists(pool: &SqlitePool, id: Uuid) -> ServiceResult<bool> {
    let exists: bool =
        sqlx::query_scalar("SELECT EXISTS(SELECT 1 FROM test_groups WHERE test_group_id = ?)")
            .bind(id)
            .fetch_one(pool)
            .await?;
    Ok(exists)
}

async fn load_members(pool: &SqlitePool, group_id: Uuid) -> ServiceResult<Vec<Uuid>> {
    let ids: Vec<Uuid> = sqlx::query_scalar(
        "SELECT person_id FROM test_group_members WHERE test_group_id = ? ORDER BY ordinal",
    )
    .bind(group_id)
    .fetch_all(pool)
    .await?;
    Ok(ids)
}

async fn insert_members(
    tx: &mut sqlx::Transaction<'_, sqlx::Sqlite>,
    group_id: Uuid,
    member_ids: &[Uuid],
) -> ServiceResult<()> {
    for (ordinal, person_id) in member_ids.iter().enumerate() {
        sqlx::query(
            "INSERT INTO test_group_members (test_group_id, person_id, ordinal) VALUES (?, ?, ?)",
        )
        .bind(group_id)
        .bind(person_id)
        .bind(ordinal as i32)
        .execute(&mut **tx)
        .await?;
    }
    Ok(())
}
