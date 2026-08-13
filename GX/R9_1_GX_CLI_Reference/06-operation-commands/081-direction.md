---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.81. direction'
source_lines: 9161-9241
---

## 6.81. direction

#### Command Description

These commands are used to add/edit or show the directions on a multi-rail ILA node. The delete command is used to delete a configured direction. When using the `direction` command, take into consideration the following characteristics:

- direction ***n*** and direction ***n+1*** are automatically assigned by the system to the **DWDM Line1** and **DWDM Line2** ports of the ***#n*** **C2ILASGH/D2ILASGM** card created, where index ***n*** is an odd integer with a value between 1 and 15.
- **DWDM Line1** port is always assigned with an odd index value.
- **DWDM Line2** port is always assigned with an even index value.

#### Command Syntax

```
add direction-<index> direction-port <value> [label <value>]
delete [-f] direction-<index>
set direction-<index> [label <value>]
show direction-<index> [label] [direction-number] [direction-card] [direction-port]
```

#### Command Usage Details

**Table 245: Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

<!-- page 400 -->

#### Command Parameters

**Table 246: Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 247: Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| index | The direction index which the user has adopted (1 and 2 are used when migrating from R6.x). | integer in the range [1..16] | n/a | add, delete, set, show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| direction-number | The 'direction-number' is either 1 or 2. It is set by the system upon the 'direction-port' configuration. This value matches '1' when the 'direction-port' selected is dwdm-line1, and '2' when 'direction-port' selected is dwdm-line2. | • 1<br>• 2 | 1 | show |
| direction-card | The 'direction-card' is set by the system, based on the 'direction-port' that the user has configured. A port is hosted in a card at IOA, the system fills up this value autonomously. | card-&lt;chassis&gt;-&lt;slot&gt; | n/a | show |
| direction-port | Instance of the card's port hosting this direction (index). The 'direction-port' is the dwdm-line1 or dwdm-line2 port instance of the ILAx that the user has assigned the direction index to:<br>• dwdm-line port instances for an 'odd' index should be a dwdm-line1 (direction-number = 1), and for 'even' index, should be dwdm-line2 (direction-number = 2).<br>• The same dwdm-line* port instance (of ILAx) can only be selected once per NE. | • port-&lt;shelf&gt;-&lt;slot&gt;-dwdm-line1<br>• port-&lt;shelf&gt;-&lt;slot&gt;-dwdm-line2 | n/a | add, show |

#### Examples

This example shows all the existing directions:

```
show direction
direction    last-modified  owner-chassis  properties    label  direction-number  direction-card  direction-port
-----------  -------------  -------------  ------------  -----  ----------------  --------------  -------------------
direction-1  0              1              user-managed         1                 card-1-1        port-1-1-dwdm-line1
direction-2  0              1              user-managed         2                 card-1-1        port-1-1-dwdm-line2
direction-3  0              1              user-managed         1                 card-1-3        port-1-3-dwdm-line1
direction-4  0              1              user-managed         2                 card-1-3        port-1-3-dwdm-line2
direction-5  0              1              user-managed         1                 card-1-5        port-1-5-dwdm-line1
direction-6  0              1              user-managed         2                 card-1-5        port-1-5-dwdm-line2
direction-7  0              1              user-managed         1                 card-1-7        port-1-7-dwdm-line1
direction-8  0              1              user-managed         2                 card-1-7        port-1-7-dwdm-line2
```

This example assigns direction-1 to port-1-3-dwdm-line1:

```
add direction-1 direction-port port-1-3-dwdm-line1
```

This example deletes direction-1:

```
delete -f direction-1
```

The -f flag forces the command without confirmation.

<!-- page 402 -->
