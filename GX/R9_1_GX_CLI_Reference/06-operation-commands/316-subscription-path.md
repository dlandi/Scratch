---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.316. subscription-path'
source_lines: 24353-24391
---

## 6.316. subscription-path

#### Command Description

These commands are used to retrieve information subscription-paths.

#### Command Syntax

```
show subscription-path-<subscription-name>/<subscription-path-name> [subscription-path] [subscription-path-origin] [subscription-path-mode]
[sample-interval] [heartbeat-interval] [suppress-redundant]
show subscription-path
```

#### Command Usage Details

**Table 733: subscription-path Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 734: subscription-path Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| subscription-name | User configured identifier of the telemetry subscription. This value is used primarily for subscriptions configured locally on the network element. For dial-in subscription this name is configured by the North Bound Interface (NBI). | string (length 1...128) | n/a | show |
| subscription-path-id | Identifier of the single subscription path in the subscription list. | int32 | n/a | show |
| subscription-path | Specifies a path in the data model path corresponding to the data in the message. | string (length 1.. 520) | n/a | show |
| subscription-path-origin | Specifies the schema tree in order to disambiguate the path. | string (length 1...64) |  | show |
| subscription-path-mode | How subscription updates are sent. | • target-defined<br>• on-change<br>• sample | target-defined | show |
| sample-interval | Time in milliseconds between the device's sample of a telemetry data source. For example, setting this to 2000 would require the local device to collect the telemetry data every 2000 milliseconds. There can be latency or jitter in transmitting the data, but the sample must occur at the specified interval. The timestamp must reflect the actual time when the data was sampled, not simply the previous sample timestamp + sample-interval. Set to 0 when optional. For a target-defined stream subscription, if the sample-interval is not set, the system automatically adjusts its value from 0 (default value) to 10 seconds. | milliseconds | 0 | show |
| heartbeat-interval | Maximum time interval in milliseconds that may pass between updates from a device to a telemetry collector. If this interval expires, but there is no updated data to send (such as if suppress updates has been _ configured), the device must send a telemetry message to the collector. Set to 0 when optional. For a target-defined stream subscription, if the sample-interval is not set and the heartbeat-interval is set to a value lower than 10 seconds, the system automatically adjusts the heartbeat-interval value to 20 seconds. | milliseconds | 0 | show |
| suppress-redundant | Boolean flag to control suppression of redundant telemetry updates to the collector platform. If this flag is set to TRUE, then the collector will only send an update at the configured interval if a subscribed data value has changed. Otherwise, the device will not send an update to the collector until expiration of the heartbeat interval. | true, false | true | show |

<!-- page 1199 -->
