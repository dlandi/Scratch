---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.203. ocm-channel'
source_lines: 16235-16268
---

## 6.203. ocm-channel

#### Command Description

The commands described in this section are used to set or show the `ocm-channel` attributes. It lists the detected carriers within the configured OXcon(s).

#### Command Syntax

```
show ocm-channel-<name>/<lower-frequency>/<upper-frequency> [opm-pwr] [connected]
```

#### Command Usage Details

**Table 492: ocm-channel Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 493: ocm-channel Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | show |
| lower-frequency | Lower frequency of the corresponding spectrum power (OPM-pwr point). | uint32 | n/a | show |
| upper-frequency | Upper frequency of the corresponding spectrum power (OPM-pwr point). | uint32 | n/a | show |
| opm-pwr | Optical Parameter Monitor - power (in dBm). | decimal64 range (-99.00..99.00) | -99 | show |
| connected | Yields 'true' if the channel is configured (involved in an oxcon). | • true<br>• false | false | show |

<!-- page 750 -->
