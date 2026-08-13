---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.362. update'
source_lines: 26981-27033
---

## 6.362. update

#### Command Description

This command is used to update a specific object attribute(s), with dependence on the provided \<type\>.

**Tip:** Select multiple instances by using wildcard (\*)

#### Command Syntax

```
update [type=]<value> [[entity-id=]<value>[,<value>]*]
```

#### Command Usage Details

**Table 830: update Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 831: update Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 832: update Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| type | Type of update: • span-loss-alarm-threshold - updates the OTS attribute span-loss-alarm-threshold.<br>• filter-insertion-date-now - updates the chassis's dust filter insertion date to the current date.<br>• set-under-commissioning - sets the entity to be under commissioning state. This is applicable to NEs entities in SLTE systems.<br>• clear-under-commissioning - sets the entity to be Ready for Service state. This is applicable to NEs entities in SLTE systems. | • span-loss-alarm-threshold<br>• filter-insertion-date-now • set-under-commissioning<br>• clear-under-commissioning | n/a |
| entity-id | Instance(s) for the required update. | &lt;entity-id&gt; | n/a |

#### Examples

The following example shows how to re-evaluate the span-loss-alarm-threshold attribute:

```
update span-loss-alarm-threshold ots-1-1-dwdm-line
```

The following example shows how to set the filter insertion date to the current date:

```
update filter-insertion-date-now chassis
```

<!-- page 1319 -->
