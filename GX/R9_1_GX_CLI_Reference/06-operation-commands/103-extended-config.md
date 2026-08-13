---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.103. extended-config'
source_lines: 10755-10787
---

## 6.103. extended-config

#### Command Description

The commands described in this section are used to add, delete or show the `extended-config` attributes. This command allows the user to configure a non-standard extended config. This introduces exceptional behavior globally in the system, and requires the knowledge of the extended-config name on the user side. The extended-config name must match a name known by the system.

#### Command Syntax

```
add extended-config-<name>
delete extended-config-<name>
show extended-config-<name> [description]
```

#### Command Usage Details

**Table 293: extended-config Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 294: extended-config Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| description | Displays the description of the extended-config provided by the system and its effect in the system. | String (length 0..255) | n/a | show |

<!-- page 477 -->
