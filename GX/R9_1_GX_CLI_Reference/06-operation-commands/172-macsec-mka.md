---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.172. macsec-mka'
source_lines: 14624-14680
---

## 6.172. macsec-mka

#### Command Description

The commands described in this section are used to add, set, and show and delete a macsec-mka attributes.

#### Command Syntax

```
set macsec-mka-<name> [alarm-report-control <value>] [label <value>] [mka-policy <value>] [connectivity-association-key <value>]
[connectivity-association-key-name <value>] [psk-lifetime <value>] [psk-expiration-warning <value>] [psk-lifetime-enable <value>]
show macsec-mka-<name> [AID] [alarm-report-control] [label] [mka-policy] [is-key-server] [connectivity-association-key]
[connectivity-association-key-name] [psk-configured-timestamp] [psk-lifetime] [psk-expiration-warning] [psk-lifetime-enable]
```

#### Command Usage Details

**Table 433: macsec-mka Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 434: macsec-mka Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| AID | A system-generated AID for the mka object | n/a | n/a | show |
| alarm-report-control | Controls the reporting of alarms for a particular object. | allowed, inhibited | Inhibited | add, set, show |
| label | User configurable label for the entity. | string | n/a | add, set, show |
| mka-policy | mka policy name to use | string | n/a | set, show |
| is-key-server | Used to identify if local end is key server | true, false | false | set, show |
| connectivity-association-key | Pre-shared Connectivity Association Key | string length "0..64" | n/a | set, show |
| connectivity-association-key-name | Pre-shared Connectivity Association Key Name | string length "0..64" | n/a | set, show |
| psk-configured-timestamp | Local NE timestamp when the PSK was configured | true, false | false | set, show |
| psk-lifetimepsk-expiration-warning | Absolute time duration in days after which the PSK will expire | range "1..173" | 14 days | set, show |
| psk-lifetime-enable | Indicates whether PSK lifetime notification is enabled or disabled | enabled, disabled | n/a | set, show |

#### Examples

This example shows how to set macsec-mka connectivity-association-key and connectivity-association-key-name on 1830 GX network element:

```
set macsec-mka-6-3-3 connectivity-association-key 1122334411223344556677889900112233445566778899001122334455667788
 connectivity-association-key-name aa
```

This example shows how to view macsec-mka entity attributes:

```
show macsec-mka-6-3-3
```

<!-- page 661 -->
