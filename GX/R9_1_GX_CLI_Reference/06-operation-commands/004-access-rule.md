---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.4. access-rule'
source_lines: 4161-4225
---

## 6.4. access-rule

#### Command Description

The commands described in this section are used to add, set or show the `access-rule` attributes. Use the delete command to delete an access-rule from an access rule list.

#### Command Syntax

```
add access-rule-<access-rule-list-name>/<access-rule-name> action <value> [sequence-id <value>] [module-name <value>] [path <value>] [attribute
<value>] [attribute-value <value>] [operation <value>] [description <value>]
delete access-rule-<access-rule-list-name>/<access-rule-name>
set access-rule-<access-rule-list-name>/<access-rule-name> [sequence-id <value>] [module-name <value>] [path <value>] [attribute <value>]
[attribute-value <value>] [operation <value>] [action <value>] [description <value>]
show access-rule-<access-rule-list-name>/<access-rule-name> [sequence-id] [module-name] [path] [attribute] [attribute-value] [operation] [action]
[description]
```

#### Command Usage Details

**Table 66: access-rule Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 67: access-rule Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| access-rule-list-name | The name of the access-rule-list. | string | n/a | add, delete, set, show |
| access-rule-name | The name of the access-rule. Represents a single access-rule, defining access to a particular target path. The rule can also consider multiple filters, including:<br>• just a particular path<br>• a path and an attribute (or more than one attribute)<br>• a path, an attribute and a value (or more than one value)<br>• a module-name<br>• the operation type (create/read/update/delete/exec ute) Paths can represent data-nodes, RPCs or notifications, as well as other non-YANG commands. If all criteria are satisfied, the rule will be applied, which means the associated access will be permitted or denied (depending on the 'action' parameter). System supports a maximum of 500 access-rules, across all access-rule-lists. | string | n/a | add, delete, set, show |
| action | The permit/deny action associated with this rule. This field needs to be provided whenever an access rule is created (e.g. is mandatory). | • permit<br>• deny | n/a | add, set, show |
| sequence-id | The id of this access-rule within the current list, used for processing all rules. Lower number ids are processed first. The id can change over the lifetime of the access-rule-list to re-sort different entries. If not provided, the sequence-id is set to the currently used latest id plus 1 (e.g. will go to the end of the list). | uint16 | If not provided, the sequence-id is set to the currently used latest id plus 1. | add, set, show |
| module-name | YANG Module to consider when considering this rule; needs to match an available data-model file. By default, the value '*' is used to represent 'any module name'. Note: this value is not validated; if a non-existing module is described here, it will imply the rule will not be valid. | string (max length 64 characters) | * | add, set, show |
| path | The target object of the access rule. May be:<br>• XPath of a YANG data node<br>• XPath of a YANG notification • XPath of a YANG RPC or a descendant<br>• External command (gNOI, etc) | string (max length 255 characters) | * | add, set, show |
| attribute | Attribute name to which this rule applies to. If not provided, the rule will apply to all attributes in the provided path. If multiple attributes are specified, then the rule applies to all of them. Note that if the rule is based on attribute-value, then this field needs to target only 1 attribute. | leaf-list of strings (max length 64 characters, max entries 10) | * | add, set, show |
| attribute-value | Attribute value to which this rule applies to. If not provided, it means the rule applies independently on the attribute value. Can only be provided if a single 'attribute' name is provided | leaf-list of strings (max length 64 characters, max entries 10) | * | add, set, show |
| operation | The list of operations that the rule applies to. The '*' value represents all operations, and is the default value. Note: YANG bits represent a data type where multiple values can be set simultaneously; so "create read update" is a valid value for this attribute. | bits<br>• create<br>• read<br>• update<br>• delete<br>• execute | * | add, set, show |
| description | A user-configurable description about this access rule. | string (max length 256 characters) | empty string | add, set, show |

#### Examples

The following command shows how to add a rule to deny the creation of a card:

```
add access-rule-1/rule path /ne/equipment/card operation create action deny
```

The following example shows how to delete an access rule:

```
delete access-rule-1/rule
```

The following example shows how to view the attributes of a rule:

```
show access-rule-1/rule
```

<!-- page 139 -->
