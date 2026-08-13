---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.363. upgrade-status'
source_lines: 27034-27070
---

## 6.363. upgrade-status

#### Command Description

This command displays all the SW versions being installed in the system, and their installation status. This status is reported at the NE level, chassis level and card level. The resource parameter specifies the object where the status is reported.

#### Command Syntax

```
show upgrade-status-<resource> [to-swload-version] [status] [start-time] [end-time] [step] [step-start-time] [details]
```

#### Command Usage Details

**Table 833: upgrade-status Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 834: upgrade-status Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| resource | The resource to which the status refers to. May represent the entire ne, a chassis, or a card. For ne and chassis, the results provide aggregated summaries of all cards in that scope. | string (length 0..255) | n/a | show |
| to-swload-version | Target Software Load Version. | string (length 1..64) | n/a | show |
| status | The current upgrade status for this resource: • idle - No active software upgrade in progress<br>• upgrade-in-progress - NE upgrade in progress<br>• upgrade-complete - NE upgrade complete<br>• upgrade-partially-failed - NE upgrade partial complete<br>• upgrade-failed - NE upgrade failed<br>• validate-in-progress - NE/Chassis/Card validate in progress<br>• validate-failed - NE/Chassis/ Card validation failed<br>• validate-complete - NE/Chassis/ Card validation complete<br>• apply-in-progress - NE/Chassis/ Card apply in progress<br>• apply-failed - NE/Chassis/Card apply failed<br>• apply-complete - NE/Chassis/ Card apply complete<br>• activate-in-progress - Chassis/ Card activation in progress<br>• activate-failed - Chassis/Card activation failed • activate-complete - Chassis/ Card activation complete.<br>• cancel-in-progress - NE/Chassis/Card applied software cancellation in progress.<br>• cancel-complete - NE/Chassis/ Card applied software canceled.<br>• cancel-failed - NE/Chassis/Card failed to cancel the applied software.<br>• no-communication - No communication. | • idle<br>• upgrade-in-progress • upgrade-complete<br>• upgrade-partially-failed<br>• upgrade-failed<br>• validate-in-progress<br>• validate-failed<br>• validate-complete<br>• apply-in-progress<br>• apply-failed<br>• apply-complete<br>• activate-in-progress<br>• activate-failed<br>• activate-complete<br>• cancel-in-progress<br>• cancel-complete<br>• cancel-failed<br>• no-communication | idle | show |
| start-time | The start timestamp of the current phase of upgrade. It displays the value 'na' if this entity was idle since startup. | • na<br>• date-and-time | n/a | show |
| end-time | The end timestamp of the current phase of upgrade. It displays the value 'na' if this entity has not finished any upgrade phase since startup. | • na<br>• date-and-time | n/a | show |
| step | The identifier for the current upgrade step. | string (length 0..128) | n/a | show |
| step-start-time | The timestamp at which the current upgrade step was initiated. | • na<br>• date-and-time | n/a | show |
| details | Details on the current upgrade. | string (length 0..255) | n/a | show |

<!-- page 1323 -->
