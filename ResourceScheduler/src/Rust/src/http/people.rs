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
    models::people::{PersonCreate, PersonDto, PersonUpdate},
    store::people as store,
};

pub fn router() -> Router<AppState> {
    Router::new()
        .route("/api/people", get(list).post(create))
        .route(
            "/api/people/{id}",
            axum::routing::put(update).delete(delete_one),
        )
}

async fn list(State(state): State<AppState>) -> ServiceResult<Json<Vec<PersonDto>>> {
    Ok(Json(store::list(&state.pool).await?))
}

async fn create(
    State(state): State<AppState>,
    Json(input): Json<PersonCreate>,
) -> ServiceResult<impl IntoResponse> {
    let row = store::create(&state.pool, input).await?;
    Ok((StatusCode::CREATED, Json(row)))
}

async fn update(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
    Json(input): Json<PersonUpdate>,
) -> ServiceResult<Json<PersonDto>> {
    Ok(Json(store::update(&state.pool, id, input).await?))
}

async fn delete_one(
    State(state): State<AppState>,
    Path(id): Path<Uuid>,
) -> ServiceResult<StatusCode> {
    store::delete(&state.pool, id).await?;
    Ok(StatusCode::NO_CONTENT)
}
