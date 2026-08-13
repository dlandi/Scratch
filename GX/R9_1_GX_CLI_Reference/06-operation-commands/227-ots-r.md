---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.227. ots-r'
source_lines: 17858-17912
---

## 6.227. ots-r

#### Command Description

These commands are used to enable, add, set or show the attributes associated with Optical Transport Section (OTS), with reduced scope.

#### Command Syntax

```
set ots-r-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [required-fiber-type-rx <value>] [fiber-length-rx <value>]
[span-loss-receive <value>] [external-attenuation-rx <value>][delta-pointloss <value>]
show ots-r-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [required-fiber-type-rx] [configured-fiber-type-rx] [fiber-length-rx]
[configured-fiber-length-rx] [span-loss-receive] [span-loss-at-amplifier] [external-attenuation-rx] [delta-pointloss] [power-actual-rx]
[connected-reference]
```

#### Command Usage Details

**Table 543: ots-r Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 544: ots-r Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| required-fiber-type-rx | The required Fiber Type on the DWDM Line, with reference for the Rx fiber. Only of relevance if control-mode = auto and when there is no fiber-connection. Fiber types:<br>• AllWave<br>• DrakaLL: Draka Long Line<br>• DSF: Dispersion Shifted Fiber<br>• LEAF: Large Effective Area Fiber<br>• LS: LS Fiber<br>• PSLC: Pure Silice Large Core<br>• PureSilica: Pure Silica • SMF-ULL: Single-Mode Fiber - Ultra Low Loss<br>• SSMF: Standard Single Mode Fiber<br>• Teralight<br>• TWC: True Wave Classic<br>• TWMinus: True Wave Minus<br>• TWPlus: True Wave Plus<br>• TWReach: True Wave Reach<br>• TWRS: True Wave Reduced Slope<br>• VistaCor | • AllWave<br>• DrakaLL<br>• DSF<br>• LEAF<br>• LS<br>• PSLC<br>• PureSilica<br>• SMF-ULL<br>• SSMF<br>• Teralight<br>• TWC<br>• TWMinus<br>• TWPlus<br>• TWReach • TWRS<br>• VistaCor | SSMF | set, show |
| fiber-length-rx | Receiving Fiber Length | unspecified, decimal64 range 0..500.0 | unspecified | set, show |
| span-loss-receive | The Span Loss at the receiving dwdm-line. | span-loss-info | 99 | set, show |
| span-loss-at-amplifier | The Span Loss detected at amplifier, when there is a fiber-connection from/ RPB to the amplifier. | span-loss-info | - | set show |
| external-attenuation-rx | External Attenuation, configured by the user. | ioa-l0:type-of-attenuation | n/a | set, show |
| target-power-setting | Allows automatic configuration of target values for oxcon. | manual, auto | auto | set, show |
| delta-pointloss | Delta Pointloss (Rx). Additional attenuation that can be determined after turning up pumps. This is the fiber contribution for the pointloss: to be fine tuned in the field. This additional optical attenuation may be due to e.g. bad splice at dwdm-line Rx, higher att. than 0.1 dB. | • not-applicable<br>• decimal64 in the range (-1..3.5dB) | not-applicable | set, show |
| connected-reference | Connected Reference. Indicates the degree the Raman is connected to. In ILA node-type(s), the direction the Raman is connected to (1 means direction 1-2, 2 means 2-1) | uint8 | 0 | show |

#### Examples

The following example shows how to set external-attenuation-rx OTS attribute:

```
set ots-r-11-2-dwdm-line external-attenuation-rx 2
```

<!-- page 892 -->
