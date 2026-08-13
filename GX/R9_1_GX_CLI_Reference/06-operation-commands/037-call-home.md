---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.37. call-home'
source_lines: 6321-6379
---

## 6.37. call-home

#### Command Description

This command is used to execute a manual connection trigger to a configured dial-out-server. It forces a connection attempt to a configured dial-out-server. If a dial-out-server is currently 'connecting', the command will force an immediate attempt, and will not wait for some time before retrying.

#### Command Syntax

```
call-home [dial-out-server-name=]<string>
```

#### Command Usage Details

**Table 145: call-home Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |
| Pre-condition | The dial-out server must be configured for this command to execute properly. |

#### Command Parameters

**Table 146: call-home Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 147: call-home Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| dial-out-server-name | The pre-configured name of the dial-out server. | string | n/a |

<!-- page 262 -->

#### Examples

This example shows how to execute the manual connection trigger to a configured dial-out-server:

```
call-home dialoutservername
```

This example shows how to display the call-home help:

```
call-home -h
```

This example shows how to execute the manual connection trigger to dial-out-server with name 'collector-x':

```
call-home collector-x
```

<!-- page 263 -->
