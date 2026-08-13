---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.360. unlock'
source_lines: 26901-26938
---

## 6.360. unlock

#### Command Description

This command will release a previously locked database (achieved by using the 'lock' command). After the 'unlock', any session will be able to perform configurations, as well as to perform a new lock. Only the session that performed the 'lock' can trigger the 'unlock'. If a session tries to unlock a non-locked database, or a database that was locked by another session, the 'unlock' command will fail. A lock will be automatically released if the session that performed the lock is closed.

#### Command Syntax

```
unlock [-h]
```

#### Command Usage Details

**Table 826: unlock Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 827: unlock Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Examples

This example shows how to unlock a previously locked database:

```
unlock
```

<!-- page 1315 -->
