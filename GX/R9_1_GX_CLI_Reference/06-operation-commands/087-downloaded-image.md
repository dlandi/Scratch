---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.87. downloaded-image'
source_lines: 9821-9860
---

## 6.87. downloaded-image

#### Command Description

This command is used to retrieve information about downloaded image files.

#### Command Syntax

```
show downloaded-image-<manifest-file>/<name> [signature]
```

#### Command Usage Details

**Table 259: downloaded-image Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 260: downloaded-image Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| manifest-file | Downloaded manifest file and it's information. | String (length 0..256) | n/a | show |
| name | The name of the downloaded software image name. | String (length 0..256) | n/a | show |
| signature | Downloaded software image file signature. | String (length 0..1024) | n/a | show |

#### Examples

This example shows all the downloaded software image files:

```
show downloaded-image
```

<!-- page 426 -->
