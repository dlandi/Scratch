---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.97. est-server'
source_lines: 10339-10385
---

## 6.97. est-server

#### Command Description

This command is used to configure the Enrollment over Secure Transport (EST) server settings.

#### Command Syntax

```
add est-server-<name>/<server-name> server-address <value> [server-port <value>] [priority <value>] [enabled <value>] [path-segment <value>]
delete est-server-<name>/<server-name>
set est-server-<name>/<server-name> [server-address <value>] [server-port <value>] [priority <value>] [enabled <value>] [path-segment
<value>]show est-server-<name>/<server-name> [server-address] [server-port] [priority] [enabled] [path-segment]
```

#### Command Usage Details

**Table 278: est-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 279: est-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| est-server | Configures the Enrollment over Secure Transport (EST) server settings. | N/A | N/A | add, set, delete |
| server-name | Specifies the name of EST server. | N/A | N/A | add, set, delete |
| server-address | Specifies the IP address of EST server. | N/A | N/A | add, set |
| server-port | Specifies the EST server port number. | N/A | 443 | add, set |
| priority | Defines the server's precedence. If omitted, the priority defaults to the current lowest precedence (highest existing number) plus one. If a priority of 10 exists, the value must be configured manually. | The value range is 1 to 10 | N/A | add, set |
| enabled | Specifies whether the switch is enabled for the EST server. | • True<br>• False | True | add, set |
| path-segment | Specifies an optional label added to the EST base url. | string (length 0..64) | N/A | add, set |

#### Examples

The following example shows how to configure the EST server by providing the server address (Default port: 443) :

```
add est-server-1/my-est-server server-address 10.23.55.123 server-port 8443
```

<!-- page 454 -->
