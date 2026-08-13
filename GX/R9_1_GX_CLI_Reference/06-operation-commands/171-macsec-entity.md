---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.171. macsec-entity'
source_lines: 14561-14623
---

## 6.171. macsec-entity

#### Command Description

The commands described in this section are used add, set, show and delete a macsec-entity.

#### Command Syntax

```
add macsec-entity-<name> [supporting-facility <value>] [enabled <value>] [alarm-report-control <value>] [label <value>] [link-security-control
<value>] [replay-protection <value>] [replay-protection-window <value>]
show macsec-entity-<name> [supporting-facility] [enabled] [AID] [oper-state] [alarm-report-control] [label] [link-security-control]
[negotiated-cipher-suite] [replay-protection] [replay-protection-window]
set macsec-entity-<name> [enabled <value>] [alarm-report-control <value>] [label <value>] [link-security-control <value>] [replay-protection
<value>] [replay-protection-window <value>]
delete macsec-entity-<name>
```

#### Command Usage Details

**Table 431: macsec-entity Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 432: macsec-entity Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the macsec entity. | string | n/a | delete |
| supporting-facility | Name of the supporting facility | string | n/a | set, show |
| enabled | Switch to enable or disable the macsec entity. | true, false | true | add, set, show |
| alarm-report-control | Controls the reporting of alarms for a particular object. | allowed, inhibited | Inhibited | add, set, show |
| label | User configurable label for the entity. | string | n/a | add, set, show |
| link-security-control | Controls the link security policy, to handle data packets when MACsec connection is not available | must-secure, should-secure | must-secure | add, set, show |
| replay-protection | Replay protection enable/disable | enable, disable | n/a | add, set, show |
| replay-protection-window | Number of packets to consider for replay protection window | range '0..4294967295' | 0 | add, set, show |

#### Examples

This example shows how to add a macsec entity on 1830 GX network element:

```
add macsec-entity-6-3-3 supporting-facility ethernet-6-3-3
```

This example shows how to enable macsec entity:

```
set macsec-entity-6-3-3 enabled true
```

This example shows how to view macsec entity attributes:

```
show macsec-entity-6-3-3
```

<!-- page 659 -->
