---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.359. trusted-certificate'
source_lines: 26835-26900
---

## 6.359. trusted-certificate

#### Command Description

These commands are used to set or show the X509v3 CA (Root and Intermediate) certificate trusted by the system. The clear (p. 307) command (`clear` `certificate`) is used to remove a certificate.

#### Command Syntax

```
set trusted-certificate[-id] [alarm-report-control <value>] [label <value>] [revocation-mode <value>]
show trusted-certificate[-id] [version] [serial-number] [subject-name] [issuer] [trust-chain] [valid-from] [valid-to] [status]
[public-key-length] [public-key-type] [signature-key-type] [signature-hash-algorithm] [certificate-bytes] [alarm-report-control] [label]
[key-usage] [extended-key-usage] [modification-time] [revocation-mode]
```

#### Command Usage Details

**Table 824: trusted-certificate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 825: trusted-certificate Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| id | X509v3 end-entity certificate that represents a one of various secure application identities. | string (length 0..4096) | n/a | set, show |
| version | X509 certificate version. | v3 | v3 | show |
| serial-number | The serial number: an ASCII hexadecimal string representing a positive (long) integer assigned by the CA. It must be unique for each certificate issued by a given CA (i.e., the issuer name and\n serial number identify a unique certificate). | String (length 0..100) | n/a | show |
| subject-name | The subject name. The subject field identifies the entity associated with the public key stored in the subject\n public key field. | String (length 0..1024) | n/a | show |
| issuer | The issuer name identifies the entity that has signed and issued the certificate. Issuers (such as a CA or\n an RA) also issue CRLs. | String (length 0..1024) | n/a | show |
| trust-chain | List of trusted certificates that constitute this certificate's trust chain. | leafref (path "/ne/ system/security/ certificates/trusted-certificate/id") | n/a | show |
| valid-from | The date from which the certificate is valid. | date-and-time | n/a | show |
| valid-to | The date after which the certificate is deemed to have expired. | date-and-time | n/a | show |
| modification-time | Timestamp of certificate installation/rotation. The timestamp '1970-01-01T00:00:00Z' means the modification time is unknown. | date-and-time | n/a | show |
| status | The current status of the X509v3 certificate. | in-use unused revoked expired available pending-import invalid untrusted future valid unsupported | n/a | show |
| public-key-length | X509v3 certificate public key algorithm and supported key length. | rsa2048 rsa3072 rsa4096 ecdsa256 ecdsa384 ecdsa521 | n/a | show |
| public-key-type | Public/private key type for X509v3 certificate. | rsa ecdsa | n/a | show |
| signature-key-type | Signature Algorithm key type which signed this X509v3 certificate. | rsa ecdsa rsassa-pss | n/a | show |
| signature-hash-algorithm | Hash algorithm used for signing this X509v3 certificate. | sha256 sha384 sha512 sha1 | n/a | show |
| certificate-bytes | A custom type that encodes the entire X.509v3 certificate as string in PEM (base64 encoding) format: -----BEGIN CERTIFICATE----- ...base64 encoded X509v3 certificate.... -----END CERTIFICATE----- | String (length 0..16384) | n/a | show |
| key-usage | Certificate's key usage purposes. | cRLSign, dataEncipherment, decipherOnly, digitalSignature, encipherOnly, keyAgreement, keyCertSign, keyEncipherment, nonRepudiation | n/a | show |
| extended-key-usage | Certificate's extended key usage purposes. | clientAuth, codeSigning, timeStamping, serverAuth, emailProtection, OCSPSigning | n/a | show |
| revocation-mode | Controls how the revocation status of the certificate is determined. | auto, force-revoked, force-unrevoked | auto | set, show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |

#### Examples

This example shows how to view the attributes of the trusted-certificate with ID cert:

```
show trusted-certificate-cert
```

This example shows how to set a trusted-certificate alarm reporting to allowed:

```
set trusted-certificate-root alarm-report-control allowed
```

<!-- page 1314 -->
