---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.288. set-alarm-state'
source_lines: 21680-21733
---

## 6.288. set-alarm-state

#### Command Description

The set-alarm-state changes the operator state of an alarm. It allows the operator to set an alarm state to a state given by parameter, and also to set some user data in the alarm itself. Sets to the current state of the selected alarms are accepted, with only the text being updated. The user can select 'none', 'ack' and 'close'. The state is mandatory. A user can set a state in all alarms or a specific set of alarms. All alarms ('all-alarms') option sets all raised alarms. The acknowledge text ('acknowledge-text') parameter is optional and inserts an acknowledge message.

#### Command Syntax

```
set-alarm-state -h
set-alarm-state [state=]<value> [[acknowledge-text=]<value>] (all-alarms | [alarm-id-list=]<value>[,<value>]*)
```

#### Command Usage Details

**Table 670: set-alarm-state Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 671: set-alarm-state Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 672: set-alarm-state Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| state | Alarms state. | operator-state | n/a |
| all-alarms | Acknowledge all currently raised alarms. | n/a | n/a |
| alarm-id-list | List of alarm-ids to change the state (from 1 up to 10 alarm ids). | leafref | n/a |
| acknowledge-text | Optional text that will be stored in the alarm. | string (length 0..256) | n/a |

#### Examples

The following command shows how to set alarm state on 28872914984089790 and 17580406225060810165:

```
set-alarm-state ack 28872914984089790,17580406225060810165
```

The following command shows how to acknowledge all alarms:

```
set-alarm-state ack all-alarms acknowledge-text=example
```

<!-- page 1087 -->
