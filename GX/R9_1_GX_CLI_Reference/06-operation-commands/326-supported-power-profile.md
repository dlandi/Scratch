---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.326. supported-power-profile'
source_lines: 24834-24867
---

## 6.326. supported-power-profile

#### Command Description

This command is used to show the supported power-profile attributes for the specified card-type. Different power profiles can be supported to reflect different scenarios when using this card. The user is able to define, per card instance, which profile is in effect. This will have impact on the power estimation for the system.

#### Command Syntax

```
show supported-power-profile-<card-type>/<name> [profile-description] [power-draw] [default]
```

#### Command Usage Details

**Table 752: supported-power-profile Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration Mode |

#### Command Parameters

**Table 753: supported-power-profile Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| card-type | The card type. | string | n/a |
| name | Profile name. | string | n/a |
| profile-description | Description of the profile. | string (length 0...255) | n/a |
| power-draw | Power draw of associated equipment when not in low-power. | decimal64 | n/a |
| default | Whether is the default value or not. | true, false | n/a |

<!-- page 1226 -->
