---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.284. serdes-template-entry'
source_lines: 21193-21260
---

## 6.284. serdes-template-entry

#### Command Description

These commands are used to enter an individual entry to the serdes-template. The commands are composed of a serdes parameter name and associated value.

#### Command Syntax

```
add serdes-template-entry-<serdes-template-name>/<serdes-template-entry-name> value <value>
set serdes-template-entry-<serdes-template-name>/<serdes-template-entry-name> [value <value>]
delete serdes-template-entry-<serdes-template-name>/<serdes-template-entry-name>
show serdes-template-entry-<serdes-template-name>/<serdes-template-entry-name>/<name> [value]
```

#### Command Usage Details

**Table 660: serdes-template-entry Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 661: serdes-template-entry Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| tom-part-number | The TOM part-number to which this template applies. | string (length 1..16) | n/a |
| name | Name of the serdes parameter. | string (length 0....256) | n/a |
| value | Value of the serdes parameter. | string (length 0...256) | n/a |

<!-- page 1067 -->

#### Examples

This example shows how to view the serdes template entries::

```
show serdes-template-entry
serdes-template-entry                              value
-------------------------------------------------  -----
serdes-template-entry-FTLC9555SEPM-NF/RxAmplitude  2
serdes-template-entry-FTLC9555SEPM-NF/RxEQ         1
serdes-template-entry-FTLC9555SEPM-NF/TxEQ         2
serdes-template-entry-T-DQ4CNT-NIN/RxAmplitude     1
serdes-template-entry-T-DQ4CNT-NIN/RxPostCursor    1
serdes-template-entry-T-DQ4CNT-NIN/RxPreCursor     1
serdes-template-entry-T-DQ4CNT-NIN/TxEQ            1
serdes-template-entry-T-DQ4CNT-NIN/TxEQAdaptive    0
serdes-template-entry-TR-FC85S-NIN/RxEQ            3
```

This example shows how to add a serdes template entry::

```
add serdes-template-entry-T-DQ4CNT-NIN/RxEQ value 3
```

This example shows how to edit a serdes template entry::

```
set serdes-template-entry-TR-FC85S-NIN/TxEQ value 2
```

<!-- page 1068 -->
