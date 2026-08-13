---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.370. validate'
source_lines: 27563-27603
---

## 6.370. validate

#### Command Description

This command is used to validate the contents of the specified configuration. This command validates any CLI command(s) used to edit a configuration datastore by creating, deleting, merging, or replacing content. This command validates any CLI command(s) used to edit a configuration datastore by creating, deleting, merging, or replacing content. If \<command\> is validated, the command replies with 'OK'. Otherwise the command will fail. Multiple CLI commands can be provided if separated with ';'.

#### Command Syntax

```
validate candidate <candidate> command <string>
```

#### Command Usage Details

**Table 847: validate command usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 848: validate Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| candidate | Candidate datastore as target for validation. | string | n/a |
| command | The command to validate. If &lt;command&gt; is validated, the command replies with 'OK'. Otherwise the command will fail. Multiple CLI commands can be provided if separated with ';'. | string | n/a |

<!-- page 1344 -->

#### Examples

This example shows how to validate one command:

```
validate 'set ne altitude 600'
```

<!-- page 1345 -->
