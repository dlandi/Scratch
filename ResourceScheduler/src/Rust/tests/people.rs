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

#[tokio::test]
async fn create_returns_201_with_personid_and_optional_email() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(json_request(
            "POST",
            "/api/people",
            &json!({"name": "Alice", "email": null}),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    assert!(body.get("personId").is_some());
    assert_eq!(body["name"], "Alice");
    assert!(body["email"].is_null());
}

#[tokio::test]
async fn update_does_not_require_if_match_header() {
    let app = fresh_app().await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/people",
                &json!({"name": "Alice", "email": null}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["personId"].as_str().unwrap();

    let resp = app
        .oneshot(json_request(
            "PUT",
            &format!("/api/people/{id}"),
            &json!({"name": "Alicia", "email": "a@example"}),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::OK);
    let body = body_json(resp).await;
    assert_eq!(body["name"], "Alicia");
    assert_eq!(body["email"], "a@example");
}

#[tokio::test]
async fn update_missing_returns_404() {
    let app = fresh_app().await;
    let id = Uuid::new_v4();
    let resp = app
        .oneshot(json_request(
            "PUT",
            &format!("/api/people/{id}"),
            &json!({"name": "X", "email": null}),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}

#[tokio::test]
async fn delete_returns_204_and_removes_from_list() {
    let app = fresh_app().await;
    let created = body_json(
        app.clone()
            .oneshot(json_request(
                "POST",
                "/api/people",
                &json!({"name": "Alice", "email": null}),
            ))
            .await
            .unwrap(),
    )
    .await;
    let id = created["personId"].as_str().unwrap();

    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/people/{id}"))
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
                .uri("/api/people")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    let arr = body_json(resp).await;
    assert_eq!(arr.as_array().unwrap().len(), 0);
}

#[tokio::test]
async fn delete_missing_returns_404() {
    let app = fresh_app().await;
    let resp = app
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/people/{}", Uuid::new_v4()))
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::NOT_FOUND);
}
