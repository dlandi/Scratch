---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.304. spectrum-control'
source_lines: 23388-23448
---

## 6.304. spectrum-control

#### Command Description

The commands described in this section are used to set or show the `spectrum-control` object attributes.

#### Command Syntax

```
add spectrum-control-<name>/<direction>/<center-frequency> [attenuation-target <value>] [target-output-power <value>]
delete spectrum-control-<name>/<direction>/<center-frequency>
set spectrum-control-<name>/<direction>/<center-frequency> [attenuation-target <value>] [target-output-power <value>]
show spectrum-control-<name>/<direction>/<center-frequency> [width] [attenuation-actual] [attenuation-target] [target-output-power]
```

#### Command Usage Details

**Table 708: spectrum-control Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 709: spectrum-control Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| direction | Ingress or Egress direction. Currently, only Egress is supported. | • ingress<br>• egress | egress | add, delete, set, show |
| center-frequency | Band slice center frequency. Managed by the system. | frequency in MHz | n/a | add, delete, set, show |
| width | Detected width from spectrum-monitoring in MHz. The system determines whether the configured center-frequency matches any of the spectrum. If it does, it returns the width, otherwise returns 0. If no ILAx card or no DGE is fiber-connected, the system also returns 0. | value in MHz | 0 | show |
| attenuation-actual | Actual calculated attenuation for the spectrum. Only readable when Dynamic Gain Equalizer (DGE, (dge-in-use = 'true')), or equivalent, is in use. | value in dB | 0 | show |
| attenuation-target | Required attenuation for the spectra, defined by the user. Only possible to configure when Dynamic Gain Equalizer (DGE, (dge-in-use = 'true')), or equivalent, is in use. | range [0.. 30] dB | 0 | set, show |
| target-output-power | The intended target output power for the spectra. | not-specified or in the range [-55 .. 55] dBm | not-specified | set, show |

#### Examples

The following example shows how to view the spectrum control parameters for all spectrum frequencies:

```
show spectrum-control
```

<!-- page 1155 -->

The following example shows how to view the spectrum control parameters of a single frequency:

```
show spectrum-control-1-7-dwdm-line2/egress/194943750
```

The following example shows how to set the attenuation-target on a specific spectrum frequency:

```
set spectrum-control-1-7-dwdm-line2/egress/194943750 attenuation-target 1
```

<!-- page 1156 -->
