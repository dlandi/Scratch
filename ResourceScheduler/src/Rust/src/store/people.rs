use sqlx::SqlitePool;
use uuid::Uuid;

use crate::error::{ServiceError, ServiceResult};
use crate::models::people::{PersonCreate, PersonDto, PersonUpdate};

pub async fn list(pool: &SqlitePool) -> ServiceResult<Vec<PersonDto>> {
    let rows =
        sqlx::query_as::<_, PersonDto>("SELECT person_id, name, email FROM people ORDER BY name")
            .fetch_all(pool)
            .await?;
    Ok(rows)
}

pub async fn create(pool: &SqlitePool, input: PersonCreate) -> ServiceResult<PersonDto> {
    let id = Uuid::new_v4();
    sqlx::query("INSERT INTO people (person_id, name, email) VALUES (?, ?, ?)")
        .bind(id)
        .bind(&input.name)
        .bind(&input.email)
        .execute(pool)
        .await?;
    Ok(PersonDto {
        person_id: id,
        name: input.name,
        email: input.email,
    })
}

pub async fn update(pool: &SqlitePool, id: Uuid, input: PersonUpdate) -> ServiceResult<PersonDto> {
    let result = sqlx::query("UPDATE people SET name = ?, email = ? WHERE person_id = ?")
        .bind(&input.name)
        .bind(&input.email)
        .bind(id)
        .execute(pool)
        .await?;
    if result.rows_affected() == 0 {
        return Err(ServiceError::NotFound);
    }
    Ok(PersonDto {
        person_id: id,
        name: input.name,
        email: input.email,
    })
}

pub async fn delete(pool: &SqlitePool, id: Uuid) -> ServiceResult<()> {
    let result = sqlx::query("DELETE FROM people WHERE person_id = ?")
        .bind(id)
        .execute(pool)
        .await?;
    if result.rows_affected() == 0 {
        return Err(ServiceError::NotFound);
    }
    Ok(())
}
