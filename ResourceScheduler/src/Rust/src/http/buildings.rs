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
    models::buildings::{BuildingCreate, BuildingDto, BuildingUpdate},
    store::buildings as store,
};

pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/buildings", get(list).post(create))
        .route(
            "/api/buildings/{id}",
            get(get_one).put(update).delete(delete_one),
        )
}

async fn list(State(state): State<AppState>) -> ServiceResult<Json<Vec<BuildingDto>>> {
    let rows = store::list(&state.pool).await?;
    Ok(Json(rows))
}

async fn get_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
) -> ServiceResult<Json<BuildingDto>> {
    match store::get(&state.pool, id).await? {
        Some(b) => Ok(Json(b)),
        None => Err(ServiceError::NotFound),
    }
}

async fn create(
    State(state): State<AppState>,
    Json(input): Json<BuildingCreate>,
) -> ServiceResult<impl IntoResponse> {
    let row = store::create(&state.pool, input).await?;
    Ok((StatusCode::CREATED, Json(row)))
}

async fn update(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
    Json(input): Json<BuildingUpdate>,
) -> ServiceResult<Json<BuildingDto>> {
    let row = store::update(&state.pool, id, input, version).await?;
    Ok(Json(row))
}

async fn delete_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
) -> ServiceResult<StatusCode> {
    store::delete(&state.pool, id, version).await?;
    Ok(StatusCode::NO_CONTENT)
}
