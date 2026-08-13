---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.335. sw-management'
source_lines: 25286-25413
---

## 6.335. sw-management

#### Command Description

This command is used to show information about software locations, activity and downloads.

#### Command Syntax

```
show sw-management [downloads|software-location-<shelf>-<slot> [software-load-active|software-load-inactive|software-load-installable>]
|software-load-active|software-load-inactive]
```

#### Command Usage Details

**Table 770: sw-management Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 771: sw-management Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| downloads | Show download history. |  | n/a |
| software-location&lt;shelf&gt;-&lt;slot&gt; | Show software loads by shelf and slot. | software-load-active, software-load-inactive, software-load-installable | n/a |
| software-load-active | Shows active software. |  | n/a |
| software-load-inactive | Shows inactive software |  | n/a |

#### Examples

This example shows how to view the downloads:

<!-- page 1244 -->

```
show sw-management downloads
```

The following example displays the output from on a 1830 GX G40 node:

```
 downloads
  manifest-G40-R5.0.0-F-2021.05.04_03_15-sim-1239.manifest
  manifest-G40-R5.0.0-F-2021.05.16_00_12-sim-1252.manifest
  manifest-G40-R5.0.0-F-2021.05.23_00_12-sim-1260.manifest
```

This example shows hoe to view active software:

```
show sw-management software-load-active
```

The following example displays the output from on a 1830 GX G40 node:

```
software-load-active
  sw-component-active/chm6-R5.0-F-20210516-1252.tar.gz
  sw-component-active/xmm4-R5.0-F-20210516-1252.tar.gz
  packaged-fw-active/CHM6/DCO-MCU-DSP-ICE
  packaged-fw-active/CHM6/DCO-MCU-DSP-P
  packaged-fw-active/CHM6/DCO-MCU-Secure
  packaged-fw-active/CHM6/DCO-OEC
  packaged-fw-active/CHM6/DCO-OEC-Bootloader
  packaged-fw-active/CHM6/DCO-OEC-FPGA
  packaged-fw-active/CHM6/DCO-SecProc
  packaged-fw-active/CHM6/Host-Bootloader
  packaged-fw-active/CHM6/Host-FPGA
  packaged-fw-active/CHM6/Host-MCU-Secure
  packaged-fw-active/XMM4/CORE_BOOT
  packaged-fw-active/XMM4/FANCTRL
  packaged-fw-active/XMM4/FANCTRL_2
  packaged-fw-active/XMM4/FCP_FPGA
  packaged-fw-active/XMM4/IOP
  packaged-fw-active/XMM4/PEM_AC
  packaged-fw-active/XMM4/PEM_DC
  packaged-fw-active/XMM4/SecMCU_BG
  packaged-fw-active/XMM4/SecMCU_MG
  swload-version             'R5.0.0'
  swload-information         ''
  swload-vendor              'Infinera'
  swload-product             'G40'
  swload-label               'G40-R5.0.0-F-2021.05.16_00_12-sim-1252'
  swload-delta-label         ''
```

The following example shows how to view inactive software:

```
show sw-management software-load-inactive
```

The following example displays the output from on a 1830 GX G40 node:

```
software-load-inactive
  swload-version                      'R5.0.0'
  swload-information                  ''
  swload-vendor                       'Infinera'
  swload-product                      'G40'
  swload-label                        'G40-R5.0.0-F-2021.05.04_03_15-sim-1239'
```

This example shows how to view the active software in a specified shelf-slot location:

```
show sw-management software-location-1-4 software-load-active
```

The following example displays the output from on a 1830 GX G40 node:

```
software-load-1-4/active
  sw-component-1-4/active/chm6_base_pkg
  swload-version                                     'R5.0.0'
  swload-information                                 ''
  swload-vendor                                      'Infinera'
  swload-product                                     'G40'
  swload-label                                       'G40-R5.0.0-F-2021.05.16_00_12-sim-1252'
  swload-delta-label                                 ''
```

<!-- page 1246 -->
