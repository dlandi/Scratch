---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.255. pump'
source_lines: 19726-19766
---

## 6.255. pump

#### Command Description

These commands are used set up a pump.

#### Command Syntax

```
set pump-<name> [label <value>] [admin-state <value>] [alarm-report-control <value>]
show pump-<name> [supporting-card] [supporting-port] [supporting-facilities] [supported-facilities] [AID] [label] [admin-state] [oper-state]
[avail-state] [managed-by] [alarm-report-control] [pump-type]
```

#### Command Usage Details

**Table 602: pump Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 603: pump Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| alarm-report-control | Controls the reporting of alarms for this particular object. | • allowed<br>• inhibited | Allowed | add,set, show |
| name | The name of the pump | string | n/a | add, set, show, |
| admin-state | The administrative state of pump | lock, unlock, maintenance | n/a | add, set, show |
| label | User-defined label for the pump. | String (length 0..256) | n/a | add, set, show |
| supporting-card | Card that holds this facility | string | n/a | show |
| supporting-facilities | An XPath reference to the parent facilities. | string | n/a | show |
| AID | The AID of the pump | string | n/a | show |
| oper-state | The operational state of the pump | enabled, disabled | n/a | show |
| avail-state | The available state of the pump | in-service, out-of-service, normal, abnormal, low-power | n/a | show |
| managed-by | Describes whether this facility was system created or not. Only user created facilities can be user deleted. | system, user | n/a | show |

<!-- page 994 -->
