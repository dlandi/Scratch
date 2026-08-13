---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.104. external-fiber-connection'
source_lines: 10788-10841
---

## 6.104. external-fiber-connection

#### Command Description

These commands are used to add, set, show or delete an external fiber connection. The external fiber connection (**external-fiber-connection**) is the physical link representation of a connection between two ports of L0 cards in different NEs or in the same NE (in disaggregated configurations).

**Note:** The **external-fiber-connection** is set autonomously by TNMS. Although it is possible to configure it manually, it is not recommended to do so.

**Note:** Before R8.0, when a user provisions an external fiber connection, the user is also expected to set port **external-connectivity** to *yes*. Starting from R8.0, when **l0-mode-op** is set to *slte*, external fiber connections can be used to represent the intra-NE fiber connections between the RD20TM ADE ports or between CAD10A DWDM and RD20TM ADE ports.

**Note:** To facilitate CableID software to use the external fiber connection when building the CableID path topology, the external fiber connection entity supports the parameter **scope** with possible values *general-purpose* and *cable-id*. If the entry represents an intra-NE fiber connection between the RD20TM ADE ports or between CAD10A DWDM and RD20TM ADE ports, with or without OPSM in between, within the same NE, users must set the **scope** parameter to *cable-id*.

**Note:** The **scope** attribute is introduced in R8.0.1. After an upgrade from an earlier release to R8.0.1 or later, the **scope** attribute is defaulted to *general-* *purpose*. To enable the CableID function, users must delete the existing **external-fiber-connection** entries and add new ones with **scope** set to *cable-id*.

#### Command Syntax

```
add external-fiber-connection-<name> src-port-name <value> dst-port-name <value> [label <value>] [scope <value>] [src-node-id <value>]
[src-card-name <value>] [dst-node-id <value>] [dst-card-name <value>] [fiber-connection-type <value>]
set external-fiber-connection-<name> [label <value>]
show external-fiber-connection-<name> [label] [scope] [src-node-id] [src-card-name] [src-port-name] [dst-node-id] [dst-card-name] [dst-port-name]
[fiber-connection-type]
delete external-fiber-connection-<name>
```

#### Command Usage Details

**Table 295: external-fiber-connection Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | None |
| Post-condition | None |

#### Command Parameters

**Table 296: external-fiber-connection Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | User defined name for the external-fiber-connection. | String (length 0..64 characters) | n/a | add, set, show, delete |
| label | User defined label. | String (length: 0..256 characters) | n/a | add, set, show |
| scope | Represents the scope of the external-fiber-connection:<br>• general-purpose - indicates the general use of external-fiber-connection to represent connectivity between two ports on the same NE or across NEs.<br>• cable-id - indicates that the external-fiber-connection configuration is additionally used by cable-id functionality. i Note: If the scope is NOT set to cable-id, the CableID verification does not include this connection in the CableID path and no verification is performed on this connection. | • general-purpose<br>• cable-id | general-purpose | add, show |
| src-node-id | Source node-id. Should be logically the same as 'ne-name', although there is no SYSTEM business logic to correct this. | String (length: 0..256 characters) | n/a | add, set, show |
| src-card-name | Source card identification. | String (length 0..64 characters) | n/a | add, set, show |
| src-port-name | Source port identification. | String (length 0..128 characters) | n/a | add, set, show |
| dst-node-id | Destination node-id. Should be logically the same as 'ne-name', although there is no SYSTEM business logic to correct this. | String (length: 0..256 characters) | n/a | add, set, show |
| dst-card-name | Destination card identification. | String (length 0..64 characters) | n/a | add, set, show |
| dst-port-name | Destination port identification. | String (length 0..128 characters) | n/a | add, set, show |
| fiber-connection-type | Type of the fiber connection. It can be one-way (unidirectional) or two-way (bidirectional). | one-way, two-way | two-way | add, set, show |

<!-- page 480 -->
