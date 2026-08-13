---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.111. file-type'
source_lines: 11205-11238
---

## 6.111. file-type

#### Command Description

This command is used to display file-type transfer information.

#### Command Syntax

```
show filetype-<name> [last-completion-status] [last-transfer] [last-duration] [last-operation]
```

#### Command Usage Details

**Table 308: file-type Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 309: file-type Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| name | The file type for which the transfer happened . | string | n/a |
| last-completion-status | Last transfer Status | string | n/a |
| last-transfer | Last transfer Start Timestamp | date-time in the format YYYY-MM-DDThh: mm:ssZ see the set-time command for detailed information. | n/a |
| last-duration | Last transfer duration | time-interval | n/a |
| last-operation | Last transfer operation | unknown, upload, download | n/a |

<!-- page 501 -->
