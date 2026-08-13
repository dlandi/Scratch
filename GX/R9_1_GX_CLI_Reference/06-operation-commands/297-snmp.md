---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.297. snmp'
source_lines: 23026-23075
---

## 6.297. snmp

#### Command Description

These commands are used to set or show the configuration of the SNMP management protocol.

**Note:** The trap-community-string is located in the snmp-target object.

#### Command Syntax

```
set snmp [enabled <value>] [port <value>]
show snmp [enabled] [port] [snmp-engine-id] [engine-boot-count]
```

#### Command Usage Details

**Table 694: snmp Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 695: snmp Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| enabled | User configurable switch to enable or disable global SNMP access. | true, false | true | set, show |
| port | User configurable port where the NE is listening for SNMP requests. | Number (range: 1..65535) | 161 | set, show |
| snmp-engine-id | SNMP EngineID of the NE. The EngineID will follow the EngineID format 3 defined in RFC3411. The MAC address in the Engine ID will be the first MAC address of the MAC addresses Pool of the NE. | String (length 0..256) | n/a | show |
| engine-boot-count | SNMP engine boot count. Counts how many times the engine has restarted. | Number (uint16) | 0 | show |

#### Examples

This example shows how to view SNMP instance:

```
show snmp
```

This example shows how to add an SNMP instance:

```
add snmp enabled true port 161
```

<!-- page 1138 -->
