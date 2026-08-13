---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.292. show commit'
source_lines: 22756-22801
---

## 6.292. show commit

#### Command Description

The `show commit` retrieves the commit record information from the system. This command allows the user to visualize the Commit Repository records. This command is available only when:

- If commit-tracking policy is enabled.
- OR If a pending Confirmed Commit exists

#### Command Syntax

```
show commit [<id>|-s=<since>|-n=<number-of-records>]
```

#### Command Usage Details

**Table 682: show commit Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 683: show Command Flags**

| Parameter | Description |
| --- | --- |
| -n | This parameter selects an exact number of records to obtain. |
| -s | This parameter which allows to pick a timestamp, showing all records created since that timestamp. |
| &lt;id&gt; | This parameter allows to select a specific commit-record. |

<!-- page 1126 -->

**Table 684: show Command Parameters**

| Parameter | Description |
| --- | --- |
| entity-id | Instance ID of the entity where to perform the show. |
| attribute | Name of the attribute to be provided. |
| value | Value of the attribute to be initialized. |
| filter | Filter (&lt;attribute&gt;=&lt;value&gt;). |

<!-- page 1127 -->
