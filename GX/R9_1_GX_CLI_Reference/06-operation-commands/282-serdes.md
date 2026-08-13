---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.282. serdes'
source_lines: 21078-21130
---

## 6.282. serdes

#### Command Description

These commands are used to add, edit or show serdes. The delete command is used to remove serdes from the configuration.

#### Command Syntax

```
add serdes-<card-name>-<port-name>/<serdes-name> value <value>
set serdes-<card-name>-<port-name>/<serdes-name> [value <value>]
show serdes-<card-name>-<port-name>/<serdes-name> [value] [status]
delete serdes-<card-name>-<port-name>/<serdes-name>
```

#### Command Usage Details

**Table 656: serdes Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 657: serdes Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-name | The name of the card supporting the TOM. | string | n/a | add, set, show, delete |
| port-name | The name of the port supporting the TOM. | string | n/a | add, set, show, delete |
| serdes-name | Name of the advanced parameter. | string | n/a | add, set, show, delete |
| value | Value of the advanced parameter. | string | n/a | add, set, show |
| status | State of the advanced parameter (as observable on the system) once it is configured. | set - Parameter set. unknown - Parameter unknown. in-progress - Parameter in progress. failed - Parameter failed. not-supported - Parameter not supported. | unknown | add, set, show |

#### Examples

This example shows how to view serdes:

```
show serdes
serdes                      value  status
--------------------------  -----  ------
serdes-1-7-T1/RxAmplitude   1      set
serdes-1-7-T1/RxPostCursor  1      set
serdes-1-7-T1/RxPreCursor   1      set
serdes-1-7-T1/TxEQ          1      set
serdes-1-7-T1/TxEQAdaptive  0      set
serdes-2-6-T2/RxEQ          3      set
```

<!-- page 1064 -->
