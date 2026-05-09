use serde::{Deserialize, Serialize};
use uuid::Uuid;

use super::enums::DeviceGroupStatus;

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceGroupDto {
    pub device_group_id: Uuid,
    pub name: String,
    pub status: DeviceGroupStatus,
    pub device_ids: Vec<Uuid>,
    pub connections: Vec<DeviceConnectionDto>,
    pub layout: Vec<DeviceLayoutEntry>,
    pub version: i32,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceConnectionDto {
    pub connection_id: Uuid,
    pub from_device_id: Uuid,
    pub to_device_id: Uuid,
    pub label: String,
}

#[derive(Debug, Clone, Serialize, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceLayoutEntry {
    pub device_id: Uuid,
    pub x: f64,
    pub y: f64,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceGroupCreate {
    pub name: String,
    pub device_ids: Vec<Uuid>,
    pub connections: Vec<DeviceConnectionDto>,
    pub layout: Vec<DeviceLayoutEntry>,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct DeviceGroupUpdate {
    pub name: String,
    pub device_ids: Vec<Uuid>,
    pub connections: Vec<DeviceConnectionDto>,
    pub layout: Vec<DeviceLayoutEntry>,
}
