---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 01-introduction
section: '1. Introduction'
source_lines: 1456-1825
---

# 1. Introduction

This chapter provides an introduction to the Command Line Interface (CLI) for management of 1830 GX-based network elements configurations and a brief description of YANG basic information.

## 1.1. Command Line Interface (CLI) Overview

The Command Line Interface (CLI) is a primary Management Interface available in Converged OS based devices (1830 GX devices). It consists on a text based interface which is intended to be human friendly, allowing for management of the entire device and associated use-cases, including configuration, monitoring and invoking of action commands. Using the CLI, a user is able to perform configuration, monitoring, maintenance, and troubleshooting of 1830 GX network elements. The Command Line Interface (CLI) on the network element supports all functions of the Nokia 1830 GX based network elements. The CLI provides a hierarchical command set which is based on run-time parsing of YANG Data Models. For more information about the YANG Data Model and YANG Model hierarchy, refer to Yang Data Model (p. 43), YANG Model Hierarchy and Object Representation (p. 43) and *1830 GX IOA YANG Reference* *Guide*. Despite being optimized for human usability, the CLI can also be used in automation environments. The CLI provides a common look-and-feel to all devices, with similar commands, keywords and behavior. The CLI provides a consistent language for the transmission and reception of network messages to/from the network element and OSS. These messages can be subdivided into two major categories: input message (or command) to a network element, output message (or response). The CLI syntax defines the grammatical rules used to formulate CLI commands, responses, and error messages.

## 1.2. Yang Data Model

CLI derives its command set from a well-structured YANG data model, so that commands can be inferred from it. This allows automation, as well as makes the entire user experience more consistent. The CLI parses the YANG model, and additionally supports custom YANG extensions that augment the functionality. YANG Model handling in defines objects (YANG containers or lists) and attributes (YANG leafs or leaf-lists), and considers that object instances are the minimum granularity of system data. This means that if an object exists, all its attributes will exist. This limits the type of operations that can be done from a CRUD (Create, Read, Update, Delete) point of view, Objects can be created or deleted; attributes can be set; both can be read. The CLI (as part of the Management Framework) will parse the YANG Model, discover all objects and attributes that exist, and make the related CRUD commands available. In addition to CRUD of data nodes.

## 1.3. 1830 GX Management Entity AIDs

Nokia nodes enable configuration of equipment and provisioning of services associated with the equipment. The management interfaces use entity identifiers to uniquely identify the managed entity, which could be an equipment or a facility. The following sections lists the 1830 GX entity AIDs.

### 1.3.1. YANG Model Hierarchy and Object Representation

<!-- page 44 -->

The YANG object representation depends on the YANG Model hierarchy. Figure 1: YANG Model Hierarchy Diagram illustrates the YANG Model hierarchy for 1830 GX. Object attributes are not represented in the diagram and not all the hierarchy levels are captured in the diagram. For the full YANG object representation, refer to *1830 GX IOA YANG Reference Guide*.

<!-- page 45 -->

**Figure 1: YANG Model Hierarchy Diagram**

![Figure from page 45](images/figure-p45-1.png)

<!-- page 46 -->

There are two representations defined for objects:

- short representation: consists in the name of the object, followed by a sequence of key values.
- long representation: consists in a sequence of all the object ancestors up to the target object, together with their keys.

Table 4: YANG object representation in CLI (p. 46) displays examples for CLI short and long representation of YANG objects.

**Table 4: YANG object representation in CLI**

| Equivalent XPath | CLI short representation | CLI long representation |
| --- | --- | --- |
| /ne | ne | ne |
| /ne/chassis[name="1"]/slot[name="1"] | slot-1-1 | ne chassis-1 slot-1 |
| /ne/system/security/user[user-name="john"] | user-john | ne system security user-john |

Multiple objects can be represented with wildcards, where key values are replaced by '\*'. For more information about wildcard usage, refer to CLI Wildcard support (p. 84).

### 1.3.2. 1830 GX G31 Managed Objects and Addressable Entities

This section provides information about the objects with specific validation or type range. Figure 2: 1830 GX G31 Chassis, components and interfaces (p. 46) illustrates the front and rear view of the 1830 GX G31 chassis and the components and interfaces.

**Figure 2: 1830 GX G31 Chassis, components and interfaces**

![Figure from page 46](images/figure-p46-1.png)

The Management Object IDs (MOIDs) of the equipment level managed objects/equipment holders/slots supported by 1830 GX G31 chassis are listed in Table 5: 1830 GX G31 Equipment AID Formats (p. 46).

**Table 5: 1830 GX G31 Equipment AID Formats**

| Managed Object (MO) | MOID | Valid Range |
| --- | --- | --- |
| CHASSIS | &lt;chassis&gt; | Chassis=1 .. Max chassis. |
| IOPANEL | &lt;chassis&gt;-11 | - |
| PEM | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=7,9; N is the slot number in which the module can be plugged in to. |
| FAN | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=6,8,10; N is the slot number in which the module can be plugged in to. |
| FRCU | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=5; N is the slot number in which the module can be plugged in to. |
| Dual-Slot sled | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=1,3; N is the slot number in which the module can be plugged in to. Note: a dual-slot sled will occupy slots 1 and 2 or slots 3 and 4. |
| Single-Slot sled (for example, CHM1R) | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=1,2,3,4; N is the slot number in which the module can be plugged in to. |
| PORT (comm-eth) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=5; N is the slot number in which the module can be plugged in to. PortName is the name of the Eth 1 port on FRCU . Note: by default, is used for DCN. |
| PORT (USB) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=5; N is the slot number in which the module can be plugged in to. PortName is the name of the USB port on FRCU . |
| console | PortName-&lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=5; N is the slot number in which the module can be plugged in to. PortName is the name of the Console port on FRCU . |
| PORT (comm-eth) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=11; N is the slot number in which the module can be plugged in to. PortName is the name of the Eth2/3/4/5 port on IO panel. Note: by default, Eth2 and Eth3 ports are used for NCT, Eth4 port is used for AUX and Eth5 port is used for CRAFT (Local Craft Interface (LCI)). |

### 1.3.3. 1830 GX G32 Managed Objects and Addressable Entities

This section provides information about the objects with specific validation or type range. Figure 3: 1830 GX G32 Chassis, components and interfaces illustrates the front and rear view of the 1830 GX G32 chassis and the components and interfaces.

**Figure 3: 1830 GX G32 Chassis, components and interfaces**

![Figure from page 48](images/figure-p48-1.png)

The Management Object IDs (MOIDs) of the equipment level managed objects/equipment holders/slots supported by 1830 GX G32 chassis are listed in Table 6: 1830 GX G32 Equipment AID Formats (p. 48).

**Table 6: 1830 GX G32 Equipment AID Formats**

| Managed Object (MO) | MOID | Valid Range |
| --- | --- | --- |
| CHASSIS | &lt;chassis&gt; | Chassis=1 .. Max chassis. |
| IOPANEL | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=16, 19; N is the slot number in which the module can be plugged in to. In R6.0, the module is only supported on slot 16. If a module is plugged into slot 19, the possible-card-type is empty. |
| PEM | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=12, 14, 17, 18; N is the slot number in which the module can be plugged in to. |
| FAN | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=11, 13, 15; N is the slot number in which the module can be plugged in to. |
| FRCU | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=5, 10; N is the slot number in which the module can be plugged in to. If a module is plugged into slot 10, the possible-card-type is empty. |
| Dual-Slot sled (for example, UTM2, OCC2T) | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=1, 3, 6, 8; N is the slot number in which the module can be plugged in to. Note: a dual-slot sled will occupy slots 1 and 2, slots 3 and 4, 6 and 7, slots 8 and 9. |
| Single-Slot sled (for example, CHM1R, CAD10A) | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=1,2,3,4,6,7,8,9; N is the slot number in which the module can be plugged in to. |
| PORT (comm-eth) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=5/10; N is the slot number in which the module can be plugged in to. PortName is the name of the Eth 1 port on FRCU . Note: by default, is used for DCN. |
| PORT (USB) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=5/10; N is the slot number in which the module can be plugged in to. PortName is the name of the USB port on FRCU . |
| console | PortName-&lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=5/10; N is the slot number in which the module can be plugged in to. PortName is the name of the Console port on FRCU . |
| PORT (comm-eth) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=16/19; N is the slot number in which the module can be plugged in to. PortName is the name of the Eth 2/3/4/5 port on IO panel. Note: by default, Eth2 and Eth3 ports are used for NCT, Eth4 port is used for AUX and Eth5 port is used for CRAFT (Local Craft Interface (LCI)). |

### 1.3.4. 1830 GX G34c Managed Objects and Addressable Entities

This section provides information about the objects with specific validation or type range. Figure 4: 1830 GX G34c Chassis, components and interfaces (p. 50) illustrates the faceplate of the 1830 GX G34c chassis, and the components and interfaces.

**Figure 4: 1830 GX G34c Chassis, components and interfaces**

![Figure from page 50](images/figure-p50-1.png)

The Management Object IDs (MOIDs) of the equipment level managed objects/equipment holders/slots supported by 1830 GX G34c chassis are listed in Table 7: 1830 GX G34c Equipment AID Formats.

**Table 7: 1830 GX G34c Equipment AID Formats**

| Managed Object (MO) | MOID | Valid Range |
| --- | --- | --- |
| CHASSIS | &lt;chassis&gt; | Chassis=1 .. Max chassis. |
| PEM | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=9, 10; N is the slot number in which the module can be plugged in to. |
| FAN34c | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=11, 14; N is the slot number in which the module can be plugged in to. |
| FRCU | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=12, 13; N is the slot number in which the module can be plugged in to (12 is node-controller, when no redundancy is required). The controller redundancy functionality is to be supported in a future release. Although the controller redundancy functionality is to be supported in a future release, a 1830 GX G34c FRCU can be plugged into slot 13. As in R6.0 there is no possible-card-type for slot 13, the 1830 GX G34c FRCU in slot 13 will remain in dormant state. |
| Dual-Slot sled (for example, C2ILASGH (ILAx), OCC2E) | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N=1, 3, 5, 7; N is the slot number in which the module can be plugged in to. Note: a dual-slot sled will occupy slots 1 and 2, slots 3 and 4, 5 and 6, slots 7 and 8. |
| PORT (comm-eth) | &lt;chassis&gt;-N-PortName | Chassis=1 .. Max chassis; N=12, 13; N is the slot number in which the module can be plugged in to. PortName is the name for Ethernet ports. |

**Tip:** In R6.0, 1830 GX G34c does not support single-slot sleds.

### 1.3.5. 1830 GX G42 Managed Objects and Addressable Entities

Nokia nodes enable configuration of equipment and provisioning of services associated with the equipment. The management interfaces use entity identifiers to uniquely identify the managed entity, which could be an equipment or a facility.

**Note:** The maximum supported multi-chassis configuration is five chassis.

<!-- page 52 -->

The following tables lists the 1830 GX G42 entity AIDs.

**Table 8: 1830 GX G42 Equipment Entity ID Formats**

| Managed Object | ID | Format |
| --- | --- | --- |
| Chassis | &lt;chassis&gt; | Chassis=1 - 5 .. Max chassis |
| IOSHELF | &lt;chassis&gt;-IOPANEL-2 |  |
| PEM | &lt;chassis&gt;-PEM-N | Chassis=1 .. Max chassis; N1=1 ..4 |
| FAN | &lt;chassis&gt;-FAN-N | Chassis=1 .. Max chassis; N1=1 ..7 |
| FAN Controller | &lt;chassis&gt;-FANCTRL-1 | Chassis=1 .. Max chassis; |
| XMM4 | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N1=1,3 |
| UCM4 | &lt;chassis-id&gt;-&lt;slot-id&gt; | Chassis=1 .. Slots = T5, T6, T10, T11 |
| CHM6 | &lt;chassis&gt;-N | Chassis=1 .. Max chassis; N1=4,5,6,7 |
| DCN | &lt;chassis&gt;-N-DCN | Chassis=1 .. Max chassis; N1=1,3 |
| AUX | &lt;chassis&gt;-N-AUX-M | Chassis=1 .. Max chassis; N1=1,3; M=1,2 |
| CRAFT | &lt;chassis&gt;-N-CRAFT | Chassis=1 .. Max chassis; N1=1,3 |
| Config USB | &lt;chassis&gt;-N-U1 | Chassis=1 .. Max chassis; N1=1,3; |
| NCT | &lt;chassis&gt;-N-NCT-M | Chassis=1 .. Max chassis; N1=1,3; M=1,2 |
| TOM | &lt;chassis&gt;-&lt;slot&gt;-&lt;subslot&gt; | Valid 100G TOM Slots: &lt;chassis&gt;-4-&lt;slot&gt; slot = 1..16 Valid 400G TOM Slots: &lt;chassis&gt;-4-&lt;slot&gt; slot = 1, 8, 9, 16 |
| UCM4 TOMs (uplink: 1…4; client: 5…14) | &lt;port-id&gt; = T1…T14 | 1-4-T1, 1-5-T7 |

**Table 9: 1830 GX G42 Termination Point Entity ID Formats**

| Managed Termination Point Object | ID | Format |
| --- | --- | --- |
| TribPTP (Auto-created on TOM insertion and auto-deleted on TOM removal.) | 100GbE --&lt;chassis&gt;-4- T&lt;slot&gt; slot = 1..16 | 1-4-T1, 1-4-T4 |
|  | 400GbE--&lt;chassis&gt;-4- T&lt;slot&gt; slot = 1, 8, 9, 16 | 1-4-T1, 1-4-T8 |
| TribPTP (Client port with sub-port, i.e. 10G) | &lt;subport-id&gt; = 1…4 | 1-4-T5.1, 1-5-T7.4 |
| TribPTP (Uplink port with no multiplexing) | &lt;port-id&gt; = T1…T4 | 1-4-T1, 1-5-T4 |
| TribPTP (Uplink port ith multiplexed ODU2(e) into ODU4) | &lt;port-id&gt; = T1…T4 | 1-4-T1, 1-5-T4 |
| ClientCTP (Client port with sub-port i.e. 10GbE, STM-64, OC-192, OTU2(e)) | &lt;subport-id&gt; = 1…4 | 1-4-T5.1, 1-5-T7.4 |
| ClientCTP (Client port with sub-port (ODU2(e)) | &lt;port-id&gt; = T1…T4 | 1- |
| ClientCTP (Uplink port with no multiplexing (OTU4)) | &lt;port-id&gt; = T1…T4 | 1-4-T1, 1-5-T4 |
| ClientCTP (Uplink port with no multiplexing (ODU4)) | &lt;port-id&gt; = T1…T4 | 1-4-T1 , 1-5-T4 |
| ClientCTP (Uplink port with multiplexed ODU2(e) into ODU4 (OTU4, ODU4)) | &lt;port-id&gt; = T1…T4 | 1-4-T1, 1-5-T4 |
| ClientCTP (Uplink port with multiplexed ODU2(e) into ODU4 (ODU2(e)) | &lt;odu-type-id&gt; = ODU2(e)-1… ODU2(e)-10 | 1-4-T1-ODU2-1, 1-5-T4-ODU2E-8 |
| GigEClientCTP (Auto created on TOM insertion and auto-deleted on TOM removal.) | 100GbE--&lt;chassis&gt;-4- T&lt;slot&gt; slot = 1..16 | 1-4-T1, 1-4-T4 |
|  | 400GbE--&lt;chassis&gt;-4- T&lt;slot&gt; slot = 1, 8, 9, 16 | 1-4-T1, 1-4-T8 |
| Tributary Side ODU Client Termination Point ODUk Where k=4, 4i, AID of the associated TRIBPTP | &lt;chassis&gt;-T&lt;slot&gt;-&lt;port&gt; | chassis&gt; = {1-30} &lt;slot&gt;= {4..7} &lt;port&gt;= {1…16) |
| Tributary Side OTU Client Termination Point OTUk Where k= 4 or 4i AID of the associated TRIBPTP | &lt;chassis&gt;-&lt;shelf&gt;-T&lt;slot&gt; | &lt;chassis&gt; = {1-30} &lt;slot&gt;= {4..7} &lt;port&gt;= {1…16) |
| Line Side OTUCni CTP | &lt;chassis&gt;-&lt;SCG&gt;-&lt;SCH&gt; | &lt;chassis&gt; = {1-30} &lt;slot&gt; = {4…7} &lt;SCG&gt; = {L1 & L2} &lt;SCH&gt;={1..2} |
| Line Side ODUCniCTP (Higher Order) | &lt;chassis&gt;-&lt;SCG&gt;-&lt;SCH&gt; | &lt;chassis&gt; = {1-30} &lt;SCG&gt; = {L1 & L2} &lt;SCH&gt;={1..20} |
| Line Side ODUCTP (Lower Order) | &lt;HO L-ODUCni CTP&gt;- &lt;ODUK&gt;#n For example 1- L1-12-ODU4#1 | &lt;chassis&gt; = {1-30} &lt;slot&gt; = {4…7} &lt;SCG&gt; = {L1 & L2} &lt;SCH&gt;={1..2} |
| Super Channel Group | &lt;chassis&gt;-&lt;SCG&gt; | &lt;chassis&gt; = {1-30} &lt;slot&gt; = {4..7} &lt;SCG&gt; = {L1 & L2} |
| Super Channel | &lt;chassis&gt;-&lt;SCG&gt;-&lt;SCH&gt; | &lt;chassis&gt; = {1-30} &lt;slot&gt; = { 4 …7} &lt;SCG&gt; = {L1 & L2} &lt;SCH&gt; = {1…6} |
| Optical channel carrier CTP | &lt;chassis&gt;-&lt;SCG&gt;-&lt;carrier&gt; | &lt;chassis&gt; = {1-30} &lt;SCG&gt; = {L1} &lt;carrier&gt; = {1-10} for 1830 GX |
| GIGE Client CTP | &lt;chassis&gt;-&lt;SLOT&gt;-&lt;GIGE&gt; | &lt;chassis&gt; = {1-30} &lt;SLOT&gt; = {4-7} &lt;GIGE&gt; = {1-18} for 1830 GX |
| SCG PTP | &lt;chm6 AID&gt;-Ln where n= _ 1, 2 | 1-4-L1, 1-4-L2 |
| SCH CTP | &lt;SCG PTP&gt;-&lt;Instance Number&gt;, where instance number= 1 | For single carrier super-channel: 1-4-L1-1 (e.g., corresponding to 1-4-L1 SCGPTP) 1-4-L2-1 (e.g., corresponding to 1-4-L2 SCGPTP) i Note: 1-4-L1-1 corresponding to 1-4-L2 SCGPTP & 1-4-L2-1 corresponding to 1-4-L1 SCGPTP is invalid combination. For a dual carrier super-channel: 1-4-L1-1, 1-4-L2-1 (i.e., architecturally both options allowed) |
| Carrier CTP | &lt;SCG PTP AID&gt;-&lt;carrier&gt; _ with up to 1 instance | 1-4-L1-1, 1-4-L2-1 |
| OCH CTP, OTUCni CTP, HO-ODUCni CTP | Same as the SCH AID |  |

### 1.3.6. Managed Objects (MO) Relationship

This section provides information about the relationship between 1830 GX chassis MOs. Table 10: 1830 GX G31/1830 GX G32 Chassis MO relationship (p. 54) lists the 1830 GX G31/1830 GX G32 MOs relationship.

**Table 10: 1830 GX G31/1830 GX G32 Chassis MO relationship**

| Managed object type | Parent object | Contained object list | Supported object list | Supporting object list |
| --- | --- | --- | --- | --- |
| CHASSIS | NE | SLOT | - | - |
| SLOT | CHASSIS | - | CHM1R RD20TM RD09SM RD66TM UTM2 RPBM SPN2 CHM2TX CHM7X CAD10A CDC8D6 OCC2T PBAx IOPANEL FAN PEM FRCU | - |
| SLOT | CARD (OCC2T) | - | OTDR8OFP2 WS04SOFP2 CAD16AOFP2 BAXOFP2 | - |
| CARD (FRCU) | - | PORT (Eth1, USB) Console | - | SLOT (FRCU) |
| CARD (IOPANEL/ FAN/PEM) | - | - | - | SLOT (IOPANEL/FAN/ PEM) |
| CARD (Service SLED/ Blank) | - | PORT | - | - |
| PORT (Eth1 as DCN) | CARD (FRCU) | comm-eth | - | - |
| PORT (USB) | CARD (FRCU) | usb | - | - |
| PORT (Eth2/3/4/5 as NCT-1/NCT-2/AUX/ CRAFT) | CARD (IOPANEL) | comm-eth | - | - |
| AMPLIFIER AMPLIFIER-RAMAN | NE-FUNCTION | - | - | - |

Table 11: 1830 GX G34c Chassis MO relationship (p. 55) lists the 1830 GX G34c MOs relationship.

**Table 11: 1830 GX G34c Chassis MO relationship**

| Managed object type | Parent object | Contained object list | Supported object list | Supporting object list |
| --- | --- | --- | --- | --- |
| CHASSIS | EQUIPMENT | SLOT | - | - |
| SLOT | CHASSIS | Inventory | ILAx ILA2M OCC2E RPBL SPN2C FAN34c PEM FRCU | - |
| SLOT | CARD (OCC2E) | PORT, Inventory | DGE2M2OFP2 OTDR8OFP2 | - |
| CARD | EQUIPMENT | SLOT, Inventory | - | - |
| AMPLIFIER | NE-FUNCTION | - | - | - |

<!-- page 56 -->

Table 12: 1830 GX G31/1830 GX G32 chassis Field Replaceable Units (FRUs) provisioning behavior (p. 56) 1830 GX G31/1830 GX G32 chassis FRUs provisioning behavior.

**Table 12: 1830 GX G31/1830 GX G32 chassis Field Replaceable Units (FRUs) provisioning behavior**

| FRU Type | Auto-provisioning | Pre-provisioning | Notes |
| --- | --- | --- | --- |
| 1830 GX G31/1830 GX G32 Chassis | Yes | Yes | Only master chassis can be auto-provisioned. Shelf controller chassis requires pre-provisioning. |
| IOPANEL | Yes | No | Auto-created as part of chassis provisioning. |
| FAN | Yes | No | Auto-created as part of chassis provisioning. |
| 1830 GX G31 PEM | Yes | No | Auto-created as part of 1830 GX G31 chassis provisioning. |
| 1830 GX G32 PEM | Yes | Yes | Auto-created as part of 1830 GX G32 chassis provisioning. By default, on a fresh chassis bring-up, the system creates all 4 PEMs. |
| FRCU | Yes | No | For 1830 GX G32, FRCU can only be auto-provisioned on the top RU/row (slot 5). |
| Service SLED (e.g. CHM1R) | Yes | Yes | - |
| Active Blank | Yes | No | Auto created with plug-in of the blank/filler plate. |

Table 13: 1830 GX G34c chassis Field Replaceable Units (FRUs) provisioning behavior 1830 GX G34c chassis FRUs provisioning behavior.

**Table 13: 1830 GX G34c chassis Field Replaceable Units (FRUs) provisioning behavior**

| FRU Type | Auto-provisioning | Pre-provisioning | Notes |
| --- | --- | --- | --- |
| 1830 GX G34c Chassis | Yes | Yes | Only master chassis can be auto-provisioned. |
| FAN34c | Yes | No | Auto-created as part of chassis provisioning. |
| PEM | Yes | No | Auto-created as part of chassis provisioning. |
| FRCU | Yes | No | In R6.0, FRCU can only be auto-provisioned. |
| Service SLED | Yes | Yes | - |
| (Carrier card OCC2E) |  |  |  |
| Active Blank | Yes | No | Auto created with plug-in of the 2-slot blank/filler plate. |

## 1.4. Value Representation

The following table lists the YANG built-in data types that are used in CLI commands.

**Table 14: Built-in Data Types**

| Data Type | Input Representation | Output Representation | Example |
| --- | --- | --- | --- |
| int8, int16, int32, int64, uint8, uint16, uint32, uint64 | Integer number with the following rules:<br>• optional "+" sign for positive numbers<br>• mandatory "-" sign for negative numbers<br>• absent sign means positive number<br>• leading zeros are allowed, but ignored | Integer number with the following rules:<br>• positive integer does not include the sign "+"<br>• no leading zeros<br>• the value zero is represented as "0" | 123 -9 |
| decimal64 | Number with the same rules as int* data types, plus:<br>• optional period ('.') as a decimal indicator, followed by a sequence of decimal digits | Number with the same rules as int* data types, plus:<br>• mandatory decimal point ('.')<br>• at least one digit before and after the decimal indicator<br>• trailing zeros are added until the total number of fraction-digits is reaches<br>• cannot have more fraction digits that what the YANG describes<br>• the value zero is represented as "0.0" | 12.005 0.0000008 -774.0 |
| string | Refer to CLI String Support (p. 73) |  | 'zig' |
| boolean | true or false | true or false, no quotes | true |
| enumeration | String matching enum name | String matching enum name, no quotes | foo |
| bit | Space separate string with quotes | Space separate string enclosed with single quotes | 'one two three' |
| binary | Base64 encoding, as string | Base64 encoding, as string, enclosed with single quotes | "Zm9vIOKZpSBiY XI=" |
| leafref | Same rules as the target leaf |  |  |
| identityref | Identity name as a string, no module name | Identity name as a string, no module name | ethernetCsmacd |
| empty | Same as boolean |  | false |
| union | Same rules as actual type |  | n/a |
| instance-identifier | Same as object short representation |  | slot-1-1 |

## 1.5. CLI Command Modes

The CLI Interface is used to access the network elements. The CLI interface provides different modes to execute commands. The list of commands available is dependent on the mode you are currently in. When you login to the CLI interface, the Root mode is active. Table 15: Accessing and Exiting Command Modes (p. 58) lists the different modes and commands to enter and exit from the mode. Enter ? to display a list of commands available to execute in a mode.

**Table 15: Accessing and Exiting Command Modes**

| Command Mode | Access Method | Prompt | Exit Method |
| --- | --- | --- | --- |
| Operational | Log in | user@ne-name&gt; | To close the session, enter exit |
| Candidate Configuration | At the direct configuration prompt, enter configure | user@ne-name# | To close the session, enter exit |

### 1.5.1. Operational Mode

The CLI supports direct configurations with write-to-running behavior. This is the default operational mode that the CLI starts up in. In this mode, users can do all supported function and any configurations are written directly to the running datastore. Therefore there is no staging phase or a commit phase as in candidate configuration which is described below. This is the simplest operational mode, and is adequate for most cases where doing multiple commands atomically is not required. By default, the user enters CLI in the normal (write-to-running) mode. Starting with R8.1, the system supports a **writable-running** policy, that allows to disable writing to running; where writing for example is; making configuration changes, is only possible via Candidate Datastore.

### 1.5.2. Candidate Configuration Mode

The CLI supports the Candidate Configuration mode. This mode provides an alternative to writing directly into the running configuration. Candidate Configuration mode allows for multi-command configurations to be committed atomically.

**Note:** In the case where user operations performed in the CLI candidate config mode that impacts traffic, the user does not receive any confirmation message warning that the operation impacts traffic.

The user can enter candidate configuration mode by entering the `configure` command. **Candidate Configuration Starting Point**

<!-- page 59 -->
- If no parameter is provided, by default the starting point in candidate is a copy of the existing running configuration. This means the subsequent commands are handled as a delta from the running configuration.
- In CLI, the `configure` command will accept a `from-default` parameter, which is a blank configuration, that implies configuration from scratch. This command will only be allowed if Candidate Datastore is empty.
- In CLI, the `configure` command will accept a `from-script=<script>` parameter, imports configuration from a text file in the form of CLI commands. The `<file>` will follow the rules defined for CLI scripts that can be executed via any NBI.
- In CLI, the `configure` command will accept a `from-commit=<commit-id` parameter,configuration from a previous commit-record.

**Note:** Candidate configuration mode (e.g. entered using the "configure" command) is the preferred way to enter Security Policy Database entries. This is because multiple commands are requires to configure an entry and the default settings (e.g. Traffic Selectors) may not be appropriate for the entry being made. Candidate configuration mode allows all parameters to be specified prior to execution allowing the set to be evaluated as a whole and not each command individually.

**Candidate Configuration Types** There are two types of candidate configuration, shared or exclusive. In shared candidate configuration mode, multiple users/sessions can view and edit the Candidate configuration at the same time. In exclusive candidate configuration mode only the current user can edit the configuration. When in exclusive mode the system configuration is locked so that trying to enter this mode in another session fails. The default candidate configuration mode is exclusive.

**Note:** Entering exclusive mode while another session already has a shared candidate is not allowed. .

**Candidate Configuration Commands** The commands allowed in candidate configuration mode are as follows:

- All standard CRUD commands
- Show commands that display the candidate configuration current values; it is not possible (from the candidate configuration mode) to view running configuration values directly.
- Some standard RPCs that do not have configuration impact (for example: ping). Definition of which RPCs can be invoked in this mode will be defined at the YANG model level, and documented. Candidate configuration can be validated before applying it with the validate command; this mirrors the generic validate command capability, but applied to the candidate configuration.
- The RPC commands that perform configurations; in this case, the configurations would be done against the Candidate Datastore.

**Discarding a Candidate Configuration** Candidate configuration can be discarded with the discard-changes command. When this command is entered, all candidate configuration content is discarded and the CLI returns to normal mode, which has impact on the prompt as well.

<!-- page 60 -->

**Committing a Candidate Configuration** Candidate configuration can committed to become the running configuration by using the commit command. This behavior has an all-or-nothing result, which means no partial configurations are possible. Candidate commit should only affect impacted objects, which means that untouched configurations do not cause traffic impact. If the commit fails, the candidate content remains the same, allowing you to fix the problem before trying again After commit, the CLI returns to normal mode, also changing the prompt. In shared candidate configuration mode, sessions that are participating in the candidate editing are warned that another session made the commit, and will also return to normal mode. The **Confirmed Commit** feature allows a two-step commit process. Initially, changes are committed, and a second confirmation is required to finalize them. Without this confirmation, changes are automatically rolled back. **Exiting Candidate Configuration Mode** Exiting a session while editing a candidate configuration in exclusive mode, warn user that configuration will be discarded, and if user agrees, discards the configuration. For shared candidate configurations, the candidate is kept even after logout, so that other users/sessions can edit it.

#### 1.5.2.1. Associating Custom Message with Commit Command

Starting with R8.1, the `commit` command supports a `-m=<message>` parameter which allows user to provide a custom message to associate with the commit. This message will appear in logs and also in the Commit Repository `show commit` command.

## 1.6. Declarative Configuration through the CLI

Both imperative and declarative configuration environments are supported to manage the configurations in 1830 GX network elements. These two configurations environments can be used interchangeably. In an imperative configuration environment, the network element is configured using single commands via, for example, the CLI. In a declarative configuration environment, the network element's configuration is done via one designated management system acting as the configuration master and using a single script file containing the full configuration of the network element (for example, a CLI script). The declarative configuration can be applied to an network element in default configuration or to an network element fully provisioned. In a declarative configuration environment, the management system pushes the script file with the new configuration into the network element and the network element checks the new configuration against syntax errors. The new configuration is processed as a whole and accepted as long as the group of commands provide a valid configuration, that is, it is not only required that each command be valid on its own, but the entire resulting configuration must be valid. If the new configuration contains invalid settings (such as setting an unsupported port mode) the whole configuration is considered to be invalid and the declarative configuration request is canceled. In this case, the network element is not changed, partial configurations of a script are not applied to the network element. If the new configuration is valid, the declarative configuration request is accepted and the current configuration of the network element is overwritten with the new configuration (independently of the amount of differences between new and current configuration). Only the differences between the new and the current configurations will affect the network element, that is, if the new and the current configurations are equal, the

<!-- page 61 -->

NE is not affected by the declarative configuration request. All configurations are accepted as long as they are valid independently of the network element's capability to implement, for example, a valid configuration for pre-provisioning an absent equipment is accepted. All the configurations included in the new configuration are implicitly trusted and considered to be intended. The configurations not included in a new configuration are implicitly deleted.

**Note:** The user executing the declarative configuration must have the necessary access permissions for all the commands in the configuration.

### 1.6.1. Managing CLI Scripts for Declarative Configuration

The CLI scripts used in a declarative configuration are text files containing CLI commands. The following rules must be taken into consideration when creating/editing the CLI script for declarative configuration:

- Comments can be added to the CLI script. The comment lines start with the character #.
- CLI scripts can use `add` and `set` commands only. All other CLI commands cannot be used.
- The set command supports the usage of wildcard \*. For example, to enable PM for several objects the command `set pm-point-* supervision-enabled true` is used.
- All the configurations included in the CLI script are implicitly trusted.
- All the configurations not included in the CLI script are implicitly deleted by the NE.
- The CLI script must not contain duplicated commands, for example, the same card cannot be created twice.
- CLI commands that require confirmation are implicitly accepted in a declarative configuration. It is not necessary to use -f to force commands but its usage is accepted.
- Sensitive data, such as passwords, can be added in the CLI script using an encrypted format (CSP encryption). For more information about how to set CSP encryption, refer to the *1830 GX* *Management Interfaces User Guide*.

#### Using CLI Scripts for Declarative Configuration

The CLI scripts used in a declarative configuration are text files containing CLI commands. If a network element is in default configuration, create a new text file with all the CLI commands for configuring the network element. In case of changing an network element already provisioned it is recommended to retrieve all the network element's configuration first, create the text file and change the intended configurations. To use a CLI script for declarative configuration, the following steps must be performed:

1. Create or update the CLI script.

A new CLI script must be created for network elements in default configuration. For network elements already provisioned, the CLI script needs to be updated with the new/changed configurations. The CLI script must be saved with .cli extension.

**Note:** The CLI script can be retrieved from the network element by using the `show config` `| display commands` CLI command. After retrieving all CLI configuration from the network element, the CLI script can be manually tailored if needed.

2. Download the CLI script into the NE via SCP/SFTP.

<!-- page 62 -->

```
download script source=<sftp|scp|http|https]://[user@]hostname/directorypath/filename>
 password=<password>
```

3. Run the CLI script with the replace flag set.

**Note:** The user executing the declarative configuration script must have necessary access permissions to execute all the commands in the configuration.

```
run script -r <script name.cli>
```

<!-- page 63 -->
