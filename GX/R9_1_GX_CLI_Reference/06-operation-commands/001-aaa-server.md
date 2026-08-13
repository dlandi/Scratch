---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.1. aaa-server'
source_lines: 4019-4083
---

## 6.1. aaa-server

#### Command Description

This command is used to add/edit/show an AAA server. Use the delete command to delete an AAA server.

**Note:** The maximum length of shared-secret is 64 characters. Any value more than 64 characters is denied with an error message.

#### Command Syntax

```
add aaa-server-<server-name> protocol-supported <value> server-address <value> [server-priority <value>] [transport <value>] [server-port
<value>] [server-port-authentication <value>] [server-port-accounting <value>] [shared-secret <value>] [role-supported <value>] [enabled <value>]
[timeout <value>] [retry <value>] [source-ip <value>] [common-password <value>][radius-options <value>]
set aaa-server-<server-name> [server-priority <value>] [server-address <value>] [transport <value>] [server-port <value>]
[server-port-authentication <value>] [server-port-accounting <value>] [shared-secret <value>] [role-supported <value>] [enabled <value>] [timeout
<value>] [retry <value>] [source-ip <value>] [common-password <value>] [radius-options <value>]
show aaa-server-<server-name> [server-priority] [protocol-supported] [server-address] [transport] [server-port] [server-port-authentication]
[server-port-accounting] [shared-secret] [role-supported] [enabled] [timeout] [retry] [source-ip] [common-password] [radius-options]
delete aaa-server-<server-name>
```

<!-- page 128 -->

#### Command Usage Details

**Table 61: aaa-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 62: aaa-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| server-name | Name of the server | string (length 1...64) | n/a | add, set, delete, show |
| common-password | Password used for RADIUS authorization after SSH public key authentication. If blank, username is reused as password for RADIUS authorization. | string (length 1...128) | n/a | add, set, show |
| server-priority | This is used to sort the servers in the order of precedence. If not provided, the server priority will be set to the lowest precedence (highest number) already existing in other AAA servers, plus 1. For example, if an AAA server with a server priority of 7 already exists and the server-priority is not specified, the new server will have its server-priority set to 8. If an AAA server with a server priority of 10 exists, this priority value must be set manually. | n/a | Number range 1-10 | add, set, show |
| protocol-supported | Specifies the protocol used for AAA. | RADIUS, TACACSPLUS | RADIUS | add, set, show |
| server-address | The IP address of AAA. | IP address | n/a | add, set, show |
| server-port | The AAA server port number. | Number | 49 | add, set, show |
| server-port-authentication | AAA server authentication port number. | Number | 1812 | add, set, show |
| server-port-accounting | AAA server accounting port number. | Number | 1813 | add, set, show |
| shared-secret | The shared secret of the aaa server. The shared secret will be displayed as *. | String (length 0...128) | sharedkey | add, set, show |
| role-supported | The configured roles for the AAA server. | accounting, authentication, authorization | authentication authorization accounting | add, set, show |
| enabled | Enable switch for this aaa-server. | true, false | true | add, set, show |
| source-ip | Source IP address used for RADIUS communications. | IP address | n/a | add, set, show |
| timeout | Specifies the response timeout of Access-Request messages sent to a AAA server in seconds. | Number (range 1..90, seconds) | 5 | add, set, show |
| retry | Specifies the number of attempted Access-Request messages to a single AAA server before failing authentication. | Number (range 0..5) | 1 3 | add, set, show |

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 1 The total wait time timeout x (retry + 1) should be less than 5 minutes.

<!-- page 130 -->

#### Examples

The following example shows how to add and enable an AAA server:

```
add aaa-server-server1 server-priority 1 protocol-supported RADIUS server-address 10.10.10.10
```
