use std::net::SocketAddr;

use anyhow::Context as _;
use resource_scheduler_api::{AppState, build_app};
use tracing_subscriber::{EnvFilter, layer::SubscriberExt as _, util::SubscriberInitExt as _};

#[tokio::main]
async fn main() -> anyhow::Result<()> {
    init_tracing();

    let database_url = std::env::var("DATABASE_URL")
        .unwrap_or_else(|_| "sqlite://./resource-scheduler.db".to_string());
    let bind_addr_raw = std::env::var("BIND_ADDR").unwrap_or_else(|_| "127.0.0.1:7070".to_string());
    let bind_addr: SocketAddr = bind_addr_raw.parse().context("invalid BIND_ADDR")?;

    let state = AppState::connect(&database_url).await?;
    state.run_migrations().await?;

    // Optional one-time seeding of demo fixture data. Off by default in
    // production; flip on with SEED_DEMO_DATA=1 (or any non-empty value)
    // when running the binary as a developer aid. The seeder is a no-op
    // when the database already has rows, so leaving it on across
    // restarts is harmless.
    let seed_enabled = std::env::var("SEED_DEMO_DATA")
        .map(|v| !v.is_empty() && v != "0" && !v.eq_ignore_ascii_case("false"))
        .unwrap_or(false);
    if seed_enabled {
        resource_scheduler_api::seed::seed_if_empty(&state.pool).await?;
    }

    let app = build_app(state);
    let listener = tokio::net::TcpListener::bind(bind_addr).await?;
    tracing::info!(%bind_addr, %database_url, "resource-scheduler-api listening");

    axum::serve(listener, app)
        .with_graceful_shutdown(shutdown_signal())
        .await?;
    Ok(())
}

fn init_tracing() {
    let filter = EnvFilter::try_from_default_env()
        .unwrap_or_else(|_| EnvFilter::new("info,resource_scheduler_api=debug,tower_http=debug"));
    tracing_subscriber::registry()
        .with(filter)
        .with(tracing_subscriber::fmt::layer())
        .init();
}

async fn shutdown_signal() {
    let _ = tokio::signal::ctrl_c().await;
    tracing::info!("shutdown signal received");
}
