---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.277. secure-entity'
source_lines: 20808-20874
---

## 6.277. secure-entity

#### Command Description

These commands are used to add, edit or show a secure entity. The delete command is used to remove a secure entity from the configuration.

#### Command Syntax

```
add secure-entity-<name> supporting-facility <value> remote-secure-entity <value> [enabled <value>] [alarm-report-control <value>] [label
<value>] [re-key-frequency <value>] [re-key-fail-policy <value>] [traffic-kill-offset <value>]
set secure-entity-<name> [enabled <value>] [alarm-report-control <value>] [label <value>] [re-key-frequency <value>] [re-key-fail-policy <value>]
[traffic-kill-offset <value>]
show secure-entity-<name> [supporting-entity-type] [enabled] [supporting-facility] [remote-secure-entity] [AID] [oper-state]
[alarm-report-control] [label] [re-key-frequency] [re-key-fail-policy] [traffic-kill-offset]
delete secure-entity-<name>
```

#### Command Usage Details

**Table 647: secure-entity Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 648: secure-entity Command Attributes**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name if the secure entity. | string | n/a | add, set, delete, show |
| supporting-facility | Name of the supporting facility | string | n/a | add, set, show |
| remote-secure-entity | AID of the remote optical carrier (for 1830 GX G40) or the remote ODU (for 1830 GX G30) or the remote OTUFlex (CHM7/CHM7x L1 Service encryption) | string | n/a | add, show |
| label | User configurable label for the entity. | string | n/a | add, set, show |
| enabled | Switch to enable or disable the secure entity. | true, false | false | add, set, show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | Inhibited | add, set, show |
| re-key-frequency | The re-key frequency for the IKE security association with the far-end IKE peer. Range and default values may be context-specific. | seconds 3600..86400 | 28800 | add, set, show |
| re-key-fail-policy | Indicates the NE's policy and consequent action when re-keying the IKE security association is unsuccessful. | kill-traffic, continue-traffic | continue-traffic | add, set, show |
| traffic-kill-offset | If the re-key fail policy is set to KILL-TRAFFIC, this attribute indicates the amount of time the system waits before killing all encrypted data security associations that are tied to this IKE SA. | 0..86400 seconds | 0 | add, set, show |

#### Examples

This example show how to add a secure entity on 1830 GX G40:

<!-- page 1044 -->

```
add secure-entity-NE202-1-4-L1-1 supporting-facility optical-carrier-1-4-L1-1 remote-secure-entity '1-4-L1-1' re-key-fail-policy kill-traffic
 traffic-kill-offset 300
```

This example show how to add a secure entity on OTUFlex Facility of CHM7/CHM7x for ODUk service encryption:

```
add secure-entity-s1-1-6-800 supporting-facility odu-1-6-L1-1 remote-secure-entity 1-6-L1-1-OTUflex-1 enabled true
```

This example show how to add a secure entity on 1830 GX G30:

```
add secure-entity-132 supporting-facility odu-1-3-2-ODU4-1 remote-secure-entity 1-4-1-ODU4-1 enabled true
```

<!-- page 1045 -->
