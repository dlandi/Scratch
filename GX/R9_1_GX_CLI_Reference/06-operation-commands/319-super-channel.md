---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.319. super-channel'
source_lines: 24454-24506
---

## 6.319. super-channel

#### Command Description

This command is used to display Super Channel configuration attributes.

#### Command Syntax

```
add super-channel-<name> carriers <value> carrier-mode <value> [label <value>] [admin-state <value>] [alarm-report-control <value>]
[contention-check-status <value>]
show super-channel-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state]
[oper-state] [avail-state] [managed-by] [alarm-report-control] [carriers] [carrier-mode] [actual-carrier-mode] [capacity] [client-mode]
[baud-rate] [application] [sop-tracking-mode] [spectral-bandwidth] [contention-check-status]
delete super-channel-<name>
```

#### Command Usage Details

**Table 738: super-channel Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate configuration mode |

#### Command Parameters

**Table 739: super-channel Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Unified channel of optical carriers. Can have many optical channels. | string (length 1.32) | n/a | show |
| admin-state | The administrative state of the managed object | lock, maintenance, unlock | unlock | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | • allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. |  | show |
| oper-state | The operational state of the super channel. | String (length 1..15) | n/a | show |
| avail-state | Availability state of an entity. | in-service, out-of-service, normal, abnormal, low-power, automatic, manual, equipment-not-present, equipment-mismatch, unassigned, faulted, partially-faulted, maintenance, supporting-faulted, facility-failure, auto-in-service, shutdown, in-test, upgrading, incomplete, reserved, active, standby, under-commissioning. | n/a | show |
| carriers | A list of carriers that are bound to this super-channel. | String (length 1..32) | n/a | show |
| label | User-defined label for this super-channel. | String (length 0..256) | n/a | show |
| supporting-card | The name of the card that supports this super channel. | string | n/a | show |
| supporting-port | The port supporting the super channel. | string | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities | string | n/a | show |
| supported-facilities | An XPath reference to the child facilities | string |  | show |
| managed-by | Describes whether this facility was system created or not. Only user created facilities can be user deleted. | system, user | system | show |
| auto-in-service-enabled | Auto-in-service switch for this facility. | enabled, disabled | n/a | show |
| valid-signal-time | Configurable time that represents a detection of a valid signal. Used for auto-in-service mechanism. | minutes range 1..7200 | 480 | show |
| remaining-valid-signal-time | Actual remaining time for this facility to be automatically enabled by the auto-in-service mechanism. | minutes | n/a | show |
| line-system-mode | Indicates the specific mode of power control configured on the L1 transponder, and specifically, on this particular SCG port within the L1 transponder. The attribute indicates the L1 &lt;-&gt; L0 local power controls to adjust the Tx power from the L1 transponder towards the L0 line-system card (such as a WSS or Mux or Amplifier). |  | openwave | show |
| contention-check-status | Contention Check state, set via DNA in openwave mode. Only applicable if openwave-contention-check is enabled at super-channel-group level. | pending, success, overridden, failk | pending | show |
| openwave-contention-check | Enables DNA assisted contention control mechanism in openwave mode. | false, true | false | show |
| expected-total-tx-power | Theoretical total TX power at Faceplate calculated based on per carrier Target TX power value. |  | -55 | show |

<!-- page 1206 -->
