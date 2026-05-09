use axum::{extract::FromRequestParts, http::request::Parts};

use crate::error::ServiceError;

/// Parses the `If-Match` header as a 32-bit version, mirroring the C#
/// `int Version` field. The C# client always sends this on PUT, DELETE,
/// and POST sub-resource verbs against versioned aggregates; missing or
/// non-integer values are surfaced as `400 Bad Request`.
pub struct IfMatch(pub i32);

impl<S: Send + Sync> FromRequestParts<S> for IfMatch {
    type Rejection = ServiceError;

    async fn from_request_parts(parts: &mut Parts, _state: &S) -> Result<Self, Self::Rejection> {
        let raw = parts
            .headers
            .get(axum::http::header::IF_MATCH)
            .ok_or(ServiceError::MissingIfMatch)?
            .to_str()
            .map_err(|_| ServiceError::MissingIfMatch)?
            .trim()
            .trim_matches('"');
        let value: i32 = raw.parse().map_err(|_| ServiceError::MissingIfMatch)?;
        Ok(IfMatch(value))
    }
}
