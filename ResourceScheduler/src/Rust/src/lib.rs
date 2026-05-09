use axum::{Router, http::StatusCode, routing::get};
use tower_http::{
    cors::{Any, CorsLayer},
    trace::TraceLayer,
};

pub use state::AppState;

pub mod error;
pub mod extractors;
pub mod http;
pub mod models;
pub mod state;
pub mod store;

pub fn build_app(state: AppState) -> Router {
    Router::new()
        .route("/healthz", get(healthz))
        .merge(http::buildings::router())
        .merge(http::devices::router())
        .merge(http::device_groups::router())
        .merge(http::people::router())
        .merge(http::test_groups::router())
        .merge(http::reservations::router())
        .with_state(state)
        .layer(TraceLayer::new_for_http())
        .layer(cors_layer())
}

async fn healthz() -> StatusCode {
    StatusCode::OK
}

fn cors_layer() -> CorsLayer {
    // Permissive while the API is anonymous. Tighten to the Blazor host
    // origin once auth lands and we know the deployed origins.
    CorsLayer::new()
        .allow_origin(Any)
        .allow_methods(Any)
        .allow_headers(Any)
}
