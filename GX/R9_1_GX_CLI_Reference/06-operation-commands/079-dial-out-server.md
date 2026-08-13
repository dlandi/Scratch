---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.79. dial-out-server'
source_lines: 9012-9100
---

## 6.79. dial-out-server

#### Command Description

These commands are used to add/edit or show the dial-out-server. The delete command is used to delete a configured dial-out-server.

#### Command Syntax

```
add dial-out-server-<name> address <value> [protocol <value>] [port <value>] [retry-policy <value>] [retry <value>] [timeout <value>]
[alarm-report-control <value>] [label <value>] [auto-connect <value>]
delete dial-out-server-<name>
set dial-out-server-<name> [address <value>] [protocol <value>] [port <value>] [retry-policy <value>] [retry <value>] [timeout <value>]
[alarm-report-control <value>] [label <value>] [auto-connect <value>]
show dial-out-server-<name> [address] [protocol] [port] [transport] [retry-policy] [retry] [timeout] [alarm-report-control] [label]
[auto-connect] [connection-state]
```

#### Command Usage Details

**Table 240: dial-out-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 241: dial-out-server Command Parameters**

| Parameter | Description | Values | Default | used in |
| --- | --- | --- | --- | --- |
| name | The name of the dial-out server. | String (length 1..64) | n/a | add, set, delete, show |
| address | Dial-out-server IPv4/IPv6 address or hostname. | IPv4/IPv6 address. | n/a | add, set, show |
| protocol | Dial-out-server session protocol to use. | netconf, restconf | netconf | add, set, show |
| port | Dial-out-server session port to use. Uses default of 4334 for netconf protocol over ssh. | Integer (range 1..65535) | 4334 | add, set, show |
| transport | Dial-out-server transport protocol. | ssh | n/a | show |
| retry-policy | The retry policy after a timeout. | • progressive-back-off<br>• retry-then-stop<br>• retry-forever | progressive-back-off | add, set, show |
| retry | The number of retries-only applicable when retry-policy is retry-then-stop. | Integer (range 0..5) | 3 | add, set, show |
| timeout | Timeout before next retry. Not applicable for progressive-back-off retry-policy (which has a dynamic timeout). | Integer (range 1..255 seconds) | 10 | add, set, show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | inhibited | add, set, show |
| label | User definable label | String (length 1..256) | n/a | add, set, show |
| auto-connect | Defines if the system automatically connects to this server or not. If true, it automatically tries to connect to this dial-out-server. If false, it can still be connected manually via the call-home RPC. | true, false | true | add, set, show |
| connection-state | Connection state to the dial-out-server. | connected - Session is currently established with 'home'. connecting - Running through the retries; also used if connected, and session abruptly is terminated. failed - All retries have failed, no further attempts are being done to connect to it. closed - session was established, and was gracefully closed. disabled - Enabled parameter is false. | disabled | show |

#### Examples

These examples show how to set attributes of a dial-out server:

```
set dial-out-server-callhome1 auto-connect false retry-policy retry-forever protocol netconf timeout 100
set dial-out-server-callhome1 auto-connect false retry-policy retry-then-stop port 4889 timeout 200 retry 5
```

These examples shows how to add a dial-out server:

```
add dial-out-server-callhome1 address 10.220.87.52
add dial-out-server-callhome1 address 10.220.87.52 port 4337
```

This example shows how to delete a dial-out server:

<!-- page 396 -->

```
delete dial-out-server-callhome1
```

This example shows how to display the configuration of the dial-out server:

```
show dial-out-server
dial-out-server            address       protocol  port  transport                 retry-policy     retry  timeout (seconds)  label
 auto-connect  connection-state
-------------------------  ------------  --------  ----  ------------------------  ---------------  -----  -----------------  -----
 ------------  ----------------
dial-out-server-callhome1  10.220.87.22  netconf   4334  ssh                       retry-then-stop  3      255                       true
    connected
```

This example shows how to initiate a call:

```
call-home callhome1
```

<!-- page 397 -->
