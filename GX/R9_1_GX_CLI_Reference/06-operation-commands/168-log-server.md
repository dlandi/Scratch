---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.168. log-server'
source_lines: 14420-14483
---

## 6.168. log-server

#### Command Description

This command is used to group or show the configuration parameters for log forwarding. Use the delete command to delete a log server from the configuration.

#### Command Syntax

```
add log-server-<name> address <IP address> [destination-facility-override <enabled|disabled>] [enabled <true|false>] [message-coalescence
<true|false>] [message-format <string>] [origin <dhcp|manual>] [pattern-match <string>] port <value> [source-facilities <string>] transport
<udp|tls|tcp> [sensitive-data <only|none|both>] [alarm-report-control <allowed|inhibited>]
set log-server-<name> address <IP address> [destination-facility-override <enabled|disabled>] [enabled <true|false>] [message-coalescence
<true|false>] [message-format <string>] [origin <dhcp|manual>] [pattern-match <string>] port <value> [source-facilities <string>] transport
<udp|tls|tcp> [sensitive-data <only|none|both>] [alarm-report-control <allowed|inhibited>]
show log-server-<name> [address] [transport] [port] [destination-facility-override] [source-facilities] [pattern-match] [message-coalescence]
[enabled] [message-format] [origin] [sensitive-data] [alarm-report-control]
delete log-server-<name>
```

#### Command Usage Details

**Table 425: log-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 426: log-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the syslog server to be configured. | String (length 0...64) | n/a | add, set, show, delete |
| address | Specifies the address of the remote host. | IPv4, IPv6, DNS domain name. | n/a | add, set, show |
| destination-facility-override | Flag indicating whether the destination facility override is enabled. When not disabled, specifies the facility used in messages delivered to the remote server. | disabled, facility number (range: 0..11 \| 16..23) | disabled | add, set, show |
| enabled | Toggles the availability of this syslog server. | true, false | true | add, set, show |
| message-coalescence | If true, prevent flooding of identical messages during abnormal conditions. If there are multiple identical log messages, there will be one message logged fully and follow with 'last message repeated n times' message." | true, false | true | add, set, show |
| message-format | Identifies the syslog messaging format. | • rfc3164<br>• rfc5424 | rfc5424 | add, set, show |
| origin | Log-server address assignment method, user can convert the DHCP configured Log-server entry in to a manual configured by changing this attribute. | • dhcp - Indicates that the Log-server address has been assigned to this system by a DHCP server.<br>• manual - Indicates that the Log-server address has been manually configured. | manual | add, set, show |
| pattern-match | The regular expression pattern for all entries. | String (length: 0..255) | n/a | add, set, show |
| port | Specifies the port number used to deliver messages to the remote server. | Port (range: 1..65535) | 514 | add, set, show |
| source-facilities | List of syslog facilities used in this configuration. | all authentication clock-daemon-15 clock-daemon-9 ftp-daemon kernel line-printer local0 local1 local2 local3 local4 local5 local6 local7 log-alert log-audit mail-system network-news ntp security syslog-internal system-daemons user-level uucp | all | add, set, show |
| transport | It is the transport protocol used when forwarding logs. | tcp, udp, tls | udp | add, set, show |
| sensitive-data | Setting to configure logging sensitive data. | none, only, both | none | add, set, show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |

#### Examples

This example shows how to set the filter such that, all severity logs of "All commands given in a protocol specific format" are sent to server at IP 10.220.225.165:

```
add log-server-1 address '10.220.225.165' source-facilities local7
```

This example shows how to set the filter such that, all severity logs of "All commands given in a protocol specific format" are sent to server at IP 10.220.225.165 logging both sensitive and non-sensitive data:

```
add log-server-1 address '10.220.225.165' source-facilities local7 sensitive-data both
```

<!-- page 653 -->
