---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.133. inci-neighbor'
source_lines: 12361-12414
---

## 6.133. inci-neighbor

#### Command Description

These commands are used to add, edit or show an INCI which is Inter-NE Communication Interface neighbor. The delete command is used to delete an INCI neighbor. Inter-NE communication infrastructure (INCI) provides API based communication infrastructure for control plane communication across different network elements (For example, Transponder (such as CHM6 on 1830 GX G40) and Line systems (FLEXILS)). INCI uses gRPC server/client as the core mechanism for inter network element communication. When INCI service is enabled, the gRPC server is started and bound to the local IP address. Similarly, when an INCI neighbor instance is configured, gRPC client sessions are established. The communication between the application and the corresponding application interface on INCI can be established through gRPC or platform specific Intelligent Peripheral Controller (IPC) mechanism. Since, DCN over AUX based interconnection is used for control plane communication between 1830 GX and FlexILS, gRPC sessions must be secured using TLS based bi-directional authentication and encryption. Digital trigger capability is supported between the Transponder (such as CHM6 on 1830 GX G40 network element) and the line system (Flex ILS) through INCI. Digital trigger provides optical layer protection switching on the line system (FlexILS) side based on the carrier level faults from the transponder side in addition to optical faults. The protection switching time is \<50 milliseconds in a inter operation deployment between the Transponder and Line system.

**Note:** Digital Trigger fault packets are H-MAC authenticated.

Upon proper fiber connections and configuration on the FlexILS network element, the OPSM Fac and connected Flex ILS NE details are updated on CHM6 SCH AID. The following command is used to view the digital trigger registration details: `show digital-trigger-registration-<SCH AID>`

#### Command Syntax

```
add inci-neighbor-<neighbor-id> neighbor-address <value> configured-node-name <value> [alarm-report-control <allowed|inhibited>]
set inci-neighbor-<neighbor-id> [neighbor-address <value>] [configured-node-name <value>] [alarm-report-control <allowed|inhibited>]
show inci-neighbor-<neighbor-id> [neighbor-address] [neighbor-port] [connection-status] [configured-node-name] [discovered-node-name]
[discovered-node-id] [alarm-report-control] [oper-state]
delete inci-neighbor-<neighbor-id>
```

#### Command Usage Details

**Table 354: inci-neighbor Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 355: inci-neighbor Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| neighbor-id | Node-ID of provisioned neighbor. | AID | n/a | add, set, show, delete |
| neighbor-address | P address of the provisioned remote neighbor NE. | IP address | n/a | add, set, show |
| configured-node-name | "User provisioned name of remote NE. Used to compare against the discovered-node-name. | string length 0...128 | n/a | add, set, show |
| alarm-report-control | Switch enabling alarm reporting. | allowed, inhibited | inhibited | add, set, show |
| discovered-node-name | Name of remote NE as sent by the remote NE. | string length 0...128 | n/a | show |
| discovered-node-id | Node ID of remote node as received from remote node. | string length 0...12 | n/a | show |
| oper-state | The operational state of this object. | enabled, disabled | n/a | show |

#### Examples

This example shows how to add an INCI neighbor:

<!-- page 553 -->

```
add inci-neighbor 1-L1-1 oper-state enabled
```

<!-- page 554 -->
