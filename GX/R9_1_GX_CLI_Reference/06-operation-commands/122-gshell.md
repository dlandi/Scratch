---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.122. gshell'
source_lines: 11772-11824
---

## 6.122. gshell

#### Command Description

This command is used to launch a Linux bash shell inside a Guest Container from within the CLI. The shell can be closed the typical way (for example, with the 'exit' command), and the shell will return to the CLI prompt. The command also allows execution of a single shell command inside the Guest Container.

#### Command Syntax

```
gshell [-h] [<cmd>]
```

#### Command Usage Details

**Table 330: gshell Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |
| Pre-condition | The dial-out server must be configured for this command to execute properly. |

#### Command Parameters

**Table 331: gshell Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 332: gshell Command Parameters**

| Parameter | Description |
| --- | --- |
| cmd | Command to execute inside the Guest Container |

#### Examples

This example shows how to open a shell on the guest container:

<!-- page 526 -->

```
gshell
```

This example shows how to execute ls -l command inside the container:

```
gshell ls -l
```

<!-- page 527 -->
