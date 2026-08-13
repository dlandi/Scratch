---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.185. nct-connection'
source_lines: 15235-15283
---

## 6.185. nct-connection

#### Command Description

This command is used to show NCT connectivity information, providing existing links between NCT ports in a multi-chassis NE. These links are dynamically filled in by the system, allowing to derive and display the NCT topology.

#### Command Syntax

```
show nct-connection-<src-port>/<dst-port> [src-chassis] [dst-chassis] [src-chassis-state] [dst-chassis-state]>
```

#### Command Usage Details

**Table 459: nct-connection Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 460: nct-connection Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| src-port | The source port of the connection. Must be an NCT port. If the port belongs to a commissioned chassis, it will be the AID of the port. If the port belongs to an unprovisioned chassis, it will have the format '&lt;chassis-serial-number&gt;-&lt;slot&gt;-NCT-&lt;id&gt;' The source port of the connection. Must be an NCT port. | string (length 0..64) | n/a |
| dst-port | The destination port of the connection. Must be an NCT port. If the port belongs to a commissioned chassis, it will be the AID of the port. If the port belongs to an unprovisioned chassis, it will have the format '&lt;chassis-serial-number&gt;-&lt;slot&gt;-NCT-&lt;id&gt;' | string (length 0..64) | n/a |
| src-chassis | The identifier of the chassis where the source port is located. If it is a commissioned chassis, it will be the AID of the chassis. If it is an unprovisioned chassis, it will have the chassis serial number | string (length 0..64) | n/a |
| dst-chassis | The identifier of the chassis where the destination port is located. If it is a commissioned chassis, it will be the AID of the chassis. If it is an unprovisioned chassis, it will have the chassis serial number. | string (length 0..64) | n/a |
| src-chassis-state | The state of the src-chassis | node controller, provisioned, unprovisioned | n/a |
| dst-chassis-state | The state of the dst-chassis | node controller, provisioned, unprovisioned | n/a |

#### Examples

This example shows how to view the NE NCT connections:

```
show nct-connection
nct-connection                       src-chassis dst-chassis src-chassis-state dst-chassis-state
------------------------------------ ----------- ----------- ----------------- -----------------
nct-connection-1-1-NCT-1/254-1-NCT-2 1           254         node-controller   provisioned
nct-connection-1-1-NCT-2/254-1-NCT-1 1           254         node-controller   provisioned
nct-connection-1-3-NCT-1/254-3-NCT-2 1           254         node-controller   provisioned
nct-connection-1-3-NCT-2/254-3-NCT-1 1           254         node-controller   provisioned
```

<!-- page 690 -->
