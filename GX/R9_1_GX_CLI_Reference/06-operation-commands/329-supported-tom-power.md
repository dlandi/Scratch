---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.329. supported-tom-power'
source_lines: 25048-25081
---

## 6.329. supported-tom-power

#### Command Description

The command described in this section is used to show `supported-tom-power` attributes.

#### Command Syntax

```
show supported-tom-power-<card-type>/<port-name>/<tom-type> [supported-power-class] [supported-max-power-draw]
```

#### Command Usage Details

**Table 758: supported-tom-power Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 759: supported-tom-power Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | The card type (for example, CHM1R). | card type | n/a | show |
| port-name | The name of the port | port name | n/a | show |
| tom-type | Pluggable form factor (TOM type identity), matching TOM required-type. | TOM type (for example, CFP2- DCO, QSFP28, QSFPDD, etc) | n/a | show |
| supported-power-class | Maximum MSA power class the host port supports for this pluggable type (may partially support that class; see supported-max-power-draw). | uint8 (range 1..8) | n/a | show |
| supported-max-power-draw | Maximum power in watts the host port allows for this pluggable type under supported-power-class. | decimal64 | n/a | show |

<!-- page 1235 -->
