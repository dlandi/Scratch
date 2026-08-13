---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.165. log-console-facility-filter'
source_lines: 14259-14301
---

## 6.165. log-console-facility-filter

#### Command Description

These commands are used to add, set or show a selector that filters messages based on their source facilities and severities. The delete command deletes a log-console-facility-filter.

#### Command Syntax

```
add log-console-facility-filter-<name> [severity <value>] [compare-op <value>]
set log-console-facility-filter-<name> [severity <value>] [compare-op <value>]
show log-console-facility-filter-<name> [severity] [compare-op]
delete log-console-facility-filter-<name>
```

#### Command Usage Details

**Table 419: log-console-facility-filter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 420: log-console-facility-filter Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Selector that allows to filter log messages based on their source facilities and severities. It identifies a single syslog facility, or all of them if the value is 'all'. | • all<br>• authentication<br>• clock-daemon-15<br>• clock-daemon-9<br>• ftp-daemon • kernel<br>• line-printer<br>• local1<br>• local2<br>• local3<br>• local4<br>• local5<br>• local6<br>• local7<br>• log-alert<br>• log-audit<br>• mail-system<br>• network-news<br>• ntp<br>• security<br>• syslog-internal<br>• system-daemons<br>• user-level<br>• uucp | n/a | add, set, delete, show |
| severity | The system log selected severity level for forwarding. Describes the option to specify how the severity comparison is performed. The default severity level is all levels. • emergency: Level 0 - System is unusable.<br>• alert: Level 1 - Action must be taken immediately.<br>• critical: Level 2 - Critical conditions.<br>• error: Level 3 - Error conditions.<br>• warning: Level 4 - Warning conditions.<br>• notice: Level 5 - Normal but significant condition.<br>• informational: Level 6 - Informational messages.<br>• debug: Level 7 - Debug-level messages. | • emergency<br>• alert<br>• critical<br>• error • warning<br>• notice<br>• informational<br>• debug | informational | add, set, show |
| compare-op | Describes the option to specify how the severity comparison is performed. | equals equals-or-higher not-equals | equals-or-higher | add, set, show |

#### Examples

This example shows how to filter log messages based on severity 'critical':

```
add log-console-facility-filter-all severity 'critical'
```

<!-- page 642 -->
