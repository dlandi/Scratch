---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.333. sw-container'
source_lines: 25194-25243
---

## 6.333. sw-container

#### Command Description

This command is used to show the list of OS-level containers.

#### Command Syntax

```
show sw-container-<container-name> [equipment] [state] [description] [cpu-usage] [memory-usage] [uptime]
```

#### Command Usage Details

**Table 766: sw-container Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 767: sw-container Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| container-name | A unique Id for each container. | String | n/a | show |
| equipment | Reference to the equipment on which the container is running. | leafref (path "../../../../ equipment/card/AID") | n/a | show |
| state | Current status of the container:<br>• off - Default state of a container, indicates it is not launched yet.<br>• up - Indicates the container is up and running.<br>• exited - Indicates the container has exited. | off up exited | n/a | show |
| description | Brief description of the container instance. | String | n/a | show |
| cpu-usage | Current usage of CPU by the container, in percentage. In a multi-core system, this indicates the overall usage relative to all cores. | Number (range 0 ...100%) | n/a | show |
| memory-usage | Current usage of memory by the container, in percentage. | percentage (range 0 ...100%) | n/a | show |
| uptime | Time since the container started. | String | n/a | show |

#### Examples

This example shows how to view all the OS-level containers and attributes:

```
show sw-container
```

This example shows how to view the attributes of the specific OS-level container:

```
show sw-container-frcu31-1-5_pyapi
```

<!-- page 1241 -->
