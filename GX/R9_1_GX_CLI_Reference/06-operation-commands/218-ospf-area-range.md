---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.218. ospf-area-range'
source_lines: 17273-17332
---

## 6.218. ospf-area-range

#### Command Description

These commands are used to add, set, show or delete an OSPF area range instance. It is used to summarize routes for an OSPF area matching address/mask. Applicable to Area Border Routers (ABRs) only.

#### Command Syntax

```
add ospf-area-range-<instance-id>/<ospf-area-id>/<prefix> [advertise <value>]
set ospf-area-range-<instance-id>/<ospf-area-id>/<prefix> [advertise <value>]
show ospf-area-range-<instance-id>/<ospf-area-id>/<prefix> [advertise]
delete ospf-area-range-<instance-id>/<ospf-area-id>/<prefix>
```

#### Command Usage Details

**Table 523: ospf-area-range Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | The OSPF instance must be created before the area can be added. |

#### Command Parameters

**Table 524: ospf-area-range Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | OSPF Router instance ID. | instance ID | n/a | add, set, show, delete |
| ospf-area-id | OSPF Router Area ID. | String | n/a | add, set, show, delete |
| prefix | IPv4 or IPv6 prefix. The ipv4-prefix type represents an IPv4 address prefix. The prefix length is given by the number following the slash character and must be less than or equal to 32. A prefix length value of n corresponds to an IP address mask that has n contiguous 1-bits from the most significant bit (MSB) and all other bits set to 0. The canonical format of an IPv4 prefix has all bits of the IPv4 address set to zero that are not part of the IPv4 prefix. The ipv6-prefix type represents an IPv6 address prefix. The prefix length is given by the number following the slash character and must be less than or equal to 128. A prefix length value of n corresponds to an IP address mask that has n contiguous 1-bits from the most\n significant bit (MSB) and all other bits set to 0. The IPv6 address must have all bits that do not belong\n to the prefix set to zero. The canonical format of an IPv6 prefix has all bits of the IPv6 address set to zero that are not part of the IPv6 prefix. Furthermore, the IPv6 address is represented as defined in Section 4 of RFC 5952. | IPv4 or IPv6 prefix | n/a | add, set, show |
| advertise | Advertise or hide. | true, false | true | add, set, show |

#### Examples

This example shows how to add an OSPF area:

```
add ospf-area-range-1/0.0.0.0
```

This example shows how to add the routes matching address/mask:

```
add ospf-area-range-1/9.9.9.9/10.220.0.0/16 advertise true
```

This example shows how to delete the routes matching address/mask:

<!-- page 843 -->

```
delete ospf-area-range-1/9.9.9.9/10.220.0.0/16
Are you sure you want to delete [ ospf-area-range-1/9.9.9.9/10.220.0.0/16 ]? [y/n] y
```

<!-- page 844 -->
