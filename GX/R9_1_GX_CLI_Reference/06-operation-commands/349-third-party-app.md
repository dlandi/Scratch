---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.349. third-party-app'
source_lines: 26241-26279
---

## 6.349. third-party-app

#### Command Description

This command is used to set or show a third party application.

#### Command Syntax

```
set third-party-app-<app-name> [version <value>] [vendor <value>] [product <value>] [label <value>] [state <value>] [information <value>] [enable
<value>]
show third-party-app-<app-name> [version] [vendor] [product] [label] [state] [information] [enable]
show third-party-app-info-<location-id>/<app-name> [version] [state] [information]
```

#### Command Usage Details

**Table 804: third-party-app Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 805: third-party-app Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| app-name | Third party app name. | string | n/a | add, set, show, delete |
| version | Third party app version. | string 0..64 | n/a | add, set, show |
| vendor | Third party app vendor. | string 0..64 | n/a | add, set, show |
| product | Third party app product. | string 0..256 | n/a | add, set, show |
| state | Third party app state. | running, stopped, failed | stopped | add, set, show |
| information | Third party app information. | string 0..1024 | n/a | add, set, show |
| enable | Third-party-app enabled state. If enabled, app is started(app is enabled also upon system restart). If disabled, app is stopped. | true, false | false | add, set, show |

<!-- page 1280 -->
