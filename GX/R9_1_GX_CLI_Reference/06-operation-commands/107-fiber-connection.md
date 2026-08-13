---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.107. fiber-connection'
source_lines: 10964-11010
---

## 6.107. fiber-connection

#### Command Description

These commands are used to add, set, show or delete a fiber-connection in an OADM/ILA topology. The fiber-connection (**fiber-connection**) is the physical link representation of a connection between two distinct ports (or two distinct sub-ports) in the same NE.

#### Command Syntax

```
add fiber-connection-<name> [label <value>] [src-port <value>] [dst-port <value>] [fiber-connection-type <value>]
set fiber-connection-<name> [label <value>] [src-port <value>] [dst-port <value>] [fiber-connection-type <value>]
show fiber-connection-<name> [label] [src-port] [dst-port] [fiber-connection-type]
delete fiber-connection-<name>
```

#### Command Usage Details

**Table 300: fiber-connection Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | None |
| Post-condition | None |

#### Command Parameters

**Table 301: fiber-connection Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | User defined name for the fiber-connection. | String (length 0..64 characters) | n/a | add, set, show, delete |
| label | User defined label. | String (length: 0..256 characters) | n/a | add, set, show |
| src-port | Source Port instance. | Instance identifier | n/a | add, set, show |
| dst-port | Destination Port instance. | Instance identifier | n/a | add, set, show |
| fiber-connection-type | Type of the fiber connection. It can be one-way (unidirectional) or two-way (bidirectional). | one-way, two-way | two-way | add, set, show |

#### Examples

This example This example shows how to add a super channel in 1830 GX G40 node. Super channel, Optical Channel, OTU, and Higher order ODU facilities are created at the time of super channel creation:

```
add fiber-connection-1-6-L2-1 carriers 1-6-L2-1 carrier-mode 800E.96P
```

<!-- page 491 -->
