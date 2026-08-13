---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.298. snmp-community'
source_lines: 23076-23122
---

## 6.298. snmp-community

#### Command Description

These commands are used to add, set, show or delete an SNMP community. This command adds a list of SNMP Community Strings.

**Note:** The trap-community-string is located in the snmp-target object.

#### Command Syntax

```
add snmp-community-<name> community-string <value> [enabled <value>] [community-string-access <value>]
set snmp-community-<name> [community-string <value>] [enabled <value>] [community-string-access <value>]
show snmp-community-<name> [community-string] [enabled] [community-string-access]
delete snmp-community-<name>
```

#### Command Usage Details

**Table 696: snmp-community Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Related Commands | add-snmp-target (p. 1140), add snmp3-user (p. 1143) |

#### Command Parameters

**Table 697: snmp-community Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name for the community (different from the community string itself). | String (length 0..64) | n/a | add, set, show, delete |
| community-string | The community string. | String (length 1..32) | n/a | add, set, show |
| enabled | User configurable switch to enable or disable this community-string. | false, true | true | add, set, show |
| community-string-access | SNMP access right of this community string. | read-only | read-only | add, set, show |

#### Examples

This example shows how to add an SNMP community:

```
add snmp-community-mycommunity community-string public community-string-access read-only enabled true
```

<!-- page 1140 -->
