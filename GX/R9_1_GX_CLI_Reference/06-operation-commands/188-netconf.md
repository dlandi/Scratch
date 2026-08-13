---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.188. netconf'
source_lines: 15409-15457
---

## 6.188. netconf

#### Command Description

These commands are used to set or show NETCONF management protocol attributes.

#### Command Syntax

```
set netconf [enabled <value>] [port <value>] [annotate-cli-name <value>] [static-info-in-notifs <value>] [hello-timeout <value>]
show netconf [enabled] [port] [annotate-cli-name] [static-info-in-notifs] [hello-timeout]
```

#### Command Usage Details

**Table 464: netconf Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 465: netconf Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| enabled | Enables/disables the NETCONF management protocol. It is not possible to disable NETCONF from within a NETCONF session. | true false | true | set, show |
| port | The port which listens for NETCONF access via ssh. | port-number (uint16, range 1..65535) | 830 | set, show |
| hello-timeout | Specifies the number of seconds that a session may exist before the hello PDU is received/transmitted. A session will be dropped if no hello PDU is received/transmitted before this number of seconds elapses. | uint16 (range 1..3600 seconds) | 2 | set, show |
| annotate-cli-name | If enabled, annotates NETCONF XML output with cli names for traceability. | true false | false | set, show |
| static-info-in-notifs | List of YANG identifiers that are statically included in notifications. If they are present in objects that are notified. Maximum elements is 10. Applicable for management protocols with support for YANG-type notifications (NETCONF, etc). For example, if object user[user-name='tom'] has had the 'timeout' attribute updated, and the static-info-in-notifs included the 'user-status' string, the associated notification would include not only the 'timeout' parameter, but also the 'user-status' (despite the fact that it had not changed). | string (length 1..64) | n/a | set, show |

#### Examples

The following example shows how to view the NETCONF management protocol attributes:

```
show netconf
```

This example shows how to enable NETCONF:

```
set netconf enabled true
```

<!-- page 701 -->
