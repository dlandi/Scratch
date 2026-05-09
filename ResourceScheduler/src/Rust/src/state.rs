use std::str::FromStr as _;

use anyhow::Context as _;
use sqlx::sqlite::{SqliteConnectOptions, SqlitePool, SqlitePoolOptions};

#[derive(Clone)]
pub struct AppState {
    pub pool: SqlitePool,
}

impl AppState {
    pub async fn connect(database_url: &str) -> anyhow::Result<Self> {
        let opts = SqliteConnectOptions::from_str(database_url)
            .context("parsing DATABASE_URL")?
            .create_if_missing(true);
        let pool = SqlitePoolOptions::new()
            .max_connections(5)
            .connect_with(opts)
            .await
            .context("opening SQLite pool")?;
        Ok(Self { pool })
    }

    /// Open an in-memory SQLite pool. Used by the router-level tests to
    /// exercise handlers without a file on disk. The pool is pinned to
    /// a single connection: each `:memory:` database is per-connection,
    /// so reusing one connection is what gives a test its own isolated
    /// store. `min_connections(1)` keeps that connection alive for the
    /// pool's lifetime, and shared cache is left off so parallel tests
    /// do not see each other's state.
    pub async fn in_memory() -> anyhow::Result<Self> {
        let opts = SqliteConnectOptions::new().in_memory(true);
        let pool = SqlitePoolOptions::new()
            .max_connections(1)
            .min_connections(1)
            .connect_with(opts)
            .await
            .context("opening in-memory SQLite pool")?;
        Ok(Self { pool })
    }

    pub async fn run_migrations(&self) -> anyhow::Result<()> {
        sqlx::migrate!()
            .run(&self.pool)
            .await
            .context("running migrations")?;
        Ok(())
    }
}
