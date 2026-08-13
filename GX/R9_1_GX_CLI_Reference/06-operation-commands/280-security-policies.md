---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.280. security-policies'
source_lines: 20940-21034
---

## 6.280. security-policies

#### Command Description

The commands described in this section are used to edit or show security-policies. The security-policies container, contains the several flags that represent the security policies of the system.

#### Command Syntax

```
set security-policies [secure-mode <value>] [strict-password-check <value>] [minimum-password-length <value>] [ssh-authentication-method
<value>] [default-user-group <value>] [enforce-password-history-check <value>] [password-history-size <value>] [aaa-authentication-method
<value>] [aaa-authorization-method <value>] [ssh-strict-host-key-checking <value>] [ssh-ciphers <value>] [ssh-macs <value>] [ssh-key-exchanges
<value>] [ssh-host-key-algorithms <value>] [ssh-public-key-algorithms <value>] [root-password <value>] [console-user-password <value>]
[console-user-enabled <value>] [csp-symmetrical-key <value>] [disable-user-lockout <value>] [db-passphrase <value>] [supported-tls-version
<value>] [tls-1.2-cipher-suites <value>] [tls-1.3-cipher-suites <value>] [mtls-authentication-method <value>] [ [tls-curves <value>]
[crl-based-revocation <value>] [crl-download-timeout <value>] [ocsp-based-revocation <value>][cert-expiring-warning <value>]
show security-policies [secure-mode] [strict-password-check] [minimum-password-length] [ssh-authentication-method] [default-user-group]
[enforce-password-history-check] [password-history-size] [aaa-authentication-method] [aaa-authorization-method] [ssh-strict-host-key-checking]
[ssh-ciphers] [ssh-macs] [ssh-key-exchanges] [ssh-host-key-algorithms] [ssh-public-key-algorithms] [root-password] [console-user-password]
[console-user-enabled] [csp-symmetrical-key] [max-system-sessions] [max-local-users] [disable-user-lockout] [db-passphrase]
[supported-tls-version] [tls-1.2-cipher-suites] [tls-1.3-cipher-suites] [mtls-authentication-method <value>] [tls-curves] [crl-based-revocation]
[crl-download-timeout] [ocsp-based-revocation][cert-expiring-warning <value>]
```

#### Command Usage Details

**Table 652: security-policies Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 1049 -->

#### Command Parameters

**Table 653: security-policies Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| secure-mode | If enabled, non-secure protocols are not supported. If disabled, non-secure protocols can be used, including: - HTTP protocol for file transfer, REST API, or any other HTTP based application. - FTP protocol for file transfer. - SNMPv2c or SNMPv3 without encryption. Enabling secure-mode will be rejected if any non-secure protocol is in use. | true, false | true | set, show |
| strict-password-check | If enabled, ensures the strict password complexity rules. Including: - minimum length of 8 characters - at least one lower case letter (a-z) - at least one upper case letter (A-Z) - at least one number (0-9) - at least one symbol () - user name cannot be part of the password If disabled, all these rules are not enforced, except: - minimum length is 1 character Once enabled, this policy only has impact on newly defined passwords. | true, false | true | set, show |
| minimum-password-length | Configurable minimum length for user passwords. When a password is changed, the password length will be verified according with this policy. | number (range 1..200) | 8 | set, show |
| ssh-authentication-method | "The method used to authenticate user for SSH access. Note: For two-factor authentication, use public-key method and employ PIN/password-protected hardware device (e.g., smart card or USB token). | • password: Indicates that password based authentication will be used for SSH access.<br>• public-key: indicates that public-key based authentication will be used for SSH access.<br>• public-key-or-password: indicates that if public-key based authentication fails then password based authentication will be used for SSH access. | password | set, show |
| default-user-group | Default roles for users access. | user's access role (MA, NA, SA, PR, NE, EA or TT). | n/a | set, show |
| enforce-password-history-check | If enabled, ensures that a new password being set cannot match any of the previous 5 password for the user. If disabled, password repetition is allowed. Once enabled, this policy only has impact on newly defined passwords. | true, false | true | set, show |
| password-history-size | The number of passwords to store for password reuse checking. | number (range 1..200) | 5 | set, show |
| aaa-authentication-method | Specifies the authentication method for the user login to the NE. | local-only: Authentication locally only. local-first-then-remote: Authentication locally first, if not pass, then use remote AAA server. remote-first-then-local: Authentication use remote AAA server first, if remote authentication failed or all servers could not be contacted, then authenticate locally. remote-unavailable-then-local: Authentication use remote AAA server first, if all servers could not be contacted, then authenticate locally. | local-only | set, show |
| aaa-authorization-method | Specifies the authorization policy for the logged user. If the user changes this parameter, he must logout and login again to apply the rules. | local-only: Authorization locally only. local-first-then-remote: Authentication locally first, if not pass, then use remote AAA server remote-if-authenticated-else-local: Local users must follow local permissions and remote users must follow the remote ones. remote-unavailable-then-local: All users (local or remote) must follow remote permissions. If the permissions return unavailable, then use local ones. | local-only | set, show |
| cert-expiring-warning | Specifies the threshold for raising the CERTIFICATE- EXPIRING alarm, that can be displayed either as days before expiration or as a percentage of the certificate lifetime ('not after date' - 'not before date' interval). For example, if a certificate has: Not Before: as Mar 1 17:22:16 2025 GMT and Not After : as Mar 10 17:22:16 2025 GMT, then that corresponds to a lifetime of 9 days. The percentage values (from 10 to 90%, in steps of 10%) are rounded up to the nearest hour. The default value is 80%. For example, if a certificate is valid for 100 days, then the warning alarm will be raised on the 80th day. Alarm can be disabled globally by setting this policy to 'disabled' by setting the percentage to either 0% or 100%. | true, false | 80% | set, show |
| ssh-strict-host-key-checking | Specify the strictness of remote ssh/sftp/scp host identity checking. | strict: Only allow connection to a remote ssh/sftp/scp host if identity provided by remote host is known. relaxed: Allow connection to a remote ssh/sftp/scp host, regardless if identity provided by remote host is known. | relaxed | set, show |
| ssh-ciphers | Allowed symmetric ciphers for SSH. | aes128-ctr aes192-ctr aes256-ctr aes128-gcm-at-openssh-com aes256-gcm-at-openssh-com chacha20-poly1305-at-openssh-com | n/a | set, show |
| ssh-macs | Allowed message authentication code algorithms for SSH. | hmac-sha2-256, hmac-sha2-512, hmac-sha2-256-etm-at-openssh-com, hmac-sha2-512-etm-at-openssh-com | n/a | set, show |
| ssh-key-exchanges | Allowed key exchange algorithms for SSH. | diffie-hellman-group-exchan ge-sha256, ecdh-sha2-nistp256, ecdh-sha2-nistp384, ecdh-sha2-nistp521,mlkem1 024-sha384,mlkem1024nist p384-sha384,mlkem768-sha 256,mlkem768nistp256-sha 256,mlkem768x25519-sha2 56,sntrup761x25519-sha512 ,sntrup761x25519-sha5 12-at-openssh-com | n/a | set, show |
| ssh-host-key-algorithms | Allowed host key algorithms for SSH. | ssh-rsa, rsa-sha2-256, rsa-sha2-512, ecdsa-sha2-nistp256, ecdsa-sha2-nistp384, ecdsa-sha2-nistp521, ssh-ed25519 | /a | set, show |
| ssh-public-key-algorithms | Allowed public key algorithms for SSH. | ssh-rsa, rsa-sha2-256, rsa-sha2-512, ecdsa-sha2-nistp256, ecdsa-sha2-nistp384, ecdsa-sha2-nistp521, ssh-ed25519, sk-ssh-ed25519-at-openssh-com, x509v3-ssh-rsa, x509v3-rsa2048-sha256, x509v3-ecdsa-sha2-nistp256, x509v3-ecdsa-sha2-nistp384, x509v3-ecdsa-sha2-nistp521, x509v3-ssh-ed25519 | n/a | set, show |
| root-password | The password of the root user. The minimum length of the root password is 1 character. | string (length 0..200) | n/a | set, show |
| console-user-enabled | A switch to enable/disable the console-user. The console-user account is an emergency account that is only usable through the serial console. Disabling this account may put the device in a position where recovery is not possible, so it is recommended to keep this account enabled. | true, false | true | set, show |
| console-user-password | The password of the console-user. The minimum length of the console-user is 1 character. i Note: It is strongly recommended to set a password for the console-user. i Note: When a user logs in via the serial console with the console-user and provides the password, the password is accepted and considered as persistent; the CONSOLE-USER-PASSWORD- NOT-SET alarm is immediately cleared. Users do not need to set the password again. This change applies starting from R9.1 onwards. i Note: It is expected that the first login of the console-user is done via the active NC controller. Doing so in other cards is possible, but will limit the scope of the password change to a local change. | string (length 0..200) | n/a | set, show |
| csp-symmetrical-key | Critical Security Parameters symmetrical key. | string (length 1..32) | prLM9KD9c7AyzAjjQepP | set, show |
| max-system-sessions | The maximum number of management sessions that the system supports. Note: session via serial console does not count against this maximum. | number | n/a | show |
| max-local-users | The maximum number of local users that can be configured in the system. | number | n/a | show |
| db-passphrase | Passphrase used for encrypting and decrypting DB snapshots. For each command associated with DB snapshots (backup, restore, etc), this db-passphrase will be used, except when it is directly provided in each command. Automatic DB snapshots will not be enabled until this parameter is set. | string (length 40..200) | n/a | set, show |
| supported-tls-version | Transport Layer Security (TLS) supported version(s). | • 1.2-only<br>• 1.3-only<br>• 1.3-with-fallback-to-1.2 | 1.2-only | set, show |
| tls-1.2-cipher-suites | Supported TLS 1.2 cipher suites. | • TLS DHE RSA WITH A _ _ _ _ ES 128 CBC SHA256 _ _ _<br>• TLS DHE RSA WITH A _ _ _ _ ES 128 GCM SHA256 _ _ _<br>• TLS DHE RSA WITH A _ _ _ _ ES 256 CBC SHA256 _ _ _<br>• TLS DHE RSA WITH A _ _ _ _ ES 256 GCM SHA384 _ _ _<br>• TLS ECDHE ECDSA W _ _ _ ITH AES 128 CBC SH _ _ _ _ A256<br>• TLS ECDHE ECDSA W _ _ _ ITH AES 128 GCM SH _ _ _ _ A256 • TLS ECDHE ECDSA W _ _ _ ITH AES 256 CBC SH _ _ _ _ A384<br>• TLS ECDHE ECDSA W _ _ _ ITH AES 256 GCM SH _ _ _ _ A384<br>• TLS ECDHE RSA WIT _ _ _ H AES 128 CBC SHA _ _ _ _ 256<br>• TLS ECDHE RSA WIT _ _ _ H AES 128 GCM SHA _ _ _ _ 256<br>• TLS ECDHE RSA WIT _ _ _ H AES 256 CBC SHA _ _ _ _ 384<br>• TLS ECDHE RSA WIT _ _ _ H AES 256 GCM SHA _ _ _ _ 384 | na | set, show |
| tls-1.3-cipher-suites | Supported TLS 1.3 cipher suites. | • TLS AES 128 GCM SH _ _ _ _ A256<br>• TLS AES 256 GCM SH _ _ _ _ A384<br>• TLS CHACHA20 POLY1 _ _ 305 SHA256 _ • TLS AES 128 CCM SH _ _ _ _ A256<br>• TLS AES 128 CCM 8 _ _ _ _ _ SHA256 | na | set, show |
| mTLS Authentication Method | Indicates the user authentication method(s) to use for access to TLS-based applications. | • certificate<br>• certificate-with-fallback-to-password<br>• password | password | set, show |
| tls-curves | Supported elliptic curve algorithms. Applies to both TLS 1.2 and 1.3. i Note: PQC algorithms are supported only for TLS 1.3. | • secp256r1<br>• secp384r1<br>• secp521r1<br>• x25519<br>• x448 PQC algorithms<br>• MLKEM512<br>• MLKEM768<br>• MLKEM1024<br>• SecP256r1MLKEM768<br>• X25519MLKEM768<br>• SecP384r1MLKEM1024 | na | set, show |
| crl-based-revocation | This policy allows to enable/disable CRL-based certificate revocation. | true, false | false | set, show |
| crl-download-timeout | Specifies the maximum time to wait (in seconds) for automatic CRL downloads. Note: This timeout does not apply to manual CRL downloads. | range: 1..60 seconds | 15 | set, show |
| ocsp-based-revocation | This policy defines whether OCSP responders can be consulted for certificate revocation checking. | true, false | false | set, show |

#### Examples

This example shows how to view security policies:

```
show security-policies
```

This example shows how to switch the user authentication method to require X.509 certificate-based authentication for access to all TLS-based applications:

```
set security-policies mtls-authentication-method certificate
```

This example shows how to enable algorithms which is the ssh-public-key-algorithm to include x509v3-ecdsa-sha2-nistp256 for certificate based SSH authentication in security policies:

```
set security-policies ssh-public-key-algorithm
 x509v3-ecdsa-sha2-nistp256,rsa-sha2-256,rsa-sha2-512,ecdsa-sha2-nistp256,ecdsa-sha2-nistp384,sshed25519
```

<!-- page 1060 -->
