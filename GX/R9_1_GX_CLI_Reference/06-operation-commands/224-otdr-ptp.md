---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.224. otdr-ptp'
source_lines: 17623-17698
---

## 6.224. otdr-ptp

#### Command Description

These commands are used to add, delete set or show the OTDR ptp. When the OTDR card is created, all otdr-ptp are created, and associated with each one of them, the corresponding otdr-ptp object to port.

#### Command Syntax

```
add otdr-ptp-<name> [label <value>] [admin-state <value>] [otdr-range <value>] [otdr-pulse-width <value>] [otdr-measurement-speed <value>]
[otdr-ior <value>] [otdr-fiber-type <value>] [otdr-resolution <value>] [otdr-direction-mode <value>] [peak-power <value>] [launching-fiber-length
<value>]
delete otdr-ptp-<name>
set otdr-ptp-<name> [label <value>] [admin-state <value>] [otdr-range <value>] [otdr-pulse-width <value>] [otdr-measurement-speed
<value>] [otdr-ior <value>] [otdr-fiber-type <value>] [otdr-resolution <value>] [otdr-direction-mode <value>] [peak-power <value>]
[launching-fiber-length <value>]
show otdr-ptp-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [otdr-range] [otdr-pulse-width] [otdr-measurement-speed] [otdr-ior] [otdr-fiber-type] [otdr-resolution]
[otdr-direction-mode] [peak-power] [otdr-last-measurement] [otdr-last-measurement-file] [otdr-fiber-break-distance] [launching-fiber-length]
```

#### Command Usage Details

**Table 536: otdr-ptp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

<!-- page 861 -->

#### Command Parameters

**Table 537: otdr-ptp Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 538: otdr-ptp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| supporting-card | Card that holds this facility. | leafref (path "../../../equipment/card/name") | n/a | show |
| supporting-port | Port that holds this facility. | leafref ( path "../../../equipment/card/port/name") | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| otdr-range | Specifies the distance range in kilometers as a basis to calculate the measurement repetition period. It is recommended that the parameter be set to the actual length of the fiber to be measured plus 10% of the length. i Note: Starting from R9.1, if the corresponding OTS can be traced from the fiber connection between the ILA/ROADM card and OTDR8OFP2, otdr-range is derived from OTS fiber-length-tx/fiber-length-rx(or OTS fiber-length-derived-rx/fiber-leng th-derived-tx if fiber-length-rx/fiber-length-tx is auto), with 10% margin and rounded up to the nearest 10 km. | • auto<br>• range [0...300.0km] | auto | add, set, show |
| otdr-pulse-width | Specifies the OTDR pulse width in nano-seconds (ns). The pulse width determines the dynamic range together with other OTDR measurement parameters. | • auto<br>• range [10...20,000 ns] | auto | add, set, show |
| otdr-measurement-speed | Specifies the OTDR measurement speed. fast – Fast speed. Approximate acquisition time: 10 seconds. medium – Medium speed. Approximate acquisition time: 15 seconds. slow – Slow speed. Approximate acquisition time: 1 minute. precision – Very slow speed for precise result. Approximate acquisition time: 3 ... 5 minutes. auto – indicates that the measurement speed shall be selected automatically. high-precision – The slowest speed for high precise result. Approximate acquisition time: 5 ... 7 minutes. | • fast<br>• medium<br>• slow<br>• precision • auto<br>• high-precision | auto | add, set, show |
| otdr-ior | Specifies the group index of refraction (IOR) of the fiber to be measured by OTDR. | • auto<br>• range [1.0...2.0] | auto | add, set, show |
| otdr-fiber-type | Specifies the fiber type of the fiber to be measured by OTDR:<br>• not-applicable<br>• auto - Automatic fiber-type (only for OTDR)<br>• not-configured - Fiber-type is not known, or not configured.<br>• AllWave<br>• DrakaLL: Draka Long Line<br>• DSF: Dispersion Shifted Fiber<br>• LEAF: Large Effective Area Fiber<br>• LS: LS Fiber<br>• PSLC: Pure Silice Large Core<br>• PureSilica: Pure Silica • SMF-ULL: Single-Mode Fiber - Ultra Low Loss<br>• SSMF: Standard Single Mode Fiber<br>• Teralight<br>• TWC: True Wave Classic<br>• TWMinus: True Wave Minus<br>• TWPlus: True Wave Plus<br>• TWReach: True Wave Reach<br>• TWRS: True Wave Reduced Slope<br>• VistaCor i Note: SMF-ULL must be configured for the integrated OTDR in RD66TM and D2ILASGM, as well as for HSC-OLS deployments. i Note: Starting from R9.1, if the corresponding OTS can be traced from the fiber connection between the ILA/ROADM card and OTDR8OFP2, otdr-fiber-type = auto is derived from OTS fiber-type-tx/fiber-type-rx. | • not-applicable<br>• auto - Automatic fiber-type (only for OTDR)<br>• not-configured - Fiber-type is not known, or not configured.<br>• AllWave<br>• DrakaLL<br>• DSF<br>• LEAF<br>• LS<br>• PSLC<br>• PureSilica<br>• SMF-ULL<br>• SSMF • Teralight<br>• TWC<br>• TWMinus<br>• TWPlus<br>• TWReach<br>• TWRS<br>• VistaCor | auto | add, set, show |
| otdr-resolution | Specifies the OTDR data sampling resolution. | • auto<br>• range [0.4...100.0] | auto | add, set, show |
| otdr-direction-mode | Specifies the OTDR measurement direction and if OTDR measurement is in-service or out-of-service. | • not-applicable<br>• co-prop-in-service - Indicates co-propagation in service<br>• co-prop-out-service - Indicates co-propagation out of service<br>• counter-prop-in-service - Indicates counter-propagation in service<br>• counter-prop-out-service - Indicates counter-propagation out of service | counter-prop-in-service | add, set, show |
| peak-power | Specifies the OTDR peak power | • auto<br>• range [5...14.5] | auto | add, set, show |
| otdr-last-measurement | Indicates the last OTDR measurement date and time on the port. | &lt;date-and-time&gt; | 0000-01-01T00:00: 00.00Z | show |
| launching-fiber-length | Specifies the launching fiber length (in meters) information for SOR to filter the launching fiber path data. A launching fiber may be used to connect an OTDR port to the fiber to be measured. The length of the launching fiber, if any, is deducted from the OTDR measurement result. Tip: The auto launching-fiber-length is not supported. | range [0...50m] | 0 | add, set, show |
| otdr-fiber-break-distance | In case the OTDR has clearly identified a fiber break in the last measurement, this attribute indicates the distance of the fiber break (in km). It indicates not-available in case OTDR has not identified a fiber break. | integer | not-available | show |
| otdr-last-measurement-file | The last OTDR measurement the generated .sor file. | string | n/a | show |

#### Examples

This example shows how to set OTDR parameters (launching-fiber-length, otdr-fiber-type, otdr-measurement-speed, otdr-pulse-width, otdr-range, and otdr-resolution) on an OTDR port:

```
set otdr-ptp-114-3.1-2 launching-fiber-length 20 otdr-fiber-type auto otdr-measurement-speed high-precision otdr-pulse-width auto otdr-range 100
 otdr-resolution auto
```

<!-- page 868 -->
