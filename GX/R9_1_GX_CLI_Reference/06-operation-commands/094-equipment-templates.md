---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.94. equipment-templates'
source_lines: 10190-10241
---

## 6.94. equipment-templates

#### Command Description

These commands are used to enable and view the serdes templates setting associated with equipment.

#### Command Syntax

```
set equipment-templates [use-serdes-templates <value>]
show equipment-templates [use-serdes-templates]
```

#### Command Usage Details

**Table 272: equipment-templates Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 273: equipment-templates Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| use-serdes-templates | Whether serdes-templates are globally enabled or not. On enabling: templates are not automatically applied; they'll be applied from that moment onward. On disabling: no impact; existing serdes configuration is kept on all TOMs, independently on whether they were applied via template or manually | enabled, disabled | disabled |

#### Examples

The following example shows how to set an equipment template:

<!-- page 445 -->

```
 set equipment-templates use-serdes-templates
Value completion:
  false    true*
set equipment-templates use-serdes-templates true
```

The following example shows how to display the equipment template attribute:

```
show equipment-templates use-serdes-templates
  equipment-templates
  use-serdes-templates             true
```

<!-- page 446 -->
