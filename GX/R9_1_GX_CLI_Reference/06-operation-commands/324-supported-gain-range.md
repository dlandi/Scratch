---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.324. supported-gain-range'
source_lines: 24743-24775
---

## 6.324. supported-gain-range

#### Command Description

This command is used to display the supported gain range.

#### Command Syntax

```
show supported-gain-range-<name>/<gain-range-type> [gain-range-min] [gain-range-max]
```

#### Command Usage Details

**Table 748: supported-gain-range command usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate configuration mode |

#### Command Parameters

**Table 749: supported-gain-range Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Flag to enable or disable access control. | true, false | false | show |
| gain-range-type | Type of gain-range | low, high, standard | n/a | show |
| gain-range-min | The minimum settable gain-target for this type of range ('standard'/ 'low'/ 'high'). | type gain-value | n/a | show |
| gain-range-max | The maximum settable gain-target for this type of range ('standard'/ 'low'/ 'high'). | type gain-value | n/a | show |

<!-- page 1221 -->
