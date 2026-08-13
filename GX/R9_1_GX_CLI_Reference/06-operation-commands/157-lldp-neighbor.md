---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.157. lldp-neighbor'
source_lines: 13753-13818
---

## 6.157. lldp-neighbor

#### Command Description

This command is used to show the LLDP remote system discovered by lldp-port. This information is kept indefinitely, until the port is decommissioned, or the data is manually cleared by user.

**Tip:** The egress direction is not supported.

#### Command Syntax

```
show lldp-neighbor-<lldp-port>/<direction> [last-update] [age] [chassis-id-subtype] [chassis-id] [port-id-subtype] [port-id] [port-description]
[system-name] [system-description] [supported-capabilities] [enabled-capabilities] [ttl]
```

#### Command Usage Details

**Table 402: lldp-neighbor Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 403: lldp-neighbor Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| lldp-port | Local port that is connected to this LLDP neighbor. | port name | n/a | show |
| direction | Direction in which the neighbor was detected. | ingress | n/a | show |
| last-update | Timestamp with the last time this neighbor info was updated. | String | n/a | show |
| age | Number of seconds since discovery. | Number (seconds) | n/a | show |
| chassis-id-subtype | This attribute describes the format of the chassis-id string. reserved - Represents another subtype, not covered by the other options. When reserved subtype is used, the chassis-id is displayed as a hex string. chassis-component - Represents a chassis identifier based on the value of entPhysicalAlias object (defined in IETF RFC 2737) for a chassis component (i.e., an entPhysicalClass value of 'chassis(3)') interface-alias - Represents a chassis identifier based on the value of ifAlias object (defined in IETF RFC 2863) for an interface on the containing chassis. port-component - Represents a chassis identifier based on the value of entPhysicalAlias object (defined in IETF RFC 2737) for a port or backplane component (i.e., entPhysicalClass value of 'port(10)' or 'backplane(4)'), within the containing chassis. mac-address - Represents a chassis identifier based on the value of a unicast source address (encoded in network byte order and IEEE 802.3 canonical bit order), of a port on the containing chassis as defined in IEEE Std 802-2001. network-address - Represents a chassis identifier based on a network address, associated with a particular chassis. The encoded address is actually composed of two fields. The first field is a single octet, representing the IANA AddressFamilyNumbers value for the specific address type, and the second field is the network address value. interface-name - Represents a chassis identifier based on the value of ifName object (defined in IETF RFC 2863) for an interface on the containing chassis. local - Represents a chassis identifier based on a locally defined value. | reserved chassis-component interface-alias port-component mac-address network-address interface-name local | n/a | show |
| chassis-id | This attribute identifies the chassis component withing the LLDP remote system. This value needs to be interpreted according with the associated chassis-id-subtype, which identifies the format of this value. | String (length 0..255) | n/a | show |
| port-id-subtype | This attribute describes the format of the port-id string. interface-alias - Means that the port-id string identifies a particular instance of the ifAlias object (defined in IETF\n RFC 2863). If the particular ifAlias object does not contain any values, another port identifier type should be used. port-component - Means that the port-id string identifies a particular instance of the entPhysicalAlias object (defined in IETF RFC 2737) for a port or backplane component. mac-address - Means that the port-id string identifies a particular unicast source address (encoded in network byte order and IEEE 802.3 canonical bit order) associated with the port (IEEE Std 802-2001). network-address - Means that the port-id string identifies a network address associated with the port. The first octet contains\n the IANA AddressFamilyNumbers enumeration value for the specific address type, and octets 2 through N contain the\n networkAddress address value in network byte order. interface-name - Means that the port-id string identifies a particular instance of the ifName object (defined in IETF RFC 2863). If the particular ifName object does not contain any values, another port identifier type should be used. agent-circuit-id - Means that the port-id string identifies an agent-local identifier of the circuit (defined in RFC 3046). local - Means that the port-id string identifies a locally assigned port ID | interface-alias port-component mac-address network-address interface-name agent-circuit-id | n/a | show |
| port-id | This attribute identifies the port within the LLDP remote system chassis. This value needs to be interpreted according with the associated port-id-subtype, which identifies the format of this value. | String (length 0..255) | n/a | show |
| port-description | The string value used to identify the description of the given port associated with the remote system. | String (length 0..255) | n/a | show |
| system-name | The string value used to identify the system name of the remote system. | String (length 0..255) | n/a | show |
| system-description | The string value used to identify the system description of the remote system. | String (length 0..255) | n/a | show |
| supported-capabilities | This attribute describes the remote system supported capabilities. | bits | n/a | show |
| enabled-capabilities | This attribute describes the remote system enabled capabilities. | bits | n/a | show |
| ttl | Remote system info Time-To-Live (TTL). The number of seconds until information expires. If the remote system doesn't provide a ttl value, this parameter is set to the global hold-on-timer. | Number (sec) | n/a | show |

#### Examples

The following example shows the LLDP remote system discovered by the system's local LLDP ports:

```
show -r lldp-neighbor
```

The following example shows the LLDP remote system discovered by the CHM1R ethernet port 1-1-3:

```
show -r lldp-neighbor-ethernet-1-1-3/ingress
```

The following example shows the LLDP remote system discovered by the AUX port in 1830 GX G30 environment:

```
show -r lldp-neighbor-comm-eth-1-11-ETH4/ingress
```

<!-- page 620 -->
