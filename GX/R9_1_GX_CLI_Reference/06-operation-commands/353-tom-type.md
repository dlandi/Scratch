---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.353. tom-type'
source_lines: 26424-26467
---

## 6.353. tom-type

#### Command Description

This command is used to show the capabilities of the supported TOM (Tunable/non-tunable Optical Module) pluggable types.

#### Command Syntax

```
show tom-type-<tom-type> [data-rate] [description] [support-third-party-toms] [generic-subtype]
```

#### Command Usage Details

**Table 811: tom-type Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 812: tom-type Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| description | Human readable description for this TOM type. | String (length 0..255) | n/a | show |
| tom-type | TOM type name. | identityref (type of the TOM) | n/a | show |
| data-rate | The approximate data-rate for this TOM type. | uint16 (Gbps) | n/a | show |
| support-third-party-toms | Whether this TOM type accepts third party TOMs in addition to supported Nokia TOMs. | false, true | n/a | show |
| generic-subtype | 3rd party subtype for this TOM. | String | n/a | show |

#### Examples

This example shows how to view the list of types of TOMs:

<!-- page 1291 -->

```
show tom-type
```

<!-- page 1292 -->
