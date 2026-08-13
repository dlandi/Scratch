---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.307. ssh-authorized-key'
source_lines: 23546-23599
---

## 6.307. ssh-authorized-key

#### Command Description

These commands are used to add, set, show an ssh authorized key. Each authorized key entry contains a trusted public key for SSHv2 user authentication. The delete command is used to remove the ssh authorized key from the configuration.

#### Command Syntax

```
add ssh-authorized-key-<user-name>/<key-id> public-key <value> [label <value>]
set ssh-authorized-key-<user-name>/<key-id> [label <value>]
show ssh-authorized-key-<user-name>/<key-id> [public-key-algorithm] [public-key] [label]
delete ssh-authorized-key-<user-name>/<key-id>
```

#### Command Usage Details

**Table 714: ssh-authorized-key Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 715: ssh-authorized-key Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| user-name | User owning the authorized key. Can be local or remote user. | String (0...32) | n/a | add, set, show, delete |
| key-id | A unique identifier (name) for this entry. | String (1...64) | n/a | add, set, show, delete |
| public-key-algorithm | The type of host key algorithm in use. | ecdsa-sha2-nistp256 ecdsa-sha2-nistp384 ecdsa-sha2-nistp521 ssh-rsa2048 ssh-rsa3072 ssh-rsa4096 | n/a | show |
| public-key | SSHv2 (OpenSSH Portable) host public key component encoded in PEM format: &lt;key type&gt;&lt;SPACE&gt;...base64 encoded OpenSSH public key....&lt;SPACE&gt;&lt;comment&gt;. | String (0..2048 characters) | n/a | add, set, show |
| label | User defined label. | string (length 0..256 characters) | n/a | add, set, show |

#### Examples

This example shows how to add an ssh authorized key:

```
add ssh-authorized-key-admin/1 public-key
 AAAAE2VjZHNhLXNoYTItbmlzdHA1MjEAAAAIbmlzdHA1MjEAAACFBACi9u/QTtQVAlVNnjHnvEXsuMW+vCnVSTrxf1wtgzOcrSFwPEqaDBEFqrtaggW40tW18rB9UP31T375EuUqLE/UPQB
mS7TPFKS9WV4KLoNt2p4GvQR87Q81cpW3T78sERrXvu6w1/bOXijeABr25IUQ8lQeTXDppeqAQEOy1Qwcaxyavw==
```

This example shows how to delete an ssh authorized key:

```
delete ssh-authorized-key-admin/1
Are you sure you want to delete [ ssh-authorized-key-admin/1 ]? [y/n] y
```

<!-- page 1163 -->
