---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.99. ethernet'
source_lines: 10495-10594
---

## 6.99. ethernet

#### Command Description

These commands are used to set/show ethernet facility attributes.

#### Command Syntax

```
set ethernet-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>] [max-packet-length <value>] [fec-mode <value>]
[tx-mapping-mode <value>] [expected-mapping-mode <value>] [time-slots <value>] [line-port <value>] [loopback <value>] [loopback-mode
<value>] [fec-degraded-ser-monitoring <value>] [fec-degraded-ser-activate-threshold <value>] [fec-degraded-ser-deactivate-threshold
<value>] [fec-degraded-ser-monitoring-period <value>] [timing-mode <value>] [test-signal-type <value>] [test-signal-direction <value>]
[test-signal-monitoring <value>] [transmit-inter-packet-gap <value>] [gfp-payload-fcs <value>] [upi-value <value>] [lldp-admin-status <value>]
[lldp-ingress-mode <value>] [lldp-egress-mode <value>]
show ethernet-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [alarm-report-control] [client-type] [service-mode] [service-mode-qualifier] [max-packet-length] [speed] [fec-ability]
[fec-mode] [tx-mapping-mode] [expected-mapping-mode] [time-slots] [line-port] [loopback] [loopback-mode] [fec-degraded-ser-monitoring]
[fec-degraded-ser-activate-threshold] [fec-degraded-ser-deactivate-threshold] [fec-degraded-ser-monitoring-period] [timing-mode]
[test-signal-type] [test-signal-direction] [test-signal-monitoring] [transmit-inter-packet-gap] [gfp-payload-fcs] [upi-value] [lldp-admin-status]
[lldp-ingress-mode] [lldp-egress-mode] [circuit-id]
```

#### Command Usage Details

**Table 282: ethernet Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 462 -->

#### Command Parameters

**Table 283: ethernet Command Flags**

| Parameter | Description |
| --- | --- |
| -m | Merge configuration (the command will not fail if the entity already exists) |

**Table 284: ethernet Command Parameters**

| Parameter | Description | Values | Default |  |
| --- | --- | --- | --- | --- |
| name | The name of the ethernet facility. | String (length 1..64 characters) with pattern '([A-Za-z0-9 -.,]*)' _ | n/a | set, show |
| supporting-card | Card that holds this facility. | card | n/a | show |
| supporting-port | Ports that hold this facility. | port | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | Instance identifier | n/a | show |
| supported-facilities | An XPath reference to the children facilities. | Instance identifier | n/a | show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | String (length 1..64 characters) | n/a | show |
| timing-mode | Indicates the timing mode of the ethernet client. This attribute is applicable to 1830 GX G40 only. | retimed, transparent | transparent | set |
| label | User defined label. | String (length 0..256 characters) | n/a | set, show |
| admin-state | The administrative state of the managed object. | lock unlock maintenance | unlock | set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| managed-by | Describes whether this facility was system created or not. | system, user | system | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed - Alarm reporting is allowed. inhibited - Alarm reporting is inhibited. | allowed | set, show |
| client-type | The protocol type of the Ethernet client. | string type identityref (base ETHERNET-CLIENT) | n/a | show |
| service-mode | Service mode for the ethernet facility: none network-wrapper - Map non- OTN signal into ODUs. adaptation - Multiplexing scenarios. switching - Map OTN signal (e.g. OTU) into ODUs. transport -Transport OTN signal (e.g. OTU) into line side ODUs. | none network-wrapper adaptation switching transport | transport | show |
| service-mode-qualifier | Service mode qualifier for the ethernet facility. | none mux-demux nofec | none | show |
| max-packet-length | Maximum transfer unit for ethernet facility, in octets. | Integer (1830 GX G30 range: 1280..18000 octects; 1830 GX G40 range: 1518..18000 octects) | 1518 | set, show |
| i Note:<br>• max-packet-length parameter is configurable on card level only and applies on all interface in that card.<br>• max-packet-length parameter is used only for determining the undersized/ oversized packet count in 100Gbe PMs. |  |  |  |  |
| speed | The speed/rate of the Ethernet client interfaces (Gbit/s). | Integer | n/a | show |
| fec-ability | Indicates the Ethernet client's capability to support FEC (Forward Error Correction). | supported not-supported | supported | show |
| fec-mode | The configured FEC mode on the Ethernet client. Default is dependent on configured client type. | disabled enabled | disabled | set, show |
| tx-mapping-mode | The tx mapping mode of client port. The possible values are dependent on the HW and configuration: GMP - Generic Mapping Procedure. BMP - BMP mapping openZR+ - mapping mode for ZR FlexE-4x100G - FlexE-4x100G for split lamda feature GFP-F GFP-F-extOPU2 - GFP-F-extOPU2 AMP | GMP BMP openZR+ FlexE-4x100G GFP-F GFP-F-extOPU2 AMP | GMP | set, show |
| expected-mapping-mode | The expected mapping mode of client port. The possible values are dependent on the HW and configuration: GMP - Generic Mapping Procedure. BMP - BMP mapping openZR+ - mapping mode for ZR FlexE-4x100G - FlexE-4x100G for split lamda feature GFP-F GFP-F-extOPU2 - GFP-F-extOPU2 AMP | GMP BMP openZR+ FlexE-4x100G GFP-F GFP-F-extOPU2 AMP | GMP | set, show |
| time-slots | Time slots of the ethernet (when tx-mapping-mode = 'openZR+'). | String (length 0..255 characters; pattern '(([0-9]+(..[0-9]+)?) (,([0-9]+(..[0-9]+)?))*)?') | n/a | set, show |
| line-port | Specify the line port for the client. Can only be configured when mapping mode is openZR+. | leafref (path "../../../equipment/card/port/name") | n/a | set, show |
| loopback | Loopback mode used to debug on the fiber connection:<br>• facility : Test towards facility side.<br>• none: Connection is not being tested.<br>• terminal: Test towards terminal side. | none facility terminal | none | set, show |
| loopback-mode | Indicates loopback action for facility or terminal. | loopback loopback-and-continue | n/a | set, show |
| fec-degraded-ser-monitoring | Allows to enable monitoring for FEC-DEGRADED-SER alarm. | disabled enabled | disabled | set, show |
| fec-degraded-ser-activate-threshold | FEC-DEGRADED-SER alarm asserted if average SER, computed over accumulated FEC symbol errors in the monitoring period exceed this threshold. | Integer (G30 range: 0.0000000001..0.0001 averageSER; 1830 GX G40 range: 0.00000000008..0.00008 averageSER) | 0.00001 | set, show |
| fec-degraded-ser-deactivate-threshold | FEC-DEGRADED-SER alarm cleared if average SER, computed over accumulated FEC symbol errors in the monitoring period is below this threshold. | Integer (range: 0.00000000008..0.00008 averageSER) | 0.000008 | set, show |
| fec-degraded-ser-monitoring-period | Monitoring period duration over which FEC symbol errors are accumulated for asserting or clearing of FEC- DEGRADED-SER alarm. | Integer number (uint8) (range: 1..50 seconds) | 10 | set, show |
| test-signal-type | The type of test pattern that is injected: none: Indicates that test pattern generation is disabled. PRBS31Q: Defined in G.709 OPU PRBS with inverted PN31. PRBS13Q: Defined in G.709 OPU PRBS with inverted PN13. scrambled-idles: Idle frame defined in 802.3 Clause 82.2.10. PRBS9: Defined in G.709 OPU PRBS with non-inverted PN9. PRBS31: Defined in G.709 OPU PRBS with inverted PN31. PRBS31 NONINV: Defined _ in G.709 OPU PRBS with non-inverted PN31. | none PRBS31Q PRBS13Q scrambled-idles PRBS9 PRBS31 PRBS31 NONINV _ | none | set, show |
| test-signal-direction | The direction of the test signal. | ingress, egress, both | egress | set, show |
| test-signal-monitoring | Monitor the incoming test signals for diagnostics. | true, false | false | set, show |
| lldp-admin-status | LLDP operational mode for this port. tx-only: LLDP agent transmits LLDP frames on this port but it does not store connected remote system information. rx-only: LLDP agent receives, but it does not transmit LLDP frames on this port. tx-and-rx: LLDP agent transmits and receives LLDP frames on this port. | tx-only rx-only tx-and-rx disabled | disabled | set, show |
| lldp-ingress-mode | If lldp enabled, define what is the LLDP behavior for this direction. | disabled snoop drop snoop-and-drop | disabled | set, show |
| lldp-egress-mode | If lldp enabled, define what is the LLDP behavior for this direction. | disabled snoop drop snoop-and-drop | disabled | set, show |
| circuit-id | System configured circuit ID. | string (0..128) | none | show |

#### Examples

This example shows how to set the lldp-admin-status to lldp-ingress-mode on an ethernet facility in G30 environment:

```
show ethernet-ethernet-1-1-3 lldp-admin-status lldp-ingress-mode
```

This example shows how to set the lldp-admin-status, lldp-ingress-mode and lldp-egress-mode attributes on an ethernet facility in G40 environment:

```
set ethernet-1-6-T3 lldp-admin-status rx-only lldp-ingress-mode snoop lldp-egress-mode snoop
```

<!-- page 471 -->
