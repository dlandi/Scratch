---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.29. bgp-instance'
source_lines: 5887-5933
---

## 6.29. bgp-instance

#### Command Description

This command is used to add/edit/show a bgp instance. Use the delete command to delete a bgp instance.

#### Command Syntax

```
add bgp-instance-<instance-id> local-as <value> [description <value>] [router-id-mode <value>] [router-id <value>]
delete bgp-instance-<instance-id>
set bgp-instance-<instance-id> [description <value>] [router-id-mode <value>] [router-id <value>]
show bgp-instance-<instance-id> [description] [vrf] [local-as] [router-id-mode] [router-id <value>]
show bgp
```

#### Command Usage Details

**Table 129: bgp-instance Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 130: bgp-instance Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | BGP instance ID. | string (length 1...255) | n/a | add, set, delete, show |
| local-as | The local autonomous system number that is to be used when establishing sessions with the remote peer or peer group. | number (1 .. 4294967295) | n/a | add, set, show |
| description | Text description | string (length 0...128) | n/a | add, set, show |
| vrf | VRF associated with this BGP instance. | path | n/a | show |
| router-id-mode | Flag to indicate router-id is loopback IP. | use-loopback, manual | use-loopback | add, set, show |
| router-id | Specifies the router ID. 0.0.0.0/0 is not supported for IPv4 and 0::0.0 is not supported for IPv6. | IPv6 or IPv4 address | n/a | add, set, show |

#### Examples

This example shows how to add a bgp instance:

```
add bgp-instance-10 local-as 24 router-id-mode use-loopback
```

<!-- page 238 -->
