---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.293. simulate'
source_lines: 22802-22854
---

## 6.293. simulate

#### Command Description

This command is used to trigger simulated events in the system (alarms, equipment, etc). **Equipment Simulation** Simulates physical connection or /disconnection of cards or toms with triggers 'plug-in-fru' or 'plug-out-fru'. Requires the holder-AID where the equipment is simulated (matching the card slot, or the tom port). For plugin, the card/tom type needs to be provided, whereas the subtype is optional. The real Hardware always has the subtype well defined. This command is usable both in a simulator and in a real system, but if simulating on real system, the simulation cannot be done if the commands interfere with real HW. For example, a user cannot simulate plugout hardware that is physically present.

**Note:** Plug-out-fru is not supported.

#### Command Syntax

```
simulate -h
simulate [trigger=]<value> ([holder-AID=]<value> [type=]<value> [[subtype=]<value>] | [alarmed-entity=]<value> [alarm-type=]<value>
[[alarm-direction=]<value>] [[alarm-location=]<value>])
```

#### Command Usage Details

**Table 685: simulate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

<!-- page 1128 -->

#### Command Parameters

**Table 686: simulate Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| trigger | The alarm event trigger to simulate:<br>• raise-alarm - Simulates the raising of an alarm.<br>• clear-alarm - Clears a simulated alarm.<br>• plug-in-fru - Simulates the plugin of equipment.<br>• plug-out-fru - Simulates the plugout of equipment. | • raise-alarm<br>• clear-alarm<br>• plug-in-fru<br>• plug-out-fru | n/a |
| holder-AID | AID of the equipment holder (slot or port) where the equipment will be simulated. | string (1..64) | n/a |
| type | Card type. | string | n/a |
| sub-type | Card subtype. | string | n/a |
| alarmed-entity | The entity affected by the alarm; if omitted when clearing alarms, all simulated alarms are cleared. | string | n/a |
| alarm-type | The alarm type to be simulated; if omitted when clearing alarms, all simulated alarms are cleared. | string | n/a |
| alarm-direction | The direction of the simulated alarm. If omitted, system selects direction automatically. | auto-direction selected automatically | auto |
| alarm-location | The location of the simulated alarm. If omitted, system selects location automatically. | auto-location selected automatically | auto |

<!-- page 1129 -->

#### Examples

This example shows how to simulate the plug-out-fru on card 1-5:

```
simulate plug-in-fru 1-5
```

<!-- page 1130 -->
