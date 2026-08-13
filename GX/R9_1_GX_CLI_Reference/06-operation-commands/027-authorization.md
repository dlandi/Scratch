---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.27. authorization'
source_lines: 5743-5795
---

## 6.27. authorization

#### Command Description

The commands described in this section are used to set or show the `authorization` attributes.

#### Command Syntax

```
set authorization [mode <value>] [read-default <value>] [write-default <value>] [exec-default <value>]
show authorization [mode] [read-default] [write-default] [exec-default] [denied-operations] [denied-data-writes] [denied-notifications]
```

#### Command Usage Details

**Table 125: authorization Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 126: authorization Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| mode | System global authorization mode - selects which kind of authorization rules are used.<br>• static-only - Only system defined static authorization rules are used.<br>• static+rules - Both user and system defined access-rules are used. System will try to identify a user configured access-rule first, and only if not found would the system rules be used as a fallback.<br>• rules-only - Only user defined access-rules are used. System will try to identify a user configured access-rule first; if not found, the global defaults (read-default/write-default/exec-default) will be used. | • static-only<br>• static+rules<br>• rules-only | static+rules | set, show |
| read-default | In case only user configured access-rules are used, this policy defines what is the action to use if a given read operation does not match any rule. Read access includes ability to do get/show commands, as well as to receive notifications. (only configurable if mode = rules-only) | • permit<br>• deny | permit | set, show |
| write-default | In case only user configured access-rules are used, this policy defines what is the action to use if a given write operation does not match any rule. Write access includes create/ update/delete commands. (only configurable if mode = rules-only) | • permit<br>• deny | deny | set, show |
| exec-default | In case only user configured access-rules are used, this policy defines what is the action to use if a given exec operation does not match any rule. Exec access includes invocation of RPCs and other commands. (only configurable if mode = rules-only) | • permit<br>• deny | permit | set, show |
| denied-operations | Number of times since the system last restarted that an Exec request was denied. | uint32 | 0 | show |
| denied-data-writes | Number of times since the system last restarted that a Write operation request was denied. | uint32 | 0 | show |
| denied-notifications | Number of times since the system last restarted that a notification was dropped for a subscription because access to the event type was denied. | uint32 | 0 | show |

#### Examples

The following example shows how to set the authorization mode to static-only:

```
set authorization mode static-only
```

<!-- page 231 -->

The following example shows how to set the authorization write-default value to deny:

```
set authorization write-default deny
```

<!-- page 232 -->
