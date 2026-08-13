---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.325. supported-port'
source_lines: 24776-24833
---

## 6.325. supported-port

#### Command Description

This command is used to display the capabilities for each port in each supported card.

#### Command Syntax

```
show supported-port-<card-type>/<port-name> [port-type] [direction] [configuration-mode] [faceplate-label] [leds] [present] [default-tom]
[parent-port] [subport-list] [allows-auto-migration]
```

#### Command Usage Details

**Table 750: supported-port Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 751: supported-port Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | Card type name (for example, CHM1R). | Card type (for example, CHM1R, FRCU31, IOPANEL) | n/a | show |
| port-name | The name of the port | string | n/a | show |
| port-type | The port type. Each port type supports different features and services. line - Refers to line-side 'colored' CWDM or DWDM optical module/transceiver. tributary - Refers to standard 'grey' interfaces/transceivers to interface with other client equipment. tributary-subport - Same as tributary, but for scenarios where the main tributary port is split into multiple subports. usb - USB port. comm - Communication ports. uplink - Refers to ports of an aggregation function that connect to an adjunct line function. optical - general optical port, with or without monitoring function. otdr - generic OTDR function, except for external OTDR measurement. ocm - OCM port pluggable - when port entity represents the L0 module cage for other pluggable "TOM" (QSFP/ SFP/ SFP+). | line tributary tributary-subport usb comm uplink optical otdr pluggable ocm | n/a | show |
| direction | Direction of the port. Port direction is of relevance for the Topology discovery on Layer 0 cards. The attribute is only exposed when its value is other than not-applicable. By convention, ports of Layer 1 cards have direction not-applicable, and therefore this attribute is not exposed on those cases. | • not-applicable<br>• tx<br>• rx<br>• rxtx - Rx and Tx, layer 0 port | not-applicable | show |
| configuration-mode | Configuration mode for the cards in this slot (or toms in this port): • system-configured - system automatically configures the card in slot, and user cannot make changes.<br>• user-configured - user can provision or de-provision cards in this slot. | system-configured user-configured | disabled | show |
| faceplate-label | Label on the hardware faceplate. Identifies the port in the card faceplate. | String (length 1..36) | n/a | show |
| leds | List of LEDs available for each port of this card. The list can contain a maximum of 10 elements. | String (length 1..20) | n/a | show |
| present | Indicates in which conditions the port is used. Related with multi-chassis environment, where some ports only exist in the Node Controller. Possible values:<br>• always - This port is always present for this card type.<br>• in-node-controller-only - This port is only present if this card is instantiated in a node controller chassis. | • always<br>• in-node-controller-only | always | show |
| default-tom | Defines the TOM that exists in this port by default (if any). | String | none | show |
| parent-port | Name of the parent port. Only applicable for sub-ports. | leafref (path "../../port/ name") | n/a | show |
| subport-list | List of sub-ports associated with this port. Only applicable when this port is a parent port. | leafref (path "../../port/ name") | n/a | show |
| allows-auto-migration | Indicates if TOMs that are plugged on this port type are auto migrated according with the equipment-policies tom-auto-migration flag. | true, false | true | show |

<!-- page 1224 -->

#### Examples

This example shows how to list the capabilities of all ports:

```
show supported-port
```

This example shows how to view the capabilities of a specific port:

```
show supported-port-CHM1R/2
```

<!-- page 1225 -->
