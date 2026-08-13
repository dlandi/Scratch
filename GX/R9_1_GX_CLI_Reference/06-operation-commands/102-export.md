---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.102. export'
source_lines: 10690-10754
---

## 6.102. export

#### Command Description

This command is used to define variables to use in CLI. Variables can be referenced with ${variable} in any CLI command. Variables are locally defined per session, so they are removed after the session is closed. This functionality is particularly useful in CLI scripts. The export command can be used to define, delete or view variables.

#### Command Syntax

```
export [-h]
export [<variable>=[<value>]]
```

#### Command Usage Details

**Table 290: export Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 291: export Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 292: export Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| variable | Name of the variable; can be any alphanumeric name. | string | n/a |
| value | Value to replace variable with; can be any supported character, including spaces. | string - can be any supported character, including spaces. | n/a |

#### Examples

This example shows how to declare a variable:

```
export SLOT_NUMBER=2
```

This example shows how to delete a variable:

```
export SLOT_NUMBER=
```

This example shows how to view all existing variables:

```
export
```

The following output is displayed:

```
export MYSESSIONID="10.100.152.29:56154"
export MYUSERNAME="admin"
```

<!-- page 476 -->
