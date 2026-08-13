---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.286. session'
source_lines: 21301-21356
---

## 6.286. session

#### Command Description

This command is used to show the list of currently established management layer sessions. Only SA users can access the list of all sessions. Remaining users can only see its own session.

**Tip:** RESTCONF sessions will be visible in CLI if using a Cookie Based Authentication. For more information about RESTCONF, refer to *1830 GX* *Management Interfaces User Guide*. By default the session has a keep-alive of 5 minutes, but can be changed by using the `cookie-timeout` attribute, see restconf (p. 1020).

#### Command Syntax

```
show session-<session-id> [session-user] [session-type] [session-protocol] [created-time] [local-ip-address] [dial-out-server-name]
```

#### Command Usage Details

**Table 664: session Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 665: session Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| session-id | Specifies a unique identifier of the current session. It indicates the IP address and transport layer port number associated with this session. If the session is initiated from the serial port, the value is 'NA'. | String | n/a | show |
| session-user | User name associated with this session. | String | n/a | show |
| session-type | Session type. | cli snmp netconf restconf webgui gnmi | n/a | show |
| session-protocol | Indicates which protocol has been used to establish the session. | telnet telnet-raw serial ssh ssh-raw https http | n/a | show |
| created-time | The timestamp the user has created for this session. | date-and-time | n/a | show |
| local-ip-address | Local ip address of the session. | IPv4 or IPv6 address | n/a | show |
| dial-out-server-name | Name of the dial-out-server associated with this session. | Name of the server; leafref (path "../../../protocols/dial-out-server/name") | n/a | show |

#### Examples

This example shows how to view the list of sessions:

```
show session
```

<!-- page 1071 -->

The following output is displayed:

```
session                   session-user session-type session-protocol created-time   local-ip-address
----------------          ------------ ------------ ---------------- -------------  ----------------
session-10.220.116.41:57659  console_user  cli      ssh               2021-05-11T18:45:11Z  10.100.210.190
```

<!-- page 1072 -->
