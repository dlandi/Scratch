---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.336. sw-service'
source_lines: 25414-25467
---

## 6.336. sw-service

#### Command Description

These commands are used to show the software service running in the system. It displays the information about the software services and containers on the node.

#### Command Syntax

```
show sw-service-<sv-name> [equipment] [location] [state] [state-details] [cpu-usage] [memory-usage] [uptime] [last-start-time] [reboot-count]
show sw-services
```

#### Command Usage Details

**Table 772: sw-service Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 773: sw-service Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| sv-name | A unique Id for each service instance on the NE. Contains card type, shelf, slot information. | String | n/a | show |
| equipment | Reference to the equipment on which the service is running. | leafref (path "../../../../ equipment/card/AID") | n/a | show |
| location | Location where the service is running - host/container info. | • string 'host'; or<br>• string (length 0..128) | n/a | show |
| state | Current status of the service. off - Default state of a service, indicates not being monitored. ok - Indicates the service is ready and functional. fail - Indicates the service failed to launch/turn-up or is unresponsive. | off ok fail | n/a | show |
| state-details | Brief description of the service status. | String | n/a | show |
| cpu-usage | Current usage of CPU by the service, in percentage. In a multi-core system, this indicates the overall usage relative to all cores. | percent | n/a | show |
| memory-usage | Current usage of memory by the service, in percentage. | percent | n/a | show |
| uptime | Time since the service turned up, in days:hours:minutes. | String | n/a | show |
| last-start-time | Time of the last service start/boot. | date-time in the format YYYY-MM-DDThh:mm:ssZ; see the set-time (p. 1087) command for detailed information. | n/a | show |
| reboot-count | The number of times a service has restarted. | uint16 | n/a | show |

#### Examples

This example shows how to display all the information about the software services and containers on the node:

```
show sw-services
```

This example shows how to display the software service DpEncrMgr running in a 1830 GX G30 system:

```
show sw-service-frcu31-1-5_host_DpEncrMgr
```

<!-- page 1248 -->
