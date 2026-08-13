---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.132. inci'
source_lines: 12320-12360
---

## 6.132. inci

#### Command Description

These commands are used to edit or show INCI which is Inter-NE Communication Interface information related to Inter NE inter-op feature. INCI provides API based communication infrastructure for control plane communication across different network elements (For example, Transponder (such as CHM6 on 1830 GX G40) and Line systems (FLEXILS)). INCI uses gRPC server/client as the core mechanism for inter network element communication. When INCI service is enabled, the gRPC server is started and bound to the local IP address. Similarly, when a INCI neighbor instance is configured, gRPC client sessions are established.

#### Command Syntax

```
set inci [inci-enabled <true|false>]
show inci [inci-enabled]
```

#### Command Usage Details

**Table 352: inci Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 353: inci Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| inci-enabled | Switch to enable INCI. | true, false | false | set, show |

#### Examples

This example shows how to enable INCI:

<!-- page 550 -->

```
set inci inci-enabled true
```

<!-- page 551 -->
