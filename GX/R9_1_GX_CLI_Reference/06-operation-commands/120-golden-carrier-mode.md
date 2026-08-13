---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.120. golden-carrier-mode'
source_lines: 11677-11718
---

## 6.120. golden-carrier-mode

#### Command Description

This command is used to retrieve configuration information from the system. This command displays non-default configurations (configurations that have their default values are skipped). The displayed configuration is fully recursive from the current CLI scope, so doing this command at the top of the CLI hierarchy will provide the complete system configuration. Alternatively, an \<entity-id\> can be provided to limit the scope of the output. If all entities of a given type are relevant, it is possible to provide the \<entity-type\> instead.

#### Command Syntax

```
show golden-carrier-mode-<card-type>/<carrier-mode> [actual-carrier-mode] [capacity] [client-mode] [baud-rate] [application] [compatibility-id]
[status] [sop-tracking-mode] [supported-subtypes] [candidate-subtypes]
```

#### Command Usage Details

**Table 326: golden-carrier-mode Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 327: golden-carrier-mode Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| card-type | Card type name. | string | n/a |
| carrier-mode | Specifies the line mode of the optical carrier. The value is specified as a tuple which contains the line capacity, client mode, baud rate, application ID and SOP tracking mode. | string (1..15) The format is as follows: &lt;Capacity&gt;&lt;ClientMode&gt;.&lt;Baud Rate&gt;&lt;Application ID&gt; Examples: - 600E.84P - 100X.73U - 325M.66P"; | n/a |
| actual-carrier-mode | The actual carrier-mode. | string (length 0..15) | n/a |
| capacity | The net capacity of the optical carrier. | Gbps | n/a |
| client-mode | This indicates digital client modes of the signal that is mapped into, and transported by the carriers within this superchannel. | ethernet, ethernet-otn | n/a |
| baud-rate | The modulated symbol rate. |  | n/a |
| application | The optical transport application ID this mode is optimized for. | application id type | n/a |
| compatibility-id | Identifies the compatible carrier modes that can be applied simultaneously | unit16 | n/a |
| status | Describes carrier mode release status. | supported, candidate, experimental, deprecated, diagnostic | n/a |
| sop-tracking-mode | The optical transport SOP tracking mode this mode is optimized for. | n/a | n/a |
| supported-subtypes | Subtypes that each carrier mode supports. | string (length 0...32) maximum 20 element | n/a |
| candidate-subtypes | Subtypes for which this carrier mode has candidate status. | string (length 0..16) maximum 10 elements | n/a |

<!-- page 523 -->
