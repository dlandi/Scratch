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

async fn create_person(app: &axum::Router) -> Uuid {
    let resp = app
        .clone()
        .oneshot(json_request(
            "POST",
            "/api/people",
            &json!({"name": "P", "email": null}),
        ))
        .await
        .unwrap();
    let body = body_json(resp).await;
    Uuid::parse_str(body["personId"].as_str().unwrap()).unwrap()
}

#[tokio::test]
async fn create_round_trips_members_in_order() {
    let app = fresh_app().await;
    let p1 = create_person(&app).await;
    let p2 = create_person(&app).await;

    let resp = app
        .clone()
        .oneshot(json_request(
            "POST",
            "/api/test-groups",
            &json!({"name": "Team", "memberIds": [p1.to_string(), p2.to_string()]}),
        ))
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CREATED);
    let body = body_json(resp).await;
    let ids = body["memberIds"].as_array().unwrap();
    assert_eq!(ids[0].as_str().unwrap(), p1.to_string());
    assert_eq!(ids[1].as_str().unwrap(), p2.to_string());
    assert_eq!(body["version"], 1);

    // Round-trip via list to make sure order persists.
    let resp = app
        .oneshot(
            Request::builder()
                .method("GET")
                .uri("/api/test-groups")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    let arr = body_json(resp).await;
    let entry = &arr.as_array().unwrap()[0];
    let ids = entry["memberIds"].as_array().unwrap();
    assert_eq!(ids[0].as_str().unwrap(), p1.to_string());
    assert_eq!(ids[1].as_str().unwrap(), p2.to_string());
}

#[tokio::test]
async fn update_with_stale_version_returns_409() {
    let app = fresh_app().await;
    let created = body_json(
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
    let id = created["testGroupId"].as_str().unwrap();

    let resp = app
        .oneshot(
            Request::builder()
                .method("PUT")
                .uri(format!("/api/test-groups/{id}"))
                .header("If-Match", "99")
                .header(header::CONTENT_TYPE, "application/json")
                .body(Body::from(
                    serde_json::to_vec(&json!({"name": "Team2", "memberIds": []})).unwrap(),
                ))
                .unwrap(),
        )
        .await
        .unwrap();
    assert_eq!(resp.status(), StatusCode::CONFLICT);
}

#[tokio::test]
async fn delete_round_trip_returns_204_and_empties_list() {
    let app = fresh_app().await;
    let created = body_json(
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
    let id = created["testGroupId"].as_str().unwrap();

    let resp = app
        .clone()
        .oneshot(
            Request::builder()
                .method("DELETE")
                .uri(format!("/api/test-groups/{id}"))
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
                .uri("/api/test-groups")
                .body(Body::empty())
                .unwrap(),
        )
        .await
        .unwrap();
    let arr = body_json(resp).await;
    assert_eq!(arr.as_array().unwrap().len(), 0);
}
