-- Devices live in exactly one Building (R14). assigned_device_group_id
-- is a denormalized convenience field that mirrors the canonical
-- DeviceGroup.DeviceIds membership; it is updated by Device-Group
-- write operations, not by Device CRUD itself, and stays NULL until
-- the Devices migration's sibling Device-Groups migration lands.
CREATE TABLE devices (
    device_id                 BLOB    PRIMARY KEY NOT NULL,
    name                      TEXT    NOT NULL,
    status                    TEXT    NOT NULL,
    building_id               BLOB    NOT NULL,
    assigned_device_group_id  BLOB,
    version                   INTEGER NOT NULL,
    FOREIGN KEY (building_id) REFERENCES buildings (building_id)
) STRICT;

CREATE INDEX idx_devices_building_id ON devices (building_id);
