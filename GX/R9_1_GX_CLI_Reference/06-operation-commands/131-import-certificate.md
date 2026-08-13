---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.131. import-certificate'
source_lines: 12237-12319
---

## 6.131. import-certificate

#### Command Description

This command allows to import one or more certificates in PEM format into the NE.

#### Command Syntax

```
import-certificate -h
import-certificate {-i, <certificate type>} [<certificate-name>] <string in PEM format> [<passphrase>]
```

#### Command Usage Details

**Table 349: import-certificate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 350: import-certificate Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -i | Import any intermediate certificates present in a PEM string bundle. |

#### Command Parameters

**Table 351: import-certificateCommand Parameters**

| Parameter | Certificate Type | Description |
| --- | --- | --- |
| intermediate-import (-i) | any or non | It is the certificate types available for import. For bundles, this option enables importing of all intermediate certificates present in the bundle, in addition to the leaf certificate. For files with a single certificate, this enables importing any intermediate certificates specified through the AIA extension. NOTE: Root certificates need to be installed individually. If no certificate-name is given, certificate name will be derived from topmost certificate issuer /CN, plus a numeric suffix. |
| certificate-name | any | Name with which this certificate will be known in the system. |
| string in PEM format | any or none for bundles | String in PEM format with certificate(s) [and private keys] to import. |
| passphrase | trusted-certificate, local-certificate, and bundles | Passphrase to decode encrypted input certificates. |

#### Examples

This example shows to import passphrase-protected local-certificate as PEM string:

```
import-certificate local-certificate certificate-name=local-cert-1 <string in PEM format> <passphrase>
```

This example shows to import peer-certificate as PEM string:

```
import-certificate peer-certificate certificate-name=peer-cert1 <string in PEM format>
```

This example shows to import trusted-certificate as PEM string:

```
import-certificate trusted-certificate certificate-name=trusted-cert-1 <string in PEM format>
```

This example shows to import trusted-certificate as PEM string bundle:

```
import-certificate trusted-certificate certificate-name=bundle-1 -i <string in PEM format>
```

<!-- page 548 -->

This example imports all intermediate certificates as a PEM string bundle. Each certificate name is auto-generated from the CA1 issuer/CN with an incrementing suffix. For instance, if the CA1 issuer is /CN=Nokia, the certificates will be named trusted-certificate-Nokia-1, trusted-certificate-Nokia-2.

```
import-certificate trusted-certificate <string in PEM format> -i
```

This example imports all intermediate certificates and a local certificate as a PEM string bundle. Each certificate name is auto-generated from the CA1 issuer /CN with an incrementing suffix. For example, for CA1 Issuer /CN=Nokia, the local-certificate-Nokia is created.

```
import-certificate trusted-certificate <string in PEM format> -i
```

<!-- page 549 -->
