-- People are simple participants. PersonDto has no Version field, so
-- People mutations carry no If-Match header (see PHASE2_CONTRACT.md
-- section 3, "People exception"). The spec calls Email "unique if set"
-- but the in-memory reference does not enforce it; no UNIQUE here.
CREATE TABLE people (
    person_id BLOB PRIMARY KEY NOT NULL,
    name      TEXT NOT NULL,
    email     TEXT
) STRICT;
