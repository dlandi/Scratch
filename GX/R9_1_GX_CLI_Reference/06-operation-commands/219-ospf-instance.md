---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.219. ospf-instance'
source_lines: 17333-17381
---

## 6.219. ospf-instance

#### Command Description

These commands are used to add, set, show and delete an OSPF protocol instance. Refer to the section clear (p. 307) to clear an OSPF instance from the configuration asynchronously.

#### Command Syntax

```
add ospf-instance-<instance-id> router-id <value> [version <value>] [description <value>] [router-id-mode <value>]
set ospf-instance-<instance-id> [router-id <value>] [description <value>] [router-id-mode <value>]
show ospf-instance-<instance-id> [router-id] [version] [description] [vrf] [router-id-mode]
delete ospf-instance-<instance-id>
```

#### Command Usage Details

**Table 525: ospf-instance Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Pre-condition | If router-id-mode : use-loopback then ipv4 LO-MGMT address must be configured. |
| Post-condition | None |

#### Command Parameters

**Table 526: ospf-instance Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| instance-id | OSPF instance ID. | uint8 (range: 0 .. 255) | n/a | add, set, show, delete |
| router-id | OSPF Router ID | dotted-quad | n/a | add, set, show |
| version | OSPF version v2 or v3. | ospfv2, ospfv3 | ospfv2 | add, set, show |
| description | Textual description of the OSPF instance. | String (length 0..128) | n/a | add, set, show |
| vrf | VRF to which this interface is bound. | leafref (path "../../../vrf/name") | n/a | show |
| router-id-mode | Flag to indicate router-id is loopback IP or manual configured. | manual, use-loopback | use-loopback | add, set, show |

#### Examples

These examples shows how to add two OSPF protocol instances:

```
add ospf-instance-1 description abc router-id-mode manual version ospfv2 router-id 100.100.1.1
add ospf-instance-2 description xyz router-id-mode use-loopback version ospfv3
```

<!-- page 846 -->
