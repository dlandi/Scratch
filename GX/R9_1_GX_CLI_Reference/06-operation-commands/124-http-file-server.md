---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.124. http-file-server'
source_lines: 11857-11900
---

## 6.124. http-file-server

#### Command Description

These commands are used to set/show HTTP file server attributes.

#### Command Syntax

```
set http-file-server [enabled <value>] [http-enabled <value>] [https-enabled <value>] [http-port <value>] [https-port <value>]
show http-file-server [enabled] [http-enabled] [https-enabled] [http-port] [https-port] [url-base]
```

#### Command Usage Details

**Table 335: http-file-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 336: http-file-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| enabled | User configurable switch to enable or disable file server access. | true, false | true | set, show |
| http-enabled | User configurable switch to enable or disable HTTP protocol for file server access. | true, false | false | set, show |
| https-enabled | User configurable switch to enable or disable HTTPS protocol for file server access. | true, false | true | set, show |
| http-port | User configurable HTTP port. | Number | 8980 | set, show |
| https-port | User configurable HTTPS (secure HTTP) port. | Number | 8981 | set, show |
| url-base | The base URL used to redirect to the file transfer application. | String (length: 1..100 characters) | /transfer | show |

#### Examples

This example shows how to view HTTP file servers:

```
show http-file-server
```

<!-- page 530 -->
