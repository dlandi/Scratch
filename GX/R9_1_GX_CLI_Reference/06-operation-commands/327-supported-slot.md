---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.327. supported-slot'
source_lines: 24868-24996
---

## 6.327. supported-slot

#### Command Description

This command is used to show the capability for each slot within each supported chassis.

#### Command Syntax

```
show supported-slot-<card-type>/<slot-name> [slot-location] [slot-vertical-position] [slot-horizontal-position] [possible-card-types]
[configuration-mode] [auto-provision-capable] [default-card] [requires-blank-when-empty] [reset-power] [virtual-slot] [leds]
show supported-slot-<chassis-type>/<slot-name> [slot-location] [slot-vertical-position] [slot-horizontal-position] [possible-card-types]
[configuration-mode] [auto-provision-capable] [default-card] [requires-blank-when-empty] [reset-power] [virtual-slot] [leds]
```

#### Command Usage Details

**Table 754: supported-slot Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 755: supported-slot Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | Card type name (for example, CHM1R). | Card type (for example, CHM1R, FRCU31, IOPANEL) | n/a | show |
| chassis-type | The type of the chassis (for example: G31, G32, G34c, G42). | String | n/a | show |
| slot-name | The name of the slot. | String | n/a | show |
| slot-location | Physical location of the slot in the chassis. | front rear | n/a | show |
| slot-vertical-position | Position of the slot vertically in the chassis, counting from the top of the chassis, in RUs. Example: position 3 means third RU starting from the top of the chassis. | uint8 | n/a | show |
| slot-horizontal-position | Position of the slot horizontally in the chassis within the current RU, counting from the left of the chassis. For back slots, the position is counted also from the left, from a point of view facing\n the rear of the chassis. | uint8 | n/a | show |
| possible-card-types | List of possible card types in this slot. The list has a maximum of 15 elements. | identityref | n/a | show |
| configuration-mode | Configuration mode for the cards in this slot (or toms in this port):<br>• system-configured - system automatically configures the card in slot, and user cannot make changes.<br>• user-configured - user can provision or de-provision cards in this slot. | system-configured user-configured | disabled | show |
| auto-provision-capable | Whether this slot supports card auto-provisioning. | true, false | n/a | show |
| default-card | Card that exists in this slot by default. | String | n/a | show |
| requires-blank-when-empty | Whether this slot requires a BLANK filler card when empty. | not-applicable optional required | n/a | show |
| reset-power | Reset power consumption for this card, at 55ºC, in W units. | decimal64 ( 2 fraction-digits) | n/a | show |
| virtual-slot | Describes whether this slot is virtual. | true, false | false | show |
| leds | List of LEDs available in the slot. The list has a maximum of 10 elements. | string (length 1..20) | n/a | show |

#### Examples

This example shows how to display the capabilities of all slots:

```
show supported-slot
```

The following output displays an example from a 1830 GX G40 node:

```
show supported-slot
supported-slot                slot-location  slot-vertical-position  slot-horizontal-position  possible-card-types  configuration-mode
 auto-provision-capable  default-card
----------------------------  -------------  ----------------------  ------------------------  -------------------  ------------------
 ----------------------  ------------
supported-slot-G42/1          front          1                       1                         XMM4                 user-configured     true
                none
supported-slot-G42/2          front          1                       2                         IOPANEL              system-configured   false
                IOPANEL
supported-slot-G42/3          front          1                       3                         XMM4                 user-configured     true
                none
supported-slot-G42/4          front          2                       1                         CHM6,UCM4            user-configured     true
                none
supported-slot-G42/5          front          2                       2                         CHM6,UCM4            user-configured     true
                none
supported-slot-G42/6          front          3                       1                         CHM6,UCM4            user-configured     true
                none
supported-slot-G42/7          front          3                       2                         CHM6,UCM4            user-configured     true
                none
supported-slot-G42/FAN-1      rear           2                       6                         FAN                  system-configured   false
                 FAN
supported-slot-G42/FAN-2      rear           2                       5                         FAN                  system-configured   false
                 FAN
supported-slot-G42/FAN-3      rear           2                       3                         FAN                  system-configured   false
                 FAN
supported-slot-G42/FAN-4      rear           2                       2                         FAN                  system-configured   false
                 FAN
supported-slot-G42/FAN-5      rear           2                       1                         FAN                  system-configured   false
                 FAN
supported-slot-G42/FAN-6      rear           1                       4                         XMM4-FAN             system-configured   false
                XMM4-FAN
supported-slot-G42/FAN-7      rear           1                       3                         XMM4-FAN             system-configured   false
                XMM4-FAN
supported-slot-G42/FANCTRL-1  rear           2                       4                         FAN-CTRL             system-configured   false
                FAN-CTRL
supported-slot-G42/PEM-1      rear           1                       6                         PEM                  user-configured     true
                 PEM
supported-slot-G42/PEM-2      rear           1                       5                         PEM                  user-configured     true
                 PEM
supported-slot-G42/PEM-3      rear           1                       2                         PEM                  user-configured     true
                 PEM
supported-slot-G42/PEM-4      rear           1                       1                         PEM                  user-configured     true
                 PEM
supported-slot                requires-blank-when-empty  reset-power (W)
----------------------------  -------------------------  ---------------
supported-slot-G42/1          required                   0.0000
supported-slot-G42/2          not-applicable             0.0000
supported-slot-G42/3          required                   0.0000
supported-slot-G42/4          required                   25.0000
supported-slot-G42/5          required                   25.0000
supported-slot-G42/6          required                   25.0000
supported-slot-G42/7          required                   25.0000
supported-slot-G42/FAN-1      not-applicable             0.0000
supported-slot-G42/FAN-2      not-applicable             0.0000
supported-slot-G42/FAN-3      not-applicable             0.0000
supported-slot-G42/FAN-4      not-applicable             0.0000
supported-slot-G42/FAN-5      not-applicable             0.0000
supported-slot-G42/FAN-6      not-applicable             0.0000
supported-slot-G42/FAN-7      not-applicable             0.0000
supported-slot-G42/FANCTRL-1  not-applicable             0.0000
supported-slot-G42/PEM-1      required                   0.0000
supported-slot-G42/PEM-2      required                   0.0000
supported-slot-G42/PEM-3      required                   0.0000
supported-slot-G42/PEM-4      required                   0.0000
```

This example shows how to display the capabilities of a specific slot from a 1830 GX G30 node:

```
show supported-slot-G31/2
```

<!-- page 1231 -->
