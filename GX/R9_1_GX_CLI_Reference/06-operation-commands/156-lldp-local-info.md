---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.156. lldp-local-info'
source_lines: 13699-13752
---

## 6.156. lldp-local-info

#### Command Description

This command is used to show the LLDP local system information sent on lldp-port.

#### Command Syntax

```
show lldp-local-info-<lldp-port> [chassis-id-subtype] [chassis-id] [port-id-subtype] [port-id] [port-description] [system-name]
[system-description] [supported-capabilities] [enabled-capabilities]
```

#### Command Usage Details

**Table 400: lldp-local-info Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 401: lldp-local-info Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| lldp-port | Local port that is connected to this LLDP neighbor. | port name |
| chassis-id-subtype | This attribute describes the format of the chassis-id string. chassis-component - Represents a chassis identifier based on the value of entPhysicalAlias object (defined in IETF RFC 2737) for a chassis component (i.e., an entPhysicalClass value of 'chassis(3)') | chassis-component |
| chassis-id | This attribute identifies the chassis component withing the LLDP remote system. This value needs to be interpreted according with the associated chassis-id-subtype, which identifies the format of this value. | String (length 0..255) Tip: 1830 GX NE uses the chassis serial number. |
| port-id-subtype | This attribute describes the format of the port-id string. local - Means that the port-id string identifies a locally assigned port ID | local |
| port-id | This attribute identifies the port within the LLDP remote system chassis. This value needs to be interpreted according with the associated port-id-subtype, which identifies the format of this value. | String (length 0..255) Tip: 1830 GX NE uses the port AID. |
| port-description | The string value used to identify the description of the given port associated with the remote system. | String (length 0..255) Tip: 1830 GX NE uses the port name. |
| system-name | The string value used to identify the system name of the remote system. | String (length 0..255) Tip: 1830 GX NE uses the NE name. |
| system-description | The string value used to identify the system description of the remote system. | String (length 0..255) Tip: 1830 GX NE uses the string 'Nokia Corporation.Converged OS, Version &lt;release id&gt; _ build &lt;software label&gt;'. _ |
| supported-capabilities | This attribute describes the remote system supported capabilities. | router bridge |
| enabled-capabilities | This attribute describes the remote system enabled capabilities. | router bridge |

#### Examples

The following example shows how to view the LLDP local system information sent on all LLDP ports:

```
show lldp-local-info
```

The following example shows how to view the LLDP local system information sent on AUX port, in 1830 GX G30 environment:

```
show lldp-local-info-comm-eth-1-11-ETH4
```

<!-- page 616 -->
