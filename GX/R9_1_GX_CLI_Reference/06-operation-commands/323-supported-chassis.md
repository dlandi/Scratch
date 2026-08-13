---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.323. supported-chassis'
source_lines: 24672-24742
---

## 6.323. supported-chassis

#### Command Description

This command is used to show the capability information for supported chassis.

#### Command Syntax

```
show supported-chassis-<chassis-type> [supported-subtype] [default-subtype] [description] [controller-redundancy-supported]
[power-control-supported] [fan-adjustment-on-altitude] [dust-filter-replacement] [depth] [height] [number-of-front-slots] [number-of-rear-slots]
[leds] [supported-subchassis-type] [supported-features]
```

#### Command Usage Details

**Table 746: supported-chassis Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 747: supported-chassis Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| chassis-type | Chassis type name. | identityref (for example: G31, G32, G34c, G42, or other chassis type) | n/a | show |
| supported-subtype | Supported chassis subtypes. May be empty if chassis doesn't support subtypes. | String | n/a | show |
| default-subtype | Default subtype supported by chassis. | String | n/a | show |
| description | Human readable description for this chassis type. | String (length 0..255) | n/a | show |
| controller-redundancy-supported | Whether this chassis supports controller redundancy or not. | true, false | n/a | show |
| power-control-supported | Whether this chassis supports power control, i.e. the ability to evaluate the power supply currently provided by the PEMs against the configured equipment. A chassis that has power control support may put some cards into low power mode when not enough power is enabled, as well as raising alarms when power protection fail. | true, false | n/a | show |
| fan-adjustment-on-altitude | Whether FAN(s) rotation are automatically adjusting based on the configured altitude. | true, false | false | show |
| dust-filter-replacement | Chassis characteristics related with dust filter (and its replacement):<br>• not-applicable - No dust filter.<br>• optional-dust-filter - Optional dust-filter and replacement.<br>• dust-filter-regularly-replaced - Dust filter must be regularly replaced. | • not-applicable<br>• optional-dust-filter<br>• dust-filter-regularly-replaced | optional-dust-filter | show |
| depth | Chassis depth in millimeters. | uint16 (mm) | n/a | show |
| height | Chassis height in RUs (Rack Units). | uint8 (RUs) | n/a | show |
| number-of-front-slots | Number of equipment holder slots in the front plate on the chassis. | uint8 | n/a | show |
| number-of-rear-slots | Number of equipment holder slots in the back plate on the chassis. | uint8 | n/a | show |
| leds | List of LEDs available for each port of this card. | String (length 1..20) | n/a | show |
| supported-subchassis-type | List of chassis-types that this chassis supports as sub-chassis. The list has a maximum of 10 elements. If empty, means this chassis-type does not support multi-chassis feature. | leafref (path ../../supported-chassis/chassis-type) | n/a | show |
| supported-features | Supported features. May be empty if no features are supported. | String (length 0..64) | n/a | show |

#### Examples

The following example shows how to list the capabilities of the chassis and lists the supported slots:

```
show supported-chassis
```

The following output is displayed for a 1830 GX G40 node:

```
supported-chassis      description      controller-redundancy-supported  power-control-supported  depth (mm)  height (RUs)
---------------------  ---------------  -------------------------------  -----------------------  ----------  ------------
supported-chassis-G42  G42 3RU chassis  true                             true                     600         3
supported-chassis      number-of-front-slots  number-of-rear-slots  leds  supported-subchassis-type
---------------------  ---------------------  --------------------  ----  -------------------------
supported-chassis-G42  7                      12                          G42
```

The following example shows how to view the capabilities of a 1830 GX G31 chassis:

```
show supported-chassis-G31
```

<!-- page 1220 -->
