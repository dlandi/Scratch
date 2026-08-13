---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.299. snmp-target'
source_lines: 23123-23176
---

## 6.299. snmp-target

#### Command Description

These commands are used to add, set, show or delete a list of SNMP targets (trap listeners).

#### Command Syntax

```
add snmp-target-<target-name> snmpv3-user <value> target-address <value> [enabled <value>] [snmp-version <value>] [trap-community-string <value>]
[target-port <value>] [target-transport <value>]
set snmp-target-<target-name> [enabled <value>] [trap-community-string <value>] [snmpv3-user <value>] [target-address <value>] [target-port
<value>] [target-transport <value>]
show snmp-target-<target-name> [enabled] [snmp-version] [trap-community-string] [snmpv3-user] [target-address] [target-port] [target-transport]
delete snmp-target-<target-name>
```

#### Command Usage Details

**Table 698: snmp-target Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Related Commands | add snmpv3-user (p. 1143) |

#### Command Parameters

**Table 699: snmp-target Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| target-name | The target listener name. Identifies the SNMP target. | String (0...32) | n/a | add, show, set, delete |
| enabled | User configurable switch to enable or disable this snmp-target. | true, false | true | add, set, show |
| snmp-version | The SNMP version. | v2c, v3 | v2c | add, set, show |
| trap-community-string | The community string used for SNMP traps. | String (0...32) | infinera | add, set, show |
| snmpv3-user | Indicate the snmpv3 user. The parameter is applicable when the snmp-version is v3. | snmpv3 user | n/a | add, set, show |
| target-address | IP address or hostname of the SNMP target. The ipv4-address type represents an IPv4 address in dotted-quad notation. The IPv4 address may include a zone index, separated by a % sign. The zone index is used to disambiguate identical address values. For link-local addresses, the zone index will typically be the interface index number or the name of an interface. If the zone index is not present, the default zone of the device will be used. The canonical format for the zone index is the numerical format. The ipv6-address type represents an IPv6 address in full, mixed, shortened, and shortened-mixed notation. The IPv6 address may include a zone index, separated by a % sign. The zone index is used to disambiguate identical address values. For link-local addresses, the zone index will typically be the interface index number or the name of an interface. If the zone index is not present, the default zone of the device will be used. The canonical format of IPv6 addresses uses the textual representation defined in Section 4 of RFC 5952. The canonical format for the zone index is the numerical format as described in Section 11.2 of RFC 4007. | String (IPv4/ IPv6 address) | n/a | add, set, show |
| target-port | UDP port number. | Number (range 0..65535) | 162 | add, set, show |
| target-transport | Type of transport for the SNMP target. | udp | udp | add, set, show |

<!-- page 1142 -->

#### Examples

This example shows how to add an SNMP target:

```
add snmp-target-mytarget snmp-version v2c target-ip 10.220.225.10 target-port 162 target-transport udp trap-community-string public enabled
 true
```

<!-- page 1143 -->
