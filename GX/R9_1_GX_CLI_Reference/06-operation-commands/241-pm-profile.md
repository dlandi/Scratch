---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.241. pm-profile'
source_lines: 18860-18898
---

## 6.241. pm-profile

#### Command Description

These commands are used to set or show a PM profile which contains information on all resources that support PM data, together with its related default configuration.. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set pm-profile [global-data-supervision <value>]
show pm-profile
```

#### Command Usage Details

**Table 574: pm-profile Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 575: pm-profile Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| global-data-supervision | This parameter provides a way to globally enable PM data-supervision without having to toggle it individually: • auto-enabled - Global enabling of PM data-supervision flag.<br>• manual - PM data-supervision flag is controlled via pm-profile-entry, or directly per pm-control-entry. | auto-enabled manual | manual | set, show |

#### Examples

This example shows how to set pm-profile global-data-supervision to auto-enabled:

```
set pm-profile global-data-supervision auto-enabled
```

<!-- page 951 -->
