---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.68. current-subscription'
source_lines: 8284-8322
---

## 6.68. current-subscription

#### Command Description

This command is used to show a list representation of telemetry subscriptions that are configured in the system, otherwise known as current telemetry subscriptions.

#### Command Syntax

```
show current-subscription-<subscription-name> [related-session-id] [related-dial-out-server] [session-type] [session-protocol] [encoding]
[transfer-mode] [updates-only] [user-access]]
```

#### Command Usage Details

**Table 215: current-subscription Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 216: current-subscription Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| subscription-name | User configured identifier of the telemetry subscription. This value is used primarily for subscriptions configured locally on the network element. For dial-in subscription this name is configured by the NBI. | string length 0...128 | n/a |
| related-session-id | Identifier of the telemetry subscription session. | string length 0...128 | n/a |
| related-dial-out-server | Identifier of the subscription dial-out server address. Only applicable to dial-out based subscriptions. | /ne/system/protocols/dial-out-server/name | n/a |
| session-type | Identifier of the type of subscription session. | • gnmi-dial-in-GNMI dial-in session type.<br>• gnmi-dial-out-tunnel-GNMI dial-out via tunnel session type.<br>• gnmi-dial-out-reverse-rpc-GNMI dial-out via reverse RPC session type. | n/a |
| session-protocol | Selection of the transport protocol for the telemetry stream. | gnmi-GNMI protocol session. | gnmi |
| encoding | Specifies the data encoding scheme to be used for data sent to and from the target device. The encoding may be specified for all data, or optionally on a per-RPC basis if supported by the target. | json, bytes, proto, ascii, json-ietf | json-ietf |
| transfer-mode | Specifies the data transfer mode to the target device. | • stream-Values streamed by the target.<br>• once-Values sent once-off by the target.<br>• poll-Values sent in response to a poll request. | stream |
| updates-only | A flag allowing to only send updates to the current state, when set to true the device will not send the initial current value, rather only changes to the initial value. | true, false | false |
| user-access | Username in order to resolve paths according to user access. | string | n/a |

<!-- page 366 -->
