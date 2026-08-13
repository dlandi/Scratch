---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.125. icdp'
source_lines: 11901-11939
---

## 6.125. icdp

#### Command Description

These commands are used to set or show Nokia Carrier Discovery Protocol. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set icdp global-switch <true|false>
show icdp [global-switch]
```

#### Command Usage Details

**Table 337: icdp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 338: icdp Command Parameters**

| Parameter | Description | Values | Default | used in |
| --- | --- | --- | --- | --- |
| global-switch | Flag to enable icdp. | true, false | true | set, show |

#### Examples

This example shows how to enable icdp:

```
set icdp global-switch true
```

<!-- page 531 -->
