use chrono::{DateTime, Utc};
use serde::{Deserialize, Serialize};
use uuid::Uuid;

use super::enums::ReservationStatus;

#[derive(Debug, Clone, Serialize, sqlx::FromRow)]
#[serde(rename_all = "camelCase")]
pub struct ReservationDto {
    pub reservation_id: Uuid,
    pub device_group_id: Uuid,
    pub test_group_id: Uuid,
    pub start_utc: DateTime<Utc>,
    pub end_utc: DateTime<Utc>,
    pub status: ReservationStatus,
    pub notes: Option<String>,
    pub version: i32,
}

#[derive(Debug, Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct ReservationCreate {
    pub device_group_id: Uuid,
    pub test_group_id: Uuid,
    pub start_utc: DateTime<Utc>,
    pub end_utc: DateTime<Utc>,
    pub notes: Option<String>,
}

/// Filter passed via query string. Each field is optional. `from_utc`
/// is an exclusive lower bound on `EndUtc`; `to_utc` is an exclusive
/// upper bound on `StartUtc`. `status_in` is a repeated query
/// parameter, one per status value.
#[derive(Debug, Default)]
pub struct ReservationFilter {
    pub device_group_id: Option<Uuid>,
    pub test_group_id: Option<Uuid>,
    pub from_utc: Option<DateTime<Utc>>,
    pub to_utc: Option<DateTime<Utc>>,
    pub status_in: Vec<ReservationStatus>,
}
