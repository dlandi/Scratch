use serde::{Deserialize, Serialize};
use uuid::Uuid;

#[derive(Debug, Clone, Serialize, sqlx::FromRow)]
#[serde(rename_all = "camelCase")]
pub struct BuildingDto {
    pub building_id: Uuid,
    pub name: String,
    pub address: String,
    pub version: i32,
}

#[derive(Debug, Deserialize)]
pub struct BuildingCreate {
    pub name: String,
    pub address: String,
}

#[derive(Debug, Deserialize)]
pub struct BuildingUpdate {
    pub name: String,
    pub address: String,
}
