---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.266. restconf'
source_lines: 20309-20365
---

## 6.266. restconf

#### Command Description

These commands are used to set or show configuration of the RESTCONF management protocol.

#### Command Syntax

```
set restconf [enabled <value>] [http-enabled <value>] [https-enabled <value>] [http-port <value>] [https-port <value>] [cookie-timeout <value>]
show restconf [enabled] [http-enabled] [https-enabled] [http-port] [https-port] [cookie-timeout] [api-root]
```

#### Command Usage Details

**Table 625: restconf Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 626: restconf Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| enabled | User configurable switch to enable or disable RESTCONF access. | true, false | true | set, show |
| http-enabled | User configurable switch to enable or disable RESTCONF HTTP access. RESTCONF HTTP access is not supported in secure mode. | true, false | false | set, show |
| https-enabled | User configurable switch to enable or disable RESTCONF HTTPS access. | true, false | true | set, show |
| http-port | User configurable RESTCONF HTTP port. | Number (range: 1..65535) | 8080 | set, show |
| https-port | User configurable RESTCONF HTTPS port. | Number (range: 1..65535) | 8181 | set, show |
| cookie-timeout | Timeout of a cookie based RESTCONF session. The cookie expiration date is reset every time there is activity on the session. | Number (range: 1..300 minutes) | 5 | set, show |
| api-root | Root of the RESTCONF API. | String (length 0..64) | n/a | show |

#### Examples

This example shows how to set the cookie validity of the RESTCONF session to 10 minutes:

```
set restconf cookie-timeout 10
```

This example shows how to view RESTCONF session attributes:

```
show restconf
```

This example shows how to enable RESTCONF:

```
set restconf enabled true
```

<!-- page 1022 -->
