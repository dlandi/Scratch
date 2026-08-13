---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.356. transfer'
source_lines: 26624-26663
---

## 6.356. transfer

#### Command Description

These commands are used to display information about file transfers. Transfer status will only exist if at least one of the operations of that kind was done for the specified filetype.

#### Command Syntax

```
set transfer [http-proxy <value>]
show transfer [debug-log-optional-content] [http-proxy]
```

#### Command Usage Details

**Table 818: transfer Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate configuration mode |

#### Command Parameters

**Table 819: transfer Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| http-proxy | Proxy server for internally-generated HTTP requests leaving the NE. This includes certificate revocation-related requests, i.e.: CRL downloads and OCSP requests. Note: This proxy is not used for file transfers. | string (length 0..1024) with pattern '((http://)?([^\s/$.?#][^\s/]*))? ' The format is '[http://]&lt;host&gt;[:&lt;port&gt;]' where:<br>• 'http://' is optional,<br>• '&lt;host&gt;' may be the IPv4 address, IPv6 address, or DNS name of the proxy server,<br>• '&lt;port&gt;' is optional. If &lt;port&gt; is omitted, the default is 1080. | n/a | set, show |
| debug-log-optional-content | List of keywords associated with optional content to be selected for debug-log upload. | string (0..64) | n/a | show |

#### Examples

This example shows how to configure the optional proxy server used for internally-generated HTTP requests leaving the NE:

```
set transfer http-proxy http://1.2.3.4:1080
```

<!-- page 1299 -->
