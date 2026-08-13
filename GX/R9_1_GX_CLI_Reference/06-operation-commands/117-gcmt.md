---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.117. gcmt'
source_lines: 11509-11556
---

## 6.117. gcmt

#### Command Description

This command is used to retrieve information about the golden carrier mode.

#### Command Syntax

```
show gcmt [<card-type>] [version]
```

#### Command Usage Details

**Table 320: gcmt Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 321: gcmt Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| object | golden carrier modes | string | n/a |
| version | table version | string | n/a |

#### Examples

This example shows how to display the golden carrier information for golden-carrier-mode-800M.95P :

```
show gcmt golden-carrier-mode-800M.95P
```

The following output is displayed:

```
show gcmt golden-carrier-mode-800M.95P
golden-carrier-mode                capacity (Gbps)  client-mode   baud-rate (GBaud)  application  compatibility-id  status
---------------------------------  ---------------  ------------  -----------------  -----------  ----------------  ---------
golden-carrier-mode-CHM6/800M.95P  800              ethernet-otn  95.2965203         P            6                 supported
```

<!-- page 516 -->
