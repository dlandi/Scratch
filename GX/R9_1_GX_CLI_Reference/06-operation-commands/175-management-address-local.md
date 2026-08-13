---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.175. management-address-local'
source_lines: 14754-14796
---

## 6.175. management-address-local

#### Command Description

This command is used to retrieve management address information about a particular chassis component. There may be multiple management addresses configured on the remote system identified by a particular index whose information is received on the local system. Each management address must have distinct 'management address type' (subtype) and 'management address' (address).

#### Command Syntax

```
show management-address-local-<lldp-port>/<address-subtype>/<address> [if-subtype] [if-id] [address-oid]
```

#### Command Usage Details

**Table 439: management-address-local Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 440: management-address-local Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| lldp-port | Local port that is connected to this LLDP neighbor. | instance |
| address-subtype | The type of management address identifier encoding used in the associated 'address' attribute. | string |
| address | The string value used to identify the management address component associated with the remote system. The purpose of this address is to contact the management entity. | string (length 0..64) |
| if-subtype | This attribute describes the basis of a particular type of interface associated with the management address. | • unknown - Interface is not known<br>• if-index - Interface identifier based on the if Index MIB object.<br>• system-port-number - Interface identifier based on the system port numbering convention. |
| if-id | The integer value used to identify the interface number regarding the management address component associated with the remote system. | integer |
| address-oid | The Object Identifier (OID) value used to identify the type of hardware component or protocol entity associated with the management address advertised by the remote system agent. | string (length 0..128) Tip: Currently this field is an empty string. |

#### Examples

This example shows how to view the management address information from the local LLDP ports:

```
show management-address-local
```

<!-- page 667 -->
