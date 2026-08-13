---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.96. est-ca'
source_lines: 10297-10338
---

## 6.96. est-ca

#### Command Description

This command is used to represent a Certificate Authority (CA) which is set for Enrollment over Secure Transport (EST).

#### Command Syntax

```
set est-ca-<name> [explicit-ca-root <value>] [root-fingerprint <value>] [auto-re-enrollment <value>] [label <value>]
show est-ca-<name> [explicit-ca-root] [root-fingerprint] [auto-re-enrollment <value>] [label]
```

#### Command Usage Details

**Table 276: est-ca Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 277: est-ca Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| est-ca | Specifies the CA instance used for EST certificate enrollment. | N/A | N/A | set, show |
| explicit-ca-root | Indicates the trusted root certificate for the EST CA. | N/A | N/A | set, show |
| root-fingerprint | Verifies the identity of the Root CA using a SHA-256 or SHA-512 hash to ensure a secure initial connection for EST certificate enrollment. | N/A | N/A | set, show |
| auto-re-enrollment | Specifies the number of days before expiration at which re-enrollment will be performed for all leaf certificates issued by this EST CA. This number can also be specified as a percentage of the 'not after date' - 'not before date' interval, rounded up. Accepted values are 10 to 90% in increments of 10%. By default, automatic re-enrollment is disabled.​ If auto re-enrollment fails for any reason, the RE-ENROLL- FAIL alarm will be raised for the affected certificate and re-enrollment will be retried once per day until either successful or the certificate becomes invalid, e.g. due to expiration. | The possible values can be;<br>• number of days before expiration (from 1-365) OR<br>• percentage of elapsed certificate lifetime (10% - 90% in 10% increments), or 'disabled' For example; if a user selects;<br>• 10% - it will raise the alarm when 10% of the certificate life-time has passed.<br>• 10 - it will raise the alarm 10 days before the certificate becomes expired.<br>• disabled - the alarm is not raised. | Disabled | set, show |

#### Examples

The following example shows how to set the auto-re-enrollment:

```
set est-ca-1 auto-re-enrollment 40
```

<!-- page 452 -->
