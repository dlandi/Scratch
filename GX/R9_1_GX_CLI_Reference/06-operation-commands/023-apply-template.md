---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.23. apply-template'
source_lines: 5540-5592
---

## 6.23. apply-template

#### Command Description

This command is used to apply templates of multiple types.

#### Command Syntax

```
apply-template [template-type=]<value> ([[applicable-tom=]<value>[,<value>]*])
```

#### Command Usage Details

**Table 117: apply-template Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 118: apply-template Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| template-type | The type of template to apply. Other parameters may be required depending on the template type. | serdes-template Applies all existing _ serdes-templates to the provided TOM list as the 'applicable-tom' parameter. If no specific TOMs are provided, all TOMs are considered for template application. | serdes-templage |
| applicable-tom | Applicable TOMS | List of TOMs to which to apply serdes-templates against. If not provided (e.g. list is empty), all system TOMs will be considered for application. | n/a |

<!-- page 217 -->

#### Examples

This example shows how to apply a template:

```
temproot@GX> apply-template serdes-template
Filtered attributes completion:
  applicable-tom=
applicable-tom completion:
  tom-1-7-T1     tom-1-7-T10    tom-1-7-T11    tom-1-7-T12    tom-1-7-T13    tom-1-7-T14    tom-1-7-T15    tom-1-7-T8     tom-1-7-T9
[ ne ]
temproot@GX> apply-template serdes-template tom-1-7-T1
applicable-tom completion:
  tom-1-7-T1     tom-1-7-T10    tom-1-7-T11    tom-1-7-T12    tom-1-7-T13    tom-1-7-T14    tom-1-7-T15
[ ne ]
temproot@GX> apply-template serdes-template tom-1-7-T1
[ ne ]
temproot@GX>
```

<!-- page 218 -->
