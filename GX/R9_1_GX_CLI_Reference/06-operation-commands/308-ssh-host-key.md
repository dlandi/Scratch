---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.308. ssh-host-key'
source_lines: 23600-23647
---

## 6.308. ssh-host-key

#### Command Description

This command is used to show global (for server and client side SSHv2 based apps) SSHv2 host keys. There must be one host key per supported algorithm. The system auto-generates a host-key in default DB and additional host-keys can be added/overwritten via the ssh-keygen RPC.

#### Command Syntax

```
show ssh-host-key-<public-key-algorithm> [public-key] [label] [fingerprint-algorithm] [fingerprint]
```

#### Command Usage Details

**Table 716: ssh-host-key Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 717: ssh-host-key Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| public-key-algorithm | The type of host key algorithm in use. | ecdsa-sha2-nistp256 ecdsa-sha2-nistp384 ecdsa-sha2-nistp521 ssh-rsa2048 ssh-rsa3072 ssh-rsa4096 ssh-ed25519 i Note: ED25519 suppport is available starting R9.0. | n/a | show |
| public-key | SSHv2 (OpenSSH Portable) host public key component encoded in PEM format: &lt;key type&gt;&lt;SPACE&gt;...base64 encoded OpenSSH public key....&lt;SPACE&gt;&lt;comment&gt;. | String (0..2048 characters) | n/a | show |
| label | User defined label. | String (length: 0..256 characters) | n/a | show |
| fingerprint-algorithm | The type of hash algorithm in use for computing the key fingerprint. | md5 sha256 | n/a | show |
| fingerprint | Fingerprint string as a sequence of pairs of hex digits. SSHv2 public key fingerprint examples for MD5 and SHA256 hash:\n md5sum fingerprint =&gt; b2:9c:cd:30:b1:38:e3:d1:17:d6:73:eb:03:9a:80:83\n sha256sum fingerprint =&gt; f4:61:58:e4:90:65:c4:70:98:7f:d1:40:0a:d8:d9:79:14:e6:91: dc:b6:ed:91:8c:c0:df:d9:65:db:dd:a0:18 | String (length: 0..95 characters) | n/a | show |

#### Examples

This example shows how to view the list of global SSHv2 host keys.

```
show ssh-host-key
```

This example shows how to view a specific global SSHv2 host key.

```
show ssh-host-key-ecdsa-sha2-nistp521
```

<!-- page 1165 -->
