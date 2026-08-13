---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.208. odu-diagnostics'
source_lines: 16581-16658
---

## 6.208. odu-diagnostics

#### Command Description

These commands are used to add, set, show or delete a set of attributes associated with ODU diagnostics. Each direction has its own values.

#### Command Syntax

```
add odu-diagnostics-<name>/<direction> [monitoring-mode <value>] [tti-style <value>] [tti-mismatch-alarm-reporting <value>] [tx-tti
<value>] [expected-tti <value>] [expected-sapi <value>] [expected-dapi <value>] [expected-operator <value>] [tx-sapi <value>] [tx-dapi
<value>] [tx-operator <value>] [tim-act-enabled <value>] [degrade-interval <value>] [degrade-threshold <value>] [test-signal-type <value>]
[test-signal-direction <value>] [test-signal-monitoring <value>]
set odu-diagnostics-<name>/<direction> [monitoring-mode <value>] [tti-style <value>] [tti-mismatch-alarm-reporting <value>] [tx-tti
<value>] [expected-tti <value>] [expected-sapi <value>] [expected-dapi <value>] [expected-operator <value>] [tx-sapi <value>] [tx-dapi
<value>] [tx-operator <value>] [tim-act-enabled <value>] [degrade-interval <value>] [degrade-threshold <value>] [test-signal-type <value>]
[test-signal-direction <value>] [test-signal-monitoring <value>]
show odu-diagnostics-<name>/<direction> [monitoring-mode] [tti-style] [tti-mismatch-alarm-reporting] [tx-tti] [rx-tti] [rx-tti-hex]
[expected-tti] [expected-sapi] [expected-dapi] [expected-operator] [tx-sapi] [tx-dapi] [tx-operator] [rx-sapi] [rx-sapi-hex] [rx-dapi]
[rx-dapi-hex] [rx-operator] [rx-operator-hex] [tim-act-enabled] [degrade-interval] [degrade-threshold] [test-signal-type] [test-signal-direction]
[test-signal-monitoring]
delete odu-diagnostics-<name>/<direction>
```

#### Command Usage Details

**Table 502: odu-diagnostics Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 772 -->

#### Command Parameters

**Table 503: odu-diagnostics Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the facility. | String (length 0..64) | n/a | add, set, show, delete |
| direction | Diagnostics direction. Can be ingress or egress. | ingress, egress | n/a | add, set, show |
| monitoring-mode | The monitoring mode on the ODU/OTU client. | unused intrusive non-intrusive limited-intrusive limited-non-intrusive | intrusive | add, set, show |
| tti-style | The configured mode of the TTI for this OTU/ODU client. ITU-T-G709: TTI is split into SAPI, DAPI and OPER bytes. proprietary: TTI is a single 64 byte string. | ITU-T-G709 proprietary | ITU-T-G709 | add, set, show |
| tti-mismatch-alarm-reporting | Indicates if TTI-Mismatch (TIM) alarm is reported or masked. If it is to be reported, indicates the criteria based on with the TIM alarm is reported. | disabled full-64-bytes SAPI DAPI OPER SAPI DAPI _ SAPI OPER _ DAPI OPER _ SAPI DAPI OPER _ _ | disabled | add, set, show |
| tx-tti | Transmit TTI - Sent by this facility to the far-end remote facility. | tti-64 Hex string with '0x' prefix (length 1..130 characters). String with "[ -~]*" (length 1..64 characters). Restricted to printable ASCII. | n/a | add, set, show |
| rx-tti | Received TTI - Received by this facility from the far-end remote facility. | string (length 0..64 characters) | n/a | show |
| rx-tti-hex | Received TTI in HEX. | string (length 0..130 characters) | n/a | show |
| expected-tti | Expected TTI - The TTI this facility expects to receive from the far-end remote facility. | Hex string with '0x' prefix (length 1..130 characters). String with "[ -~]*" (length 1..64 characters). Restricted to printable ASCII. | "" | add, set, show |
| expected-sapi | The expected SAPI (Source Access Point Identifier). | tti-15 Hex string with '0x' prefix (length 1..32). String with "[ -~]*" (length 1..15). Restricted to printable ASCII. | "" | add, set, show |
| expected-dapi | The expected DAPI (Destination Access Point Identifier). | tti-15 Hex string with '0x' prefix (length 1..32 characters). String with "[ -~]*" (length 1..15 characters). Restricted to printable ASCII. | "" | add, set, show |
| expected-operator | The expected operator specific bytes. | tti-32 Hex string with '0x' prefix (length 1..66 characters). String with "[ -~]*" (length 1..32 characters). Restricted to printable ASCII. | "" | add, set, show |
| tx-sapi | The transmitted SAPI bytes. | tti-15 Hex string with '0x' prefix (length 1..32 characters). String with "[ -~]*" (length 1..15 characters). Restricted to printable ASCII. | "" | add, set, show |
| tx-dapi | The transmitted DAPI bytes. | tti-15 Hex string with '0x' prefix (length 1..32 characters). String with "[ -~]*" (length 1..15 characters). Restricted to printable ASCII. | "" | add, set, show |
| tx-operator | The transmitted operator specific bytes. | tti-32 Hex string with '0x' prefix (length 1..66 characters). String with "[ -~]*" (length 1..32 characters). Restricted to printable ASCII. | n/a | add, set, show |
| rx-sapi | The received SAPI bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. | String with "[ -~]*" (length 0..15 characters). | n/a | show |
| rx-sapi-hex | Received SAPI in HEX. | Hex string with '0x' prefix (length 0..32 characters). | n/a | show |
| rx-dapi | The received DAPI bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. | String with "[ -~]*" (length 0..15 characters). | n/a | show |
| rx-dapi-hex | Received DAPI in HEX. | Hex string with '0x' prefix (length 0..32 characters). | n/a | show |
| rx-operator | The received operation specific bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. | String with "[ -~]*" (length 0..32 characters). | n/a | show |
| rx-operator-hex | Received operator in HEX. | Hex string with '0x' prefix (length 0..66 characters). | n/a | show |
| tim-act-enabled | Support configurable TIM action which decides if insert maintenance signal per TIM: enable or disable, default is disable. | enabled, disabled | disabled | add, set, show |
| degrade-interval | The consecutive number of 1s intervals with the number of detected block errors exceeding the block error threshold for each of those seconds for the purposes of SDBER detection. | Number (range: 2..10 seconds) | 7 | add, set, show |
| degrade-threshold | The threshold in percentage of block errors versus total blocks at which a degrade-interval number of seconds will be considered degraded for the purposes of SDBER detection. | percentage (range: 0 .. 100%) | 30 | show |
| test-signal-type | The type of test pattern that is injected:<br>• none - Indicates that test pattern generation is disabled.<br>• PRBS31Q - Defined in G.709 OPU PRBS with inverted PN31. • PRBS13Q - Defined in G.709 OPU PRBS with inverted PN13.<br>• scrambled-idles - Idle frame defined in 802.3 Clause 82.2.10.<br>• PRBS9 - Defined in G.709 OPU PRBS with non-inverted PN9. PRBS31 - Defined in G.709 OPU PRBS with inverted PN31. PRBS31 NONINV - Defined in G.709 OPU PRBS with _ non-inverted PN31. | none PRBS31Q PRBS13Q scrambled-idles PRBS9 PRBS31 PRBS31 NONINV _ | none | add, set, show |
| test-signal-direction | The direction of the test signal. | ingress, egress, both | ingress | add, set, show |
| test-signal-monitoring | Monitor the incoming test signals for diagnostics. | true, false | false | add, set, show |

#### Examples

This example shows how to set up odu diagnostics in a 1830 GX G40 node:

```
set odu-diagnostics-Lodu4i/ingress tti-mismatch-alarm-reporting SAPIDAPI-OPER
```

<!-- page 777 -->
