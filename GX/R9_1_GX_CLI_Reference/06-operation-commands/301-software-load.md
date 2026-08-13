---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.301. software-load'
source_lines: 23226-23284
---

## 6.301. software-load

#### Command Description

These commands are used to show the information on the Software Load present in the system.

#### Command Syntax

```
show software-load-<location-id>/<swload-state> [swload-version] [swload-manifest] [swload-prepared] [swload-status] [swload-information]
[swload-activation-type] [swload-vendor] [swload-product] [swload-label] [swload-delta-label] [swload-pkg-type]
show software-load-<swload-state> [swload-version] [swload-manifest] [swload-prepared] [swload-status] [swload-information]
[swload-activation-type] [swload-vendor] [swload-product] [swload-label] [swload-delta-label] [swload-pkg-type]
```

#### Command Usage Details

**Table 702: software-load Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 703: software-load Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| location-id | Location of the equipment. | String (length 0..64) | n/a | show |
| swload-state | SW load subcomponent state. active - Active software load. inactive - Inactive software load. installable - Installable software load. | active inactive installable | n/a | show |
| swload-version | Software load version. | String (length 0..64) | n/a | show |
| swload-manifest | Software load manifest file. Only of relevance for software load installable. | String (length 0..256) | n/a | show |
| swload-prepared | Software load prepared. Only of relevance for software load installable. | true, false | n/a | show |
| swload-status | Software load current status. status-unknown - Software load status unknown. validate-in-progress - Software load validation in progress. validate-complete - Software load validation completed. validate-failed - Software load validation failed. apply-in-progress - Software load apply in progress. apply-complete - Software load apply completed. apply-failed - Software load apply failed. activate-in-progress - Software load activation in progress. activate-failed - Software load activation failed. activate-complete - Software load activation completed. cancel-in-progress - Software load cancel in progress. cancel-failed - Software load cancel failed. cancel-complete - Software load cancel completed validate-timeout - Software load validate timeout. apply-timeout - Software load apply timeout. activate-timeout - Software load activate timeout. cancel-timeout - Software load cancel timeout. | status-unknown validate-in-progress validate-complete validate-failed apply-in-progress apply-complete apply-failed activate-in-progress activate-failed activate-complete cancel-in-progress cancel-failed cancel-complete validate-timeout apply-timeout activate-timeout cancel-timeout | n/a | show |
| swload-information | Software load information. | String (length 0..1024) | n/a | show |
| swload-activation-type | Software load activation type. Only of relevance for software load state installable. direct - No reboot type determined. warmstart - Update requires warm reboot. coldstart - Update requires cold reboot. | direct warmstart coldstart | n/a | show |
| swload-vendor | Software load vendor. | String (length 0..256) | n/a | show |
| swload-product | Software load product. | String (length 0..256) | n/a | show |
| swload-label | Software load label. | String (length 0..256) | n/a | show |
| swload-delta-label | Software load delta label. | String (length 0..256) | n/a | show |
| swload-pkg-type | Software load package type | String (length 0..256) | n/a | show |

#### Examples

This example shows how to view software load with active state:

```
show software-load-active
```

This example shows how to view software load on card located on chassis 1, slot 5, with active sw-load state:

```
show software-load-1-5/active
```

<!-- page 1148 -->
