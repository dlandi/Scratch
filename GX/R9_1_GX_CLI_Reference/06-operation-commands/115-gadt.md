---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.115. gadt'
source_lines: 11379-11428
---

## 6.115. gadt

#### Command Description

This command is used to retrieve information about golden carrier application information .

#### Command Syntax

```
show gadt [<application-description-H|application-description-P|application-description-S|application-description-U>]
```

#### Command Usage Details

**Table 316: gadt Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 317: gadt Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| application-description-H | Detailed description of application ID | string | n/a |
| application-description-P | Detailed description of application ID | string | n/a |
| application-description-S | Detailed description of application ID | string | n/a |
| application-description-U | Detailed description of application ID | string | n/a |

#### Examples

This example shows how to display the application description for H:

```
show gadt application-description-H
```

The following output is displayed:

<!-- page 511 -->

```
  application-description-H
  application-description                'Subsea modes using Uniform and Hybrid modulation'
```

<!-- page 512 -->
