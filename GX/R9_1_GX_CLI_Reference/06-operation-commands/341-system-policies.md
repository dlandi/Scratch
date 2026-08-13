---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.341. system-policies'
source_lines: 25797-25836
---

## 6.341. system-policies

#### Command Description

The commands described in this section are used to set or show the `system-policies` attributes.The commands associated with the Commit Repository are only available if commit-tracking is enabled.

#### Command Syntax

```
set system-policies [commit-tracking <value>] [writable-running <value>]
show system-policies [commit-tracking] [writable-running]
```

#### Command Usage Details

**Table 784: system-policies Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 785: system-policies Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| commit-tracking | Enables the commit-repository feature. With this feature enabled, all configuration changes done via running or candidate datastores are stored as commit-records, which can then be visualized, compared and rolled back. Disabling this policy will clear the commit-repository of all commit-records. | • enabled<br>• disabled | disabled | set, show |
| writable-running | Disabling writable-running policy makes it impossible to do configure commands via running datastore, making it mandatory to use the candidate datastore. This implies:<br>• CLI no longer allows write commands (add/set/delete) except in Candidate mode<br>• NETCONF allows edit-config only via Candidate Datastore<br>• RESTCONF becomes read-only<br>• gNMI becomes read-only (telemetry is still allowed); gNOI is still allowed<br>• WebGUI becomes read-only<br>• TL1 becomes read-only | • enabled<br>• disabled | disabled | set, show |

#### Examples

The following command shows how to enable the Commit Repository.

```
set system-policies commit tracking enabled
```

<!-- page 1262 -->
