---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.261. recovery'
source_lines: 19993-20057
---

## 6.261. recovery

#### Command Description

These commands are used configure and display the status of system recovery from chassis storage.

#### Command Syntax

```
set recovery [restore-from-chassis-storage <value>]
show recovery [restore-from-chassis-storage] [restore-status] [backup-status] [last-backup] [next-backup]
```

#### Command Usage Details

**Table 614: recovery Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 615: recovery Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| restore-from-chassis-storage | Type of system recovery from chassis storage:<br>• disabled - Chassis storage is not used for restoration in this NE.<br>• auto-restore - SW and DB are stored on the chassis storage and restored in recovery mode. A manual clear recovery-mode command is then. This allows the user to validate the restored system before affecting the HW.<br>• auto-in-service - SW and DB are stored on the chassis storage and restored in recovery mode. On successful restore, the NC will automatically leave recovery mode. | • disabled<br>• auto-restore<br>• auto-in-service | auto-in-service | set, show |
| restore-status | Current state of the restoration:<br>• init - Provisioning service is starting<br>• image-install-in-progress - Installing backup image<br>• db-restore-in-progress - Restoring database<br>• check-completed - Provisioning service completed provisioning<br>• failed - Provisioning failed, requires manual provisioning<br>• disabled - Provisioning service is disabled, no backups are being performed • wait-for-upgrade - Waiting for system reboot after image upgrade<br>• wait-for-db-restore - Waiting for system reboot after database restore | • init<br>• image-install-in-progress<br>• db-restore-in-progress<br>• check-completed<br>• failed<br>• disabled<br>• wait-for-upgrade<br>• wait-for-db-restore | init | show |
| backup-status | Current state of the last backup:<br>• successful - Provisioning service is enabled; backups are being performed successfully<br>• failed - Provisioning service is enabled; a backup failed.<br>• in-progress - Backup is in progress<br>• unknown - Backup is in an unknown state | • successful<br>• failed<br>• in-progress<br>• unknown | Unknown | show |
| last-backup | Timestamp with the last backup performed. | • Time stamp in system format (date-time or time only)<br>• never | never | show |
| next-backup | Timestamp for the next backup to be performed. | • Time stamp in system format (date-time or time only)<br>• never | never | show |

#### Examples

This example shows how to view recovery attributes:

<!-- page 1008 -->

```
show recovery
  recovery
  restore-from-chassis-storage    auto-in-service
  restore-status                  check-completed
  backup-status                   successful
  last-backup                     '2023-01-11T14:02:11Z'
  next-backup                     '2023-01-12T13:57:50Z'
```

This example shows how to view recovery restore-from-chassis-storage attribute:

```
show recovery restore-from-chassis-storage
  recovery
  restore-from-chassis-storage    auto-in-service
```

The following example show how to set recovery restore-from-chassis-storage attribute:

```
set recovery restore-from-chassis-storage auto-in-service
```

<!-- page 1009 -->
