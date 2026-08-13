---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.80. diff'
source_lines: 9101-9160
---

## 6.80. diff

#### Command Description

This command is used to perform a diff comparison between a candidate configuration and the current system configuration. This command must be run in Exclusive Candidate Configuration mode. This command displays a list of differences between candidate datastore configuration and the current system configuration. The differences consist on created, deleted or changed objects, and displays only configurable attributes. In the context of the diff, the 'new' data refers to the candidate configuration, and the 'old' data refers to the current system configuration. The `diff commit` displays if the commit-records exist that is when the `commit-tracking` is enabled see system-policies (p. 1260), or a pending Confirmed Commit exists, see commit (p. 332). The command allows data to be presented in multiple ways:

- as a normal diff style output, where + represents added objects or new values, and - represents deleted objects or old values.
- as a side-by-side diff, by using the -t flag.
- as CLI commands that can be done to perform same configurations as the ones done in candidate datastore, by using the -c flag.
- as CLI command that provides the list of commands being executed, by using the -d flag.

#### Command Syntax

```
diff [-t|-c] candidate
diff commit <id> [<id>]
```

#### Command Usage Details

**Table 242: diff Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration mode |

<!-- page 398 -->

#### Command Parameters

**Table 243: diff Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| candidate | Target to be compared | candidate | n/a |
| commit | This command displays a list of differences between commit &lt;id&gt; and current configuration. It also presents a difference of the changes between these two commits (cumulatively). |  |  |
| command flags | -t Display the diff data in a table (side-by-side diff style) -c Display the diff data as CLI commands |  |  |

**Table 244: diff commit Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| &lt;id&gt; | It is a system generated commit-id. |  |  |

#### Examples

This example shows how to display the diff between candidate datastore and current configuration in side-by-side style:

```
diff -t candidate
```

This example shows how to display the diff between a commit-record and the running configuration or another commit-record in side-by-side style:

```
diff commit <id> [<id>]
```

<!-- page 399 -->
