---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.231. oxcon'
source_lines: 18146-18223
---

## 6.231. oxcon

#### Command Description

These commands are used to add, delete the Optical Cross Connection (OXcon), and set or show the OXcon attributes.

#### Command Syntax

```
add oxcon-<name> source <value> destination <value> [label <value>] [activation-mode <value>] [auto-delete <value>] [activation-request-fwd
<value>] [activation-request-bwd <value>] [direction <value>] [target-output-power-src <value>] [target-output-power-dst <value>] [circuit-id
<value>]
set oxcon-<name> [label <value>] [auto-delete <value>] [activation-request-fwd <value>] [activation-request-bwd <value>] [target-output-power-src
<value>] [target-output-power-dst <value>] [circuit-id <value>]
show oxcon-<name> [AID] [label] [oper-state] [avail-state] [managed-by] [activation-mode] [auto-recovery-state] [auto-delete]
[activation-request-fwd] [activation-request-bwd] [activation-state-fwd] [activation-state-bwd] [source] [destination] [direction]
[monitored] [target-output-power-src] [target-output-power-dst] [target-actual-power-dst] [target-actual-power-src] [target-actual-psd-dst]
[target-actual-psd-src] [circuit-id]
delete oxcon-<name>
```

#### Command Usage Details

**Table 551: oxcon Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

<!-- page 913 -->

**Table 552: oxcon Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | set, show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64) | n/a | show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby. | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. | system, user | system | show |
| activation-mode | OXcon activation mode:<br>• automatic - The service is activated automatically by the system on creation. Similarly, service is deactivated automatically on deletion. This mode is supported in the standard L0 mode of operation.<br>• manual - The service activation and deactivation are controlled manually through activation-req-fwd/bwd settings. This mode is supported in the SLTE L0 mode of operation.<br>• activate-on-create - The service activation and deactivation are controlled manually through activation-request-fwd/bwd settings. This mode is supported in the HSC OLS L0 mode of operation. | • automatic<br>• manual<br>• activate-on-create | • automatic, for standard mode<br>• manual, for SLTE mode<br>• activate-on-create, for HSC OLS mode | add, show |
| auto-recovery-state | Only of relevance for SLTE applications. It displays the auto recovery state:<br>• not-applicable - the ase-insertion-enable is disabled, or in terrestrial mode.<br>• active - valid reference trace is available, toggling limit not reached.<br>• failed - toggling limit exceeded or valid reference trace not available. Auto-channel recovery is deactivated.<br>• not-available - the reference trace is not yet available.<br>• waiting-for-reference - the reference power is not yet available<br>• paused - Paused between toggling limits. | • not-applicable<br>• active<br>• failed<br>• not-available<br>• waiting-for-reference<br>• paused | not-applicable | show |
| auto-delete | When enabled, the system may auto-delete this OXcon once activation intent and states are changed to deactivated. When disabled, the OXcon stays until explicitly deleted.<br>• disabled : The auto-delete feature is disabled on MC.<br>• enabled : When auto-delete is enabled, the system automatically deletes MC when NMC associated with it is deleted. This attribute is allowed to be enabled on RD66TM sleds (l0-mode-op=hsc-ols). This attribute is always disabled if l0-mode-op is standard or slte. | • disabled<br>• enabled | disabled | add, set, show |
| activation-request-fwd | Activation request for the forward direction (source to destination). This attribute is applicable only when activation-mode is manual:<br>• no-request - This is the default request type on creation. This causes the system to remain in the deactivated state. This cannot be set by the user subsequently.<br>• activate - Request to activate the service on the local node in the given direction. • deactivate - Request to deactivate the service on the local node in the given direction. This attribute is only of relevance for SLTE and HSC OLS applications. | • no-request<br>• activate<br>• deactivate | • no-request<br>• activate, for HSC OLS L0 mode of operation. | add, set, show |
| activation-request-bwd | Activation request for the backward direction (destination to source). This attribute is applicable only when activation-mode is manual:<br>• no-request - This is the default request type on creation. This causes the system to remain in the deactivated state. This cannot be set by the user subsequently.<br>• activate - Request to activate the service on the local node in the given direction.<br>• deactivate - Request to deactivate the service on the local node in the given direction. This attribute is only of relevance for SLTE and HSC OLS applications. | • no-request<br>• activate<br>• deactivate | • no-request<br>• activate, for HSC OLS L0 mode of operation. | add, set, show |
| activation-state-fwd | Activation state of the forward direction (source to destination). This attribute is applicable only when activation-mode is manual:<br>• not-applicable - The attribute is not applicable in automatic mode.<br>• activated - In activated state.<br>• partially-activated - The service is partially activated in the given direction.<br>• faulted - The service is faulted in the given direction.<br>• deactivated - The service is deactivated in the given direction. This attribute is only of relevance for SLTE and HSC OLS applications. | • not-applicable<br>• activated<br>• partially-activated<br>• faulted<br>• deactivated | • not-applicable, for standard mode<br>• deactivated, for SLTE and HSC OLS mode | show |
| activation-state-bwd | Activation state of the backward direction (destination to source). This attribute is applicable only when activation-mode is manual:<br>• not-applicable - The attribute is not applicable in automatic mode.<br>• activated - In activated state. • partially-activated - The service is partially activated in the given direction.<br>• faulted - The service is faulted in the given direction.<br>• deactivated - The service is deactivated in the given direction. This attribute is only of relevance for SLTE and HSC OLS applications. | • not-applicable<br>• activated<br>• partially-activated<br>• faulted<br>• deactivated | • not-applicable, for standard mode<br>• deactivated, for SLTE and HSC OLS mode | show |
| source | The source end-point required for OXcon creation. | Instance ID | n/a | add, show |
| destination | The destination end-point required for OXcon creation. | Instance ID | n/a | add, show |
| direction | Indicates whether the OXcon is unidirectional (one-way) or bi-directional (two-way). | • two-way<br>• one-way | two-way | add, show |
| monitored | Monitoring/ not-monitored indication; does not change during OXcon lifetime. | true, false | true | show |
| target-output-power-src | The source interface target power. | decimal64 with 2 fraction-digits (range: -18.00..15.00dBm) | • -5 dBm, beginning with R6.0.2, if upon Add/Drop OXcon creation the OXcon source is the Tributary NMC and the OXcon destination is the RD20TM/RD09SM NMC. • 0 dBm, for disaggregated RD09SM-based ROADM with CD-AD, upon OXcon creation between the NMC of the WS04S ADE port and the NMC of the CAD16A Tributary port.<br>• 0 dBm, for the remaining cases. | add, set, show |
| target-output-power-dst | The destination interface target power. | decimal64 with 2 fraction-digits (range: -18.00..15.00dBm) | • -5 dBm, beginning with R6.0.2, if upon Add/Drop OXcon creation the OXcon source is the RD20TM/RD09SM NMC and the OXcon destination is the Tributary NMC.<br>• 0 dBm, for disaggregated RD09SM-based ROADM with CD-AD, upon OXcon creation between the NMC of the WS04S ADE port and the NMC of the CAD16A Tributary port.<br>• 0 dBm, for the remaining cases. | add, set, show |
| target-actual-power-dst | Value as calculated by Power Control if target-power-setting is set to auto. Otherwise it is the exact value configured at target-output-power-dst/ src.", | decimal64 with 2 fraction-digits (range: -99.00..99.00 dBm) | -99 | show |
| target-actual-power-src | Value as calculated by Power Control if target-power-setting is set to auto. Otherwise it is the exact value configured at target-output-power-dst/ src.", | decimal64 with 2 fraction-digits (range: -99.00..99.00 dBm) | -99 | show |
| target-actual-psd-dst | Actual PSD destination. | • not-applicable - Not Applicable/ Not specified/ Unknown<br>• decimal64 with 2 fraction-digits in nW/GHz | not-applicable | show |
| target-actual-psd-src | Actual PSD source. | • not-applicable - Not Applicable/ Not specified/ Unknown<br>• decimal64 with 2 fraction-digits in nW/GHz | not-applicable | show |
| circuit-id | Path/ service name of optical cross-connection. | String (length 0..128 characters) | not-applicable | add, set, show |

#### Examples

This example shows how to create the OXcon without the target output power values:

```
add oxcon-1-80-192262500__1-3.1-dwdm-line-192262500 source 'nmc-1-80-192262500' destination 'nmc-1-3.1-dwdm-line-192262500'
```

This example shows how to create the OXcon with the target output power values:

```
add oxcon-1 source nmc-RD20-1-1_192800000-34000 destination nmc-cdc-1-4_192800000-34000 target-output-power-src 1 target-output-power-dst 1
```

<!-- page 922 -->
