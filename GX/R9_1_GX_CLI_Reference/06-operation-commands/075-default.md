---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.75. default'
source_lines: 8621-8696
---

## 6.75. default

#### Command Description

This command can be used to assign default value(s) for the targeted entities. It is useful if the user wants to reset the entity after making changes to it. Some of the configuration attributes will not be reset to default, including mandatory parameters and attributes only settable at creation. If attributes are specified, only those will be set to their default value, otherwise, all possible ║ configuration attributes will be defaulted.

**Note:** Select multiple instances by using wildcard (\*). See the examples below.

**Note:** The default command is not supported in 1830 GX G30 Releases 5.0 and 5.1.

#### Command Syntax

```
default [-f] [entity-id=][,]* [[attribute=][,]*]
```

#### Command Usage Details

**Table 230: default Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 231: default Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Forces the command without confirmation. |

<!-- page 379 -->

**Table 232: default Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| entity id | Instance ID of the entity to be returned to the default state. | 1830 GX Management Entity AIDs (p. 43) | n/a |
| attribute | Attribute names to be defaulted. If empty, default all entities' attributes. | string | n/a |

#### Examples

This example shows how to set the default config attributes of this user:

```
default user-peter
```

This example shows how to reset defaults for the one particular alarm-severity-entry:

```
default alarm-severity-entry-FAN/EQPTMSMT
```

This example shows how to reset the defaults for the alarm-severity-entries for FAN resources:

```
default alarm-severity-entry-FAN/*
```

This example shows how to reset the defaults for the entire alarm-severity-entry table:

```
default alarm-severity-entry-*
```

This example shows how to reset the default label and phy-mode attributes for tom-1-4-T16:

```
default tom-1-4-T16 label,phy-mode
```

<!-- page 380 -->
