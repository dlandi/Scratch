---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.253. protection-unit'
source_lines: 19539-19584
---

## 6.253. protection-unit

#### Command Description

These commands are used to set or show a protection unit.

#### Command Syntax

```
set protection-unit-<protection-group-name>/<protection-unit-name> [alarm-report-control <allowed|inhibited>] [label <value>]
show protection-unit-<protection-group-name>/<protection-unit-name> [AID] [transport-entity] [state] [role] [alarm-report-control] [label]
```

#### Command Usage Details

**Table 598: protection-unit Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 599: protection-unit Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| protection-group-name | The name of the protection group | string | n/a | set, show |
| protection-unit-name | The name of the protection unit | string | n/a | set, show |
| alarm-report-control | Switch enabling alarm reporting | allowed, inhibited | inhibited | set, show |
| label | User-configurable name of the protection unit. | string | na | set, show |
| AID | The object identifier | string | n/a | show |
| transport-entity | The instance identifier of the transport entity. | string | n/a | show |
| state | The state of the protection-unit. | active, standby, available, unknown | n/a | show |
| role | Protection unit role | working, protection | n/a | show |

#### Examples

This example shows how to set a protection unit in 1830 GX G40 environment:

```
set protection-unit-test/1-6-T1 alarm-report-control inhibited
```

<!-- page 987 -->
