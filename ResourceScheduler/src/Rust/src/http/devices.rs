use axum::{
    Json, Router,
    extract::{Path, State},
    http::StatusCode,
    response::IntoResponse,
    routing::get,
};
use uuid::Uuid;

use crate::{
    AppState,
    error::{ServiceError, ServiceResult},
    extractors::IfMatch,
    models::devices::{DeviceCreate, DeviceDto, DeviceUpdate},
    store::devices as store,
};

pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/devices", get(list).post(create))
        .route(
            "/api/devices/{id}",
            get(get_one).put(update).delete(delete_one),
        )
}

async fn list(State(state): State<AppState>) -> ServiceResult<Json<Vec<DeviceDto>>> {
    Ok(Json(store::list(&state.pool).await?))
}

async fn get_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
) -> ServiceResult<Json<DeviceDto>> {
    match store::get(&state.pool, id).await? {
        Some(d) => Ok(Json(d)),
        None => Err(ServiceError::NotFound),
    }
}

async fn create(
    State(state): State<AppState>,
    Json(input): Json<DeviceCreate>,
) -> ServiceResult<impl IntoResponse> {
    let row = store::create(&state.pool, input).await?;
    Ok((StatusCode::CREATED, Json(row)))
}

async fn update(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
    Json(input): Json<DeviceUpdate>,
) -> ServiceResult<Json<DeviceDto>> {
    Ok(Json(store::update(&state.pool, id, input, version).await?))
}

async fn delete_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<StatusCode> {
    store::delete(&state.pool, id, version).await?;
    Ok(StatusCode::NO_CONTENT)
}
