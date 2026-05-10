use axum::{
    Json,
    http::StatusCode,
    response::{IntoResponse, Response},
};
use serde::Serialize;

/// Wire shape of a domain rule violation. The C# client deserializes the
/// same camelCase fields and constructs a typed `ValidationException`.
/// See `Docs/PHASE2_CONTRACT.md` and `RemoteClientService.ApiError`.
#[derive(Debug, Serialize)]
#[serde(rename_all = "camelCase")]
struct ApiErrorBody {
    rule_id: String,
    message: String,
}

/// Errors returned from store / handler functions. Each variant maps to a
/// specific HTTP status: domain rule violations carry the spec rule id so
/// the client can route them to a typed exception.
#[derive(Debug, thiserror::Error)]
pub enum ServiceError {
    #[error("not found")]
    NotFound,

    #[error("version conflict")]
    Conflict,

    #[error("rule {rule_id}: {message}")]
    Validation { rule_id: String, message: String },

    #[error("missing or malformed If-Match header")]
    MissingIfMatch,

    /// Malformed request input that is not a domain rule violation, e.g.
    /// an unparseable query parameter. Surfaces as `400 Bad Request` with
    /// the same `{ruleId, message}` body shape so the C# client can route
    /// it through `ValidationException`; the synthetic id `BadRequest`
    /// distinguishes it from real spec rule ids.
    #[error("bad request: {0}")]
    BadRequest(String),

    #[error(transparent)]
    Internal(#[from] anyhow::Error),
}

impl ServiceError {
    pub fn validation(rule_id: impl Into<String>, message: impl Into<String>) -> Self {
        Self::Validation {
            rule_id: rule_id.into(),
            message: message.into(),
        }
    }
}

impl From<sqlx::Error> for ServiceError {
    fn from(e: sqlx::Error) -> Self {
        // Row-not-found is reachable from store helpers that expect a
        // single row; map it to NotFound so the handler returns 404.
        match e {
            sqlx::Error::RowNotFound => ServiceError::NotFound,
            other => ServiceError::Internal(anyhow::Error::new(other)),
        }
    }
}

impl IntoResponse for ServiceError {
    fn into_response(self) -> Response {
        match self {
            ServiceError::NotFound => StatusCode::NOT_FOUND.into_response(),
            ServiceError::Conflict => StatusCode::CONFLICT.into_response(),
            ServiceError::Validation { rule_id, message } => (
                StatusCode::BAD_REQUEST,
                Json(ApiErrorBody { rule_id, message }),
            )
                .into_response(),
            ServiceError::MissingIfMatch => StatusCode::BAD_REQUEST.into_response(),
            ServiceError::BadRequest(message) => (
                StatusCode::BAD_REQUEST,
                Json(ApiErrorBody {
                    rule_id: "BadRequest".to_string(),
                    message,
                }),
            )
                .into_response(),
            ServiceError::Internal(err) => {
                tracing::error!(?err, "internal error");
                StatusCode::INTERNAL_SERVER_ERROR.into_response()
            }
        }
    }
}

pub type ServiceResult<T> = Result<T, ServiceError>;
