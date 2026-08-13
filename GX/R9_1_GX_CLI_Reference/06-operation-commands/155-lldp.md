---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.155. lldp'
source_lines: 13654-13698
---

## 6.155. lldp

#### Command Description

These commands are used to set or show the LLDP hold on timer. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set lldp [hold-on-timer <value>]
show lldp [hold-on-timer]
```

#### Command Usage Details

**Table 398: lldp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 399: lldp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| hold-on-timer | Time to keep neighbor information, in case neighbor does not have an explicit Time-To-Live (TTL) TLV. | Number (seconds) | 900 | set, show |

#### Examples

This example shows how to view the global LLDP configuration:

```
show lldp
```

This example shows how to set the lldp hold-on-timer:

```
set lldp hold-on-timer 60
```

<!-- page 613 -->
