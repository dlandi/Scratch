---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.180. message'
source_lines: 15019-15063
---

## 6.180. message

#### Command Description

This command is used to send a message to other CLI sessions. By default the message is broadcast to all CLI sessions. The target CLI session can also be set based on the session-id, username etc. The command can be executed from NETCONF and RESTCONF as well, but the message will only toe sent to CLI sessions.

#### Command Syntax

```
message [message-content=]<value> [[target=]<value>]
```

#### Command Usage Details

**Table 449: messagekill-session Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 450: message Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| message content | The message text to broadcast | string (1-4096 characters) | n/a |
| target | The CLI sessions to which the message will be sent | • all - All CLI session<br>• local - Only local sessions (serial console or CRAFT)<br>• remote - Only remote sessions (eg. SSH) • session-id - Only to a specific session identified by the session-id<br>• username - All CLI sessions of a specific user | all |

#### Examples

This example sends a message to all CLI sessions

```
message "System will reboot in 5 minutes"
```

This example sends a message to a specific session

```
message "System reboot in 5 minutes" 0.0.0.0:36
```

<!-- page 680 -->
