---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.5. access-rule-list'
source_lines: 4226-4275
---

## 6.5. access-rule-list

#### Command Description

The commands described in this section are used to add, set or show the `access-rule-list` attributes. Use the delete command to delete an access rule list. The `access-rule-list` is a group of access-rules, organized by which user-groups the rules apply to. It is created by the user. The access-rule-list is processed in order, as given by the sequence-id parameter.

#### Command Syntax

```
add access-rule-list-<name> [user-group <value>] [sequence-id <value>] [description <value>]
delete access-rule-list-<name>
set access-rule-list-<name> [user-group <value>] [sequence-id <value>] [description <value>]
show access-rule-list-<name> [user-group] [sequence-id] [description]
```

#### Command Usage Details

**Table 68: access-rule-list Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 69: access-rule-list Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the access-rule-list. | string | n/a | add, delete, set, show |
| user-group | List of user-groups that this access-rule-list applies to. The default value '*' is used as a match-all representation, meaning this access-rule-list applies to all existing user-groups. A maximum of 20 user-groups can be referenced. | leaf-list of user-group names or '*' | * | add, set, show |
| sequence-id | The id of this access-rule within the current list, used for processing all rules. Lower number ids are processed first. The id can change over the lifetime of the access-rule-list to re-sort different entries. If not provided, the sequence-id is set to the currently used latest id plus 1 (e.g. will go to the end of the list). | uint16 | If not provided, the sequence-id is set to the currently used latest id plus 1. | add, set, show |
| description | A generic description of this access-rule-list. | string (length 0..256 characters) | n/a | add, set, show |

#### Examples

The following example shows how to add an access rule list:

```
add access-rule-list-A user-group XY
```

The following example shows how to delete an access rule list:

```
delete access-rule-list-A
```

<!-- page 141 -->
