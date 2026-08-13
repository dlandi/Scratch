---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.199. nw-xconnect'
source_lines: 15977-16056
---

## 6.199. nw-xconnect

#### Command Description

The commands described in this section are used to add, set or show the `nw-xconnect` attributes. Use the delete command to delete a `nw-xconnect`.

#### Command Syntax

```
add nw-xconnect-<name> endpoint1 <value> endpoint2 <value> [xcon-type <value>] [rate <value>]
delete nw-xconnect-<name>
set nw-xconnect-<name> [rate <value>]
show nw-xconnect-<name> [AID] [oper-state] [avail-state] [endpoint1] [endpoint2] [xcon-type] [rate]
```

#### Command Usage Details

**Table 484: nw-xconnect Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 485: nw-xconnect Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service out-of-service normal automatic manual supporting-faulted | n/a | show |
| endpoint1 | The first endpoint of a networking cross-connection. It is mandatory to set the parameter upon nw-xconnect creation. If xcon-type = L1-ETH-to-GCC0 or L1-ETH-TO-OSC, an instance-identifier to comm-eth MO of a chassis. If xcon-type = L1-GCC0-to-GCC0, an instance-identifier to a GCC0 comm-channel MO If xcon-type=L1-OSC-TO-OSC, and instance-identifier to a OSCX-5 comm-channel MO. | instance-identifier | n/a | add, show |
| endpoint2 | The second endpoint of a networking cross-connection. If xcon-type = L1-ETH-to-GCC0 or L1-GCC0-to-GCC0, an instance-identifier to a GCC0 comm-channel MO. If xcon-type = L1-ETH-to-OSC or L1-OSC-to-OSC, an instance-identifier to a OSC-5 comm-channel MO. | instance-identifier | n/a | add, show |
| xcon-type | The XCON type of this object:<br>• L1-ETH-TO-GCC0 - L1-ETH to GCC0 user channel cross-connection.<br>• L1-GCC0-TO-GCC0 - GCC0 to GCC0 user channel cross-connection.<br>• L1-ETH-TO-OSC - L1-ETH to OSC user channel cross-connection.<br>• L1-OSC-TO-OSC - L1 OSC to OSC user channel cross-connection. | • L1-ETH-TO-GCC0<br>• L1-GCC0-TO-GCC0<br>• L1-ETH-TO-OSC<br>• L1-OSC-TO-OSC | L1-GCC0-TO-GCC0 | add, show |
| rate | Maximum bandwidth rate of the user channel (in Mbps units). | uint8, range [1 .. 20] Mbps | 13 - For For L1-ETH-TO-GCC0 . 20 - For L1-ETH-TO-OSC and L1- OSC-TO-OSC. | add, set, show |

#### Examples

The following example shows how to set a comm-eth to mode L1:

```
set comm-eth-1-12-ETH4 mode L1
```

The following example shows how to create a ETH-GCC0 User Channel cross-connection:

```
add nw-xconnect-test xcon-type L1-ETH-TO-GCC0 endpoint1 comm-eth-1-12-ETH4 endpoint2 comm-channel-ch1
```

<!-- page 735 -->

The following example shows how to create a GCC0-GCC0 User Channel cross-connection:

```
add nw-xconnect-test xcon-type L1-GCC0-TO-GCC0 endpoint1 comm-channel-ch1 endpoint2 comm-channel-ch2
```

The following example shows how to create a ETH-OSC User Channel cross-connection:

```
add nw-xconnect-test1 xcon-type L1-ETH-TO-OSC endpoint1 comm-eth-1-16-ETH4 endpoint2 comm-channel-1-6-dwdm-line-OSCX5
```

The following example shows how to create a OSC-OSC User Channel cross-connection:

```
add nw-xconnect-OSCuserch1 xcon-type L1-OSC-TO-OSC endpoint1 comm-channel-2-5-dwdm-line1-OSCX5 endpoint2 comm-channel-2-5-dwdm-line2-OSCX5
```

The following example shows how to delete a User Channel cross-connection:

```
delete nw-xconnect-test
```

<!-- page 736 -->
