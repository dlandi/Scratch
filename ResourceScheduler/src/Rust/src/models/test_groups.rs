use serde::{Deserialize, Serialize};
use uuid::Uuid;

#[derive(Debug, Clone, Serialize)]
#[serde(rename_all = "camelCase")]
pub struct TestGroupDto {
    pub test_group_id: Uuid,
    pub name: String,
    pub member_ids: Vec<Uuid>,
    pub version: i32,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct TestGroupCreate {
    pub name: String,
    pub member_ids: Vec<Uuid>,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct TestGroupUpdate {
    pub name: String,
    pub member_ids: Vec<Uuid>,
}
