---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.164. log-console'
source_lines: 14213-14258
---

## 6.164. log-console

#### Command Description

These commands are used to set or show the attributes of the console logging supported by the system.

#### Command Syntax

```
set log-console [source-facilities <value>] [enabled <value>]
show log-console [source-facilities] [enabled]
```

#### Command Usage Details

**Table 417: log-console Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 418: log-console Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| source-facilities | List of syslog facilities used in this configuration. | • all<br>• authentication<br>• clock-daemon-15<br>• clock-daemon-9<br>• ftp-daemon<br>• kernel<br>• line-printer<br>• local1 • local2<br>• local3<br>• local4<br>• local5<br>• local6<br>• local7<br>• log-alert<br>• log-audit<br>• mail-system<br>• network-news<br>• ntp<br>• security<br>• syslog-internal<br>• system-daemons<br>• user-level<br>• uucp | n/a | set, show |
| enabled | Switches on and off the console logging. | true, false | false | set, show |

#### Examples

This example shows how to view the log-console attributes:

```
show log-console
```

This example shows how to enable the log-console:

```
set log-console enabled true
```

<!-- page 639 -->
