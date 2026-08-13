---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.285. serial-console'
source_lines: 21261-21300
---

## 6.285. serial-console

#### Command Description

These commands are used to set or show the global configuration of all serial console ports in the system. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set serial-console [global-switch <value>] [global-timeout <value>]
show serial-console [global-switch] [global-timeout]
```

#### Command Usage Details

**Table 662: serial-console Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 663: serial-console Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| global-switch | Allow access by serial-console. Note: each console port can override this global behavior. | enabled, disabled | enabled | set, show |
| global-timeout | Serial console inactivity timeout. Can be set to zero to disable inactivity timer. | Number (minutes) | 60 | set, show |

#### Examples

This example shows how to set serial console attributes:

```
set serial-console global-switch enabled global-timeout 80
```

<!-- page 1069 -->
