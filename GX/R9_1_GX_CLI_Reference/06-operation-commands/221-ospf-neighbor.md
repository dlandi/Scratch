---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.221. ospf-neighbor'
source_lines: 17444-17504
---

## 6.221. ospf-neighbor

#### Command Description

The command described in this section is used to show the `ospf-neighbor` attributes.

#### Command Syntax

```
show ospf-neighbor-<instance-id>/<ospf-area-id>/<ospf-if-name>/<router-id> [state] [role] [address] [priority]
```

#### Command Usage Details

**Table 529: ospf-neighbor Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |
| Pre-condition | The neighbor NEs have been configured with OSPF instance(s), OSPF areas(s), and OSPF interface(s). The NEs are properly connected with DCN interface bridges. i Note: An OSPF neighbor is deleted when the OSPF interface is down. |

#### Command Parameters

**Table 530: ospf-neighbor Command Parameters**

| Parameter | Description | Values | Used in |
| --- | --- | --- | --- |
| instance-id | OSPF instance ID. | uint8 (range: 0 .. 255) | show |
| ospf-area-id | OSPF Router Area ID. | dotted-quad | show |
| ospf-if-name | Reference of the interface in an OSPF area. | leafref (path "../../../../../interface/if-name") | show |
| router-id | OSPF neighbor router ID. | Example: 22.22.22.10 | show |
| state | OSPF neighbor states. | • down: Initial state, where no Hello packets are received from neighbor.<br>• init: Hello packets are received from the neighbor, but bidirectional communication is not yet established.<br>• 2-way: Bidirectional communication is established; DR/BDR election occurs in this state.<br>• exstart: DR and BDR have been elected and master-slave relation is determined.<br>• exchange: Neighbors exchange DataBaseDescriptor (DBD) packets containing LSA headers.<br>• loading: Routers exchange full Link State information based on DBDs.<br>• full: Full adjacency is achieved. | show |
| role | OSPF router role. | • drother: Designated Router Other.<br>• dr: Designated Router.<br>• bdr: Backup Designated Router.<br>• ptp: Point-to-point. | show |
| address | OSPF neighbor address. | IPv4/IPv6 address. | show |
| priority | OSPF router priority. On a multi-access network, this value is for the Designated Router (DR) election. The priority is ignored on other interface types. A router with a higher priority will be preferred in the election and a value of 0 indicates the router is not eligible to become a Designated Router or Backup Designated Router (BDR). | Integer, uint8 | show |

#### Examples

The following command shows how to show the OSPF neighbor information about all the OSPF neighbor routers:

```
show ospf-neighbor
```

The following output is displayed:

```
ospf-neighbor                                                      state  role  address                    priority  last-modified  owner-chassis  properties
-----------------------------------------------------------------  -----  ----  -------------------------  --------  -------------  -------------  --------------
ospf-neighbor-1/0.0.0.0/1-8-dwdm-line-1GE-OSCX1-MGMT/10.10.97.194  full   ptp   10.10.97.194               1         0              1              system-managed
ospf-neighbor-2/0.0.0.0/1-8-dwdm-line-1GE-OSCX1-MGMT/10.10.97.194  full   ptp   fe80::972b:8bc8:c880:4084  1         0              1              system-managed
```

The following command shows how to show the OSPF neighbor information about OSPF neighbor router *22.22.22.30* for OSPF interface *DCN* in OSPF area *5.5.5.5* for OSPF instance *2* :

```
show ospf-neighbor-2/5.5.5.5/DCN/22.22.22.30
```

<!-- page 852 -->
