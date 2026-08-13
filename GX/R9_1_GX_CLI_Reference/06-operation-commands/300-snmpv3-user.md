---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.300. snmpv3-user'
source_lines: 23177-23225
---

## 6.300. snmpv3-user

#### Command Description

These commands are used to add, set, show or delete a list of SNMP V3 user.

#### Command Syntax

```
add snmpv3-user-<snmpv3-user-name> auth-passphrase <value> priv-passphrase <value> [user-sec-level <value>] [auth-protocol <value>]
[priv-protocol <value>]
set snmpv3-user-<snmpv3-user-name> [user-sec-level <value>] [auth-protocol <value>] [auth-passphrase <value>] [priv-protocol <value>]
[priv-passphrase <value>]
show snmpv3-user-<snmpv3-user-name> [user-sec-level] [auth-protocol] [auth-passphrase] [priv-protocol] [priv-passphrase]
delete snmpv3-user-<snmpv3-user-name>
```

#### Command Usage Details

**Table 700: snmpv3-user Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Related Commands | add snmp-target (p. 1140), add snmp-community (p. 1138) |

#### Command Parameters

**Table 701: snmpv3-user Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| snmpv3-user-name | The SNMP Version 3 user name | String (length 1...32) | n/a | add, set, delete, show |
| user-sec-level | Specifies the SNMPv3 user security level. | auth-no-priv auth-priv no-auth-no-priv | no-auth-no-priv | add, set, show |
| auth-protocol | Specifies the authentication protocol that the SNMPv3 user being created will use. | SHA | SHA | add, set, show |
| auth-passphrase | Specifies the SNMPv3 authentication pass phrase. | String (length 8...64) | n/a | add, set, show |
| priv-protocol | Specifies the privacy protocol that the SNMPv3 user being created will use. | DES, AES128, AES192, AES256 | AES128 | add, set, show |
| priv-passphrase | Specifies the SNMPv3 privacy pass phrase. | String (length 8...64) | n/a | add, set, show |

#### Examples

This example shows how to add an SNMP V3 user:

```
add snmpv3-user-bob user-sec-level auth-priv auth-protocol SHA auth-passphrase public123 priv-protocol AES128 priv-passphrase private123
```

<!-- page 1145 -->
