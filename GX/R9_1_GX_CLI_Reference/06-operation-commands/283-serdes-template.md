---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.283. serdes-template'
source_lines: 21131-21192
---

## 6.283. serdes-template

#### Command Description

This command is used to auto-configure serdes for 3rd party TOMs. serdes-templates are created by the user per tom-part-number and apply to all line cards that support serdes; when a TOM is plugged-in with that part-number, the template will be automatically applied. The user can narrow down the list of ports to which the template applies, but by default all ports are considered. Manual configuration of serdes can still be done, and will be kept even if it deviates from the template. Switching a TOM with another TOM with a different part-number will imply a reset of the serdes configuration, and re-apply of the new template (if existing). Application of serdes-templates is dependent on the use-serdes-templates flag being set to 'true'. The user can force the re-application of a serdes-template by using the 'apply-template' command.

#### Command Syntax

```
add serdes-template-<name>[tom-part-number <value>] [card-types-applicable <value>] [ports-applicable <value>]
set serdes-template-<tom-part-number> [ports-applicable <value>]
delete serdes-template-<name>
show serdes-template-<tom-part-number> [ports-applicable]
```

#### Command Usage Details

**Table 658: serdes-template Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 659: serdes-template Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| tom-part-number | The TOM part-number to which this template applies. | string (length 1..16) | n/a |
| ports-applicable | The list of ports to which this template is applicable, or 'all' if all ports are to be considered (default). | all, string (length 1..16) maximum elements 20 | n/a |

#### Examples

This example shows how to view a serdes-template:

```
show serdes-template
serdes-template                  ports-applicable
-------------------------------  ----------------
serdes-template-FTLC9555SEPM-NF  T10
serdes-template-T-DQ4CNT-NIN     T1
serdes-template-TR-FC85S-NIN     T2
```

This example shows how to add a serdes-template:

```
add serdes-template-TR-FC85S-NIN ports-applicable
Value completion:
  T1     T10    T11    T12    T13    T14    T15    T16    T2     T3     T4     T5     T6     T7     T8     T9     all
```

This example shows how to delete a serdes-template:

```
delete serdes-template-FTLC9555SEPM-NF ports-applicable=T11
Are you sure you want to delete [ serdes-template-FTLC9555SEPM-NF ]? [y/n] n
```

<!-- page 1066 -->
