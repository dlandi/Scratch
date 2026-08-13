---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.332. sw-component'
source_lines: 25144-25193
---

## 6.332. sw-component

#### Command Description

This command is used to show the software load component details.

#### Command Syntax

```
show sw-component-<location-id>/<swload-state>/<name> [state] [version] [description]
show sw-component-<swload-state>/<name> [state] [version] [description]
```

#### Command Usage Details

**Table 764: sw-component Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 765: sw-component Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| location-id | Location ID (&lt;chassis-id&gt;-&lt;slot-id&gt;) of the equipment SW load subcomponent. | string (length 0..64) | n/a | show |
| swload-state | SW load subcomponent state:<br>• active - Active software load.<br>• inactive - Inactive software load.<br>• installable - Installable software load. | active inactive installable | n/a | show |
| name | Package name. | String (length 0..256 characters) | n/a | show |
| state | Package state:<br>• installed - Software package installed.<br>• not-installed - Software package not installed.<br>• installation-failed - Software package install failed.<br>• unknown - Software package state unknown. | installed not-installed installation-failed unknown | unknown | show |
| version | Package version. | String (length 0..64 characters) | n/a | show |
| description | Package information. | String (length 0..512) | n/a | show |

#### Examples

This example shows how to view all the SW load components and attributes:

```
show sw-component
```

This example shows how to view the attributes of the specific SW load component from a 1830 GX G30 node:

```
show sw-component-1-5/active/frcu31_ztp_pkg
```

<!-- page 1239 -->
