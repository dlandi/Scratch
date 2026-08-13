---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.334. sw-control-rule'
source_lines: 25244-25285
---

## 6.334. sw-control-rule

#### Command Description

These commands are used to add, set or show option service-specific custom rules to override the default action upon service failure. The delete command is used to delete a software control rule from the configuration.

#### Command Syntax

```
add sw-control-rule-<service-name> fail-action <value>
set sw-control-rule-<service-name> [fail-action <value>]
show sw-control-rule-<service-name> [fail-action]
delete sw-control-rule-<service-name>
```

#### Command Usage Details

**Table 768: sw-control-rule Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 769: sw-control-rule Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| service-name | Name of the service to be monitored. | String (length 0..64) | n/a | add, set, delete, show |
| fail-action | The action to be taken. • default-action - performs the policy of restarting the service, then rebooting the system if service not recovered.<br>• ignore - specifies that no automatic action taken in case of service failure.<br>• system-restart - performs a warm restart the system/ card software immediately upon service failure. | default-action ignore system-restart | n/a | add, set, show |

#### Examples

This example shows how to add a software control rule in a 1830 GX G40 node:

```
add sw-control-rule-xmm4-1-1_host_KeyManagement fail-action default-action
```

<!-- page 1243 -->
