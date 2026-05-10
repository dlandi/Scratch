use std::str::FromStr as _;

use axum::{
    Json, Router,
    extract::{Path, RawQuery, State},
    http::StatusCode,
    response::IntoResponse,
    routing::{get, post},
};
use chrono::{DateTime, Utc};
use uuid::Uuid;

use crate::{
    AppState,
    error::{ServiceError, ServiceResult},
    extractors::IfMatch,
    models::{
        enums::ReservationStatus,
        reservations::{ReservationCreate, ReservationDto, ReservationFilter},
    },
    store::reservations as store,
};

pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/reservations", get(list).post(create))
        .route("/api/reservations/{id}/confirm", post(confirm))
        .route("/api/reservations/{id}/cancel", post(cancel))
}

async fn list(
    State(state): State<AppState>,
    RawQuery(query): RawQuery,
) -> ServiceResult<Json<Vec<ReservationDto>>> {
    let filter = parse_filter(query.as_deref().unwrap_or(""))?;
    Ok(Json(store::list(&state.pool, &filter).await?))
}

async fn create(
    State(state): State<AppState>,
    Json(input): Json<ReservationCreate>,
) -> ServiceResult<impl IntoResponse> {
    let row = store::create(&state.pool, input).await?;
    Ok((StatusCode::CREATED, Json(row)))
}

async fn confirm(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<Json<ReservationDto>> {
    Ok(Json(store::confirm(&state.pool, id, version).await?))
}

async fn cancel(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<Json<ReservationDto>> {
    Ok(Json(store::cancel(&state.pool, id, version).await?))
}

/// Parses the reservation list filter from a raw query string. Supports
/// `deviceGroupId`, `testGroupId`, `fromUtc`, `toUtc`, and repeated
/// `statusIn` parameters; unknown keys are ignored. A malformed value
/// yields `400 Bad Request` with no body.
fn parse_filter(query: &str) -> ServiceResult<ReservationFilter> {
    let mut filter = ReservationFilter::default();
    if query.is_empty() {
        return Ok(filter);
    }
    for (key, value) in url::form_urlencoded::parse(query.as_bytes()) {
        match key.as_ref() {
            "deviceGroupId" => {
                filter.device_group_id =
                    Some(Uuid::parse_str(&value).map_err(|_| bad_query("deviceGroupId"))?);
            }
            "testGroupId" => {
                filter.test_group_id =
                    Some(Uuid::parse_str(&value).map_err(|_| bad_query("testGroupId"))?);
            }
            "fromUtc" => {
                filter.from_utc = Some(parse_utc(&value).map_err(|_| bad_query("fromUtc"))?);
            }
            "toUtc" => {
                filter.to_utc = Some(parse_utc(&value).map_err(|_| bad_query("toUtc"))?);
            }
            "statusIn" => {
                let s = ReservationStatus::from_str(&value).map_err(|_| bad_query("statusIn"))?;
                filter.status_in.push(s);
            }
            _ => {}
        }
    }
    Ok(filter)
}

fn parse_utc(value: &str) -> Result<DateTime<Utc>, chrono::ParseError> {
    DateTime::parse_from_rfc3339(value).map(|dt| dt.with_timezone(&Utc))
}

fn bad_query(field: &str) -> ServiceError {
    ServiceError::BadRequest(format!("invalid query parameter: {field}"))
}
