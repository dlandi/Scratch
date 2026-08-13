---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.291. show'
source_lines: 21878-22755
---

## 6.291. show

#### Command Description

The `show` retrieves information from the system. This command allows the user to visualize system information. The `show` command has multiple modes, each supporting different parameters and options:

- show managed entities - displays the values of the selected configuration and state attributes.
- show alarm - displays currently raised alarms. Refer to alarm (p. 178) for the command description, syntax and parameters.
- show config - displays system configuration. Refer to config (p. 334) for the command description, syntax and parameters.
- show log - display log files. Refer to log (p. 633) for the command description, syntax and parameters.
- show pm - displays the PM statistics data measured by the system. Refer to pm (p. 934) for the command description, syntax and parameters.

The `show` managed entities mode, displays a list of child entities and attribute-value pairs, according to the input parameters. If no entity or attributes are provided, the complete content of the current entity is displayed. It is possible to provide either an individual \<entity-id\>, or a generic \<entity-type\> to visualize its contents (for one specific entity, or for all of that type, respectively). Additionally, one or more filters may be used to display only matching entities. Starting R9.0, the show command output is sorted numerically. It is also possible to reference multiple instances or attributes using a wildcard (\*):

- as replacement of the whole instance id (card-\*)
- as replacement of part of the instance id (port-1-4-\*)
- for auto-completing instance ids (user-admin\* for all user-names starting with 'admin')
- for auto-completing attribute names (admin\* instead of admin-state).

For more information about wildcard usage, refer to CLI Wildcard support (p. 84). It is also possible to use [] to represent ranges (card-1-[1..4]) or lists (interface-[DCN,CRAFT]). Displaying a single managed entity is done in list format, but multiple entities are displayed in tabular format.

**Note:** If a user executes a `show` command which generates more than 50000 objects, an error message is displayed to indicate that the output is too large.

<!-- page 1093 -->

#### Command Syntax

```
show -h
show [<command flag> ...] [(<entity-id>|<entity-type>)] [(<attribute id>|<filter>) ...]
show -r
```

#### Command Usage Details

**Table 678: show Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 679: show Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -l | Long display; shows additional information. |
| -r=&lt;n&gt; | Operate the command recursively for n levels down; if n is not provided full recursion is used. |
| -c | Displays the container instances only (no attributes). |
| -a | Displays the attributes only (no container instances). |
| -t | Displays information in tabular format. |
| -x | Displays configuration/read-write attributes only. |
| -o | Displays state/read-only attributes only. |
| -d | Displays default values as additional information; (*) is shown if current value is default (-d requires list view; use '\|display list' if necessary). |

**Table 680: show Command Parameters**

| Parameter | Description |
| --- | --- |
| entity-id | Instance ID of the entity where to perform the show. |
| attribute | Name of the attribute to be provided. |
| value | Value of the attribute to be initialized. |
| filter | Filter (&lt;attribute&gt;=&lt;value&gt;). |

The attributes of the following entities can be retrieved:

**Table 681: show Command Entity Types**

| Entity Type | Description |
| --- | --- |
| aaa-server | The name of the aaa server. See aaa-server (p. 127) for more information. |
| access-control-list | Attributes and objects pertaining to ACLs. These values can be retrieved by using show access-control-list. See access-control-list (p. 133) for more information. |
| access-rule | Single access-rule in a group of access rules, defining access to a particular target path. See access-rule (p. 134) for more information. |
| access-rule-list | Group of access-rules, organized by which user-groups the rules apply to. See access-rule-list (p. 139) for more information. |
| ace | Set of attributes for an access control entry (ACE). See ace (p. 141) for more information. |
| acl | Set of attributes associated with every access control list (ACL). An ACL can have one or more ACEs. See acl (p. 144) for more information. |
| adg | Set of Add/Drop Group attributes on OADM nodes. See adg (p. 173) for more information. |
| advanced-parameter | See advanced-parameter (p. 175) for more information. |
| alarm | Currently raised alarms. See alarm (p. 178) for more information. |
| alarm-control | Attribute associated with alarm management control (ARC). See alarm-control (p. 182) for more information. |
| alarm-inventory | See alarm-inventory (p. 184) for more information. |
| alarm-severity-entry | Individual entry in alarm-severity-profile. See alarm-severity-entry (p. 186) for more information. |
| alarm-severity-profile | See alarm-severity-entry (p. 186) for more information. |
| alarms | Top level container for all system alarms, which are defined as an undesirable state in a resource that requires corrective action. See alarm (p. 178) for more information. |
| amplifier | Managed Object for optical amplifier (EDFA amplifier). See amplifier (p. 191) for more information. |
| amplifier-raman | See amplifier-raman (p. 205) for more information. |
| auth-key | See auth-key (p. 226) for more information. |
| authorization | See authorization (p. 228) for more information. |
| bgp-instance | See bgp-instance (p. 236) for more information. |
| bgp-neighbor | See bgp-neighbor (p. 238) for more information. |
| capabilities | See capabilities (p. 264) for more information. |
| card | Card base object. This object has parameters that are common to all existing card types (controller, fan, tom etc). See card (p. 265) for more information. |
| carrier-neighbor | See carrier-neighbor (p. 277) for more information. |
| cdp | CRL Distribution Point (CDP) for automatic download and periodic refresh of a specified CRL. See cdp (p. 279) for more information. |
| certificate | All managed local/trusted/peer X509v3 certificates on the system. See certificate (p. 287) for more information. |
| chassis | Chassis base object. See chassis (p. 291) for more information. |
| cli | Set of attributes of the Command Line Interface (CLI) management protocol. See cli (p. 310) for more information. |
| cli-session-config | Set of attributes of the Command Line Interface (CLI) session. See cli-session-config (p. 313) for more information. |
| clock | Set of attributes of the system's clock. See clock (p. 315) for more information. |
| comm-channel | See comm-channel (p. 320) for more information. |
| comm-eth | Set of attributes of the communication ethernet port. See comm-eth (p. 327) for more information. |
| config | System's configuration. See config (p. 334) for more information. |
| connection-ports | Connection ports on a given degree. See connection-ports (p. 339) for more information. |
| console | Parameters associated with this card's serial console port. See console (p. 341) for more information. |
| controller-card | See controller-card (p. 344) for more information. |
| crl | Installed Certificate Revocation Lists (CRLs). See crl (p. 349) for more information. |
| current-advanced-parameter | See current-advanced-parameter (p. 359) for more information. |
| current-alarms | List of currently raised alarms. See current-alarms (p. 361) for more information. |
| current-fw | List of current firmware available in the cards. See current-fw (p. 362) for more information. |
| current-subscription | See current-subscription (p. 364) for more information. |
| custom-tlv | See custom-tlv (p. 366) for more information. |
| data-model | Available YANG Data models for loading/unloading. See data-model (p. 367) for more information. |
| data-path-encryption | Top-level container for all data path encryption services and entities. To view all data path encryption use the command show data-path-encryption. See data-path-encryption (p. 368) for more information. |
| database | The list of the databases in the system. See database (p. 369) for more information. |
| degree | See degree (p. 380) for more information. |
| dhcp-relay | See dhcp-relay (p. 391) for more information. |
| dial-out-server | See dial-out-server (p. 393) for more information. |
| direction | See direction (p. 399) for more information. |
| dns | See dns (p. 407) for more information. |
| dns-server | The address of the DNS server. See dns-server (p. 409) for more information. |
| downloaded-image | Downloaded software image files. See downloaded-image (p. 425) for more information. |
| downloads | Downloaded manifest files and associated image files. The list can be retrieved by using show downloads. See downloads (p. 426) for more information. |
| dsc | See dsc (p. 427) for more information. |
| dsc-group | See dsc-group (p. 431) for more information. |
| encryption-algorithm | See encryption-algorithm (p. 437) for more information. |
| equipment | Container for all equipment related resources. The list can be retrieved by using show equipment. See equipment (p. 439) for more information. |
| equipment-capabilities | Top level container for all equipment capabilities. The list can be retrieved by using show equipment-capabilities. |
| equipment-policies | Container with all existing equipment policies. See equipment-policies (p. 441) for more information. |
| eth-zr | The Ethernet ZR facility. See eth-zr (p. 454) for more information. |
| ethernet | The Ethernet facility. See ethernet (p. 461) for more information. |
| external-fiber-connection | External fiber connection connecting two ports of L0 cards in different NEs. See external-fiber-connection (p. 477) for more information. |
| facilities | The top-level facility root node under which all other facilities are present. The list can be retrieved by using show facilities. |
| fc | See fc (p. 482) for more information. |
| fiber-connection | Physical link representation of a connection between two distinct ports (or two distinct sub-ports) in the same NE. See fiber-connection (p. 489) for more information. |
| file-server | User configurable file-server (e.g SFTP server), to be used by transfer operations (upload/download). See file-server (p. 496) for more information. |
| flexo | See flexo (p. 501) for more information. |
| flexo-group | See flexo-group (p. 505) for more information. |
| fru-info | See fru-info (p. 509) for more information. |
| gadt | See gadt (p. 510) for more information. |
| gcmt | See gapt (p. 512) for more information. |
| golden-carrier-mode | See golden-carrier-mode (p. 521) for more information. |
| grpc | Set of attributes of the gNMI/gRPC management protocol. See grpc (p. 523) for more information. |
| http-file-server | See http-file-server (p. 528) for more information. |
| icdp | See icdp (p. 530) for more information. |
| ike-sa-proposal | See ike-sa-proposal (p. 533) for more information. |
| ikev2 | See ikev2 (p. 535) for more information. |
| ikev2-local-instance | See ikev2-local-instance (p. 536) for more information. |
| ikev2-peer | See ikev2-peer (p. 538) for more information. |
| image-keys | Container for image keys. The list can be retrieved by using show image-keys. |
| inci | See inci (p. 549) for more information. |
| inci-neighbor | See inci-neighbor (p. 551) for more information. |
| interface | See interface (p. 554) for more information. |
| inventory | Inventory data for a present FRU. See inventory (p. 567) for more information. |
| ip-monitoring | See ip-monitoring (p. 570) for more information. |
| ipsec-sa-proposal | See ipsec-sa-proposal (p. 572) for more information. |
| ipsec-sa-re-key | See ipsec-sa-re-key (p. 574) for more information. |
| ipsec-spd-entry | See ipsec-spd-entry (p. 576) for more information. |
| ipsec-traffic-selector | See ipsec-traffic-selector (p. 579) for more information. |
| ipv4-address | The IPv4 address on the interface. See ipv4-address (p. 581) for more information. |
| ipv4-static-route | A list of IPv4 static routes. See ipv4-static-route (p. 583) for more information. |
| ipv6-address | The IPv6 address on the interface. See ipv6-address (p. 586) for more information. |
| ipv6-static-route | A list of IPv6 static routes. See ipv6-static-route (p. 588) for more information. |
| ISK | Image Signing Key (ISK) from the system. See ISK (p. 591) for more information. |
| key-replacement-package | See key-replacement-package (p. 594) for more information. |
| KRK | Image Root Key (KRK) list. See KRK (p. 597) for more information. |
| l0-capabilities | See l0-capabilities (p. 599) for more information. |
| led | Representation of a LED in an FRU. See led (p. 600) for more information. |
| leds | To view the list of the system leds use the command show leds. See led (p. 600) for more information. |
| line-ptp | See line-ptp (p. 604) for more information. |
| links | See links (p. 611) for more information. |
| lldp | Global LLDP configuration attribute. See lldp (p. 612) for more information. |
| lldp-local-info | See lldp-local-info (p. 613) for more information. |
| lldp-neighbor | LLDP remote system discovered by lldp-port. See lldp-neighbor (p. 616) for more information. |
| lldp-port-statistics | LLDP frame reception statistics for a particular port and direction. See lldp-port-statistics (p. 620) for more information. |
| local-certificate | X509v3 end-entity certificate that represents one of various secure application identities. See local-certificate (p. 622) for more information. |
| local-ports | See local-ports (p. 627) for more information. |
| local-subnet | See local-subnet (p. 629) for more information. |
| log | Log files. See log (p. 633) for more information. |
| log-console | Set of attributes of the console logging supported by the system. See log-console (p. 637) for more information. |
| log-console-facility-filter | Selector that allows to filter log messages based on their source facilities and severities. See log-console-facility-filter (p. 639) for more information. |
| log-file | Local syslog files supported by the system. See log-file (p. 642) for more information. |
| log-file-facility-filter | Selector that allows to filter log messages based on their source facilities and severities. See log-file-facility-filter (p. 646) for more information. |
| log-server | Grouping the configuration parameters for log forwarding. See log-server (p. 649) for more information. |
| log-server-facility-filter | Selector that allows to filter log messages based on their source facilities and severities. See log-server-facility-filter (p. 653) for more information. |
| management-address | See management-address (p. 663) for more information. |
| management-address-local | See management-address-local (p. 665) for more information. |
| manifest | Downloaded manifest file and its information. See manifest (p. 667) for more information. |
| mc | See mc (p. 671) for more information. |
| modules-adg | See modules-adg (p. 680) for more information. |
| modules-degree | See modules-degree (p. 682) for more information. |
| monitored-channel | See monitored-channel (p. 684) for more information. |
| nct-connection | See nct-connection (p. 687) for more information. |
| ne | See ne (p. 690) for more information. |
| ne-function | See ne-function (p. 697) for more information. |
| netconf | Set of attributes of the configuration of the NETCONF management protocol. See netconf (p. 699) for more information. |
| network-xconnect | See network-xconnect (p. 701) for more information. |
| networking | Top level container for networking model. The list can be retrieved by using show networking. See networking (p. 702) for more information. |
| networking-services | See networking-services (p. 703) for more information. |
| next-hop | Next-hop of a route item. See next-hop (p. 704) for more information. |
| nmc | See nmc (p. 706) for more information. |
| ntp | See ntp (p. 723) for more information. |
| ntp-key | Keys to be used for NTP authentication. See ntp-key (p. 725) for more information. |
| ntp-server | Configured NTP server. See ntp-server (p. 727) for more information. |
| ntp-server-status | NTP server status. See ntp-server-status (p. 730) for more information. |
| nw-xconnect | See nw-xconnect (p. 732) for more information. |
| oadm-capabilities | See oadm-capabilities (p. 736) for more information. |
| oc | See oc (p. 738) for more information. |
| ochm | See ochm (p. 743) for more information. |
| ocm-channel | See ocm-ptp (p. 754) for more information. |
| ocsp-server | See ocsp-server (p. 758) for more information. |
| odu | odu4 (100G) or oduflex (400G) facility representing low order ODUs that XCONs are mapped into. See odu (p. 761) for more information. |
| odu-diagnostics | See odu-diagnostics (p. 771) for more information. |
| oms | See oms (p. 777) for more information. |
| ops | Optical Physical Section (OPS) facility. See ops (p. 790) for more information. |
| optical-carrier | See optical-carrier (p. 796) for more information. |
| optical-channel | See optical-channel (p. 807) for more information. |
| optical-ptp | See optical-ptp (p. 810) for more information. |
| osc | See osc (p. 829) for more information. |
| ospf-area | See ospf-area (p. 839) for more information. |
| ospf-area-range | See ospf-area-range (p. 841) for more information. |
| ospf-instance | OSPF protocol instances. See ospf-instance (p. 844) for more information. |
| ospf-interface | See ospf-interface (p. 846) for more information. |
| ospfv3-ipsec-security-association | See ospfv3-ipsec-security-association (p. 852) for more information. |
| otdr | See otdr (p. 854) for more information. |
| otdr-ptp | See otdr-ptp (p. 860) for more information. |
| ots | See ots (p. 868) for more information. |
| ots-diagnostics | See ots-diagnostics (p. 883) for more information. |
| ots-r | See ots-r (p. 887) for more information. |
| otu | See otu (p. 896) for more information. |
| otu-diagnostics | See otu-diagnostics (p. 904) for more information. |
| oxcon | See oxcon (p. 912) for more information. |
| packaged-fw | Firmware version included in this software-load. See packaged-fw (p. 922) for more information. |
| peer-certificate | X509v3 end-entity certificate that represents a trusted 'remote peer' certificate for L1 encryption secure application. See peer-certificate (p. 927) for more information. |
| pm | PM statistics data measured by the system. See pm (p. 934) for more information. |
| pm-catalog | PM catalog which contains information on all PM parameters, such as units and type. The list can be retrieved by using show pm-catalog. See pm-catalog (p. 942) for more information. |
| pm-control | Configuration for currently existing resources in the system that support PM data. The list can be retrieved by using show pm-control. See pm-control (p. 943) for more information. |
| pm-control-entry | PM configuration for one particular resource, for one particular period, direction and location. See pm-control-entry (p. 944) for more information. |
| pm-parameter | Catalog information for a single PM parameter. See pm-parameter (p. 946) for more information. |
| pm-profile | PM profile which contains information on all resources that support PM data, together with its related default configuration. Changing this configuration has an impact on newly created objects. The list can be retrieved by using show pm-profile. See pm-profile (p. 949) for more information. |
| pm-profile-entry | PM configuration per resource type. See pm-profile-entry (p. 951) for more information. |
| pm-resource | PM configuration per resource instance. See pm-resource (p. 953) for more information. |
| pm-threshold | See pm-threshold (p. 955) for more information. |
| pm-threshold-profile | PM configuration per parameter, for this resource type. See pm-threshold-profile (p. 957) for more information. |
| port | Set of attributes of a card port. See pm-threshold-profile (p. 957) for more information. |
| property | See property (p. 974) for more information. |
| protection | See protection (p. 976) for more information. |
| protection-group | See protection-group (p. 977) for more information. |
| protection-unit | See protection-unit (p. 985) for more information. |
| protocols | Container of management protocol objects. The list can be retrieved by using show protocols. See protocols (p. 987) for more information. |
| pump | See pump (p. 992) for more information. |
| pump-power | See pump-power (p. 994) for more information. |
| recovery | See recovery (p. 1005) for more information. |
| remote-ports | See remote-ports (p. 1009) for more information. |
| remote-subnet | See remote-subnet (p. 1011) for more information. |
| resources | See resources (p. 1013) for more information. |
| restconf | Set of attributes of the configuration of the RESTCONF management protocol. See restconf (p. 1020) for more information. |
| rib | List of RIB entries. See rib (p. 1022) for more information. |
| route | List of system routes from various sources, such as dynamic protocols and static route. See route (p. 1024) for more information. |
| routing | Container of routing subsystem. The list can be retrieved by using show routing. See routing (p. 1026) for more information. |
| rsc | See rsc (p. 1027) for more information. |
| scheduled-tasks | Container of individual user-configurable scheduled commands. The list can be retrieved by using show scheduled-tasks. See scheduled-task (p. 1036) for more information. |
| secure-application | List of the secured applications and parameters. See secure-application (p. 1039) for more information. |
| secure-applications | List of the secured applications which uses X509v3 certificate as its digital identity. See secure-application (p. 1039) for more information. |
| secure-entity | See secure-entity (p. 1042) for more information. |
| secure-entity-sa-proposal | See secure-entity-sa-proposal (p. 1045) for more information. |
| security | Top level security container. See security (p. 1047) for more information. |
| security-policies | Container with the several flags that represent the security policies of the system. See security-policies (p. 1048) for more information. |
| security-policy-database | See security-policy-database (p. 1060) for more information. |
| serdes | See serdes (p. 1062) for more information. |
| serdes-template | See serdes-template (p. 1064) for more information. |
| serdes-template-entry | See serdes-template-entry (p. 1066) for more information. |
| serial-console | Global configuration of all serial console ports in the system. See serial-console (p. 1068) for more information. |
| services | Services of multiple types commissioned in this NE. The list can be retrieved by using show services. |
| session | List of currently established management layer sessions. See session (p. 1069) for more information. |
| slot | Slot equipment holder details. See slot (p. 1132) for more information. |
| snmp | See snmp (p. 1136) for more information. |
| snmp-community | List of SNMP Community Strings. Note: trap-community-string is located in the snmp-target object. See snmp-community (p. 1138) for more information. |
| snmp-target | List of SNMP targets (trap listeners). See snmp-target (p. 1140) for more information. |
| snmpv3-user | See snmpv3-user (p. 1143) for more information. |
| software-load | Information on the Software Load present in the system. See software-load (p. 1145) for more information. |
| software-location | Software load information associated to each of the equipment. See software-location (p. 1148) for more information. |
| spectrum | See spectrum (p. 1149) for more information. |
| spectrum-control | See spectrum-control (p. 1153) for more information. |
| spectrum-monitoring | See spectrum-monitoring (p. 1156) for more information. |
| ssh | Control attributes of ssh access. See ssh (p. 1159) for more information. |
| ssh-authorized-key | SSHv2 authorized keys entry. Each authorized key entry contains a trusted remote public key for SSHv2 server side host authentication. See ssh-authorized-key (p. 1161) for more information. |
| ssh-host-key | Global (for server and client side SSHv2 based apps) SSHv2 host keys. See ssh-host-key (p. 1163) for more information. |
| ssh-known-host | SSHv2 known hosts entry. See ssh-known-host (p. 1168) for more information. |
| stm | See stm (p. 1185) for more information. |
| subscription-paths | See subscription-path (p. 1196) for more information. |
| subscriptions | This container holds information for telemetry subscriptions. The list can be retrieved by using show subscriptions. See subscriptions (p. 1199) for more information. |
| subtype-constraint | See subtype-constraint (p. 1200) for more information. |
| super-channel | Unified channel of optical carriers. Can have many optical channels. See super-channel (p. 1202) for more information. |
| super-channel-group | See super-channel-group (p. 1206) for more information. |
| supported-card | Capability information for supported card. See supported-card (p. 1210) for more information. |
| supported-carrier-mode | See supported-carrier-mode (p. 1215) for more information. |
| supported-chassis | Capability information for supported chassis. See supported-chassis (p. 1217) for more information. |
| supported-gain-range | See supported-gain-range (p. 1220) for more information. |
| supported-port | Capabilities for each port in each supported card. See supported-port (p. 1221) for more information. |
| supported-power-profile | See supported-power-profile (p. 1225) for more information. |
| supported-slot | Capability for each slot within each supported chassis. See supported-slot (p. 1226) for more information. |
| supported-tom | Capability information for supported TOM (Tunable/non-tunable Optical Module) in the scope of a particular card. See supported-tom (p. 1231) for more information. |
| supporting-interface | See supporting-interface (p. 1236) for more information. |
| sw-component | Software load component details. See sw-component (p. 1237) for more information. |
| sw-container | List of OS-level containers. See sw-container (p. 1239) for more information. |
| sw-control-rule | See sw-control-rule (p. 1241) for more information. |
| sw-management | Software load details. The list can be retrieved by using show sw-management. See sw-management (p. 1243) for more information. |
| sw-service | Software service running in the system. See sw-service (p. 1246) for more information. |
| sw-subcomponent | Software load subcomponent details. See sw-subcomponent (p. 1248) for more information. |
| syslog | Set of attributes configuration for logging functionality via syslog. Includes control of local log files, remote logging configuration and logging in serial console. See syslog (p. 1252) for more information. |
| system | System Configuration container. The list can be retrieved by using show system. See system (p. 1256) for more information. |
| system-capabilities | Top level container for all capability information. This data is read-only, and expected to be informative to the user regarding what are the system capabilities. This information is static and independent on current configuration. Capabilities can be updated only: - with SW upgrade - with a dedicated capabilities file update (for specific cases only). The list can be retrieved by using show system-capabilities. |
| task | User configurable scheduled task. Can define single occurrence or periodic commands. See task (p. 1265) for more information. |
| telemetry | Top level configuration and state for the device telemetry system. The list can be retrieved by using show telemetry. See telemetry (p. 1268) for more information. |
| template | See template (p. 1269) for more information. |
| template-group | See template-group (p. 1271) for more information. |
| templates | See templates (p. 1273) for more information. |
| third-party-app | See third-party-app (p. 1278) for more information. |
| third-party-fw | See third-party-fw (p. 1280) for more information. |
| tom | Tunable/non-tunable Optical Module. See tom (p. 1283) for more information. |
| tom-type | Capability information for supported TOM (Tunable/non-tunable Optical Module). See tom-type (p. 1290) for more information. |
| topology | Topology information related with this NE. The information can be retrieved by using show topology. See topology (p. 1292) for more information. |
| transfer | Information associated with file transfer. The information can be retrieved by using show transfer. See transfer (p. 1297) for more information. |
| transfer-status | File transfer status. The information can be retrieved by using show transfer-status. See transfer-status (p. 1299) for more information. |
| trib-ptp | Optional service-specific custom rules to override default action upon service failure. See trib-ptp (p. 1303) for more information. |
| trusted-certificate | X509v3 CA (Root and Intermediate) certificate that the system trusts. See trusted-certificate (p. 1310) for more information. |
| unprovisioned-inventory | See unprovisioned-inventory (p. 1315) for more information. |
| upgrade-status | See upgrade-status (p. 1319) for more information. |
| usb | Represents the USB function of this port. See usb (p. 1333) for more information. |
| user | An authorized user. See user (p. 1336) for more information. |
| user-group | List of user groups, each one with its own access permissions. Each user will be associated with a list of groups, and will derive its permissions from them. See user-group (p. 1341) for more information. |
| vrf | See vrf (p. 1348) for more information. |
| xcon | Layer 1 digital services that are currently provisioned in the system. This includes pre-provisioned XCONs too. See xcon (p. 1350) for more information. |
| ztp | Zero Touch Provisioning (ZTP) status. See ztp (p. 1356) for more information. |

#### Examples

This example shows how to view the ntp server details:

```
show ntp
```

The following output is displayed:

```
  ntp
  ntp-key-10
  ntp-enabled             true
  ntp-auth-enabled        false
  ntp-active-server       ' '
```

This example shows how to view the ntp-key details:

```
show ntp-key
```

The following output is displayed:

```
ntp-key key-type is-trusted
---------- -------- ----------
ntp-key-10 md5 false
```

This example shows how to view all the cards:

```
show card
```

The following output is displayed:

```
card              required-type  required-PON    category      chassis-name  slot-name  max-power-draw (W)  last-reboot-time
----------------  -------------  --------------  ------------  ------------  ---------  ------------------  ----------------------
card-1-1          XMM4           'XMM4'          controller    1             1          58.80
card-1-IOPANEL-2  IOPANEL        'GX-IOP'        other         1             2          26.30               2020-07-23T05:09:43GMT
card-1-4          CHM6           auto            line-card     1             4          473.10              2020-07-26T16:27:44GMT
card-1-FAN-1      FAN            'GX-FANMODULE'  fan           1             FAN-1      66.70               2020-07-23T05:09:41GMT
card-1-FAN-2      FAN            'GX-FANMODULE'  fan           1             FAN-2      66.70               2020-07-23T05:09:41GMT
card-1-FAN-3      FAN            'GX-FANMODULE'  fan           1             FAN-3      66.70               2020-07-23T05:09:42GMT
card-1-FAN-4      FAN            'GX-FANMODULE'  fan           1             FAN-4      66.70               2020-07-23T05:09:42GMT
card-1-FAN-5      FAN            'GX-FANMODULE'  fan           1             FAN-5      66.70               2020-07-23T05:09:42GMT
card-1-FAN-6      XMM4-FAN       'GX-FAN-XMM4'   fan           1             FAN-6      28.40               2020-07-23T05:09:42GMT
card-1-FAN-7      XMM4-FAN       'GX-FAN-XMM4'   fan           1             FAN-7      28.40               2020-07-23T05:09:42GMT
card-1-FANCTRL-1  FAN-CTRL       'GX-FAN-CTRL'   other         1             FANCTRL-1  1.20                2020-07-23T05:09:43GMT
card-1-PEM-1      PEM            'GX-PEM-AC'     power-supply  1             PEM-1      ---                 2020-07-23T05:09:42GMT
card-1-PEM-2      PEM            'GX-PEM-AC'     power-supply  1             PEM-2      ---                 2020-07-23T05:09:42GMT
card-1-PEM-3      PEM            'GX-PEM-AC'     power-supply  1             PEM-3      ---                 2020-07-23T05:09:42GMT
card-1-PEM-4      PEM            'GX-PEM-AC'     power-supply  1             PEM-4      ---                 2020-07-23T05:09:43GMT
card              last-reboot-reason  alias-name  AID          admin-status  oper-status  avail-status    alarm-report-control  label
----------------  ------------------  ----------  -----------  ------------  -----------  --------------  --------------------  -----
card-1-1                                          1-1          unlock        up           in-service      allowed
card-1-IOPANEL-2  simulated reboot                1-2          ---           up           in-service      allowed
card-1-4          simulated reboot                1-4          unlock        down         out-of-service  allowed
card-1-FAN-1      simulated reboot                1-FAN-1      ---           up           in-service      allowed
card-1-FAN-2      simulated reboot                1-FAN-2      ---           up           in-service      allowed
card-1-FAN-3      simulated reboot                1-FAN-3      ---           up           in-service      allowed
card-1-FAN-4      simulated reboot                1-FAN-4      ---           up           in-service      allowed
card-1-FAN-5      simulated reboot                1-FAN-5      ---           up           in-service      allowed
card-1-FAN-6      simulated reboot                1-FAN-6      ---           up           in-service      allowed
card-1-FAN-7      simulated reboot                1-FAN-7      ---           up           in-service      allowed
card-1-FANCTRL-1  simulated reboot                1-FANCTRL-1  ---           up           in-service      allowed
card-1-PEM-1      simulated reboot                1-PEM-1      unlock        up           in-service      allowed
card-1-PEM-2      simulated reboot                1-PEM-2      unlock        up           in-service      allowed
card-1-PEM-3      simulated reboot                1-PEM-3      unlock        up           in-service      allowed
card-1-PEM-4      simulated reboot                1-PEM-4      unlock        up           in-service      allowed
```

This example shows how to view chassis required type:

<!-- page 1111 -->

```
show chassis-1 required-*
```

The following output is displayed:

```
chassis-1
  required-type          G42
```

This example shows how to view inventory-1:

```
show inventory-1
```

The following output is displayed:

```
inventory-1
  hardware-version         'shelf 1 hw 0.0'
  actual-type              'G42'
  PON                      'G42'
  serial-number            'ABCDEFGH4'
  clei                     'CLEI:00000'
  vendor                   'Infinera'
  part-number              'PN:00000'
  manufacture-date         '2020-12-12T00:00:00Z'
  insertion-date           '2021-02-04T13:28:09Z'
  fw-status                not-applicable
```

This example shows how to view the possible-tom-types to configure TOM in 1830 GX:

```
show port
```

The following output is displayed:

```
 port                 alias-name  AID        admin-status  oper-status  avail-status       alarm-report-control  label  port-type
-------------------  ----------  ---------  ------------  -----------  -----------------  --------------------  -----  ---------
port-1-1-AUX-1                   1-1-AUX-1  unlock        up           normal in-service  allowed                      comm
port-1-1-AUX-2                   1-1-AUX-2  unlock        up           normal in-service  allowed                      comm
port-1-1-CRAFT                   1-1-CRAFT  unlock        up           normal in-service  allowed                      comm
port-1-1-DCN                     1-1-DCN    unlock        up           normal in-service  allowed                      comm
port-1-1-U1                      1-1-U1     unlock        up           normal in-service  allowed                      usb
port-1-4-L1                      1-4-L1     unlock        up           normal in-service  allowed                      line
port-1-4-L2                      1-4-L2     unlock        up           normal in-service  allowed                      line
port-1-4-T1                      1-4-T1     unlock        up           normal in-service  allowed                      tributary
port-1-4-T10                     1-4-T10    unlock        up           normal in-service  allowed                      tributary
port-1-4-T11                     1-4-T11    unlock        up           normal in-service  allowed                      tributary
port-1-4-T12                     1-4-T12    unlock        up           normal in-service  allowed                      tributary
port-1-4-T13                     1-4-T13    unlock        up           normal in-service  allowed                      tributary
port-1-4-T14                     1-4-T14    unlock        up           normal in-service  allowed                      tributary
port-1-4-T15                     1-4-T15    unlock        up           normal in-service  allowed                      tributary
port-1-4-T16                     1-4-T16    unlock        up           normal in-service  allowed                      tributary
port-1-4-T2          porttwo     1-4-T2     unlock        up           normal in-service  allowed                      tributary
port-1-4-T3          tom1        1-4-T3     unlock        up           normal in-service  allowed                      tributary
port-1-4-T4                      1-4-T4     unlock        up           normal in-service  allowed                      tributary
port-1-4-T5                      1-4-T5     unlock        up           normal in-service  allowed                      tributary
port-1-4-T6                      1-4-T6     unlock        up           normal in-service  allowed                      tributary
port-1-4-T7                      1-4-T7     unlock        up           normal in-service  allowed                      tributary
port-1-4-T8                      1-4-T8     unlock        up           normal in-service  allowed                      tributary
port-1-4-T9                      1-4-T9     unlock        up           normal in-service  allowed                      tributary
port-1-IOPANEL-2-U1              1-2-U1     unlock        up           normal in-service  allowed                      usb
port-1-IOPANEL-2-U2              1-2-U2     unlock        up           normal in-service  allowed                      usb
port                 hosted-interface            connected-to  possible-tom-types    actual-type  required-type
-------------------  --------------------------  ------------  --------------------  -----------  -------------
port-1-1-AUX-1       interface-1-AUX-1                         ---                   ---          ---
port-1-1-AUX-2       interface-1-AUX-2                         ---                   ---          ---
port-1-1-CRAFT       interface-CRAFT                           ---                   ---          ---
port-1-1-DCN         interface-DCN                             ---                   ---          ---
port-1-1-U1          ---                                       ---                   ---          ---
port-1-4-L1          super-channel-group-1-4-L1                [ ]                   empty        none
port-1-4-L2          super-channel-group-1-4-L2                [ ]                   empty        none
port-1-4-T1          ---                                       [TOM-400G, TOM-100G]  empty        none
port-1-4-T10         ---                                       [TOM-100G]            empty        none
port-1-4-T11         ---                                       [TOM-100G]            empty        none
port-1-4-T12         ---                                       [TOM-100G]            empty        none
port-1-4-T13         ---                                       [TOM-100G]            empty        none
port-1-4-T14         ---                                       [TOM-100G]            empty        none
port-1-4-T15         ---                                       [TOM-100G]            empty        none
port-1-4-T16         ---                                       [TOM-400G, TOM-100G]  empty        none
port-1-4-T2          ---                                       [TOM-100G]            empty        none
port-1-4-T3          ---                                       [TOM-100G]            empty        none
port-1-4-T4          ---                                       [TOM-100G]            empty        none
port-1-4-T5          ---                                       [TOM-100G]            empty        none
port-1-4-T6          ---                                       [TOM-100G]            empty        none
port-1-4-T7          ---                                       [TOM-100G]            empty        none
port-1-4-T8          ---                                       [TOM-400G, TOM-100G]  empty        none
port-1-4-T9          ---                                       [TOM-400G, TOM-100G]  empty        none
port-1-IOPANEL-2-U1  ---                                       ---                   ---          ---
port-1-IOPANEL-2-U2  ---                                       ---                   ---          ---
```

This example shows how to view inventory data in 1830 GX. Inventory data contains equipment serial number, hardware version, firmware version, part number ,etc.:

```
show inventory
```

The following output is displayed:

```
inventory              hardware-version  actual-type  serial-number  clei          vendor    part-number    PON
---------------------  ----------------  -----------  -------------  ------------  --------  -------------  ------------
inventory-1            shelf 1 hw 0.0    G40          ABCDEFGH_17    CLEI:00000    Infinera  PN:00000       G40
inventory-1-1          HW:000001         XMM4         SN:000001      CLEI:0001     Infinera  XMM4_XYZ       XMM4
inventory-1-2          HW:000002         IOPANEL      SN:000002      CLEI:0002     Infinera  IOPANEL_XYZ    GX-IOP
inventory-1-FAN-1      HW:00FAN1         FAN          SN:00FAN1      CLEI:FAN1     Infinera  FAN_XYZ        GX-FANMODULE
inventory-1-FAN-2      HW:00FAN2         FAN          SN:00FAN2      CLEI:FAN2     Infinera  FAN_XYZ        GX-FANMODULE
inventory-1-FAN-3      HW:00FAN3         FAN          SN:00FAN3      CLEI:FAN3     Infinera  FAN_XYZ        GX-FANMODULE
inventory-1-FAN-4      HW:00FAN4         FAN          SN:00FAN4      CLEI:FAN4     Infinera  FAN_XYZ        GX-FANMODULE
inventory-1-FAN-5      HW:00FAN5         FAN          SN:00FAN5      CLEI:FAN5     Infinera  FAN_XYZ        GX-FANMODULE
inventory-1-FAN-6      HW:00FAN6         XMM4-FAN     SN:00FAN6      CLEI:FAN6     Infinera  FAN_XYZ        GX-FAN-XMM4
inventory-1-FAN-7      HW:00FAN7         XMM4-FAN     SN:00FAN7      CLEI:FAN7     Infinera  FAN_XYZ        GX-FAN-XMM4
inventory-1-FANCTRL-1  HW:FAN-CTRL-1     FAN-CTRL     SN:00FANCTRL   CLEI:FANCTRL  Infinera  FAN-CTRL_XYZ   GX-FAN-CTRL
inventory-1-PEM-1      HW:00PEM1         PEM          SN:00PEM1      CLEI:PEM1     Infinera  PEM_XYZ        GX-PEM-AC
inventory-1-PEM-2      HW:00PEM2         PEM          SN:00PEM2      CLEI:PEM2     Infinera  PEM_XYZ        GX-PEM-AC
inventory-1-PEM-3      HW:00PEM3         PEM          SN:00PEM3      CLEI:PEM3     Infinera  PEM_XYZ        GX-PEM-AC
inventory-1-PEM-4      HW:00PEM4         PEM          SN:00PEM4      CLEI:PEM4     Infinera  PEM_XYZ        GX-PEM-AC
inventory              manufacture-date  insertion-date  fw-version
---------------------  ----------------  --------------  ----------
inventory-1            12/12/2020        12/12/20        FW:0000
inventory-1-1          12/12/2020        12/12/2020      Empty
inventory-1-2          12/12/2020        12/12/2020      FW:000002
inventory-1-FAN-1      12/12/2020        12/12/2020      NA
inventory-1-FAN-2      12/12/2020        12/12/2020      NA
inventory-1-FAN-3      12/12/2020        12/12/2020      NA
inventory-1-FAN-4      12/12/2020        12/12/2020      NA
inventory-1-FAN-5      12/12/2020        12/12/2020      NA
inventory-1-FAN-6      12/12/2020        12/12/2020      NA
inventory-1-FAN-7      12/12/2020        12/12/2020      NA
inventory-1-FANCTRL-1  12/12/2020        12/12/2020      Empty
inventory-1-PEM-1      12/12/2020        12/12/2020      FW:00PEM1
inventory-1-PEM-2      12/12/2020        12/12/2020      FW:00PEM2
inventory-1-PEM-3      12/12/2020        12/12/2020      FW:00PEM3
inventory-1-PEM-4      12/12/2020        12/12/2020      FW:00PEM4
```

This example shows how to view the RSA and ECDSA key pair.

```
systemx> show ssh-host-key
ssh-host-key
--------------------------------
ssh-host-key-ecdsa-sha2-nistp521
ssh-host-key-ssh-rsa4096
ssh-host-key                      public-key                                                                                      ->
--------------------------------  ------------------------------------------------------------------------------------------------->
ssh-host-key-ecdsa-sha2-nistp521  AAAAE2VjZHNhLXNoYTItbmlzdHA1MjEAAAAIbmlzdHA1MjEAAACFBAEpRgyv4Ja998BF46n2UfpA3CZqswbcITeReNivPbJ1->
ssh-host-key-ssh-rsa4096          AAAAB3NzaC1yc2EAAAADAQABAAACAQD7SPqIi99KrjwLOQlNb+DbYsrVTJM62z8MFU+16IhLt6pnI4wNSLgAKEDY35CgmBnF->
ssh-host-key                      label  fingerprint
--------------------------------  -----  -------------------------------------------------------
ssh-host-key-ecdsa-sha2-nistp521         521 SHA256:4vDg2G054Kr01I8hWWlU5BUSi9/pP182kRaWuLN7AMo
ssh-host-key-ssh-rsa4096                 4096 SHA256:au1wtBv7q2dTY8KmA5e1h4RcETvg/OaM1iLsvCe5wZc
```

This example shows how to view the supported cards.

```
show supported-card
supported-card           supported-subtype
-----------------------  -----------------------------
supported-card-BLANK
supported-card-CHM6      C8,C6,C4,C14,C13
supported-card-FAN       single-rotar,counter-rotating
supported-card-FAN-CTRL
supported-card-IOPANEL
supported-card-PEM       DC,AC
supported-card-XMM4
supported-card-XMM4-FAN
supported-card           description                                                                  card-width
-----------------------  ---------------------------------------------------------------------------  -----------
supported-card-BLANK     Generic Blank card, with multiple form factors                               single-slot
supported-card-CHM6      CHM6 (Coherent Module with ICE6) Transponder                                 single-slot
supported-card-FAN       Fan Module - provides cooling to the chassis line cards.                     single-slot
supported-card-FAN-CTRL  FAN controller module - controls chassis FANs.                               single-slot
supported-card-IOPANEL   Input/Outpul Panel - Auxiliary panel with USB ports and chassis level LEDs.  single-slot
supported-card-PEM       PEM (Power Entry Module) - Provides AC or DC power to the chassis.           single-slot
supported-card-XMM4      XMM4 (GX Main Module 4) - Main Controller card                               single-slot
supported-card-XMM4-FAN  XMM4 Fan Module - provides cooling to the XMM4 controller cards.             single-slot
supported-card           card-height (RUs)  is-field-replaceable  category      max-power-draw (W)
-----------------------  -----------------  --------------------  ------------  ------------------
supported-card-BLANK     1                  true                  other         0.00
supported-card-CHM6      1                  true                  line-card     469.00
supported-card-FAN       1                  true                  fan           117.12
supported-card-FAN-CTRL  1                  true                  other         1.20
supported-card-IOPANEL   1                  true                  other         26.30
supported-card-PEM       1                  true                  power-supply  0.00
supported-card-XMM4      1                  true                  controller    58.80
supported-card-XMM4-FAN  1                  true                  fan           23.00
supported-card           location-led-support  sw-support-revision
-----------------------  --------------------  -------------------
supported-card-BLANK     false                 ---
supported-card-CHM6      true                  1
supported-card-FAN       false                 ---
supported-card-FAN-CTRL  true                  ---
supported-card-IOPANEL   true                  ---
supported-card-PEM       false                 ---
supported-card-XMM4      true                  1
supported-card-XMM4-FAN  false                 ---
```

This example shows how to view the configuration of super-channel:

```
show super-channel-1-6-L2-1
  super-channel-1-6-L2-1
  supporting-card                     1-6
  supporting-port                     L2
  supporting-facilities
  supported-facilities                optical-carrier-1-6-L2-1,optical-channel-1-6-L2-1
  AID                                 '1-6-L2-1'
  label                               ''
  admin-state                         unlock
  oper-state                          disabled
  avail-state                         'facility-failure automatic out-of-service'
  managed-by                          user
  alarm-report-control                allowed
  carriers                            1-6-L2-1
  carrier-mode                        '800E.96P'
  capacity                            800 Gbps
  client-mode                         ethernet
  baud-rate                           95.6390657 GBaud
  application                         'P'
  spectral-bandwidth                  101.61651 GHz
```

This example shows how to view the configuration of an optical carrier:

```
show optical-carrier-1-6-L2-1
  optical-carrier-1-6-L2-1
  supporting-card                       1-6
  supporting-port                       L2
  supporting-facilities                 super-channel-group-1-6-L2,super-channel-1-6-L2-1
  supported-facilities                  comm-channel-1-6-L2-1
  AID                                   '1-6-L2-1'
  label                                 ''
  admin-state                           unlock
  oper-state                            disabled
  avail-state                           'facility-failure automatic out-of-service'
  managed-by                            system
  alarm-report-control                  allowed
  frequency                             unknown value/wrong enum MHz
  frequency-offset                      0 MHz
  wavelength                            0.000 nm
  tx-power                              -6.00 dBm
  tx-cd                                 0.00 ps/nm
  pre-fec-q-sig-deg-threshold           6.00 dB
  pre-fec-q-sig-deg-hysteresis          0.5 dB
  dgd-high-threshold                    300 ps
  post-fec-q-sig-deg-threshold          18.0 dB
  post-fec-q-sig-deg-hysteresis         2.5 dB
  enable-advanced-parameters            false
```

This example shows how to view system's facilities:

```
show facilities
  facilities
  super-channel-group-1-6-L1
  super-channel-group-1-6-L2
  super-channel-group-1-7-L1
  super-channel-group-1-7-L2
  super-channel-1-6-L2-1
  optical-carrier-1-6-L2-1
  optical-channel-1-6-L2-1
  otu-1-6-L2-1-OTUCni
  odu-1-6-L2-1-ODUCni
  trib-ptp-1-6-T1
  trib-ptp-1-6-T10
  trib-ptp-1-6-T16
  trib-ptp-1-6-T2
  trib-ptp-1-6-T4
  trib-ptp-1-6-T5
  trib-ptp-1-6-T8
  trib-ptp-1-7-T4
  trib-ptp-1-7-T6
  comm-channel-1-6-L2-1
  ethernet-1-6-T1
  ethernet-1-6-T10
  ethernet-1-6-T16
  ethernet-1-6-T2
  ethernet-1-6-T4
  ethernet-1-6-T5
  ethernet-1-6-T8
  ethernet-1-7-T4
  ethernet-1-7-T6
```

This example shows how to view the configuration of a specific facility:

```
show facilities otu-1-6-L2-1-OTUCni
  otu-1-6-L2-1-OTUCni
  otu-diagnostics-1-6-L2-1-OTUCni/ingress
  supporting-card                                      1-6
  supporting-port                                      L2
  supporting-facilities                                optical-channel-1-6-L2-1
  supported-facilities                                 odu-1-6-L2-1-ODUCni
  AID                                                  '1-6-L2-1'
  label                                                ''
  admin-state                                          unlock
  oper-state                                           disabled
  avail-state                                          'facility-failure supporting-faulted automatic out-of-service'
  managed-by                                           system
  alarm-report-control                                 allowed
  otu-type                                             OTUCni
  rate                                                 800.0 Gbit/s
  otu-name                                             'OTUC8i'
  service-mode                                         adaptation
  service-mode-qualifier                               'ODUC8i'
  fec-mode                                             enabled
  fec-generation-mode                                  enabled
  loopback                                             none
```

This example shows how to view all xcon configuration:

```
 show xcon
xcon    AID         oper-state  avail-state   label  source            destination  direction
------  ----------  ----------  ------------  -----  ----------       -----------  ---------
xcon-channa  1-6-T16,1-6-L2-1-ODU4i-401  disabled    supporting-faulted automatic out-of-service         ethernet-1-6-T16  odu-low4     two-way
xcon         time-slots  type      payload-type  network-mapping  protection-type  circuit-id
 circuit-id-suffix
-----------  ----------  --------  ------------  ---------------  ---------------  ---------------------------------------------------
 -----------------
xcon-channa  401..480    add-drop  100GBE-LAN    ODU4i            unprotected      2021-03-10T12:18:17Z|GX|1-6-T16,1-6-L2-1-ODU4i-401|
xcon         signaling-type
-----------  --------------
xcon-channa  manual
```

This example shows how to view the configuration of an odu:

<!-- page 1120 -->

```
show odu-low4
  odu-low4
  odu-diagnostics-low4/ingress
  supporting-card                           1-6
  supporting-port                           L2
  supporting-facilities                     odu-1-6-L2-1-ODUCni,optical-carrier-1-6-L2-1
  supported-facilities
  AID                                       '1-6-L2-1-ODU4i-401'
  label                                     ''
  admin-state                               unlock
  oper-state                                disabled
  avail-state                               'facility-failure supporting-faulted automatic out-of-service'
  managed-by                                user
  alarm-report-control                      allowed
  parent-odu                                1-6-L2-1-ODUCni
  odu-type                              A    ODU4i
  rate                                      100.0 Gbit/s
  odu-name                                  'ODU4i-401'
  class                                     low-order
  service-mode                              adaptation
  service-mode-qualifier                    none
  time-slots                                '401..480'
```

This example shows how to view a trusted certificate:

```
show certificates trusted-certificate-root
trusted-certificate-root
version                               v3
  serial-number                         '743DEBB7F4C7C4C991D3D860B82709A367F226E0'
  subject-name                          '/C=IN/ST=KAR/L=BLR/O=INFIN/OU=GX/CN=Infinera/emailAddress=anantheshv@infinera.com'
  issuer                                '/C=IN/ST=KAR/L=BLR/O=INFIN/OU=GX/CN=Infinera/emailAddress=anantheshv@infinera.com'
  valid-from                            '2020-10-18T08:32:50Z'
  valid-to                              '2025-10-17T08:32:50Z'
  status                                available
  public-key-length                     rsa2048
  public-key-type                       rsa
  signature-key-type                    rsa
  signature-hash-algorithm              sha256
  certificate-bytes                     '-----BEGIN CERTIFICATE-----
                                        MIID5TCCAs2gAwIBAgIUdD3rt/THxMmR09hguCcJo2fyJuAwDQYJKoZIhvcNAQEL
                                        BQAwgYExCzAJBgNVBAYTAklOMQwwCgYDVQQIDANLQVIxDDAKBgNVBAcMA0JMUjEO
                                        MAwGA1UECgwFSU5GSU4xCzAJBgNVBAsMAkdYMREwDwYDVQQDDAhJbmZpbmVyYTEm
                                        MCQGCSqGSIb3DQEJARYXYW5hbnRoZXNodkBpbmZpbmVyYS5jb20wHhcNMjAxMDE4
                                        MDgzMjUwWhcNMjUxMDE3MDgzMjUwWjCBgTELMAkGA1UEBhMCSU4xDDAKBgNVBAgM
                                        A0tBUjEMMAoGA1UEBwwDQkxSMQ4wDAYDVQQKDAVJTkZJTjELMAkGA1UECwwCR1gx
                                        ETAPBgNVBAMMCEluZmluZXJhMSYwJAYJKoZIhvcNAQkBFhdhbmFudGhlc2h2QGlu
                                        ZmluZXJhLmNvbTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKHVCQY3
                                        K5TOA3aUWv52FNHmNRhptOkd5n1dFLzVbAM9nth4Oc2ZXqhqskJ/7ONTaNKwN6b4
                                        b7ssaCvj6sY6fJREtC9kcwi8F/bApQ0txVdEUyrOCGuZpaYgoXhb5C3kynEB0CWn
                                        9tNxjjIgl/YMCPURo/5ixUBt1kYlhPl5R7EtazHf75c5vRTwR55D3tOnVkdGG2yo
                                        qD2ze5aJAJ/Qtm4t/pc6SMXlsmQ2bmBBTBwIcwiKD33dsSG2kBj+aDA3UGHYxgEj
                                        +tl5buyN7Er3wSnYo5ce50QjKSMhkJnBggG1yWft4FbqWUHOREK/IIk6bG9DKTTe
                                        0XbcVNU2cwCjLoMCAwEAAaNTMFEwHQYDVR0OBBYEFLY+5r7Lz3As53X0EbNFYN+i
                                        mirjMB8GA1UdIwQYMBaAFLY+5r7Lz3As53X0EbNFYN+imirjMA8GA1UdEwEB/wQF
                                        MAMBAf8wDQYJKoZIhvcNAQELBQADggEBAH3ijHfHjQGCfLLxkLMiPnQC73ihfORj
                                        GbB4vyBu5xXJ9ig6HSUDyrxSIOdzCRP2bKiioZ8SQJUo4Li0agRPfZsI9q7BgpME
                                        ZfGyhtvHb1BvG8xaW0K9saKnpGY2mDK81pfb731iXISgQuk7iF6C5U4en+zPtnyJ
                                        h1IydNioieqRzBvxobv7njBh9VvKw6kSwMhsdK270BfhmGasI8PBwb2YnTHykmf4
                                        3PX1JijwusjInqel8hh3MMM3u5HdZ/zjtlvtYEQcBp2EP4KirRf0Yfch/ai53zPh
                                        FURBlSMdqmglaC/gs6nArkc9F5G92MyZsgPDIQmLZMUeul/dkxbqeuQ=
                                        -----END CERTIFICATE-----
                                        '
  alarm-report-control                  allowed
```

This example shows how to view a local certificate:

```
show certificates local-certificate-client
  local-certificate-client
  version                               v3
  serial-number                         '538FD88D6430585A04ED33763CF06581AED4F4D2'
  subject-name                          '/C=IN/ST=KAR/L=BLR/O=INFN/OU=GX/CN=NE9/emailAddress=anantheshv@infinera.com'
  issuer                                '/C=IN/ST=KAR/L=BLR/O=INFIN/OU=GX/CN=Infinera/emailAddress=anantheshv@infinera.com'
  valid-from                            '2020-12-31T07:36:03Z'
  valid-to                              '2023-04-05T07:36:03Z'
  status                                unused
  public-key-length                     rsa2048
  public-key-type                       rsa
  signature-key-type                    rsa
  signature-hash-algorithm              sha256
  certificate-bytes                     '-----BEGIN CERTIFICATE-----
                                        MIID1zCCAr+gAwIBAgIUU4/YjWQwWFoE7TN2PPBlga7U9NIwDQYJKoZIhvcNAQEL
                                        BQAwgYExCzAJBgNVBAYTAklOMQwwCgYDVQQIDANLQVIxDDAKBgNVBAcMA0JMUjEO
                                        MAwGA1UECgwFSU5GSU4xCzAJBgNVBAsMAkdYMREwDwYDVQQDDAhJbmZpbmVyYTEm
                                        MCQGCSqGSIb3DQEJARYXYW5hbnRoZXNodkBpbmZpbmVyYS5jb20wHhcNMjAxMjMx
                                        MDczNjAzWhcNMjMwNDA1MDczNjAzWjB7MQswCQYDVQQGEwJJTjEMMAoGA1UECAwD
                                        S0FSMQwwCgYDVQQHDANCTFIxDTALBgNVBAoMBElORk4xCzAJBgNVBAsMAkdYMQww
                                        CgYDVQQDDANORTkxJjAkBgkqhkiG9w0BCQEWF2FuYW50aGVzaHZAaW5maW5lcmEu
                                        Y29tMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1PL0+DI0riCkr5Y5
                                        Ookl61pnu8E8qd2V3ANH9QKG5IgvAVjiDWnYm2wpmUJpU7guK40Wd9tP5sFczNwC
                                        2Tq0FbaWTKMjrmqcomeMYuQpUdq/71wWtY/eTsxRQGCSd+oZnUOVB8vkEeHWgH4v
                                        KENMDvtQqGoPqFJZtcNeoE53DM/tJu1z6UL3Nmg3E2gMcouoHQhwEqfvwGzwe1so
                                        JPVNXdp3EcZz9MFjH7jh4+1ogSgBCKXh83+GahSAI7OxeOeURGv4bEd2kV0HVBkU
                                        C+eAYtezHLr/Iwf1KqnQP9wG5WqKcioFl/yBKCm5k4r2l1eZ+7Scqu0et5jMRhkC
                                        chcBjwIDAQABo0wwSjAfBgNVHSMEGDAWgBS2Pua+y89wLOd19BGzRWDfopoq4zAJ
                                        BgNVHRMEAjAAMAsGA1UdDwQEAwIE8DAPBgNVHREECDAGggRTaW05MA0GCSqGSIb3
                                        DQEBCwUAA4IBAQBDJE5maaeIiQfsX6FHlkwvL/RQ+AGuZHU44sJAwr88lEF/heEW
                                        DWb57wkSwWv/Lg0werLS0noC0pfCsWndES82N6O8akB9OElZvnLCoquTRh08Z9Od
                                        HmDqahiNeFFroAn8mn0lz9M6e627rVoxz3kGaWaYGxE8MYE97fl/+7VMajSEITMi
                                        RY2MDv/xSST5lGbRIw0yH62NVxAHUmJV+RObuU22jiA4LXN4Jtim6Wh6O8EQDtpN
                                        K/C6oBJYDJACz0PBDfd6ijh3dYiFq2aNggMLuxTuiOEijyEHbdudpFPufCAFGoJW
                                        6pRpLZJQnF0w5HeOJb1DdvKiVpyQt/kSlgg0
                                        -----END CERTIFICATE-----
                                        '
  subject-alternative-names             'DNS:Sim9'
  alarm-report-control                  allowed
```

r

<!-- page 1125 -->
