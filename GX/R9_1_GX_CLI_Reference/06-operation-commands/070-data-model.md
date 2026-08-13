---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.70. data-model'
source_lines: 8356-8396
---

## 6.70. data-model

#### Command Description

These commands are used to enable or show the attributes of the available YANG Data models for loading/unloading.

#### Command Syntax

```
set data-model-<name> [enabled <value>]
show data-model-<name> [description] [enabled]
```

#### Command Usage Details

**Table 219: data-model Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 220: data-model Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Model name. | String (length 0..256) | n/a | set, show |
| description | Data model description. | String (length 0..256) | n/a | show |
| enabled | Allows to load/unload this data model. A loaded data model means that it can be used via the management interfaces. | true false | false | set, show |

#### Examples

The following example shows how to view all available YANG Data models for loading/unloading.

```
show data-model
```

<!-- page 368 -->
