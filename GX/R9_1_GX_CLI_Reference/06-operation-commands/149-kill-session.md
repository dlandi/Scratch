---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.149. kill-session'
source_lines: 13283-13320
---

## 6.149. kill-session

#### Command Description

This command is used to close any established session, independently on the type of the session (CLI, NETCONF, etc). The \<session-id\> needs to match an existing session-id (in the form '\<ip-address\>:\<port\>'), but cannot match the id for the current session (a normal 'exit' command should be used instead). Use the show session command to display a list of current sessions.

#### Command Syntax

```
kill-session <session-id>
```

#### Command Usage Details

**Table 387: kill-session Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 388: kill-session Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| session-id | An existing session-id | string | n/a |

#### Examples

This example shows how to kill the specified session:

```
kill-session 10.24.11.25:56212
```

<!-- page 597 -->
