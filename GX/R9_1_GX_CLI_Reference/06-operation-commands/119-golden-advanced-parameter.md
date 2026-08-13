---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.119. golden-advanced-parameter'
source_lines: 11605-11676
---

## 6.119. golden-advanced-parameter

#### Command Description

This command is used to show the `golden-advanced-parameter` attributes. The system supports a Golden Advanced Parameter Table (GAPT) to support advanced parameters pre-provisioning.

#### Command Syntax

```
show golden-advanced-parameter-<card-type>/<name> [description] [type] [supported-values] [direction] [multiplicity] [configuration-impact]
[service-impact]
```

#### Command Usage Details

**Table 324: golden-advanced-parameter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 325: golden-advanced-parameter Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-type | Card type name. | string | n/a | show |
| name | Name of the advanced parameter. This parameter is read-only. | string (length 0..256) | n/a | show |
| description | A human readable description of this advanced parameter. This parameter is read-only. | string (length 0..256) | n/a | show |
| type | Data type of the advanced parameter. This parameter is read-only. | string (length 0..255) | n/a | show |
| supported-values | This list indicates the possible values that this parameter can take as an input. It is a list of ranges or discrete numbers. This parameter is read-only. | string (length 0..256) | n/a | show |
| direction | Advanced parameter is applicable to the specified direction. This parameter is read-only. | • transmit<br>• receive<br>• transmit-and-receive | n/a | show |
| multiplicity | Identifies the number of values users need to enter for this advanced parameter. Same range or allowed-values will apply for each entry. This parameter is read-only. | Integer, uint8 | n/a | show |
| configuration-impact | Identifies the configuration steps to apply the change. This parameter is read-only. | • no-change<br>• no-reacquire<br>• reacquire<br>• full-config-pll-change<br>• full-config-no-pll-change | n/a | show |
| service-impact | Identifies if applying this parameter change causes service impact. If it is service-affecting, users must perform an admin lock/ maintenance operation or other relevant operations. This parameter is read-only. | • service-affecting<br>• non-service-affecting | n/a | show |

#### Examples

This example shows how to display the information about the golden advanced parameters:

```
show golden-advanced-parameter
```

This example shows how to display the information about the golden advanced parameters on CHM7:

```
show golden-advanced-parameter-CHM7/*
```

This example shows how to display the information about a golden advanced parameter on CHM7:

```
show golden-advanced-parameter-chm7/EEPNNLMitigation
```

The following output is displayed:

```
golden-advanced-parameter-CHM7/EEPNNLMitigation
  description                                                  'Equalizer enhanced phase noise and nonlinearities mitigation algorithm'
  type                                                         'integer'
  supported-values                                             '0-12'
  direction                                                    receive
  multiplicity                                                 2
  configuration-impact                                         no-reacquire
  service-impact                                               service-affecting
```

<!-- page 521 -->
