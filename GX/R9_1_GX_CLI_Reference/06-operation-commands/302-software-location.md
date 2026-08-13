---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.302. software-location'
source_lines: 23285-23322
---

## 6.302. software-location

#### Command Description

This command is used to retrieve information about the location of software.

#### Command Syntax

```
show software-location-<location-id>
```

#### Command Usage Details

**Table 704: software-location Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 705: software-location Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| location-id | Location of the equipment. | String (length 0..64) | n/a | show |

#### Examples

This example shows how to retrieve software location information from card in chassis 1 and slot 5:

```
show software-location-1-5
```

<!-- page 1149 -->
