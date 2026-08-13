---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.273. sc-rx'
source_lines: 20614-20646
---

## 6.273. sc-rx

#### Command Description

The commands described in this section are used to show the Receiving Secure Channel (`sc-rx`) attributes.

#### Command Syntax

```
show sc-rx-<name>/<index> [sci-rx] [state] [association-number] [key-identifier] [next-packet-number]
```

#### Command Usage Details

**Table 639: sc-rx Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 640: sc-rx Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | show |
| index | Receiving Secure Channel Index. | uint8 | n/a | show |
| sci-rx | Receiving Secure Channel Identifier hex string. | hex-string (length 16) | n/a | show |
| state | State of the secure channel returned by MKA stack: • in-use: Indicates Secure Association(s) under this Secure Channel is in use.<br>• not-in-use: No Secure Association(s) under this Secure Channel are in use. | • in-use<br>• not-in-use | n/a | show |

<!-- page 1034 -->
