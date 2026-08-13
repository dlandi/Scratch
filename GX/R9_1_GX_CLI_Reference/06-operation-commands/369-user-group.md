---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.369. user-group'
source_lines: 27493-27562
---

## 6.369. user-group

#### Command Description

These commands are used to add, set or show user groups and attributes. Each user will be associated with a list of groups, and will derive its permissions from them. Use the delete command to delete a user-group.

#### Command Syntax

```
add user-group-<name> [description <value>]
delete user-group-<name>
set user-group-<name> [description <value>]
show user-group-<name> [description]
```

#### Command Usage Details

**Table 845: user-group Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 846: user-group Command Attributes**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the group. | String (1...64 characters) | n/a | set, show |
| description | Long description of the user group. | String (1...128 characters) | n/a | set, show |

#### Examples

The following example shows how to add a new user-group:

<!-- page 1342 -->

```
add user-group-XY
```

The following example shows how to delete a new user-group:

```
delete user-group-XY
```

This following example shows how to view the descriptions of the user groups:

```
show user-group
```

The following output is displayed:

```
suser-group     description
-------------  ------------------------
user-group-EA  Encryption Administrator
user-group-MA  Monitoring Access
user-group-NA  Network Administrator
user-group-NE  Network Engineer
user-group-PR  Provisioning
user-group-SA  Security Administrator
user-group-TT  Turn-up and Test
```

<!-- page 1343 -->
