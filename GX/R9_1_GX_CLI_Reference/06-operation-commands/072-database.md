---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.72. database'
source_lines: 8431-8528
---

## 6.72. database

#### Command Description

The `show database` command is used to show the list of the databases in the system. The `clear database` command is used to perform a full wipe of database contents. **clear database** The `clear database` command sets the NE database to default and reboots the system. This operation will be potentially traffic affecting, and may cause connectivity interruption to the system. Three types of clear actions are supported. For details, go to the clear-type description (p. 371).

**Note:** The system may report some transient alarms following the controller card reboot as part of the clear database command execution. It is recommended to ignore such transient alarms following the clear database command execution.

**Note:** This command does not wipe logs, PM data and other non-configuration data. See the 'clear system' command when needing a secure wipe or factory default.

#### Command Syntax

```
clear [-f] database [[clear-type=]<value>] [script=]<value> [new-admin-user=]<value> [new-admin-password=]<value>
show database-<database-type> [database-state] [database-version] [database-vendor] [database-product] [ne-name] [node-controller-serial-number]
[loopback-ipv4] [loopback-ipv6] [backup-time] [description]
```

#### Command Usage Details

**Table 222: database Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

<!-- page 370 -->

#### Command Parameters

**Table 223: database Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 224: database Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| database-type | The database type of database identifier. | active manual oneday onehour oneweek rollback temp | n/a | show |
| database-state | Indicates the state of the database. | active inactive | n/a | show |
| database-version | Indicates the database version. | String (length 0..20) | n/a | show |
| database-vendor | Vendor information of the database. | String (length 0..32) | n/a | show |
| database-product | Indicates the network element family this database belongs to. | String (length 0..32) | n/a | show |
| ne-name | User assigned name for this NE as present in this database. | String (length 0..256) | n/a | show |
| node-controller-serial-number | Serial number of the node controller. | String (length 0..32) | n/a | show |
| loopback-ipv4 | loopback ipv4 address. | Ipv4 address | n/a | show |
| loopback-ipv6 | loopback ipv6 address. | Ipv6 address | n/a | show |
| backup-time | Indicates the database snapshot backup time. | String | n/a | show |
| description | Database description. | String (length 0..128) | n/a | show |
| clear-type | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory defaults.<br>• keep-networking: Full wipe of DB contents is to be performed, but network configurations are to be kept. In this case, new-admin-user and new-admin-password must be provided for the system to auto-create the new admin user after clearing the database.<br>• initialize-from-script: Full wipe of DB contents is to be performed, but the database is to be initialized from the pre-defined script. The script must be pre-stored in the system. Additionally, new-admin-user and new-admin-password must be provided for the system to auto-create the new admin user. i Note: The keep-networking and initialize-from-script options require the new-admin-user and new-admin-password parameters to be provided and they can only be triggered by the SA user. | • full<br>• keep-networking<br>• initialize-from-script | full | clear |
| script | The script to execute after clearing the database. The script parameter may be an absolute path for a .cli file, or just the filename if the script is present in the default script directory (/storage/scripts). The script must always include the .cli extension and reference a CLI script (.cli). This script needs to match the criteria already covered by the run script command. The file needs to exist, and needs to be readable by users. i Note: This parameter is mandatory for users to clear database with clear-type set to initialize-from-script. | string Examples:<br>• /tmp/my script.cli _<br>• my script.cli _ | n/a | activate, clear, download, prepare-upgrade |
| new-admin-user | The user-name that is auto-configured after the database is wiped. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | String (0..64 characters) | n/a | activate, clear, download, prepare-upgrade |
| new-admin-password | The password for the new-admin-user that is auto-configured after the database is wiped. The password can be provided as a password hash ( format: $&lt;id&gt;$&lt;salt&gt;$&lt;hash&gt;; only id 6 (SHA512) is supported; salt size is between 2 and 16 chars), or as plain text. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | string pattern: "$6$[A-Za-z0-9./]{2,16}$[A-Za-z0-9./]+" | n/a | activate, clear, download, prepare-upgrade |

#### Examples

The following example shows how to view the list of databases:

```
show database
```

The following example shows how to view one database:

```
show database-active
```

This example shows how to do a full wipe of database:

```
clear database
```

This example shows how to do a full wipe of database without confirmation:

<!-- page 373 -->

```
clear -f database
```

This example shows how to clear the database and retain the IP configurations:

```
clear database clear-type=keep-networking new-admin-user=mynewuser new-admin-password=MyPass!
```

This example shows how to clear the database and initialize the database from the pre-defined script:

```
clear database clear-type=initialize-from-script script=script.cli new-admin-user=mynewuser new-admin-password=MyPass!
```

<!-- page 374 -->
