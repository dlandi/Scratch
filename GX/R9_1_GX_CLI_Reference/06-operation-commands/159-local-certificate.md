---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.159. local-certificate'
source_lines: 13882-13956
---

## 6.159. local-certificate

#### Command Description

These commands are used to set or show the attributes of the X.509v3 end-entity certificate that represents one of various secure application identities. The clear certificate command is used to clear a certificate, for additional information refer to clear (p. 307).

#### Command Syntax

```
set local-certificate-<id> [revocation-mode <value>] [alarm-report-control <value>] [label <value>]
show local-certificate-<id> [version] [serial-number] [subject-name] [issuer] [trust-chain] [valid-from] [valid-to] [modification-time] [status]
[public-key-length] [public-key-type] [signature-key-type] [signature-hash-algorithm] [certificate-bytes] [key-usage] [extended-key-usage]
[revocation-mode] [used-by] [self-signed] [subject-alternative-names] [alarm-report-control] [label]
```

#### Command Usage Details

**Table 406: local-certificate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 407: local-certificate Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| id | A unique object identifier for the certificate. It is a custom type for certificate name. | String (length 1..128) with pattern '([A-Za-z0-9 .,/@][A-Za- _ z0-9 -.,/@]*)' _ | n/a | set, show |
| version | X509 certificate version. | v3 | v3 | show |
| serial-number | ASCII hexadecimal string representing a positive (long) integer assigned by the CA. It must be unique for each certificate issued by a given CA (i.e., the issuer name and\n serial number identify a unique certificate). | String (length 0..100) | n/a | show |
| subject-name | A custom type to represent X.500 distinguished names (DN). The subject field identifies the entity associated with the public key stored in the subject public key field. | String (length 0..1024) | n/a | show |
| issuer | The issuer name identifies the entity that has signed and issued the certificate. Issuers (such as a CA or\n an RA) also issue CRLs. | String (length 0..1024) | n/a | show |
| trust-chain | Lists trusted certificates that constitute this certificate's trust chain. | String | n/a | show |
| valid-from | The date from which the certificate is valid. | String date-time in the format YYYY-MM-DDThh: mm:ssZ see the set-time (p. 1087) command for detailed information. | n/a | show |
| valid-to | The date after which the certificate is deemed to have expired. | String date-time in the format YYYY-MM-DDThh: mm:ssZ see the set-time (p. 1087) command for detailed information. | n/a | show |
| status | The current status of the X509v3 certificate.<br>• If CSR pending - pending-import<br>• If past validity date - expired • If revoked - revoked<br>• If trust chain broken - untrusted<br>• If not yet reached validity period - future<br>• Otherwise - valid | in-use unused revoked expired available | n/a | show |
| public-key-length | X509v3 certificate public key algorithm and supported key length. | enumeration: rsa2048 rsa3072 rsa4096 ecdsa256 ecdsa384 ecdsa521 | n/a | show |
| public-key-type | Public/private key type for X509v3 certificate. | rsa ecdsa | n/a | show |
| signature-key-type | Signature Algorithm key type which signed this X509v3 certificate. | rsa ecdsa | n/a | show |
| signature-hash-algorithm | Hash algorithm used for signing this X509v3 certificate. | sha256 sha384 sha512 | n/a | show |
| certificate-bytes | The number of bytes. A custom type that encodes the entire X.509v3 certificate as string in PEM (base64 encoding) format: -----BEGIN CERTIFICATE----- ...base64 encoded X509v3 certificate.... -----END CERTIFICATE----- | String (length 0..16384) | n/a | show |
| self-signed | True if certificate is self-signed (does not have a trust chain). | true, false | false | show |
| subject-alternative-names | Contains a list of subject alternative name(X509v3 extension SAN) entries separated by &lt;SPACE&gt;&lt;PIPE&gt;&lt;SPACE&gt; delimiters (e.g. 'URI:https:// www.example.com \| DNS:example.com'). | String (length 0..4096) | n/a | show |
| key-usage | Certificate's key usage purposes. | key-usage-type: cRLSign, dataEncipherment, decipherOnly, digitalSignature, encipherOnly, keyAgreement, keyCertSign, keyEncipherment, nonRepudiation | n/a | show |
| extended-key-usage | Certificate's extended key usage purposes. | extended-key-usage-ty pe: clientAuth, codeSigning, timeStamping, serverAuth, emailProtection, OCSPSigning | n/a | show |
| modification-time | Timestamp of certificate installation/rotation. | The timestamp '1970-01-01T00:00:00Z' means the modification time is unknown. | n/a | show |
| revocation-mode | Controls how the revocation status of the certificate is determined. | auto, force-revoked, force-unrevoked | auto | set, show |
| used-by | List of foreign keys representing secure-applications, ikev2-peers, etc., presently using the certificate | instance-identifier | n/a | show |
| alarm-report-control | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>• inhibited - Alarm reporting is inhibited. | • allowed<br>• inhibited | allowed | set, show |
| label | User defined label. | String (length: 0..256) | n/a | set, show |

#### Examples

This example show how to view the attributes of the local-certificate with ID cert:

```
show local-certificate-cert
```

This example show how to set the local-certificate alarm-report-control to inhibited:

```
set local-certificate-cert alarm-report-control inhibited
```

This example show how to set alarm reporting for a local-certificate:

```
set local-certificate-client alarm-report-control allowed
```

<!-- page 627 -->
