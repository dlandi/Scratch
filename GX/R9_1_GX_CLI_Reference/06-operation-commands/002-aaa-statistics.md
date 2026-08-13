---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.2. aaa-statistics'
source_lines: 4084-4130
---

## 6.2. aaa-statistics

#### Command Description

This command can be used to view the AAA statistics for AAA servers that use the TACACS+ protocol.

#### Command Syntax

```
show aaa-statistics-<server-name> [connection-failures] [authentication-requests] [authentication-rejects] [authorization-requests]
[authorization-rejects] [accounting-requests]
```

#### Command Usage Details

**Table 63: aaa-statistics Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 64: aaa-statistics Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| server-name | Name of the server | string (length 1...64) | n/a | show |
| connection-failures | Displays the number of connection failures, which include failures due to unavailable servers and timeouts. | Integer | n/a | show |
| authentication-requests | Displays the number of authentication requests. i Note: For TACACS+, the default authentication protocol includes both PAP and CHAP. The authentication requests counter tracks each retry and each authentication protocol attempted independently, so authentication requests may increase by 2 for each retry. | Integer | n/a | show |
| authentication-rejects | Displays the number of authentication rejects. | Integer | n/a | show |
| authorization-requests | Displays the number of authorization requests. | Integer | n/a | show |
| authorization-rejects | Displays the number of authorization rejects. | Integer | n/a | show |
| accounting-requests | Displays the number of accounting requests. | Integer | n/a | show |

#### Examples

The following example shows how to view an AAA server's statistics:

<!-- page 132 -->

```
show aaa-statistics-tacacs_server1
```

<!-- page 133 -->
