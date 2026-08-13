---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.77. delete'
source_lines: 8752-8957
---

## 6.77. delete

#### Command Description

The `delete` command is used to delete an existing managed entity from the database. For the 'delete' command, a confirmation prompt will be displayed, unless the -f flag is provided. When the command is used to delete a managed entity instance, the deletion will remove the targeted managed entity and all the sub-level objects. The deletion of the managed entity instance is not allowed if it or its sub-level containers are currently used to carry services or other traffic affecting configurations. In this case, the deletion request will fail. Starting from R9.0, the `delete` with a `-b` flag introduces the best effort delete feature that only bypasses the check for object existence without returning a failure.

- Standard behavior: `delete <object>` fails if the object does not exist.
- New behavior with best effort : `delete –b <object>` does not fail if \<object\> does not exist.

**Note:**

  - The `-b` option does not raise an error if the specified object is missing. However, it may still produce other errors, such as precondition failures.
  - `delete -b` has the same functionality as a NETCONF 'remove' operation.

There are multiple entities which are system managed that cannot be deleted manually using this command. In general, entities that are manually created can also be manually deleted. Certain entities have delete pre-conditions. One or multiple filters may be provided to affect only matching entities, using parameter \<filter\>. It is possible to delete multiple instances of the same type by using wildcards (\*):

- as replacement of the whole instance id (card-\*)
- as replacement of part of the instance id (tom-1-4-\*; tom-1-\*-T3)
<!-- page 384 -->
- as replacement of all following (to the right) keys (odu-1-2-\* instead of odu-1-2-\*-\*)
- for auto-completing instance ids (odu-\*)

For more information about wildcards usage, refer to CLI Wildcard support (p. 84). It is also possible to use [] to represent ranges (card-1-[1..4]) or lists (for example: interface-[DCN,CRAFT]); delete tom-1-4-[T1,T8,T16]). If multiple instances are selected by the 'delete' command and a confirmation is needed, it is necessary to confirm each instance individually. Using the -v flag performs a command validation only (no deletion request is executed). If valid, the command replies with 'OK'. Otherwise the command will fail.

#### Command Syntax

```
delete -h
delete [-f|-v|-b] <entity-id> [<filter> ...]
```

#### Command Parameters

**Table 235: delete Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -v | Validates the command. |
| -f | Forces the command without confirmation. |
| -b | best effort delete, displays no error even if the object does not exist. |

**Table 236: delete Command Parameters**

| Parameter | Description | Value | Default |
| --- | --- | --- | --- |
| entity id | Instance ID of the entity to be created. | 1830 GX Management Entity AIDs (p. 43) | n/a |
| filter | Filter | (&lt;attribute&gt;=&lt;value&gt;) | n/a |

The following entities can be deleted:

<!-- page 385 -->

**Table 237: delete Command Entities**

| Entity | Description |
| --- | --- |
| aaa-server | The name of the aaa server. See aaa-server (p. 127) for more information. |
| access-rule | Single access-rule in a group of access rules, defining access to a particular target path. See access-rule (p. 134) for more information. |
| access-rule-list | Group of access-rules, organized by which user-groups the rules apply to. See access-rule-list (p. 139) for more information. |
| ace | Set of attributes for an access control entry (ACE). See ace (p. 141) for more information. |
| acl | Set of attributes associated with every access control list (ACL). An ACL can have one or more ACEs. See acl (p. 144) for more information. |
| adg | Set of Add/Drop Group attributes on OADM nodes. See adg (p. 173) for more information. |
| advanced-parameter | See advanced-parameter (p. 175) for more information. |
| auth-key | See auth-key (p. 226) for more information. |
| bgp-instance | See bgp-instance (p. 236) for more information. |
| bgp-neighbor | See bgp-neighbor (p. 238) for more information. |
| card | Card base object. This object has parameters that are common to all existing card types (controller, fan, tom etc). See card (p. 265) for more information. |
| cdp | CRL Distribution Point (CDP) for automatic download and periodic refresh of a specified CRL. See cdp (p. 279) for more information. |
| chassis | See chassis (p. 291) for more information. |
| comm-channel | See comm-channel (p. 320) for more information. |
| degree | See degree (p. 380) for more information. |
| dial-out-server | See dial-out-server (p. 393) for more information. |
| direction | See direction (p. 399) for more information. |
| dns-server | The address of the DNS server. See dns-server (p. 409) for more information. |
| dsc-group | See dsc-group (p. 431) for more information. |
| encryption-algorithm | See encryption-algorithm (p. 437) for more information. |
| external-fiber-connection | External fiber connection connecting two ports of L0 cards in different NEs. See external-fiber-connection (p. 477) for more information. |
| fiber-connection | Physical link representation of a connection between two distinct ports (or two distinct sub-ports) in the same NE. See fiber-connection (p. 489) for more information. |
| file-server | User configurable file-server (e.g SFTP server), to be used by transfer operations (upload/download). See file-server (p. 496) for more information. |
| flexo-group | See flexo-group (p. 505) for more information. |
| ike-sa-proposal | See ike-sa-proposal (p. 533) for more information. |
| ikev2-peer | See ikev2-peer (p. 538) for more information. |
| inci-neighbor | See inci-neighbor (p. 551) for more information. |
| interface | See interface (p. 554) for more information. |
| ip-monitoring | See ip-monitoring (p. 570) for more information. |
| ipsec-sa-proposal | See ipsec-sa-proposal (p. 572) for more information. |
| ipsec-sa-re-key | See ipsec-sa-re-key (p. 574) for more information. |
| ipsec-spd-entry | See ipsec-spd-entry (p. 576) for more information. |
| ipsec-traffic-selector | See ipsec-traffic-selector (p. 579) for more information. |
| ipv4-address | The IPv4 address on the interface. See ipv4-address (p. 581) for more information. |
| ipv4-static-route | A list of IPv4 static routes. See ipv4-static-route (p. 583) for more information. |
| ipv6-address | The IPv6 address on the interface. See ipv6-address (p. 586) for more information. |
| ipv6-static-route | A list of IPv6 static routes. See ipv6-static-route (p. 588) for more information. |
| local-ports | See local-ports (p. 627) for more information. |
| local-subnet | See local-subnet (p. 629) for more information. |
| log-console-facility-filter | Selector that allows to filter log messages based on their source facilities and severities. See log-console-facility-filter (p. 639) for more information. |
| log-file | Local syslog files supported by the system. See log-file (p. 642) for more information. |
| log-file-facility-filter | Selector that allows to filter log messages based on their source facilities and severities. See log-file-facility-filter (p. 646) for more information. |
| log-server | Grouping the configuration parameters for log forwarding. See log-server (p. 649) for more information. |
| log-server-facility-filter | Selector that allows to filter log messages based on their source facilities and severities. See log-server-facility-filter (p. 653) for more information. |
| oadm-capabilities | See oadm-capabilities (p. 736) for more information. |
| modules-adg | See modules-adg (p. 680) for more information. |
| modules-degree | See modules-degree (p. 682) for more information. |
| nmc | See nmc (p. 706) for more information. |
| ntp-key | Keys to be used for NTP authentication. See ntp-key (p. 725) for more information. |
| ntp-server | Configured NTP server. See ntp-server (p. 727) for more information. |
| nw-xconnect | See nw-xconnect (p. 732) for more information. |
| odu | odu4 (100G) or oduflex (400G) facility representing low order ODUs that XCONs are mapped into. See odu (p. 761) for more information. |
| ospf-area | See ospf-area (p. 839) for more information. |
| ospf-area-range | See ospf-area-range (p. 841) for more information. |
| ospf-instance | OSPF protocol instances. See ospf-instance (p. 844) for more information. |
| ospf-interface | See ospf-interface (p. 846) for more information. |
| ospfv3-ipsec-security-association | See ospfv3-ipsec-security-association (p. 852) for more information. |
| otdr | See otdr (p. 854) for more information. |
| otdr-ptp | See otdr-ptp (p. 860) for more information. |
| otu | See otu (p. 896) for more information. |
| oxcon | See oxcon (p. 912) for more information. |
| pm-threshold | See pm-threshold (p. 955) for more information. |
| protection | See protection (p. 976) for more information. |
| remote-ports | See remote-ports (p. 1009) for more information. |
| remote-subnet | See remote-subnet (p. 1011) for more information. |
| secure-entity | See secure-entity (p. 1042) for more information. |
| security-policies | See security-policies (p. 1048) for more information. |
| serdes | See serdes (p. 1062) for more information. |
| snmp-community | List of SNMP Community Strings. Note: trap-community-string is located in the snmp-target object. See snmp-community (p. 1138) for more information. |
| snmp-target | List of SNMP targets (trap listeners). See snmp-target (p. 1140) for more information. |
| snmpv3-user | See snmpv3-user (p. 1143) for more information. |
| ssh-authorized-key | SSHv2 authorized keys entry. Each authorized key entry contains a trusted remote public key for SSHv2 server side host authentication. See ssh-authorized-key (p. 1161) for more information. |
| ssh-known-host | SSHv2 known hosts entry. See ssh-known-host (p. 1168) for more information. |
| sw-control-rule | See sw-control-rule (p. 1241) for more information. |
| task | User configurable scheduled task. Can define single occurrence or periodic commands. See task (p. 1265) for more information. |
| template | See template (p. 1269) for more information. |
| template-group | See template-group (p. 1271) for more information. |
| tom | Transceiver Optical Module. See tom (p. 1283) for more information. |
| user | An authorized user. See user (p. 1336) for more information. |
| user-group | See user-group (p. 1341) for more information. |
| xcon | Layer 1 digital services that are currently provisioned in the system. This includes pre-provisioned XCONs too. See xcon (p. 1350) for more information. |

#### Examples

This example shows how to delete card-1-4 entity:

```
delete card-1-4
```

This example shows how to delete the user-tom entity (force delete without confirmation):

```
delete -f user-tom
```

This example shows how to test whether it is possible to delete the user-john entity :

```
delete -v user-john
```

This example shows how to delete all cards:

```
delete card-*
```

This example shows how to delete users that are in only the Monitoring Access user group:

```
delete user user-group=MA
```

This example shows how to delete users tom and john:

<!-- page 390 -->

```
delete user-[tom,john]
```

This example shows how to delete card-1-CHM6:

```
delete card-1-CHM6
```

This example shows how to delete the lower order ODUflexi:

```
delete LO-ODUflexi
```

This example shows how to delete super-channel:

```
delete super-channel-1-CHM6-L1
```

This example hows how to delete an ssh know host:

```
delete ssh-known-host-Server_243
Are you sure you want to delete [ ssh-known-host-Server_243 ]? [y/n] y
```

<!-- page 391 -->
