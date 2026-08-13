---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.328. supported-tom'
source_lines: 24997-25047
---

## 6.328. supported-tom

#### Command Description

This command is used to display the capability information for supported TOM (Tunable/non-tunable Optical Module) in the scope of a particular card.

#### Command Syntax

```
show supported-tom-<card-type>/<port-name>/<tom-type>/<tom-subtype-group> [supported-subtype] [supported-phy-modes] [default-phy-mode]
```

#### Command Usage Details

**Table 756: supported-tom Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 757: supported-tom Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | The card type (for example, CHM1R). | card type | n/a | show |
| port-name | The name of the port | port name | n/a | show |
| tom-type | TOM type name. | TOM type (for example, CFP2-DCO, QSFP28, QSFPDD, etc) | n/a | show |
| tom-subtype-group | TOM subtype group. | string (length 0..32), for example '4x100GE-breakout' | n/a | show |
| supported-subtype | Supported subtypes for this TOM type in this particular card/port. | String | n/a | show |
| supported-phy-mode | The phy-modes that are supported in this TOM for this card. | 100GE, 200GE, 400GE, 2x100GE, 4x100GE, 100G, 4x10G, 4x10GE, 1GE, 2G5, 10G, 10GE, 2G5E, 40GE, 40G, 4x100G, 200G | n/a | show |
| default-phy-mode | The phy-mode that is used by default in this TOM for this card. | 100GE, 200GE, 400GE, 2x100GE, 4x100GE, 100G, 4x10G, 4x10GE, 1GE, 2G5, 10G, 10GE, 2G5E, 40GE, 40G, 4x100G, 200G | n/a | show |

#### Examples

This example shows how to list all the supported TOM entities and attributes:

```
show supported-tom
```

These examples shows how to display the capabilities of specific TOM entities:

```
show supported-tom-CHM1R/6/QSFP28
show supported-tom-CHM1R/6/QSFPDD/2x100GE-breakout
```

<!-- page 1233 -->
