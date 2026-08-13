---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.239. pm-control-entry'
source_lines: 18763-18799
---

## 6.239. pm-control-entry

#### Command Description

These commands are used to set or show the PM configuration for one particular resource, for one particular period, direction and location.

#### Command Syntax

```
set pm-control-entry-<resource>/<period>/<direction>/<location> [data-supervision <value>] [tca-supervision <value>]
show pm-control-entry-<resource>/<period>/<direction>/<location> [supported-parameters] [data-supervision] [tca-supervision]
```

#### Command Usage Details

**Table 570: pm-control-entry Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 571: pm-control-entry Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource | Existing system resource. | Instance ID | n/a | set, show |
| period | Time period for PM data. | PM period | n/a | set, show |
| direction | PM parameter direction. | ingress, egress, all, na | all | set, show |
| location | PM parameter location | all, na, near-end, far-end | all | set, show |
| supported-parameters | List of PM parameters that this resource type supports for this direction/location with a maximum of 100 elements. | PM parameter | n/a | show |
| data-supervision | PM data supervision for this resource. | true, false | n/a | set, show |
| tca-supervision | TCA supervision for this resource. | true, false | n/a | set, show |

<!-- page 946 -->
