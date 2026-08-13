---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.121. grpc'
source_lines: 11719-11771
---

## 6.121. grpc

#### Command Description

These commands are used to enable or show gNMI/gRPC management protocol. This object is managed by the system and can not be manually deleted.

#### Command Syntax

```
set grpc [enabled <value>] [port <value>] [gnmi-get-encoding-granularity <value>]
show grpc [enabled] [port] [gnmi-get-encoding-granularity]
```

#### Command Usage Details

**Table 328: grpc Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 329: grpc Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| enabled | Enables/disables the gRPC management protocol. | true false | true | set, show |
| port | The port which listens for gNMI access via gRPC, where the gRPC is enabled. | number | 50051 | set, show |
| gnmi-get-encoding-granularity | Allows to configure the granularity of data in gNMI Get responses, when encoded with JSON. • per-path - puts all path data on a Update message.<br>• per-object - divides the path data into multiple Update messages, one per YANG container/list entry. | per-path per-object | per-object | set, show |

#### Examples

This example shows how to view the gRPC management protocol attributes:

```
show grpc
```

This example shows how to disable gRPC:

```
set grpc enabled false
```

This example shows how to enable gRPC:

```
set grpc enabled true
```

<!-- page 525 -->
