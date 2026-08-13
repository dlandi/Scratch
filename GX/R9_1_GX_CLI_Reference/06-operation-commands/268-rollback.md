---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.268. rollback'
source_lines: 20406-20435
---

## 6.268. rollback

#### Command Description

The `rollback commit` must be executed using the commit parameter, and optionally a specific commit-id (if not provided, the most recent commit-record is used). This command works both in Running Datastore (default) or in Candidate Datastore. The rollback operation can be performed on Candidate Datastore, only if it is empty (e.g. this is a way to initialize the Candidate). This command will consider all commit records up to the provided commit-id, obtain the reverse-commands for each record, and replay them in order (from latest to oldest).

#### Command Syntax

```
rollback (commit [<commit-id>] |
```

#### Command Usage Details

**Table 629: rollback Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 630: rollback Command Parameters**

| Parameter | Description |
| --- | --- |
| commit | This CLI command will revert the current Datastore either Running or Candidate, depending on operational mode to the state it was prior to that commit-record. Incase a commit-id is not provided, latest commit record is used. |

<!-- page 1024 -->
