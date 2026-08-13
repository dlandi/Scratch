---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.321. supported-card'
source_lines: 24568-24632
---

## 6.321. supported-card

#### Command Description

This command is used to show the capability information for supported card.

#### Command Syntax

```
show supported-card-<card-type> [node-type-compatibility] [sw-support-revision] [supported-subtype] [description] [default-card-mode]
[supported-card-mode] [card-width] [card-height] [is-field-replaceable] [category] [grid-mode-support] [max-power-draw] [leds]
[location-led-support] [console-port-support] [default-console-baud-rate] [support-serdes-config] [supported-bands] [supported-features]
```

#### Command Usage Details

**Table 742: supported-card Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 743: supported-card Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | Card type name. | identityref | n/a | show |
| node-type-compatibility | Node Type Compatibility refers to supported NE Node-type for a sled card. Only of relevance for line-card(s) and carrier-card(s). Possible values: • all - compatibility with any node-type.<br>• ILA - compatibility with ILA (In-Line Amplifier, two-directions) node type.<br>• OADM - compatibility with OADM (Optical Add/Drop Multiplexer) node type. | • all<br>• ILA<br>• OADM | n/a | show |
| sw-support-revision | Software revision currently installed. | uint16 | 0 | show |
| supported-subtype | Supported card subtypes; may be empty if card doesn't support subtypes. | String | n/a | show |
| description | Human readable description for this card-type. | String (length 0..255) | n/a | show |
| default-card-mode | The default card-mode, for cards whose supported-card-mode is not empty. Only relevant if the card has the concept of card-mode. | String (length 0..20) | n/a | show |
| supported-card-mode | Supported card-modes. May be empty if card does not support any card-mode. | String (length 0..20) | n/a | show |
| card-width | Number of slots this card occupies. It is not-applicable for RU equipment:<br>• na - Not Applicable.<br>• single-slot - single slot width.<br>• double-slot - double slot width.<br>• half-slot - half slot width.<br>• triple-slot - three slot width. | na single-slot double-slot half-slot triple-slot | n/a | show |
| card-height | Card height in RUs (Rack Units). | uint8 (in RUs) | n/a | show |
| is-field-replaceable | Whether this card-type is a field replaceable unit (FRU). | true, false | n/a | show |
| category | Card category. | controller line-card fan power-supply other carrier-card blank | n/a | show |
| grid-mode-support | Grid-mode capabilities:<br>• not-applicable - Not applicable.<br>• flexible-c-band-only - Flexible C-band without fixed-grid characterization.<br>• general-c-band - 4.85THz C-band, fixed or flexi-grid.<br>• general-fixed-c-band - 4.85THz 50GHz, 75GHz or 100GHz, C-band support. Only of relevance for line-card(s). | • not-applicable<br>• flexible-c-band-only<br>• general-c-band<br>• general-fixed-c-band | general-c-band | show |
| max-power-draw | Maximum power draw for this card in Watts. | decimal64 (2 fraction-digits) (W) | n/a | show |
| leds | List of LEDs available for each port of this card. | String (length 1..20) | n/a | show |
| location-led-support | Whether this card-type supports location-led operation. | true, false | n/a | show |
| console-port-support | Whether this card-type supports a serial console port, with or without auto-sensing capabilities:<br>• no - card-type does not have a serial console port.<br>• yes-with-auto-sensing-baud-rate - card-type has a serial console port, supporting auto-sensing of baud-rate.<br>• yes-with-fixed-baud-rate - card-type has a serial console port, supporting manually configured baud-rate. | • no<br>• yes-with-auto-sensing-baud-rate<br>• yes-with-fixed-baud-rate | no | show |
| default-console-baud-rate | Defines the default baud-rate for cards with fixed baud-rate. | unknown 9600 19200 38400 57600 115200 | n/a | show |
| support-serdes-config | If true, it means this card-type allows the user to configure 3rd Party TOM SerDes values. If false, the card has no need for such customization. | true, false | false | show |
| supported-bands | List of bands supported by a card's port. Only applicable to optical dwdm(-line) and AD/ ADE ports.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-band (4.85 THz). • superC-band - SuperC-band (6.1 THz).<br>• standardL-band - Standard L-band (4.85 THz).<br>• standardC-standardL-band - Standard C or Standard L band. | • not-applicable<br>• standardC-band<br>• superC-band • standardL-band<br>• standardC-standardL-bands | standardC-band | show |
| supported-features | Supported features; may be empty if no features are not supported. | String (length 1..64) | n/a | show |

#### Examples

This example shows how to list the capabilities of all cards:

```
show supported-card
```

This example shows how to list the capabilities of a specific card (CHM1R) including the list of supported slots:

```
show supported-card-CHM1R
```

<!-- page 1215 -->
