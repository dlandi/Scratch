---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.345. template'
source_lines: 26008-26059
---

## 6.345. template

#### Command Description

These commands are used to add, set, show and delete the template entry that is defined by an object and attribute pair, and then the value to be used as the default for that attribute. A template represents the single template entry, allowing an individual rule for defining a default value for a given attribute.

#### Command Syntax

```
add template-<template-group-name>/<template-name> object <value> attribute <value> value <value> [sequence-id <value>] [label <value>]
[condition <value>]
set template-<template-group-name>/<template-name> [sequence-id <value>] [object <value>] [attribute <value>] [value <value>] [label <value>]
[condition <value>]
show template-<template-group-name>/<template-name> [sequence-id] [object] [attribute] [value] [label] [condition]
delete template-<template-group-name>/<template-name>
apply-template [template-type=]<value> [[applicable-tom=]<value>[,<value>]*] [[template-group=]<value>] [[template-entry=]<value>] [dry-run]
```

#### Command Usage Details

**Table 793: template Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 794: template Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| template-group name | Represents name of the template-group | string | n/a | add, set, show, delete |
| template-name | Represents name of the template entry | string | n/a | add, set, show, delete |
| sequence-id | Represents id of this template entry, it is used to define the order in which templates are processed. Lower number ids are processed first. | Number (range 1.. 65535) | n/a | add, set, show |
| object | object name to apply to (e.g. odu) | string | n/a | add, set, show |
| attribute | attribute name to apply to (e.g. admin-state) | string | n/a | add, set, show |
| value | Represents the value to apply on the template (e.g. lock) - mandatory | string | n/a | add, set, show |
| label | Represents the label to apply on the template - optional | string | n/a | add, set, show |
| condition | Represents the condition to apply on the template (e.g. service-type=OTU4)- optional | string | n/a | add, set, show |

#### Examples:

The following examples shows how to add two templates:

```
add template-1/AcmeContact object ne attribute contact value Acme
add template-1/FansInMaintenance object card attribute admin-state value maintenance condition card-type=FAN
```

<!-- page 1271 -->
