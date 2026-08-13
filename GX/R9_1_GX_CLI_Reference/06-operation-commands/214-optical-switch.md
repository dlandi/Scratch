---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.214. optical-switch'
source_lines: 17022-17105
---

## 6.214. optical-switch

#### Command Description

The commands described in this section are used to set or show the `optical-switch` attributes.

#### Command Syntax

```
set optical-switch-<name> [label <value>] [alarm-report-control <value>] [protection-type <value>] [switching-mode <value>]
[reversion-mode <value>] [hold-off-timer <value>] [wtr-timer <value>] [och-center-frequency <value>] [working-switch-threshold <value>]
[protection-switch-threshold <value>] [switch-threshold-enable <value>] [working-los-threshold <value>] [protection-los-threshold <value>]
[facility-los-threshold <value>] [wavelength-band <value>] [los-threshold-hysteresis] [switch-threshold-hysteresis]
show optical-switch-<name> [AID] [label] [oper-state] [avail-state] [alarm-report-control] [supporting-card] [supporting-working-port]
[supporting-protection-port] [protection-type] [pg-state] [active-path] [switch-role] [switching-mode] [reversion-mode] [hold-off-timer]
[wtr-timer] [last-request] [last-switch-trigger] [och-center-frequency] [working-switch-threshold] [protection-switch-threshold]
[working-path-degree] [protection-path-degree] [switch-threshold-enable] [working-los-threshold] [protection-los-threshold]
[facility-los-threshold] [wavelength-band] [los-threshold-hysteresis] [switch-threshold-hysteresis]
```

#### Command Usage Details

**Table 514: optical-switch Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

<!-- page 821 -->

**Table 515: optical-switch Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of optical-switch is the same as the one of the parent OPSM card. For example: if the card AID is 1-3.2 the cli-name is optical-switch-1-3.2. | string | n/a | set, show |
| AID | The AID of optical-switch is the same as the one of the parent OPSM card. | string | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| supporting-card | Displays the parent OPSM Card. | leaf-reference | n/a | show |
| supporting-working-port | Displays the optical-ptp of Working port on the parent OPSM card. | leaf-reference | n/a | show |
| supporting-protection-port | Displays the optical-ptp of the Protection port on the parent OPSM card. | leaf-reference | n/a | show |
| protection-type | Defines the protection-type. The user can configure the protection-type to: not-applicable i Note: The not-applicable protection type is deprecated starting from R7.2. Users can see this option for the OPSM on a node upgraded from R7.1 but the option will not be available for configurations after a new protection type is selected. oms - OMS protection i Note: The oms protection type is supported on the OPSM in C2ILASGH-based FOADM (bidi EDFA terminal) 1830 GX G34c nodes in the current release. och-cs - OCH client-side protection och-ls - OCH line-side protection i Note: The och-ls protection type is supported for the OPSM- PT and planned for the OPSM. multi-channel - multi OCH protection | • not-applicable<br>• oms<br>• och-cs<br>• och-ls<br>• multi-channel | och-cs for OPSM och-ls for OPSM-PT multi-channel for OPSM if ne l0-mode-op is set to slte. | set, show |
| pg-state | Displays the state of the protection. | • no-request<br>• do-not-revert<br>• manual-to-working<br>• manual-to-protection<br>• forced-to-working<br>• forced-to-protection<br>• protection-lockout<br>• sf-on-working<br>• sf-on-protection<br>• sd-on-working sd-on-protection<br>• wait-to-restore<br>• unavailable | unavailable | show |
| active-path | Displays the current active path of the optical-switch. | • working<br>• protection | working | show |
| switch-role | Indication for the cascading/ non-cascading OPSM switch role of the optical-switch:<br>• standalone - Regular protection (2-path protection or any other). | • standalone | standalone | show |
| switching-mode | Defines the switching mode: Unidirectional - Selection based on the local fault conditions and protection commands alone. Only unidirectional is supported. | • unidirectional | unidirectional | set, show |
| reversion-mode | Defines if the WTR-based reversion is enabled on the optical-switch. | • revertive<br>• non-revertive | non-revertive | set, show |
| hold-off-timer | Defines the time the system holds before performing the switch upon SF/SD signal detection. It is represented in milliseconds with a granularity of 1 msecs. | uint16 (range: 0 ... 2440 msec) | 0 | set, show |
| wtr-timer | Defines the WTR timer. This is applicable only when the reversion-mode is revertive. Its is represented in seconds. The granularity is 1 second. | uint16 (range: 0 ... 3600sec) | 300 | set, show |
| last-request | Displays the last user request received on the optical-switch. The external protection commands result in the update of last-request. Upon successful validation, the last-request is applied on the OPSM/OPSM-PT. | • not-applicable<br>• clear<br>• manual-to-working<br>• manual-to-protection<br>• forced-to-working<br>• forced-to-protection<br>• protection-lockout | not-applicable | show |
| last-switch-trigger | Displays the trigger for the last protection switch on the optical-switch | • not-applicable<br>• manual-to-working<br>• manual-to-protection<br>• forced-to-working<br>• forced-to-protection<br>• lockout<br>• sf-on-working<br>• sf-on-protection<br>• sd-on-working<br>• sd-on-protection<br>• wtr<br>• port-lock | not-applicable | show |
| working-path-degree | Displays the degree number of the working path degree. The value of zero denotes that the working path degree is not associated yet. | degree number range | 0 | show |
| protection-path-degree | Displays the degree number of the protection path degree. The value of zero denotes that the protection path degree is not associated yet. | degree number range | 0 | show |
| working-los-threshold | Defines the Signal Fail (SF) threshold for the Working Path. It is represented in dBm. | optical power in the range of -55.0 to 0 dBm. | -23dBm | set, show |
| protection-los-threshold | Defines the Signal Fail (SF) threshold for the Protection Path. It is represented in dBm. | optical power in the range of -55.0 to 0 dBm. | -23dBm | set, show |
| switch-threshold-enable | Enables the protection switching based on SD threshold configured for Working and Protection Paths. | • enabled<br>• disabled | disabled | set, show |
| working-switch-threshold | Defines the Signal Degrade (SD) threshold for the Working Path. It is represented in dBm. | optical power in the range of -55.0 to 0 dBm. | -18dBm | set, show |
| protection-switch-threshold | Defines the Signal Degrade (SD) threshold for the Protection Path. It is represented in dBm. | optical power in the range of -55.0 to 0 dBm. | -18dBm | set, show |
| facility-los-threshold | Defines the threshold of the facility port, power level below it will lead to loss of signal. | optical power in the range of -55 to 15 dBm | -30.0dBm | set, show |
| och-center-frequency | Defines the och center frequency. It is applicable to OPSM-PT only. It is not exposed on the OPSM. | frequency in the range of [190625000 ... 196725000] MHz and 0MHz | 0 | set, show |
| wavelength-band | Defines the wavelength band: o-band (1310) or c-band (1550). | • o-band<br>• c-band | c-band | set, show |
| los-threshold-hysteresis | SF threshold hysteresis (in dB). Applies to both working-switch-threshold and protect-switch-threshold. The recommended configured value for MCHP and OMSP deployments is 1dB. | decimal64 (range 0.5 to 5.0dB, in steps of 0.1dB) (2 fraction digits) | 3dB | set, show |
| switch-threshold-hysteresis | SD threshold hysteresis (in dB). Applies to both working-switch-threshold and protect-switch-threshold. The recommended configured value for MCHP and OMSP deployments is 1dB. | decimal64 (range 0.5 to 5.0dB, in steps of 0.1dB) (2 fraction digits) | 2dB | set, show |

#### Examples

The following example shows how to set the optical switch label:

```
set -f optical-switch-1-1.1 label infinera_swich
```

The following example shows how to set the optical switch **protection-type** to *oms*:

```
set optical-switch-5-5.2 protection-type oms
```

<!-- page 829 -->
