---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.116. gapt'
source_lines: 11429-11508
---

## 6.116. gapt

#### Command Description

This command is used to list the golden advanced parameters from the Golden Advanced Parameters Table (GAPT).

#### Command Syntax

```
show gapt-<card-type> [version] [applicable-resource-type]
```

#### Command Usage Details

**Table 318: gapt Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 319: gapt Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| card-type | Card type name. | string | n/a |
| version | Table version. | string (length 0..5) | n/a |
| applicable-resource-type | The managed resource type(s) that are applicable for this particular advanced parameter. | string | n/a |

#### Examples

This example shows how to display the Golden Advanced Parameters Tables:

```
show gapt
```

<!-- page 513 -->

The following output is displayed:

```
gapt        version  applicable-resource-type
----------  -------  ------------------------
gapt-CHM6   1.8
gapt-CHM7   3.0
gapt-CHM7T
```

This example shows how to display golden advanced parameters from the GAPT supported by CHM7:

```
show gapt-chm7
```

The following output is displayed:

```
  golden-advanced-parameter-CHM7/BOASetting
  golden-advanced-parameter-CHM7/CRAvgNCtrl
  golden-advanced-parameter-CHM7/ClockPreEmphasis
  golden-advanced-parameter-CHM7/EEPNNLMitigation
  golden-advanced-parameter-CHM7/FastCDAcqTimeCtrl
  golden-advanced-parameter-CHM7/NLC
  golden-advanced-parameter-CHM7/PDLMitigation
  golden-advanced-parameter-CHM7/RxCDCompCtrl
  golden-advanced-parameter-CHM7/RxCDMode
  golden-advanced-parameter-CHM7/RxCDSlope
  golden-advanced-parameter-CHM7/RxRollOff
  golden-advanced-parameter-CHM7/TxEqWindowSel
  golden-advanced-parameter-CHM7/TxHighFreqGain
  golden-advanced-parameter-CHM7/TxSignalBWControl
  gapt-CHM7
  version                                                       '3.0'
  applicable-resource-type
```

<!-- page 514 -->
