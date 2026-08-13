---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.309. ssh-keygen'
source_lines: 23648-23713
---

## 6.309. ssh-keygen

#### Command Description

This command is used to generate a ssh private/public key pair. The existing keys in the system will be replaced with newly generated private/public key pair. The key length can be selected with the -b option; if not provided, the system will generate a key with a default length, depending on key type. The default key type is RSA (Rivest–Shamir–Adleman), unless the -t option is provided. RSA supports key lengths of 1024 or 2048 bits (default 2048). ECDSA (Elliptic Curve Digital Signature Algorithm) supports key lengths of 256, 384 or 521 bits (default 256). ED25519 (Edwards-curve Digital Signature Algorithm) supports key length of 256.

**Note:** ED25519 suppport is available starting R9.0.

The default key type is RSA, unless the -t option is provided. The generated public key can then be obtained using the command: \> show ssh-host-key

#### Command Syntax

```
ssh-keygen [-b=<Key-length>] [-l=<key-label>] [-t=<type>]
```

#### Command Usage Details

**Table 718: ssh-keygen Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

<!-- page 1166 -->

#### Command Parameters

**Table 719: ssh-keygen Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| -b=&lt;key-length&gt; | Strength of the key used for regenerating the private-public key pair. | • 2048,3072,4096 for RSA<br>• 256,384,521 for ECDSA.<br>• 256 for ED25519 If secure mode (check security-policies/ secure-mode) is enabled:<br>• RSA only supports key length of 4096 bits<br>• ECDSA supports key lengths of 384 or 521 bits (default 384) If secure mode is disabled:<br>• RSA only supports key length of 2048, 3072 or 4096 bits (default 2048)<br>• ECDSA supports key lengths of 256, 384 or 521 bits (default 256) i Note: ED25519 supports a key-length of 256 irrespective of the secure mode. | n/a |
| -l=&lt;key-label&gt; | Label associated with the key. If no value provided, label will be the value of ne-id. | string | n/a |
| -t=&lt;type&gt; | Specify type of key to generate. | rsa, ecdsa, ed25519 | rsa |

<!-- page 1167 -->

#### Examples

This example shows how to generate a key pair with the default key length and key type:

```
ssh-keygen
```

This example shows how to generate a key pair with a key length of 1024 bits and the default key type:

```
ssh-keygen -b=1024
```

This example shows how to generate a key pair with a key length of 2048 bits and the RSA key type:

```
ssh-keygen -b=2048 -t=rsa
```

This example shows how to generate a key pair with a key length of 521 bits and the ECDSA key type:

```
ssh-keygen -b=521 -t=ecdsa
```

<!-- page 1168 -->
