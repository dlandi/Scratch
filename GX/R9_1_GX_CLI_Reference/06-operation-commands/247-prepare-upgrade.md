---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.247. prepare-upgrade'
source_lines: 19142-19231
---

## 6.247. prepare-upgrade

#### Command Description

This command is used to prepare the network element software for upgrade. The prepare-upgrade options are:

- validate, which validates the software label.
- apply, which applies the software label.

#### Command Syntax

```
prepare-upgrade [-h]
prepare-upgrade [-f] [-i] [-u] [option=]<value> [manifest=]<value> [[db-action=]<value>] [[clear-type=]<value>] [script=]<value>
[new-admin-user=]<value> [new-admin-password=]<value>
```

#### Command Usage Details

**Table 586: prepare-upgrade Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 587: prepare-upgrade Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -i | Ignore validation failures. |
| -u | Unattended, auto-activate software after prepare-upgrade apply. |
| -f | Force command without confirmation. |

**Table 588: prepare-upgrade Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| option | Predefined options available for prepare-upgrade:<br>• validate - validates the software manifest.<br>• apply - applies the software manifest. | validate apply | n/a |
| manifest | Manifest to be prepared for upgrade. | leafref (path "/ioa-ne:ne/ioa-ne:system/ioa-ne:sw-management/ioa-ne:downloads/ioa-ne:manifest/ ioa-ne:manifest-file") | n/a |
| db-action | Specify the expected database operation during activating software image. It is valid for unattended upgrade only.<br>• empty-db: Activate the software image with empty database.<br>• upgrade-db: Activate the software image with upgrading the current database.<br>• rollback: Rollback to the previous active software image. | • empty-db<br>• upgrade-db<br>• rollback | upgrade-db |
| 5 clear-type | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory defaults.<br>• keep-networking: Full wipe of DB contents is to be performed, but network configurations are to be kept. In this case, new-admin-user and new-admin-password must be provided for the system to auto-create the new admin user after clearing the database. | • full<br>• keep-networking<br>• initialize-from-script | full |

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 5 Only valid for an unattended operation with db-action set to empty-db.
| script | The script to execute after clearing the database. The script parameter may be an absolute path for a .cli file, or just the filename if the script is present in the default script directory (/storage/scripts). The script must always include the .cli extension and reference a CLI script (.cli). This script needs to match the criteria already covered by the run script command. The file needs to exist, and needs to be readable by users. i Note: This parameter is mandatory for users to clear database with clear-type set to initialize-from-script. | string Examples:<br>• /tmp/my script.cli _<br>• my script.cli _ | n/a |
| new-admin-user | The user-name that is auto-configured after the database is wiped. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | String (0..64 characters) | n/a |
| new-admin-password | The password for the new-admin-user that is auto-configured after the database is wiped. The password can be provided as a password hash ( format: $&lt;id&gt;$&lt;salt&gt;$&lt;hash&gt;; only id 6 (SHA512) is supported; salt size is between 2 and 16 chars), or as plain text. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | string pattern: "$6$[A-Za-z0-9./]{2,16}$[A-Za-z0-9./]+" | n/a |

#### Examples

This example shows how to validate the downloaded \<label\>:

```
prepare-upgrade validate <label>
```

This example shows how to apply the downloaded \<label\>:

```
prepare-upgrade apply <label>
```

This example shows how to apply the downloaded \<label\> with ignore failure option:

```
prepare-upgrade -i apply  <label>
```

This example shows how to auto-activate software after prepare-upgrade:

```
prepare-upgrade -u apply  <label>
```

This example shows how to validate the downloaded 1830 GX G40 manifest:

```
prepare-upgrade validate -f G40-R4.0.0-F-2021.06.03_08_22-sim-188.manifest
```

This example shows how to apply the downloaded 1830 GX G40 manifest:

```
prepare-upgrade apply -f G40-R4.0.0-F-2021.06.03_08_22-sim-188.manifest
```

<!-- page 970 -->
