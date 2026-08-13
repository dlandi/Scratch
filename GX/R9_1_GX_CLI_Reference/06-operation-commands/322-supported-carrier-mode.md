---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.322. supported-carrier-mode'
source_lines: 24633-24671
---

## 6.322. supported-carrier-mode

#### Command Description

This command is used to display a list of supported carrier modes.

#### Command Syntax

```
show supported-carrier-mode-<name>/<carrier-mode> [capacity] [client-mode] [baud-rate] [application] [compatibility-id] [status]]
[supported-subtypes]
```

#### Command Usage Details

**Table 744: supported-carrier-mode Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate configuration mode |

#### Command Parameters

**Table 745: supported-carrier-mode Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| name | The name of the carrier mode. | string |
| carrier-mode | An acronymized code (handle) that is indicative of the optical carrier line mode (4-tuple) combination. The format is as follows: &lt;Capacity&gt;&lt;ClientMode&gt;.&lt;Baud Rate&gt;&lt;Application ID&gt; Examples: - 600E.84P - 100X.73U - 325M.66P | string length 1...15 |
| capacity | The net capacity of the optical carrier. | gbps |
| client-mode | This indicates digital client modes of the signal that is mapped into, and transported by the carriers within this superchannel. | ethernet, ethernet-otn |
| baud-rate | The modulated symbol rate. | string |
| application | The optical transport application ID this mode is optimized for | string length 1...15 |
| compatibility-id | Identifies the compatible carrier modes that can be applied simultaneously. | value |
| status | The state of the carrier mode. | string |
| supported-subtypes | Subtypes that each carrier mode supports | string (length 0..32) Maximum 20 elements |

<!-- page 1217 -->
