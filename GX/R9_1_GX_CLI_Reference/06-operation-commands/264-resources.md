---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.264. resources'
source_lines: 20148-20209
---

## 6.264. resources

#### Command Description

This command is used to show system or card resources.

#### Command Syntax

```
show resources-<name> [supported-carriers] [unassigned-carriers] [supported-sub-components]
show resources-<name> [supported-carriers] [unassigned-carriers] [supported-sub-components] [internal-cell-switch-total-bandwidth]
[internal-cell-switch-available-bandwidth] [paired-slot-total-bandwidth] [paired-slot-available-bandwidth]
```

#### Command Usage Details

**Table 620: resources Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 621: resources Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| name | The name of the resource | string | n/a |
| supported carriers | A list of carriers that are bound to this resource. | string | n/a |
| unassigned-carriers | Names of the carriers that are not yet assigned to a resource. | string | n/a |
| supported-sub-components | Names of sub-components present in this card, which can be addressed for certain operations like restart. | string | n/a |
| internal-cell-switch-total-bandwidth | Total internal cell-switch bandwidth. i Note: This parameter is applicable only for SPN2/SPN2C cards. | number | Defaults to 600.000 Gbit/s if the SPN2/ SPN2C card has paired slot interconnection capability |
| internal-cell-switch-available-bandwidth | Available internal cell-switch bandwidth. i Note: This parameter is applicable only for SPN2/SPN2C cards. | number | Defaults to 600.000 Gbit/s if the SPN2/ SPN2C card has paired slot interconnection |
| paired-slot-total-bandwidth | Total supported bandwidth for the paired slot connection. i Note: This parameter is applicable only for SPN2/SPN2C cards that support Paired Slots. | number | • Defaults to 0 if the SPN2/SPN2C card does not have paired slot interconnection capability<br>• Defaults to 500.000 Gbit/s if the SPN2/SPN2C card has paired slot interconnection capability |
| paired-slot-available-bandwidth | Available bandwidth for the paired slot connection. i Note: This parameter is applicable only for SPN2/SPN2C cards that support Paired Slots. | number | • Defaults to 0 if the SPN2/SPN2C card does not have paired slot interconnection capability<br>• Defaults to 500.000 Gbit/s if the SPN2/SPN2C card has paired slot interconnection |

#### Examples

The following command shows how to show resources of paired SPN2/SPN2C card-254-1:

<!-- page 1015 -->

```
show resources-254-1
```

The following output is displayed:

```
  resources-254-1
  supported-carriers                          254-1-1,254-1-2,254-1-3,254-1-4
  unassigned-carriers                         254-1-1,254-1-3,254-1-4
  supported-sub-components
  internal-cell-switch-total-bandwidth        600.000 Gbit/s
  internal-cell-switch-available-bandwidth    358.750 Gbit/s
  paired-slot-total-bandwidth                 500.000 Gbit/s
  paired-slot-available-bandwidth             258.750 Gbit/s
```

<!-- page 1016 -->
