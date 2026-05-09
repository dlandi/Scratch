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

async fn post_json(app: &axum::Router, uri: &str, body: &Value) -> axum::response::Response {
    app.clone()
        .oneshot(json_request("POST", uri, body))
        .await
        .unwrap()
}

async fn create_building(app: &axum::Router) -> Uuid {
    let resp = post_json(
        app,
        "/api/buildings",
        &json!({"name": "Lab North", "address": "x"}),
    )
    .await;
    let body = body_json(resp).await;
    Uuid::parse_str(body["buildingId"].as_str().unwrap()).unwrap()
}

async fn create_device(app: &axum::Router, building_id: Uuid, name: &str, status: &str) -> Uuid {
    let resp = post_json(
        app,
        "/api/devices",
        &json!({"name": name, "status": status, "buildingId": building_id.to_string()}),
    )
    .await;
    let body = body_json(resp).await;
    Uuid::parse_str(body["deviceId"].as_str().unwrap()).unwrap()
}

#[tokio::test]
async fn create_empty_group_returns_201_inactive() {
    let app = fresh_app().await;
    let resp = post_json(
        &app,
        "/api/device-groups",
        &json!({"name": "G", "deviceIds": [], "connections": [], "layout": []}),
    )
    .await;
    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    assert_eq!(body["status"], "Inactive");
    assert_eq!(body["version"], 1);
    assert_eq!(body["deviceIds"].as_array().unwrap().len(), 0);
}

#[tokio::test]
async fn create_with_connection_referencing_non_member_returns_400_r6() {
    let app = fresh_app().await;
    let bldg = create_building(&app).await;
    let d1 = create_device(&app, bldg, "D1", "Available").await;
    let stranger = Uuid::new_v4();

    let resp = post_json(
        &app,
        "/api/device-groups",
        &json!({
            "name": "G",
            "deviceIds": [d1.to_string()],
            "connections": [{
                "connectionId": Uuid::nil().to_string(),
                "fromDeviceId": d1.to_string(),
                "toDeviceId": stranger.to_string(),
                "label": ""
            }],
            "layout": [],
        }),
    )
    .await;
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R6");
}

#[tokio::test]
async fn create_round_trips_members_and_layout() {
    let app = fresh_app().await;
    let bldg = create_building(&app).await;
    let d1 = create_device(&app, bldg, "D1", "Available").await;
    let d2 = create_device(&app, bldg, "D2", "Available").await;

    let resp = post_json(
        &app,
        "/api/device-groups",
        &json!({
            "name": "G",
            "deviceIds": [d1.to_string(), d2.to_string()],
            "connections": [],
            "layout": [
                {"deviceId": d1.to_string(), "x": 0.25, "y": 0.5},
                {"deviceId": d2.to_string(), "x": 0.75, "y": 0.5},
            ],
        }),
    )
    .await;
    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    let ids = body["deviceIds"].as_array().unwrap();
    assert_eq!(ids[0].as_str().unwrap(), d1.to_string());
    assert_eq!(ids[1].as_str().unwrap(), d2.to_string());
    let layout = body["layout"].as_array().unwrap();
    assert_eq!(layout.len(), 2);

    // Round-trip via GET to make sure persistence preserved order.
    let group_id = body["deviceGroupId"].as_str().unwrap();
    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri(format!("/api/device-groups/{group_id}"))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    let fetched = body_json(resp).await;
    let ids = fetched["deviceIds"].as_array().unwrap();
    assert_eq!(ids[0].as_str().unwrap(), d1.to_string());
    assert_eq!(ids[1].as_str().unwrap(), d2.to_string());
}

#[tokio::test]
async fn activate_empty_returns_400_r7() {
    let app = fresh_app().await;
    let group = body_json(
        post_json(
            &app,
            "/api/device-groups",
            &json!({"name": "G", "deviceIds": [], "connections": [], "layout": []}),
        )
        .await,
    )
    .await;
    let id = group["deviceGroupId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{id}/activate"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R7");
}

#[tokio::test]
async fn activate_with_offline_member_returns_400_r5() {
    let app = fresh_app().await;
    let bldg = create_building(&app).await;
    let d_off = create_device(&app, bldg, "Down", "Offline").await;

    let group = body_json(
        post_json(
            &app,
            "/api/device-groups",
            &json!({
                "name": "G",
                "deviceIds": [d_off.to_string()],
                "connections": [],
                "layout": [],
            }),
        )
        .await,
    )
    .await;
    let id = group["deviceGroupId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{id}/activate"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R5");
}

#[tokio::test]
async fn activate_then_deactivate_round_trip() {
    let app = fresh_app().await;
    let bldg = create_building(&app).await;
    let d1 = create_device(&app, bldg, "D1", "Available").await;

    let group = body_json(
        post_json(
            &app,
            "/api/device-groups",
            &json!({
                "name": "G",
                "deviceIds": [d1.to_string()],
                "connections": [],
                "layout": [],
            }),
        )
        .await,
    )
    .await;
    let id = group["deviceGroupId"].as_str().unwrap();

    // Activate
    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{id}/activate"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let body = body_json(resp).await;
    assert_eq!(body["status"], "Active");
    assert_eq!(body["version"], 2);

    // Member's assignedDeviceGroupId should now point at the group.
    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("GET")
                .uri(format!("/api/devices/{d1}"))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    let dev = body_json(resp).await;
    assert_eq!(dev["assignedDeviceGroupId"].as_str().unwrap(), id);

    // Deactivate
    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{id}/deactivate"))
                .header("If-Match", "2")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let body = body_json(resp).await;
    assert_eq!(body["status"], "Inactive");
    assert_eq!(body["version"], 3);
}

#[tokio::test]
async fn second_activation_with_overlapping_member_returns_400_r3() {
    let app = fresh_app().await;
    let bldg = create_building(&app).await;
    let d1 = create_device(&app, bldg, "D1", "Available").await;

    // Group A: includes D1 and is activated.
    let g_a = body_json(
        post_json(
            &app,
            "/api/device-groups",
            &json!({
                "name": "A",
                "deviceIds": [d1.to_string()],
                "connections": [],
                "layout": [],
            }),
        )
        .await,
    )
    .await;
    let a_id = g_a["deviceGroupId"].as_str().unwrap();
    let _ = app
        .clone()
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{a_id}/activate"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();

    // Group B: also includes D1, attempt to activate.
    let g_b = body_json(
        post_json(
            &app,
            "/api/device-groups",
            &json!({
                "name": "B",
                "deviceIds": [d1.to_string()],
                "connections": [],
                "layout": [],
            }),
        )
        .await,
    )
    .await;
    let b_id = g_b["deviceGroupId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("POST")
                .uri(format!("/api/device-groups/{b_id}/activate"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R3");
}

#[tokio::test]
async fn delete_then_get_returns_404() {
    let app = fresh_app().await;
    let group = body_json(
        post_json(
            &app,
            "/api/device-groups",
            &json!({"name": "G", "deviceIds": [], "connections": [], "layout": []}),
        )
        .await,
    )
    .await;
    let id = group["deviceGroupId"].as_str().unwrap();

    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/device-groups/{id}"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NO_CONTENT);

    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri(format!("/api/device-groups/{id}"))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}
