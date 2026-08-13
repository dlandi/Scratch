---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.361. unprovisioned-inventory'
source_lines: 26939-26980
---

## 6.361. unprovisioned-inventory

#### Command Description

This command is used to show a .ist of detected inventory but not yet accepted by the Node Controller in Multi-Chassis configuration.

#### Command Syntax

```
show unprovisioned-inventory-<chassis-serial-number>/<slot-name> [hardware-version] [actual-type] [actual-subtype] [sw-support-revision] [PON]
[serial-number] [clei] [vendor] [part-number] [manufacture-date] [detection-timestamp]
```

#### Command Usage Details

**Table 828: unprovisioned-inventory Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 829: unprovisioned-inventory Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| chassis-serial-number | The residing chassis serial number | string (length 0..16) | n/a |
| slot-name | The residing slot name for the equipment. If the equipment is the chassis, the slot-name is empty. | string (length 0..16) | n/a |
| hardware-version | Hardware version of this FRU. | string | n/a |
| actual-type | FRU type of actual equipment. | string | n/a |
| actual-subtype | FRU subtype of actual equipment - only available if applicable. | string | n/a |
| PON | Current PON of the equipment. | string | n/a |
| serial-number | Serial number of the equipment. | string (length 1..16) | n/a |
| clei | Common Language Equipment Identifier. | string | n/a |
| vendor | Vendor of this equipment. | string | n/a |
| part-number | Part number for this equipment. | string | n/a |
| manufacture-date | Manufacture Date in the date-time format YYYY-MM-DDThh:mm:ssZ see the set-time command for detailed information. | string | n/a |
| detection-timestamp | Timestamp with the last time the unprovisioned equipment was detected by the Node Controller. | date-and-time | n/a |

<!-- page 1317 -->
