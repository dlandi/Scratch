---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.251. protection-group'
source_lines: 19421-19498
---

## 6.251. protection-group

#### Command Description

These commands are used to add, set and show a protection group. The delete command is used to remove a protection group from the configuration.

#### Command Syntax

```
add protection-group-<name> protection-type <value> working-pu <value> protection-pu <value> reliable-cp <value> [label <value>] [pg-request
<value>][pg-control-request <value>] [switching-mode <value>] [reversion-mode <value>] [hold-off-timer <value>] [wtr-timer <value>]
[client-side-olos-trigger <value>] [client-side-sd-trigger <value>] [network-side-csf-trigger <value>] [network-side-sd-trigger <value>]
[alarm-report-control <value>]
set protection-group-<name> [label <value>] [pg-request <value>] [pg-control-request <value>] [switching-mode <value>] [reversion-mode <value>]
[hold-off-timer <value>] [wtr-timer <value>] [protection-pu <value>] [client-side-olos-trigger <value>] [client-side-sd-trigger <value>]
[network-side-csf-trigger <value>] [network-side-sd-trigger <value>] [alarm-report-control <value>]
show protection-group-<name> [AID] [label] [protection-type] [pg-state] [pg-request] [pg-control-request] [switching-mode] [reversion-mode]
[hold-off-timer] [wtr-timer] [remaining-wtr] [last-switch-trigger] [working-pu] [protection-pu] [reliable-cp] [client-side-olos-trigger]
[client-side-sd-trigger] [network-side-csf-trigger] [network-side-sd-trigger] [switch-failure-reason] [alarm-report-control]
delete protection-group-<name>
```

#### Command Usage Details

**Table 594: protection-group Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 978 -->

#### Command Parameters

**Table 595: protection-group Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the protection-group | string | n/a | add, set, show, delete |
| protection-type | Represents the protection type this PG has. | y-cable, snc-n | y-cable | add, set, show |
| working-pu | The working Protection uUnit (PU) associated with the protection group. | string length 1...32 | n/a | add, set, show |
| protection-pu | The protecting pProtection uUnit associated with the protection group. | string length 1...32 | n/a | add, set, show |
| reliable-cp | The reliable connection point associated with the protection group. | snc-n length 1..32 | n/a | add, set, show |
| label | The user configurable label for the protection-group. | string | n/a | add, set, show |
| pg-request | The management of protection switching action. | • clear<br>• manual-to-working<br>• manual-to-protection<br>• forced-to-working<br>• forced-to-protection,<br>• protection-lockout<br>• exercise (applicable only for bidirectional SNCP) | clear | add, set, show |
| pg-control-request | Protection group control request. | • freeze<br>• clear-freeze | clear-freeze | add,set, show |
| pg-state | Specifies the current state of the protection group. | Applicable for unidirectional and bidirectional SNCP and Y-Cable protection:<br>• no-request<br>• do-not-revert<br>• protection-lockout<br>• frozen<br>• forced-to -working<br>• forced-to -protection<br>• manual-to-working<br>• manual-to-protection<br>• sf-on-working<br>• sf-on -protection<br>• sd-on-working<br>• sd-on-protection<br>• wait-to-restore<br>• unavailable Applicable only for bi-directional SNCP: • protection-lockout-remote<br>• sf-on-working-remote<br>• sf-on-protection -remote<br>• sd-on-working-remote<br>• sd-on -protection-remote<br>• forced-to-working-remote<br>• forced-to -protection remote<br>• manual-to -working-remote<br>• manual-to-protection-remote<br>• wait-to-restore-remote<br>• exercise<br>• exercise-remote | unavailable | show |
| switching-mode | Protection switching mode. | unidirectional, bidirectional | unidirectional | add, set, show |
| reversion-mode | Enable or disable automatic reversion protection status after wtr-time delay. | revertive, non-revertive | non-revertive | add, set, show |
| hold-off-timer | Switching trigger soaking time before switching, measured and set in 1-millisecond steps. | milliseconds range 0...10000 | 0 | add, set, show |
| wtr-timer | Trigger clearance soaking time before reverting to the working protection unit, measured and set in 1-second steps. Only applicable in revertive mode. | seconds range 60..720 | 300 | add, set, show |
| remaining-wtr | Specifies the remaining time in WTR timer. Only applicable in revertive mode. | seconds range 0..720 | n/a | show |
| client-side-olos-trigger | Considers a local client-side RX OLOS defect as a trigger for switch-over. | enabled, disabled | disabled | add, set, show |
| client-side-sd-trigger | Considers a local client-side RX SD defect as a trigger for switch-over. | enabled, disabled | disabled | add, set, show |
| network-side-csf-trigger | Considers a network-side ingress CSF defect as a trigger for switch-over. | enabled, disabled | disabled | add, set, show |
| network-side-sd-trigger | Considers a network-side ingress SD defect as a trigger for switch-over. | enabled, disabled | disabled | add, set, show |
| alarm-report-control | Switch to enable alarm reporting. | allowed, inhibited | inhibited | add, set, show |
| switch-failure-reason | The reason the switch failed | none, request-timer-expiry, request-timer-expiry | none | show |
| last-switch-trigger | Specifies the last reason that triggered a protection switchover. | Applicable for Y-cable protection, unidirectional SNCP and bidirectional SNCP:<br>• clear<br>• manual-to-working<br>• manual-to-protection<br>• forced-to-working<br>• forced-to-protection • lockout<br>• sf-on-working<br>• sf-on-protection<br>• sd-on-working<br>• sd-on-protection<br>• wtr-timer-expiration Applicable only for bidirectional SNCP:<br>• sf-on-working remote<br>• sf-on-protection-remote<br>• sd-on-working-remote<br>• sd-on-protection-remote<br>• protection-lockout-remote<br>• forced-to-working-remote<br>• forced-to-protection-remote<br>• manual-to-working-remote<br>• manual-to-protection-remote<br>• wtr-timer-expiration-remote | clear | show |

#### Examples

This example shows how to add a protection group in 1830 GX G40 environment:

```
add protection-group-test working-pu 1-6-T1 protection-pu 1-6-T2 protection-type y-cable
```

This example shows how to add a protection group in 1830 GX G30 environment:

<!-- page 983 -->

```
add protection-group-pg1016 protection-type snc-n working-pu 201-1-10-ODU2-1 protection-pu 201-1-16-ODU2-1 reliable-cp 201-1-13-ODU2
```

<!-- page 984 -->
