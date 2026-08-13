---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.53. comm-channel'
source_lines: 7286-7384
---

## 6.53. comm-channel

#### Command Description

These commands are used to add, set or show communications channel attributes. The delete command is used to remove a communications channel from the configuration. The comm-channel is a re-usable grouping that formulates the basic comm channel facility structure.

#### Command Syntax

```
add -m comm-channel-<name> type <value> parent <value> [label <value>] [admin-state <value>] [alarm-report-control <value>] [mtu <value>] [mode
<value>]
```

**Note:** The add command for comm-channel works in merge mode only. Using the -m flag performs a merge, which is the best effort add. If the target entity does not exist, it is created. If it exists, it is updated with any attributes present on the "add" command.

```
delete comm-channel-<name>
set comm-channel-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [type <value>] [mtu <value>] [mode <value>]
show comm-channel-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [type] [bandwidth] [operational-bandwidth] [mtu] [parent] [fcs-length] [mru]
[restart-timer] [max-failure] [peer-address] [mode]
```

#### Command Usage Details

**Table 181: comm-channel Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 321 -->

#### Command Parameters

**Table 182: comm-channel Command Flags**

| Parameter | Description |
| --- | --- |
| -m | Merge configuration (the command will not fail if the entity already exists) |

**Table 183: comm-channel Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the channel. | String (length 1..64) | n/a | add, set, show, delete |
| supporting-card | Card that holds this facility. | String | n/a | show |
| supporting-port | Ports that hold this facility. | String | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance-identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance-identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User-defined label for the card. | String (length 0..256) | n/a | add, set, show |
| admin-state | The administrative state of the channel. | lock, unlock, maintenance | unlock | add, set, show |
| oper-state | The operational state of the channel. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user created facilities can be user deleted. | system, user | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object.<br>• allowed: Alarm reporting is allowed.<br>• inhibited: Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| type | Indicates the type of control channel. OFEC-CC: Control channel available due to Nokia's propriety optical FEC overhead. GCC0: GCC0 overhead of OTUk. GCC1: GCC1 bytes within ODUk overhead. OSCX1: OSCX1 bytes. (1830 GX G30 only) OSCX2: OSCX2 bytes. (1830 GX G30 only) OSCX3: OSCX3 connectivity. (1830 GX G30 only) OSCX4: OSCX4 connectivity. (1830 GX G30 only) OSCX5: OSCX5 for L1 Aux user-channel. (1830 GX G30 only). FCC1: FCC overhead for L3 communication channel on CHM7. 1GE-OSCX1: 1GE-OSCX1 connectivity. (1830 GX G30 only) 1GE-OSCX2: 1GE-OSCX2 connectivity. (1830 GX G30 only) iGCC: iGCC communication chanel channel (1830 GX G31 only) | • OFEC-CC<br>• GCC0<br>• GCC1<br>• OSCX1<br>• OSCX2<br>• OSCX3<br>• OSCX4<br>• OSCX5<br>• FCC1<br>• 1GE-OSCX1 • 1GE-OSCX2<br>• iGCC | n/a | add, set, show |
| mtu | The maximum transmission unit size in octets for the physical Ethernet port of comm channel. This parameter is available only when the mode is L3. | Integer, uint16, (range: 1280..1500 octets (1830 GX G30 only); 1280..9202 octets (1830 GX G40 only) | 1500 For 1830 GX G40 Interface Types:<br>• DCN: 1518<br>• AUX: 1518<br>• AUX2: 1518<br>• OFEC-CC: 1518<br>• Craft: 1518<br>• CHM6: 1518<br>• UCM4: 1518 | add, set, show |
| parent | Parent object of the comm-channel. Only of relevance when type is GCC0 or, GCC1 or iGCC. | Instance identifier | n/a | add, set, show |
| bandwidth | Indicates the channel's bandwidth/ capacity. This is system determined based on the underlying facilities that support this control channel. | value | n/a | show |
| operational-bandwidth | Indicates the control channel's operational bandwidth/capacity. i Note: Operational bandwidth is displayed for OSCX comm-channels. | value | n/a | show |
| fcs-length | Specifies whether the Frame Check Sequence (FCS) is a 16-bit or 32-bit value. | 16, 32 (bits) | 16 | show |
| mru | Specifies the MRU (Maximum- Receive-Unit) in the Information and Padding fields. This parameter is available only when the mode is L3. | Integer, uint16, (range: 64..1500 octets) | 1500 | show |
| restart-timer | Specifies the restart timer of the PPP protocol in seconds. This parameter is available only when the mode is L3. | Integer (range: 1..10 seconds) | 3 | show |
| max-failure | Specifies the maximum failure value of the PPP protocol. Max- Failure indicates the number of Configure-Nak packets sent without sending a Configure-Ack before assuming that configuration is not converging. Any further Configure-Nak packets for peer requested options are converted to Configure-Reject packets, and locally desired options are no longer appended. This parameter is available only when the mode is L3. | Integer, uint8, (range: 2..10) | 5 | show |
| peer-address | The IP address on the peer node. This parameter is available only when the mode is L3. | IPv4/IPv6 address. | 0.0.0.0 | show |
| mode | Indicates the mode of operation of control channel. The mode of operation is common for comm-eth and comm-channel objects. The values can be:<br>• L1: L1 ETH User Channel Mode.<br>• L2: L2 iGCC interface.<br>• L3: L3 IP Mode (Default). | • L1<br>• L2<br>• L3 | L3 | set, show |

<!-- page 326 -->

#### Examples

The following example shows how to set the alarm-report-control to allowed in 1830 GX G40 environment:

```
set comm-channel-1-5-L1-1 alarm-report-control allowed
```

The following example shows how to add the GCC0 communication channel in 1830 GX G30 environment:

```
add comm-channel-gcc0 type GCC0 label GCC0 alarm-report-control allowed mtu 1500 parent otu-1-6-1-OTUC4
```

The following example shows how to add the GCC1 communication channel in 1830 GX G30 environment:

```
add comm-channel-141 type GCC1 parent odu-1-4-1-ODUC4
```

The following example shows how to add the iGCC communication channel as L2 mode in 1830 GX G31 environment:

```
add -m comm-channel-18531 type iGCC parent optical-carrier-185-3-1 mode L2
```

<!-- page 327 -->
