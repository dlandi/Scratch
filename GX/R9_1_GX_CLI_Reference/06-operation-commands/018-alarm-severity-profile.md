---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.18. alarm-severity-profile'
source_lines: 5263-5306
---

## 6.18. alarm-severity-profile

#### Command Description

This command is used to set or show the alarm severity for a alarm profile. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set alarm-severity-profile <profile-entry> severity <critical|event|major|minor|not-reported|warning>
show alarm-severity-profile
```

**Note:** To display a list of available profiles, press the "?" after `set alarm-severity-profile`.

#### Command Usage Details

**Table 106: alarm-severity-profile Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 107: alarm-severity-profile Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| profile entry | The profile to be modified. | string | n/a | set |
| severity | The assigned severity of the profile. | critical, event, major, minor, not-reported, warning | n/a | set |

#### Examples

This example shows how to set the severity of an alarm profile:

<!-- page 190 -->

```
set alarm-severity-profile alarm-severity-entry-trusted-certificate/CERTIFICATE-EXPIRED severity critical
```

<!-- page 191 -->
