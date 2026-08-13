---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.196. ntp-key'
source_lines: 15836-15879
---

## 6.196. ntp-key

#### Command Description

These commands are used to add, configure, show and delete NTP keys to be used for NTP authentication.

#### Command Syntax

```
add ntp-key-<key-id> key-type <value> key-value <value> [is-trusted <value>]
set ntp-key-<key-id> [key-type <value>] [key-value <value>] [is-trusted <value>]
show ntp-key-<key-id> [key-type] [key-value] [is-trusted]
delete ntp-key-<key-id>
```

#### Command Usage Details

**Table 478: ntp-key Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 479: ntp-key Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| key-id | NTP Key-ID. Key to be used for NTP authentication. | Number (range: 1..65534) | n/a | add, set, show, delete |
| key-type | The key type. Hash algorithm for NTP message digest computation. | sha-1 aes-cmac sha-256 md5 | n/a | add, set, show |
| key-value | NTP Key-value. | String (length: 8..40) | n/a | add, set, show |
| is-trusted | Indicates a trusted NTP key. | true, false | false | add, set, show |

#### Examples

This example shows how to add an NTP key:

```
add ntp-key-2 is-trusted true key-type sha-1 key-value Infinera
```

<!-- page 727 -->
