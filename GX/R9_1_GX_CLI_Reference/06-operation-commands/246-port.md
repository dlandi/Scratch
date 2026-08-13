---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.246. port'
source_lines: 19077-19141
---

## 6.246. port

#### Command Description

These commands is used are set/show port attributes. Use the `set port ?` command to display a list of the configurable port object attributes. In 1830 GX G40, there are three types of ports: comm-eth, USB and trib/line. comm-eth and USB ports are on XMMs only, and are found on Slot 1 XMMs. The trib/ line ports can be seen on a CHM6 or a CHM7 (slots 4-7) only after a TOM is presentprovisioned in the CHM6/CHM7. Port objects are managed by the system and can not be manually deleted.

#### Command Syntax

```
set port-<card-name>-<port-name> [alias-name <value>] [admin-state <value>] [alarm-report-control <value>] [label <value>] [connected-to <value>]
[external-connectivity <value>] [diverse-routing <value>] [port-usage <value>]
show port-<card-name>-<port-name> [alias-name] [AID] [admin-state] [oper-state] [avail-state] [alarm-report-control] [label] [port-type]
[direction] [parent-port] [subport-list] [hosted-interface] [supported-type] [installed-type] [connected-to] [external-connectivity]
[diverse-routing] [port-usage]
```

#### Command Usage Details

**Table 584: port Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 585: port Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-name | The name of the object card. | String. | n/a | set, show |
| port-name | The name of the object port. | String. | n/a | set, show |
| alias-name | User-defined alias for this entity. Alphanumeric string with a dash or an underscore. Allowed characters A-Z, a-z, 0-9, _ \-/,\.]*'; | String (0..256 characters) | n/a | set, show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object: allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed inhibited | allowed | set, show |
| label | User defined label. | String (length: 0..256 characters) | n/a | set, show |
| port-type | The port type. Each port type supports different features and services. line - Refers to line-side 'colored' CWDM or DWDM optical module/transceiver. tributary - Refers to standard 'grey' interfaces/transceivers to interface with other client equipment. tributary-subport - Same as tributary, but for scenarios where the main tributary port is split into multiple subports. usb - USB port. comm - Communication ports. uplink - Refers to ports of an aggregation function that connect to an adjunct line function. optical - general optical port, with or without monitoring function. otdr - generic OTDR function, except for external OTDR measurement. ocm - OCM port pluggable - when port entity represents the L0 module cage for other pluggable "TOM" (QSFP/ SFP/ SFP+). | line tributary tributary-subport usb comm uplink optical otdr pluggable ocm | n/a | show |
| parent-port | Name of the parent port. Only applicable for sub-ports. | leafref (path "../../port/ name") | n/a | show |
| subport-list | List of sub-ports associated with this port. Only applicable when this port is a parent port. | leafref (path "../../port/ name") | n/a | show |
| direction | Direction of the port. Port direction is of relevance for the Topology discovery on Layer 0 cards. The attribute is only exposed when its value is other than not-applicable. By convention, ports of Layer 1 cards have direction not-applicable, and therefore this attribute is not exposed on those cases. | • not-applicable<br>• tx<br>• rx<br>• rxtx - Rx and Tx, layer 0 port | not-applicable | show |
| hosted-interface | Top level interface hosted in this port. | Instance identifier | n/a | show |
| supported-type | List of supported types in this equipment holder. If a specific type is provisioned, the list has only that type. | String (length: 0..32 characters) | n/a | show |
| installed-type | Currently installed type in this equipment holder. If empty, means no FRU is present. | String (length: 0..32 characters) | n/a | show |
| connected-to | i Note: The setting of this attribute is optional and, if using TNMS, it is not recommended to be set. Indicate neighbour port entity to which the current port is connected to. This is not validated by the NE and can be used by the customers (or NMS) for topology construction. This parameter is available independently on any automated discovery mechanisms that may exist in the port. For NMS, the format of the string is defined as '&lt;ne-name&gt;/&lt;port AID&gt;'; for example, if ne-name is set to GXG30 and the port AID is 1-1-ade1 the connected-to string is 'GXG30/1-1-ade1'. | String (length: 0..128 characters) | n/a | set, show |
| external-connectivity | Indicates whether the port is intended to be connected to another (external) NE. | no, yes | • yes, for an optical-ptp with ptp-type=dwdm-line<br>• no, for remaining cases. | set, show |
| 4 diverse-routing | Controls enabling/disabling of diverse routing capability. | true, false | false | set, show |
| port-usage | Port usage type. Only applicable for line-side ports. It's used to support the interoperation with Photonic Service Switch (PSS) for:<br>• CHM6<br>• CHM7<br>• CHM7X | • normal: Normal port usage - default behavior.<br>• pss-cluster: Port is part of PSS cluster - restricted access. | normal | set, show |

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 4 diverse-routing attribute is applicable to CHM7 and CHM7X currently.

<!-- page 965 -->

#### Examples

This example shows how to view all ports:

```
show port
```

<!-- page 966 -->
