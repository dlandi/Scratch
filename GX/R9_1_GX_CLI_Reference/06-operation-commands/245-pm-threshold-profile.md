---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.245. pm-threshold-profile'
source_lines: 19025-19076
---

## 6.245. pm-threshold-profile

#### Command Description

These commands are used to set or show PM configuration per parameter, for this resource type.

#### Command Syntax

```
set pm-threshold-profile-<resource-type>/<direction>/<location>/<period>/<parameter> [low-threshold <value>] [high-threshold <value>]
show pm-threshold-profile-<resource-type>/<direction>/<location>/<period>/<parameter> [low-threshold] [high-threshold] [default-low-threshold]
[default-high-threshold] [min-value] [max-value]
```

#### Command Usage Details

**Table 582: pm-threshold-profile Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 583: pm-threshold-profile Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource | Existing system resource. | Instance ID | n/a | set, show |
| period | Time period for PM data. | PM period | n/a | set, show |
| direction | PM parameter direction. | ingress, egress, all, na | all | set, show |
| location | PM parameter location | all, na, near-end, far-end | all | set, show |
| parameter | PM parameter identifier (can be a counter or a gauge). | PM parameter | n/a | set, show |
| low-threshold | Configured low threshold value for resources that have this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | n/a | set, show |
| high-threshold | Configured high threshold value for resources that have this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | n/a | set, show |
| default-low-threshold | System defined default value for low threshold for this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | n/a | show |
| default-high-threshold | System defined default value for high threshold for this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | n/a | show |
| min-value | Minimum value for this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | n/a | show |
| max-value | Maximum value for this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | n/a | show |

#### Examples

This example shows how to set the contents of the a pm parameter threshold profile:

<!-- page 959 -->

```
set pm-threshold-profile-OTUC14i/ingress/near-end/pm-24h/errored-seconds high-threshold 1200
```

<!-- page 960 -->
