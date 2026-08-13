---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.200. oadm-capabilities'
source_lines: 16057-16098
---

## 6.200. oadm-capabilities

#### Command Description

This command is used to show OADM capabilities.

#### Command Syntax

```
show oadm-capabilities [max-degrees] [max-adgs]
```

#### Command Usage Details

**Table 486: oadm-capabilities Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 487: oadm-capabilities Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| max-degrees | Maximum number of degrees; 0 if not supported. Degrees are only supported in OADM node types. The maximum degrees for ILA node-type is 0 by convention (ILA has only 2 directions, fixedly). This is not shown in supported capabilities. i Note: The maximum number of Degrees is not necessarily the deployment supported configurations, but the maximum number of a 'working' degree-number. | 20 (uint8) | n/a | show |
| max-adgs | Maximum number of ADGs (Add/ Drop Group(s)); 0 if not supported. ADGs are only supported in OADM node types. | 110 (uint8) | n/a | show |

#### Examples

The following example shows how to view OADM capabilities:

```
show oadm-capabilities
  oadm-capabilities
  max-degrees                    20
  max-adgs                       110
```

<!-- page 738 -->
