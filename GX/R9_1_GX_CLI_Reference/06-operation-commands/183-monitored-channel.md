---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.183. monitored-channel'
source_lines: 15153-15199
---

## 6.183. monitored-channel

#### Command Description

The command described in this section is used to show the **monitored-channel** attributes.

#### Command Syntax

```
show monitored-channel-<name>/<frequency> [monitored-optical-power] [monitored-width]
```

#### Command Usage Details

**Table 455: monitored-channel Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 456: monitored-channel Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Corresponds to the (WS04S) card name with the suffix -1 or -2, for ocm1-in or ocm2-in (respectively). | string | n/a | show |
| frequency | Nominal Center Frequency (MHz) of the carrier (channel). | uint32 | n/a | show |
| monitored-optical-power | Measured power for the corresponding carrier (channel) in dBm. The value -99.00 means no power. | decimal 64 range (-99.00..99.00) | -99 | show |
| monitored-width | Carrier (channel) width configured at the NMC within the oxcon source/ destination, in MHz. | uint32 | 0 | show |

#### Examples

The following example shows how to view all the monitored channel attributes/parameters of one channel:

```
show monitored-channel-1-1.2-ocm1-in/195987500
```

The following example shows how to view the monitored optical power of one channel:

```
show monitored-channel-1-1.2-ocm1-in/195987500 monitored-optical-power
```

<!-- page 686 -->
