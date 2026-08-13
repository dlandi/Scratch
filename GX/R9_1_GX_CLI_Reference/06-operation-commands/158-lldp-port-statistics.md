---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.158. lldp-port-statistics'
source_lines: 13819-13881
---

## 6.158. lldp-port-statistics

#### Command Description

This command is used to show LLDP frame reception statistics for a particular port and direction. All counter values in a particular entry shall be maintained on a continuing basis and shall not be deleted upon expiration of TTL timing counters associated with the LLDP neighbor information. All statistical counters associated with a particular port on the local LLDP agent become frozen whenever the **lldp-admin-status** is disabled for the same port.

**Tip:** The egress direction is not supported.

#### Command Syntax

```
show lldp-port-statistics-<lldp-port>/<direction> [last-change-time] [last-clear-time] [total-ageouts] [total-discarded-frames] [error-frames]
[total-frames-in] [total-frames-out] [total-discarded-tlvs] [total-unrecognized-tlvs]
```

#### Command Usage Details

**Table 404: lldp-port-statistics Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 405: lldp-port-statistics Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| lldp-port | Local port that is associated with the LLDP agent. | port name | n/a | show |
| direction | Direction associated with lldp statistics. | ingress | n/a | show |
| last-change-time | The timestamp associated with the last time this port received LLDP updates. | String | when lldp neighbor is formed | show |
| last-clear-time | The timestamp associated with the last time this port was cleared. | String | 0000-01-01T00:00:00Z | show |
| total-ageout | A count of the times that a neighbor’s information is deleted from the lldp-neighbor list due to TTL timer expiration. | String | 0 | show |
| total-discarded-frames | A count of all LLDPDUs received and then discarded. | Number | 0 | show |
| error-frames | A count of all LLDPDUs received at the port with one or more detectable errors. | Number | 0 | show |
| total-frames-in | A count of all LLDP frames received at the port. | Number | 0 | show |
| total-frames-out | A count of all LLDP frames transmitted through the port. | Number | 0 | show |
| total-discarded-tlvs | A count of all TLVs received at the port and discarded for any reason. | Number | 0 | show |
| total-unrecognized-tlvs | This counter provides a count of all TLVs not recognized by the receiving LLDP local agent. | Number | 0 | show |

#### Examples

The following example shows the command to retrieve LLDP frame reception statistics on all the LLDP ports:

```
show lldp-port-statistics
```

The following example shows the command to retrieve LLDP frame reception statistics on the CHM1R Ethernet port 1-1-3:

```
show lldp-port-statistics-ethernet-1-1-3/ingress
```

The following example shows the command to retrieve LLDP frame reception statistics on the DCN port, in 1830 GX G30 environment:

```
show lldp-port-statistics-comm-eth-1-5-ETH1/ingress
```

<!-- page 622 -->
