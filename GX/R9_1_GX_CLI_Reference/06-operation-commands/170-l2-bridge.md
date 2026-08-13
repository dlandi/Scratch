---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.170. L2-bridge'
source_lines: 14528-14560
---

## 6.170. L2-bridge

#### Command Description

The commands described in this section are used to set or show the `L2-bridge` attributes.

#### Command Syntax

```
set L2-bridge-<bridge-name> [chassis-name <value>] [description <value>]
show L2-bridge-<bridge-name> [chassis-name] [description]
```

#### Command Usage Details

**Table 429: L2-bridge Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 430: L2-bridge Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| bridge-name | The name of the bridge. | String (length 0..64) | n/a | set, show |
| chassis-name | Associated chassis name to this L2 bridge. | leafref | n/a | set, show |
| description | Description of the bridge and its intended purpose. | String (length 0..255) | n/a | set, show |

<!-- page 657 -->
