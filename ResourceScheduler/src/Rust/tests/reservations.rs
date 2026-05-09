use axum::body::Body;
use axum::http::{Request, StatusCode, header};
use http_body_util::BodyExt as _;
use resource_scheduler_api::{AppState, build_app};
use serde_json::{Value, json};
use tower::ServiceExt as _;
use uuid::Uuid;

async fn fresh_app() -> axum::Router {
    let state = AppState::in_memory().await.expect("in-memory pool");
    state.run_migrations().await.expect("migrations");
    build_app(state)
}

async fn body_json(resp: axum::response::Response) -> Value {
    let bytes = resp
        .into_body()
        .collect()
        .await
        .unwrap()
        .to_bytes()
        .to_vec();
    serde_json::from_slice(&bytes).expect("response body is JSON")
}

fn json_request(method: &str, uri: &str, body: &Value) -> Request<Body> {
    Request::builder()
        .method(method)
        .uri(uri)
        .header(header::CONTENT_TYPE, "application/json")
        .body(Body::from(serde_json::to_vec(body).unwrap()))
        .unwrap()
}

/// Builds a minimal valid environment: building, device (Available),
/// activated device-group containing it, and a test-group. Returns the
/// (active device-group id, test-group id) pair so the test can post
/// reservations against a real configuration.
async fn build_world(app: &axum::Router) -> (Uuid, Uuid) {
    let bldg = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/buildings",
                &json!({"name": "B", "address": "x"}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let bldg_id = bldg["buildingId"].as_str().unwrap().to_owned();

    let dev = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/devices",
                &json!({"name": "D", "status": "Available", "buildingId": bldg_id}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let dev_id = dev["deviceId"].as_str().unwrap();

    let group = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/device-groups",
                &json!({
                    "name": "G",
                    "deviceIds": [dev_id],
                    "connections": [],
                    "layout": [],
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let group_id = group["deviceGroupId"].as_str().unwrap().to_owned();
    let activate_resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{group_id}/activate"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(activate_resp.status(), StatusCode::OK);

    let test_group = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/test-groups",
                &json!({"name": "Team", "memberIds": []}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let test_group_id = test_group["testGroupId"].as_str().unwrap().to_owned();

    (
        Uuid::parse_str(&group_id).unwrap(),
        Uuid::parse_str(&test_group_id).unwrap(),
    )
}

#[tokio::test]
async fn create_with_end_at_or_before_start_returns_400_r13() {
    let app = fresh_app().await;
    let (gid, tid) = build_world(&app).await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/reservations",
            &json!({
                "deviceGroupId": gid.to_string(),
                "testGroupId": tid.to_string(),
                "startUtc": "2026-05-09T12:00:00Z",
                "endUtc":   "2026-05-09T12:00:00Z",
                "notes": null,
            }),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R13");
}

#[tokio::test]
async fn create_with_unknown_device_group_returns_400_r8() {
    let app = fresh_app().await;
    let (_gid, tid) = build_world(&app).await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/reservations",
            &json!({
                "deviceGroupId": Uuid::new_v4().to_string(),
                "testGroupId": tid.to_string(),
                "startUtc": "2026-05-09T12:00:00Z",
                "endUtc":   "2026-05-09T13:00:00Z",
                "notes": null,
            }),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R8");
}

#[tokio::test]
async fn create_succeeds_with_pending_status() {
    let app = fresh_app().await;
    let (gid, tid) = build_world(&app).await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/reservations",
            &json!({
                "deviceGroupId": gid.to_string(),
                "testGroupId": tid.to_string(),
                "startUtc": "2026-05-09T12:00:00Z",
                "endUtc":   "2026-05-09T13:00:00Z",
                "notes": "first",
            }),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    assert_eq!(body["status"], "Pending");
    assert_eq!(body["version"], 1);
    assert_eq!(body["notes"], "first");
}

#[tokio::test]
async fn confirm_reservation_when_group_inactive_returns_400_r9() {
    let app = fresh_app().await;
    let (gid, tid) = build_world(&app).await;

    // Create the reservation while the group is Active...
    let r = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": tid.to_string(),
                    "startUtc": "2026-05-09T12:00:00Z",
                    "endUtc":   "2026-05-09T13:00:00Z",
                    "notes": null,
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let rid = r["reservationId"].as_str().unwrap();

    // ...then deactivate the group.
    let _ = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{gid}/deactivate"))
                .header("If-Match", "2")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    // Confirm should fail with R9.
    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/reservations/{rid}/confirm"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R9");
}

#[tokio::test]
async fn confirm_with_overlapping_confirmed_returns_400_r10() {
    let app = fresh_app().await;
    let (gid, tid) = build_world(&app).await;

    // First reservation: confirmed, 12:00-13:00.
    let r1 = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": tid.to_string(),
                    "startUtc": "2026-05-09T12:00:00Z",
                    "endUtc":   "2026-05-09T13:00:00Z",
                    "notes": null,
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let r1_id = r1["reservationId"].as_str().unwrap().to_owned();
    let _ = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/reservations/{r1_id}/confirm"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    // Second reservation: 12:30-13:30 on the SAME group + a fresh test group.
    let other_team = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/test-groups",
                &json!({"name": "Other", "memberIds": []}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let other_tid = other_team["testGroupId"].as_str().unwrap();
    let r2 = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": other_tid,
                    "startUtc": "2026-05-09T12:30:00Z",
                    "endUtc":   "2026-05-09T13:30:00Z",
                    "notes": null,
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let r2_id = r2["reservationId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/reservations/{r2_id}/confirm"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R10");
}

#[tokio::test]
async fn cancel_round_trip_changes_status_and_bumps_version() {
    let app = fresh_app().await;
    let (gid, tid) = build_world(&app).await;
    let r = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": tid.to_string(),
                    "startUtc": "2026-05-09T12:00:00Z",
                    "endUtc":   "2026-05-09T13:00:00Z",
                    "notes": null,
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let rid = r["reservationId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/reservations/{rid}/cancel"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let body = body_json(resp).await;
    assert_eq!(body["status"], "Cancelled");
    assert_eq!(body["version"], 2);
}

#[tokio::test]
async fn list_filter_status_in_returns_only_matching_rows() {
    let app = fresh_app().await;
    let (gid, tid) = build_world(&app).await;

    // Create three reservations: one Pending, one Confirmed, one Cancelled.
    let r_pending = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": tid.to_string(),
                    "startUtc": "2026-05-09T08:00:00Z",
                    "endUtc":   "2026-05-09T09:00:00Z",
                    "notes": "p",
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let _r_pending_id = r_pending["reservationId"].as_str().unwrap();

    let r_conf = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": tid.to_string(),
                    "startUtc": "2026-05-09T10:00:00Z",
                    "endUtc":   "2026-05-09T11:00:00Z",
                    "notes": "c",
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let r_conf_id = r_conf["reservationId"].as_str().unwrap();
    let _ = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/reservations/{r_conf_id}/confirm"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    let r_cxl = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/reservations",
                &json!({
                    "deviceGroupId": gid.to_string(),
                    "testGroupId": tid.to_string(),
                    "startUtc": "2026-05-09T15:00:00Z",
                    "endUtc":   "2026-05-09T16:00:00Z",
                    "notes": "x",
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let r_cxl_id = r_cxl["reservationId"].as_str().unwrap();
    let _ = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/reservations/{r_cxl_id}/cancel"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    // statusIn=Pending&statusIn=Confirmed should return 2 rows, in
    // ascending startUtc order.
    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/api/reservations?statusIn=Pending&statusIn=Confirmed")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let arr = body_json(resp).await;
    let rows = arr.as_array().unwrap();
    assert_eq!(rows.len(), 2);
    assert_eq!(rows[0]["status"], "Pending");
    assert_eq!(rows[1]["status"], "Confirmed");
}
