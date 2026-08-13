---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.230. otu-diagnostics'
source_lines: 18046-18145
---

## 6.230. otu-diagnostics

#### Command Description

These commands are used to set or show the attributes associated with OTU diagnostics. Each direction has its own values.

#### Command Syntax

```
set otu-diagnostics-<name>/<direction> [monitoring-mode <value>] [tti-style <value>] [tti-mismatch-alarm-reporting <value>] [tx-tti <value>]
[expected-tti <value>] [expected-sapi <value>] [expected-dapi <value>] [expected-operator <value>] [tx-sapi <value>] [tx-dapi <value>]
[tx-operator <value>] [tim-act-enabled <value>] [degrade-interval <value>] [degrade-threshold <value>]
show otu-diagnostics-<name>/<direction> [monitoring-mode] [tti-style] [tti-mismatch-alarm-reporting] [tx-tti] [rx-tti] [rx-tti-hex]
[expected-tti] [expected-sapi] [expected-dapi] [expected-operator] [tx-sapi] [tx-dapi] [tx-operator] [rx-sapi] [rx-sapi-hex] [rx-dapi]
[rx-dapi-hex] [rx-operator] [rx-operator-hex] [tim-act-enabled] [degrade-interval] [degrade-threshold]
```

<!-- page 905 -->

**Note:** There are two possible methods to manage the TTIs:

1. proprietary

proprietary method uses the following parameters:

    - tx-tti
    - rx-tti
    - rx-tti-hex
    - expected-tti 2. ITU-based ITU based method uses the following parameters:
    - expected-sapi
    - expected-dapi
    - expected-operator
    - tx-sapi
    - tx-dapi
    - tx-operator
    - rx-sapi
    - rx-dapi
    - rx-operator

All of the above listed parameters are mutually exclusive; the proprietary TTIs display the data as a single buffer, whereas the ITU based one splits it by SAPI/DAPI/operator categories.

#### Command Usage Details

**Table 549: otu-diagnostics Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 906 -->

#### Command Parameters

**Table 550: otu-diagnostics Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| direction | Diagnostics direction. Can be ingress or egress. | ingress, egress | n/a | add, set, show |
| monitoring-mode | The monitoring mode on the ODU/OTU client. | unused intrusive non-intrusive limited-intrusive limited-non-intrusive | intrusive | add, set, show |
| tti-style | The configured mode of the TTI for this OTU/ODU client:<br>• ITU-T-G709: TTI is split into SAPI, DAPI and OPER bytes.<br>• proprietary: TTI is a single 64 byte string. | ITU-T-G709 proprietary | ITU-T-G709 | add, set, show |
| tti-mismatch-alarm-reporting | Indicates if TTI-Mismatch (TIM) alarm is reported or masked. If it is to be reported, indicates the criteria based on with the TIM alarm is reported. | disabled full-64-bytes SAPI DAPI OPER SAPI DAPI _ SAPI OPER _ DAPI OPER _ SAPI DAPI OPER _ _ | disabled | add, set, show |
| tx-tti | Transmit TTI - Sent by this facility to the far-end remote facility. Can be a hexadecimal string starting with '0x' or printable ASCII string with size 64. | • Hex string with '0x' prefix (length 1..130) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..64). Restricted to printable ASCII. | "" | add, set, show |
| rx-tti | Received TTI - Received by this facility from the far-end remote facility. | string (length 1..64). Restricted to printable ASCII. | n/a | show |
| rx-tti-hex | Received TTI in HEX. | Hex string with '0x' prefix (length 1..130) (pattern '(0x(([0-9A-Fa-f])([0-9A-Fa-f]))*)?') | n/a | show |
| expected-tti | Expected TTI - The TTI this facility expects to receive from the far-end remote facility. | • Hex string with '0x' prefix (length 1..130) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..64). Restricted to printable ASCII. | "" | add, set, show |
| expected-sapi | The expected SAPI (Source Access Point Identifier). | • Hex string with '0x' prefix (length 1..32) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..15). Restricted to printable ASCII. | "" | add, set, show |
| expected-dapi | The expected DAPI (Destination Access Point Identifier). | • Hex string with '0x' prefix (length 1..32) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..15). Restricted to printable ASCII. | "" | add, set, show |
| expected-operator | The expected operator specific bytes. | • Hex string with '0x' prefix (length 1..66) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..32). Restricted to printable ASCII. | "" | add, set, show |
| tx-sapi | The transmitted SAPI bytes. | Hex string with '0x' prefix (length 1..32). String with "[ -~]*" (length 1..15). Restricted to printable ASCII. • Hex string with '0x' prefix (length 1..32) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..15). Restricted to printable ASCII. | "" | add, set, show |
| tx-dapi | The transmitted DAPI bytes. | • Hex string with '0x' prefix (length 1..32) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..15). Restricted to printable ASCII. | "" | add, set, show |
| tx-operator | The transmitted operator specific bytes. | • Hex string with '0x' prefix (length 1..66) (pattern '(0x(([0-9A-Fa-f]) ([0-9A-Fa-f]))*)?')<br>• string (length 1..32). Restricted to printable ASCII. | "" | add, set, show |
| rx-sapi | The received SAPI bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. | String with "[ -~]*" (length 1..15). | n/a | show |
| rx-sapi-hex | Received SAPI in HEX. | Hex string with '0x' prefix (length 1..32). | n/a | show |
| rx-dapi | The received DAPI bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. | string (length 1..15). | n/a | show |
| rx-dapi-hex | Received DAPI in HEX. | Hex string with '0x' prefix (length 1..32). | n/a | show |
| rx-operator | The received operation specific bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. | string (length 1..32). | n/a | show |
| rx-operator-hex | Received operator in HEX. | Hex string with '0x' prefix (length 1..66). | n/a | show |
| tim-act-enabled | Support configurable TIM action which decides if insert maintenance signal per TIM: enabled or disabled. By default is disabled. | enabled, disabled | disabled | add, set, show |
| degrade-interval | The consecutive number of 1s intervals with the number of detected block errors exceeding the block error threshold for each of those seconds for the purposes of SDBER detection. | uint8 (range: 2..10 seconds) | 7 | add, set, show |
| degrade-threshold | The threshold in percentage of block errors versus total blocks at which a degrade-interval number of seconds will be considered degraded for the purposes of SDBER detection. | uint8 (range: 0 .. 100%) | 30% | show |

#### Examples

This example shows how to set up OTU diagnostics in a 1830 GX G30 node:

```
set otu-diagnostics-1-1-1-OTUC1/ingress tti-mismatch-alarm-reporting SAPI
```

This example shows how to set up OTU diagnostics in a 1830 GX G40 node:

<!-- page 911 -->

```
set otu-diagnostics-1-4-L1-1-OTUCni/ingress tti-mismatch-alarm-reporting SAPI
```

<!-- page 912 -->
