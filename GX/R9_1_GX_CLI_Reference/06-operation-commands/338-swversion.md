---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.338. swversion'
source_lines: 25519-25563
---

## 6.338. swversion

#### Command Description

This command is used to retrieve the active, inactive and/or installable versions of the software present on the network element.

#### Command Syntax

```
swversion
```

#### Command Usage Details

**Table 776: swversion Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration mode |

#### Command Parameters

None.

#### Examples

This example shows how to display active, inactive and/or installable versions of the software:

```
swversion
```

The following example displays the output of a 1830 GX G40 node:

```
software-load-active           R4.0.0          G40-R4.0.0-F-2021.06.03_08_22-sim-188
software-load-inactive         R4.0.0          G40-R4.0.0-F-2021.04.15_14_26-sim-112
software-load-installable      R4.0.0          G40-R4.0.0-F-2021.06.03_08_22-sim-188
software-load-1-1/active       R4.0.0          G40-R4.0.0-F-2021.06.03_08_22-sim-188
software-load-1-1/inactive     R4.0.0          G40-R4.0.0-F-2021.04.15_14_26-sim-112
software-load-1-1/installable  R4.0.0          G40-R4.0.0-F-2021.06.03_08_22-sim-188
```

<!-- page 1252 -->
