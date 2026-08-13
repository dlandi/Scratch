---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.256. pump-power'
source_lines: 19767-19803
---

## 6.256. pump-power

#### Command Description

These commands are used to set up a Raman pump.

#### Command Syntax

```
set pump-power-<name>/<pump-id> [target-pump-power <value>]
show pump-power-<name>/<pump-id> [target-pump-power] [configured-pump-power] [min-target-pump-power] [max-target-pump-power] [actual-pump-power]
```

#### Command Usage Details

**Table 604: pump-power Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 605: pump-power Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the pump | string | n/a | set, show |
| pump-id | The 'pump-id' is an integer identifying the number of the pump. | integer | n/a | set, show |
| target-pump-power | Raman Pump Power required in dBm units. Applicable when the control-mode is manual. • If the card is RPBM, the target-pump-power must be in the range of 12 to 30dBm<br>• 0 - for not-applicable case i Note: If the target-pump-power value is outside the operating range of the Raman sled, the alarm PUMP-Fail (STAT FAIL PSTAT) is _ _ raised. | • value in the range: -99.00..99.00 dBm<br>• not-applicable - Not Applicable/ Not specified. | not-applicable | set, show |
| min-target-pump-power | Minimum target pump power. | • decimal64, fraction-digits 2, range: -99.00..99.00 dBm<br>• not-applicable - for not-applicable case | not-applicable | show |
| max-target-pump-power | Maximum target pump power. | • decimal64, fraction-digits 2, range: -99.00..99.00 dBm<br>• not-applicable - for not-applicable case | not-applicable | show |
| configured-pump-power | The pump power configured in the hardware in dBm units. Value can be derived automatically, if control-mode is auto, or otherwise via the target-pump-power. | • range: -99.00..99.00 dBm<br>• not-applicable - for not-applicable case | not-applicable | show |
| actual-pump-power | The actual values which are currently measured in each pump. | range: -99.00..99.00 dBm | -99 | show |

<!-- page 997 -->
