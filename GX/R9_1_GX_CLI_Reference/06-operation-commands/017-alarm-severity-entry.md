---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.17. alarm-severity-entry'
source_lines: 5197-5262
---

## 6.17. alarm-severity-entry

#### Command Description

The commands described in this section are used to set or show the individual entry in alarm-severity-entry. It allows to configure the severity for one particular alarm. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set alarm-severity-entry-<resource-type>/<alarm-type>/<direction>/<location> [severity <value>]
show alarm-severity-entry-<resource-type>/<alarm-type>/<direction>/<location> [severity] [service-affecting]
```

**Tip:** To display a list of available entities, press the "?" after `set alarm-severity-entity`.

#### Command Usage Details

**Table 104: alarm-severity-entry Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 105: alarm-severity-entry Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource-type | Type of resource to be modified (for example, CHM1R, FAN, 1830 GX G31, etc). | string | n/a | set, show |
| alarm-type | Type of alarm, based on an abbreviated code (for example, ADMIN-LOCK, BERSD, etc). | string | n/a | set, show |
| direction | Configured direction of the current resource type (alarm direction). Can be ingress or egress. | na, ingress, egress | n/a | set, show |
| location | Configured location of the current resource type (alarm location). | na, near-end, far-end | n/a | set, show |
| severity | Configured severity of the current resource type. | critical event major minor not-reported warning | n/a | set, show |
| service-affecting | Possible alarm service affecting category. Indicates if the alarm affects service. | indeterminate sa nsa sa-nsa | n/a | show |

#### Examples

This example shows how to display all the alarm severity entries:

```
show alarm-severity-entry
```

This example shows how to display the parameters of the alarm entry:

```
show alarm-severity-entry-OTU2/TIM/ingress/near-end
```

This example shows how to set the severity of an alarm entry:

<!-- page 188 -->

```
set alarm-severity-entry-OTU2/TIM/ingress/near-end severity minor
```

This example shows how to set the severity of an alarm profile:

```
set alarm-severity-entry-tom/EQPTCOMFAIL severity major
```

<!-- page 189 -->
