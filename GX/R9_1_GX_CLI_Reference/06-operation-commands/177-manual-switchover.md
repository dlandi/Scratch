---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.177. manual-switchover'
source_lines: 14860-14918
---

## 6.177. manual-switchover

#### Command Description

This command is used to perform a manual switchover.

#### Command Syntax

```
manual-switchover [-f] [resource=]<value>
```

#### Command Usage Details

**Table 443: manual-switchover Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 444: manual-switchover Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| resource | The object to be manually switched. | AID | n/a |

#### Examples

This example shows how to perform a manual switchover in a 1830 GX G42 chassis:

```
manual-switchover card-1-3
Controller will switchover and connection to the management interface will be lost. Do you want to continue? [y/n]
```

Confirm at this prompt to proceed with the manual switch operation. This example shows how to perform a manual switchover in a 1830 GX G32 chassis:

<!-- page 670 -->

```
manual-switchover card-1-5
Controller will switchover and connection to the management interface will be lost. Do you want to continue? [y/n]
```

Confirm at this prompt to proceed with the manual switch operation. This example shows how to perform a manual switchover in a 1830 GX G34c chassis:

```
manual-switchover card-1-12
Controller will switchover and connection to the management interface will be lost. Do you want to continue? [y/n]
```

Confirm at this prompt to proceed with the manual switch operation.

**Note:** For more information and the procedure for manual switchover, see the *1830 GX Software Management Procedures Guide*.

<!-- page 671 -->
