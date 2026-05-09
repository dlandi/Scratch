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
    error::ServiceResult,
    extractors::IfMatch,
    models::test_groups::{TestGroupCreate, TestGroupDto, TestGroupUpdate},
    store::test_groups as store,
};

pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/test-groups", get(list).post(create))
        .route(
            "/api/test-groups/{id}",
            axum::routing::put(update).delete(delete_one),
        )
}

async fn list(State(state): State<AppState>) -> ServiceResult<Json<Vec<TestGroupDto>>> {
    Ok(Json(store::list(&state.pool).await?))
}

async fn create(
    State(state): State<AppState>,
    Json(input): Json<TestGroupCreate>,
) -> ServiceResult<impl IntoResponse> {
    let row = store::create(&state.pool, input).await?;
    Ok((StatusCode::CREATED, Json(row)))
}

async fn update(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    IfMatch(version): IfMatch,
    Json(input): Json<TestGroupUpdate>,
) -> ServiceResult<Json<TestGroupDto>> {
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
