---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.148. key-replacement-package'
source_lines: 13240-13282
---

## 6.148. key-replacement-package

#### Command Description

This command is used to show key replacement package (KRP) attributes.

#### Command Syntax

```
show key-replacement-package [KRP-name] [KRP-version] [key-name] [key-serial-number] [issuer-name] [key-length] [key-payload] [KRK-name]
[signature-hash-scheme] [signature-algorithm] [signature-payload] [signature-gen-time] [install-status]
```

#### Command Usage Details

**Table 385: key-replacement-package Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 386: key-replacement-package Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| KRP-name | Identifier for member CPUs on cards starts at 0. | number |
| key-name | The name of the resource | string |
| KRP-version | Package version | string |
| key-serial-number | Key Serial Number | string |
| issuer-name | Name of the CSA (Code Signing Appliance) | string length 0..20 |
| key-length | Key Payload (hex format) | 0..1100 |
| key-payload | Key length in bits | bits |
| KRK-name | Name of the KRK (Image root key) that signed this ISK. | string |
| signature-hash-scheme | Hashing Scheme | SHA2 256, SHA2 384, SHA2 512 _ _ _ |
| signature-algorithm | Signature algorithm | ECDSA, RSA, none |
| signature-payload | Signature payload | hex string 0..1024 |
| signature-gen-time | Signature Generation Time | date-time in the format YYYY-MM-DDThh: mm:ssZ see the set-time command for detailed information. |
| install-status | Indicates if this KRP has been installed in the system. | not-installed. installing, installed, failed |

<!-- page 596 -->
