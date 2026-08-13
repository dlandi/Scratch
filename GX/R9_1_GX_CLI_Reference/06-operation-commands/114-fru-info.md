---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.114. fru-info'
source_lines: 11348-11378
---

## 6.114. fru-info

#### Command Description

This command is used to display the packaged FRU information associated to a particular equipment-type.

#### Command Syntax

```
show fru-info-<manifest-file>/<equipment-type>
```

#### Command Usage Details

**Table 314: fru-info Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 315: fru-info Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| manifest-file | The manifest file | string ( length 0..256) | n/a |
| equipment-type | Type of the equipment. | string ( length 0..32 ) | n/a |

<!-- page 510 -->
