---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.182. modules-degree'
source_lines: 15109-15152
---

## 6.182. modules-degree

#### Command Description

These commands are used to add, delete modules to a degree and to set or show the object attributes.

#### Command Syntax

```
add modules-degree-<degree-number>/<index> supported-card <value>
set modules-degree-<degree-number>/<index> [supported-card <value>]
show modules-degree-<degree-number>/<index> [supported-card]
delete modules-degree-<degree-number>/<index>
```

#### Command Usage Details

**Table 453: modules-degree command usage**

| Section | Description |
| --- | --- |
| User Access Privilege Level | Network Administrator, Network Engineer |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 454: modules-degree Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| degree-number | Degree number must be greater than zero and not greater than max-degrees. | number in the range [1..20] | n/a | add, set, show, delete |
| index | Card with index 1 must be the card/ subcard/ module fibered to the Degree(s). | 1 | n/a | add, set, show, delete |
| supported-card | Instance of card or subcard that belongs to the degree. In R6.0, the card must be RD09SM or RD20TM: | instance identifier | n/a | add, show |

#### Example

This example shows how to associate a card to a degree in 1830 GX G30 OADM:

```
add modules-degree-1/1 supported-card card-1-1
```

<!-- page 684 -->
