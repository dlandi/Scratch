---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.181. modules-adg'
source_lines: 15064-15108
---

## 6.181. modules-adg

#### Command Description

These commands are used to add, delete modules to an ADG and to set or show the object attributes.

#### Command Syntax

```
add modules-adg-<adg-number>/<index> supported-card <value> [ocm-monitoring <value>]
set modules-adg-<adg-number>/<index> [supported-card <value>] [ocm-monitoring <value>]
show modules-adg-<adg-number>/<index> [supported-card] [ocm-monitoring]
delete modules-adg-<adg-number>/<index>
```

#### Command Usage Details

**Table 451: modules-adg command usage**

| Section | Description |
| --- | --- |
| User Access Privilege Level | Network Administrator, Network Engineer |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 452: modules-adg Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| adg-number | ADG identifier as a number. | number in the range [1..110] | n/a | add, set, show, delete |
| index | Card within ADG, that is fibered to the degree. Card with index 1 must be the card/ subcard/ module fibered to the Degree(s). | number in the range [1 .. 4] | n/a | add, set, show, delete |
| supported-card | Instance of the card for the ADG. | instance identifier | n/a | add, set, show |
| ocm-monitoring | Set upon creation, cannot be changed after supported-card being assigned. By default, the value is 'true', but can optionally be configured to 'false' for OMD cards directly connected to PAx/ BAX or C2ILASGH degrees (For example, FOADM nodes). | • true<br>• false | true | add, show |

#### Example

This example shows how to associate a card to an Add/Drop Group (ADG) in 1830 GX G30 OADM:

```
add modules-adg-1/1 supported-card card-1-6
```

<!-- page 682 -->
