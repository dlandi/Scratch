use serde::{Deserialize, Serialize};
use uuid::Uuid;

use super::enums::DeviceStatus;

#[derive(Debug, Clone, Serialize, sqlx::FromRow)]
#[serde(rename_all = "camelCase")]
pub struct DeviceDto {
    pub device_id: Uuid,
    pub name: String,
    pub status: DeviceStatus,
    pub building_id: Uuid,
    pub assigned_device_group_id: Option<Uuid>,
    pub version: i32,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceCreate {
    pub name: String,
    pub status: DeviceStatus,
    pub building_id: Uuid,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceUpdate {
    pub name: String,
    pub status: DeviceStatus,
    pub building_id: Uuid,
}
