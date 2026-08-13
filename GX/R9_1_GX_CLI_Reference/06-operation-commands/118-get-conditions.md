---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.118. get-conditions'
source_lines: 11557-11604
---

## 6.118. get-conditions

#### Command Description

This command is used to retrieve conditions. A condition is an alarm that is not considered current. This can happen for multiple reasons including the following:

- alarm severity is configured as 'not-reported' or 'not-alarmed'
- alarm is suppressed due to alarm correlation
- alarm is suppressed due to ARC
- alarms is suppressed due to AINS

#### Command Syntax

```
get-conditions [[direction=]<value>] [[resource=]<value>] [[resource-type=]<value>] [[alarm-type=]<value>] [[location=]<value>] [[AID=]<value>]
```

#### Command Usage Details

**Table 322: get-conditions Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 323: get-conditions Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| direction | Direction of the condition. | all, na, ingress, egress | all |
| resource | Resource |  | n/a |
| resource-type | Resource type | string | n/a |
| alarm-type | Type of alarm | string | n/a |
| location | Location of the condition. | all, na, near-end, far-end | all |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | string | n/a |

#### Examples

This example shows how to display the current condition.

```
get conditions
```

<!-- page 518 -->
