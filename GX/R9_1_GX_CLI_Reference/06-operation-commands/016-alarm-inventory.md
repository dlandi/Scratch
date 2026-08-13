---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.16. alarm-inventory'
source_lines: 5153-5196
---

## 6.16. alarm-inventory

#### Command Description

The command described in this section is used to show the inventory with all possible alarm types for the system, containing static information for each alarm type.

#### Command Syntax

```
show alarm-inventory-<alarm-type> [resource-type] [alarm-category] [alarm-type-description] [corrective-action] [default-severity]
[service-affecting]
```

#### Command Usage Details

**Table 102: alarm-inventory Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 103: alarm-inventory Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| alarm-type | Type of alarm, based on an abbreviated code. | identityref | n/a | show |
| resource-type | Type of resource. | identityref | n/a | show |
| alarm-category | Category of the alarm type. | communication facility equipment environmental processing-error software quality-of-service security | n/a | show |
| alarm-type-description | Description of the type of the alarm. | string (length 0..128) | n/a | show |
| corrective-action | System provided information on how to correct the situation that triggered this alarm. | string (length 0..256) | n/a | show |
| default-severity | List of possible default severities for this alarm type. The same alarm may have different default severities depending of the resource-type it applies to. | critical major minor warning not-reported event | n/a | show |
| service-affecting | Information on whether this alarm is service affecting or not. In some cases, the same alarm may be simultaneously 'sa' and 'nsa', depending on the resource-type it applies to. | indeterminate, sa, nsa, sa-nsa | n/a | show |

#### Examples

```
show alarm-inventory-TIM
show alarm-inventory-MSIM
```

<!-- page 186 -->
