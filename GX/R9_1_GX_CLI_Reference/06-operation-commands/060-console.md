---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.60. console'
source_lines: 7749-7812
---

## 6.60. console

#### Command Description

These commands are used to set or show console attributes.

#### Command Syntax

```
set console-<name> [baud-rate <value>] [local-switch <value>]
show console-<name> [baud-rate] [actual-baud-rate] [auto-sensing-state] [local-switch] [status]
```

#### Command Usage Details

**Table 197: console Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 198: console Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Forces the command without confirmation. |
| -v | Validates the command. |

**Table 199: console Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the card. | String | n/a | set, show |
| baud-rate | The baud rate of console port that is supported by the system (baud). In auto-sensing-mode, the system will auto-detect the baud-rate based on 'ENTER' presses on serial console client side. The detected baud-rate is then locked, and shown in the 'actual-baud-rate' parameter. i Note: not all cards support auto-sensing capability. | auto-sensing 9600 19200 38400 57600 115200 | in 1830 GX G30: 9600 in 1830 GX G40: 115200 | set, show |
| actual-baud-rate | The actual baud-rate for this card's console port. If auto-sensing is enabled, this will reveal the detected baud-rate. If a fixed baud-rate is configured, this will match the configured baud-rate. | 9600 19200 38400 57600 115200 | n/a | show |
| auto-sensing-state | Current state of the auto-sensing mechanism. Only visible if auto-sensing is enabled for this port. i Note: In regard to auto-sensing, the system will auto-detect the baud rate based on 'ENTER' pressed on serial console client side. The detected baud-rate is then locked, and shown in the 'actual-baud-rate' parameter. Not all cards support the auto-sensing capability. | sensing, locked | sensing | show |
| local-switch | Defines the global access to all card's console port: • use-global-switch - Console switch is using the global switch configuration.<br>• force-enable - Console switch is enabled.<br>• force-disable - Console switch is disabled. Access can be overridden per console port at the card level. | use-global-switch force-enable force-disable | use-global-switch | set, show |
| status | Current status of the console for this card. | disabled, enabled | enabled | show |

#### Examples

This example shows how to display all console attributes:

```
show console
```

This example shows how to display the console baud rate value:

```
show console baud-rate
```

This example shows how to enable access control:

```
set console baud-rate 115200 local-switch force-enable
```

<!-- page 344 -->
