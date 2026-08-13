---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.51. cli-session-config'
source_lines: 7196-7238
---

## 6.51. cli-session-config

#### Command Description

These commands are used to set or show the configuration of the Command Line Interface (CLI) session attributes.

#### Command Syntax

```
set cli-session-config-<session-id> [cli-lines <value>] [cli-columns <value>] [interactive-mode <value>] [display-timestamp <value>]
show cli-session-config-<session-id> [cli-lines] [cli-columns] [interactive-mode] [display-timestamp]
```

#### Command Usage Details

**Table 177: cli-session-config Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 178: cli-session-config Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| session-id | CLI session ID. | String | n/a | show |
| cli-lines | Configurable number of rows to be used for display before pausing the output. After pausing, pressing [SPACEBAR] will resume display. | Number (range: 10..1000) | 40 | set, show |
| cli-columns | Configurable number of columns to be used for display. | Number (range: 80..4000) | 80 | set, show |
| interactive-mode | Determines if the CLI shall issue interactive prompt (e.g., for prompting additional information, or for confirmation of user-initiated actions). This attribute can be set to: true (default value) - CLI will prompt user. false - CLI will suppress any prompt to the user. This parameter is set per CLI session and it is not persistent. | true, false | true | set, show |
| display-timestamp | Determines if the current timestamp is printed on every CLI command. | true, false | false | set, show |

#### Examples

```
show cli-session-config-10.19.204.27:52361  #shows all the attributes of the cli session.
set cli-session-config-10.19.204.27:52361 cli-lines 30  #sets the number of cli rows to be used for display to 30.
set cli-session-config-10.19.204.27:52361 cli-columns 100  #sets the number of cli columns to be used for display to 100.
```

<!-- page 315 -->
