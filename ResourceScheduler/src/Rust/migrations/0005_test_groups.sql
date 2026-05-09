-- A Test-Group is a named team of People that reserves Device-Groups.
-- Membership is ordered by `ordinal` to mirror the C# `List<Guid>
-- MemberIds` semantics.
CREATE TABLE test_groups (
    test_group_id BLOB    PRIMARY KEY NOT NULL,
    name          TEXT    NOT NULL,
    version       INTEGER NOT NULL
) STRICT;

CREATE TABLE test_group_members (
    test_group_id BLOB    NOT NULL,
    person_id     BLOB    NOT NULL,
    ordinal       INTEGER NOT NULL,
    PRIMARY KEY (test_group_id, person_id),
    FOREIGN KEY (test_group_id) REFERENCES test_groups (test_group_id),
    FOREIGN KEY (person_id) REFERENCES people (person_id)
) STRICT;

CREATE INDEX idx_tgm_person ON test_group_members (person_id);
