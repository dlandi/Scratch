---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.167. log-file-facility-filter'
source_lines: 14367-14419
---

## 6.167. log-file-facility-filter

#### Command Description

These commands are used to add/set/show a selector that filters messages based on their source facilities and severities. The delete command is used to delete the log-file-facility-filter.

#### Command Syntax

```
add log-file-facility-filter-<log-file-name>/<log-file-facility-filter-name> [severity <value>] [compare-op <value>]
set log-file-facility-filter-<log-file-name>/<log-file-facility-filter-name> [severity <value>] [compare-op <value>]
show log-file-facility-filter-<log-file-name>/<log-file-facility-filter-name> [severity] [compare-op]
delete log-file-facility-filter-<log-file-name>/<log-file-facility-filter-name>
```

#### Command Usage Details

**Table 423: log-file-facility-filter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 424: log-file-facility-filter Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Identifies a single syslog facility, or all of them if value is 'all'. | kernel user-level mail-system system-daemons authentication syslog-internal line-printer network-news uucp clock-daemon-9 security ftp-daemon ntp log-audit log-alert clock-daemon-15 local0 local1 local2 local3 local4 local5 local6 local7 all | n/a | add, delete, show |
| log-file-facility-filter-name | Facility filter selector. | string | n/a | show |
| severity | The system log selected severity level for forwarding. Describes the option to specify how the severity comparison is performed. The default severity level is all levels. | emergency alert critical error warning notice informational debug | informational | add, show |
| compare-op | Describes the option to specify how the severity comparison is performed. | equals equals-or-higher not-equals | equals-or-higher | add, show |

#### Examples

This example shows how to log all error, critical, alert and emergency messages that happen in the system in a log file called faults:

```
add log-file-faults source-facilities all
add log-file-facility-filter-faults/all severity error compare-op equals-or-higher
```

This example shows how to log all warnings and above message from Nokia Apps and all errors from Line Cards:

```
add log-file-lcAndApps source-facilities local4,local7
add log-file-facility-filter-lcAndApps/local7 severity warning compare-op equals-or-higher
add log-file-facility-filter-lcAndApps/local4 severity error compare-op equals-or-higher
```

<!-- page 649 -->
