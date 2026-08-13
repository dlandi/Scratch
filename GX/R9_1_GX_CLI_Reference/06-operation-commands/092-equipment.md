---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.92. equipment'
source_lines: 10063-10129
---

## 6.92. equipment

#### Command Description

This command is used to display installed equipment information.

#### Command Syntax

```
show equipment [<option>]
```

#### Command Usage Details

**Table 268: equipment Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 269: equipment Command Parameters**

| Parameter | Description | Value |
| --- | --- | --- |
| equipment option i Note: Your options will vary according to you installation. | The equipment to be viewed. | n/a |

#### Examples

This example shows how to display equipment-policies information:

```
show equipment card-1-1
```

The following output is displayed:

<!-- page 440 -->

```
  card-1-1
  controller-card-1-1
  console-1-1
  port-1-1-AUX-1
  port-1-1-AUX-2
  port-1-1-CRAFT
  port-1-1-DCN
  port-1-1-U1
  required-type                    XMM4
  required-subtype                 ''
  category                         controller
  chassis-name                     1
  slot-name                        '1'
  max-power-draw                   58.80 W
  last-reboot-reason               'Unknown'
  alias-name                       ''
  AID                              '1-1'
  admin-state                      unlock
  oper-state                       enabled
  avail-state                      'normal in-service'
  alarm-report-control             allowed
  label                            ''
```

<!-- page 441 -->
