---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.173. mka-policy'
source_lines: 14681-14717
---

## 6.173. mka-policy

#### Command Description

The commands described in this section are used add, set, show and delete a mka-policy MACsec Key Agreement (MKA) policy referenced by the macsec entity related to ethernet facility.

#### Command Syntax

```
add mka-policy-<name> [key-server-priority <value>] [macsec-cipher-suite <value>] [confidentiality-offset <value>] [sak-rekey-interval <value>]
show mka-policy-<name> [key-server-priority] [macsec-cipher-suite] [confidentiality-offset] [sak-rekey-interval]
set mka-policy-<name> [key-server-priority <value>] [macsec-cipher-suite <value>] [confidentiality-offset <value>] [sak-rekey-interval <value>]
delete mka-policy-<name>
```

#### Command Usage Details

**Table 435: mka-policy Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 436: mka-policy Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of mka-policy | string | n/a | delete |
| key-server-priority | Key server priority used by MKA protocol to select key-server | string | n/a | set, show |
| macsec-cipher-suite | Cipher suites for Secure Association Key(SAK) derivation | true, false | true | add, set, show |
| confidentiality-offset | The confidentiality offset specifies a number of octets in an Ethernet frame that are sent in unencrypted plain-text | allowed, inhibited | Inhibited | add, set, show |
| sak-rekey-interval | Secure Association Key(SAK) rekey interval in seconds | range '0\|30..65535' | 30s | add, set, show |

<!-- page 663 -->
