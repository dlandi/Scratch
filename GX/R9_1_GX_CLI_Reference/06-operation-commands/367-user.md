---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.367. user'
source_lines: 27388-27466
---

## 6.367. user

#### Command Description

These commands are used to add, set, show or delete users and attributes.

#### Command Syntax

```
add user-<user-name> [password <value>] [password-hashed <value>] [user-group <value>] [display-name <value>] [max-invalid-login <value>]
[suspension-time <value>] [timeout <value>] [password-aging-interval <value>] [enabled <value>] [force-password-change <value>] [max-sessions
<value>] [alarm-report-control <value>] [label <value>]
set user-<user-name> [password <value>] [password-hashed <value>] [user-group <value>] [display-name <value>] [max-invalid-login <value>]
[suspension-time <value>] [timeout <value>] [password-aging-interval <value>] [enabled <value>] [force-password-change <value>] [max-sessions
<value>] [alarm-report-control <value>] [label <value>]
show user-<user-name> [password] [password-hashed] [user-group] [display-name] [max-invalid-login] [suspension-time] [timeout]
[password-aging-interval] [password-expiration-date] [enabled] [user-status] [force-password-change] [max-sessions] [last-login-date]
[failed-logins] [user-aaa-type] [alarm-report-control] [label]
delete user-<user-name>
```

#### Command Usage Details

**Table 842: user Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 843: user Command Attributes**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| user-name | The name of the user to be added. | String (1...32) | n/a | add, set, delete |
| password | The password for this user. | String (max 200 characters) | n/a | add, set, show |
| password-hashed | Hashed password of the user. It is made of three mandatory fields, where the dollar sign is the field separator. The structure is: $id$salt$encrypted. Only id 6 (SHA512) is supported. Salt minimum size is 2. | string (length 0..106) | n/a | add, set, show |
| user-group | The associated user groups. See the section User groups and access privilege (p. 89) for more information. | EA, MA, NA, NE, PR, SA, TT, and user-defined | n/a | add, set, show |
| display-name | The display name for this user. | string (0-128) | n/a | add, set, show |
| max-invalid-login | This attribute is the maximum number of consecutive and invalid login attempts before an account is suspended (locked out). | number (range 0-255) | 5 | add, set, show |
| suspension-time | This attribute is the duration of UID suspension following consecutive invalid login attempts. | minutes (range 0 - 1440) | 5 | add, set, show |
| timeout | This attribute is the Session Time Out Interval. | minutes (range 0 - 1440) | 60 | add, set |
| password-aging-interval | This attribute is the Password Aging Interval. | number (range 0 to 999 days) | 90 | add, set, show |
| password-expiration-date | This attribute shows the password expiration date. | string (example: 1970-01-01T00:00:00Z) | n/a | show |
| enabled | Enable switch for the user, allows admins to explicitly disable users. | false, true | true | add, set, show |
| user-status | This attribute shows the user status. User with status 'enabled' will have access to the system. User with status 'disabled' not have access to the system. User with status 'password-aged' will have access to the system but will be forced to change his password on first-time login. User with status 'lockout' means the account is locked out due to unsuccessful login attempts. | enabled disabled password-aged lockout | disabled | show |
| force-password-change | Allows administrator to force user to change password on next login. | true, false | false | add, set, show |
| max-sessions | This attribute specifies the maximum number of sessions allowed for this user. | number (range 1-20) | 10 | add, set, show |
| last-login-date | The last login date/time of the user. | date-and-time | 1970-01-01T00:00:00Z | show |
| failed-logins | Number of previous failed logins. Resets to zero upon a successful login. | number | 0 | show |
| user-aaa-type | Indicates the authentication method of the user. | remote, local | local | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | add, set, show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |

#### Examples

This example shows how to add new instance of a user named user2 :

```
add user-user2 max-sessions 3 suspension-time 5 timeout 30 user-group SA,NA,TT
```

This example shows how to add new instance of type 'user', initializing attribute max-sessions directly :

```
add user-john max-sessions 3
```

<!-- page 1339 -->

This example shows how to test whether it is possible to add a new instance of type 'user' with id 'smith':

```
add -v user-smith
```

<!-- page 1340 -->
