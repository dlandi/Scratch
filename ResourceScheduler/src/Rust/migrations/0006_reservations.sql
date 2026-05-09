-- Reservations are bookings of a Device-Group by a Test-Group across a
-- UTC time window. start_utc and end_utc are stored as ISO 8601 TEXT;
-- queries use SQLite's datetime() function on these columns when
-- canonical ordering matters (sub-second precision varies between
-- chrono outputs).
CREATE TABLE reservations (
    reservation_id  BLOB    PRIMARY KEY NOT NULL,
    device_group_id BLOB    NOT NULL,
    test_group_id   BLOB    NOT NULL,
    start_utc       TEXT    NOT NULL,
    end_utc         TEXT    NOT NULL,
    status          TEXT    NOT NULL,
    notes           TEXT,
    version         INTEGER NOT NULL,
    FOREIGN KEY (device_group_id) REFERENCES device_groups (device_group_id),
    FOREIGN KEY (test_group_id) REFERENCES test_groups (test_group_id)
) STRICT;

CREATE INDEX idx_reservations_device_group ON reservations (device_group_id);
CREATE INDEX idx_reservations_test_group   ON reservations (test_group_id);
