---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.373. xcon'
source_lines: 27722-27790
---

## 6.373. xcon

#### Command Description

These commands are used to add, edit or show Layer 1 digital services that are currently provisioned in the system. This includes pre-provisioned XCONs as well. The delete command is used to delete an XCON from the configuration.

#### Command Syntax

```
add xcon-<name> source <value> destination <value> [payload-type <value>] [direction <value>] [label <value>] [circuit-id-suffix <value>]
set xcon-<name> [label <value>] [circuit-id-suffix <value>]
show xcon-<name> [AID] [oper-state] [avail-state] [source] [destination] [payload-type] [direction] [label] [circuit-id-suffix] [managed-by]
[payload-treatment] [network-mapping] [type] [protection-type] [circuit-id] [from-adaptation] [to-adaptation] [used-resources]
delete xcon-<name>
```

#### Command Usage Details

**Table 853: xcon Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 854: xcon Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | A user configured name for the XCON. | String (length: 0..64) | n/a | add, set, show, delete |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| source | The source end-point between which the XCON needs to be created. | Instance ID | n/a | add, set, show |
| destination | The destination end-point between which the XCON needs to be created. | Instance ID | n/a | add, set, show |
| payload-type | Indicates a generic, high-level source (from) client payload type of the digital XCON.<br>• 100GBE A generic payload type for all 100GBASE-X Ethernet clients when provisioning a digital XCON.<br>• 400GBE A generic payload type for all 400GBASE-X Ethernet clients when provisioning a digital XCON.<br>• OTU4 A generic payload type for OTU4 Transport w/o FEC service<br>• 100G A generic payload type for ODU4 switching services<br>• ODU2 A generic payload type for ODU2 switching services<br>• ODU2e A generic payload type for ODU2e switching services<br>• 0GBE<br>• OC192<br>• STM64<br>• 10G<br>• empty - Not applicable for 2-step XCON approach | 100GBE 400GBE OTU4 100G ODU2 ODU2e empty | n/a | show |
| direction | Indicates whether the digital XCON is unidirectional (one-way) or bi-directional (two-way). | • two-way | two-way | add, set, show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| circuit-id-suffix | User-configured circuit ID suffix. | String (length 0..48) | n/a | add, set, show |
| src-time-slots | Time-slots allocated to the source lo-oduj in this xcon. Not applicable if source facility is not an ODU facility. Value can be:<br>• omitted/empty - in which case system will audst-allocate time-slots based on the src-instance-id, which becomes mandatory (this is only supported for non ODUflex scenarios.)<br>• starting time-slot - system automatically allocates the rest of the time-slots sequentially from this starting point; will fail if those time-slots are not available<br>• time-slot list - full list of time-slots, using a comma separated list, with 'x..y' representing ranges; the total number of time-slots need to match the associated payload-type (e.g. 80 time-slots for 100G payload, 320 time-slots for 400G payload, etc) | String (length: 0..255) | n/a | show |
| dst-time-slots | Time-slots allocated to the destination looduj in this xcon. Not applicable if destination facility is not an ODU facility. Value can be:<br>• omitted/empty - in which case system will audst-allocate time-slots based on the src-instance-id, which becomes mandatory (this is only supported for non ODUflex scenarios.)<br>• starting time-slot - system automatically allocates the rest of the time-slots sequentially from this starting point; will fail if those time-slots are not available • time-slot list - full list of time-slots, using a comma separated list, with 'x..y' representing ranges; the total number of time-slots need to match the associated payload-type (e.g. 80 time-slots for 100G payload, 320 time-slots for 400G payload, etc) | String (length: 0..255) | n/a | show |
| payload-treatment | The treatment that this payload will have. Will be automatically derived from the payload-type. transport - payload-treatment for ethernet ctp xcon. transport-without-fec - payload-treatment for client otu4 and line odu4 xcon when PT is OTU4. switching - payload-treatment for client odu4 and line odu4 xcon when PT is 100G. regen - payload-treatment for two line lo-odu4 xcon when PT is OTU4. regen-switching - payload-treatment for two line lo-odu4 xcon when PT is 100G. | transport, switching, transport-without-fec, regen, regen-switching. | n/a | show |
| network-mapping | Indicates the server layer protocol type that supports this XCON. | ODUCn ODUCni ODUCni-M ODU4 ODU4i ODUflexi ODUflex ODU0 ODU1 ODU2 ODU2e | n/a | show |
| type | Type of XCON | add, drop, add-drop, express | n/a | show |
| protection-type | Represents the protection type this XCON has. | • y-cable<br>• snc-n<br>• snc-i<br>• unprotected | unprotected | show |
| managed-by | Describes whether this xcon was system created or not. Only user created xcon can be user deleted. | system, user | user | show |
| from-adaptation | Indicate server layer adaptation at client side. | string | n/a | show |
| to-adaptation | Indicate server layer adaptation at line side. | string | n/a | show |
| used-resources | List of resources being used by this XCON besides the two main source/destination end-points. | string length 0..64 | n/a | show |

#### Examples

The following examples show how to add XCONs in a 1830 GX G30 node:

```
add xcon-22 source odu-1-2-4-ODUflex destination odu-1-2-2-ODUFlex-1
add xcon-11 source odu-1-1-3-ODU4 destination odu-1-1-1-ODU4-1
```

The following examples show how to add XCONs in a 1830 GX G40 node:

```
add xcon-1 source ethernet-1-4-T1 destination odu-1-4-T1-1-ODUni-1
add xcon-channa source ethernet-1-6-T16 destination odu-low4
```

<!-- page 1356 -->
