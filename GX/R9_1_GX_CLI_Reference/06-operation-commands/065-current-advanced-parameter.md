---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.65. current-advanced-parameter'
source_lines: 8105-8177
---

## 6.65. current-advanced-parameter

#### Command Description

This command is used to show the current values of the advanced parameters, which are running on the system.

#### Command Syntax

```
show current-advanced-parameter-<optical-carrier-name>/<current-advanced-parameter-name> [value]
```

#### Command Usage Details

**Table 209: current-advanced-parameter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 210: current-advanced-parameter Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| optical-carrier-name | The name of the optical carrier supporting the advanced parameter. | string |
| current-advanced-parameter-name | The name of the advanced parameter. | string |
| value | The value of the advanced parameter, which is running on the system.. | string |

#### Examples

This example shows how to view the current running values of the advanced parameters:

```
show current-advanced-parameter
```

<!-- page 360 -->

This example shows how to view the current running values of the advanced parameters on optical-carrier-1-4-L2-1: -1-7-L1-1:

```
show current-advanced-parameter-1-7-L1-1/*show current-advanced-parameter-1-4-L2-1/*
```

Example of an output retrieved from the system:\<codeblock id="codeblock\_azq\_wkh\_43c" class="+ topic/pre pr-d/codeblock "\>current-advanced-parameter value ----------------------------------------------------- ------------------------------- current-advanced-parameter-1-7-L1-1/BOASetting 3 current-advanced-parameter-1-7-L1-1/ CRAvgNCtrl 0 current-advanced-parameter-1-7-L1-1/ClockPreEmphasis 20 0 current-advanced-parameter-1-7-L1-1/EEPNNLMitigation 0 1 current-advanced-parameter-1-7-L1-1/FastCDAcqTimeCtrl -22000 22000 current-advanced-parameter-1-7-L1-1/NLC 10 current-advanced-parameter-1-7-L1-1/PDLMitigation 0 current-advanced-parameter-1-7-L1-1/PMDEquUpdSpeed 0 20 (not the configured value) current-advanced-parameter-1-7-L1-1/RxCDCompCtrl -30000 100000 0 current-advanced-parameter-1-7-L1-1/RxCDMode 1 current-advanced-parameter-1-7-L1-1/RxCDSlope 0 current-advanced-parameter-1-7-L1-1/RxRollOff 0 50 current-advanced-parameter-1-7-L1-1/RxVOAMode 2 0 current-advanced-parameter-1-7-L1-1/TxEqWindowSel 0 current-advanced-parameter-1-7-L1-1/ TxHighFreqGain 0 current-advanced-parameter-1-7-L1-1/TxSignalBWControl 50\</codeblock\>

```
current-advanced-parameter                              value
....................................................... ....................
current-advanced-parameter-1-4-L2-1/BOASetting          3
current-advanced-parameter-1-4-L2-1/CRAvgNCtrl          0
current-advanced-parameter-1-4-L2-1/ClockPreEmphasis    26 4096
current-advanced-parameter-1-4-L2-1/EEPNNLMitigation    0 1
current-advanced-parameter-1-4-L2-1/FastCDAcqTimeCtrl   -22000 22000
current-advanced-parameter-1-4-L2-1/NLC                 10
current-advanced-parameter-1-4-L2-1/PDLMitigation       0
current-advanced-parameter-1-4-L2-1/PMDEquUpdSpeed      20
current-advanced-parameter-1-4-L2-1/RxCDCompCtrl        -30000 50000 0
current-advanced-parameter-1-4-L2-1/RxCDMode            1
current-advanced-parameter-1-4-L2-1/RxCDSlope           0
current-advanced-parameter-1-4-L2-1/RxRolloff           0 50
current-advanced-parameter-1-4-L2-1/RxVOAMode           2 0
current-advanced-parameter-1-4-L2-1/TxEqWindowSel       0
current-advanced-parameter-1-4-L2-1/TxHighFreqGain      0
current-advanced-parameter-1-4-L2-1/TxSignalBWControl   50
```

\<note id="note\_lzg\_dvf\_4hc" class="- topic/note "\>PMDEquUpdSpeed cannot be added or modified; it is to be supported in a future release.\</note\>

<!-- page 361 -->
