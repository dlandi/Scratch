use axum::{
    Json, Router,
    extract::{Path, State},
    http::StatusCode,
    response::IntoResponse,
    routing::{get, post},
};
use uuid::Uuid;

use crate::{
    AppState,
    error::{ServiceError, ServiceResult},
    extractors::IfMatch,
    models::device_groups::{DeviceGroupCreate, DeviceGroupDto, DeviceGroupUpdate},
    store::device_groups as store,
};

pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/device-groups", get(list).post(create))
        .route(
            "/api/device-groups/{id}",
            get(get_one).put(update).delete(delete_one),
        )
        .route("/api/device-groups/{id}/activate", post(activate))
        .route("/api/device-groups/{id}/deactivate", post(deactivate))
}

async fn list(State(state): State<AppState>) -> ServiceResult<Json<Vec<DeviceGroupDto>>> {
    Ok(Json(store::list(&state.pool).await?))
}

async fn get_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
) -> ServiceResult<Json<DeviceGroupDto>> {
    match store::get(&state.pool, id).await? {
        Some(g) => Ok(Json(g)),
        None => Err(ServiceError::NotFound),
    }
}

async fn create(
    State(state): State<AppState>,
    Json(input): Json<DeviceGroupCreate>,
) -> ServiceResult<impl IntoResponse> {
    let row = store::create(&state.pool, input).await?;
    Ok((StatusCode::CREATED, Json(row)))
}

async fn update(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
    Json(input): Json<DeviceGroupUpdate>,
) -> ServiceResult<Json<DeviceGroupDto>> {
    Ok(Json(store::update(&state.pool, id, input, version).await?))
}

async fn activate(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<Json<DeviceGroupDto>> {
    Ok(Json(store::activate(&state.pool, id, version).await?))
}

async fn deactivate(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<Json<DeviceGroupDto>> {
    Ok(Json(store::deactivate(&state.pool, id, version).await?))
}

async fn delete_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<StatusCode> {
    store::delete(&state.pool, id, version).await?;
    Ok(StatusCode::NO_CONTENT)
}
