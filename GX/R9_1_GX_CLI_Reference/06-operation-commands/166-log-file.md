---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.166. log-file'
source_lines: 14302-14366
---

## 6.166. log-file

#### Command Description

These commands are used to add/set/show/delete local syslog files supported to the system. The locally saved log files can be cleared or deleted using the clear (p. 307) or delete commands respectively. The default system log file cannot be edited/deleted.

#### Command Syntax

```
add log-file-<name> [number-of-files <value>] [max-file-size <value>] [source-facilities <string>] [pattern-match <string>] [sensitive-data
<value>]
set log-file-<name> [number-of-files <value>] [max-file-size <value>] [source-facilities <value>] [pattern-match <value>] [sensitive-data
<value>]
show log-file-<name> [number-of-files] [max-file-size] [source-facilities] [pattern-match] [sensitive-data]
delete log-file-<name>
```

#### Command Usage Details

**Table 421: log-file Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 422: log-file Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The file name without the .log extension. | String (length 0...128 characters) | n/a | add, set, show, delete |
| number-of-files | Maximum number of log files retained. When rotating files due to max size being reached, the oldest files will be discarded if the total number of files is greater than number of files. | uint8 (range 1..20) | 10 | add, set, show |
| max-file-size | Maximum file size before rotation (in megabytes). | uint8 (range 1..30, megabytes) | 30 | add, set, show |
| source-facilities | List of syslog facilities used in this configuration. | all authentication clock-daemon-15 clock-daemon-9 ftp-daemon kernel line-printer local0 local1 local2 local3 local4 local5 local6 local7 log-alert log-audit mail-system network-news ntp security syslog-internal system-daemons user-level uucp | all | add, set, show |
| pattern-match | Regex pattern that all entries need to obey. | String (length: 0..255) | n/a | add, set, show |
| sensitive-data | Whether the local file has logs include sensitive data. | none, both, only | • none - for regular log-file, and<br>• only - for log-file-default-audit-dbg | add, set, show |

#### Examples

This example shows how to add all log server facilities:

```
add log-file-file1 source-facilities local6,local3,local2
```

This example shows how to log all error, critical, alert and emergency messages that happen in the system, in a log file called faults:

```
add log-file-faults source-facilities all
add log-file-facility-filter-faults/all severity error compare-op equals-or-higher
```

This example shows how to log all the changes made to DB messages without debug:

```
add log-file-myevents source-facilities local6
```

<!-- page 645 -->

**Note:** By default, severity is set to informational and above.

<!-- page 646 -->
