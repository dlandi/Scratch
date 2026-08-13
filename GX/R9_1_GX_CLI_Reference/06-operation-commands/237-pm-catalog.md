---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.237. pm-catalog'
source_lines: 18677-18722
---

## 6.237. pm-catalog

#### Command Description

This command is used to show the contents of PM catalog.

#### Command Syntax

```
show pm-catalog <string>
```

#### Command Usage Details

**Table 566: pm-catalog Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 567: pm-catalog Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| string | The catalog name. | string | n/a | show |

#### Examples

This example shows how to view the contents of the pm catalog parameter undersized:

```
show pm-catalog parameter-undersized
```

The following output is displayed:

```
pm-parameter-undersized
  units                                packets
  type                                 counter
```

<!-- page 943 -->
