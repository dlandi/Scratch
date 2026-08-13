---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.233. password'
source_lines: 18267-18350
---

## 6.233. password

#### Command Description

This command allows a user to change its own password in an interactive way. **Changing own password** Every user can change its own password. It is necessary to provide both the current/old password, as well as the new password. This is supported only for local users. A password may contain up to 200 characters. For resetting other user's password, as an admin, use the 'set user-\<username\> force-password-change true' command. The password input can be done in two ways:

1. Inline with the command: the passwords can be provided as normal parameters, with the disadvantage that they are not hidden from view while being

inputted. There is no interactive prompt afterwards. This method can be used in scripts and automation.

2. Interactively: the passwords will be prompted for input; for security reasons, they are not echoed, so it is necessary to confirm the new password by inputting it

twice. Password input can be cancelled with Ctrl+C. To be used by users directly.

**Tip:** When inputting passwords inline, the standard string rules apply:

  - Special Characters \"' need to be escaped with \
  - Special Characters #?\|; need to be enclosed with quotes (single or double)

For interactive input, passwords can be inputted directly, without considering these rules.

#### Command Syntax

```
password [old-password=<old-password>] [new-password=<new-password>]
```

<!-- page 925 -->

#### Command Usage Details

**Table 555: password Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 556: password Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 557: password Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| old-password | The old password inline with the command. | string | n/a |
| new-password | The the new password inline with the command. | string (up to 200 characters) | n/a |

#### Examples

This example shows how to change the current user password interactively:

```
password
```

The following prompt is displayed:

```
Please provide the old password:
Please provide the new password:
Please confirm the new password:
```

This example shows how to change the current user password inline:

```
password old-password=Infinera4u! new-password=Infinera4us!
```

<!-- page 926 -->

This example shows how to change the current user password inline with special characters:

```
password old-password='Infinera4u#' new-password='Infinera4us#'
```

<!-- page 927 -->
