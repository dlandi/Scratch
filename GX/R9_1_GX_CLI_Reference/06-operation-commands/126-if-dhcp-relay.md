---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.126. if-dhcp-relay'
source_lines: 11940-11987
---

## 6.126. if-dhcp-relay

#### Command Description

The commands described in this section are used to set or show the `if-dhcp-relay` attributes.

#### Command Syntax

```
set if-dhcp-relay-<if-name> [dhcp-relay-enabled <value>]
show if-dhcp-relay-<if-name> [dhcp-relay-enabled]
```

#### Command Usage Details

**Table 339: if-dhcp-relay Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 340: if-dhcp-relay Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| if-name | The interface object identifier. | String (length 0..255) | n/a | set, show |
| dhcp-relay-enabled | Enables dhcp-relay function on this interface. Obeys global dhcp-relay settings. | • true<br>• false | false | set, show |

#### Examples

The following command shows how to

```
show if-dhcp-relay-1-8-dwdm-line-1GE-OSCX1-MGMT dhcp-relay-enabled
```

The following command shows how to

<!-- page 532 -->

```
set if-dhcp-relay-1-8-dwdm-line-1GE-OSCX1-MGMT dhcp-relay-enabled true
```

<!-- page 533 -->
