---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.14. alarm'
source_lines: 5025-5100
---

## 6.14. alarm

#### Command Description

This command is used to clear alarms that have no auto criteria to be cleared and to show currently raised alarms. **clear alarm** In the majority of cases, Alarms have an automatic raise criteria, and equally a clear criteria. There is a very small subset of alarms that do not have a clear criteria, and instead require user to 'acknowledge' and 'clear' the alarm manually. The list of these alarms is available within the alarm-inventory list, for entries that have can-be-cleared-by-user = true. In CLI, this list can be obtained with "show alarm-inventory can-be-cleared-by-user=true".

**Note:** This mechanism applies only to system alarms that are not associated with any particular resource, but with the system itself in that they are system wide/global alarms.

**show alarm** This command is used to retrieve information from the system. Using the 'alarm' keyword allows to visualize the currently raised alarms. Providing additional parameters allows to filter the results for specific alarms types, severities, entities, etc.

**Tip:** Use `show alarm <tab>` to auto-complete all possible filters that are supported.

**Tip:** For OTU alarms raised while a CHM6 card is warm rebooting, it may take up to five minutes for the OTU alarms raised during reboot to be shown as cleared using the -a show alarm command.

#### Command Syntax

```
clear [-f] alarm [alarm-type=]<value> [[resource=]<value>[,<value>]*]
show alarm-<alarm-id> [resource] [resource-type] [AID] [alarm-type] [alarm-type-description] [direction] [location] [perceived-severity]
[reported-time] [service-affecting] [alarm-category] [additional-details] [corrective-action] [label] [last-changed-time] [operator-state]
[operator-text] [operator-name] [operator-last-action]
show alarms
```

#### Command Usage Details

**Table 97: alarm Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 98: alarm Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 99: alarm Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| alarm-type | the type of alarm to clear. | string | n/a | clear |
| alarm-id | Alarm instance that represents a raised alarm, when entry is created, or a cleared alarm, when entry is deleted. | - | - | show |
| resource | Existing system resource. | - | - | show |
| resource-type | Type of resource. | - | - | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | - | - | show |
| alarm-type | Type of alarm, based on an abbreviated code. | - | - | show |
| alarm-type-description | Description of the type of the alarm. | - | - | show |
| direction | Direction of the alarm. | - | - | show |
| location | Location of the alarm. | - | - | show |
| perceived-severity | Severity of the alarm. | - | - | show |
| reported-time | Occurrence timestamp for the alarm. | - | - | show |
| service-affecting | Information on whether this alarm is service affecting or not. | - | - | show |
| alarm-category | Category of the alarm type. | - | - | show |
| additional-details | Free string with additional relevant information provided by the system (length 0..256). | - | - | show |
| corrective-action | System provided information on how to correct the situation that triggered this alarm. | - | - | show |
| label | User label. | - | - | show |
| last-changed-time | Timestamp of the last change occurred in the alarm. | - | - | show |
| operator-state | State of the alarm according with operator action. | - | - | show |
| operator-text | Text provided by operator when changing alarm state (length 0..256). | - | - | show |
| operator-name | Username that last changed the state of this alarm. | - | - | show |
| operator-last-action | Timestamp when the alarm was last changed by operator. | - | - | show |

#### Examples

This example shows how to clear the alarm type DBRESTOREFAIL:

```
clear alarm DBRESTOREFAIL
```

<!-- page 182 -->
