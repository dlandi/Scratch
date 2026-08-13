---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.15. alarm-control'
source_lines: 5101-5152
---

## 6.15. alarm-control

#### Command Description

The commands described in this section are used to set or show the parameters related with alarm management control.

#### Command Syntax

```
set alarm-control [arc-behavior <value>] [alarm-soaking-behavior <value>]
show alarm-control [arc-behavior] [alarm-soaking-behavior]
```

#### Command Usage Details

**Table 100: alarm-control Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 101: alarm-control Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| arc-behavior | System wide alarm-reporting-control (ARC) behavior switch.<br>• clear-alarms: when ARC is set to 'inhibit', clears current alarms.<br>• leave-alarms: when ARC is set to 'inhibit', leaves current alarms in a raised mode. | • leave-alarms<br>• clear-alarms | leave-alarms | set, show |
| alarm-soaking-behavior | System -wide alarm-soaking-behavior switch:<br>• automatic: soaking time used is defined in FM profile.<br>• no-soak: certain alarms specified in FM profile won't have soaking time. i Note: The OSC LOF alarm uses the per-facility user-configurable lof-soak-timer (p. 834) setting for the PBAx card. | • automatic<br>• no-soak | automatic | set, show |

#### Examples

This example shows how to set the alarm control to leave the current alarms in a raised mode:

```
set alarm-control arc-behavior leave-alarms
```

This example shows how to set the alarm control to clear alarms:

```
set alarm-control arc-behavior clear-alarms
```

This example shows how to view the alarm control parameters:

```
show alarm-control
```

<!-- page 184 -->
