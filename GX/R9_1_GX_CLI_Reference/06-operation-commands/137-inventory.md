---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.137. inventory'
source_lines: 12629-12702
---

## 6.137. inventory

#### Command Description

These commands are used to show the inventory data for a present FRU.

#### Command Syntax

```
show inventory-<card-name>-<port-name> [hardware-version] [actual-type] [actual-subtype] [sw-support-revision] [PON] [serial-number]
[clei] [vendor] [part-number] [manufacture-date] [insertion-date] [number-of-lanes] [vendor-compliance-code] [actual-power-class]
[actual-max-power-draw] [fw-status]
show inventory-<card-name>.<slot-name> [hardware-version] [actual-type] [actual-subtype] [sw-support-revision] [PON] [serial-number]
[clei] [vendor] [part-number] [manufacture-date] [insertion-date] [number-of-lanes] [vendor-compliance-code] [actual-power-class]
[actual-max-power-draw] [fw-status]
show inventory-<chassis-name>-<slot-name> [hardware-version] [actual-type] [actual-subtype] [sw-support-revision] [PON] [serial-number]
[clei] [vendor] [part-number] [manufacture-date] [insertion-date] [number-of-lanes ][vendor-compliance-code] [actual-power-class]
[actual-max-power-draw] [fw-status]
show inventory-<name> [hardware-version] [actual-type] [actual-subtype] [sw-support-revision] [PON] [serial-number] [clei] [vendor] [part-number]
[manufacture-date] [insertion-date] [number-of-lanes] [vendor-compliance-code] [actual-power-class] [actual-max-power-draw] [fw-status]
```

#### Command Usage Details

**Table 362: inventory Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

<!-- page 568 -->

**Table 363: inventory Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the object. | String | n/a | show |
| chassis-name | Chassis name. | String (length 0..64) | n/a | show |
| card-name | Card base object. | This object has parameters that are common to all existing card types (controller, fan, etc). | n/a | show |
| port-name | The name of the port object. | String | n/a | show |
| hardware-version | Hardware version of this FRU. | String | n/a | show |
| actual-type | FRU type of actual equipment. | String | n/a | show |
| actual-subtype | FRU subtype of actual equipment - only available if applicable. | String | n/a | show |
| sw-support-revision | Software revision currently installed. | Number | 0 | show |
| PON | Current PON of the equipment. | String | n/a | show |
| serial-number | Serial number of the equipment. | String | n/a | show |
| clei | Common Language Equipment Identifier. | String | n/a | show |
| vendor | Part number for this equipment. | String | n/a | show |
| manufacture-date | Manufacture Date in a date-time format (YYYY-MM-DDThh:mm:ssZ) or 'NA' if not available. | String | n/a | show |
| insertion-date | Insertion Date in a date-time format (YYYY-MM-DDThh:mm:ssZ) or 'NA' if not available. | String | n/a | show |
| number-of-lanes | When applicable, provides number of supported optical lanes in this equipment. | Number | n/a | show |
| vendor-compliance-code | Vendor Compliance Code information for 3rd party TOMs. | string (length 0...18) | n/a | show |
| actual-power-class | Power class reported by the pluggable (for example from module management data), when available. Only populated for third-party transceiver subtypes on hosts that enforce power-class rules. Native qualified subtypes do not populate this leaf. Complementary host limits are published in system capabilities (supported-tom-power on the corresponding supported port). | uint8 (range 1..8) | n/a | show |
| actual-max-power-draw | Maximum power draw indicated by the pluggable for the reported power class, when available. Populated under the same conditions as actual-power-class. | decimal64 | n/a | show |
| fw-status | not-applicable - Card doesn't have upgradeable firmware. current - All components have current firmware. not-current - At least one component does not have current firmware. unavailable - Information on all firmware status is currently unavailable. | not-applicable current not-current unavailable | not-applicable | show |

#### Examples

This example shows how to view all inventory information:

```
show inventory
```

This example shows how to view the inventory information of a TOM object:

```
show inventory-1-3-1
```

<!-- page 570 -->
