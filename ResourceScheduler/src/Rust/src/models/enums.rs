use serde::{Deserialize, Serialize};

/// Lifecycle status of a single Device. Spec 7.1 / `DeviceStatus.cs`.
#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash, Serialize, Deserialize, sqlx::Type)]
#[sqlx(rename_all = "PascalCase")]
pub enum DeviceStatus {
    Available,
    Maintenance,
    Offline,
    Retired,
}

/// Lifecycle status of a Device-Group. Spec 7.2.
#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash, Serialize, Deserialize, sqlx::Type)]
#[sqlx(rename_all = "PascalCase")]
pub enum DeviceGroupStatus {
    Inactive,
    Active,
}

/// Lifecycle status of a Reservation. Spec 7.3.
#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash, Serialize, Deserialize, sqlx::Type)]
#[sqlx(rename_all = "PascalCase")]
pub enum ReservationStatus {
    Pending,
    Confirmed,
    Cancelled,
    Completed,
}

impl std::str::FromStr for ReservationStatus {
    type Err = String;
    fn from_str(s: &str) -> Result<Self, Self::Err> {
        match s {
            "Pending" => Ok(Self::Pending),
            "Confirmed" => Ok(Self::Confirmed),
            "Cancelled" => Ok(Self::Cancelled),
            "Completed" => Ok(Self::Completed),
            other => Err(format!("invalid ReservationStatus: {other}")),
        }
    }
}
