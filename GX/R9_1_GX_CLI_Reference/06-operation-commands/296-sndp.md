---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.296. sndp'
source_lines: 22987-23025
---

## 6.296. sndp

#### Command Description

The commands described in this section are used to set or show the `sndp` attributes.

#### Command Syntax

```
set sndp [sndp-enabled <value>]
show sndp [sndp-enabled]
```

#### Command Usage Details

**Table 692: SNDP Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 693: SNDP Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| sndp-enabled | This is a switch to control the sndp feature. | true, false | true | set, show |

#### Examples

This example shows how to show the sndp interface neighbor:

```
show sndp sndp-enabled
```

<!-- page 1136 -->
