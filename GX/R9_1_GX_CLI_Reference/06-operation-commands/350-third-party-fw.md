---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.350. third-party-fw'
source_lines: 26280-26319
---

## 6.350. third-party-fw

#### Command Description

This command is used to show third-party firmware information.

#### Command Syntax

```
show third-party-fw-<fw-name> [file-status] [path] [version] [crc] [vendor] [part-number] [nsa-upgrade-version] [present-in-eqpt]
[applicable-eqpt]
```

#### Command Usage Details

**Table 806: third-party-fw Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 807: third-party-fw Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| fw-name | The name of the firmware | string | n/a | show |
| file-status | Firmware file status. | valid, invalid, missing | n/a | show |
| path | Path for the firmware image. | string | 0...255 | show |
| version | The vendor of the firmware. | string | 0...64 | show |
| crc | Cyclic redundancy check (CRC) of the firmware image, used to validate the file when present. | string | 0...64 | show |
| vendor | The vendor of the firmware. | string | 0...64 | show |
| part-number | The part-number of the firmware. | string | 0...64 | show |
| nsa-upgrade-version | Versions from where the upgrade is non service affecting (nsa). | string | 0...255 | show |
| present-in-eqpt | List of resources that contain this version. | max-elements 80 | n/a | show |
| applicable-eqpt | List of resources that this firmware can be applied to | max-elements 80 | n/a | show |

<!-- page 1282 -->
