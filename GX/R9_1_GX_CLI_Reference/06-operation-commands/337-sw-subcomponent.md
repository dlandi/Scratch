---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.337. sw-subcomponent'
source_lines: 25468-25518
---

## 6.337. sw-subcomponent

#### Command Description

These commands are used to show the software load subcomponent details.

#### Command Syntax

```
show sw-subcomponent-<location-id>/<swload-state>/<sw-component-name>/<sw-subcomponent-name> [state] [version] [description]
show sw-subcomponent-<swload-state>/<sw-component-name>/<sw-subcomponent-name> [state] [version] [description]
```

#### Command Usage Details

**Table 774: sw-subcomponent Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 775: sw-subcomponent Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| location-id | Location ID (&lt;chassis-id&gt;-&lt;slot-id&gt;) of the SW load subcomponent. | string (length 0..64) | n/a | show |
| swload-state | SW load subcomponent state:<br>• active - Active software load.<br>• inactive - Inactive software load.<br>• installable - Installable software load. | active inactive installable | n/a | show |
| sw-component-name | Component name | string (length 1..256) | n/a | show |
| sw-subcomponent-name | Subcomponent name | string (length 1..256) | n/a | show |
| state | Package state:<br>• installed - Software package installed.<br>• not-installed - Software package not installed.<br>• installation-failed - Software package install failed.<br>• unknown - Software package state unknown. | installed not-installed installation-failed unknown | unknown | show |
| version | Package version. | String (length 0..64) | n/a | show |
| description | Package information. | String (length 0..512) | n/a | show |

#### Examples

This example shows how to display all the SW load components and attributes:

```
show sw-subcomponent
```

This example shows how to display the attributes of a specific SW load component:

```
show sw-subcomponent-1-5/active/frcu31_ztp_pkg/ztp.deb
```

<!-- page 1250 -->
