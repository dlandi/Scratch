---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.176. manifest'
source_lines: 14797-14859
---

## 6.176. manifest

#### Command Description

These commands are used to show the downloaded manifest file and it's information.

#### Command Syntax

```
show manifest-<manifest-file> [manifest-signature] [downloaded-on] [information]
show manifest-component-<manifest-file>/<equipment-type>/<name> [state] [version] [description]
show manifest-firmware-<manifest-file>/<equipment-type>/<fw-name> [fw-version]
```

#### Command Usage Details

**Table 441: manifest Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 442: manifest Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| manifest-file | Downloaded manifest file and it's information. | String (length 0..256) | n/a | show |
| equipment-type | The packaged FRU information associated to a particular equipment-type. | String (length 0..32) | n/a | show |
| name | Package name. | String ( length 1..256) | n/a | show |
| fw-name | Name of the firmware. | String (length 0..32) | n/a | show |
| manifest-signature | Manifest file signature. | hex-string (length 0..1024) | n/a | show |
| downloaded-image | Downloaded image file. | file name | n/a | show |
| downloaded-on | Manifest file downloaded timestamp. | String (date-time in the format YYYY-MM- DDThh:mm:ssZ see the set-time (p. 1087) command for detailed information.) | n/a | show |
| information | Information on the manifest downloaded. | String (length 0..256) | n/a | show |
| state | Package state: installed - software package installed not-installed - software package not installed. installation-failed - software package install failed. unknown - software package state unknown. | installed not-installed installation-failed unknown | unknown | show |
| version | Package version. | string (length 0..64) | n/a | show |
| description | Package information. | string ( length 0..512) | n/a | show |
| fw-version | Included version of the firmware. | string (length 0..32) | n/a | show |

#### Examples

This example shows how to view the list of manifests:

```
show manifest
```

This example shows how to view the manifest parameters of one 1830 GX G30 manifest file:

```
show manifest-G30-R5.0.0-F-2021.11.04_20_04-46.manifest
```

This example shows how to view the manifest parameters of one 1830 GX G40 manifest file:

```
show manifest-G40_BASIC-R6.1.0-F-2023.01.04_15_40-x-sim-u-1940.manifest
```

<!-- page 669 -->
