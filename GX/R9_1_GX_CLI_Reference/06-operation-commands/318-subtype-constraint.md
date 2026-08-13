---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.318. subtype-constraint'
source_lines: 24418-24453
---

## 6.318. subtype-constraint

#### Command Description

This command is used to show software subtype-constraint information.

#### Command Syntax

```
show subtype-constraint-<card-type>/<subtype> [min-capacity] [max-capacity] [supported-applications] [unsupported-applications] [description]
```

#### Command Usage Details

**Table 736: subtype-constraint Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 737: subtype-constraint Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | Represents a group of related PONs for a card type to which these constraints applies. | string (length 1...20) | n/a | show |
| subtype | Represents a group of related PONs for a card type to which these constraints applies. | string length 1...20 | n/a | show |
| min-capacity | The minimum capacity supported by this subtype. | gbps | n/a | show |
| max-capacity | The maximum capacity supported by this subtype. -1 means there is no maximum capacity constraint. | gbps | n/a | show |
| supported-applications | List of applications supported by this subtype. If this list is empty, then this constraint is not applicable. | application type | n/a | show |
| unsupported-applications | List of applications not supported by this subtype. If this list is empty, then this constraint is not applicable. | application type | n/a | show |
| description | Subtype description | string length 1...255 | n/a | show |

<!-- page 1202 -->
