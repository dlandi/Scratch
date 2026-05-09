-- Initial schema. Buildings only; further resources will be added in
-- subsequent migrations as each Phase 2 endpoint lands.
--
-- Conventions:
--   * Aggregate identifiers are BLOB columns holding 16-byte UUIDs.
--   * version is the optimistic-concurrency counter; the C# client
--     sends its observed version in the If-Match header on every PUT,
--     DELETE, and POST sub-resource verb. New rows start at 1.

-- Building name is described as "unique, human-readable" in spec 4.7,
-- but the in-memory reference implementation does not enforce
-- uniqueness on create or update, and the contract pins the Rust
-- server to match the in-memory rule surface exactly. No UNIQUE
-- constraint here; revisit if a rule id is added to the spec.
CREATE TABLE buildings (
    building_id BLOB    PRIMARY KEY NOT NULL,
    name        TEXT    NOT NULL,
    address     TEXT    NOT NULL,
    version     INTEGER NOT NULL
) STRICT;
