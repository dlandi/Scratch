---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.242. pm-profile-entry'
source_lines: 18899-18934
---

## 6.242. pm-profile-entry

#### Command Description

These commands are used to set or show the PM configuration per resource type.

#### Command Syntax

```
set pm-profile-entry-<resource-type>/<direction>/<location>/<period> [default-data-supervision <value>] [default-tca-supervision <value>]
show pm-profile-entry-<resource-type>/<direction>/<location>/<period> [default-data-supervision] [default-tca-supervision]
```

#### Command Usage Details

**Table 576: pm-profile-entry Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 577: pm-profile-entry Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource | Type of resource. | Instance ID | n/a | set, show |
| period | Time period for PM data. | PM period | n/a | set, show |
| direction | PM parameter direction. | ingress, egress, all, na | all | set, show |
| location | PM parameter location | all, na, near-end, far-end | all | set, show |
| default-data-supervision | For newly created resources of this type, whether they have PM data supervision automatically enabled or not. | true, false | n/a | set, show |
| default-tca-supervision | For newly created resources of this type, whether they have PM threshold crossing supervision automatically enabled or not. | true, false | n/a | set, show |

<!-- page 953 -->
