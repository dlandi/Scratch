---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.169. log-server-facility-filter'
source_lines: 14484-14527
---

## 6.169. log-server-facility-filter

#### Command Description

These commands allow to filter log messages based on their source facilities and severities. This is a filter based on source-facilities leaf-list and can only add a filter to the configured source facilities. The delete command can be used to delete a log-server facility filter.

#### Command Syntax

```
add log-server-facility-filter-<log-server-name>/<log-server-facility-filter-name> [severity <value>] [compare-op <value>]
set log-server-facility-filter-<log-server-name>/<log-server-facility-filter-name> [severity <value>] [compare-op <value>]
show log-server-facility-filter-<log-server-name>/<log-server-facility-filter-name> [severity] [compare-op]
delete log-server-facility-filter-<log-server-name>/<log-server-facility-filter-name>
```

#### Command Usage Details

**Table 427: log-server-facility-filter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 428: log-server-facility-filter Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| log-server-name | The file name without the .log extension. | String (length 0...128 characters) | n/a | add, set, show, delete |
| log-server-facility-filter-name | Facility selector. Identifies a single syslog facility, or all of them if value is 'all'. | kernel user-level mail-system system-daemons authentication syslog-internal line-printer network-news uucp clock-daemon-9 security ftp-daemon ntp log-audit log-alert clock-daemon-15 local0 local1 local2 local3 local4 local5 local6 local7 all | n/a | add, delete, show |
| severity | The system log selected severity level for forwarding. Describes the option to specify how the severity comparison is performed. The default severity level is all levels. | emergency alert critical error warning notice informational debug | informational | add, show |
| compare-op | Describes the option to specify how the severity comparison is performed. | equals equals-or-higher not-equals | equals-or-higher | add, show |

#### Examples

This example shows how to add all log server facilities:

```
add log-server-facility-filter-server184/local2 severity debug
```

<!-- page 656 -->
