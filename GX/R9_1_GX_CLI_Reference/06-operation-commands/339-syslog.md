---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.339. syslog'
source_lines: 25564-25667
---

## 6.339. syslog

#### Command Description

These commands are used to set or show the configuration for logging functionality via syslog. Includes control of local log files, remote logging configuration and logging in serial console.

#### Command Syntax

```
set syslog [remote-logging-switch <value>] [source-address <value>] [log-file-message-coalescence <value>] [alarm-report-control <value>] [label
<value>] [log-relay <value>] [assignment-method <value>] [niap-compliant-logging <value>] [privacy-mode <value>]
show syslog [remote-logging-switch] [source-address] [log-file-message-coalescence] [alarm-report-control] [label] [log-relay]
[assignment-method] [niap-compliant-logging] [privacy-mode]
```

#### Command Usage Details

**Table 777: syslog Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 778: syslog Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| remote-logging-switch | Flag to enable remote logging switch. If false, disable all remote logging destinations. | true, false | true | set, show |
| source-address | Source address or hostname to inserted in HOST field of log message. | IPv4, IPv6, DNS domain name. | localhost | set, show |
| log-file-message-coalescence | If true, prevent flooding of identical messages during abnormal conditions. If there are multiple identical log messages for log files, there will be one \n message logged fully and follow with 'last message repeated n times' message. | true, false | true | set, show |
| log-relay | Flag to enable remote logging from a shelf controller to a node controller. If false, disable all remote logging from shelf controller to node controller | true, false | false | set, show |
| assignment-method | Define the assignment method of syslog. The assignment method can be: manual, dhcp or both. | both,manual, dhcp | both | set, show |
| niap-compliant-logging | Flag to enable or disable NIAP complaint logging. Sets whether the logs are NIAP compliant or not. | true, false | false | set, show |
| privacy-mode | Flag to enable/disable the GDRP filter. | true, false | false | set, show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |

#### Examples

This example shows how to display syslog information:

```
show syslog
```

This example shows how to enable alarm reporting:

```
set syslog alarm-report-control enabled
```

<!-- page 1254 -->

### 6.339.1. Syslog Severity and Facilities

**Table 779: Syslog Message Severity**

| Severity | Description |
| --- | --- |
| Emergency | System is unusable |
| Alert | Action must be taken immediately |
| Critical | Critical conditions |
| Error | Error conditions |
| Warning | Warning conditions |
| Notice | Normal but significant conditions |
| Informational | Informational messages |
| Debug | Debug-level messages |

Facility represents the SW group that created the syslog message. The following table lists how the facilities are used.

**Table 780: Syslog Facilities**

| Number | RFC Description | CLI facility name | What the facility is used for |
| --- | --- | --- | --- |
| 0 | kernel messages | kernel | kernel messages |
| 1 | user-level messages | user-level | user-level messages |
| 2 | mail system | mail-system |  |
| 3 | system daemons | system-daemons | system daemons |
| 4 | security/authorization messages | authentication | Authentication/authorization attempts messages |
| 5 | messages generated internally by syslogd | syslog-internal |  |
| 6 | line printer subsystem | line-printer |  |
| 7 | network news subsystem | network-news |  |
| 8 | UUCP subsystem | uucp |  |
| 9 | clock daemon | clock-daemon-9 |  |
| 10 | security/authorization messages | security | Security related events and error messages |
| 11 | FTP daemon | ftp-daemon | FTP daemon |
| 12 | NTP subsystem | ntp |  |
| 13 | log audit | log-audit |  |
| 14 | log alert | log-alert |  |
| 15 | clock daemon | clock-daemon-15 |  |
| 16 | local use 0 (local0) | local0 | Alarms |
| 17 | local use 1 (local1) | local1 | All commands, in a protocol agnostic format |
| 18 | local use 2 (local2) | local2 | All commands, in a protocol specific format |
| 19 | local use 3 (local3) | local3 |  |
| 20 | local use 4 (local4) | local4 | Line cards logs above error severity level |
| 21 | local use 5 (local5) | local5 |  |
| 22 | local use 6 (local6) | local6 | Any changes to the configuration DB |
| 23 | local use 7 (local7) | local7 | All Nokia applications |
|  |  | all | all facilities |

<!-- page 1256 -->
