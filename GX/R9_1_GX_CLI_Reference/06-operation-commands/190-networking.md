---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.190. networking'
source_lines: 15492-15536
---

## 6.190. networking

#### Command Description

These commands are used to show/set networking information.

#### Command Syntax

```
set networking [use-as-source <value>]
show networking [use-as-source]
```

#### Command Usage Details

**Table 467: networking Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 468: networking Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| use-as-source | Interface to use as source address. | string (length 0..64) | n/a | set, show |

#### Examples

This example shows how to view networking information:

```
show networking
```

This example shows how to set the interface to use as source address:

```
set networking use-as-source LO-MGMT
```

<!-- page 703 -->
