---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.162. lock'
source_lines: 14052-14098
---

## 6.162. lock

#### Command Description

This command is used to lock the database access to the current session. This command grants exclusive write access to the current CLI session. It is intended for ensuring configuration mastership for a small time. While the database is locked, another session (CLI or other type) that tries to make a configuration will receive an error. A lock will not be possible if another session is currently holding the lock. The lock can be released:

- by using the 'unlock' command.
- by closing the session that performed the lock (explicitly via user, via administrator closing the session, or via inactivity time-out)

#### Command Syntax

```
lock [-h]
```

#### Command Usage Details

**Table 412: lock Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 413: lock Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Command Parameters

None.

<!-- page 632 -->

#### Examples

This example shows how to lock the database write access to the current session:

```
lock
```

<!-- page 633 -->
