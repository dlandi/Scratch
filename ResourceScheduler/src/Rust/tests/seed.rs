use resource_scheduler_api::{AppState, seed};
use sqlx::Row as _;

async fn fresh_state() -> AppState {
    let state = AppState::in_memory().await.expect("in-memory pool");
    state.run_migrations().await.expect("migrations");
    state
}

async fn count(pool: &sqlx::SqlitePool, table: &str) -> i64 {
    let q = format!("SELECT COUNT(*) FROM {table}");
    sqlx::query_scalar(&q).fetch_one(pool).await.unwrap()
}

#[tokio::test]
async fn seed_populates_all_tables_with_expected_counts() {
    let state = fresh_state().await;
    seed::seed_if_empty(&state.pool).await.expect("seed");

    assert_eq!(count(&state.pool, "buildings").await, 2);
    assert_eq!(count(&state.pool, "devices").await, 24);
    assert_eq!(count(&state.pool, "device_groups").await, 6);
    assert_eq!(count(&state.pool, "device_group_connections").await, 15);
    assert_eq!(count(&state.pool, "people").await, 4);
    assert_eq!(count(&state.pool, "test_groups").await, 2);
    assert_eq!(count(&state.pool, "reservations").await, 11);
}

#[tokio::test]
async fn seed_is_idempotent_when_data_already_present() {
    let state = fresh_state().await;
    seed::seed_if_empty(&state.pool).await.expect("first seed");
    let buildings_after_first = count(&state.pool, "buildings").await;

    // Second call should detect existing data and skip; no duplicates.
    seed::seed_if_empty(&state.pool).await.expect("second seed");
    let buildings_after_second = count(&state.pool, "buildings").await;
    assert_eq!(buildings_after_first, buildings_after_second);
}

#[tokio::test]
async fn seed_reflects_assigned_device_group_id_for_active_group_members() {
    let state = fresh_state().await;
    seed::seed_if_empty(&state.pool).await.expect("seed");

    // Devices in the four Active groups should have a non-NULL
    // assigned_device_group_id; devices not in any active group (or
    // only in Inactive drafts) should be NULL.
    let active: i64 = sqlx::query_scalar(
        "SELECT COUNT(*) FROM devices WHERE assigned_device_group_id IS NOT NULL",
    )
    .fetch_one(&state.pool)
    .await
    .unwrap();
    // Four active groups, each with four members in the seed.
    assert_eq!(active, 16);
}

#[tokio::test]
async fn seed_orders_device_group_members_by_ordinal() {
    let state = fresh_state().await;
    seed::seed_if_empty(&state.pool).await.expect("seed");

    // Bench A's first member is PSU-01, mirroring the JSON order. Use
    // the deterministic seed UUIDs to find the row.
    let bench_a = uuid::Uuid::parse_str("cccccccc-0000-0000-0000-000000000001").unwrap();
    let first_member: uuid::Uuid = sqlx::query(
        "SELECT m.device_id FROM device_group_members m \
         WHERE m.device_group_id = ? ORDER BY m.ordinal LIMIT 1",
    )
    .bind(bench_a)
    .fetch_one(&state.pool)
    .await
    .unwrap()
    .get(0);
    let psu01 = uuid::Uuid::parse_str("bbbbbbbb-0000-0000-0000-000000000012").unwrap();
    assert_eq!(first_member, psu01);
}
