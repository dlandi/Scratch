---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.43. cert-gen'
source_lines: 6632-6681
---

## 6.43. cert-gen

#### Command Description

This command is used to generate a self-signed certificate.

#### Command Syntax

```
cert-gen [-f] [certificate-name=]<value> [[days=]<value>] [[org-name=]<value>] [[common-name=]<value>] [[subject=]<value>] [SAN=]<value>]
[auto-install <true|false>]
```

#### Command Usage Details

**Table 158: cert-gen Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 159: cert-gen Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 160: cert-gen Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| certificate-name | The name of the certificate. | string (length 1..128) | n/a |
| days | Number of days a certificate is valid for. | number | 365 |
| org-name | Organization Name. | string (length 1..64) | n/a |
| common-name | IP or host name to identify the server. | string (length 1..64) | n/a |
| subject | Full certificate subject name. When generating the self-signed certificate, it needs to specify either subject or common-name. | string (length 1..1024) | n/a |
| auto-install | Auto-assign certificate to any secure-application without active certificate. | true, false | true |

#### Examples

This example shows how to generate a self-signed certificate:

```
cert-gen -f certificate-name=self-signed-cert1 days=10 org-name=testorg common-name=testcert auto-install=true
```

<!-- page 284 -->
