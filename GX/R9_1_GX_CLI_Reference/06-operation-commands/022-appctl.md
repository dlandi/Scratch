---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.22. appctl'
source_lines: 5507-5539
---

## 6.22. appctl

#### Command Description

This command is used to control third-party applications.

#### Command Syntax

```
appctl [app-name=]<value> [command=]<value> [[target=]<value>] [[parameters=]<value>[,<value>]*]
```

#### Command Usage Details

**Table 115: appctl Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 116: appctl Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| app-name | Third party app name. | leafref (path /ioa-ne:ne/ioa-ne:system/ioa-ne:sw-management/ioa-ne:third-party-app/ ioa-ne:app-name) | n/a |
| command | Application control commands: restart - Restarts the third party application. netls - Shows the list of subnet networks used by the containers. exec - Execute third party application operation in parameters. | restart netls exec | n/a |
| target | Command executed for the entire system or the chassis/card AID. | string system | system |
| parameters | Optional parameters to be passed in the command with max-elements 50. Applicable when command = 'restart' or command = 'exec'. | string | n/a |

<!-- page 216 -->
