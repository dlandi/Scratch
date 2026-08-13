---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.206. ocsp-server'
source_lines: 16381-16437
---

## 6.206. ocsp-server

#### Command Description

These commands are used to add, edit delete or show the attributes of an Online Certificate Status Protocol (OCSP) server. Establishing an OCSP server includes defining the URL, relative priority, and enabled status of an OCSP responder.

#### Command Syntax

```
add ocsp-server-<name> url <value> priority <value> [enabled <value>]
set ocsp-server-<name> [priority <value>] [enabled <value>]
delete ocsp-server-<name>
show ocsp-server
show ocsp-server-<name> [url] [enabled] [priority] [last-query]
```

#### Command Usage Details

**Table 498: ocsp-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 499: ocsp-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| url | HTTP URL of OCSP responder. The format is 'http:// &lt;host&gt;[:&lt;port&gt;]' where • '&lt;host&gt;' may be IPv4/v6 address, or DNS name of the server hosting the OCSP responder<br>• '&lt;port&gt;' is the optional port number, otherwise default HTTP port is used (80) For example: http:// ocsp.example.or | string length (0..1024) pattern 'http://([^\s/$.?#][^\s/]*)' | n/a | add, set, show |
| priority | This is used to sort the OCSP responders in order of precedence. Lower numbered OCSP responders are consulted before higher numbered ones. | uint8 (range 1..10) | n/a | add, set, show |
| enabled | The flag that controls whether this OCSP server should be consulted for revocation status. | true, false | false | add, set, show |
| last-query | Timestamp of last successful query. | never, date-and-time | n/a | show |

#### Examples

This example shows how to add an OCSP server:

```
add ocsp-server-1 url http://1.2.3.4:8101 priority 3 enabled true
```

This example shows how to display the attributes of an OCSP server:

<!-- page 760 -->

```
show ocsp-server-1
         ocsp-server-1 url 'http://1.2.3.4:8101'
         enabled true priority 3
         last-query '2022-09-26T15:05:20Z'
```

<!-- page 761 -->
