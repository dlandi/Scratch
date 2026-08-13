---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.220. ospf-interface'
source_lines: 17382-17443
---

## 6.220. ospf-interface

#### Command Description

These commands are used to add, set, show or delete an OSPF interface.

#### Command Syntax

```
add ospf-interface-<instance-id>/<ospf-area-id>/<ospf-if-name> [ospf-if-routing <value>] [enable <value>] [hello-interval <value>]
[router-dead-interval <value>] [retransmission-interval <value>] [transmit-delay <value>] [ospf-cost <value>] [priority <value>]
[ospf-auth-enable <value>] [ospf-auth-algorithm <value>] [ospf-auth-key <value>]
set ospf-interface-<instance-id>/<ospf-area-id>/<ospf-if-name> [ospf-if-routing <value>] [enable <value>] [hello-interval <value>]
[router-dead-interval <value>] [retransmission-interval <value>] [transmit-delay <value>] [ospf-cost <value>] [priority <value>]
[ospf-auth-enable <value>] [ospf-auth-algorithm <value>] [ospf-auth-key <value>]
show ospf-interface-<instance-id>/<ospf-area-id>/<ospf-if-name> [ospf-if-routing] [enable] [hello-interval] [router-dead-interval]
[retransmission-interval] [transmit-delay] [ospf-cost] [ospf-network-type] [priority] [ospf-auth-enable] [ospf-auth-algorithm] [ospf-auth-key]
delete ospf-interface-<instance-id>/<ospf-area-id>/<ospf-if-name>
```

#### Command Usage Details

**Table 527: ospf-interface Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | The OSPF instance and OSPF area must to be present to add an OSPF interface. |

#### Command Parameters

**Table 528: ospf-interface Command Parameters**

| Parameter | Description |  | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | OSPF instance ID. | uint8 (range: 0 .. 255) | n/a | add, set, show, delete |
| ospf-area-id | OSPF Router Area ID. | dotted-quad | n/a | add, set, show, delete |
| ospf-if-name | Reference of the interface in an OSPF area. | leafref (path "../../../../../interface/if-name") | n/a | add, set, show, delete |
| ospf-if-routing | Specifies if Routing is enabled and if so, if Routing is passive or active.<br>• active: This link is advertised and routing messages are transported over this link.<br>• passive: This link is advertised, routing messages are not transported over this link.<br>• auto: ospf-if-routing will be automatically derived from the interface type. | active passive auto | auto | add, set, show |
| enable | Enable/disable OSPF protocol on the interface. | true, false | true | add, set, show |
| hello-interval | Specifies the Hello Interval in seconds. | uint16 (range 1..32767 seconds) | 10 | add, set, show |
| router-dead-interval | Specifies the Router Dead Interval in seconds. | uint16 (range: 4..65535 seconds) | 40 | add, set, show |
| retransmission-interval | Specifies the Retransmission Interval in seconds. | uint16 (range: 2..3600 seconds) | 5 | add, set, show |
| transmit-delay | Estimated time needed to transmit Link State Update (LSU) packets on the interface (seconds). LSAs have their age incremented by this amount when advertised on the interface. A sample value would be 1 second. | uint16 (range: 1..450 seconds) | 1 | add, set, show |
| ospf-cost | OSPF link cost. | uint32 (range: 1..65535) | 10 | add, set, show |
| ospf-network-type | OSPF Interface Network Types. | broadcast, point-to-point | broadcast | show |
| priority | Configure OSPF router priority. On multi-access network this value is for Designated Router (DR) election. The priority is ignored on other interface types. A router with a higher priority will be preferred in the election and a value of 0 indicates the router is not eligible to become Designated Router or Backup Designated Router (BDR). | uint8 (range: 0 .. 255) | 1 | add, set, show |
| ospf-auth-enable | Enable/Disable Authentication. Only of relevance for ospfv2 or ospfv3. | true, false | false | add, set, show |
| ospf-auth-algorithm | Cryptographic algorithm associated with key. Only of relevance for ospfv2. | none, HMAC SHA 256 _ _ | HMAC SHA 256 _ _ | add, set, show |
| ospf-auth-key | Authentication key string in ASCII format. Only of relevance for ospfv2. | String (length 0 .. 256) | n/a | add, set, show |

#### Examples

This example shows how to add an OSPF interface:

```
add -m ospf-interface-1/0.0.0.0/1-AUX-1 enable true hello-interval 6 router-dead-interval 18 retransmission-interval 2 transmit-delay 1 ospfcost
 140 ospf-if-routing active priority 255 ospf-auth-enable false ospfauth-algorithm HMAC_SHA_256 ospf-auth-key 1
```

<!-- page 849 -->
