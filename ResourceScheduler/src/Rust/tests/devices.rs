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

async fn create_building(app: &axum::Router) -> Uuid {
    let resp = app
        .clone()
        .oneshot(json_request(
            "POST",
            "/api/buildings",
            &json!({"name": "Lab North", "address": "123 Lab St"}),
        ))
        .await
        .unwrap();
    let body = body_json(resp).await;
    Uuid::parse_str(body["buildingId"].as_str().unwrap()).unwrap()
}

#[tokio::test]
async fn create_with_unknown_building_returns_400_r14() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/devices",
            &json!({
                "name": "Probe-1",
                "status": "Available",
                "buildingId": Uuid::new_v4().to_string(),
            }),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R14");
    assert!(
        body["message"]
            .as_str()
            .unwrap()
            .contains("existing Building")
    );
}

#[tokio::test]
async fn create_returns_201_with_status_as_string() {
    let app = fresh_app().await;
    let building_id = create_building(&app).await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/devices",
            &json!({
                "name": "Probe-1",
                "status": "Maintenance",
                "buildingId": building_id.to_string(),
            }),
        ))
        .await
        .unwrap();

    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    assert_eq!(body["name"], "Probe-1");
    assert_eq!(body["status"], "Maintenance"); // string, not integer
    assert_eq!(body["buildingId"], building_id.to_string());
    assert!(body["assignedDeviceGroupId"].is_null());
    assert_eq!(body["version"], 1);
}

#[tokio::test]
async fn update_bumps_version_and_round_trips_status() {
    let app = fresh_app().await;
    let building_id = create_building(&app).await;

    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/devices",
                &json!({
                    "name": "Probe-1",
                    "status": "Available",
                    "buildingId": building_id.to_string(),
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["deviceId"].as_str().unwrap().to_owned();

    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/devices/{id}"))
                .header("If-Match", "1")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({
                        "name": "Probe-1",
                        "status": "Offline",
                        "buildingId": building_id.to_string(),
                    }))
                    .unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let body = body_json(resp).await;
    assert_eq!(body["status"], "Offline");
    assert_eq!(body["version"], 2);
}

#[tokio::test]
async fn update_with_stale_version_returns_409() {
    let app = fresh_app().await;
    let building_id = create_building(&app).await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/devices",
                &json!({
                    "name": "Probe-1",
                    "status": "Available",
                    "buildingId": building_id.to_string(),
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["deviceId"].as_str().unwrap().to_owned();

    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/devices/{id}"))
                .header("If-Match", "99")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({
                        "name": "Probe-1",
                        "status": "Available",
                        "buildingId": building_id.to_string(),
                    }))
                    .unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CONFLICT);
}

#[tokio::test]
async fn update_with_unknown_building_returns_400_r14() {
    let app = fresh_app().await;
    let building_id = create_building(&app).await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/devices",
                &json!({
                    "name": "Probe-1",
                    "status": "Available",
                    "buildingId": building_id.to_string(),
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["deviceId"].as_str().unwrap().to_owned();

    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/devices/{id}"))
                .header("If-Match", "1")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({
                        "name": "Probe-1",
                        "status": "Available",
                        "buildingId": Uuid::new_v4().to_string(),
                    }))
                    .unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R14");
}

#[tokio::test]
async fn delete_then_get_404() {
    let app = fresh_app().await;
    let building_id = create_building(&app).await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/devices",
                &json!({
                    "name": "Probe-1",
                    "status": "Available",
                    "buildingId": building_id.to_string(),
                }),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["deviceId"].as_str().unwrap().to_owned();

    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/devices/{id}"))
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
                .uri(format!("/api/devices/{id}"))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}
