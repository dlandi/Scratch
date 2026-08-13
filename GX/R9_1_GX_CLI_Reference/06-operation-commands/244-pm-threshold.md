---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.244. pm-threshold'
source_lines: 18986-19024
---

## 6.244. pm-threshold

#### Command Description

These commands are used to add, set, show or delete a PM threshold.

#### Command Syntax

```
add pm-threshold-<resource>/<period>/<direction>/<location>/<parameter> [low-threshold <value>] [high-threshold <value>]
set pm-threshold-<resource>/<period>/<direction>/<location>/<parameter> [low-threshold <value>] [high-threshold <value>]
show pm-threshold-<resource>/<period>/<direction>/<location>/<parameter> [low-threshold] [high-threshold]
delete pm-threshold-<resource>/<period>/<direction>/<location>/<parameter>
```

#### Command Usage Details

**Table 580: pm-threshold Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 581: pm-threshold Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource | Existing system resource. | Instance ID | n/a | add, set, show, delete |
| period | Time period for PM data. | PM period | n/a | add, set, show, delete |
| direction | PM parameter direction. | ingress, egress, all, na | all | add, set, show, delete |
| location | PM parameter location | all, na, near-end, far-end | all | add, set, show, delete |
| parameter | PM parameter identifier (can be a counter or a gauge). | PM parameter | n/a | add, set, show, delete |
| low-threshold | Configured low threshold value for resources that have this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | na | add, set, show |
| high-threshold | Configured high threshold value for resources that have this parameter. | Number (uint64, int64, decimal64); na (not applicable); not-supported | na | add, set show |

<!-- page 957 -->
