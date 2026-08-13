---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.45. certificate'
source_lines: 6739-6805
---

## 6.45. certificate

#### Command Description

This command is used to delete already imported local/trusted/peer X509v3 certificates and to show a list of current certificates and certificate revocations. The show certificates displays all managed local/trusted/peer X509v3 certificates on the system that were imported by download mechanism in PKCS#12 or PKCS#7 secure bundles.

#### Command Syntax

```
clear [-f] certificate [type=]<value> [id=]<value>
show certificate-revocation
show certificates
```

#### Command Usage Details

**Table 163: certificate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 164: certificate Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| type | The certificate type which indicates if the certificate id represents a trusted/local/peer certificate, or any of the purge options below:<br>• purge-all-invalid<br>• purge-all-unused • purge-all-expired<br>• purge-local-unused<br>• purge-peer-unused | trusted, local, peer | n/a | clear |
| id | Certificate ID. The id must match a currently installed but unused certificate of the provided type. Use &lt;tab&gt; to obtain the list of certificate-ids that can be cleared. | string (length 0...128) | n/a | clear |

#### Examples

This example shows how to delete the local-certificate certX:

```
clear certificate local certX
```

This example shows how to delete the trusted-certificate certY:

```
clear certificate type=trusted id=certY
```

This example shows how to display all the system's certificates:

```
show certificates
```

This example shows how to delete a trusted certificate with id=root:

```
clear certificate trusted id=root
Are you sure? [y/n] y
```

This example shows how to delete a local certificate with id=client:

```
clear certificate local id=client
Are you sure? [y/n] y
```

<!-- page 289 -->
