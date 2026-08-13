---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.59. connection-ports'
source_lines: 7680-7748
---

## 6.59. connection-ports

#### Command Description

This command is used to show connection ports.

#### Command Syntax

```
show connection-ports-<degree-number>/<index> [port-name]
```

#### Command Usage Details

**Table 195: connection-ports Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 196: connection-ports Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| degree-number | The degree number should be greater than zero and not greater than max-degrees. | uint16 (range 1..20) | n/a | show |
| index | Always 1 in 1830 GX (since dwdm-line ports are bi-directional). | uint32 (range 1..2) | n/a | show |
| port-name | The dwdm-line port of RD or ILAx card. As a consequence, OMS of the corresponding dwdm-port is created. | instance-identifier | n/a | show |

<!-- page 340 -->

#### Examples

The following example shows how to view all the connection ports in the node:

```
show connection-ports*
```

Example of an output retrieved from the system:

```
connection-ports       port-name
---------------------  -------------------
connection-ports-1/1   port-1-3-dwdm-line1
connection-ports-2/1   port-1-5-dwdm-line1
connection-ports-3/1   port-3-3-dwdm-line1
connection-ports-4/1   port-4-3-dwdm-line1
connection-ports-5/1   port-4-1-dwdm-line1
connection-ports-6/1   port-5-3-dwdm-line1
connection-ports-36/1  port-5-1-dwdm-line1
```

The following example shows how to view connection ports on the card equipped in chassis 5, slot 1:

```
show connection-ports-5/1
```

Example of an output retrieved from the system:

```
connection-ports-5/1
  port-name                         port-4-1-dwdm-line1
```

<!-- page 341 -->
