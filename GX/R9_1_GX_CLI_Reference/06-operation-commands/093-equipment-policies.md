---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.93. equipment-policies'
source_lines: 10130-10189
---

## 6.93. equipment-policies

#### Command Description

These commands are used to set or show the equipment policies attributes. It is used to enable automatic update of tom subtype based on the present equipment.

#### Command Syntax

```
set equipment-policies [tom-auto-migration <value>][auto-assigned-directions <value>] [auto-assigned-degrees <value>] [cable-id-control <value>]
[chassis-assignment-mode <value>] [comm-eth-location <value>]
show equipment-policies [tom-auto-migration] [auto-assigned-directions] [auto-assigned-degrees] [cable-id-control] [chassis-assignment-mode]
[comm-eth-location]
```

#### Command Usage Details

**Table 270: equipment-policies Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 271: equipment-policies Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| tom-auto-migration | Enables automatic update of tom subtype based on present equipment. This update may have direct impact on existing configurations. Note: this has impact on tom subtype migration, but not for tom type migration (e.g. no update between QSFPDD and QSFP28). | enabled disabled | n/a | set, show |
| auto-assigned-directions | Enables/Disables the automatic direction assignment when a card that supports directions is provisioned. By default, it is enabled. | enabled, disabled | enabled | set, show |
| auto-assigned-degrees | Enables automatic degree assignment when a card that supports degree(s) is provisioned. | enabled, disabled | disabled enabled, if l0-mode-op is hsc-ols | set, show |
| cable-id-control | The attribute enables/disable the CableID verification function. The default value depends on the NE l0-mode-op value. A user can manually configure the policy at run time. | enabled, disabled | enabled, if l0-mode-op is slte disabled, if l0-mode-op is standard or hsc-ols | set, show |
| chassis-assignment-mode | Determines if the chassis ID assignment is done manually or automatically. Manual mode - where sub-chassis ID is assigned either via user configuration or ZTP mechanism. | manual | manual | set, show |
| comm-eth-location | Physical location of the communication Ethernet ports. For 1830 GX G31 and 1830 GX G32 chassis, the following values are allowed:<br>• prefer-dcn-in-back - The DCN port is at the back of the chassis.<br>• prefer-dcn-in-front - The DCN port is at the front of the chassis. For 1830 GX G34c chassis, the following values are allowed:<br>• eth5-as-dcn - The DCN port role is assigned to Eth 5 port.<br>• eth5-as-craft - The CRAFT port role is assigned to Eth 5 port. | For 1830 GX G31 and 1830 GX G32 chassis:<br>• prefer-dcn-in-back<br>• prefer-dcn-in-front For 1830 GX G34c chassis:<br>• eth5-as-dcn<br>• eth5-as-craft | For 1830 GX G31 and 1830 GX G32 chassis:<br>• prefer-dcn-in-front For 1830 GX G34c chassis:<br>• eth5-as-craft | set, show |

#### Examples

The following example shows how to view the equipment policies attributes:

```
show equipment-policies
```

The following example shows how to disable automatic update of tom subtype based on present equipment:

```
set equipment-policies tom-auto-migration disabled
```

<!-- page 443 -->

The following example shows how to enable tom auto-migration equipment policies:

```
set equipment-policies tom-auto-migration enabled
```

<!-- page 444 -->
