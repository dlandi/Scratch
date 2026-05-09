use axum::body::Body;
use axum::http::{Request, StatusCode, header};
use http_body_util::BodyExt as _;
use resource_scheduler_api::{AppState, build_app};
use serde_json::{Value, json};
use tower::ServiceExt as _;

async fn fresh_app() -> axum::Router {
    let state = AppState::in_memory().await.expect("in-memory pool");
    state.run_migrations().await.expect("migrations");
    build_app(state)
}

async fn body_bytes(resp: axum::response::Response) -> Vec<u8> {
    resp.into_body()
        .collect()
        .await
        .unwrap()
        .to_bytes()
        .to_vec()
}

async fn body_json(resp: axum::response::Response) -> Value {
    let bytes = body_bytes(resp).await;
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

#[tokio::test]
async fn list_starts_empty() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/api/buildings")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    assert_eq!(body_bytes(resp).await, b"[]");
}

#[tokio::test]
async fn create_returns_201_with_camel_case_body() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/buildings",
            &json!({"name": "Lab North", "address": "123 Lab St"}),
        ))
        .await
        .unwrap();

    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    assert!(body.get("buildingId").is_some(), "buildingId present");
    assert_eq!(body["name"], "Lab North");
    assert_eq!(body["address"], "123 Lab St");
    assert_eq!(body["version"], 1);
}

#[tokio::test]
async fn get_missing_returns_404() {
    let app = fresh_app().await;
    let id = uuid::Uuid::new_v4();
    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri(format!("/api/buildings/{id}"))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}

#[tokio::test]
async fn list_after_create_returns_one_row() {
    let app = fresh_app().await;
    let _ = app
        .clone()
        .oneshot(json_request(
            "POST",
            "/api/buildings",
            &json!({"name": "A", "address": "x"}),
        ))
        .await
        .unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/api/buildings")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let arr = body_json(resp).await;
    assert_eq!(arr.as_array().unwrap().len(), 1);
    assert_eq!(arr[0]["name"], "A");
}

#[tokio::test]
async fn update_with_correct_version_bumps_to_two() {
    let app = fresh_app().await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/buildings",
                &json!({"name": "A", "address": "x"}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["buildingId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/buildings/{id}"))
                .header("If-Match", "1")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({"name": "A2", "address": "x2"})).unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let body = body_json(resp).await;
    assert_eq!(body["name"], "A2");
    assert_eq!(body["address"], "x2");
    assert_eq!(body["version"], 2);
}

#[tokio::test]
async fn update_with_stale_version_returns_409() {
    let app = fresh_app().await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/buildings",
                &json!({"name": "A", "address": "x"}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["buildingId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/buildings/{id}"))
                .header("If-Match", "99")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({"name": "B", "address": "y"})).unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CONFLICT);
}

#[tokio::test]
async fn update_missing_returns_404() {
    let app = fresh_app().await;
    let id = uuid::Uuid::new_v4();
    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/buildings/{id}"))
                .header("If-Match", "1")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({"name": "B", "address": "y"})).unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}

#[tokio::test]
async fn delete_with_correct_version_returns_204_then_get_is_404() {
    let app = fresh_app().await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/buildings",
                &json!({"name": "A", "address": "x"}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["buildingId"].as_str().unwrap();

    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/buildings/{id}"))
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
                .uri(format!("/api/buildings/{id}"))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}

#[tokio::test]
async fn delete_with_stale_version_returns_409() {
    let app = fresh_app().await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/buildings",
                &json!({"name": "A", "address": "x"}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["buildingId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/buildings/{id}"))
                .header("If-Match", "42")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CONFLICT);
}

#[tokio::test]
async fn delete_blocked_by_r15_when_device_references_building() {
    let app = fresh_app().await;
    // Create a building.
    let building = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/buildings",
                &json!({"name": "A", "address": "x"}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = building["buildingId"].as_str().unwrap().to_owned();

    // Create a device that references it.
    let _ = app
        .clone()
        .oneshot(json_request(
            "POST",
            "/api/devices",
            &json!({
                "name": "Probe",
                "status": "Available",
                "buildingId": id.clone(),
            }),
        ))
        .await
        .unwrap();

    // Attempt to delete the building: must fail R15.
    let resp = app
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/buildings/{id}"))
                .header("If-Match", "1")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
    let body = body_json(resp).await;
    assert_eq!(body["ruleId"], "R15");
}

#[tokio::test]
async fn put_missing_if_match_returns_400() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(json_request(
            "PUT",
            &format!("/api/buildings/{}", uuid::Uuid::new_v4()),
            &json!({"name": "B", "address": "y"}),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::BAD_REQUEST);
}
