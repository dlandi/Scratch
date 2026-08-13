---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.101. expect'
source_lines: 10636-10689
---

## 6.101. expect

#### Command Description

This command is used to ensure that an attribute matches the expected value. This command can be used to validate an existing attribute value against an expected value. If the value matches, no output is seen, but if it doesn't, an ERROR message is shown. This command can be used in scripts and other types of automation to guarantee specific assumptions. It can be invoked multiple times to validate multiple attributes. Wildcards to select multiple object instances or attributes are not supported. A common application is to validate some system details at the beginning of a script. If the script is executed with 'stop-on-error' mode, a failed 'expect' would stop the script immediately. A -r flag enables regex mode, which makes the expected-value a regular expression instead of a literal string, allowing for a much more flexible check.

#### Command Syntax

```
expect [-r] [<instance>=]<value> [<expected-value>=]<value> [<attribute>=]<value>
```

#### Command Usage Details

**Table 287: expect Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 288: expect Command Flags**

| Parameter | Description |
| --- | --- |
| -r | Enables regex mode. If omitted, the expected-value is a literal string. |

<!-- page 473 -->

**Table 289: expect Command Parameters**

| Parameter | Description |
| --- | --- |
| instance | An existing instance of an object. |
| attribute | The name of the attribute to validate, belonging to &lt;instance&gt;. |
| expected-value | The expected value. If -r is provided, may be a regex. |

#### Examples

This example shows how to ensure the ne-name is GX-NE-12:

```
expect ne ne-name GX-NE-123
```

This example shows how to ensure the running SW is based on Release 5:

```
expect -r software-load-active swload-version R5.*
```

<!-- page 474 -->
