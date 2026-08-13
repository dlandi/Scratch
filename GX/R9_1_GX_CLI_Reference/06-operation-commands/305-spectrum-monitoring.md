---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.305. spectrum-monitoring'
source_lines: 23449-23497
---

## 6.305. spectrum-monitoring

#### Command Description

The command described in this section are used to show the `spectrum-monitoring` attributes.

#### Command Syntax

```
show spectrum-monitoring-<name>/<direction>/<center-frequency> [width] [lower-frequency] [upper-frequency] [target-actual-power] [power-actual]
[psd-actual]
```

#### Command Usage Details

**Table 710: spectrum-monitoring Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 711: spectrum-monitoring Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | show |
| direction | Ingress or Egress direction. Currently, only Egress is supported. | • ingress<br>• egress | egress | show |
| center-frequency | Band slice center frequency. Managed by the system. | frequency in MHz | n/a | show |
| width | Received NMC width. | value in MHz | 50000 | show |
| lower-frequency | Lower Frequency of a Media Channel. | value in MHz | n/a | show |
| upper-frequency | Upper Frequency of a Media Channel. | value in MHz | n/a | show |
| target-actual-power | Target actual power, as calculated by ATPS. The value is of relevance when target-power-setting = auto, otherwise it will be displayed the default value (when target-power-setting = manual). | n/a | -99dBm | show |
| power-actual | Currently received power (dBm). The value -99dBm means that:<br>• the power not yet measured (measurement is performed by the OCM at DGE2 card), or<br>• no power detected (or no appropriate fiber-connection). For the egress direction (the only currently supported):<br>• the power actual should be calibrated at DWDM Line port output, at same reference point where the target power is defined. | n/a | -99 | show |
| psd-actual | Currently calculated PSD. The Power Spectral Density does not depend on the spectra width. | value in nW/GHz (2 digit precision) | not-applicable | show |

<!-- page 1158 -->

#### Examples

The following example shows how to view all the spectrum monitoring parameters of a single frequency:

```
show spectrum-monitoring-1-7-dwdm-line2/egress/191968750
```

<!-- page 1159 -->
