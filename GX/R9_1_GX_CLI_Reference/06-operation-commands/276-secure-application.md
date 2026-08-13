---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.276. secure-application'
source_lines: 20737-20807
---

## 6.276. secure-application

#### Command Description

The commands described in this section are used to edit or show a secure-application or show secure-applications. A secured application represents an application which uses X509v3 certificate as its digital identity.

#### Command Syntax

```
set secure-application-<id> [active-certificate-id <value>] [verify-client-cert <value>]
show secure-application-<id> [type] [active-certificate-id] [in-use] [status] [verify-client-cert]
show secure-applications
```

#### Command Usage Details

**Table 645: secure-application Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 646: secure-application Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| id | A unique object identifier for the secure application. | string (length 0..20) | n/a | set, show |
| in-use | Active certificate for this secure application. | type leafref (path "../active-certificate-id") | n/a | show |
| status | Indicates whether this secure application is enabled or disabled. | • enabled<br>• disabled | n/a | show |
| type | Specifies whether secure application acts as a server or client. | server, client | n/a | show |
| active-certificate-id | List of assigned certificates for this secure application. | string (length 0..128) | n/a | set, show |
| verify-client-cert | Enables or disables TLS Mutual Authentication. Controls client certificate verification behavior at TLS handshake:<br>• disabled - Indicates that client certificate is not requested.<br>• required - Indicates that client certificate is required and validated. For TLS Mutual Authentication, this must be set to 'required'. Note that changes to this attribute will take effect for new TLS connections; it will have no impact on existing connections. | disabled, required | disabled | set, show |

#### Examples

This example shows how to view the parameters of the secured applications:

```
show secure-application
```

This example shows how to view the collection of secured applications:

```
show secure-applications
```

<!-- page 1041 -->

This example shows how to set a secure application:

```
set secure-application-WebGUI active-certificate-id client
```

This example shows how to specify multiple certificates for a secure application:

```
set secure-application-gRPC active-certificate-id certX,certY
```

This example shows how to view the in-use certificate and the status of the secure application:

```
show secure-application-<id> [type] [active-certificate-id] [in-use] [status] [verify-client-cert]
```

<!-- page 1042 -->
