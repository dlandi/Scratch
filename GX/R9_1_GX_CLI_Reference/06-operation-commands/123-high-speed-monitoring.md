---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.123. high-speed-monitoring'
source_lines: 11825-11856
---

## 6.123. high-speed-monitoring

#### Command Description

The commands described in this section are used to set or show the `high-speed-monitoring` attributes.

#### Command Syntax

```
set high-speed-monitoring [enabled <value>] [port <value>]
show high-speed-monitoring [enabled] [port]
```

#### Command Usage Details

**Table 333: high-speed-monitoring Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 334: high-speed-monitoring Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| enabled | User configurable switch to enable or disable high speed monitoring. | • true<br>• false | false | set, show |
| port | User configurable port. | uint16 (range [1..max]) | 57500 | set, show |

<!-- page 528 -->
