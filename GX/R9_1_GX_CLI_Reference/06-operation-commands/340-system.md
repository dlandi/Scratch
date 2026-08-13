---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.340. system'
source_lines: 25668-25796
---

## 6.340. system

#### Command Description

The `set system` command is used to set system attributes including the following:

- clock System clock.
- fdr Flight Data Recorder(FDR)
- file-servers Container of all configured file-servers.
- networking
- ntp Network Time Protocol Configuration
- protocols Container of management protocol objects.
- scheduled-tasks Container of individual user-configurable scheduled co
- security Top level security container.
- sw-services Information about the software services and containers
- syslog
- telemetry Top level configuration and state for the device telemetry system.

**Note:** Some entities are system managed, which means that they cannot be created using the 'set' command.

Using the -m flag performs a merge, which in fact is a best effort add. If the target entity doesn't exist, it will be created; if it exists, it will be updated with any attributes present on the 'add' command. Using the -v flag performs a command validation only (the target entity is not created). If valid, the command replies with 'OK'. Otherwise the command will fail.

**Note:** Where appropriate the set commands are described individually. See the Table of Contents for a list of the documented commands.

The `clear system` command, wipes the system/specific instance and resets to the factory configurations. This action will do a secure wipe of the system data.

<!-- page 1257 -->

The following modes are available:

- factory-reset: Resets the system or a particular equipment to factory configuration. This command will stop target traffic services and remove respective files and user configurations. This may imply loss of connectivity.

**Note:** The factory-reset command:

**▪**does not take effect for 1830 GX G30 Optical Carrier-Cards (OCC2T, OCC2E); **▪**is supported for 1830 GX G30 controller card (FRCU).

**Note:** The `clear system factory-reset` with the `shutdown` option is not supported on the L0 cards.

- full-wipe: Cleans the entire system and reinstall the SW on the controller and the line-cards. This command will stop all traffic services, remove all files, configurations and software from the system. This operation implies loss of connectivity. A base software reinstall from ONIE is required to recover the system.

**Note:** The full-wipe command:

**▪**is not supported for 1830 GX G30 line cards (CHM1R, UTM2) and Optical Carrier-Cards (OCC2T,OCC2E); **▪**is supported for 1830 GX G30 controller card (FRCU) (the clear system full-wipe command in the current release will take effect on FRCU

only).

- inactive: Clear the inactive software partition.

The target may be either the entire system, or a specific chassis or card (via its AID). If no target is provided, the entire system will be cleared. The user may also select the restart-behavior, which may be either a standard restart (default) or a shutdown, in which case the system will not restart after the clear is complete.

**Tip:** For a simple database wipe, please see 'clear database'.

<!-- page 1258 -->

#### Command Syntax

```
show system
set system [-h|-v| -m] <objec-id> [(<attribute> <value>) ...]
clear [-f] system [[action=]<value>] [[restart-behavior=]<value>] [type=]<value> [[target=]<value>]
```

#### Command Usage Details

**Table 781: system Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode (only for set and show commands) |

#### Command Parameters

**Table 782: system Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 783: system Command Parameters**

| Parameter | Description |
| --- | --- |
| action | Action to clean the partition (delete). |
| restart-behavior | The behavior of the restart (restart or shutdown). |
| type | Type of system clearing (factory-reset, full-wipe or inactive). |
| target | Target instance to be cleared, entire system or chassis/card AID. |
| objec-id | object-id list includes: clock/ System clock. file-servers/ Container of all configured file-servers. networking/ ntp/ Network Time Protocol Configuration protocols/ Container of management protocol objects. scheduled-tasks/ Container of individual user-configurable scheduled commands. security/ Top level security container. sw-services/ Information about the software services and containers on the node. syslog/ telemetry/ Top level configuration and state for the sevice telemetry system. |
| attribute | The attribute of the object-id. It depends on the object. |

#### Examples

The following example shows how to perform a secure wipe of the system:

```
clear system full-wipe
```

The following example shows how to perform a secure wipe of the system followed by a shutdown:

```
clear system full-wipe shutdown
```

The following example shows how to perform a factory reset of the entire system followed by a shutdown:

```
clear system factory-reset shutdown
```

The following example shows how to enabled DNS networking:

```
set system networking dns enabled true
```

The following example shows how to enable SNMP on the network element:

```
set system protocols snmp enabled true
```

The following example shows how to enable the syslog log console:

```
set system syslog log-console enabled true
```

<!-- page 1260 -->
