-- A Device-Group is the schedulable unit. Member ordering is preserved
-- by the `ordinal` column on the members table, mirroring the C#
-- `List<Guid> DeviceIds` semantics. Layout is normalized [0..1]
-- coordinates per device on the Designer canvas. Connections are
-- physical links between two member devices and may carry a label.
--
-- Foreign key constraints are declared but SQLite's PRAGMA
-- foreign_keys is left at its default (off): cascading deletes are
-- performed in the store layer so domain-rule mapping stays explicit.

CREATE TABLE device_groups (
    device_group_id BLOB    PRIMARY KEY NOT NULL,
    name            TEXT    NOT NULL,
    status          TEXT    NOT NULL,
    version         INTEGER NOT NULL
) STRICT;

CREATE TABLE device_group_members (
    device_group_id BLOB    NOT NULL,
    device_id       BLOB    NOT NULL,
    ordinal         INTEGER NOT NULL,
    PRIMARY KEY (device_group_id, device_id),
    FOREIGN KEY (device_group_id) REFERENCES device_groups (device_group_id),
    FOREIGN KEY (device_id) REFERENCES devices (device_id)
) STRICT;

CREATE INDEX idx_dgm_device_id ON device_group_members (device_id);

CREATE TABLE device_group_connections (
    connection_id   BLOB    PRIMARY KEY NOT NULL,
    device_group_id BLOB    NOT NULL,
    from_device_id  BLOB    NOT NULL,
    to_device_id    BLOB    NOT NULL,
    label           TEXT    NOT NULL,
    FOREIGN KEY (device_group_id) REFERENCES device_groups (device_group_id)
) STRICT;

CREATE INDEX idx_dgc_group ON device_group_connections (device_group_id);

CREATE TABLE device_group_layout (
    device_group_id BLOB NOT NULL,
    device_id       BLOB NOT NULL,
    x               REAL NOT NULL,
    y               REAL NOT NULL,
    PRIMARY KEY (device_group_id, device_id),
    FOREIGN KEY (device_group_id) REFERENCES device_groups (device_group_id)
) STRICT;
