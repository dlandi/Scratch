---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.40. card'
source_lines: 6449-6549
---

## 6.40. card

#### Command Description

These commands are used to add, edit, show or delete a card-base object. This object has parameters that are common to all existing card types (controller, fan, etc).

#### Command Syntax

```
add card-<name> required-type <value> chassis-name <value> slot-name <value> [required-subtype <value>] [card-mode <value>] [subslot-name
<value>] [power-profile <value>] [alias-name <value>] [admin-state <value>] [alarm-report-control <value>] [label <value>]
set card-<name> [required-subtype <value>] [card-mode <value>] [power-profile <value>] [alias-name <value>] [admin-state <value>]
[alarm-report-control <value>] [label <value>]
show card-<name> [required-type] [required-subtype] [card-mode] [category] [chassis-name] [slot-name] [subslot-name] [max-power-draw]
[power-profile] [last-reboot-reason] [last-reboot-time] [parent-card] [subcard-list] [alias-name] [AID] [admin-state] [oper-state] [avail-state]
[alarm-report-control] [label]
delete card-<name>
```

#### Command Usage Details

**Table 152: card Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 266 -->

#### Command Parameters

**Table 153: card Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Card base object. | This object has parameters that are common to all existing card types (controller, fan, etc). | n/a | add, set, delete |
| required-type | The card type. The required type filed is applicable to the following cards:<br>• BAXOFP2<br>• BLANK<br>• BLANK2<br>• CAD10A<br>• CAD16AOFP2<br>• CDC4D4OFP2<br>• CDC8D6<br>• CHM1R<br>• CHM2T<br>• CHM6<br>• CHM7<br>• CHM7X<br>• DGE2M2OFP2<br>• DGE2M2WOFP2<br>• FAN<br>• FAN-CTRL<br>• FAN32<br>• FAN34c • FC01MDUP<br>• FC04MDUP<br>• FRCU<br>• FRCU31<br>• FRCU32<br>• ILAx<br>• ILA2M<br>• PBAx<br>• IOPANEL<br>• IOPANEL32<br>• OCC2E<br>• OCC2T<br>• OCMH8OFP2<br>• OMD32E<br>• OMD40E<br>• OMD48E<br>• OMD48S<br>• OMD64<br>• OMD64S<br>• OPSOFP2<br>• OPSPTOFP2<br>• OTDR8OFP2<br>• OTSCSOFP2<br>• PAxOFP2 • PEM<br>• RD12TI<br>• RD09SM<br>• RD20TM<br>• RD32TH<br>• RD66TM<br>• RPBL<br>• RPBM<br>• SPN2<br>• SPN2C<br>• UCM4<br>• UTM2<br>• WS04SOFP2<br>• XMM4<br>• XMM4-FAN | String (length 0..32) | n/a | add, show |
| card-mode | The configured card-mode, identifies specific card functionality. • For BAXOFP2, the supported card-mode strings are: ▪ drop (default) - only allowed when BAX is part of an ADG. ▪ add - only allowed when BAX is part of an ADG. ▪ degree - only allowed if BAX is part of a degree 'modules-degree'.<br>• For PAxOFP2, the supported card-mode string is: ▪ degree (default) - applicable if PAx is part of a degree 'modules-degree'. Currently it is not configurable.<br>• For C2ILASGH, the supported card-mode strings are: ▪ ila-mode - the system spontaneously sets C2ILASGH card-mode to ila-mode when the card is equipped into one NE of node-type ILA. ▪ oadm-mode - the system spontaneously set C2ILASGH card-mode to oadm-mode when the card is equipped into one NE of node-type OADM.<br>• For UTM2, the supported card-mode strings are: ▪ normal-ext (default) - is supported starting from R7.0, and, by default, it is the working mode applied to all newly created UTM2 cards. With this working mode, for one line port (port 1 or 2 configured with 100G and 200G service including 2x100G grey muxponder service), the maximum total bandwidth for sub-5G clients is 20G. The maximum total bandwidth per UTM2 card for sub-5G client signals is 20G. ▪ normal - this is a legacy working mode used prior to R7.0, and is only applicable to cards previously configured as normal and migrated to R7.0. Starting from R7.0, it is not possible to set UTM2 card-mode to normal. With this working mode, for one line port (port 1 or 2 configured with 100G and 200G service), the maximum total bandwidth for sub-5G clients is 10G. The maximum total bandwidth per UTM2 card for sub-5G client signals is 20G. For these cards: – the card mode can be set to normal-ext by a user. – the card mode is automatically set to normal-ext after a reset to the factory default or a database cleanup. ▪ grey-muxponder - working mode used for 100G grey muxponder application. In this case, UTM2 only has 100G capacity and works as a grey muxponder for client signals on ports 7-18 to be multiplexed into port 3.<br>• For RD20TM, the supported card-mode strings are: ▪ standard - it is the default card mode when l0-mode-op is standard. ▪ slte - it is applicable when l0-mode-op is slte. The card-mode is set to slte when the RD20TM card is facing the subsea. ▪ slte-backhaul - it is the default card mode when l0-mode-op is slte. When changing the card-mode change from slte to slte-backhaul, a warm reboot of the RD20TM module is recommended to clear any communication alarms. | string (length 0..20) | n/a | add, set, show |
| required-subtype | The subtype of the card. Required sub-type field is applicable for the following cards:<br>• FAN: counter-rotating, single-rotor<br>• PEM: AC, DC<br>• PAxOFP2: ER (default value), LR, IR.<br>• RPBM: MP4, MP5 • RPBL: LS2, MP6<br>• CHM6: C13, C14, C4, C6, C8, C9 and C15 | String (length 0..32) | n/a | add, set, show |
| chassis-name | Chassis where this card is located. | String (length 1...255) | 1 | add, show |
| slot-name | Slot where this card is located. | String | n/a | add, show |
| subslot-name | Subslot where this card is located, e.g. 1-2.3 (slot 2, subslot 3). 'subslot-name' can only be set on (sub)card creation. | String | n/a | add |
| power-profile | User configured power draw for this card. i Note: For CHM7 card only the value of high is supported. | high: 445 ( for CHM6/CHM6L) and 470 (for CHM7), medium: 400 (for CHM6/CHM6L) low: 355 (for CHM6/CHM6L) | n/a | add, set, show |
| category | card category | line-card, fan, power-supply, other, carrier-card, blank | n/a | show |
| last-reboot-reason | Reason why the last reboot was done. | String | n/a | show |
| last-reboot-time | Timestamp of the last reboot event of a card. | String | n/a | show |
| max-power-draw | Maximum power draw for this card. | decimal64 | n/a | show |
| parent-card | Name of the parent card, only applicable for subcard(s). | Path: /ne/equipment/card/name | n/a | show |
| admin-state | The administrative state of the managed object. | lock, maintenance, unlock | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | Inhibited | add, set, show |
| alias-name | User defined alias for this entity. | String (length 0..256) | n/a | add, set, show |
| label | User-defined label for the card. | String (length 0..256) | n/a | add, set, show |
| subcard-list | List of sub-cards associated with this card. Only applicable for carrier cards. | Path: /ne/equipment/card/name | n/a | show |

#### Examples

The following example shows how to add CHM1R card in chassis 1 and slot 2 with name '1-4':

```
add card-1-4 required-type CHM1R chassis-name 1 slot-name 2
```

<!-- page 276 -->

The following example shows how to add CHM1R card in chassis 1 and slot 2 with name '1-CHM1R':

```
add card-1-CHM1R required-type CHM1R chassis-name 1 slot-name 2
```

The following example shows how to add a card:

```
add card-1-7 chassis-name 1 required-type CHM6 slot-name 7  required-subtype C6 power-profile low
```

The following example shows how to edit a card:

```
set card-1-7 power-profile medium
Deploying card with non-default power allocation may result in hardware damage. Do you want to continue? [y/n] y
```

The following example shows how to show card attributes:

```
show card-1-7 power-profile
  card-1-7
  power-profile         'medium'
```

The following example shows how to set RD20TM card mode to SLTE:

```
set card-1-6 card-mode slte
```

<!-- page 277 -->
