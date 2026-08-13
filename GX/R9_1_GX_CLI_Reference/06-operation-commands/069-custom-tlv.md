---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.69. custom-tlv'
source_lines: 8323-8355
---

## 6.69. custom-tlv

#### Command Description

This command is used to show a list of Organizational Specific TLVs (Type-Lengh-Value) parameters information.

#### Command Syntax

```
show custom-tlv-<lldp-port>/<direction>/<oui>/<subtype> [value]
```

#### Command Usage Details

**Table 217: custom-tlv Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 218: custom-tlv Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| lldp-port | Local port that is associated with the LLDP agent. | string length 0...64 |
| direction | Direction associated with lldp statistics. | ingress, egress |
| oui | The Organization Unique Identifier (OUI) of this TLV. Hexadecimal representation of the 24 bit identifier. | string length 1...6 |
| subtype | The sub-type identifier of the TLV in the scope of the OUI The firmware name | string length 1...507 |

<!-- page 367 -->
