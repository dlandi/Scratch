---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.310. ssh-known-host'
source_lines: 23714-23765
---

## 6.310. ssh-known-host

#### Command Description

These commands are used to add, set, show or delete an SSHv2 known hosts entry.

#### Command Syntax

```
add ssh-known-host-<id> address <value> public-key-algorithm <value> public-key <value> [label <value>]
set ssh-known-host-<id> [label <value>]
show ssh-known-host-<id> [address] [public-key-algorithm] [public-key] [label]
delete ssh-known-host-<id>
```

#### Command Usage Details

**Table 720: ssh-known-host Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 721: ssh-known-host Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| id | A unique identifier (name) for this entry. | String (length 0..64 characters) | n/a | add, set, show, delete |
| address | The hostname/IPv4/IPv6 address of the allowed/known peer host. The ipv4-address type represents an IPv4 address in dotted-quad notation. The IPv4 address may include a zone index, separated by a % sign. The zone index is used to disambiguate identical address values. For link-local addresses, the zone index will typically be the interface index number or the name of an interface. If the zone index is not present, the default zone of the device will be used. The canonical format for the zone index is the numerical format. The ipv6-address type represents an IPv6 address in full, mixed, shortened, and shortened-mixed notation. The IPv6 address may include a zone index, separated by a % sign. The zone index is used to disambiguate identical address values. For link-local addresses, the zone index will typically be the interface index number or the name of an interface. If the zone index is not present, the default zone of the device will be used. The canonical format of IPv6 addresses uses the textual representation defined in Section 4 of RFC 5952. The canonical format for the zone index is the numerical format as described in Section 11.2 of RFC 4007. | hostname/IPv4/IPv6 address | n/a | add, show |
| public-key-algorithm | The type of host key algorithm in use. | • ecdsa-sha2-nistp256<br>• ecdsa-sha2-nistp384<br>• ecdsa-sha2-nistp521<br>• ssh-rsa2048<br>• ssh-rsa3072<br>• ssh-rsa4096<br>• ssh-ed25519 i Note: ED25519 suppport is available starting R9.0.<br>• | n/a | add, show |
| public-key | SSHv2 (OpenSSH Portable) host public key component encoded in PEM format: &lt;key type&gt;&lt;SPACE&gt;...base64 encoded OpenSSH public key....&lt;SPACE&gt;&lt;comment&gt;. | String (0..2048 characters) | n/a | add, show |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |

#### Examples

These examples shows how to add an ssh known host:

```
add ssh-known-host-Server_243 address 10.100.210.243 public-key-algorithm ecdsa-sha2-nistp256 public-key
 AAAAE2VjZHNhLXNoYTItbmlzdHAyNTYAAAAIbmlzdHAyNTYAAABBBKLGcqBqXHDdtHGPIyQuT2r+f4UlNhKqHR3CGnGqC2puTKv2HFIpPPr8zbz6K2703vXlqo42gtwFDIdYpw2dfHY=
add ssh-known-host-Server_243_rsa address 10.100.210.243 public-key-algorithm ssh-rsa4096 public-key
 AAAAB3NzaC1yc2EAAAADAQABAAACAQDJgbRZVoZfuwYHe6zoZD9ywqhrk53HwGIoRe49JrSfWzDXHatWqPVceRxapL5PDc7PRAP5TYfnLVSkrTXpkv9M6Tw7RrvLc9ZG2O4/j80HLzMEeRU
mHCPa9po33GQ+pRZpY4o0PBRjbeNhnM8B9od5kUe68vq60/z8H8zsdjh6xFVKt/MPQT9M5+6meBN2AAnD/3RkZnpA0gRLSdmtMJX3F4JrEtapZ+xLGiESqShEjsVR7XcIRcBDBdJWJjxIn1e
phpw5bOuu+pJfxMAETUG3xFjV2jsIbsRwRwWKaIK8ZCRIOQ6SLFLlvyQq+hMVJSUcmAhpq4MaC3z+aMQdgsyYQfemZpccOWQPkR2NGv7Kt2DDiUxbMXLyyM8+KbchrgLuEFmvq/OmGTE50Q1
nDL9Kx3BqyLyiE00G/yVwoKYiPtyFjQBtgGvFzan3sLDH+KEJIFhFw3GYw/nMVDBUhjSR67JVT2IP0JLbb9Tx5TN0AfOm+akETNDstk0znddQaXQGN7lXGalgq1IaEYlkqMmlS8X/WAEFu8Y
T4AbwjvW8VROCg5hrty5l1s0r825q8C7IL9N5hkiLv+fNyM33/LZ7UQZI9YdUejdaq/ebjLW58/mlni93kwPW5/WgYtGF+R+RKsmAMlwuwYhR4MOyAeiu1czVc+oAA2BAHcRcjZ1AEQ==
```

<!-- page 1171 -->
