# Parameter index

1787 distinct parameter and attribute names across 3800 parameter rows. Use this to answer "which command sets *X*". The description shown is the first one the document gives for that name; other commands may define it differently, so always confirm on the command page.

| Parameter | Commands | First description |
| --- | --- | --- |
| `&lt; &gt;` | `topology` | Topology instance to be viewed:<br>• inci - Refer to for inci (p. 549) additional information on INCI parameters.<br>• links - Refer to links (p. 611) for addit |
| `&lt;attribute&gt;` | `sort` | Any attribute name that exists in the context of the output. |
| `&lt;command&gt;` | `display`, `exclude`, `grep`, `highlight`, `include`, `linenum`, `more` | Any display command such as tree or show. |
| `&lt;entity id&gt;` | `edit`, `tree` | Instance ID of the entity to be addressed. |
| `&lt;filter&gt;` | `exclude`, `grep`, `include` | Text to be filtered. It can have spaces if enclosed by quotes and supports regex (regular expressions). |
| `&lt;id&gt;` | `diff`, `show commit` | It is a system generated commit-id. |
| `&lt;mode&gt;` | `display` | The display mode to be selected. The supported modes for the show command are:<br>• list - The default display mode. Displays the results as a standard list, on |
| `&lt;name&gt;` | `alias`, `unalias` | Name of the alias to add. |
| `&lt;option&gt;` | `grep` | The following options are supported for grep:<br>• -a=&lt;n&gt; - Number of lines of context to show after the actual match.<br>• -b=&lt;n&gt; - Number of lines |
| `&lt;show command&gt;` | `sort` | show command. |
| `&lt;value&gt;` | `alias` | Value to replace the alias name with. |
| `&lt;word&gt;` | `highlight` | Any word to highlight. May contain spaces, if enclosed by quotes. |
| `&lt;words&gt;` | `begin`, `until` | Line to begin with. May contain spaces if in quotes. |
| `-a` | `download`, `show`, `tree`, `unalias`, `upload` | Displays the attributes only (no container instances). |
| `-b` | `delete` | best effort delete, displays no error even if the object does not exist. |
| `-b=&lt;key-length&gt;` | `ssh-keygen` | Strength of the key used for regenerating the private-public key pair. |
| `-c` | `show` | Displays the container instances only (no attributes). |
| `-c=&lt;count&gt;` | `ping` | Stop after sending count ECHO REQUEST packets. With deadline option, ping waits for _ count ECHO REPLY packets, until the timeout expires. _ |
| `-d` | `show` | Displays default values as additional information; (*) is shown if current value is default (-d requires list view; use '\ |
| `-d=depth` | `tree` | Maximum display depth of the directory tree. The depth specifies the number of levels to be displayed in the tree syntax. If not specified all levels are displa |
| `-e` | `run` | Configure error option (-e flag in CLI) , with options:<br>• continue: continue on error (default)<br>• stop: stop on error<br>• rollback: rollback on error |
| `-ebin=&lt;number&gt;` | `pm` | Last bin to display; by default, all available bins are displayed. |
| `-et=&lt;timestamp&gt;` | `pm` | End time of the PM record entry; same format as for -st. |
| `-f` | `ISK`, `KRK`, `activate`, `add`, `alarm`, `app`, `cert-gen`, `change-ztp-mode`, `clear`, `console`, `database`, `db-migrate` +17 more | Forces the command without confirmation. |
| `-h` | `activate`, `add`, `alias`, `begin`, `call-home`, `cancel-upgrade`, `clear`, `console`, `convert`, `default`, `delete`, `display` +37 more | Displays help for this command. |
| `-i` | `download`, `import-certificate`, `prepare-upgrade`, `sort` | Inverts the order. |
| `-i=&lt;chassis-name&gt;` | `pm` | Shows the PM data for the chassis with chassis-name=&lt;chassis-name&gt;. i Note: The chassis flag (-i) can only be provided once per command. Chassis flag repr |
| `-i=&lt;interface&gt;` | `ping`, `traceroute` | Specifies source interface. By default, the interface is selected according to the routing table. |
| `-id` | `commit` | This command &lt;id&gt; defines the ID of the commit confirmed, commit persist and confirmed cancel commands. |
| `-l` | `show` | Long display; shows additional information. |
| `-l=&lt;key-label&gt;` | `ssh-keygen` | Label associated with the key. If no value provided, label will be the value of ne-id. |
| `-m` | `add`, `comm-channel`, `commit`, `ethernet` | Merge configuration (will not fail if entity already exists). If valid, the command replies with 'OK'. Otherwise the command will fail. |
| `-m=&lt;hopcnt&gt;` | `traceroute` | Specifies the maximum number of hops (max time-to-live value) traceroute will probe. |
| `-n` | `log`, `show commit` | This parameter selects an exact number of records to obtain. |
| `-n=&lt;number-of-records&gt;` | `pm` | Maximum number of records that will be retrieved, per chassis. The default is 1000 records. |
| `-o` | `show`, `tree` | Displays state/read-only attributes only. |
| `-q` | `run` | Execute CLI script in quiet mode. Only command output is shown to user; otherwise both command being executed and command output is shown. |
| `-r` | `cable-id-path`, `expect`, `run` | Enables regex mode. If omitted, the expected-value is a literal string. |
| `-r=&lt;n&gt;` | `show` | Operate the command recursively for n levels down; if n is not provided full recursion is used. |
| `-s` | `download`, `show commit`, `upload` | This parameter which allows to pick a timestamp, showing all records created since that timestamp. |
| `-s=&lt;pktsize&gt;` | `ping` | Specifies the number of octets to be sent, exclusive of all headers. Default is 56, plus 8 octets of ICMP header for a total packet size of 64 octets. |
| `-s=&lt;skip-records&gt;` | `pm` | Skips the first entries of number of records. Allows a user to specify a number of records that will be skipped, so that the total data can be fetched in multip |
| `-sbin=&lt;number&gt;` | `pm` | First bin to display; by default, bin 0 (current data) is the first bin. |
| `-st=&lt;timestamp&gt;` | `pm` | Start time of the PM record entry; format example: 2021-02-01T10:00:00+01:00. |
| `-t` | `log`, `pm`, `show` | Displays information in tabular format. |
| `-t=&lt;type&gt;` | `ssh-keygen` | Specify type of key to generate. |
| `-u` | `download`, `prepare-upgrade` | Auto prepare and auto activate file after a successful download. Only some files support 'activation'; others just ignore this flag. |
| `-v` | `activate`, `add`, `console`, `delete`, `set` | Validates the command. |
| `-v=&lt;vrf&gt;` | `ping`, `traceroute` | Specifies VRF. By default, use the MGMT VRF. i Note: The interface and VRF name parameters are mutually exclusive. |
| `-w=&lt;timeout&gt;` | `ping`, `traceroute` | Specify a timeout, in seconds, before ping exits regardless of how many packets have been sent or received. In this case ping does not stop after count packet a |
| `-x` | `show` | Displays configuration/read-write attributes only. |
| `2 clear-type` | `activate` | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory default |
| `3 clear-type` | `download` | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory default |
| `4 diverse-routing` | `port` | Controls enabling/disabling of diverse routing capability. |
| `5 clear-type` | `prepare-upgrade` | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory default |
| `7 egress-port-list` | `trib-ptp` | A list of port AIDs that are bound to this trib-ptp for diverse-routing. |
| `aaa-authentication-method` | `security-policies` | Specifies the authentication method for the user login to the NE. |
| `aaa-authorization-method` | `security-policies` | Specifies the authorization policy for the logged user. If the user changes this parameter, he must logout and login again to apply the rules. |
| `aaa-server` | `add`, `delete`, `set`, `show` | The name of the aaa server. See aaa-server (p. 127) for more information. |
| `accepted-group-id` | `flexo` | The received group instance id on the FlexO interface. |
| `accepted-iid` | `flexo` | The received iid on the FlexO interface. |
| `accepted-time-slots` | `odu` | Received and accepted TS for the LO-ODU entity. |
| `accepted-trib-port-number` | `odu` | Received and accepted Tributary Port Number for the LO- ODU entity. |
| `access-control-list` | `set`, `show` | Attributes and objects pertaining to ACLs. These values can be retrieved by using show access-control-list. See access-control-list (p. 133) for more informatio |
| `access-rule` | `add`, `delete`, `set`, `show` | Single access-rule in a group of access rules, defining access to a particular target path. See access-rule (p. 134) for more information. |
| `access-rule-list` | `add`, `delete`, `set`, `show` | Group of access-rules, organized by which user-groups the rules apply to. See access-rule-list (p. 139) for more information. |
| `access-rule-list-name` | `access-rule` | The name of the access-rule-list. |
| `access-rule-name` | `access-rule` | The name of the access-rule. Represents a single access-rule, defining access to a particular target path. The rule can also consider multiple filters, includin |
| `accounting-requests` | `aaa-statistics` | Displays the number of accounting requests. |
| `ace` | `add`, `delete`, `set`, `show` | Set of attributes for an access control entry (ACE). See ace (p. 141) for more information. |
| `acknowledge-text` | `set-alarm-state` | Optional text that will be stored in the alarm. |
| `acl` | `add`, `delete`, `set`, `show` | Set of attributes associated with every access control list (ACL). An ACL can have one or more ACEs. See acl (p. 144) for more information. |
| `acrd-reference` | `nmc` | Automatic Channel Recovery Detection reference trace acquisition. By default, it is set to manual-acquisition-mode.<br>• manual-acquisition-mode: Manual acquisi |
| `action` | `access-rule`, `ace`, `ip-monitoring`, `ipsec-spd-entry`, `system` | The action to take when the monitoring goes into 'failed' state. |
| `activate-file` | `activate` | Command parameter for activating a file. For activate-file specific parameters, refer to Table 77: activate activate-file Command Parameters (p. 151). |
| `activation-mode` | `oxcon` | OXcon activation mode:<br>• automatic - The service is activated automatically by the system on creation. Similarly, service is deactivated automatically on del |
| `activation-request-bwd` | `oxcon` | Activation request for the backward direction (destination to source). This attribute is applicable only when activation-mode is manual:<br>• no-request - This  |
| `activation-request-fwd` | `oxcon` | Activation request for the forward direction (source to destination). This attribute is applicable only when activation-mode is manual:<br>• no-request - This i |
| `activation-state-bwd` | `oxcon` | Activation state of the backward direction (destination to source). This attribute is applicable only when activation-mode is manual:<br>• not-applicable - The  |
| `activation-state-fwd` | `oxcon` | Activation state of the forward direction (source to destination). This attribute is applicable only when activation-mode is manual:<br>• not-applicable - The a |
| `active-certificate-id` | `secure-application` | List of assigned certificates for this secure application. |
| `active-controller-slot` | `chassis` | Identifies the active controller slot number. A change in this attribute allows the check of a switchover (the switchover check is not applicable to G31 chassis |
| `active-path` | `optical-switch` | Displays the current active path of the optical-switch. |
| `actual-bandwidth` | `nmc-f` | Actual Bandwidth of the NMC Filler. |
| `actual-baud-rate` | `console` | The actual baud-rate for this card's console port. If auto-sensing is enabled, this will reveal the detected baud-rate. If a fixed baud-rate is configured, this |
| `actual-carrier-mode` | `golden-carrier-mode` | The actual carrier-mode. |
| `actual-frequency` | `optical-carrier` | A super set for line and client side carrier frequency, specific sub-range is depend on application. 0 represents a non-initialized frequency. |
| `actual-lower-frequency` | `nmc-f` | Actual lower Frequency of the NMC Filler. |
| `actual-max-power-draw` | `inventory` | Maximum power draw indicated by the pluggable for the reported power class, when available. Populated under the same conditions as actual-power-class. |
| `actual-power-class` | `inventory` | Power class reported by the pluggable (for example from module management data), when available. Only populated for third-party transceiver subtypes on hosts th |
| `actual-power-draw` | `chassis` | Actual power draw on the chassis |
| `actual-power-draw-alarm-threshold` | `chassis` | The actual power draw value at the chassis at which the PWRDRAW alarm is raised. User configured limit of power usable by this chassis. This parameter is not ap |
| `actual-power-support` | `optical-ptp` | Port power monitoring support. |
| `actual-pump-power` | `pump-power` | The actual values which are currently measured in each pump. |
| `actual-raman-osc-gain` | `amplifier-raman` | Indicates the OSC Raman gain. It is the actual Raman gain OSC. Note: when Raman amplifier is disabled (or card's oper-state = disabled), the value is 0. The val |
| `actual-raman-signal-gain` | `amplifier-raman` | Indicates the Raman Signal Gain. It is the actual Raman gain of C- Band (signal). Note: when Raman amplifier is disabled (or card's oper-state = disabled), the  |
| `actual-rx-frequency` | `optical-carrier` | A super set for line and client side carrier frequency, specific sub-range is depend on application. 0 represents a non-initialized frequency. |
| `actual-subtype` | `inventory`, `unprovisioned-inventory` | FRU subtype of actual equipment - only available if applicable. |
| `actual-transmission-band` | `amplifier` | Currently assigned transmission band. If amplifier is not at a degree, it will be 4.85 THz by convention. |
| `actual-type` | `inventory`, `unprovisioned-inventory` | FRU type of actual equipment. |
| `actual-upper-frequency` | `nmc-f` | Actual Upper Frequency of the NMC Filler. |
| `ad-direction` | `ocm-mp`, `ocm-ptp` | Reference to the AD (coupler/ splitter) DWDM port. |
| `additional-details` | `alarm`, `controller-card` | Additional details for synchronization status. |
| `additional-info` | `cable-id-path`, `raman-calibration` | Indicates any information for troubleshooting when the calibration-state is fail or out-dated. |
| `additional-key-exchange-id` | `additional-key-exchange` | Specifies the number of rounds of additional key exchange algorithms to be configured. |
| `address` | `dial-out-server`, `dns-server`, `log-server`, `management-address`, `management-address-local`, `ospf-neighbor`, `ssh-known-host` | The IP address of the DNS server. |
| `address-family` | `rib` | Address family. |
| `address-oid` | `management-address`, `management-address-local` | The Object Identifier (OID) value used to identify the type of hardware component or protocol entity associated with the management address advertised by the re |
| `address-subtype` | `management-address`, `management-address-local` | The type of management address identifier encoding used in the associated 'address' attribute. |
| `adg` | `add`, `delete`, `set`, `show` | Set of Add/Drop Group attributes on OADM nodes. See adg (p. 173) for more information. |
| `adg-number` | `adg`, `modules-adg`, `ocm-ptp` | ADG identifier as a number. |
| `admin-state` | `acl`, `amplifier`, `amplifier-raman`, `ase-idler-source`, `card`, `chassis`, `cid-ptp`, `comm-channel`, `comm-eth`, `dsc`, `dsc-group`, `eth-zr` +38 more | The administrative state of the managed object. |
| `advanced-parameter` | `add`, `delete`, `set`, `show` | See advanced-parameter (p. 175) for more information. |
| `advanced-parameter-name` | `advanced-parameter` | The name of the advanced parameter. |
| `advertise` | `ospf-area-range` | Advertise or hide. |
| `advertised` | `ipv4-static-route`, `ipv6-static-route` | When set to YES, the static route is advertised in the routing protocol. For OSPF, the static route will be advertised as an AS external route, if OSPF is confi |
| `afi-safi` | `bgp-neighbor` | Specifies the afi-safi value. GNE only exports and imports IPv4 or IPv6 unicast with afi-safi value set to IPv4 unicast or IPv6 unicast. _ _ |
| `age` | `carrier-neighbor`, `lldp-neighbor` | Hardware version of this FRU. |
| `aid` | `alarm`, `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `chassis`, `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet` +47 more | Resource Access Identifier (AID). Identifies an instance within a specific resource type. |
| `alarm` | `clear`, `show` | Clear alarms that have no auto criteria to be cleared. For additional details, refer to alarm (p. 178). |
| `alarm-category` | `alarm`, `alarm-inventory` | Category of the alarm type. |
| `alarm-control` | `set`, `show` | Attribute associated with alarm management control (ARC). See alarm-control (p. 182) for more information. |
| `alarm-direction` | `simulate` | The direction of the simulated alarm. If omitted, system selects direction automatically. |
| `alarm-id` | `alarm` | Alarm instance that represents a raised alarm, when entry is created, or a cleared alarm, when entry is deleted. |
| `alarm-id-list` | `set-alarm-state` | List of alarm-ids to change the state (from 1 up to 10 alarm ids). |
| `alarm-inventory` | `show` | See alarm-inventory (p. 184) for more information. |
| `alarm-location` | `simulate` | The location of the simulated alarm. If omitted, system selects location automatically. |
| `alarm-report control` | `rsc` | Flag indicating if alarm reporting is enabled. |
| `alarm-report-control` | `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `card`, `chassis`, `comm-channel`, `dial-out-server`, `dsc`, `dsc-group`, `eth-zr`, `ethernet` +49 more | Flag indicating if alarm the reporting is allowed. It controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br |
| `alarm-report-ready` | `chassis`, `ne` | Represents the alarm monitoring state for this chassis. After a system restart, alarms are kept persistent for a grace minute period, after which they will be c |
| `alarm-severity-entry` | `set`, `show` | Individual entry in alarm-severity-profile. It allows to configure the severity for one particular alarm. See alarm-severity-entry (p. 186) for more information |
| `alarm-severity-profile` | `show` | See alarm-severity-entry (p. 186) for more information. |
| `alarm-soaking-behavior` | `alarm-control` | System -wide alarm-soaking-behavior switch:<br>• automatic: soaking time used is defined in FM profile.<br>• no-soak: certain alarms specified in FM profile won |
| `alarm-type` | `alarm`, `alarm-inventory`, `alarm-severity-entry`, `get-conditions`, `simulate` | The alarm type to be simulated; if omitted when clearing alarms, all simulated alarms are cleared. |
| `alarm-type-description` | `alarm`, `alarm-inventory` | Description of the type of the alarm. |
| `alarmed-entity` | `simulate` | The entity affected by the alarm; if omitted when clearing alarms, all simulated alarms are cleared. |
| `alarms` | `show` | Top level container for all system alarms, which are defined as an undesirable state in a resource that requires corrective action. See alarm (p. 178) for more  |
| `algorithm` | `encryption-algorithm` | The encryption algorithm for the IKE SA. |
| `alias-name` | `card`, `chassis`, `port`, `tom` | User defined alias for this entity. |
| `all-alarms` | `set-alarm-state` | Acknowledge all currently raised alarms. |
| `alloc-bandwidth` | `nmc-f` | Allocated Bandwidth of the NMC Filler. |
| `alloc-lower-frequency` | `nmc-f` | Allocated Lower Frequency of the NMC Filler. |
| `alloc-upper-frequency` | `nmc-f` | Allocated Upper Frequency of the NMC Filler. |
| `allocated-spectrum-list` | `submarine-link` | Allocated spectrum blocks for the link configured as a set of start frequency, end frequency pairs. It is a list of frequencies defined in a (start-freq, end-fr |
| `allow-switching` | `verify` | In protection scenarios, it specifies whether the Cable-id function is allowed to initiate switching on OPSM to verify working and protection paths:<br>• true - |
| `allows-auto-migration` | `supported-port` | Indicates if TOMs that are plugged on this port type are auto migrated according with the equipment-policies tom-auto-migration flag. |
| `altitude` | `ne` | Altitude of the NE. |
| `amp-control-support` | `amplifier` | Whether 'control-mode' can be configured as 'auto-max-pw' or not. |
| `amplifier` | `set`, `show` | Managed Object for optical amplifier (EDFA amplifier). See amplifier (p. 191) for more information. |
| `amplifier-enable` | `amplifier`, `amplifier-raman` | Enable or disable the amplifier. Output power is dependent on:<br>• node-type ILA: existence of OSC signal<br>• node-type OADM: OMS related facilities and exist |
| `amplifier-mode` | `amplifier` | The operating mode of the amplifier (gain or power control). Only constant-gain is used. |
| `amplifier-raman` | `show` | See amplifier-raman (p. 205) for more information. |
| `amplifier-turn-on-delay` | `amplifier` | Allows the user to configure the timer value for the pre-amplifier of RD20TM card. The value can be within the range of 0 to 24 minutes. By default, the value i |
| `amplifier-turn-on-remain` | `amplifier` | Display the remaining time of the amplifier-turn-on-delay timer. This attribute is applicable to RD20TM card. |
| `amplifier-type` | `amplifier` | Type of the amplifier HW. |
| `annotate-cli-name` | `netconf` | If enabled, annotates NETCONF XML output with cli names for traceability. |
| `anti-replay-window` | `ipsec-spd-entry` | When action = 'protect', indicates the replay window size tolerance. |
| `api-root` | `restconf` | Root of the RESTCONF API. |
| `app` | `clear` | Clears installed apps. For additional details, refer to app (p. 213). |
| `app-name` | `app`, `appctl`, `third-party-app` | Third party app name. |
| `applicable-eqpt` | `third-party-fw` | List of resources that this firmware can be applied to |
| `applicable-resource-type` | `gapt` | The managed resource type(s) that are applicable for this particular advanced parameter. |
| `applicable-tom` | `apply-template` | Applicable TOMS |
| `application` | `golden-carrier-mode`, `supported-carrier-mode` | The optical transport application ID this mode is optimized for. |
| `application-description-h` | `gadt` | Detailed description of application ID |
| `application-description-p` | `gadt` | Detailed description of application ID |
| `application-description-s` | `gadt` | Detailed description of application ID |
| `application-description-u` | `gadt` | Detailed description of application ID |
| `arc-behavior` | `alarm-control` | System wide alarm-reporting-control (ARC) behavior switch.<br>• clear-alarms: when ARC is set to 'inhibit', clears current alarms.<br>• leave-alarms: when ARC i |
| `arguments` | `run` | Optional arguments to the script. |
| `ase-activation-state` | `nmc` | Indicates the state of ASE-Idler injection on the NMC. This is applicable to the NMC on the dwdm-line side:<br>• not-applicable: The attribute is not applicable |
| `ase-attenuation-compensation-actual` | `nmc` | Displays the relative attenuation introduced by the automatic mux control, relative to the manual configurations (band profile, NMC offset and NMC Tx Profile).  |
| `ase-idler-enable` | `ase-idler-service` | • enabled: ASE idler signal filling on the unused and nmc-failed portions of the band spectrum is enabled.<br>• disabled: ASE idler signal filling on the unused |
| `ase-idler-state` | `ase-idler-service` | • ase-enabled: ASE idler signal filling is complete on the band spectrum.<br>• ase-partially-enabled: ASE idler signal filling is incomplete on the band spectru |
| `ase-insertion-control` | `nmc` | Indicates the criteria for ASE Insertion. This is applicable only when ASE insertion is enabled:<br>• adg-input-delta - ASE insertion criteria is power delta on |
| `ase-insertion-delta` | `nmc` | The amount by which the signal power must drop below the reference power for the NMC to initiate a replacement of the NMC with ASE (NMC-P). The value 99.9 dB me |
| `ase-insertion-enable` | `nmc` | Indicates if the ASE Idler insertion on NMC failure is enabled. |
| `ase-insertion-soak-timer` | `nmc` | The duration, in seconds, for which NMC failure is soaked before proceeding with ASE Idler injection if ASE insertion is enabled. |
| `ase-power-actual-tx` | `nmc` | ASE power of the protection ASE. |
| `ase-source-connected` | `optical-ptp` | Displays whether PTP is connected from an ASE Idler (connection from 'Out') or not:<br>• true: A fiber connection is provisioned between OTSCS and RD ADE port c |
| `assigned-degree` | `oms` | Display degree number when card is added in modules-degree. Only of relevance for node-type(s) OADM, when OMS monitoring-mode is 'intrusive'. This attribute is  |
| `assignment-method` | `dns`, `ntp`, `syslog` | Indicates whether the system contains manual and/or dhcp configured values. The assignment method can be both manual and dhcp. |
| `associated-cdp` | `crl` | The configured CDP which downloaded this CRL, if applicable. |
| `associated-comm-channel` | `interface-neighbor` | Associated communication channel of provisioned neighbor. |
| `associated-secure-entity` | `security-policy-database` | List of all SPD entries associated with this far-end peer for which IKE negotiates security associations (keys). |
| `attenuation-actual` | `ochm`, `spectrum-control` | DGE VOA attenuation of channel. This attribute is applicable to HSC OLS nodes. |
| `attenuation-actual-ase` | `nmc` | Output attenuation of parent NMC. |
| `attenuation-control-mode-rx` | `oms` | Attenuation control mode Rx (input) of the channel applicable to all channels of the OMS. The parameter is applicable to WS04S ADE ports in disaggregated ADG- D |
| `attenuation-control-mode-tx` | `oms` | Attenuation control mode Tx (output) of the channel applicable to all channels of the OMS. |
| `attenuation-control-state-rx` | `oms` | Displays the attenuation control state Rx (input) of the channel:<br>• unknown : default value, awaiting update.<br>• not-applicable : not Applicable, for manua |
| `attenuation-control-state-tx` | `oms` | Displays the attenuation control state Tx (output) of the channel:<br>• unknown : default value, awaiting update.<br>• not-applicable : not Applicable, for manu |
| `attenuation-setting` | `spectrum` | Unique attenuation value for entire spectrum [dB]. Editable if the attenuation-control-mode = "manual" and control-mode = "auto-max-pw". |
| `attenuation-target` | `spectrum-control` | Required attenuation for the spectra, defined by the user. Only possible to configure when Dynamic Gain Equalizer (DGE, (dge-in-use = 'true')), or equivalent, i |
| `attribute` | `access-rule`, `add`, `default`, `expect`, `set`, `show`, `show commit`, `system`, `template` | Name of the attribute to be provided. |
| `attribute-value` | `access-rule` | Attribute value to which this rule applies to. If not provided, it means the rule applies independently on the attribute value. Can only be provided if a single |
| `auth-key` | `add`, `delete`, `set`, `show` | See auth-key (p. 226) for more information. |
| `auth-key-id` | `ntp-server` | Key ID to be used for this server. |
| `auth-passphrase` | `snmpv3-user` | Specifies the SNMPv3 authentication pass phrase. |
| `auth-protocol` | `snmpv3-user` | Specifies the authentication protocol that the SNMPv3 user being created will use. |
| `auth-status` | `ntp-server-status` | Authentication status of NTP server. |
| `authentication-rejects` | `aaa-statistics` | Displays the number of authentication rejects. |
| `authentication-requests` | `aaa-statistics` | Displays the number of authentication requests. i Note: For TACACS+, the default authentication protocol includes both PAP and CHAP. The authentication requests |
| `authentication-scheme` | `ikev2-peer` | IKEv2 authentication mechanism with the peer. |
| `authorization` | `show` | See authorization (p. 228) for more information. |
| `authorization-rejects` | `aaa-statistics` | Displays the number of authorization rejects. |
| `authorization-requests` | `aaa-statistics` | Displays the number of authorization requests. |
| `auto-assigned-degrees` | `equipment-policies` | Enables automatic degree assignment when a card that supports degree(s) is provisioned. |
| `auto-assigned-directions` | `equipment-policies` | Enables/Disables the automatic direction assignment when a card that supports directions is provisioned. By default, it is enabled. |
| `auto-connect` | `dial-out-server` | Defines if the system automatically connects to this server or not. If true, it automatically tries to connect to this dial-out-server. If false, it can still b |
| `auto-delete` | `mc`, `nmc`, `oxcon` | When enabled, the system may auto-delete this MC once it has no associated NMC. When disabled, the MC stays until explicitly deleted. • disabled : The auto-dele |
| `auto-in-service-enabled` | `line-ptp`, `super-channel`, `super-channel-group`, `trib-ptp` | Auto-in-service switch for this facility. |
| `auto-install` | `cert-gen` | Auto-assign certificate to any secure-application without active certificate. |
| `auto-negotiation` | `comm-eth` | Auto negotiation mode. |
| `auto-otdr-state` | `ots-r-auto-otdr` | Displays the status of the automatic OTDR execution for the corresponding OTS-R facility:<br>• not-applicable: Hardware do not support auto otdr.<br>• not-avail |
| `auto-provision-capable` | `supported-slot` | Whether this slot supports card auto-provisioning. |
| `auto-re-enrollment` | `est-ca` | Specifies the number of days before expiration at which re-enrollment will be performed for all leaf certificates issued by this EST CA. This number can also be |
| `auto-recovery-state` | `oxcon` | Only of relevance for SLTE applications. It displays the auto recovery state:<br>• not-applicable - the ase-insertion-enable is disabled, or in terrestrial mode |
| `auto-sensing-state` | `console` | Current state of the auto-sensing mechanism. Only visible if auto-sensing is enabled for this port. i Note: In regard to auto-sensing, the system will auto-dete |
| `automatic-otdr` | `ots-r-auto-otdr` | Enables/disables OTDR based automatic fiber check. On disabling, it terminates an ongoing automatic OTDR test. The attribute persists over warm/ cold restart an |
| `avail-state` | `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `card`, `chassis`, `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet` +38 more | Availability state of an entity. |
| `available-resources` | `line-ptp` | Provide an aggregate view of all available resources on the DSP. |
| `available-space` | `usb` | The current available storage space in the file-system associated with this USB port. Applicable if the type is storage. |
| `available-time-slots` | `eth-zr`, `odu` | A list of time-slots that are available for provisioning new services. |
| `backup-port` | `interface` | Reference to the physical port that supports this interface (if applicable). |
| `backup-status` | `recovery` | Current state of the last backup:<br>• successful - Provisioning service is enabled; backups are being performed successfully<br>• failed - Provisioning service |
| `backup-time` | `database` | Indicates the database snapshot backup time. |
| `band-actual` | `oms` | Actual band negotiated at the link.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-band (4.85 THz).<br>• superC-band -  |
| `band-allowed` | `oms` | The allowed band for Rx / Tx at this OMS facility.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-band (4.85 THz).<br>• |
| `band-required` | `optical-ptp` | Required Transmission Band(s) for the DWDM-line port.<br>• not-applicable - Required transmission band(s) not applicable.<br>• standardC-band - Required transmi |
| `band-target` | `oms` | Derived band at ILA amplifier, or received band from OSC.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-band (4.85 THz |
| `bands-supported` | `adg`, `degree`, `optical-ptp` | List of bands supported by an ADG, with dependence on supported cards.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-b |
| `bands-supported-link` | `oms` | The evaluated link capability, based on OSC information.<br>• not-applicable - Not applicable for non DWDM-line OMS.<br>• standardC-band-only - Only standard C- |
| `bandwidth` | `comm-channel` | Indicates the channel's bandwidth/ capacity. This is system determined based on the underlying facilities that support this control channel. |
| `baud-rate` | `console`, `golden-carrier-mode`, `optical-carrier`, `supported-carrier-mode` | The baud rate of console port that is supported by the system (baud). In auto-sensing-mode, the system will auto-detect the baud-rate based on 'ENTER' presses o |
| `being-deleted` | `ISK` | If the system is in the process of deleting this ISK, it is set to true. Otherwise, it is set to false. This is set to true once the system gets an ISK delete c |
| `bgp-instance` | `add`, `delete`, `show` | See bgp-instance (p. 236) for more information. |
| `bgp-neighbor` | `add`, `delete`, `show` | See bgp-neighbor (p. 238) for more information. |
| `bichm` | `optical-carrier` | The BICHM (bit interleaved coded hybrid modulation) incremental step in 1/128 bits/symbol added to base modulation bits/symbol for the hybrid modes modulation-f |
| `bridge-name` | `L2-bridge` | The name of the bridge. |
| `bu-segment-index` | `submarine-link` | Defines the index of the segment location associated to the BU. Defines how many segments away the branching unit is located from the branch node. This is 1 in  |
| `bytes` | `ipsec-sa-re-key` | The rekeying frequency for the IPsec child security association with the far-end peer based on amount of bytes transmitted. |
| `bytes-transferred` | `transfer-status` | Bytes that have been transferred so far. |
| `cable-id-control` | `equipment-policies` | The attribute enables/disable the CableID verification function. The default value depends on the NE l0-mode-op value. A user can manually configure the policy  |
| `cable-id-state` | `cable-id-status` | Display the cable-id state:<br>• idle - cable-id verification is not running.<br>• running-incl-switching - cable-id verification is running for both active and |
| `calibrated-delta-pointloss` | `raman-calibration` | The attribute represents the suggested delta-pointloss at the end of each iteration of the automatic Raman gain calibration (in dB). The value not-available ind |
| `calibration-state` | `raman-calibration` | Displays the state of the automatic Raman gain calibration process:<br>• not-available: Raman calibration has not been triggered or no prior calibration has occ |
| `cancel` | `commit` | Command parameter for canceling the commit. |
| `candidate` | `diff`, `validate` | Target to be compared |
| `candidate-subtypes` | `golden-carrier-mode` | Subtypes for which this carrier mode has candidate status. |
| `capabilities` | `show` | See capabilities (p. 264) for more information. |
| `capacity` | `golden-carrier-mode`, `interlaken`, `optical-carrier`, `supported-carrier-mode` | The net capacity of the optical carrier. |
| `card` | `add`, `delete`, `set`, `show` | Card base object. This object has parameters that are common to all existing card types (controller, fan, tom etc). See card (p. 265) for more information. |
| `card name` | `property` | The name of the card the property applies to. |
| `card-height` | `supported-card` | Card height in RUs (Rack Units). |
| `card-mode` | `card` | The configured card-mode, identifies specific card functionality. • For BAXOFP2, the supported card-mode strings are: ▪ drop (default) - only allowed when BAX i |
| `card-name` | `advanced-parameter`, `comm-eth`, `current-fw`, `inventory`, `port`, `serdes`, `slot`, `sub-component`, `tom`, `usb` | The name of the card supporting the advanced parameter. |
| `card-type` | `gapt`, `golden-advanced-parameter`, `golden-carrier-mode`, `subtype-constraint`, `supported-card`, `supported-port`, `supported-power-profile`, `supported-slot`, `supported-tom`, `supported-tom-power` | Card type name. |
| `card-type-a` | `cable-id-path` | Displays the card type of the end A of the port pair:<br>• RD20TM<br>• CAD10A The card-type-A is a CableId capable sled. i Note: In R8.0.1, the sleds supported  |
| `card-type-z` | `cable-id-path` | Displays the card type of the end Z of the port pair:<br>• RD20TM<br>• CAD10A The card-type-Z is a CableId capable sled. i Note: In R8.0.1, the sleds supported  |
| `card-width` | `supported-card` | Number of slots this card occupies. It is not-applicable for RU equipment:<br>• na - Not Applicable.<br>• single-slot - single slot width.<br>• double-slot - do |
| `carrier-mode` | `golden-carrier-mode`, `supported-carrier-mode` | Specifies the line mode of the optical carrier. The value is specified as a tuple which contains the line capacity, client mode, baud rate, application ID and S |
| `carrier-neighbor` | `show` | See carrier-neighbor (p. 277) for more information. |
| `carrier-type` | `optical-carrier` | The type of the carrier. |
| `carriers` | `dsc-group`, `eth-zr`, `flexo-group`, `super-channel` | The carrier associated to this facility. Possible values can be any card/ resources/supported-carriers. |
| `category` | `card`, `supported-card` | card category |
| `cd-compensation-mode` | `optical-carrier` | Chromatic dispersion compensation value source mode. |
| `cd-compensation-value` | `optical-carrier` | Manual chromatic dispersion compensation value. |
| `cd-range-high` | `optical-carrier` | High value of chromatic dispersion search range. i Note: This parameter is not configurable for the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q- |
| `cd-range-low` | `optical-carrier` | Low value of chromatic dispersion search range. i Note: This parameter is not configurable for the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-D |
| `cdp` | `add`, `delete`, `set`, `show` | CRL Distribution Point (CDP) for automatic download and periodic refresh of a specified CRL. See cdp (p. 279) for more information. |
| `center-freq-granularity` | `degree` | Granularity of allowed center frequencies. The base frequency for this computation is 193.1 THz (G.694.1). |
| `center-frequency` | `mc`, `nmc`, `spectrum-control`, `spectrum-monitoring` | Center Frequency of the MC, determined by the system (in MHz). |
| `cert-expiring-warning` | `security-policies` | Specifies the threshold for raising the CERTIFICATE- EXPIRING alarm, that can be displayed either as days before expiration or as a percentage of the certificat |
| `certificate` | `clear`, `show` | Clears installed x509 certificates. For additional details, refer to certificate (p. 287). |
| `certificate = &lt;value&gt;` | `display-cert` | The certificate to display. |
| `certificate-bytes` | `local-certificate`, `peer-certificate`, `trusted-certificate` | The number of bytes. A custom type that encodes the entire X.509v3 certificate as string in PEM (base64 encoding) format: -----BEGIN CERTIFICATE----- ...base64  |
| `certificate-name` | `cert-gen`, `csr-gen`, `download`, `est`, `upload` | The name of the certificate. |
| `channel group` | `super-channel-group` | The name of the channel group |
| `chassis` | `add`, `delete`, `set`, `show` | See chassis (p. 291) for more information. |
| `chassis-assignment-mode` | `equipment-policies` | Determines if the chassis ID assignment is done manually or automatically. Manual mode - where sub-chassis ID is assigned either via user configuration or ZTP m |
| `chassis-id` | `lldp-local-info`, `lldp-neighbor` | This attribute identifies the chassis component withing the LLDP remote system. This value needs to be interpreted according with the associated chassis-id-subt |
| `chassis-id-subtype` | `lldp-local-info`, `lldp-neighbor` | This attribute describes the format of the chassis-id string. chassis-component - Represents a chassis identifier based on the value of entPhysicalAlias object  |
| `chassis-location` | `chassis` | User-defined location. |
| `chassis-name` | `L2-bridge`, `card`, `current-fw`, `inventory`, `slot`, `vrf` | Chassis where this card is located. |
| `chassis-role` | `chassis` | Identifies the role of the chassis in a multi-chassis NE. |
| `chassis-serial-number` | `unprovisioned-inventory` | The residing chassis serial number |
| `chassis-type` | `supported-chassis`, `supported-slot` | Chassis type name. |
| `circuit-id` | `ethernet`, `fc`, `oc`, `otu`, `oxcon`, `stm` | System configured circuit id. |
| `circuit-id-suffix` | `xcon` | User-configured circuit ID suffix. |
| `class` | `odu` | This attribute indicates the class/category of the ODUCn/ ODUk entity. 'High order' refers to the top-most ODUCn/ ODUk entity that is created by the system. All |
| `clear-target` | `crl` | The CRL to be cleared. Could be:<br>• single-crl : Remove the CRL specified in &lt;crl-name&gt; (default). For 'single-crl', the &lt;crl-name&gt; is mandatory a |
| `clear-type` | `database` | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory default |
| `clei` | `inventory`, `unprovisioned-inventory` | Common Language Equipment Identifier. |
| `cli` | `protocols`, `set`, `show` | Set of attributes of the Command Line Interface (CLI) management protocol. See cli (p. 310) for more information. |
| `cli-columns` | `cli-session-config` | Configurable number of columns to be used for display. |
| `cli-lines` | `cli-session-config` | Configurable number of rows to be used for display before pausing the output. After pausing, pressing [SPACEBAR] will resume display. |
| `cli-session-config` | `set`, `show` | Set of attributes of the Command Line Interface (CLI) session. See cli-session-config (p. 313) for more information. |
| `client-defect-indicator` | `odu` | Indicates current defect status on client side. |
| `client-mode` | `golden-carrier-mode`, `supported-carrier-mode` | This indicates digital client modes of the signal that is mapped into, and transported by the carriers within this superchannel. |
| `client-side-olos-trigger` | `protection-group` | Considers a local client-side RX OLOS defect as a trigger for switch-over. |
| `client-side-sd-trigger` | `protection-group` | Considers a local client-side RX SD defect as a trigger for switch-over. |
| `client-signal-type` | `odu` | Client signal type for ODUflex CBR client signals. It is used for rate matching and bandwidth validation in the ODU cross connection. This parameter applies to  |
| `client-type` | `ethernet` | The protocol type of the Ethernet client. |
| `clli` | `ne` | Common Language Location Identifier (CLLI) is a 20-character standardized geographic identifier that uniquely identifies the functional category of the equipmen |
| `clock` | `set`, `show` | Set of attributes of the system's clock. See clock (p. 315) for more information. |
| `cmd` | `gshell` | Command to execute inside the Guest Container |
| `comm-channel` | `add`, `delete`, `set`, `show` | See comm-channel (p. 320) for more information. |
| `comm-eth` | `set`, `show` | Set of attributes of the communication Ethernet port. See comm-eth (p. 327) for more information. |
| `comm-eth-location` | `equipment-policies` | Physical location of the communication Ethernet ports. For 1830 GX G31 and 1830 GX G32 chassis, the following values are allowed:<br>• prefer-dcn-in-back - The  |
| `command` | `appctl`, `convert`, `scheduled-task`, `task`, `validate` | CLI command; should be enclosed in quotes; if multiple commands are to be converted, they should be separated by semi-colon (;) |
| `command flag` | `super-channel-group`, `trib-ptp` | -m |
| `command flags` | `diff` | -t Display the diff data in a table (side-by-side diff style) -c Display the diff data as CLI commands |
| `command-type` | `scheduled-task`, `task` | Type of configured command. |
| `commissioning-snr-margin` | `submarine-link` | SNR margin at the time of commissioning. |
| `commit` | `diff`, `rollback` | This command displays a list of differences between commit &lt;id&gt; and current configuration. It also presents a difference of the changes between these two  |
| `commit-tracking` | `system-policies` | Enables the commit-repository feature. With this feature enabled, all configuration changes done via running or candidate datastores are stored as commit-record |
| `common-name` | `cert-gen` | IP or host name to identify the server. |
| `common-password` | `aaa-server` | Password used for RADIUS authorization after SSH public key authentication. If blank, username is reused as password for RADIUS authorization. |
| `community-string` | `snmp-community` | The community string. |
| `community-string-access` | `snmp-community` | SNMP access right of this community string. |
| `compare-op` | `log-console-facility-filter`, `log-file-facility-filter`, `log-server-facility-filter` | Describes the option to specify how the severity comparison is performed. |
| `compatibility-id` | `golden-carrier-mode`, `supported-carrier-mode` | Identifies the compatible carrier modes that can be applied simultaneously |
| `condition` | `ntp-server-status`, `template` | Represents the condition to apply on the template (e.g. service-type=OTU4)- optional |
| `confidentiality-offset` | `mka-policy` | The confidentiality offset specifies a number of octets in an Ethernet frame that are sent in unencrypted plain-text |
| `config` | `show` | System's configuration. See config (p. 334) for more information. |
| `configuration-impact` | `golden-advanced-parameter` | Identifies the configuration steps to apply the change. This parameter is read-only. |
| `configuration-mode` | `supported-port`, `supported-slot` | Configuration mode for the cards in this slot (or toms in this port): • system-configured - system automatically configures the card in slot, and user cannot ma |
| `configured-ambient-temperature` | `chassis` | Configured ambient temperature for the chassis, used to compute the FRU's power consumption. |
| `configured-node-name` | `inci-neighbor` | "User provisioned name of remote NE. Used to compare against the discovered-node-name. |
| `configured-pump-power` | `pump-power` | The pump power configured in the hardware in dBm units. Value can be derived automatically, if control-mode is auto, or otherwise via the target-pump-power. |
| `configured-spectrum` | `oms` | Applicable to SLTE deployments. It is a list of lower and upper frequency values, in MHz, of the usable spectrum configured for SLTE deployments. In case of an  |
| `confirm-timeout` | `commit` | This parameter can be provided in this case in seconds, defining how long the commit will be pending before rollback. The default rollback time is 10 minutes. |
| `confirmed` | `commit` | Command parameter for initiating a confirmed commit. |
| `connect-retry-interval` | `bgp-neighbor` | Time interval in seconds between attempts to establish a session with the peer. |
| `connected` | `ocm-channel` | Yields 'true' if the channel is configured (involved in an oxcon). |
| `connected-amp-edfa-optimum-gain` | `amplifier-raman` | Connected EDFA Optimum Gain. Connected EDFA Optimum Gain 0 indicates that the optimum gain is not known, in case of disaggregated Raman. The attribute is only o |
| `connected-amplifier` | `amplifier-raman` | Connected Amplifier. The system reports the degree that corresponds to the amplifier where Raman is fiber-connected to.<br>• connected-amplifier indicates the d |
| `connected-reference` | `ots-r` | Connected Reference. Indicates the degree the Raman is connected to. In ILA node-type(s), the direction the Raman is connected to (1 means direction 1-2, 2 mean |
| `connected-to` | `port` | i Note: The setting of this attribute is optional and, if using TNMS, it is not recommended to be set. Indicate neighbour port entity to which the current port  |
| `connection-failures` | `aaa-statistics` | Displays the number of connection failures, which include failures due to unavailable servers and timeouts. |
| `connection-ports` | `show` | Connection ports on a given degree. See connection-ports (p. 339) for more information. |
| `connection-state` | `dial-out-server` | Connection state to the dial-out-server. |
| `connectivity-association-key` | `macsec-mka` | Pre-shared Connectivity Association Key |
| `connectivity-association-key-name` | `macsec-mka` | Pre-shared Connectivity Association Key Name |
| `console` | `set`, `show` | Parameters associated with this card's serial console port. See console (p. 341) for more information. |
| `console-port-support` | `supported-card` | Whether this card-type supports a serial console port, with or without auto-sensing capabilities:<br>• no - card-type does not have a serial console port.<br>•  |
| `console-user-enabled` | `security-policies` | A switch to enable/disable the console-user. The console-user account is an emergency account that is only usable through the serial console. Disabling this acc |
| `console-user-password` | `security-policies` | The password of the console-user. The minimum length of the console-user is 1 character. i Note: It is strongly recommended to set a password for the console-us |
| `contact` | `ne` | The administrator contact information for the system. |
| `container-name` | `sw-container` | A unique Id for each container. |
| `contention-check-status` | `super-channel` | Contention Check state, set via DNA in openwave mode. Only applicable if openwave-contention-check is enabled at super-channel-group level. |
| `control-mode` | `amplifier`, `amplifier-raman` | Defines whether amplifier gain is automatically set by system or manually. The attribute auto-max-pw is the auto mode targeting maximum output power. |
| `control-speed-factor` | `amplifier`, `oms` | Control speed factor for the DGE power control algorithm. The value is conveyed to system power control on the EDFA object. |
| `control-state` | `amplifier`, `amplifier-raman` | Indicates the current state of the power control adjustment for the preamplifier:<br>• unknown : default value, awaiting update.<br>• not-applicable : if in man |
| `controller-card` | `show` | See controller-card (p. 344) for more information. |
| `controller-redundancy-supported` | `supported-chassis` | Whether this chassis supports controller redundancy or not. |
| `cookie-timeout` | `restconf` | Timeout of a cookie based RESTCONF session. The cookie expiration date is reset every time there is activity on the session. |
| `corrective-action` | `alarm`, `alarm-inventory` | System provided information on how to correct the situation that triggered this alarm. |
| `cpu` | `ISK`, `KRK` | Identifier for member CPUs on cards starts at 0. |
| `cpu-usage` | `sw-container`, `sw-service` | Current usage of CPU by the container, in percentage. In a multi-core system, this indicates the overall usage relative to all cores. |
| `crc` | `third-party-fw` | Cyclic redundancy check (CRC) of the firmware image, used to validate the file when present. |
| `created-time` | `session` | The timestamp the user has created for this session. |
| `criteria` | `cert-to-name` | Defines the specific attributes and conditions required for the rule to be invoked. |
| `crl` | `clear`, `show` | Clears one or more installed Certificate Revocation Lists (CRLs) from the system. For additional details, refer to crl (p. 349). |
| `crl-based-revocation` | `security-policies` | This policy allows to enable/disable CRL-based certificate revocation. |
| `crl-download-timeout` | `security-policies` | Specifies the maximum time to wait (in seconds) for automatic CRL downloads. Note: This timeout does not apply to manual CRL downloads. |
| `crl-name` | `crl` | The name of the CRL to be cleared. Use &lt;tab&gt; to obtain the list of CRL names that can be cleared. Otherwise, &lt;crl-name&gt; should be omitted. |
| `crl-number` | `crl` | Increases the sequence number for a given CRL scope and CRL issuer. |
| `csp-symmetrical-key` | `security-policies` | Critical Security Parameters symmetrical key. |
| `current-advanced-parameter` | `show` | See current-advanced-parameter (p. 359) for more information. |
| `current-advanced-parameter-name` | `current-advanced-parameter` | The name of the advanced parameter. |
| `current-alarms` | `show` | List of currently raised alarms. See current-alarms (p. 361) for more information. |
| `current-equipment` | `slot` | Name of the equipment that is currently required in this slot. |
| `current-fw` | `show` | List of current firmware available in the cards. See current-fw (p. 362) for more information. |
| `current-state` | `cable-id-path` | State of the CableID port-pair verification:<br>• idle (default value): CableID-based verification is not running.<br>• running-incl-switching: CableID-based ve |
| `current-subscription` | `show` | See current-subscription (p. 364) for more information. |
| `current-time` | `clock` | Indicates the current Date and Time of this NE. |
| `custom-tlv` | `show` | See custom-tlv (p. 366) for more information. |
| `data-model` | `set`, `show` | Available YANG Data models for loading/unloading. See data-model (p. 367) for more information. |
| `data-model-openconfig` | `protocols` | Show data-model-openconfig protocols |
| `data-path-encryption` | `show` | Top-level container for all data path encryption services and entities. To view all data path encryption use the command show data-path-encryption. See data-pat |
| `data-path-encryption-san-ike-id-match` | `ikev2` | A global, L1 encryption-specific policy that indicates whether the NE must validate Certificate subject alternate name to match the IKE ID (OPT-IN) or not (OPT- |
| `data-rate` | `tom-type` | The approximate data-rate for this TOM type. |
| `data-supervision` | `pm-control`, `pm-control-entry` | Real-time data supervision for this resource. |
| `data-type` | `pm` | Type of PM data to clear:<br>• current<br>• history<br>• real-time |
| `database` | `activate`, `clear`, `show` | Set NE database to default and reboots the system. For additional details, refer to database (p. 369). |
| `database-product` | `database` | Indicates the network element family this database belongs to. |
| `database-state` | `database` | Indicates the state of the database. |
| `database-type` | `database` | The database type of database identifier. |
| `database-vendor` | `database` | Vendor information of the database. |
| `database-version` | `database` | Indicates the database version. |
| `days` | `cert-gen` | Number of days a certificate is valid for. |
| `db-action` | `activate`, `download`, `prepare-upgrade` | Specifies the expected database operation:<br>• empty-db: Activate the software image with empty database.<br>• upgrade-db: Activate the software image with upg |
| `db-entry-name` | `named-value-set` | Name of the data base entry. |
| `db-instance` | `activate`, `activate-snapshot`, `take-snapshot` | The database snapshot to be activated. |
| `db-passphrase` | `activate`, `download`, `security-policies`, `take-snapshot` | Passphrase used for encrypting and decrypting DB snapshots. For each command associated with DB snapshots (backup, restore, etc), this db-passphrase will be use |
| `debug-entity` | `upload` | Targets a specific entity in the system for having its Logs to be collected. Can be a chassis or a card. |
| `debug-log-optional-content` | `transfer` | List of keywords associated with optional content to be selected for debug-log upload. |
| `default` | `supported-power-profile` | Whether is the default value or not. |
| `default-card` | `supported-slot` | Card that exists in this slot by default. |
| `default-card-mode` | `supported-card` | The default card-mode, for cards whose supported-card-mode is not empty. Only relevant if the card has the concept of card-mode. |
| `default-console-baud-rate` | `supported-card` | Defines the default baud-rate for cards with fixed baud-rate. |
| `default-data-supervision` | `pm-profile-entry` | For newly created resources of this type, whether they have PM data supervision automatically enabled or not. |
| `default-high-threshold` | `pm-threshold-profile` | System defined default value for high threshold for this parameter. |
| `default-interactive-mode` | `cli` | Defines whether CLI sessions have interactive-mode enabled or disabled by default. Note: changing this parameter will not affect existing CLI sessions, only new |
| `default-low-threshold` | `pm-threshold-profile` | System defined default value for low threshold for this parameter. |
| `default-phy-mode` | `supported-tom` | The phy-mode that is used by default in this TOM for this card. |
| `default-severity` | `alarm-inventory` | List of possible default severities for this alarm type. The same alarm may have different default severities depending of the resource-type it applies to. |
| `default-subtype` | `supported-chassis` | Default subtype supported by chassis. |
| `default-tca-supervision` | `pm-profile-entry` | For newly created resources of this type, whether they have PM threshold crossing supervision automatically enabled or not. |
| `default-tom` | `supported-port` | Defines the TOM that exists in this port by default (if any). |
| `default-user-group` | `security-policies` | Default roles for users access. |
| `degrade-interval` | `odu-diagnostics`, `otu-diagnostics` | The consecutive number of 1s intervals with the number of detected block errors exceeding the block error threshold for each of those seconds for the purposes o |
| `degrade-threshold` | `odu-diagnostics`, `otu-diagnostics` | The threshold in percentage of block errors versus total blocks at which a degrade-interval number of seconds will be considered degraded for the purposes of SD |
| `degree` | `add`, `delete`, `set`, `show` | See degree (p. 380) for more information. |
| `degree-expected-rx-power` | `submarine-link` | Indicates the expected receive power at the degree. It is for the total expected receive power at the degree and not for the individual submarine links. |
| `degree-number` | `connection-ports`, `degree`, `modules-degree` | Degree number should be greater than zero and not greater than max-degrees. |
| `degree-target-tx-power` | `submarine-link` | Indicates the target transmit power for the degree. It is for the launch power at the ROADM into the primary fiber link. This is a mandatory parameter for the l |
| `delay` | `ntp-server-status` | Delay along path to the server in milliseconds. |
| `delay-measurement-enable` | `odu` | The enable switching of delay-measurement function, when applicable. |
| `delta-pointloss` | `ots-r` | Delta Pointloss (Rx). Additional attenuation that can be determined after turning up pumps. This is the fiber contribution for the pointloss: to be fine tuned i |
| `denied-data-writes` | `authorization` | Number of times since the system last restarted that a Write operation request was denied. |
| `denied-notifications` | `authorization` | Number of times since the system last restarted that a notification was dropped for a subscription because access to the event type was denied. |
| `denied-operations` | `authorization` | Number of times since the system last restarted that an Exec request was denied. |
| `depth` | `supported-chassis` | Chassis depth in millimeters. |
| `description` | `L2-bridge`, `access-rule`, `access-rule-list`, `bgp-instance`, `bgp-neighbor`, `data-model`, `database`, `extended-config`, `golden-advanced-parameter`, `ipsec-spd-entry`, `manifest`, `ospf-instance` +11 more | Database description. |
| `destination` | `download`, `ikev2-peer`, `ip-monitoring`, `oxcon`, `upload`, `xcon` | The destination end-point required for OXcon creation. |
| `destination-facility-override` | `log-server` | Flag indicating whether the destination facility override is enabled. When not disabled, specifies the facility used in messages delivered to the remote server. |
| `destination-ip-address` | `ace` | Specifies the destination IP of this filter. |
| `destination-lower-port` | `ace` | The lower bound on the destination Layer 4 TCP/UDP port number |
| `destination-prefix` | `next-hop`, `route` | IP destination prefix. |
| `destination-upper-port` | `ace` | The upper bound on the destination Layer 4 TCP/UDP port number |
| `details` | `transfer-status`, `upgrade-status` | Details of transfer phase |
| `detection-timestamp` | `unprovisioned-inventory` | Timestamp with the last time the unprovisioned equipment was detected by the Node Controller. |
| `dgd-high-threshold` | `dsc-group`, `optical-carrier` | The threshold to raise the DGD- OORH alarm (in ps). |
| `dge-in-use` | `spectrum` | Indicates if a DGE is in used for the respective DWDM line. It reports true if the corresponding OMS monitoring-mode is ila-with-equalization. |
| `dh-group` | `additional-key-exchange`, `ike-sa-proposal`, `ipsec-sa-proposal`, `secure-entity-sa-proposal` | A list of IKE SA Diffie-Hellman groups + advertised to the far-end IKE peer. |
| `dhcp-relay` | `set`, `show` | See dhcp-relay (p. 391) for more information. |
| `dhcp-relay-enabled` | `if-dhcp-relay` | Enables dhcp-relay function on this interface. Obeys global dhcp-relay settings. |
| `dial-out-server` | `add`, `delete`, `set`, `show` | See dial-out-server (p. 393) for more information. |
| `dial-out-server-name` | `call-home`, `session` | Name of the dial-out-server associated with this session. |
| `direction` | `ace`, `add`, `alarm`, `alarm-severity-entry`, `custom-tlv`, `delete`, `get-conditions`, `golden-advanced-parameter`, `lldp-neighbor`, `lldp-port-statistics`, `management-address`, `ochm` +15 more | See direction (p. 399) for more information. |
| `direction-card` | `direction` | The 'direction-card' is set by the system, based on the 'direction-port' that the user has configured. A port is hosted in a card at IOA, the system fills up th |
| `direction-number` | `direction` | The 'direction-number' is either 1 or 2. It is set by the system upon the 'direction-port' configuration. This value matches '1' when the 'direction-port' selec |
| `direction-port` | `direction` | Instance of the card's port hosting this direction (index). The 'direction-port' is the dwdm-line1 or dwdm-line2 port instance of the ILAx that the user has ass |
| `discovered-node-id` | `inci-neighbor` | Node ID of remote node as received from remote node. |
| `discovered-node-name` | `inci-neighbor` | Name of remote NE as sent by the remote NE. |
| `discovery-cycle-time` | `interface-neighbor` | Periodicity at which sndp discover messages will be sent. |
| `discovery-enabled` | `interface-neighbor` | It is a switch to enable or disable discovery on the local interface. |
| `discovery-timeout` | `interface-neighbor` | Time after which discovery is considered as failed; when this timeout occurs, neighbor-adjacency state will transition to blackout. |
| `display-name` | `user` | The display name for this user. |
| `display-timestamp` | `cli-session-config` | Determines if the current timestamp is printed on every CLI command. |
| `display-type` | `display-cert` | Defines the requested type of display operation. |
| `distance` | `ipv4-static-route`, `ipv6-static-route` | Distance to the next hop. |
| `dns` | `show` | See dns (p. 407) for more information. |
| `dns-server` | `add`, `delete`, `set`, `show` | The address of the DNS server. See dns-server (p. 409) for more information. |
| `downloaded-from-uri` | `crl` | The HTTP URI from which this CRL was auto-downloaded. Not applicable to manually downloaded CRLs. |
| `downloaded-image` | `manifest`, `show` | Downloaded software image files. See downloaded-image (p. 425) for more information. |
| `downloaded-on` | `manifest` | Manifest file downloaded timestamp. |
| `downloads` | `show`, `sw-management` | Downloaded manifest files and associated image files. The list can be retrieved by using show downloads. See downloads (p. 426) for more information. |
| `dpd-delay` | `ikev2-peer` | The interval to check the liveness of a peer actively. Only of relevance for scope management IPsec and name not global. |
| `drop-rate` | `ip-monitoring` | The accepted drop rate of ping in 10% steps. |
| `dsc` | `show` | See dsc (p. 427) for more information. |
| `dsc-group` | `add`, `delete`, `show` | See dsc-group (p. 431) for more information. |
| `dst-active` | `clock` | Whether daylight saving is active. |
| `dst-card-name` | `external-fiber-connection`, `submarine-link` | Destination card identification. |
| `dst-chassis` | `nct-connection` | The identifier of the chassis where the destination port is located. If it is a commissioned chassis, it will be the AID of the chassis. If it is an unprovision |
| `dst-chassis-state` | `nct-connection` | The state of the dst-chassis |
| `dst-node-id` | `external-fiber-connection`, `submarine-link` | Destination node-id. Should be logically the same as 'ne-name', although there is no SYSTEM business logic to correct this. |
| `dst-port` | `fiber-connection`, `nct-connection` | Destination Port instance. |
| `dst-port-name` | `external-fiber-connection`, `submarine-link` | Destination port identification. |
| `dst-time-slots` | `xcon` | Time-slots allocated to the destination looduj in this xcon. Not applicable if destination facility is not an ODU facility. Value can be:<br>• omitted/empty - i |
| `duplex-mode` | `comm-eth` | Duplex mode. It is only valid if auto-negotiation is disabled. unknown - Link is currently disconnected or initializing. full - Full duplex. half - Half duplex. |
| `dust-filter-replacement` | `supported-chassis` | Chassis characteristics related with dust filter (and its replacement):<br>• not-applicable - No dust filter.<br>• optional-dust-filter - Optional dust-filter a |
| `dynamic-ts` | `ipsec-spd-entry` | Indicates whether dynamic traffic selector is enabled in this SPD entry. |
| `effective-date` | `crl` | The issue date of the CRL. |
| `enable` | `ospf-interface`, `third-party-app` | Enable/disable OSPF protocol on the interface. |
| `enable-advanced-parameters` | `optical-carrier` | Controls enabling/disabling of configuring advanced parameters for this object. |
| `enable-dcn-interworking` | `ots` | This attribute is visible only if osc-compatibility is set to osc-7100. Enables/Disables the DCN interworking with 7100 over the OC3 OSC:<br>• false - disables  |
| `enable-serdes` | `tom` | Controls enabling/disabling of configuring TOM SerDes. |
| `enabled` | `aaa-server`, `bgp-neighbor`, `cdp`, `cert-to-name`, `cli`, `data-model`, `dns`, `est-server`, `grpc`, `high-speed-monitoring`, `http-file-server`, `ip-monitoring` +15 more | Enables/disables the CLI management protocol. It is not possible to disable CLI from within a CLI session. |
| `enabled-capabilities` | `lldp-local-info`, `lldp-neighbor` | This attribute describes the remote system enabled capabilities. |
| `encoding` | `current-subscription` | Specifies the data encoding scheme to be used for data sent to and from the target device. The encoding may be specified for all data, or optionally on a per-RP |
| `encryption-algorithm` | `add`, `db-protection-scheme`, `delete`, `secure-entity-sa-proposal`, `show` | See encryption-algorithm (p. 437) for more information. |
| `encryption-key-length` | `secure-entity-sa-proposal` | The secure entity SA encryption algorithm key length. |
| `end-time` | `scheduled-task`, `task`, `upgrade-status` | Timestamp to stop the periodic task. Not relevant for single-occurrence tasks. |
| `endpoint1` | `nw-xconnect` | The first endpoint of a networking cross-connection. It is mandatory to set the parameter upon nw-xconnect creation. If xcon-type = L1-ETH-to-GCC0 or L1-ETH-TO- |
| `endpoint2` | `nw-xconnect` | The second endpoint of a networking cross-connection. If xcon-type = L1-ETH-to-GCC0 or L1-GCC0-to-GCC0, an instance-identifier to a GCC0 comm-channel MO. If xco |
| `enforce-password-history-check` | `security-policies` | If enabled, ensures that a new password being set cannot match any of the previous 5 password for the user. If disabled, password repetition is allowed. Once en |
| `engine-boot-count` | `snmp` | SNMP engine boot count. Counts how many times the engine has restarted. |
| `entity` | `activate`, `add`, `calibrate`, `delete`, `pm-control`, `profile-control`, `terminate` | Description |
| `entity id` | `add`, `default`, `delete`, `set` | Instance ID of the entity to be created. |
| `entity type` | `show` | Description |
| `entity-id` | `config`, `show`, `show commit`, `update` | Instance ID of the entity where to perform the show. |
| `entity-type` | `config` | Entity type to retrieve the configuration. |
| `eqpt-fw` | `activate` | Command parameter for activating a new FW image in a given resource. For eqpt-fw specific parameters, refer to Table 81: activate eqpt-fw command parameters (p. |
| `equipment` | `show`, `status`, `sw-container`, `sw-service` | Container for all equipment related resources. The list can be retrieved by using show equipment. See equipment (p. 439) for more information. |
| `equipment option` | `equipment` | The equipment to be viewed. |
| `equipment-capabilities` | `show` | Top level container for all equipment capabilities. The list can be retrieved by using show equipment-capabilities. |
| `equipment-discovery-ready` | `chassis`, `ne` | Represents the equipment discovery state for the current chassis. It remains as 'false' until all equipment was discovered during startup. Equipment added after |
| `equipment-policies` | `set`, `show` | Container with all existing equipment policies. See equipment-policies (p. 441) for more information. |
| `equipment-type` | `fru-info`, `manifest`, `packaged-fw` | Type of the equipment. |
| `error-frames` | `lldp-port-statistics` | A count of all LLDPDUs received at the port with one or more detectable errors. |
| `esn` | `ipsec-sa-proposal`, `ipsec-spd-entry` | Extended Sequence Number (ESN) support. |
| `est-ca` | `est`, `est-ca` | Represents a Certificate Authority (CA) set for EST. |
| `est-server` | `est-server` | Configures the Enrollment over Secure Transport (EST) server settings. |
| `eth-zr` | `set`, `show` | The Ethernet ZR facility. See eth-zr (p. 454) for more information. |
| `ethernet` | `set`, `show` | The Ethernet facility. See ethernet (p. 461) for more information. |
| `exclude-column` | `log` | column to exclude |
| `exec-default` | `authorization` | In case only user configured access-rules are used, this policy defines what is the action to use if a given exec operation does not match any rule. Exec access |
| `expected-dapi` | `odu-diagnostics`, `otu-diagnostics` | The expected DAPI (Destination Access Point Identifier). |
| `expected-fan-type` | `chassis` | Defines what is the expected type of FANs that this chassis will have. It is not possible to configure each FAN slot individually, this needs to be done at the  |
| `expected-fw-version` | `current-fw` | Expected version of the firmware. |
| `expected-mapping-mode` | `ethernet`, `fc`, `oc`, `otu`, `stm` | The expected mapping mode of client port. The possible values are dependent on the HW and configuration |
| `expected-msi` | `odu` | Expected MSI values (up to 80). For format see rx-msi without valid/invalid flag. User-friendly representation of expected-msi-hex. |
| `expected-msi-hex` | `odu` | Expected MSI hex values (up to 80). |
| `expected-operator` | `odu-diagnostics`, `ots-diagnostics`, `otu-diagnostics` | The expected operator specific bytes. The value of this attribute is the expected trail trace identifier of the NE connected on the other end of the fiber. It i |
| `expected-payload-type` | `odu` | Expected payload-type of ODU. |
| `expected-pem-type` | `chassis` | Defines what is the expected type of PEMs that this chassis will have. It is not possible to configure each PEM slot individually, as all PEMs need to be of the |
| `expected-sapi` | `odu-diagnostics`, `otu-diagnostics` | The expected SAPI (Source Access Point Identifier). |
| `expected-serial-number` | `chassis` | Inform the NC the serial number of a sub-chassis. For the main-chassis, the value is auto-filled with its own serial number. |
| `expected-time-slots` | `odu` | Expected TS for the LO-ODU entity. |
| `expected-total-tx-power` | `super-channel` | Theoretical total TX power at Faceplate calculated based on per carrier Target TX power value. |
| `expected-trib-port-number` | `odu` | Expected Tributary Port Number for the LO-ODU entity. |
| `expected-tti` | `oc`, `odu-diagnostics`, `otu-diagnostics`, `stm` | Expected TTI - The TTI this facility expects to receive from the far-end remote facility. |
| `expected-value` | `expect` | The expected value. If -r is provided, may be a regex. |
| `explicit-ca-root` | `est-ca` | Indicates the trusted root certificate for the EST CA. |
| `extended-key-usage` | `csr-gen`, `est`, `local-certificate`, `peer-certificate`, `trusted-certificate` | The Extended Key Usage type(s) for the certificate. |
| `external-attenuation-rx` | `ots`, `ots-r` | External Attenuation, configured by the user. |
| `external-attenuation-rx-measured` | `ots-r-auto-otdr` | Displays the attenuation (point losses) value between the span fiber and DWDM Line-In port of the Raman card, that is measured by the automatic OTDR Raman pre-c |
| `external-attenuation-tx` | `ots` | External padding attenuation at transmitting direction. It is required for tilt control. |
| `external-connectivity` | `port` | Indicates whether the port is intended to be connected to another (external) NE. |
| `external-fiber-connection` | `add`, `delete`, `set`, `show` | External fiber connection connecting two ports of L0 cards in different NEs. See external-fiber-connection (p. 477) for more information. |
| `faceplate-label` | `supported-port` | Label on the hardware faceplate. Identifies the port in the card faceplate. |
| `facilities` | `show` | The top-level facility root node under which all other facilities are present. The list can be retrieved by using show facilities. |
| `facility-los-threshold` | `optical-switch` | Defines the threshold of the facility port, power level below it will lead to loss of signal. |
| `fail-action` | `sw-control-rule` | The action to be taken. • default-action - performs the policy of restarting the service, then rebooting the system if service not recovered.<br>• ignore - spec |
| `failed-logins` | `user` | Number of previous failed logins. Resets to zero upon a successful login. |
| `fan-adjustment-on-altitude` | `supported-chassis` | Whether FAN(s) rotation are automatically adjusting based on the configured altitude. |
| `fast-sop-mode` | `optical-carrier` | Specify if enable fast SOP (state of polarization) change tracking; if enabled, the interface will tolerate very fast SOP and transient. i Note: This parameter  |
| `fc` | `show` | See fc (p. 482) for more information. |
| `fc-type` | `fc` | The type of fc signal. |
| `fcs-length` | `comm-channel` | Specifies whether the Frame Check Sequence (FCS) is a 16-bit or 32-bit value. |
| `fdd-clear-threshold` | `eth-zr` | The threshold for FEC Detected Degrade (FDD) alarm clear. decimal64(9) Unit : Average BER |
| `fdd-monitoring` | `eth-zr` | The configured FEC Detected Degrade (FDD) monitoring mode. |
| `fdd-threshold` | `eth-zr` | The threshold for FEC Detected Degrade (FDD) alarm. It is the number of slots to be supported as times of 100G: rate-class/100. Unit : Average BER |
| `fec-ability` | `ethernet` | Indicates the Ethernet client's capability to support FEC (Forward Error Correction). |
| `fec-degraded-ser-activate-threshold` | `ethernet` | FEC-DEGRADED-SER alarm asserted if average SER, computed over accumulated FEC symbol errors in the monitoring period exceed this threshold. |
| `fec-degraded-ser-deactivate-threshold` | `ethernet` | FEC-DEGRADED-SER alarm cleared if average SER, computed over accumulated FEC symbol errors in the monitoring period is below this threshold. |
| `fec-degraded-ser-monitoring` | `ethernet` | Allows to enable monitoring for FEC-DEGRADED-SER alarm. |
| `fec-degraded-ser-monitoring-period` | `ethernet` | Monitoring period duration over which FEC symbol errors are accumulated for asserting or clearing of FEC- DEGRADED-SER alarm. |
| `fec-generation-mode` | `otu` | The configured FEC generation mode on the OTUk/OTUCn client towards the far-end receiver. |
| `fec-mode` | `ethernet`, `otu` | The configured FEC mode on the Ethernet client. Default is dependent on configured client type. |
| `fec-type` | `eth-zr`, `flexo`, `flexo-group`, `otu` | The FEC type. |
| `fed-clear-threshold` | `eth-zr` | The threshold for FEC Excessive Degrade (FED) alarm clear. Unit : Average BER |
| `fed-monitoring` | `eth-zr` | The configured FEC Detected Degrade (FED) monitoring mode. |
| `fed-threshold` | `eth-zr` | The threshold for FEC Excessive Degrade. Unit : Average BER |
| `fiber-connection` | `add`, `delete`, `set`, `show` | Physical link representation of a connection between two distinct ports (or two distinct sub-ports) in the same NE. See fiber-connection (p. 489) for more infor |
| `fiber-connection-list` | `cable-id-path`, `supporting-fiber-connection` | Displays a list of supporting-fiber-connections. It is displayed when -r flag is used. |
| `fiber-connection-type` | `external-fiber-connection`, `fiber-connection`, `submarine-link` | Type of the fiber connection. It can be one-way (unidirectional) or two-way (bidirectional). |
| `fiber-length` | `submarine-link` | Defines the fiber length, in km, of the associated fiber pair ID. This does not include the length of the branch segments. |
| `fiber-length-derived-rx` | `ots` | Estimated fiber length, calculated from the configured fiber-type and span loss measured via OSC powers.from the span-loss-reference setting: if measured the va |
| `fiber-length-derived-tx` | `ots` | Estimated fiber length, calculated from the configured fiber-type and span loss measured via OSC powers.from the span-loss-reference setting: if measured the va |
| `fiber-length-offset` | `optical-ptp` | Fiber patch cord length between the Raman DWDM port and the base card DWDM line port. |
| `fiber-length-rx` | `ots`, `ots-r` | Receiving fiber length for the receive direction. It is required for tilt control (if tilt-control-mode = auto) and when Raman backward pumping is deployed. |
| `fiber-length-tx` | `ots` | Transmitting fiber length for the transmit direction. It is required for tilt control (if tilt-control-mode = auto). |
| `fiber-pair-id` | `submarine-link` | Defines the fiber pair ID of the fiber in the fiber bundle that is associated with the link. |
| `fiber-spectral-attenuation-tilt-rx` | `ots` | Fiber attenuation tilt per Terahertz (in dB/THz). Required for tilt control (if tilt-control-mode is set to auto-planned). Configuration mode depends on tilt-co |
| `fiber-spectral-attenuation-tilt-tx` | `ots` | Fiber attenuation tilt per Terahertz (in dB/THz). Since different transmission bands are supported, it is simpler to enter this parameter independent of the tra |
| `fiber-type-rx` | `ots` | Fiber Type at OTS receiver. It uniquely identifies the fiber-type ( it allows PCL to know the intercept and slope). Fiber-types value can be:<br>• not-applicabl |
| `fiber-type-tx` | `ots` | Fiber Type at OTS transmitter. It uniquely identifies the fiber-type ( it allows PCL to know the intercept and slope). |
| `field` | `cert-to-name` | Applicable only when Map-Type selected is "extract". Specifies the certificate attribute used to determine the user identity. |
| `file` | `clear` | Removes a particular file from the system. For additional details, refer to file (p. 491). |
| `file-path` | `file`, `file-operation` | Current file path. |
| `file-server` | `add`, `delete`, `download`, `set`, `show`, `upload` | User configurable file-server (e.g SFTP server), to be used by transfer operations (upload/download). See file-server (p. 496) for more information. |
| `file-status` | `third-party-fw` | Firmware file status. |
| `file-type` | `activate` | The type of file to activate. |
| `filename` | `file`, `transfer-status` | The name of the file to be displayed including the path to the file. |
| `filetype` | `download`, `file`, `transfer-status`, `upload` | Predefined file type available for download. |
| `filter` | `delete`, `set`, `show`, `show commit`, `status` | Filter |
| `filter-insertion-date` | `chassis` | Filter insertion date, if applicable. This parameter is not applicable to 1830 GX G31 chassis. |
| `filter-maintenance-interval` | `chassis` | Configuration for the filter replacement. When the configured time interval expires, system reports an alarm indicating that dust filter needs to be replaced. T |
| `fingerprint` | `ssh-host-key` | Fingerprint string as a sequence of pairs of hex digits. SSHv2 public key fingerprint examples for MD5 and SHA256 hash:\n md5sum fingerprint =&gt; b2:9c:cd:30:b |
| `fingerprint-algorithm` | `ssh-host-key` | The type of hash algorithm in use for computing the key fingerprint. |
| `fix-rx-attenuation` | `optical-ptp` | Fixed Attenuator before port Rx. 0 (dB) is equivalent to no fixed attenuator. |
| `fix-tx-attenuation` | `optical-ptp` | Fixed Attenuator after port Tx. 0 (dB) is equivalent to no fixed attenuator. i Note: The parameter fix-tx-attenuation is only visible when ops.port-expansion=y- |
| `flexo` | `set`, `show` | See flexo (p. 501) for more information. |
| `flexo-group` | `add`, `delete`, `set`, `show` | See flexo-group (p. 505) for more information. |
| `flow-control` | `comm-eth` | Specifies the type of flow control to be supported. Applicable when the auto-negotiation is disabled. unknown - Link is currently disconnected or initializing.  |
| `foic-type` | `flexo` | FOICx.k lanes mean using k parallel lanes to carry a FlexO-x interface, where order x signifies the interface rate in units of 100G. A unique FOICx.k identifica |
| `force-password-change` | `user` | Allows administrator to force user to change password on next login. |
| `forced-shutdown` | `amplifier` | For cards with dual-band, one amplifier can be forced to be shutdown by setting this attribute to 'true'. |
| `forward-defect-trigger` | `trib-ptp` | Indicates on the egress, if NE receives a client forward defect (e.g., LF, ODU-AIS) whether to let it flow through towards the line side (network side) or trigg |
| `frequency` | `ipsec-sa-re-key`, `monitored-channel`, `optical-carrier`, `scheduled-task`, `task` | Frequency interval for setting up a periodic scheduled task. If empty (default value), represents a single-occurrence task. |
| `frequency-offset` | `optical-carrier` | A super set range for line and client side carrier, specific sub-range is depend on application. Frequency-offset can be used for bright tuning of the wavelengt |
| `from-adaptation` | `xcon` | Indicate server layer adaptation at client side. |
| `from-commit=&lt;commit-id&gt;` | `configure` | This parameter allows to leverage the to initialize the candidate from the configuration associated with a past commit. This option is only available when the C |
| `from-default` | `configure` | This parameter allows user to start the Candidate configuration from an empty slate, effectively removing all non-default configurations present in the system f |
| `from-script=&lt;script&gt;` | `configure` | This parameter allows users to use a CLI configuration script as source for the Candidate Configuration, effectively replacing the Running Configuration with wh |
| `fru-info` | `show` | See fru-info (p. 509) for more information. |
| `function` | `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `raman-calibration` | Describes the function of the object:<br>• 'pa' for pre-amplifierspa - for pre-amplifier<br>• 'ba' for booster (amplifiers)<br>• 'inline' for both amplifiers of |
| `fw-image-name` | `activate` | The firmware file name. |
| `fw-name` | `current-fw`, `manifest`, `packaged-fw`, `third-party-fw` | Name of the firmware. |
| `fw-status` | `current-fw`, `inventory` | not-applicable - Card doesn't have upgradeable firmware. current - All components have current firmware. not-current - At least one component does not have curr |
| `fw-version` | `current-fw`, `manifest`, `packaged-fw` | Current version of the firmware. |
| `gadt` | `show` | See gadt (p. 510) for more information. |
| `gain-adjustment` | `amplifier` | Applicable for auto control mode. The gain offset is defined by the user. The value is used for adjustment of gain when the amplifier is in automatic control mo |
| `gain-calibration-error` | `raman-calibration` | Represents the residual gain error after each iteration (in dB). The value not-available indicates that is Not-available/ Not specified. |
| `gain-operating` | `amplifier` | Operating gain of the amplifier that is the actually configured gain on the amplifier. When card is plugged out, or EDFA disabled, gain-operating is 0.0. |
| `gain-range-actual` | `amplifier` | The current working gain range. |
| `gain-range-control` | `amplifier` | Control mode for the amplifier gain switch (for amplifiers with multiple gain ranges). In R6.0:<br>• if the control-mode is set to manual, the gain-range-contro |
| `gain-range-max` | `supported-gain-range` | The maximum settable gain-target for this type of range ('standard'/ 'low'/ 'high'). |
| `gain-range-min` | `supported-gain-range` | The minimum settable gain-target for this type of range ('standard'/ 'low'/ 'high'). |
| `gain-range-target` | `amplifier` | Applicable for manual gain-range-control:<br>• standard – single range amplifier working range.<br>• low – the low range for multi working range.<br>• high – th |
| `gain-range-type` | `supported-gain-range` | Type of gain-range |
| `gain-target` | `amplifier` | Applicable for manual control mode. Used for setting the gain to the amplifier for constant-gain mode. |
| `gcmt` | `show` | See gapt (p. 512) for more information. |
| `generic-subtype` | `tom-type` | 3rd party subtype for this TOM. |
| `global-data-supervision` | `pm-profile` | This parameter provides a way to globally enable PM data-supervision without having to toggle it individually: • auto-enabled - Global enabling of PM data-super |
| `global-switch` | `icdp`, `serial-console` | Allow access by serial-console. Note: each console port can override this global behavior. |
| `global-timeout` | `serial-console` | Serial console inactivity timeout. Can be set to zero to disable inactivity timer. |
| `gnmi-get-encoding-granularity` | `grpc` | Allows to configure the granularity of data in gNMI Get responses, when encoded with JSON. • per-path - puts all path data on a Update message.<br>• per-object  |
| `golden-carrier-mode` | `show` | See golden-carrier-mode (p. 521) for more information. |
| `grid-mode` | `oms` | Indicates the grid type of the OMS layer: flexible : flexible grid: allows user to create/ delete of MC with different widths. fixed-50G-96ch : 50GHz fixed grid |
| `grid-mode-support` | `oms`, `supported-card` | Grid-mode capabilities:<br>• not-applicable - Not applicable.<br>• flexible-c-band-only - Flexible C-band without fixed-grid characterization.<br>• general-c-ba |
| `grid-spacing` | `optical-carrier` | Fixed Grid tunability for new 3rd party TOM (GHz). |
| `group-id` | `dsc-group`, `flexo-group` | Optional parameter on dsc-group creation, specifies the dsc-group group number that the dsc is a member of for a given optical-carrier. If not provided, it is a |
| `grpc` | `protocols`, `set`, `show` | Set of attributes of the gNMI/gRPC management protocol. See grpc (p. 523) for more information. |
| `gsnr` | `submarine-link` | Indicates the expected GSNR of the link (dB). |
| `hardware-version` | `inventory`, `unprovisioned-inventory` | Hardware version of this FRU. |
| `heartbeat-interval` | `subscription-path` | Maximum time interval in milliseconds that may pass between updates from a device to a telemetry collector. If this interval expires, but there is no updated da |
| `height` | `supported-chassis` | Chassis height in RUs (Rack Units). |
| `hello-interval` | `ospf-interface` | Specifies the Hello Interval in seconds. |
| `hello-timeout` | `netconf` | Specifies the number of seconds that a session may exist before the hello PDU is received/transmitted. A session will be dropped if no hello PDU is received/tra |
| `high-threshold` | `pm-threshold`, `pm-threshold-profile` | Configured high threshold value for resources that have this parameter. |
| `hold-off-timer` | `optical-switch`, `protection-group` | Switching trigger soaking time before switching, measured and set in 1-millisecond steps. |
| `hold-on-timer` | `lldp` | Time to keep neighbor information, in case neighbor does not have an explicit Time-To-Live (TTL) TLV. |
| `hold-time` | `bgp-neighbor` | Time interval in seconds that a BGP session will be considered active in the absence of keepalive or other messages from the peer. The hold-time is typically se |
| `holder-aid` | `simulate` | AID of the equipment holder (slot or port) where the equipment will be simulated. |
| `host-card` | `ikev2-local-instance` | The reference to the service card on which this IKEv2 protocol instance is running. |
| `host-card-encryption-capability` | `ikev2-local-instance` | Indicates whether the card on which this IKEv2 local instance is running, supports the ability to do encryption. |
| `hosted-interface` | `port` | Top level interface hosted in this port. |
| `http-enabled` | `http-file-server`, `restconf` | User configurable switch to enable or disable RESTCONF HTTP access. RESTCONF HTTP access is not supported in secure mode. |
| `http-file-server` | `protocols`, `set`, `show` | See http-file-server (p. 528) for more information. |
| `http-password` | `est` | The credentials used to authenticate a user when accessing resources protected by the HTTP protocol. |
| `http-port` | `http-file-server`, `restconf` | User configurable RESTCONF HTTP port. |
| `http-proxy` | `transfer` | Proxy server for internally-generated HTTP requests leaving the NE. This includes certificate revocation-related requests, i.e.: CRL downloads and OCSP requests |
| `http-user` | `est` | Indicates the username presented to a server to authenticate and gain access to the protected resources via the HTTP protocol. |
| `https-enabled` | `http-file-server`, `restconf` | User configurable switch to enable or disable RESTCONF HTTPS access. |
| `https-port` | `http-file-server`, `restconf` | User configurable RESTCONF HTTPS port. |
| `i` | `ethernet` |  |
| `icdp` | `show` | See icdp (p. 530) for more information. |
| `id` | `cert-to-name`, `certificate`, `local-certificate`, `peer-certificate`, `secure-application`, `ssh-known-host`, `telemetry`, `trusted-certificate` | Indicates the unique identifier for the entry. |
| `if-description` | `interface` | A textual description of the interface. |
| `if-dhcp-relay` | `interface` | Enables dchp-relay function on a specific interface. It decides on which interface the DHCP/v6 relay can be run. Obeys global dhcp-relay settings. |
| `if-id` | `management-address`, `management-address-local` | The integer value used to identify the interface number regarding the management address component associated with the remote system. |
| `if-name` | `if-dhcp-relay`, `interface`, `ipv4-address`, `ipv6-address` | The interface object identifier. |
| `if-subtype` | `management-address`, `management-address-local` | This attribute describes the basis of a particular type of interface associated with the management address. |
| `if-type` | `interface` | The type of the interface. ethernet: For all Ethernet-like interfaces, regardless of speed, as per RFC 3635. software-loopback: Software Loopback interface type |
| `iid` | `flexo` | Uniquely identify each member of a group and the order of each member in the group. This information is required in the reordering process. Don’t need to be seq |
| `ike-sa-proposal` | `add`, `delete`, `set`, `show` | See ike-sa-proposal (p. 533) for more information. |
| `ikev2` | `set`, `show` | See ikev2 (p. 535) for more information. |
| `ikev2-local-instance` | `security-policy-database`, `set`, `show` | See ikev2-local-instance (p. 536) for more information. |
| `ikev2-local-instance-name` | `additional-key-exchange`, `encryption-algorithm`, `ike-sa-proposal`, `ikev2-peer`, `ipsec-sa-proposal`, `ipsec-sa-re-key`, `ipsec-spd-entry`, `ipsec-traffic-selector`, `local-ports`, `local-subnet`, `remote-ports`, `remote-subnet` | The name (ID) of the local IKE protocol daemon instance. |
| `ikev2-peer` | `add`, `delete`, `re-auth`, `re-key`, `security-policy-database`, `set`, `show` | See ikev2-peer (p. 538) for more information. |
| `ikev2-peer-name` | `additional-key-exchange`, `encryption-algorithm`, `ike-sa-proposal`, `ikev2-peer`, `ipsec-sa-proposal`, `ipsec-sa-re-key`, `ipsec-spd-entry`, `ipsec-traffic-selector`, `local-ports`, `local-subnet`, `remote-ports`, `remote-subnet` | A unique identifier for each IKE peer association. |
| `image-keys` | `show` | Container for image keys. The list can be retrieved by using show image-keys. |
| `in-use` | `secure-application` | Active certificate for this secure application. |
| `inci` | `show` | See inci (p. 549) for more information. |
| `inci-enabled` | `inci` | Switch to enable INCI. |
| `inci-neighbor` | `add`, `delete`, `set`, `show` | See inci-neighbor (p. 551) for more information. |
| `index` | `connection-ports`, `direction`, `modules-adg`, `modules-degree`, `sc-rx`, `sc-tx` | The direction index which the user has adopted (1 and 2 are used when migrating from R6.x). |
| `information` | `manifest`, `third-party-app` | Third party app information. |
| `initial-path` | `file-server` | The directory in the file server that is used as source/destination. |
| `input parameters` | `verify` |  |
| `input-attenuation-actual` | `nmc` | Actual input attenuation (in dB). |
| `input-attenuation-compensation-actual` | `nmc` | Displays the relative attenuation introduced by the automatic demux/drop control, relative to the manual configurations (band profile, NMC offset and NMC Rx Pro |
| `input-attenuation-offset` | `nmc` | Offset factor on attenuation configured by user for ingress/ demux WSS. Configurable target input attenuation offset value in the range [-5..5]dB This attribute |
| `input-attenuation-target` | `nmc` | Configurable target input attenuation. |
| `input-power-max` | `nmc` | Maximum Input Power. |
| `input-power-min` | `nmc` | Minimum Input Power. |
| `input-power-min-offset` | `nmc` | Minimum Input Power offset, of relevance for NMCs within MCs. |
| `input-power-mon` | `amplifier` | Monitored aggregate input power.-99.00 means no power. |
| `input-power-typical` | `nmc` | Typical Input Power. |
| `input-psd-max` | `nmc` | Calculated by the system from input-power-max. |
| `input-psd-min` | `nmc` | Calculated by the system from input-power-min. |
| `input-psd-profile` | `ops` | The nominal expected tributary channel power density (in dBm/12.5GHz unit). |
| `input-psd-typical` | `nmc` | Calculated by the system from input-power-typical. |
| `insertion-date` | `inventory` | Insertion Date in a date-time format (YYYY-MM-DDThh:mm:ssZ) or 'NA' if not available. |
| `install-status` | `key-replacement-package` | Indicates if this KRP has been installed in the system. |
| `installed-type` | `port` | Currently installed type in this equipment holder. If empty, means no FRU is present. |
| `instance` | `expect`, `ospf` | An existing instance of an object. |
| `instance-id` | `auth-key`, `bgp-instance`, `bgp-neighbor`, `bgp-network`, `dsc-group`, `ospf-area`, `ospf-area-range`, `ospf-instance`, `ospf-interface`, `ospf-neighbor`, `ospfv3-ipsec-security-association` | For identifying the dsc-group logic number, is added to the dsc-group model for creation. The attribute is optional and will be automatically created if not spe |
| `integrity-algorithm` | `db-protection-scheme`, `ike-sa-proposal`, `ipsec-sa-proposal`, `ospfv3-ipsec-security-association`, `secure-entity-sa-proposal` | Type of integrity algorithm used for DB. |
| `integrity-status` | `db-protection-scheme` | Indicates the status of integrity check. |
| `interactive-mode` | `cli-session-config` | Determines if the CLI shall issue interactive prompt (e.g., for prompting additional information, or for confirmation of user-initiated actions). This attribute |
| `interface` | `ace`, `acl`, `add`, `delete`, `ikev2-peer`, `ipv4-static-route`, `ipv6-static-route`, `next-hop`, `set`, `show`, `supporting-interface` | See interface (p. 554) for more information. |
| `interface-type` | `eth-zr` | Interface type of ZR TOM:<br>• 400ZR: Media-interface 400ZR- CFEC-DP-16QAM |
| `intermediate-results` | `raman-calibration` | Indicates the intermediate raman calibration results. |
| `internal-cell-switch-available-bandwidth` | `resources` | Available internal cell-switch bandwidth. i Note: This parameter is applicable only for SPN2/SPN2C cards. |
| `internal-cell-switch-total-bandwidth` | `resources` | Total internal cell-switch bandwidth. i Note: This parameter is applicable only for SPN2/SPN2C cards. |
| `interstage-loss` | `amplifier` | Interstage loss detected by the Power Control. In R6.0, the attribute is only relevant when node-type = ILA. |
| `interstage-support` | `amplifier` | True if interstage port is supported in this amplifier. |
| `inventory` | `show` | Inventory data for a present FRU. See inventory (p. 567) for more information. |
| `ip` | `ipv4-address`, `ipv6-address` | The IPv4 addresses on the interface. The following addresses are disallowed from being configured: 1. Addresses beginning with 0 (current network) 2. Addresses  |
| `ip-address` | `ntp-server` | NTP Server IP address. Ipv4/Ipv6/hostname supported. |
| `ip-monitoring` | `add`, `delete`, `set`, `show` | See ip-monitoring (p. 570) for more information. |
| `ipsec-mode` | `ospfv3-ipsec-security-association` | Indicates IPsec mode. Only transport mode is supported. |
| `ipsec-protocol` | `ipsec-spd-entry`, `ospfv3-ipsec-security-association` | Indicates the use of ESP or AH IPsec protocols. |
| `ipsec-sa-proposal` | `add`, `delete`, `set`, `show` | See ipsec-sa-proposal (p. 572) for more information. |
| `ipsec-sa-re-key` | `add`, `delete`, `set`, `show` | See ipsec-sa-re-key (p. 574) for more information. |
| `ipsec-security-association` | `re-key` | Points to IPsec SPD entry object (Child SA) |
| `ipsec-spd-entry` | `add`, `delete`, `set`, `show` | See ipsec-spd-entry (p. 576) for more information. |
| `ipsec-spd-entry-name` | `encryption-algorithm`, `ipsec-sa-proposal`, `ipsec-sa-re-key`, `ipsec-spd-entry`, `ipsec-traffic-selector`, `local-ports`, `local-subnet`, `remote-ports`, `remote-subnet` | A unique name to identify this SPD entry. |
| `ipsec-traffic-selector` | `add`, `delete`, `set`, `show` | See ipsec-traffic-selector (p. 579) for more information. |
| `ipsec-traffic-selector-name` | `ipsec-traffic-selector`, `local-ports`, `local-subnet`, `remote-ports`, `remote-subnet` | A unique name to identify this IPsec traffic selector entry. |
| `ipv4-address` | `add`, `delete`, `set`, `show` | The IPv4 address on the interface. See ipv4-address (p. 581) for more information. |
| `ipv4-address-assignment-method` | `interface` | IPv4 address assignment method. |
| `ipv4-destination-prefix` | `ipv4-static-route` | IPv4 destination prefix. |
| `ipv4-enabled` | `interface` | Controls whether IPv4 is enabled or disabled on this interface. When IPv4 is enabled, this interface is connected to an IPv4 stack, and the interface can send\n |
| `ipv4-loopback-address` | `carrier-neighbor` | IPv4 loopback address of the neighbor; may be empty if not configured. |
| `ipv4-static-route` | `add`, `delete`, `show` | A list of IPv4 static routes. See ipv4-static-route (p. 583) for more information. |
| `ipv6-address` | `add`, `delete`, `set`, `show` | The IPv6 address on the interface. See ipv6-address (p. 586) for more information. |
| `ipv6-address-assignment-method` | `interface` | IPv6 address assignment method. |
| `ipv6-destination-prefix` | `ipv6-static-route` | IPv6 destination prefix. |
| `ipv6-enabled` | `interface` | Controls whether IPv6 is enabled or disabled on this\n interface. When IPv6 is enabled, this interface is connected to an IPv6 stack, and the interface can send |
| `ipv6-loopback-address` | `carrier-neighbor` | IPv6 loopback address of the neighbor; may be empty if not configured. |
| `ipv6-static-route` | `add`, `delete`, `show` | A list of IPv6 static routes. See ipv6-static-route (p. 588) for more information. |
| `is-field-replaceable` | `supported-card` | Whether this card-type is a field replaceable unit (FRU). |
| `is-foadm` | `degree` | True if there is no WSS component at the Degree (at 'modules-degree') and PAx assigned to the degree appropriately. |
| `is-key-in-use` | `ISK` | Indicates if the key is in use in this FRU. |
| `is-key-server` | `macsec-mka` | Used to identify if local end is key server |
| `is-key-verified` | `ISK` | Indicates if the key is verified in this FRU. |
| `is-node-controller` | `chassis` | Indicates if this chassis the node controller of this NE. |
| `is-trusted` | `ntp-key` | Indicates a trusted NTP key. |
| `isk` | `clear`, `show` | Deletes Image Signing Key (ISK) from the system. For additional details, refer to ISK (p. 591). |
| `issuer` | `cert-to-name`, `crl`, `local-certificate`, `peer-certificate`, `trusted-certificate` | Specifies additional restriction that filters certificates generated by a specific issuer. |
| `issuer-name` | `ISK`, `KRK`, `key-replacement-package` | Name of the CSA (Code Signing Appliance). |
| `issuing-distribution-point-uri` | `crl` | Identifies the issuer's distribution point name URI(s) for the CRL. Only HTTP URIs are supported. This may be an empty list |
| `jitter` | `ntp-server-status` | Jitter along path to the server in milliseconds. |
| `keepalive-interval` | `bgp-neighbor` | Time interval in seconds between transmission of keepalive messages to the neighbor. Typically set to 1/3 the hold-time. |
| `key` | `auth-key` | The pre-shared key for OSPFv3 IPsec integrity protection. |
| `key-algorithm` | `csr-gen`, `est` | Specifies the algorithm to be used for a new key pair for this CSR. |
| `key-from-certificate` | `csr-gen`, `est` | Allows to reuse the key pair from an existing local-certificate. |
| `key-id` | `ntp-key`, `ssh-authorized-key` | A unique identifier (name) for this entry. |
| `key-length` | `ISK`, `KRK`, `encryption-algorithm`, `key-replacement-package` | Key length in bits. |
| `key-name` | `ISK`, `KRK`, `key-replacement-package` | The name of the Image Signing Key (ISK) resource. The name of the key to be deleted needs to be provided. |
| `key-payload` | `ISK`, `KRK`, `key-replacement-package` | Key Payload (hex format). |
| `key-replacement-package` | `show` | See key-replacement-package (p. 594) for more information. |
| `key-serial-number` | `ISK`, `KRK`, `key-replacement-package` | Key Serial Number. A list of carriers that are bound to this resource. |
| `key-server-priority` | `mka-policy` | Key server priority used by MKA protocol to select key-server |
| `key-type` | `ntp-key` | The key type. Hash algorithm for NTP message digest computation. |
| `key-usage` | `csr-gen`, `est`, `local-certificate`, `peer-certificate`, `trusted-certificate` | The Key Usage type(s) for the certificate. |
| `key-value` | `ntp-key` | NTP Key-value. |
| `keying-tries` | `ikev2-peer` | The number of rekeying attempts once a peer is considered dead. Only of relevance for scope management IPsec and name not global. |
| `known-errors` | `bgp-neighbor` | Current BGP Session state errors if any ASCII format. |
| `krk` | `show` | Image Root Key (KRK) list. See KRK (p. 597) for more information. |
| `krk-name` | `ISK`, `key-replacement-package` | Name of the KRK (Image root key) that signed this ISK. |
| `krp` | `activate` | Command parameter for installing a Key Replacement Package (KPR). |
| `krp-name` | `key-replacement-package` | Identifier for member CPUs on cards starts at 0. |
| `krp-version` | `key-replacement-package` | Package version |
| `l0-capabilities` | `show` | See l0-capabilities (p. 599) for more information. |
| `l0-comm-interface-type` | `osc` | OSC IETF ip-version (IPv4/ IPv6) This attribute indicates the SCN IP interface type (IPv4 or IPv6) to be used by L0 Applications for inter- NE communication. |
| `l0-mode-op` | `ne` | Operation mode for Power Control and services. This attribute is applicable only for 1830 GX G30. The L0 mode of operation can be:<br>• standard: the default op |
| `l0-ocm` | `status` | (1830 GX G30 only) Retrieves the OCM dashboard which provides a view from an OCM (Optical Channel Monitor) point of view. It lists, per degree, information rega |
| `l0-oxcon` | `status` | (1830 GX G30 only) Retrieves the OXcon dashboard which provides a view of system's Optical Cross Connections (OXcons), entities that are specific for L0 setups. |
| `l0-spectrum` | `status` | (1830 GX G30 only) Retrieves the spectrum power in an horizontal line, using an ASCII character-set. It is applicable to RD66TM and G2PBALPBAx card types. &lt;p |
| `l1-traffic` | `status` | Retrieves the L1 traffic dashboard which provides a table containing all configured L1 traffic ports in the system, and information associated with each port. B |
| `label` | `ace`, `activate`, `adg`, `alarm`, `amplifier`, `amplifier-raman`, `ase-idler-source`, `card`, `cert-to-name`, `chassis`, `cid-ptp`, `comm-channel` +70 more | Represents the label to apply on the template - optional |
| `laser-safety-mode` | `ots` | Laser Safety Mode of the OTS instance:<br>• OPLM - Optical Power Limited Mode<br>• APSD - Automatic Power Shut Down |
| `laser-state` | `optical-ptp` | The emitting pump (e.g. booster) laser state. RD amplifiers: source (Tx) pump disabled. Raman modules: Pump Laser, and actual traffic emitted from dwdm-line por |
| `laser-toggling-for-tts` | `fc` | Enable or disable the laser-toggling-for-tts when tts is enabled/disabled for 32GFC client. |
| `last-backup` | `recovery` | Timestamp with the last backup performed. |
| `last-calibration-timestamp` | `raman-calibration` | Time when the last time the automatic Raman gain calibration rpc was completed with or without errors. |
| `last-change-time` | `interface-neighbor`, `lldp-port-statistics` | Provide a timestamp indicating when the interface neighbor information was last updated. |
| `last-changed` | `current-alarms` | Timestamp of the last change in the current alarm list (either a raise or clear event). |
| `last-changed-time` | `alarm` | Timestamp of the last change occurred in the alarm. |
| `last-clear-time` | `lldp-port-statistics` | The timestamp associated with the last time this port was cleared. |
| `last-completion-status` | `file-type`, `transfer-status` | Last transfer Status |
| `last-duration` | `file-type`, `transfer-status` | Last transfer duration |
| `last-login-date` | `user` | The last login date/time of the user. |
| `last-measurement` | `ocm-ptp` | Last OCM scan measurement date and time. ('never' is an extended part for yang:date-and-time) |
| `last-operation` | `file-type` | Last transfer operation |
| `last-query` | `ocsp-server` | Timestamp of last successful query. |
| `last-reboot-reason` | `card` | Reason why the last reboot was done. |
| `last-reboot-time` | `card` | Timestamp of the last reboot event of a card. |
| `last-request` | `optical-switch` | Displays the last user request received on the optical-switch. The external protection commands result in the update of last-request. Upon successful validation |
| `last-start-time` | `sw-service` | Time of the last service start/boot. |
| `last-switch-trigger` | `optical-switch`, `protection-group` | Specifies the last reason that triggered a protection switchover. |
| `last-test-qualifier` | `cable-id-path` | Display last test status:<br>• up-to-date - Up to date, when cable-id test completed.<br>• out-dated - Out dated, when there is any fault on fiber. |
| `last-test-timestamp` | `cable-id-path` | Timestamp for the last cable-id verification for the port pair. |
| `last-time-jump` | `clock` | Indicates last system time jump in the format '&lt;time1&gt; to &lt;time2&gt;'. Time jumps of less than 10 seconds are ignored. |
| `last-transfer` | `file-type`, `transfer-status` | Last transfer Start Timestamp |
| `last-update` | `carrier-neighbor`, `lldp-neighbor` | Time of the last update |
| `last-update-result` | `cdp` | Result of the most recent CRL update. |
| `last-update-time` | `cdp` | Timestamp of most recent CRL update. |
| `last-used-local-certificate` | `ikev2-peer` | A reference to the specific local entity leaf certificate that was last used during the IKE authentication with the far-end peer. |
| `last-used-peer-certificate` | `ikev2-peer` | A reference to the specific peer leaf certificate that was last used to authenticate the far-end IKE peer. |
| `last-used-time` | `crl` | Timestamp of last usage of this CRL for revocation checking. |
| `latitude` | `ne` | Latitude of the network element. |
| `launch-condition` | `submarine-link` | Defines the launch option for the Tx pre-emphasis. |
| `launching-fiber-length` | `otdr-ptp` | Specifies the launching fiber length (in meters) information for SOR to filter the launching fiber path data. A launching fiber may be used to connect an OTDR p |
| `led` | `show` | Representation of a LED in an FRU. See led (p. 600) for more information. |
| `led-mode` | `activate` | Indicates the LED mode behavior:<br>• flash (default value) - for an amber light flashing/blinking at the frequency of 1Hz (usually used for LED location)<br>•  |
| `leds` | `show`, `supported-card`, `supported-chassis`, `supported-port`, `supported-slot` | To view the list of the system leds use the command show leds. See led (p. 600) for more information. |
| `line-encoding` | `optical-carrier` | Currently line-encoding mode. |
| `line-port` | `ethernet` | Specify the line port for the client. Can only be configured when mapping mode is openZR+. |
| `line-ptp` | `add`, `set`, `show` | See line-ptp (p. 604) for more information. |
| `line-system-mode` | `line-ptp`, `super-channel` | Indicates the specific mode of power control configured on the L1 transponder, and specifically, on this particular SCG port within the L1 transponder. The attr |
| `line-system-mode.` | `super-channel-group` | Indicates the specific mode of power control configured on the L1 transponder, and specifically, on this particular SCG port within the L1 transponder. |
| `link-degrade-indication` | `eth-zr` | The local and remote link degradation status:<br>• none: no Link degradation.<br>• local-degraded: link has local degradation.<br>• remote-degraded: link has re |
| `link-name` | `submarine-link` | Defines the name of the link. |
| `link-security-control` | `macsec-entity` | Controls the link security policy, to handle data packets when MACsec connection is not available |
| `links` | `show` | See links (p. 611) for more information. |
| `lldp` | `set`, `show` | Global LLDP configuration attribute. See lldp (p. 612) for more information. |
| `lldp-admin-status` | `comm-eth`, `ethernet` | LLDP operational mode for this port. tx-only: LLDP agent transmits LLDP frames on this port but it does not store connected remote system information. rx-only:  |
| `lldp-egress-mode` | `ethernet` | If lldp enabled, define what is the LLDP behavior for this direction. |
| `lldp-ingress-mode` | `ethernet` | If lldp enabled, define what is the LLDP behavior for this direction. |
| `lldp-local-info` | `show` | See lldp-local-info (p. 613) for more information. |
| `lldp-mgmt-addr-if` | `comm-eth` | Specify which interface's IP address to be used for management address. This parameter must be explicitly set by the user and is applicable when the lldp-admin- |
| `lldp-neighbor` | `show` | LLDP remote system discovered by lldp-port. See lldp-neighbor (p. 616) for more information. |
| `lldp-port` | `custom-tlv`, `lldp-local-info`, `lldp-neighbor`, `lldp-port-statistics`, `management-address`, `management-address-local` | Local port that is associated with the LLDP agent. |
| `lldp-port-statistics` | `show` | LLDP frame reception statistics for a particular port and direction. See lldp-port-statistics (p. 620) for more information. |
| `lldp-transmit-interval` | `comm-eth` | The interval to transmit LLDP Tx TLVs (in seconds). |
| `loading-policy` | `ots` | Currently this attribute is applicable to SLTE only. Indicates which policy is to be used for a degree internally by Loading Manager for filtering loading reque |
| `local-address` | `ikev2-local-instance` | Local IPv4 address for IKEv2 channel with prefix-length 32. |
| `local-address-assignment-method` | `ikev2-local-instance` | Local IP address assignment method for IKEv2 channel. |
| `local-as` | `bgp-instance` | The local autonomous system number that is to be used when establishing sessions with the remote peer or peer group. |
| `local-carrier` | `carrier-neighbor` | Local carrier instance that has discovered this neighbor node |
| `local-carrier-id` | `carrier-neighbor` | AID of local carrier |
| `local-certificate` | `ikev2-peer`, `set`, `show` | X509v3 end-entity certificate that represents one of various secure application identities. See local-certificate (p. 622) for more information. |
| `local-identity` | `ikev2-peer` | Identity of local IKE instance. |
| `local-identity-type` | `ikev2-peer` | Type of local identity |
| `local-interface` | `interface-neighbor` | Name of interface neighbor. |
| `local-ip-address` | `session` | Local ip address of the session. |
| `local-port` | `bootstrap` | Supporting-port of the MGMT vrf osc-eth interface for the OSC link to the neighbor (same value as in 'show interface', e.g. 1-1-dwdm-line1). The implementation  |
| `local-ports` | `add`, `delete`, `show` | See local-ports (p. 627) for more information. |
| `local-subnet` | `add`, `delete`, `show` | See local-subnet (p. 629) for more information. |
| `local-switch` | `console` | Defines the global access to all card's console port: • use-global-switch - Console switch is using the global switch configuration.<br>• force-enable - Console |
| `location` | `alarm`, `alarm-severity-entry`, `get-conditions`, `led`, `pm`, `pm-control-entry`, `pm-profile-entry`, `pm-threshold`, `pm-threshold-profile`, `sw-service` | The location of the FRU, that is, the AID of equipment location of the LED (may be a chassis, card or a port AID). |
| `location-id` | `packaged-fw`, `software-load`, `software-location`, `sw-component`, `sw-subcomponent` | Location ID (&lt;chassis-id&gt;-&lt;slot-id&gt;) of the SW load subcomponent. Software load information associated to each of the equipment. |
| `location-led` | `activate` | Command parameter for starting a location LED test. For location-led specific parameters, refer to Table 80: activate location-led command parameters (p. 156). |
| `location-led-support` | `supported-card` | Whether this card-type supports location-led operation. |
| `lof-soak-timer` | `osc` | LOF soak timer configuration:<br>• short: No additional soak timer beyond the base LOF soak for OSC LOF. An OSC LOF alarm is raised almost immediately after the |
| `log` | `clear`, `show` | Removes content for a specific log-file. For additional details, refer to log (p. 633). |
| `log-console` | `set`, `show` | Set of attributes of the console logging supported by the system. See log-console (p. 637) for more information. |
| `log-console-facility-filter` | `add`, `delete`, `set`, `show` | Selector that allows to filter log messages based on their source facilities and severities. See log-console-facility-filter (p. 639) for more information. |
| `log-file` | `add`, `delete`, `set`, `show` | Local syslog files supported by the system. See log-file (p. 642) for more information. |
| `log-file-facility-filter` | `add`, `delete`, `set`, `show` | Selector that allows to filter log messages based on their source facilities and severities. See log-file-facility-filter (p. 646) for more information. |
| `log-file-facility-filter-name` | `log-file-facility-filter` | Facility filter selector. |
| `log-file-message-coalescence` | `syslog` | If true, prevent flooding of identical messages during abnormal conditions. If there are multiple identical log messages for log files, there will be one \n mes |
| `log-file-name` | `log` | The name of the log file to have it's contents removed. |
| `log-relay` | `syslog` | Flag to enable remote logging from a shelf controller to a node controller. If false, disable all remote logging from shelf controller to node controller |
| `log-server` | `add`, `delete`, `set`, `show` | Grouping the configuration parameters for log forwarding. See log-server (p. 649) for more information. |
| `log-server-facility filter` | `set` | Selector that allows to filter log messages based on their source facilities and severities. See log-server-facility-filter (p. 653) for more information. |
| `log-server-facility-filter` | `add`, `delete`, `show` | Selector that allows to filter log messages based on their source facilities and severities. See log-server-facility-filter (p. 653) for more information. |
| `log-server-facility-filter-name` | `log-server-facility-filter` | Facility selector. Identifies a single syslog facility, or all of them if value is 'all'. |
| `log-server-name` | `log-server-facility-filter` | The file name without the .log extension. |
| `logging-action` | `ace` | Flag to indicate if logging needs to be done once the ACE rule is matched. |
| `logname` | `log` | name of the log to display |
| `longitude` | `ne` | Longitude of the network element. |
| `loopback` | `activate`, `ethernet`, `fc`, `interlaken`, `oc`, `optical-carrier`, `otu`, `stm` | Loopback mode.Useful to debug on the fiber connection. |
| `loopback-host-interface` | `eth-zr` | Loopback on host interface. Useful to debug on the fiber connection. |
| `loopback-ipv4` | `database` | loopback ipv4 address. |
| `loopback-ipv6` | `database` | loopback ipv6 address. |
| `loopback-mode` | `ethernet`, `fc`, `oc`, `otu`, `stm` | Indicates loopback action for facility or terminal. |
| `loopback-modem-interface` | `eth-zr` | Loopback on modem interface. Useful to debug on the fiber connection. |
| `loopback-state` | `nmc` | Indicates the state of the Loopback request. This parameter is applicable only to tributary NMCs on the AD port and is updated by the loopback manager. • unknow |
| `los-threshold-hysteresis` | `optical-switch` | SF threshold hysteresis (in dB). Applies to both working-switch-threshold and protect-switch-threshold. The recommended configured value for MCHP and OMSP deplo |
| `loss-calibration-by-otdr` | `ots-r-auto-otdr` | Specifies if external-attenuation values used in Power Control come from user-configured attributes or automatically measured attributes:<br>• none: For green f |
| `low-threshold` | `pm-threshold`, `pm-threshold-profile` | Configured low threshold value for resources that have this parameter. |
| `lower-frequency` | `mc`, `mc-f`, `ocm-channel`, `oms`, `spectrum-monitoring` | Lower Frequency of a Media Channel. |
| `mac-address` | `comm-eth` | MAC Address of the port. |
| `macsec-cipher-suite` | `mka-policy` | Cipher suites for Secure Association Key(SAK) derivation |
| `managed-by` | `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet`, `fc`, `flexo`, `flexo-group`, `interlaken`, `line-ptp`, `mc` +23 more | Describes whether this facility was system created or not. Only user-created facilities can be user deleted. |
| `management-address` | `show` | See management-address (p. 663) for more information. |
| `management-address-local` | `show` | See management-address-local (p. 665) for more information. |
| `manifest` | `prepare-upgrade`, `show` | Downloaded manifest file and its information. See manifest (p. 667) for more information. |
| `manifest-file` | `downloaded-image`, `fru-info`, `manifest` | The manifest file |
| `manifest-signature` | `manifest` | Manifest file signature. |
| `manufacture-date` | `inventory`, `unprovisioned-inventory` | Manufacture Date in a date-time format (YYYY-MM-DDThh:mm:ssZ) or 'NA' if not available. |
| `map-type` | `cert-to-name` | Specifies whether to map the certificate to an explicit username or extract the identity from a designated certificate field. |
| `mapped-user-name` | `cert-to-name` | Required only if Map-Type is "map". Defines the local or remote user identity associated with the presented X.509 client certificate. |
| `max-add-drop-ports` | `adg` | The max number of ports available for a given ADG. |
| `max-adgs` | `oadm-capabilities` | Maximum number of ADGs (Add/ Drop Group(s)); 0 if not supported. ADGs are only supported in OADM node types. |
| `max-capacity` | `subtype-constraint` | The maximum capacity supported by this subtype. -1 means there is no maximum capacity constraint. |
| `max-degrees` | `oadm-capabilities` | Maximum number of degrees; 0 if not supported. Degrees are only supported in OADM node types. The maximum degrees for ILA node-type is 0 by convention (ILA has  |
| `max-failure` | `comm-channel` | Specifies the maximum failure value of the PPP protocol. Max- Failure indicates the number of Configure-Nak packets sent without sending a Configure-Ack before  |
| `max-file-size` | `log-file` | Maximum file size before rotation (in megabytes). |
| `max-invalid-login` | `user` | This attribute is the maximum number of consecutive and invalid login attempts before an account is suspended (locked out). |
| `max-local-users` | `security-policies` | The maximum number of local users that can be configured in the system. |
| `max-packet-length` | `ethernet` | Maximum transfer unit for ethernet facility, in octets. |
| `max-power-draw` | `card`, `supported-card` | Maximum power draw for this card. |
| `max-sessions` | `user` | This attribute specifies the maximum number of sessions allowed for this user. |
| `max-slots` | `degree` | Maximum number of slots permitted to be joined together to form a media channel. Must be greater than or equal to the min-slots. |
| `max-system-sessions` | `security-policies` | The maximum number of management sessions that the system supports. Note: session via serial console does not count against this maximum. |
| `max-target-pump-power` | `pump-power` | Maximum target pump power. |
| `max-value` | `pm-threshold-profile` | Maximum value for this parameter. |
| `mc` | `show` | See mc (p. 671) for more information. |
| `media-interface` | `optical-carrier` | Media interface type of ZR tom. |
| `memory-usage` | `sw-container`, `sw-service` | Current usage of memory by the container, in percentage. |
| `message content` | `message` | The message text to broadcast |
| `message-coalescence` | `log-server` | If true, prevent flooding of identical messages during abnormal conditions. If there are multiple identical log messages, there will be one message logged fully |
| `message-format` | `log-server` | Identifies the syslog messaging format. |
| `metadata-from-certificate` | `csr-gen`, `est` | A local-certificate id to be used as metadata source. Metadata details can be overridden separately. |
| `metadata-from-cnf` | `csr-gen`, `est` | Multi-line string input of cnf with metadata. Metadata details can be overridden separately. OpenSSL CSR request configuration for metadata-template from-openss |
| `metadata-template` | `csr-gen`, `est` | Selects the possible sources for the CSR metadata, including reusing it from an existing certificate, loading from an openssl cnf file, or using a generic templ |
| `min-capacity` | `subtype-constraint` | The minimum capacity supported by this subtype. |
| `min-slots` | `degree` | Minimum number of slots permitted to be joined together to form a media channel. Must be less than or equal to the max-slots. |
| `min-target-pump-power` | `pump-power` | Minimum target pump power. |
| `min-value` | `pm-threshold-profile` | Minimum value for this parameter. |
| `min-wss-bias-tx` | `oms` | Specify the minimum aggregate (NMC level) attenuation on any owned slice of valid channels that the Mux WSS control should target. This indirectly defines the r |
| `minimum-password-length` | `security-policies` | Configurable minimum length for user passwords. When a password is changed, the password length will be verified according with this policy. |
| `mka-policy` | `macsec-mka` | mka policy name to use |
| `mode` | `authorization`, `comm-channel`, `comm-eth`, `db-protection-scheme`, `dhcp-relay`, `ipsec-spd-entry` | Current Protection Scheme of DB. Can be changed via 'db-migrate' RPC. |
| `modification-time` | `local-certificate`, `peer-certificate`, `trusted-certificate` | Timestamp of certificate installation/rotation. |
| `modulation-format` | `eth-zr`, `flexo-group`, `optical-carrier` | Current modulation format. |
| `module-name` | `access-rule` | YANG Module to consider when considering this rule; needs to match an available data-model file. By default, the value '*' is used to represent 'any module name |
| `modules-adg` | `add`, `delete`, `set`, `show` | See modules-adg (p. 680) for more information. |
| `modules-degree` | `add`, `delete`, `set`, `show` | See modules-degree (p. 682) for more information. |
| `monitored` | `oxcon` | Monitoring/ not-monitored indication; does not change during OXcon lifetime. |
| `monitored-channel` | `show` | See monitored-channel (p. 684) for more information. |
| `monitored-optical-power` | `monitored-channel` | Measured power for the corresponding carrier (channel) in dBm. The value -99.00 means no power. |
| `monitored-port` | `ocm-mp` | The port that is being monitored. Can be different of supporting-port for a non-integrated OCM. • not-applicable - Not Applicable/ Not specified.<br>• instance- |
| `monitored-width` | `monitored-channel` | Carrier (channel) width configured at the NMC within the oxcon source/ destination, in MHz. |
| `monitoring-instance` | `ipv4-static-route`, `ipv6-static-route` | Monitoring instance name, applicable only if this route is being monitored. |
| `monitoring-mode` | `odu-diagnostics`, `oms`, `osc`, `ots-diagnostics`, `otu-diagnostics` | OMS monitoring mode. For node-type ILA, the default value is not-monitored. For node-type ILA with integrated DGE ( hyperscale ILA nodes), the default value is  |
| `monitoring-state` | `ipv4-static-route`, `ipv6-static-route`, `nmc`, `nmc-f`, `ocm-mp`, `ocm-ptp`, `oms`, `optical-ptp` | The system reports this attribute, to indicate whether the NMC is intended to be in use (instead of simply being pre-provisioned). Monitoring state is enabled i |
| `mru` | `comm-channel` | Specifies the MRU (Maximum- Receive-Unit) in the Information and Padding fields. This parameter is available only when the mode is L3. |
| `msim-config` | `odu` | Specifies MSIM alarm reporting or not when msi value received does not followed G.709 definition. |
| `mtls authentication method` | `security-policies` | Indicates the user authentication method(s) to use for access to TLS-based applications. |
| `mtu` | `comm-channel`, `comm-eth` | The maximum transmission unit size in octets for the physical Ethernet port of comm channel. This parameter is available only when the mode is L3. |
| `multiplicity` | `golden-advanced-parameter` | Identifies the number of values users need to enter for this advanced parameter. Same range or allowed-values will apply for each entry. This parameter is read- |
| `name` | `ISK`, `KRK`, `access-rule-list`, `acl`, `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `cable-id-path`, `capabilities`, `card`, `cdp` +97 more | Task name |
| `named-value-set-name` | `named-value-set` | User assigned name for this named-value-set. |
| `nct-connection` | `show` | See nct-connection (p. 687) for more information. |
| `ne` | `set`, `show` | See ne (p. 690) for more information. |
| `ne-function` | `show` | See ne-function (p. 697) for more information. |
| `ne-id` | `carrier-neighbor`, `ne` | Id of the remote network element. |
| `ne-location` | `ne` | Name of the location of this particular network element. |
| `ne-name` | `carrier-neighbor`, `database`, `ne` | User assigned name for this NE as present in this database. |
| `ne-site` | `ne` | Name or CLLI of the site where this network element is located. |
| `ne-sub-location` | `ne` | Name of the secondary location of this particular NE. |
| `ne-type` | `carrier-neighbor`, `ne` | Type of the remote network element |
| `ne-vendor` | `ne` | Vendor name of the NE. |
| `near-end-tda` | `trib-ptp` | The switching of near end TDA. |
| `negotiated-hold-time` | `bgp-neighbor` | Negotiated hold time between two BGP neighbors. |
| `neighbor-address` | `bootstrap`, `inci-neighbor` | P address of the provisioned remote neighbor NE. |
| `neighbor-adjacency-state` | `interface-neighbor` | Indicates protocol state. |
| `neighbor-id` | `inci-neighbor` | Node-ID of provisioned neighbor. |
| `neighbor-interface-name` | `interface-neighbor` | Indicates discovered neighbor interface name. |
| `neighbor-ipv4-address` | `interface-neighbor` | Indicates discovered neighbor ipv4 address. |
| `neighbor-ipv6-address` | `interface-neighbor` | Indicates discovered neighbor ipv6 address. |
| `neighbor-ne-id` | `interface-neighbor` | Indicates discovered neighbor ne ID. |
| `neighbor-ne-name` | `interface-neighbor` | Indicates discovered neighbor ne name. |
| `neighbor-router-id` | `interface-neighbor` | Indicates discovered neighbor router ID. |
| `netconf` | `protocols`, `set`, `show` | Set of attributes of the configuration of the NETCONF management protocol. See netconf (p. 699) for more information. |
| `netmask` | `ipv4-address` | The subnet specified as a netmask for a particular address. Only valid netmasks are allowed to be configured. |
| `network-mapping` | `xcon` | Indicates the server layer protocol type that supports this XCON. |
| `network-prefix` | `bgp-network` | Specifies the network prefix. |
| `network-side-csf-trigger` | `protection-group` | Considers a network-side ingress CSF defect as a trigger for switch-over. |
| `network-side-sd-trigger` | `protection-group` | Considers a network-side ingress SD defect as a trigger for switch-over. |
| `network-xconnect` | `show` | See network-xconnect (p. 701) for more information. |
| `networking` | `show` | Top level container for networking model. The list can be retrieved by using show networking. See networking (p. 702) for more information. |
| `networking-services` | `show` | See networking-services (p. 703) for more information. |
| `new-admin-password` | `activate`, `database`, `download`, `prepare-upgrade` | The password for the new-admin-user that is auto-configured after the database is wiped. The password can be provided as a password hash ( format: $&lt;id&gt;$& |
| `new-admin-user` | `activate`, `bootstrap`, `database`, `download`, `prepare-upgrade` | The user-name that is auto-configured after the database is wiped. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-n |
| `new-file-path` | `file`, `file-operation` | New file path when the operation is rename. |
| `new-password` | `bootstrap`, `password` | The the new password inline with the command. |
| `new-time` | `set-time` | Time to set in the system. |
| `next-backup` | `recovery` | Timestamp for the next backup to be performed. |
| `next-hop` | `ip-monitoring`, `show` | Next-hop of a route item. See next-hop (p. 704) for more information. |
| `next-hop-address` | `ipv4-static-route`, `ipv6-static-route`, `next-hop` | Next hope address. |
| `next-layer-protocol` | `ipsec-traffic-selector` | Indicates the inner protocol (upper layer), obtained from the IPv4 protocol or the IPv6 next header field. |
| `next-run` | `scheduled-task`, `task` | Next run timestamp. May be 'never' for finished tasks. |
| `next-update` | `crl` | The date by which the next CRL will be issued. |
| `next-update-time` | `cdp` | Timestamp of next CRL update. |
| `niap-compliant-logging` | `syslog` | Flag to enable or disable NIAP complaint logging. Sets whether the logs are NIAP compliant or not. |
| `nmc` | `add`, `delete`, `set`, `show` | See nmc (p. 706) for more information. |
| `nmoper-alarm-reporting` | `ots-diagnostics` | If enabled, the mismatch beween expected-operator and rx-operator parameters will raise the Neighbor-Mismatch Operator- Specific field (NMOPER) alarm. Disabling |
| `no-switchover` | `chassis` | If enabled, the standby controller will be locked out from taking over the active card. This means no manual or autonomous switchover will happen. This paramete |
| `node-controller-chassis-name` | `ne` | Selects the chassis that has the role of the network element controller. i Note: Changing node-controller-chassis-name requires a manual database reset. Therefo |
| `node-controller-serial-number` | `database` | Serial number of the node controller. |
| `node-type` | `ne` | Node Type refers to the main function NE agent operates. Used to distinguish the NE function as an ILA (In-Line Amplifier, applicable to 1830 GX G34c only) or a |
| `node-type-compatibility` | `supported-card` | Node Type Compatibility refers to supported NE Node-type for a sled card. Only of relevance for line-card(s) and carrier-card(s). Possible values: • all - compa |
| `nsa-upgrade-version` | `third-party-fw` | Versions from where the upgrade is non service affecting (nsa). |
| `ntp` | `set`, `show` | See ntp (p. 723) for more information. |
| `ntp-active-server` | `ntp` | Currently active NTP server. |
| `ntp-auth-enabled` | `ntp` | Whether NTP authentication is enabled. |
| `ntp-enabled` | `ntp` | Whether ntp is enabled. |
| `ntp-key` | `add`, `delete`, `set`, `show` | Keys to be used for NTP authentication. See ntp-key (p. 725) for more information. |
| `ntp-server` | `add`, `delete`, `set`, `show` | Configured NTP server. See ntp-server (p. 727) for more information. |
| `ntp-server-status` | `show` | NTP server status. See ntp-server-status (p. 730) for more information. |
| `number` | `additional-key-exchange`, `encryption-algorithm`, `ike-sa-proposal`, `ipsec-sa-proposal`, `secure-entity-sa-proposal` | The proposal number for the IKE SA. |
| `number-of-alarms` | `current-alarms` | Number of currently raised alarms. |
| `number-of-files` | `log-file` | Maximum number of log files retained. When rotating files due to max size being reached, the oldest files will be discarded if the total number of files is grea |
| `number-of-front-slots` | `supported-chassis` | Number of equipment holder slots in the front plate on the chassis. |
| `number-of-lanes` | `inventory` | When applicable, provides number of supported optical lanes in this equipment. |
| `number-of-pumps` | `amplifier-raman` | Number of pumps for the required-equipped card. This value dictates the number of pump-power objects exposed by the system. |
| `number-of-rear-slots` | `supported-chassis` | Number of equipment holder slots in the back plate on the chassis. |
| `number-of-runs` | `task` | Applicable when frequency is configured. This attribute defines the number of times a periodic task is executed before stopping. |
| `number-of-switchover-events` | `controller-card` | Number of times that an active controller card has switchover. Value only visible on active controller card. |
| `nw-xconnect` | `add`, `delete`, `show` | See nw-xconnect (p. 732) for more information. |
| `oadm-capabilities` | `add`, `delete`, `set`, `show` | See oadm-capabilities (p. 736) for more information. |
| `objec-id` | `system` | object-id list includes: clock/ System clock. file-servers/ Container of all configured file-servers. networking/ ntp/ Network Time Protocol Configuration proto |
| `object` | `gcmt`, `template` | object name to apply to (e.g. odu) |
| `oc` | `set`, `show` | See oc (p. 738) for more information. |
| `oc-type` | `oc` | Type of SONET signal. Level N of OC-N (Optical Carrier level N). |
| `och-center-frequency` | `optical-switch` | Defines the och center frequency. It is applicable to OPSM-PT only. It is not exposed on the OPSM. |
| `och-connection` | `nmc` | Optical channel connection. |
| `ochm` | `show` | See ochm (p. 743) for more information. |
| `ocm-channel` | `show` | See ocm-ptp (p. 754) for more information. |
| `ocm-enable` | `ocm-mp`, `ocm-ptp` | Enables regular power monitoring. |
| `ocm-monitoring` | `modules-adg` | Set upon creation, cannot be changed after supported-card being assigned. By default, the value is 'true', but can optionally be configured to 'false' for OMD c |
| `ocsp-based-revocation` | `security-policies` | This policy defines whether OCSP responders can be consulted for certificate revocation checking. |
| `ocsp-server` | `show` | See ocsp-server (p. 758) for more information. |
| `odu` | `add`, `delete`, `set`, `show` | odu4 (100G) or oduflex (400G) facility representing low order ODUs that XCONs are mapped into. See odu (p. 761) for more information. |
| `odu-diagnostics` | `set`, `show` | See odu-diagnostics (p. 771) for more information. |
| `odu-name` | `odu` | A system-defined user friendly name for this odu, considering both the type and the rate. Examples: ODU4, ODUC8i. |
| `odu-type` | `odu` | The protocol type of the ODUk/ODUCn client. ODUCn : OTUCn protocol layer. "ODUCni: Nokia proprietary OTUCni protocol layer. ODU4: ODU4 protocol layer. ODU4i: No |
| `offset` | `ntp-server-status` | Offset of clock to the peer in milliseconds. |
| `old-password` | `password` | The old password inline with the command. |
| `olos-shutdown-disable` | `amplifier` | If it is set to be true, on input OLOS, EDFA shutdown does not depend on absence of input light. It is visible at:<br>• pre-amplifier of RD20TM;<br>• CAD10A amp |
| `olos-shutdown-soak-timer` | `amplifier` | On input OLOS, the system soaks for the specified time (in milliseconds), and if the fault still persists, it triggers the consequent action (shutdown). The ran |
| `oms` | `set`, `show` | See oms (p. 777) for more information. |
| `openwave-contention-check` | `super-channel` | Enables DNA assisted contention control mechanism in openwave mode. |
| `oper-state` | `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `card`, `chassis`, `cid-ptp`, `comm-channel`, `comm-eth`, `dsc`, `dsc-group`, `eth-zr` +44 more | The operational state of this object. |
| `operation` | `access-rule`, `file`, `file-operation`, `transfer-status` | The list of operations that the rule applies to. The '*' value represents all operations, and is the default value. Note: YANG bits represent a data type where  |
| `operation-type` | `protection-switch` | The type of protection switch command. |
| `operational-bandwidth` | `comm-channel` | Indicates the control channel's operational bandwidth/capacity. i Note: Operational bandwidth is displayed for OSCX comm-channels. |
| `operational-duplex-mode` | `comm-eth` | Operational duplex mode. |
| `operational-flow-control` | `comm-eth` | Operational flow control. |
| `operational-rate` | `comm-eth` | Operational Ethernet rate (1/10/100/1000/10000 Mbits or maximum). |
| `operator-last-action` | `alarm` | Timestamp when the alarm was last changed by operator. |
| `operator-name` | `alarm` | Username that last changed the state of this alarm. |
| `operator-state` | `alarm` | State of the alarm according with operator action. |
| `operator-text` | `alarm` | Text provided by operator when changing alarm state (length 0..256). |
| `opm-pwr` | `ocm-channel` | Optical Parameter Monitor - power (in dBm). |
| `ops` | `set`, `show` | Optical Physical Section (OPS) facility. See ops (p. 790) for more information. |
| `optical-carrier` | `set`, `show` | See optical-carrier (p. 796)for more information. |
| `optical-carrier-name` | `advanced-parameter`, `current-advanced-parameter` | The name of the optical carrier. |
| `optical-channel` | `set`, `show` | See optical-channel (p. 807) for more information. |
| `optical-ptp` | `show` | See optical-ptp (p. 810) for more information. |
| `optimum-edfa-gain` | `amplifier` | System reports the optimum EDFA gain the required equipped EDFA has. By convention, the system reports 0 dB when the card is not required equipped. |
| `option` | `prepare-upgrade` | Predefined options available for prepare-upgrade:<br>• validate - validates the software manifest.<br>• apply - applies the software manifest. |
| `optional-content` | `upload` | List of keywords associated with optional content to be selected for debug-log upload. |
| `opucn-time-slots` | `odu` | Opucn Time slots of the ODUCn.Optional parameter on LO-ODU creation, identifies the ODU within the parent/ high-order ODU. If not provided, it is automatically  |
| `org-name` | `cert-gen` | Organization Name. |
| `origin` | `dns-server`, `ipv4-address`, `ipv4-static-route`, `ipv6-address`, `ipv6-static-route`, `log-server`, `ntp-server` | DNS address assignment method, the user can convert DHCP configured DNS entry into a manual configured by changing this attribute. |
| `original-recover-mode-reason` | `ne` | Original reason for recover mode. Displays the original recover mode reason, available only when current reason has changed. |
| `osc` | `set`, `show` | See osc (p. 829) for more information. |
| `osc-compatibility` | `ots` | Beginning with R7.0, user can configure osc-compatibility. The following 3 use-cases are supported:<br>• osc-compatibility = osc-g30 and osc-less = disabled.<br |
| `osc-control` | `osc` | OSC control configuration.<br>• auto - Automatic attenuation control mode in which system will decide the attenuation value.<br>• manual - Manual attenuation co |
| `osc-less` | `ots` | OSC-less mode is required to provide interworking with systems with no compatible OSC or spans with losses not compatible with the OSC budget. |
| `osc-less-support` | `ots` | Indicates whether osc-less mode configuration is supported/ allowed or not: • for node-type ILA: always 'false'.<br>• for node-type OADM: 'true' for RD09SM/RD20 |
| `osc-mode` | `osc` | Currently used system OSC bitrate. OC3 represents 155Mbit/s. OC3 is not supported for HSC OLS nodes. 1GE represents 1GE OSC with FEC (1.25Gbit/s datarate). It i |
| `osc-wavelength` | `osc` | Indicates the wavelength of the OSC channel transmitted. Value is read from OSC transceiver (SFP or other). By convention, 0 indicates no OSC power can be read. |
| `oscc-support` | `osc` | OSC Control support. |
| `ospf` | `clear` | Clears an ospf-instance asynchronously. For additional details, refer to ospf (p. 837). |
| `ospf-area` | `add`, `delete`, `set`, `show` | See ospf-area (p. 839) for more information. |
| `ospf-area-id` | `auth-key`, `ospf-area`, `ospf-area-range`, `ospf-interface`, `ospf-neighbor`, `ospfv3-ipsec-security-association` | OSPF Router Area ID. |
| `ospf-area-range` | `add`, `delete`, `set`, `show` | See ospf-area-range (p. 841) for more information. |
| `ospf-area-type` | `ospf-area` | OSPF Router Area Type. |
| `ospf-auth-algorithm` | `ospf-interface` | Cryptographic algorithm associated with key. Only of relevance for ospfv2. |
| `ospf-auth-enable` | `ospf-interface` | Enable/Disable Authentication. Only of relevance for ospfv2 or ospfv3. |
| `ospf-auth-key` | `ospf-interface` | Authentication key string in ASCII format. Only of relevance for ospfv2. |
| `ospf-cost` | `ospf-interface` | OSPF link cost. |
| `ospf-if-name` | `auth-key`, `ospf-interface`, `ospf-neighbor`, `ospfv3-ipsec-security-association` | Reference of the interface in an OSPF area. |
| `ospf-if-routing` | `ospf-interface` | Specifies if Routing is enabled and if so, if Routing is passive or active.<br>• active: This link is advertised and routing messages are transported over this  |
| `ospf-instance` | `add`, `delete`, `set`, `show` | OSPF protocol instances. See ospf-instance (p. 844) for more information. |
| `ospf-interface` | `add`, `delete`, `set`, `show` | See ospf-interface (p. 846) for more information. |
| `ospf-network-type` | `ospf-interface` | OSPF Interface Network Types. |
| `ospfv3-ipsec-security-association` | `add`, `delete`, `set`, `show` | See ospfv3-ipsec-security-association (p. 852) for more information. |
| `otdr` | `activate`, `add`, `delete`, `set`, `show` | See otdr (p. 854) for more information. |
| `otdr-direction-mode` | `otdr-ptp` | Specifies the OTDR measurement direction and if OTDR measurement is in-service or out-of-service. |
| `otdr-error` | `otdr` | Error message produced when the measurement ends with error. |
| `otdr-fiber-break-distance` | `otdr-ptp` | In case the OTDR has clearly identified a fiber break in the last measurement, this attribute indicates the distance of the fiber break (in km). It indicates no |
| `otdr-fiber-check` | `activate` | Command parameter for triggering an automatic OTDR measurement. For otdr specific parameters, refer to Table 83: activate otdr-fiber-check command parameters (p |
| `otdr-fiber-type` | `otdr-ptp` | Specifies the fiber type of the fiber to be measured by OTDR:<br>• not-applicable<br>• auto - Automatic fiber-type (only for OTDR)<br>• not-configured - Fiber-t |
| `otdr-file-prefix` | `activate` | Specifies/indicates the optional user-defined file name prefix of the current OTDR measurement result files. |
| `otdr-file-prefix-requested` | `otdr` | Indicates the requested file name prefix for RD66 and D2ILA OTDR test results. Synced from otdr-file-prefix. Only applicable for RD66 and D2ILA cards. |
| `otdr-ior` | `otdr-ptp` | Specifies the group index of refraction (IOR) of the fiber to be measured by OTDR. |
| `otdr-laser-state` | `otdr` | Indicates the current status of the OTDR laser. |
| `otdr-last-measurement` | `otdr-ptp` | Indicates the last OTDR measurement date and time on the port. |
| `otdr-last-measurement-file` | `otdr-ptp` | The last OTDR measurement the generated .sor file. |
| `otdr-measurement-direction` | `otdr` | Indicates the Scan direction:<br>• not-available: Indicates scan is not running.<br>• tx: Indicates scan is running in the tx direction.<br>• rx: Indicates scan |
| `otdr-measurement-port` | `otdr` | Indicates the OTDR port number where a measurement is currently taking place.<br>• 0 - indicates that the card is not measuring any port;<br>• non-zero - indica |
| `otdr-measurement-speed` | `otdr-ptp` | Specifies the OTDR measurement speed. fast – Fast speed. Approximate acquisition time: 10 seconds. medium – Medium speed. Approximate acquisition time: 15 secon |
| `otdr-measurement-time` | `otdr` | Indicates the time remaining in current measurement running. |
| `otdr-ongoing-measurement-profile` | `otdr` | Displays which pre-defined OTDR measurement profile is in progress:<br>• none: Indicates automatic otdr scan is not running.<br>• short: Indicates baseline otdr |
| `otdr-ptp` | `add`, `delete`, `set`, `show` | See otdr-ptp (p. 860) for more information. |
| `otdr-pulse-width` | `otdr-ptp` | Specifies the OTDR pulse width in nano-seconds (ns). The pulse width determines the dynamic range together with other OTDR measurement parameters. |
| `otdr-range` | `otdr-ptp` | Specifies the distance range in kilometers as a basis to calculate the measurement repetition period. It is recommended that the parameter be set to the actual  |
| `otdr-resolution` | `otdr-ptp` | Specifies the OTDR data sampling resolution. |
| `otdr-state` | `otdr` | Indicates the current status of the OTDR. The status change will trigger change notification: not-available – Status is not available idle – Idle status measuri |
| `ots` | `set`, `show` | See ots (p. 868) for more information. |
| `ots-diagnostics` | `show` | See ots-diagnostics (p. 883) for more information. |
| `ots-r` | `show` | See ots-r (p. 887) for more information. |
| `otu` | `add`, `delete`, `set`, `show` | See otu (p. 896) for more information. |
| `otu-diagnostics` | `set`, `show` | See otu-diagnostics (p. 904) for more information. |
| `otu-name` | `otu` | A system-defined user friendly name for this otu, considering both the type and the rate. Examples: OTUC4, OTUC5i90" |
| `otu-type` | `otu` | The protocol type of the OTUk/OTUCn client. |
| `oui` | `custom-tlv` | The Organization Unique Identifier (OUI) of this TLV. Hexadecimal representation of the 24 bit identifier. |
| `output parameters` | `verify` |  |
| `output-attenuation-actual` | `nmc` | Actual output attenuation. |
| `output-attenuation-compensation-actual` | `nmc`, `nmc-f` | Displays the relative attenuation introduced by the automatic mux control, relative to the manual configurations (band profile, NMC offset and NMC Rx Profile).  |
| `output-attenuation-offset` | `nmc` | Offset factor on attenuation configured by user for egress/ mux WSS. Configurable target output attenuation offset value in the range [-5..5]dB This attribute i |
| `output-attenuation-target` | `nmc` | Configurable target output attenuation. |
| `output-power-mon` | `amplifier` | Monitored aggregate signal output power [dBm]. -99.00 means no power |
| `output-power-mon-with-ase` | `amplifier` | Monitored aggregate total output power including both signal and ASE. -99.00 means no power. |
| `output-voa-actual` | `amplifier` | Actual VOA attenuation at output of the amplifier. i Note: The attribute is not-applicable whenever the card is (required equipped but) not actually equipped, o |
| `output-voa-attenuation` | `amplifier` | Applicable for manual control mode: target VOA attenuation at output of the amplifier (line padding VOA). Applicable if the amplifier function is 'ba' or if amp |
| `oxcon` | `add`, `delete`, `set`, `show` | See oxcon (p. 912) for more information. |
| `packaged-fw` | `show` | Firmware version included in this software-load. See packaged-fw (p. 922) for more information. |
| `packets` | `ipsec-sa-re-key` | The rekeying frequency for the IPsec child security association with the far-end peer based on amount of packets transmitted. |
| `paired-slot-available-bandwidth` | `resources` | Available bandwidth for the paired slot connection. i Note: This parameter is applicable only for SPN2/SPN2C cards that support Paired Slots. |
| `paired-slot-total-bandwidth` | `resources` | Total supported bandwidth for the paired slot connection. i Note: This parameter is applicable only for SPN2/SPN2C cards that support Paired Slots. |
| `parameter` | `pm-parameter`, `pm-threshold`, `pm-threshold-profile` | PM parameter identifier (can be a counter or a gauge). |
| `parameters` | `appctl` | Optional parameters to be passed in the command with max-elements 50. Applicable when command = 'restart' or command = 'exec'. |
| `paraphrase` | `activate-snapshot` | Short description of the database to be activated. |
| `parent` | `comm-channel`, `otu` | For line OTU, indicates the parent facility. |
| `parent-card` | `card` | Name of the parent card, only applicable for subcard(s). |
| `parent-facility` | `nmc` | Parent facility: can be either a Media Channel or an OMS. Only set upon creation. The referenced supporting-card must be part of a Degree (cannot be in an ADG). |
| `parent-odu` | `odu` | For low order ODUs, points to the the parent HO-ODU name. |
| `parent-oms` | `mc` | Parent Media Channel. Only set upon MC creation. The referenced supporting-card must be part of a Degree (cannot be in an ADG). |
| `parent-port` | `port`, `supported-port` | Name of the parent port. Only applicable for sub-ports. |
| `part-number` | `third-party-fw`, `unprovisioned-inventory` | Part number for this equipment. |
| `partner-amplifier` | `amplifier` | The partner amplifier for PAx/ BAX instalments. |
| `passive-shelf-detection` | `chassis` | RepresentsAllows the passive shelf detecntion for the current chassis. When 'true', enables the system to automatically detect the presence or absence of passiv |
| `passphrase` | `download` | To decode encrypted input files. Applicable for filetypes 'local-certificate' or 'peer-certificate'. |
| `password` | `bgp-neighbor`, `download`, `file-server`, `upload`, `user` | Password as TCP-MD5 authentication key in ASCII format. |
| `password-aging-interval` | `user` | This attribute is the Password Aging Interval. |
| `password-expiration-date` | `user` | This attribute shows the password expiration date. |
| `password-hashed` | `user` | Hashed password of the user. It is made of three mandatory fields, where the dollar sign is the field separator. The structure is: $id$salt$encrypted. Only id 6 |
| `password-history-size` | `security-policies` | The number of passwords to store for password reuse checking. |
| `path` | `access-rule`, `download`, `third-party-fw`, `upload` | The target object of the access rule. May be:<br>• XPath of a YANG data node<br>• XPath of a YANG notification • XPath of a YANG RPC or a descendant<br>• Extern |
| `path-segment` | `est-server` | Specifies an optional label added to the EST base url. |
| `pattern-match` | `log-file`, `log-server` | Regex pattern that all entries need to obey. |
| `payload-treatment` | `xcon` | The treatment that this payload will have. Will be automatically derived from the payload-type. transport - payload-treatment for ethernet ctp xcon. transport-w |
| `payload-type` | `xcon` | Indicates a generic, high-level source (from) client payload type of the digital XCON.<br>• 100GBE A generic payload type for all 100GBASE-X Ethernet clients wh |
| `peak-power` | `otdr-ptp` | Specifies the OTDR peak power |
| `peer-address` | `comm-channel` | The IP address on the peer node. This parameter is available only when the mode is L3. |
| `peer-as` | `bgp-neighbor` | AS number of the peer. |
| `peer-certificate` | `ikev2-peer`, `set`, `show` | X509v3 end-entity certificate that represents a trusted 'remote peer' certificate for L1 encryption secure application. See peer-certificate (p. 927) for more i |
| `peer-identity` | `ikev2-peer` | Identity of remote IKE instance. |
| `peer-identity-type` | `ikev2-peer` | Type of peer identity. |
| `pem-over-voltage-threshold` | `chassis` | Over voltage threshold on PEM input feed. |
| `pem-under-voltage-threshold` | `chassis` | Under voltage threshold on PEM input feed. |
| `perceived-severity` | `alarm` | Severity of the alarm. |
| `period` | `pm`, `pm-control-entry`, `pm-profile-entry`, `pm-threshold`, `pm-threshold-profile` | Time period for PM data. |
| `persist` | `commit` | Command parameter for confirming the commit. |
| `persistent` | `scheduled-task`, `task` | If true, this scheduled task will persist a system restart. |
| `pg-control-request` | `protection-group` | Protection group control request. |
| `pg-request` | `protection-group` | The management of protection switching action. |
| `pg-state` | `optical-switch`, `protection-group` | Specifies the current state of the protection group. |
| `phy-mode` | `tom` | Configured Phy Mode. |
| `ping-dest` | `ping` | IP address of the destination of the ICMP ECHO REQUEST datagram. _ |
| `pktsize` | `traceroute` | Specifies the total size of the probing packet. |
| `pm` | `clear`, `show` | Removes or resets PM data. For additional details, refer to pm (p. 934). |
| `pm-catalog` | `show` | PM catalog which contains information on all PM parameters, such as units and type. The list can be retrieved by using show pm-catalog. See pm-catalog (p. 942)  |
| `pm-control` | `show` | Configuration for currently existing resources in the system that support PM data. The list can be retrieved by using show pm-control. See pm-control (p. 943) f |
| `pm-control-entry` | `set`, `show` | PM configuration for one particular resource, for one particular period, direction and location. See pm-control-entry (p. 944) for more information. |
| `pm-filter` | `pm` | Filter to be applied to the PM data.<br>• AID Resource Access Identifier (AID). Identifies an instance within a specific resource type.<br>• data-type Type of P |
| `pm-parameter` | `show` | Catalog information for a single PM parameter. See pm-parameter (p. 946) for more information. |
| `pm-profile` | `show` | PM profile which contains information on all resources that support PM data, together with its related default configuration. Changing this configuration has an |
| `pm-profile-entry` | `set`, `show` | PM configuration per resource type. See pm-profile-entry (p. 951) for more information. |
| `pm-resource` | `set`, `show` | PM configuration per resource instance. See pm-resource (p. 953) for more information. |
| `pm-threshold` | `add`, `delete`, `set`, `show` | See pm-threshold (p. 955) for more information. |
| `pm-threshold-profile` | `set`, `show` | PM configuration per parameter, for this resource type. See pm-threshold-profile (p. 957) for more information. |
| `poll` | `ntp-server-status` | Indicates the polling interval in seconds. |
| `pon` | `inventory`, `unprovisioned-inventory` | Current PON of the equipment. |
| `port` | `cli`, `connect`, `dial-out-server`, `grpc`, `high-speed-monitoring`, `ikev2-peer`, `log-server`, `netconf`, `set`, `show`, `snmp`, `ssh` | The port which listens for CLI access via ssh. |
| `port-a` | `cable-id-path` | Displays the instance identifier of the sled port for the port pair. It identifies one end of the CableId port-pair. The port-A is a port on a CableId capable s |
| `port-a-to-port-z-last-test-status` | `cable-id-path` | Display the cable id test results for endpoints A-Z:<br>• not-verified - cable-id verification is not initiated.<br>• pass - cable-id verification passed.<br>•  |
| `port-a-to-port-z-path-status` | `cable-id-path` | Display the protection path status for endpoints A-Z. It indicates if the port-A has optical continuity to the port-Z:<br>• enabled - all sleds supporting the c |
| `port-description` | `lldp-local-info`, `lldp-neighbor` | The string value used to identify the description of the given port associated with the remote system. |
| `port-direction-convention` | `optical-ptp` | IOA port (PTP) direction convention. ( Only of relevance for ports exposing OTS and OMS-nim, i.e. ILA. ) |
| `port-expansion` | `ops` | Intended for Y-cable expansion. |
| `port-id` | `lldp-local-info`, `lldp-neighbor` | This attribute identifies the port within the LLDP remote system chassis. This value needs to be interpreted according with the associated port-id-subtype, whic |
| `port-id-subtype` | `lldp-local-info`, `lldp-neighbor` | This attribute describes the format of the port-id string. local - Means that the port-id string identifies a locally assigned port ID |
| `port-name` | `advanced-parameter`, `comm-eth`, `connection-ports`, `current-fw`, `inventory`, `port`, `serdes`, `supported-port`, `supported-tom`, `supported-tom-power`, `tom`, `usb` | The name of the port supporting the advance parameter. |
| `port-type` | `port`, `supported-port` | The port type. Each port type supports different features and services. line - Refers to line-side 'colored' CWDM or DWDM optical module/transceiver. tributary  |
| `port-usage` | `port` | Port usage type. Only applicable for line-side ports. It's used to support the interoperation with Photonic Service Switch (PSS) for:<br>• CHM6<br>• CHM7<br>• C |
| `port-z` | `cable-id-path` | Displays the instance identifier of the sled port for the port pair. It identifies one end of the CableId port-pair. The port-Z is a port on a CableId capable s |
| `port-z-to-port-a-last-test-status` | `cable-id-path` | Display the cable id test results for endpoints A-Z:<br>• not-verified - cable-id verification is not initiated.<br>• pass - cable-id verification passed.<br>•  |
| `port-z-to-port-a-path-status` | `cable-id-path` | Display the protection path status for endpoints A-Z. It indicates if the port-Z has optical continuity to the port-A:<br>• enabled - all sleds supporting the c |
| `ports-applicable` | `serdes-template` | The list of ports to which this template is applicable, or 'all' if all ports are to be considered (default). |
| `position-in-rack` | `chassis` | Position of the chassis within the rack. |
| `possible-card-types` | `supported-slot` | List of possible card types in this slot. The list has a maximum of 15 elements. |
| `post-fec-q-sig-deg-hysteresis` | `dsc-group`, `optical-carrier` | Hysteresis to account for raising of the POST-FEC-Q-SIGNAL- DEGRADE alarm. |
| `post-fec-q-sig-deg-threshold` | `dsc-group`, `optical-carrier` | The threshold based on which the POST-FEC-Q-SIGNAL-DEGRADE alarm is raised. |
| `post-login-message` | `ssh` | Welcome message displayed after user login. |
| `post-quantum-preshared-key-scheme` | `ikev2-peer` | Specifies the Post Quantum Preshared key scheme. If this value is set to Disabled, then PPK is disabled. If this option is set to Manual, then PPK must be manua |
| `power-actual` | `ochm`, `spectrum-monitoring` | Currently received power (dBm). The value -99dBm means that:<br>• the power not yet measured (measurement is performed by the OCM at DGE2 card), or<br>• no powe |
| `power-actual-rx` | `nmc`, `nmc-f`, `optical-ptp`, `osc` | Optical Power Received, actual measurement. |
| `power-actual-tx` | `nmc`, `nmc-f`, `optical-ptp`, `osc` | Optical Power Transmitted, actual measurement. |
| `power-before-output-voa` | `amplifier` | Measured optical power before output VOA [dBm]. Applicable if the amplifier function is 'ba' or if amplifier/ supporting-card is ILAx. |
| `power-class-override` | `tom` | Used to override the power class for 3rd party TOM. |
| `power-control-supported` | `supported-chassis` | Whether this chassis supports power control, i.e. the ability to evaluate the power supply currently provided by the PEMs against the configured equipment. A ch |
| `power-draw` | `supported-power-profile` | Power draw of associated equipment when not in low-power. |
| `power-limited` | `chassis` | Indicates if the chassis power consumption is limited by reducing max fan speed. i Note: This attribute is applicable only for 1830 GX G34c chassis. It is edita |
| `power-mode` | `tom` | Specifies if the TOM is configured to function in the low power mode. Value of powered indicates that the TOM operates in the normal power mode, whereas a value |
| `power-profile` | `card` | User configured power draw for this card. i Note: For CHM7 card only the value of high is supported. |
| `power-redundancy` | `chassis` | Configuration of the PEM redundancy mode. (Not applicable to 1830 GX G34c and 1830 GX G31.) one-plus-one - PEM is redundant within a bank of 2 PEMs. one-for-n - |
| `power-threshold-high` | `line-ptp`, `trib-ptp` | The default system threshold (known as 'Overload') that triggers the OPR-OORH alarm (i.e., when the optical power received is greater than this value). Note tha |
| `power-threshold-high-offset` | `line-ptp`, `trib-ptp` | A user configurable attribute that results in the 'effective upper threshold' based on which the system raises the OPR-OORH alarm. The effective threshold will  |
| `power-threshold-low` | `line-ptp`, `trib-ptp` | The default system threshold (known as 'Sensitivity') that triggers the OPR-OORL alarm (i.e., when the optical power received is below this value). Note that th |
| `power-threshold-low-offset` | `line-ptp`, `trib-ptp` | A user configurable attribute that results in the 'effective lower threshold' based on which the system raises the OPR-OORL alarm. The effective threshold will  |
| `ppk-id` | `ikev2-peer` | Specifies the PPK ID. |
| `ppk-key` | `ikev2-peer` | Specifies the PPK Key. i Note: It is recommended to set the ppk-key to 256 bits to provide 128 bits of PQC security. |
| `ppk-required` | `ikev2-peer` | Indicates whether PPK use is mandatory or optional for the IKEv2 peer. i Note: If this parameter is set to true and the peer does not support PPK, the connectio |
| `pre-fec-q-sig-deg-hysteresis` | `dsc-group`, `optical-carrier` | Hysteresis to account for raising of the PRE-FEC-Q-SIGNAL-DEGRADE alarm. |
| `pre-fec-q-sig-deg-threshold` | `dsc-group`, `optical-carrier` | The threshold based on which the PRE-FEC-Q-SIGNAL-DEGRADE alarm is raised. 0 implies threshold crossing alarming disabled. Specific sub-range is per carrier use |
| `pre-login-message` | `ssh` | Welcome message displayed before user login. |
| `pre-shared-key-type` | `ikev2-peer` | The type of pre-shared key scheme. |
| `preemphasis` | `optical-carrier` | Preemphasis of transmitted signal. i Note: This parameter is not configurable for the 1830 GX G30 SPN2, SPN2C, or CHM1R card with line pluggable TOM-400G-Q-DWDM |
| `preemphasis-value` | `optical-carrier` | Preemphasis of transmitted signal. i Note: This parameter is not configurable for the 1830 GX G30 SPN2, SPN2C, or CHM1R card with line pluggable TOM-400G-Q-DWDM |
| `preferred-controller-slot` | `chassis` | Specify a controller slot as the preferred one. The active controller role reverts to the preferred-controller-slot after a reversion timer (5 minutes) has elap |
| `prefix` | `local-subnet`, `ospf-area-range`, `remote-subnet` | IPv4 or IPv6 prefix. The ipv4-prefix type represents an IPv4 address prefix. The prefix length is given by the number following the slash character and must be  |
| `prefix-length` | `ipv6-address` | The length of the subnet prefix. Only valid prefixes are allowed to be configured. i Note: IPv6 /127 subnet is currently supported on unprotected DCN-A (DCN) in |
| `present` | `supported-port`, `usb` | Indicates in which conditions the port is used. Related with multi-chassis environment, where some ports only exist in the Node Controller. Possible values:<br> |
| `present-in-eqpt` | `third-party-fw` | List of resources that contain this version. |
| `previous-output` | `scheduled-task`, `task` | Output of the previous task run. |
| `previous-result` | `scheduled-task`, `task` | Previous task run result. |
| `previous-run` | `scheduled-task`, `task` | Previous task run timestamp. |
| `prf` | `ike-sa-proposal` | A list of protocol proposals when negotiating the IKE SA + with the far-end IKE peer. |
| `priority` | `cert-to-name`, `est-server`, `ipsec-spd-entry`, `ocsp-server`, `ospf-interface`, `ospf-neighbor` | Configure OSPF router priority. On multi-access network this value is for Designated Router (DR) election. The priority is ignored on other interface types. A r |
| `priv-passphrase` | `snmpv3-user` | Specifies the SNMPv3 privacy pass phrase. |
| `priv-protocol` | `snmpv3-user` | Specifies the privacy protocol that the SNMPv3 user being created will use. |
| `privacy-mode` | `syslog` | Flag to enable/disable the GDRP filter. |
| `probe-interval` | `ip-monitoring` | The time between two consecutive pings in seconds. |
| `product` | `third-party-app` | Third party app product. |
| `profile entry` | `alarm-severity-profile` | The profile to be modified. |
| `profile-data` | `profile-control` | Profile data to be inputted. The details are specific of the type of profile being considered, and only for 'write' requests. It is not used with power-profile  |
| `profile-description` | `supported-power-profile` | Description of the profile. |
| `propagate-shutdown` | `optical-carrier` | When the attribute value is set to yes, the transmit laser will be shutdown if the whole service of the direction has signal failure, the function mainly used i |
| `propagate-shutdown-holdoff-timer` | `optical-carrier` | The hold off time of propagate shutdown. i Note: This parameter is not configurable for the SPN2 or SPN2C card. |
| `property` | `show` | See property (p. 974) for more information. |
| `property-name` | `property` | The property to be set. Supported values are fast-client-recovery and max-packet length.<br>• fast-client-recovery - Indicates if fast client signal recovery is |
| `protection` | `delete`, `show` | See protection (p. 976) for more information. |
| `protection-group` | `add`, `protection-switch`, `set`, `show` | See protection-group (p. 977) for more information. |
| `protection-group-name` | `protection-unit` | The name of the protection group |
| `protection-los-threshold` | `optical-switch` | Defines the Signal Fail (SF) threshold for the Protection Path. It is represented in dBm. |
| `protection-mode` | `interface` | Reference to user given protection mode for interface. unknown: Unknown/Transient protection state; output only. protected: Protected by redundant ports. unprot |
| `protection-path-degree` | `optical-switch` | Displays the degree number of the protection path degree. The value of zero denotes that the protection path degree is not associated yet. |
| `protection-pu` | `protection-group` | The protecting pProtection uUnit associated with the protection group. |
| `protection-state` | `interface` | Reference to current state of protection of interface so by default its unknown. unknown: Unknown/Transient protection state; output only. protected: Protected  |
| `protection-switch-threshold` | `optical-switch` | Defines the Signal Degrade (SD) threshold for the Protection Path. It is represented in dBm. |
| `protection-type` | `optical-switch`, `protection-group`, `xcon` | Represents the protection type this PG has. |
| `protection-unit` | `set`, `show` | See protection-unit (p. 985) for more information. |
| `protection-unit-name` | `protection-unit` | The name of the protection unit |
| `protocol` | `ace`, `dial-out-server`, `file-server` | The internet protocol number. |
| `protocol-id` | `ike-sa-proposal`, `ipsec-sa-proposal` | The protocol ID (type) for which the IKE proposal applies to. |
| `protocol-supported` | `aaa-server` | Specifies the protocol used for AAA. |
| `protocols` | `show` | Container of management protocol objects. The list can be retrieved by using show protocols. See protocols (p. 987) for more information. |
| `proxy-arp-enabled` | `interface` | Controls whether or not Proxy ARP is to be enabled on the interface. This attribute is only applicable to the DCN interface. |
| `psd-actual` | `spectrum-monitoring` | Currently calculated PSD. The Power Spectral Density does not depend on the spectra width. |
| `psd-actual-rx` | `nmc` | Calculated by the system from power-actual-rx (i.e. dependent on spectrum width). |
| `psd-actual-tx` | `nmc` | Calculated by the system from power-actual-tx (i.e. dependent on spectrum width). |
| `psk-ascii` | `ikev2-peer` | Plain-text ASCII value for the PSK. |
| `psk-configured-timestamp` | `ikev2-peer`, `macsec-mka` | Local NE timestamp when the PSK was configured. |
| `psk-expiration-warning` | `ikev2-peer` | An absolute time duration (in days) at which the network element provides a warning when the PSK is about to expire. |
| `psk-hex` | `ikev2-peer` | Binary, hexadecimal value for the PSK. |
| `psk-lifetime` | `ikev2-peer` | Absolute time duration in days after which the PSK will expire. |
| `psk-lifetime-enable` | `macsec-mka` | Indicates whether PSK lifetime notification is enabled or disabled |
| `psk-lifetimepsk-expiration-warning` | `macsec-mka` | Absolute time duration in days after which the PSK will expire |
| `ptp-type` | `optical-ptp` | Type of Optical PTP. |
| `public-key` | `ssh-authorized-key`, `ssh-host-key`, `ssh-known-host` | SSHv2 (OpenSSH Portable) host public key component encoded in PEM format: &lt;key type&gt;&lt;SPACE&gt;...base64 encoded OpenSSH public key....&lt;SPACE&gt;&lt; |
| `public-key-algorithm` | `ssh-authorized-key`, `ssh-host-key`, `ssh-known-host` | The type of host key algorithm in use. |
| `public-key-length` | `local-certificate`, `peer-certificate`, `trusted-certificate` | X509v3 certificate public key algorithm and supported key length. |
| `public-key-type` | `local-certificate`, `peer-certificate`, `trusted-certificate` | Public/private key type for X509v3 certificate. |
| `pump` | `show` | See pump (p. 992) for more information. |
| `pump-enable` | `ase-idler-source` | ASE Idler source enabling. |
| `pump-id` | `pump-power` | The 'pump-id' is an integer identifying the number of the pump. |
| `pump-power` | `show` | See pump-power (p. 994) for more information. |
| `pump-state` | `amplifier`, `ase-idler-source` | The amplifier's pump working status. |
| `rack-name` | `chassis` | User-defined rack name (within the location). |
| `raman-coefficient-rx` | `ots` | Raman coefficient per Terahertz (in dB/THz/W). Required for tilt control (if tilt-control-mode = auto). Configuration mode depends on tilt-control-mode. |
| `raman-coefficient-tx` | `ots` | Raman coefficient per Terahertz (in dB/THz/W). Since different transmission bands are supported, it is simpler to enter this parameter \n independent of the tra |
| `raman-osc-gain` | `amplifier` | Required when Raman backward pumping is deployed. The value is entered by the user in case the Raman card and pre-amplifier are in different NEs, otherwise it i |
| `raman-signal-gain` | `amplifier` | Raman Gain of C-Band (signal).<br>• If there is a fiber-connection from/to Raman, the raman-signal-gain at amplifier needs to be appropriately configured autono |
| `raman-state` | `amplifier-raman` | State of the current Raman state/ amplifier. • disabled: Disabled local and remote Raman.<br>• disabled-from-remote: Disabled locally because of remote Raman di |
| `rate` | `comm-eth`, `dsc-group`, `eth-zr`, `flexo-group`, `nw-xconnect`, `oc`, `odu`, `optical-carrier`, `otu` | Carried signal basic rate class. |
| `re-auth-fail-policy` | `ikev2-peer` | Bring down the data path encrypted service if re-authentication was unsuccessful. |
| `re-auth-frequency` | `ikev2-peer` | The re-authentication frequency for the IKE security association with the far-end IKE peer. Range and default values may be context-specific. |
| `re-auth-traffic-kill-offset` | `ikev2-peer` | If the re-authentication fail policy is set to KILL-TRAFFIC, this attribute indicates the amount of time the system waits before killing all Child SAs that are  |
| `re-key-fail-policy` | `ikev2-peer`, `secure-entity` | If the re-key fail policy is set to KILL- TRAFFIC, this attribute indicates the amount of time the system waits before killing all encrypted data security assoc |
| `re-key-frequency` | `ikev2-peer`, `secure-entity` | re-key frequency for the IKE security association with the far-end IKE peer. Range and default values may be context-specific. |
| `re-key-traffic-kill-offset` | `ikev2-peer` | If the re-key fail policy is set to KILL- TRAFFIC, this attribute indicates the amount of time the system waits before killing all encrypted data security assoc |
| `reach` | `ntp-server-status` | Indicates the reachability of the configured server. This is an 8-bit shift register with the most recent probe in the 2^0 position. The value 377 indicates tha |
| `read-default` | `authorization` | In case only user configured access-rules are used, this policy defines what is the action to use if a given read operation does not match any rule. Read access |
| `real-time-data-last-reset` | `pm-resource` | Date and time of the last real time data reset for this resource. If the data was never reset, this is the date and time of this resource's creation. |
| `real-time-supervision` | `pm-resource` | Real-time data supervision for this resource. |
| `reboot-count` | `sw-service` | The number of times a service has restarted. |
| `recover-mode` | `clear`, `ne` | Clears recover-mode flag For additional details, refer to recover-mode (p. 1004). |
| `recover-mode-reason` | `ne` | Reason for recover mode. Available only when ne is in recover mode. |
| `recovery` | `show` | See recovery (p. 1005) for more information. |
| `redundancy-standby-status` | `controller-card` | State of the controller redundancy. |
| `redundancy-state` | `comm-eth` | Redundancy state of the comm port: none - No redundancy. active - Port is active. standby - Port is on standby. |
| `redundancy-status` | `controller-card` | The redundancy state of the controller card. |
| `refid` | `ntp-server-status` | Reference clock type or address for the peer. |
| `refresh-interval` | `cdp` | Defines when CRL should be refreshed/updated. |
| `related-dial-out-server` | `current-subscription` | Identifier of the subscription dial-out server address. Only applicable to dial-out based subscriptions. |
| `related-session-id` | `current-subscription` | Identifier of the telemetry subscription session. |
| `reliable-cp` | `protection-group` | The reliable connection point associated with the protection group. |
| `remaining-valid-signal-time` | `line-ptp`, `super-channel`, `trib-ptp` | Actual remaining time for this facility to be automatically enabled by the auto-in-service mechanism. |
| `remaining-wtr` | `protection-group` | Specifies the remaining time in WTR timer. Only applicable in revertive mode. |
| `remote-address` | `bgp-neighbor`, `bgp-network` | Address of the BGP peer. |
| `remote-carrier-id` | `carrier-neighbor` | AID of the remote carrier connected to the local carrier. Implies a specific remote port id |
| `remote-logging-switch` | `syslog` | Flag to enable remote logging switch. If false, disable all remote logging destinations. |
| `remote-ports` | `add`, `delete`, `show` | See remote-ports (p. 1009) for more information. |
| `remote-secure-entity` | `secure-entity` | AID of the remote optical carrier (for 1830 GX G40) or the remote ODU (for 1830 GX G30) or the remote OTUFlex (CHM7/CHM7x L1 Service encryption) |
| `remote-subnet` | `add`, `delete`, `show` | See remote-subnet (p. 1011) for more information. |
| `replay-protection` | `macsec-entity` | Replay protection enable/disable |
| `replay-protection-window` | `macsec-entity` | Number of packets to consider for replay protection window |
| `reported-time` | `alarm` | Occurrence timestamp for the alarm. |
| `required-fiber-type-rx` | `ots-r` | The required Fiber Type on the DWDM Line, with reference for the Rx fiber. Only of relevance if control-mode = auto and when there is no fiber-connection. Fiber |
| `required-subtype` | `card`, `chassis`, `tom` | The subtype of the card. Required sub-type field is applicable for the following cards:<br>• FAN: counter-rotating, single-rotor<br>• PEM: AC, DC<br>• PAxOFP2:  |
| `required-type` | `card`, `chassis`, `tom` | The card type. The required type filed is applicable to the following cards:<br>• BAXOFP2<br>• BLANK<br>• BLANK2<br>• CAD10A<br>• CAD16AOFP2<br>• CDC4D4OFP2<br> |
| `requires-blank-when-empty` | `supported-slot` | Whether this slot requires a BLANK filler card when empty. |
| `reserved-power-draw` | `chassis` | Worst case power drawn by the chassis including power reserved for commons and power drawn by provisioned equipment. |
| `reset-power` | `supported-slot` | Reset power consumption for this card, at 55ºC, in W units. |
| `resource` | `activate`, `alarm`, `get-conditions`, `manual-switchover`, `pm`, `pm-control-entry`, `pm-profile-entry`, `pm-resource`, `pm-threshold`, `pm-threshold-profile`, `upgrade-status` | The object to be manually switched. |
| `resource-id` | `restart` | Entity to restart. |
| `resource-mode` | `flexo` | Resource mode configuration to support (ADM) add-drop or (XC) add-drop with regen |
| `resource-type` | `alarm`, `alarm-inventory`, `alarm-severity-entry`, `get-conditions`, `pm`, `pm-resource` | Type of resource. |
| `resources` | `show` | See resources (p. 1013) for more information. |
| `restart-behavior` | `system` | The behavior of the restart (restart or shutdown). |
| `restart-timer` | `comm-channel` | Specifies the restart timer of the PPP protocol in seconds. This parameter is available only when the mode is L3. |
| `restart-type` | `activate` | Specifies the type of system restart. |
| `restconf` | `protocols`, `set`, `show` | Set of attributes of the configuration of the RESTCONF management protocol. See restconf (p. 1020) for more information. |
| `restore-from-chassis-storage` | `recovery` | Type of system recovery from chassis storage:<br>• disabled - Chassis storage is not used for restoration in this NE.<br>• auto-restore - SW and DB are stored o |
| `restore-status` | `recovery` | Current state of the restoration:<br>• init - Provisioning service is starting<br>• image-install-in-progress - Installing backup image<br>• db-restore-in-progr |
| `result` | `file` | The file operation result. |
| `retransmission-interval` | `ospf-interface` | Specifies the Retransmission Interval in seconds. |
| `retry` | `aaa-server`, `dial-out-server` | Specifies the number of attempted Access-Request messages to a single AAA server before failing authentication. |
| `retry-policy` | `dial-out-server` | The retry policy after a timeout. |
| `reversion-mode` | `optical-switch`, `protection-group` | Enable or disable automatic reversion protection status after wtr-time delay. |
| `revocation-mode` | `local-certificate`, `peer-certificate`, `trusted-certificate` | Controls how the revocation status of the certificate is determined. |
| `rib` | `show` | List of RIB entries. See rib (p. 1022) for more information. |
| `rib-name` | `next-hop`, `rib`, `route` | The name of the RIB. |
| `role` | `ops`, `ospf-neighbor`, `protection-unit` | Protection unit role |
| `role-supported` | `aaa-server` | The configured roles for the AAA server. |
| `root-fingerprint` | `est-ca` | Verifies the identity of the Root CA using a SHA-256 or SHA-512 hash to ensure a secure initial connection for EST certificate enrollment. |
| `root-password` | `security-policies` | The password of the root user. The minimum length of the root password is 1 character. |
| `route` | `show` | List of system routes from various sources, such as dynamic protocols and static route. See route (p. 1024) for more information. |
| `router-dead-interval` | `ospf-interface` | Specifies the Router Dead Interval in seconds. |
| `router-id` | `bgp-instance`, `ospf-instance`, `ospf-neighbor` | Specifies the router ID. 0.0.0.0/0 is not supported for IPv4 and 0::0.0 is not supported for IPv6. |
| `router-id-mode` | `bgp-instance`, `ospf-instance` | Flag to indicate router-id is loopback IP. |
| `routing` | `show` | Container of routing subsystem. The list can be retrieved by using show routing. See routing (p. 1026) for more information. |
| `rsc` | `show` | See rsc (p. 1027) for more information. |
| `rsc-power-rx` | `rsc` | The received Pilot Tone integrated power. |
| `rsc-power-tx` | `rsc` | The transmitted Pilot Tone integrated power. |
| `rx-attenuation` | `optical-carrier` | Supports configurable optical attenuation at receiver side which is based on the hardware capability on the port. i Note: This parameter is not configurable for |
| `rx-dapi` | `odu-diagnostics`, `otu-diagnostics` | The received DAPI bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. |
| `rx-dapi-hex` | `odu-diagnostics`, `otu-diagnostics` | Received DAPI in HEX. |
| `rx-fiber-type` | `submarine-link` | Indicates the Rx fiber type of the link. |
| `rx-frequency` | `optical-carrier` | The rx laser frequency. A super set for line and client side carrier frequency, specific sub-range is depend on application. 0 represents a non-initialized freq |
| `rx-msi` | `odu` | Received and accepted MSI values (up to 80), including a valid/invalid indication (valid if acceptance process successful, invalid if not; when invalid the last |
| `rx-msi-hex` | `odu` | Received and accepted MSI hex values (up to 80) (if acceptance process was not successful the last accepted MSI set is shown). |
| `rx-operator` | `odu-diagnostics`, `ots-diagnostics`, `otu-diagnostics` | The value of this attribute is used to allow the user to verify the NE connected on the other end of the fiber. The value is the trail trace identifier of the N |
| `rx-operator-hex` | `odu-diagnostics`, `otu-diagnostics` | Received operator in HEX. |
| `rx-payload-type` | `odu` | Received payload-type of ODU. |
| `rx-sapi` | `odu-diagnostics`, `otu-diagnostics` | The received SAPI bytes as an ASCII string; will not be available if bytes cannot be encoded as a printable string. |
| `rx-sapi-hex` | `odu-diagnostics`, `otu-diagnostics` | Received SAPI in HEX. |
| `rx-tti` | `odu-diagnostics`, `otu-diagnostics` | Received TTI - Received by this facility from the far-end remote facility. |
| `rx-tti-hex` | `odu-diagnostics`, `otu-diagnostics` | Received TTI in HEX. |
| `sak-rekey-interval` | `mka-policy` | Secure Association Key(SAK) rekey interval in seconds |
| `sample-interval` | `subscription-path` | Time in milliseconds between the device's sample of a telemetry data source. For example, setting this to 2000 would require the local device to collect the tel |
| `san` | `csr-gen`, `est` | The certificate SAN (Subject Alternate Name) fields. SANs are specified as Type-Value comma separated list. The only valid types are 'IP' and 'DNS'. |
| `sanity-check-override` | `activate`, `activate-snapshot`, `download` | Action to override the sanity check. |
| `scheduled-tasks` | `show` | Container of individual user-configurable scheduled commands. The list can be retrieved by using show scheduled-tasks. See scheduled-task (p. 1036) for more inf |
| `sci-rx` | `sc-rx` | Receiving Secure Channel Identifier hex string. |
| `sci-tx` | `sc-tx` | Transmitting Secure Channel Identifier hex string. |
| `scope` | `external-fiber-connection`, `ikev2-local-instance` | Represents the scope of the external-fiber-connection:<br>• general-purpose - indicates the general use of external-fiber-connection to represent connectivity b |
| `script` | `activate`, `database`, `download`, `prepare-upgrade` | The script to execute after clearing the database. The script parameter may be an absolute path for a .cli file, or just the filename if the script is present i |
| `script-dir` | `cli` | Location in the filesystem where CLI scripts are stored. |
| `script-name` | `run` | The script name is a relative path to the script directory. |
| `search` | `dns` | DNS-search-suffix name. It must contain at least a single dot. To clear the value, set the string to an empty string. |
| `secure-application` | `set`, `show` | List of the secured applications and parameters. See secure-application (p. 1039) for more information. |
| `secure-applications` | `set`, `show` | List of the secured applications which use X509v3 certificate as its digital identity. See secure-application (p. 1039) for more information. |
| `secure-entity` | `add`, `delete`, `re-key`, `set`, `show` | See secure-entity (p. 1042) for more information. |
| `secure-entity-sa-proposal` | `show` | See secure-entity-sa-proposal (p. 1045) for more information. |
| `secure-mode` | `security-policies` | If enabled, non-secure protocols are not supported. If disabled, non-secure protocols can be used, including: - HTTP protocol for file transfer, REST API, or an |
| `secure-session` | `bgp-neighbor` | Authentication method of the session to the peer. |
| `security` | `show` | Top level security container. See security (p. 1047) for more information. |
| `security-policies` | `delete`, `set`, `show` | See security-policies (p. 1048) for more information. |
| `security-policy-database` | `add`, `set`, `show` | See security-policy-database (p. 1060) for more information. |
| `segment-list` | `submarine-link` | Defines the list of fiber segments that make up the submarine link. |
| `self-signed` | `local-certificate`, `peer-certificate` | True if certificate is self-signed (does not have a trust chain). |
| `sensitive-data` | `log-file`, `log-server` | Whether the local file has logs include sensitive data. |
| `sequence-id` | `access-rule`, `access-rule-list`, `ace`, `template` | Represents id of this template entry, it is used to define the order in which templates are processed. Lower number ids are processed first. |
| `serdes` | `add`, `delete`, `set`, `show` | See serdes (p. 1062) for more information. |
| `serdes-name` | `serdes` | Name of the advanced parameter. |
| `serdes-template` | `show` | See serdes-template (p. 1064) for more information. |
| `serdes-template-entry` | `show` | See serdes-template-entry (p. 1066) for more information. |
| `serial-console` | `protocols`, `set`, `show` | Global configuration of all serial console ports in the system. See serial-console (p. 1068) for more information. |
| `serial-number` | `inventory`, `local-certificate`, `peer-certificate`, `trusted-certificate`, `unprovisioned-inventory` | Serial number of the equipment. |
| `server-address` | `aaa-server`, `dhcp-relay`, `est-server`, `file-server` | DHCP server ip-addresses; when enabled at least one IP address should be configured. |
| `server-name` | `aaa-server`, `aaa-statistics`, `est-server` | Name of the server |
| `server-port` | `aaa-server`, `est-server`, `file-server` | The AAA server port number. |
| `server-port-accounting` | `aaa-server` | AAA server accounting port number. |
| `server-port-authentication` | `aaa-server` | AAA server authentication port number. |
| `server-priority` | `aaa-server` | This is used to sort the servers in the order of precedence. If not provided, the server priority will be set to the lowest precedence (highest number) already  |
| `service-affecting` | `alarm`, `alarm-inventory`, `alarm-severity-entry` | Information on whether this alarm is service affecting or not. |
| `service-impact` | `golden-advanced-parameter` | Identifies if applying this parameter change causes service impact. If it is service-affecting, users must perform an admin lock/ maintenance operation or other |
| `service-mode` | `ethernet`, `fc`, `oc`, `odu`, `otu`, `stm` | This attribute is to align with legacy Nokia OTN virtualization attribute (SM). The 'service mode' attribute indicates the OTUk/OTUCn client's treatment/ proces |
| `service-mode-qualifier` | `ethernet`, `fc`, `oc`, `odu`, `otu`, `stm` | This attribute is to align with legacy Nokia OTN virtualization attribute (SMQ). The 'service mode qualifier' attribute further adds to the 'service mode' attri |
| `service-name` | `sw-control-rule` | Name of the service to be monitored. |
| `service-type` | `line-ptp`, `trib-ptp` | service-type to provision line side service. CHM1R:<br>• DP-16QAM-400G-OpenZR+<br>• DP-16QAM-400G<br>• DP-16QAM-E-400G<br>• DP-8QAM-300G<br>• DP-QPSK-200G<br>•  |
| `services` | `show` | Services of multiple types commissioned in this NE. The list can be retrieved by using show services. |
| `session` | `show` | List of currently established management layer sessions. See session (p. 1069) for more information. |
| `session-id` | `cli-session-config`, `kill-session`, `session`, `transfer-status` | CLI session ID. |
| `session-protocol` | `current-subscription`, `session` | Indicates which protocol has been used to establish the session. |
| `session-state` | `bgp-neighbor` | Current BGP Session state in ASCII format. |
| `session-type` | `current-subscription`, `session` | Session type. |
| `session-user` | `session` | User name associated with this session. |
| `session-user-name` | `transfer-status` | Last transfer session-user-name. |
| `severity` | `alarm-severity-entry`, `alarm-severity-profile`, `log-console-facility-filter`, `log-file-facility-filter`, `log-server-facility-filter` | Configured severity of the current resource type. |
| `shared-secret` | `aaa-server` | The shared secret of the aaa server. The shared secret will be displayed as *. |
| `show-alarm-columns` | `cli` | Columns to display in the output of 'show alarm' CLI command. Possible values:<br>• list of columns, separated by a comma ','.<br>• default-columns, show the pr |
| `signature` | `downloaded-image` | Downloaded software image file signature. |
| `signature-algorithm` | `ISK`, `key-replacement-package` | Signature Algorithm. |
| `signature-gen-time` | `ISK`, `key-replacement-package` | Signature Generation Time. |
| `signature-hash-algorithm` | `crl`, `csr-gen`, `est`, `local-certificate`, `peer-certificate`, `trusted-certificate` | Hash algorithm to be used. Default value depends on the selected key-algorithm. |
| `signature-hash-scheme` | `ISK`, `key-replacement-package` | Hashing Scheme |
| `signature-key-type` | `crl`, `local-certificate`, `peer-certificate`, `trusted-certificate` | Signature algorithm key type for certificate/CRL. |
| `signature-payload` | `ISK`, `key-replacement-package` | Signature Payload. |
| `slot` | `show` | Slot equipment holder details. See slot (p. 1132) for more information. |
| `slot-horizontal-position` | `supported-slot` | Position of the slot horizontally in the chassis within the current RU, counting from the left of the chassis. For back slots, the position is counted also from |
| `slot-location` | `supported-slot` | Physical location of the slot in the chassis. |
| `slot-name` | `card`, `slot`, `supported-slot`, `unprovisioned-inventory` | Slot where this card is located. |
| `slot-vertical-position` | `supported-slot` | Position of the slot vertically in the chassis, counting from the top of the chassis, in RUs. Example: position 3 means third RU starting from the top of the ch |
| `slot-width` | `mc`, `mc-f` | Slot width, as calculated by the system, from upper-frequency - lower-frequency. |
| `slot-width-granularity` | `degree` | Width of a slot (measured in GHz). |
| `sndp-enabled` | `sndp` | This is a switch to control the sndp feature. |
| `snmp` | `protocols`, `set`, `show` | Set of attributes of the configuration of the SNMP management protocol. See snmp (p. 1136) for more information. |
| `snmp-community` | `add`, `delete`, `set`, `show` | List of SNMP Community Strings. Note: trap-community-string is located in the snmp-target object. See snmp-community (p. 1138) for more information. |
| `snmp-engine-id` | `snmp` | SNMP EngineID of the NE. The EngineID will follow the EngineID format 3 defined in RFC3411. The MAC address in the Engine ID will be the first MAC address of th |
| `snmp-target` | `add`, `delete`, `set`, `show` | List of SNMP targets (trap listeners). See snmp-target (p. 1140) for more information. |
| `snmp-version` | `snmp-target` | The SNMP version. |
| `snmpv3-user` | `add`, `delete`, `set`, `show`, `snmp-target` | See snmpv3-user (p. 1143) for more information. |
| `snmpv3-user-name` | `snmpv3-user` | The SNMP Version 3 user name |
| `software-load` | `show` | Information on the Software Load present in the system. See software-load (p. 1145) for more information. |
| `software-load-active` | `sw-management` | Shows active software. |
| `software-load-inactive` | `sw-management` | Shows inactive software |
| `software-location` | `show` | Software load information associated to each of the equipment. See software-location (p. 1148) for more information. |
| `software-location&lt;shelf&gt;-&lt;slot&gt;` | `sw-management` | Show software loads by shelf and slot. |
| `sop-data-collection` | `optical-carrier` | Controls enabling/disabling sop data collection, providing the collection interval in ms. |
| `sop-tracking-mode` | `golden-carrier-mode` | The optical transport SOP tracking mode this mode is optimized for. |
| `source` | `download`, `oxcon`, `xcon` | The source end-point required for OXcon creation. |
| `source-address` | `syslog` | Source address or hostname to inserted in HOST field of log message. |
| `source-facilities` | `log-console`, `log-file`, `log-server` | List of syslog facilities used in this configuration. |
| `source-ip` | `aaa-server` | Source IP address used for RADIUS communications. |
| `source-ip-address` | `ace` | Specifies the source IP of this filter. |
| `source-lower-port` | `ace` | The lower bound of the source Layer 4 TCP/UDP port number. |
| `source-protocol` | `route` | Source protocol for the route entry. |
| `source-upper-port` | `ace` | The upper port bound of the source Layer 4 TCP/UDP port number. |
| `span-loss-aging-margin-rx` | `ots` | Span loss aging margin. It is used by system for defining value of NMC input power range and span loss high alarm. |
| `span-loss-alarm-threshold` | `ots` | The threshold for span loss alarm. The value is autonomously set by the system. It is persistent, that is, it is kept after warm-/ cold-boot. In span-loss-refer |
| `span-loss-at-amplifier` | `ots-r` | The Span Loss detected at amplifier, when there is a fiber-connection from/ RPB to the amplifier. |
| `span-loss-baseline-rx` | `ots` | The Rx span loss baseline (dB). Only applicable for HSC-OLS. |
| `span-loss-baseline-tx` | `ots` | The Tx span loss baseline (dB). Only applicable for HSC-OLS. |
| `span-loss-control` | `amplifier` | Span Loss Control configuration:<br>• enabled: perform automatic Span Loss Control<br>• disabled: no Span Loss Control. This configuration is of particular rele |
| `span-loss-derived-rx` | `ots` | Measured span loss (based on OSC). The value includes the losses external to the fiber. A value of 99dB means OLOS. Only of relevance when OSC channel exists. |
| `span-loss-derived-tx` | `ots` | Measured span loss from the downstream NE received by the upstream NE via OSC. The value includes the losses external to the fiber. A value of 99dB means OLOS.  |
| `span-loss-receive` | `ots`, `ots-r` | Fiber loss on the receiver side (in dB). The configuration of the value is relevant for Raman control. The default value 0 means there is no loss: when Raman is |
| `span-loss-reference` | `ots` | Determines the span-loss source being currently used by the system to calculate automatic target OXCon powers: • If set to measured assumes downstream measured  |
| `span-loss-transmit` | `ots` | Fiber loss on the transmitter side (in dB). This is only the loss of the fiber. Additional loss such as coming from patch panel is entered via the external-atte |
| `special-next-hop` | `ipv4-static-route`, `ipv6-static-route`, `route` | The routes to be advertised to external AS must exist in the forwarding table installed by an Interior Gateway Protocol (IGP) such as OSPF or static routes, but |
| `spectral-bandwidth` | `optical-carrier` | Spectral bandwidth associated with this carrier(s). |
| `spectrum` | `show` | See spectrum (p. 1149) for more information. |
| `spectrum-control` | `show` | See spectrum-control (p. 1153) for more information. |
| `spectrum-monitoring` | `show` | See spectrum-monitoring (p. 1156) for more information. |
| `speed` | `ethernet`, `fc`, `oc`, `stm` | The speed/rate of the signal in Gbit/s. |
| `spi` | `auth-key`, `ospfv3-ipsec-security-association` | A unique security parameter index (SPI) for this SA. |
| `src-card-name` | `external-fiber-connection`, `submarine-link` | Source card identification. |
| `src-chassis` | `nct-connection` | The identifier of the chassis where the source port is located. If it is a commissioned chassis, it will be the AID of the chassis. If it is an unprovisioned ch |
| `src-chassis-state` | `nct-connection` | The state of the src-chassis |
| `src-node-id` | `external-fiber-connection`, `submarine-link` | Source node-id. Should be logically the same as 'ne-name', although there is no SYSTEM business logic to correct this. |
| `src-port` | `fiber-connection`, `nct-connection` | Source Port instance. |
| `src-port-name` | `external-fiber-connection`, `submarine-link` | Source port identification. |
| `src-time-slots` | `xcon` | Time-slots allocated to the source lo-oduj in this xcon. Not applicable if source facility is not an ODU facility. Value can be:<br>• omitted/empty - in which c |
| `ssh` | `protocols`, `set`, `show` | Control attributes of ssh access. See ssh (p. 1159) for more information. |
| `ssh-authentication-method` | `security-policies` | "The method used to authenticate user for SSH access. Note: For two-factor authentication, use public-key method and employ PIN/password-protected hardware devi |
| `ssh-authorized-key` | `add`, `delete`, `set`, `show` | SSHv2 authorized keys entry. Each authorized key entry contains a trusted remote public key for SSHv2 server side host authentication. See ssh-authorized-key (p |
| `ssh-ciphers` | `security-policies` | Allowed symmetric ciphers for SSH. |
| `ssh-host-key` | `show` | Global (for server and client side SSHv2 based apps) SSHv2 host keys. See ssh-host-key (p. 1163) for more information. |
| `ssh-host-key-algorithms` | `security-policies` | Allowed host key algorithms for SSH. |
| `ssh-key-exchanges` | `security-policies` | Allowed key exchange algorithms for SSH. |
| `ssh-known-host` | `add`, `delete`, `set`, `show` | SSHv2 known hosts entry. See ssh-known-host (p. 1168) for more information. |
| `ssh-macs` | `security-policies` | Allowed message authentication code algorithms for SSH. |
| `ssh-public-key-algorithms` | `security-policies` | Allowed public key algorithms for SSH. |
| `ssh-strict-host-key-checking` | `security-policies` | Specify the strictness of remote ssh/sftp/scp host identity checking. |
| `start` | `local-ports`, `remote-ports` | The values for the starting port. |
| `start-time` | `scheduled-task`, `task`, `upgrade-status` | Timestamp to start the task. For periodic tasks, this is the timestamp for the first trigger of the task. If not provided, uses current time as star time. |
| `started-time` | `ikev2-local-instance` | Local system timestamp when this IKEv2 instance was started. |
| `state` | `manifest`, `ospf-neighbor`, `protection-unit`, `sc-rx`, `sc-tx`, `set-alarm-state`, `sw-component`, `sw-container`, `sw-service`, `sw-subcomponent`, `third-party-app` | The state of the protection-unit. |
| `state-details` | `sw-service` | Brief description of the service status. |
| `static-info-in-notifs` | `netconf` | List of YANG identifiers that are statically included in notifications. If they are present in objects that are notified. Maximum elements is 10. Applicable for |
| `static-route` | `ip-monitoring` | The list of connected static routes for this Monitoring instance. |
| `statistics` | `clear` | Clears event counters (statistics) for the specified objects. For additional details, refer to statistics (p. 1171). |
| `status` | `advanced-parameter`, `console`, `crl`, `golden-carrier-mode`, `led`, `local-certificate`, `peer-certificate`, `secure-application`, `serdes`, `supported-carrier-mode`, `trusted-certificate`, `upgrade-status` | The current state of the advanced parameter. |
| `step` | `upgrade-status` | The identifier for the current upgrade step. |
| `step-start-time` | `upgrade-status` | The timestamp at which the current upgrade step was initiated. |
| `stm` | `set`, `show` | See stm (p. 1185) for more information. |
| `stm-type` | `stm` | The type of SDH signal (STM-N). |
| `stop` | `local-ports`, `remote-ports` | The values for the stopping port. If the stopping port is not set, the system assumes the value is 0. However, the value 0 is only accepted by the system if the |
| `stratum` | `ntp-server-status` | Indicates the stratum of the remote peer. |
| `strict-password-check` | `security-policies` | If enabled, ensures the strict password complexity rules. Including: - minimum length of 8 characters - at least one lower case letter (a-z) - at least one uppe |
| `string` | `pm-catalog` | The catalog name. |
| `sub-component` | `restart` | Card HW or SW sub-component to restart. |
| `sub-component-name` | `sub-component` | The name of the the sub-component. |
| `sub-type` | `simulate` | Card subtype. |
| `subcard-list` | `card` | List of sub-cards associated with this card. Only applicable for carrier cards. |
| `subcommands` | `clear` | Description |
| `subject` | `cert-gen`, `csr-gen`, `est` | Full certificate subject name. When generating the self-signed certificate, it needs to specify either subject or common-name. |
| `subject-alternative-names` | `local-certificate`, `peer-certificate` | Contains a list of subject alternative name(X509v3 extension SAN) entries separated by &lt;SPACE&gt;&lt;PIPE&gt;&lt;SPACE&gt; delimiters (e.g. 'URI:https:// www |
| `subject-name` | `local-certificate`, `peer-certificate`, `trusted-certificate` | A custom type to represent X.500 distinguished names (DN). The subject field identifies the entity associated with the public key stored in the subject public k |
| `subport-list` | `port`, `supported-port` | List of sub-ports associated with this port. Only applicable when this port is a parent port. |
| `subscription-name` | `current-subscription`, `subscription-path` | User configured identifier of the telemetry subscription. This value is used primarily for subscriptions configured locally on the network element. For dial-in  |
| `subscription-path` | `subscription-path` | Specifies a path in the data model path corresponding to the data in the message. |
| `subscription-path-id` | `subscription-path` | Identifier of the single subscription path in the subscription list. |
| `subscription-path-mode` | `subscription-path` | How subscription updates are sent. |
| `subscription-path-origin` | `subscription-path` | Specifies the schema tree in order to disambiguate the path. |
| `subscription-paths` | `show` | See subscription-path (p. 1196) for more information. |
| `subscriptions` | `show` | This container holds information for telemetry subscriptions. The list can be retrieved by using show subscriptions. See subscriptions (p. 1199) for more inform |
| `subslot-name` | `card` | Subslot where this card is located, e.g. 1-2.3 (slot 2, subslot 3). 'subslot-name' can only be set on (sub)card creation. |
| `subtype` | `custom-tlv`, `subtype-constraint` | The sub-type identifier of the TLV in the scope of the OUI The firmware name |
| `subtype-constraint` | `show` | See subtype-constraint (p. 1200) for more information. |
| `super-channel` | `set`, `show` | Unified channel of optical carriers. Can have many optical channels. See fiber-connection (p. 489) for more information. |
| `super-channel-group` | `set`, `show` | See super-channel-group (p. 1206) for more information. |
| `support-serdes-config` | `supported-card` | If true, it means this card-type allows the user to configure 3rd Party TOM SerDes values. If false, the card has no need for such customization. |
| `support-third-party-toms` | `tom-type` | Whether this TOM type accepts third party TOMs in addition to supported Nokia TOMs. |
| `supported carriers` | `resources` | A list of carriers that are bound to this resource. |
| `supported-applications` | `subtype-constraint` | List of applications supported by this subtype. If this list is empty, then this constraint is not applicable. |
| `supported-bands` | `supported-card` | List of bands supported by a card's port. Only applicable to optical dwdm(-line) and AD/ ADE ports.<br>• not-applicable -Transmission band not applicable.<br>•  |
| `supported-capabilities` | `lldp-local-info`, `lldp-neighbor` | This attribute describes the remote system supported capabilities. |
| `supported-card` | `modules-adg`, `modules-degree`, `show` | Capability information for supported card. See supported-card (p. 1210) for more information. |
| `supported-card-mode` | `supported-card` | Supported card-modes. May be empty if card does not support any card-mode. |
| `supported-carrier-mode` | `show` | See supported-carrier-mode (p. 1215) for more information. |
| `supported-chassis` | `show` | Capability information for supported chassis. See supported-chassis (p. 1217) for more information. |
| `supported-facilities` | `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet`, `fc`, `flexo`, `flexo-group`, `interlaken`, `line-ptp`, `mc` +20 more | An XPath reference to the children facilities. |
| `supported-features` | `supported-card`, `supported-chassis` | Supported features; may be empty if no features are not supported. |
| `supported-gain-range` | `show` | See supported-gain-range (p. 1220) for more information. |
| `supported-max-power-draw` | `supported-tom-power` | Maximum power in watts the host port allows for this pluggable type under supported-power-class. |
| `supported-parameters` | `pm-control-entry` | List of PM parameters that this resource type supports for this direction/location with a maximum of 100 elements. |
| `supported-phy-mode` | `supported-tom`, `tom` | The phy-modes that are supported in this TOM for this card. |
| `supported-port` | `show` | Capabilities for each port in each supported card. See supported-port (p. 1221) for more information. |
| `supported-power-class` | `supported-tom-power` | Maximum MSA power class the host port supports for this pluggable type (may partially support that class; see supported-max-power-draw). |
| `supported-power-profile` | `show` | See supported-power-profile (p. 1225) for more information. |
| `supported-roles` | `ops` | OPS facility supported roles. The system exposes what configurations are possible, for the purpose of managers being able to offer the appropriate options for t |
| `supported-slot` | `show` | Capability for each slot within each supported chassis. See supported-slot (p. 1226) for more information. |
| `supported-sub-components` | `resources` | Names of sub-components present in this card, which can be addressed for certain operations like restart. |
| `supported-subchassis-type` | `supported-chassis` | List of chassis-types that this chassis supports as sub-chassis. The list has a maximum of 10 elements. If empty, means this chassis-type does not support multi |
| `supported-subtype` | `supported-card`, `supported-chassis`, `supported-tom` | Supported card subtypes; may be empty if card doesn't support subtypes. |
| `supported-subtypes` | `golden-carrier-mode`, `supported-carrier-mode` | Subtypes that each carrier mode supports. |
| `supported-tls-version` | `security-policies` | Transport Layer Security (TLS) supported version(s). |
| `supported-tom` | `show` | Capability information for supported TOM (Tunable/non-tunable Optical Module) in the scope of a particular card. See supported-tom (p. 1231) for more informatio |
| `supported-type` | `port` | List of supported types in this equipment holder. If a specific type is provisioned, the list has only that type. |
| `supported-values` | `golden-advanced-parameter` | This list indicates the possible values that this parameter can take as an input. It is a list of ranges or discrete numbers. This parameter is read-only. |
| `supporting-card` | `amplifier`, `amplifier-raman`, `ase-idler-service`, `ase-idler-source`, `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet`, `fc`, `flexo` +29 more | Card that holds this object. |
| `supporting-facilities` | `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet`, `fc`, `flexo`, `flexo-group`, `interlaken`, `line-ptp`, `mc` +21 more | An XPath reference to the parent facilities. |
| `supporting-facility` | `macsec-entity`, `secure-entity` | Name of the supporting facility |
| `supporting-fiber-connection` | `cable-id-path` | Container with the list of fiber connections (fiber-connection-list). The container is displayed when -r flag is used. |
| `supporting-input-port` | `amplifier`, `amplifier-raman`, `ase-idler-source`, `otdr`, `raman-calibration` | Rx (input) Port that holds this object. |
| `supporting-interface` | `show` | See supporting-interface (p. 1236) for more information. |
| `supporting-output-port` | `amplifier`, `amplifier-raman`, `ase-idler-source`, `otdr` | Tx (output) Port that holds this object. |
| `supporting-port` | `cid-ptp`, `comm-channel`, `dsc`, `dsc-group`, `eth-zr`, `ethernet`, `fc`, `flexo`, `flexo-group`, `interface`, `interlaken`, `line-ptp` +21 more | Port that holds this facility. |
| `supporting-protection-port` | `optical-switch` | Displays the optical-ptp of the Protection port on the parent OPSM card. |
| `supporting-working-port` | `optical-switch` | Displays the optical-ptp of Working port on the parent OPSM card. |
| `suppress-redundant` | `subscription-path` | Boolean flag to control suppression of redundant telemetry updates to the collector platform. If this flag is set to TRUE, then the collector will only send an  |
| `suspension-time` | `user` | This attribute is the duration of UID suspension following consecutive invalid login attempts. |
| `sv-name` | `sw-service` | A unique Id for each service instance on the NE. Contains card type, shelf, slot information. |
| `sw-component` | `show` | Software load component details. See sw-component (p. 1237) for more information. |
| `sw-component-name` | `sw-subcomponent` | Component name |
| `sw-container` | `show` | List of OS-level containers. See sw-container (p. 1239) for more information. |
| `sw-control-rule` | `add`, `delete`, `set`, `show` | See sw-control-rule (p. 1241) for more information. |
| `sw-management` | `show` | Software load details. The list can be retrieved by using show sw-management. See sw-management (p. 1243) for more information. |
| `sw-service` | `show` | Software service running in the system. See sw-service (p. 1246) for more information. |
| `sw-subcomponent` | `show` | Software load subcomponent details. See sw-subcomponent (p. 1248) for more information. |
| `sw-subcomponent-name` | `sw-subcomponent` | Subcomponent name |
| `sw-support-revision` | `inventory`, `supported-card` | Software revision currently installed. |
| `swimage` | `activate` | Command parameter for activating the currently installable software image. For swimage specific parameters, refer to Table 78: activate swimage command paramete |
| `switch-failure-reason` | `protection-group` | The reason the switch failed |
| `switch-role` | `optical-switch` | Indication for the cascading/ non-cascading OPSM switch role of the optical-switch:<br>• standalone - Regular protection (2-path protection or any other). |
| `switch-target` | `protection-switch` | The target of the switch command, which is not needed for release and lockout operation. |
| `switch-threshold-enable` | `optical-switch` | Enables the protection switching based on SD threshold configured for Working and Protection Paths. |
| `switch-threshold-hysteresis` | `optical-switch` | SD threshold hysteresis (in dB). Applies to both working-switch-threshold and protect-switch-threshold. The recommended configured value for MCHP and OMSP deplo |
| `switching-mode` | `optical-switch`, `protection-group` | Protection switching mode. |
| `swload-activation-type` | `software-load` | Software load activation type. Only of relevance for software load state installable. direct - No reboot type determined. warmstart - Update requires warm reboo |
| `swload-delta-label` | `software-load` | Software load delta label. |
| `swload-information` | `software-load` | Software load information. |
| `swload-label` | `software-load` | Software load label. |
| `swload-manifest` | `software-load` | Software load manifest file. Only of relevance for software load installable. |
| `swload-pkg-type` | `software-load` | Software load package type |
| `swload-prepared` | `software-load` | Software load prepared. Only of relevance for software load installable. |
| `swload-product` | `software-load` | Software load product. |
| `swload-state` | `packaged-fw`, `software-load`, `sw-component`, `sw-subcomponent` | SW load subcomponent state. active - Active software load. inactive - Inactive software load. installable - Installable software load. |
| `swload-status` | `software-load` | Software load current status. status-unknown - Software load status unknown. validate-in-progress - Software load validation in progress. validate-complete - So |
| `swload-vendor` | `software-load` | Software load vendor. |
| `swload-version` | `software-load` | Software load version. |
| `syslog` | `set`, `show` | Set of attributes configuration for logging functionality via syslog. Includes control of local log files, remote logging configuration and logging in serial co |
| `system` | `clear`, `show`, `status` | Wipes the system/specific instance and resets to the factory configurations. For additional details, refer to system (p. 1256). |
| `system-capabilities` | `show` | Top level container for all capability information. This data is read-only, and expected to be informative to the user regarding what are the system capabilitie |
| `system-description` | `lldp-local-info`, `lldp-neighbor` | The string value used to identify the system description of the remote system. |
| `system-name` | `lldp-local-info`, `lldp-neighbor` | The string value used to identify the system name of the remote system. |
| `target` | `appctl`, `configure`, `message`, `statistics`, `system`, `topology` | The CLI sessions to which the message will be sent |
| `target-actual-power` | `ochm`, `spectrum-monitoring` | Target power computed by ATPS. This attribute is applicable to HSC OLS nodes. |
| `target-actual-power-dst` | `oxcon` | Value as calculated by Power Control if target-power-setting is set to auto. Otherwise it is the exact value configured at target-output-power-dst/ src.", |
| `target-actual-power-src` | `oxcon` | Value as calculated by Power Control if target-power-setting is set to auto. Otherwise it is the exact value configured at target-output-power-dst/ src.", |
| `target-actual-psd-dst` | `oxcon` | Actual PSD destination. |
| `target-actual-psd-src` | `oxcon` | Actual PSD source. |
| `target-address` | `connect`, `snmp-target` | The target-address which may be IPv4, IPv6 or hostname (if DNS configured). It requires connectivity. It is a mandatory attribute. |
| `target-command` | `est` | Indicates the type of target command. |
| `target-file` | `file` | Filepath of the file to be deleted. |
| `target-name` | `snmp-target` | The target listener name. Identifies the SNMP target. |
| `target-output-power` | `ase-idler-source`, `oms`, `osc`, `spectrum-control` | ASE pump output power required (if manually configured). |
| `target-output-power-dst` | `oxcon` | The destination interface target power. |
| `target-output-power-src` | `oxcon` | The source interface target power. |
| `target-port` | `snmp-target` | UDP port number. |
| `target-power-adjust-cband` | `ots` | User-defined C-band power offset. Currently this attribute is applicable to HSC OLS only. |
| `target-power-adjust-lband` | `ots` | User-defined L-band power offset. Only applicable for sleds supporting L-band. Currently this attribute is applicable to HSC OLS only. |
| `target-power-ase` | `nmc` | Target output power of parent NMC. |
| `target-power-setting` | `optical-ptp`, `ots`, `ots-r` | This attribute is applicable to both HSC OLS and Standard OLS modes . It defines how the target power of the drop OXcon is determined:<br>• auto, the system aut |
| `target-pump-power` | `pump-power` | Raman Pump Power required in dBm units. Applicable when the control-mode is manual. • If the card is RPBM, the target-pump-power must be in the range of 12 to 3 |
| `target-raman-gain` | `amplifier-raman` | Indicates the target Raman gain:<br>• The target Raman gain, configurable in case the control-mode is different than auto.<br>• In case control-mode is auto, th |
| `target-representation` | `convert` | Protocol to convert the command to. |
| `target-select` | `verify` | For fiber-connection verification, the attribute can be:<br>• &lt;port-id&gt; - port instance-identifier of the cable-id capable card to be verified.<br>• If th |
| `target-transport` | `snmp-target` | Type of transport for the SNMP target. |
| `task` | `add`, `delete`, `set`, `show` | User configurable scheduled task. Can define single occurrence or periodic commands. See task (p. 1265) for more information. |
| `task-name` | `run` | The task name to be executed. |
| `task-status` | `scheduled-task`, `task` | Current operational state of the scheduled task. |
| `tca-supervision` | `pm-control-entry` | TCA supervision for this resource. |
| `tda-degrade-mode` | `trib-ptp` | The switching of defect BERSD-ODU trig ALS. |
| `telemetry` | `show` | Top level configuration and state for the device telemetry system. The list can be retrieved by using show telemetry. See telemetry (p. 1268) for more informati |
| `template` | `add`, `delete`, `show` | See template (p. 1269) for more information. |
| `template-group` | `add`, `delete`, `show` | See template-group (p. 1271) for more information. |
| `template-group name` | `template` | Represents name of the template-group |
| `template-name` | `template` | Represents name of the template entry |
| `template-type` | `apply-template` | The type of template to apply. Other parameters may be required depending on the template type. |
| `templates` | `show` | See templates (p. 1273) for more information. |
| `test-duration` | `bert` | specifies the duration of the test is run in seconds |
| `test-id` | `bert` | specifies the identifier for the test. If the user does not specify a test-id, a unique test-id is generated by the system |
| `test-progress` | `cable-id-status` | Display the cable-id test progress. It uses a string to show the progress:<br>• "Not applicable" - If cable-id-state is disabled.<br>• "N out of M completed" -  |
| `test-signal-direction` | `bert`, `ethernet`, `fc`, `oc`, `odu-diagnostics`, `stm` | The direction of the test signal. |
| `test-signal-monitoring` | `ethernet`, `fc`, `oc`, `odu-diagnostics`, `stm` | Monitor the incoming test signals for diagnostics. |
| `test-signal-monitoring-direction` | `bert` | specifies the direction of the test monitoring. 100GE / 400GE / CarrierCTP support ingress. ODU4 supports ingress and egress |
| `test-signal-monitoring-type` | `bert` | specifies the type of signal associated with monitoring. 100GE / 400GE support scrambled-idles, ODU4 supports PRBS31 and PRBS31NONINV, CarrierCTP supports fec-f |
| `test-signal-type` | `bert`, `ethernet`, `fc`, `oc`, `odu-diagnostics`, `stm` | The type of test pattern that is injected. |
| `third-party-app` | `set`, `show` | See third-party-app (p. 1278) for more information. |
| `third-party-fw` | `show` | See third-party-fw (p. 1280) for more information. |
| `tilt-actual` | `amplifier` | Spectrum Tilt (measured by the EDFA). A 0dB reading indicates: no tilt, or amplifier not available. |
| `tilt-adjustment` | `amplifier` | Used to offset the target tilt when tilt-control-mode = 'auto' / 'auto-planned'. The actual tilt may differ from the requested tilt-adjustment. |
| `tilt-control-mode` | `amplifier` | Specify the gain tilt control mode. Defines whether amplifier tilt is automatically set by system or configured manually by the user. i Note: When amplifier fun |
| `tilt-target` | `amplifier` | Target gain tilt of the amplifier. Applicable for manual control mode. Changing the attribute: a warning "This attribute may be traffic affecting" is issued. |
| `tim-act-enabled` | `odu-diagnostics`, `otu-diagnostics` | Support configurable TIM action which decides if insert maintenance signal per TIM: enable or disable, default is disable. |
| `tim-monitor` | `oc`, `stm` | Switch for enabled tim defect monitor mode. |
| `time in seconds` | `sleep` | Duration of delay in seconds. |
| `time-of-last-switchover` | `controller-card` | Timestamp of the last controller switchover event. Value only visible on active controller card. |
| `time-slots` | `ethernet`, `odu`, `otu` | Time slots of the ethernet (when tx-mapping-mode = 'openZR+'). |
| `time-source` | `clock` | Indicates the source of the system current time. ntp - Indicates that NE uses NTP for synchronization. manual - Indicates that NE uses NE internal clock for Syn |
| `timeout` | `aaa-server`, `activate`, `dial-out-server`, `user` | Specifies the response timeout of Access-Request messages sent to a AAA server in seconds. |
| `timezone` | `clock` | Indicates the Name of the Time Zone of this NE. |
| `timing-mode` | `ethernet` | Indicates the timing mode of the ethernet client. This attribute is applicable to 1830 GX G40 only. |
| `tls-1.2-cipher-suites` | `security-policies` | Supported TLS 1.2 cipher suites. |
| `tls-1.3-cipher-suites` | `security-policies` | Supported TLS 1.3 cipher suites. |
| `tls-curves` | `security-policies` | Supported elliptic curve algorithms. Applies to both TLS 1.2 and 1.3. i Note: PQC algorithms are supported only for TLS 1.3. |
| `to-adaptation` | `xcon` | Indicate server layer adaptation at line side. |
| `to-swload-version` | `upgrade-status` | Target Software Load Version. |
| `tom` | `add`, `delete`, `set`, `show` | Transceiver Optical Module. See tom (p. 1283) for more information. |
| `tom-auto-migration` | `equipment-policies` | Enables automatic update of tom subtype based on present equipment. This update may have direct impact on existing configurations. Note: this has impact on tom  |
| `tom-part-number` | `serdes-template`, `serdes-template-entry` | The TOM part-number to which this template applies. |
| `tom-subtype-group` | `supported-tom` | TOM subtype group. |
| `tom-type` | `show`, `supported-tom`, `supported-tom-power`, `tom-type` | Capability information for supported TOM (Tunable/non-tunable Optical Module). See tom-type (p. 1290) for more information. |
| `topology` | `clear`, `show` | Manually removes existing topology neighbor information. For additional details, refer to topology (p. 1292). |
| `total-ageout` | `lldp-port-statistics` | A count of the times that a neighbor’s information is deleted from the lldp-neighbor list due to TTL timer expiration. |
| `total-available-power` | `chassis` | Total available power from the installed and active PEMs in the chassis after accounting for redundancy. |
| `total-bytes` | `transfer-status` | Total file size in bytes. Zero until known at the transfer start. |
| `total-discarded-frames` | `lldp-port-statistics` | A count of all LLDPDUs received and then discarded. |
| `total-discarded-tlvs` | `lldp-port-statistics` | A count of all TLVs received at the port and discarded for any reason. |
| `total-frames-in` | `lldp-port-statistics` | A count of all LLDP frames received at the port. |
| `total-frames-out` | `lldp-port-statistics` | A count of all LLDP frames transmitted through the port. |
| `total-pump-power` | `amplifier-raman` | Operating Total Pump Power. When the value of Total Pump Power is available: the raman pump power actually being sent on port 401, upstream, in the DWDM Line. - |
| `total-reflectance-rx-measured` | `ots-r-auto-otdr` | Displays the total reflectance value between the span fiber and DWDM Line-In port of the Raman card, that is measured by the automatic OTDR Raman pre-check feat |
| `total-space` | `usb` | The total storage space available in the file-system associated with this USB port. Applicable if the type is storage. |
| `total-time-slots` | `eth-zr`, `odu` | The member of the slots to be supported as times of 100G: rate-class/100. |
| `total-unrecognized-tlvs` | `lldp-port-statistics` | This counter provides a count of all TLVs not recognized by the receiving LLDP local agent. |
| `tr-dest` | `traceroute` | IP address of the destination of the ICMP ECHO REQUEST datagram. _ |
| `traffic-kill-offset` | `secure-entity` | If the re-key fail policy is set to KILL-TRAFFIC, this attribute indicates the amount of time the system waits before killing all encrypted data security associ |
| `transfer` | `show` | Information associated with file transfer. The information can be retrieved by using show transfer. See transfer (p. 1297) for more information. |
| `transfer-mode` | `current-subscription` | Specifies the data transfer mode to the target device. |
| `transfer-progress` | `transfer-status` | Transfer completion percentage. |
| `transfer-status` | `show` | File transfer status. The information can be retrieved by using show transfer-status. See transfer-status (p. 1299) for more information. |
| `transfer-type` | `transfer-status` | Last transfer type:<br>• sync - last transfer type sync.<br>• async - last transfer type async |
| `transmit-delay` | `ospf-interface` | Estimated time needed to transmit Link State Update (LSU) packets on the interface (seconds). LSAs have their age incremented by this amount when advertised on  |
| `transport` | `dial-out-server`, `log-server` | Dial-out-server transport protocol. |
| `transport-entity` | `protection-unit` | The instance identifier of the transport entity. |
| `trap-community-string` | `snmp-target` | The community string used for SNMP traps. |
| `trib-port-number` | `odu` | Number of OPUk/OPUCn trib ports that are part of this ODUk/ODUCn container. |
| `trib-ptp` | `set`, `show` | Optional service-specific custom rules to override default action upon service failure. See trib-ptp (p. 1303) for more information. |
| `tributary-disable-action` | `trib-ptp` | Indicates what action the network element performs towards the client equipment (connected over the TOM) when a line-side failure is observed. This includes shu |
| `tributary-disable-holdoff-ti mer` | `trib-ptp` | The hold off time of client shutdown or replacement signal at egress direction. 0 means holdoff functionality disabled. |
| `trigger` | `calibrate`, `simulate` | The alarm event trigger to simulate:<br>• raise-alarm - Simulates the raising of an alarm.<br>• clear-alarm - Clears a simulated alarm.<br>• plug-in-fru - Simul |
| `trust-chain` | `local-certificate`, `peer-certificate`, `trusted-certificate` | Lists trusted certificates that constitute this certificate's trust chain. |
| `trusted-certificate` | `set`, `show` | X509v3 CA (Root and Intermediate) certificate that the system trusts. See trusted-certificate (p. 1310) for more information. |
| `tti-mismatch-alarm-reporting` | `odu-diagnostics`, `otu-diagnostics` | Indicates if TTI-Mismatch (TIM) alarm is reported or masked. If it is to be reported, indicates the criteria based on with the TIM alarm is reported. |
| `tti-port-id` | `ots-diagnostics` | The port-id in OTS TTI is the AID of the port but limited to 32 printable characters. It is only applicable and visible when tti-style is 'proprietary'. tti-por |
| `tti-style` | `oc`, `odu-diagnostics`, `ots-diagnostics`, `otu-diagnostics`, `stm` | The configured mode of the TTI for this OTU/ODU client. |
| `ttl` | `ace`, `lldp-neighbor` | Remote system info Time-To-Live (TTL). The number of seconds until information expires. If the remote system doesn't provide a ttl value, this parameter is set  |
| `tts` | `fc` | Enable or disable the Transmitter Training Signal (TTS) support for 32GFC client. |
| `tx-cd` | `optical-carrier` | The configured transmit pre-compensation chromatic dispersion. |
| `tx-dapi` | `odu-diagnostics`, `otu-diagnostics` | The transmitted DAPI bytes. |
| `tx-fiber-type` | `submarine-link` | Indicates the Tx fiber type of the link. |
| `tx-filter-roll-off` | `optical-carrier` | Transmitter filter roll off factor. For the SPN2, SPN2C, or CHM1R card with line pluggable TOM-400GXR-Q-DWDM, this parameter is read-only and the default value  |
| `tx-mapping-mode` | `ethernet`, `fc`, `oc`, `otu`, `stm` | The tx mapping mode of client port. The possible values are dependent on the HW and configuration. |
| `tx-operator` | `odu-diagnostics`, `ots-diagnostics`, `otu-diagnostics` | The transmitted operator specific bytes. The value of tx-operator is transmitted to the NE connected on the other end of the fiber via the OTS TTI Operator Spec |
| `tx-payload-type` | `odu` | Transmitter payload-type of ODU. |
| `tx-power` | `optical-carrier` | The optical carrier's transmit power into the fiber from the transponder's optics. The accuracy of the Tx Power can be adjusted in steps of 0.5 dBm. The range o |
| `tx-power-adjustment` | `osc` | OSC transmitting power adjustment to the automatically calculated Tx power target. If required by planning, in osc-control auto mode, the OSC Tx power can be ma |
| `tx-sapi` | `odu-diagnostics`, `otu-diagnostics` | The transmitted SAPI bytes. |
| `tx-tti` | `oc`, `odu-diagnostics`, `otu-diagnostics`, `stm` | Transmit TTI - Sent by this facility to the far-end remote facility. |
| `type` | `acl`, `auth-key`, `calibrate`, `certificate`, `comm-channel`, `crl`, `db-migrate`, `golden-advanced-parameter`, `ntp-server-status`, `pm-parameter`, `profile-control`, `restart` +8 more | Card type. |
| `type-select` | `verify` | Type of verification. Currently only fiber-connection verification is supported. |
| `unassigned-carriers` | `resources` | Names of the carriers that are not yet assigned to a resource. |
| `units` | `pm-parameter` | Units for the parameter. |
| `universal-time` | `clock` | Indicates the UTC Date and Time of this NE. |
| `unprovisioned-inventory` | `show` | See unprovisioned-inventory (p. 1315) for more information. |
| `unsupported-applications` | `subtype-constraint` | List of applications not supported by this subtype. If this list is empty, then this constraint is not applicable. |
| `updates-only` | `current-subscription` | A flag allowing to only send updates to the current state, when set to true the device will not send the initial current value, rather only changes to the initi |
| `upgrade-status` | `show`, `tom` | See upgrade-status (p. 1319) for more information. |
| `upper-frequency` | `mc`, `mc-f`, `ocm-channel`, `oms`, `spectrum-monitoring` | Upper Frequency of a Media Channel. |
| `uptime` | `clock`, `sw-container`, `sw-service` | Time since the container started. |
| `uptime-seconds` | `clock` | Indicates how long the system has been running, in seconds. |
| `url` | `cdp`, `ocsp-server` | HTTP URL of CRL. The CRL will be fetched from this location. |
| `url-base` | `http-file-server` | The base URL used to redirect to the file transfer application. |
| `usb` | `show` | Represents the USB function of this port. See usb (p. 1333) for more information. |
| `usb-path` | `usb` | Local filesystem path on where this USB file-system is mounted; this can be used as a target/ source for file transfer operations. Applicable if the type is sto |
| `use-as-source` | `networking` | Interface to use as source address. |
| `use-serdes-templates` | `equipment-templates` | Whether serdes-templates are globally enabled or not. On enabling: templates are not automatically applied; they'll be applied from that moment onward. On disab |
| `used` | `cid-ptp` | It is true when CableID functionality is supported. |
| `used-by` | `local-certificate` | List of foreign keys representing secure-applications, ikev2-peers, etc., presently using the certificate |
| `used-resources` | `line-ptp`, `odu`, `xcon` | Provide an aggregate view of all used resources on the DSP. |
| `user` | `add`, `delete`, `set`, `show` | An authorized user. See user (p. 1336) for more information. |
| `user-aaa-type` | `user` | Indicates the authentication method of the user. |
| `user-access` | `current-subscription` | Username in order to resolve paths according to user access. |
| `user-group` | `access-rule-list`, `add`, `delete`, `set`, `show`, `user` | See user-group (p. 1341) for more information. |
| `user-name` | `connect`, `file-server`, `ssh-authorized-key`, `user` | User name. |
| `user-sec-level` | `snmpv3-user` | Specifies the SNMPv3 user security level. |
| `user-status` | `user` | This attribute shows the user status. User with status 'enabled' will have access to the system. User with status 'disabled' not have access to the system. User |
| `valid-from` | `local-certificate`, `peer-certificate`, `trusted-certificate` | The date from which the certificate is valid. |
| `valid-signal-time` | `line-ptp`, `super-channel`, `super-channel-group`, `trib-ptp` | Configurable time that represents a detection of a valid signal. Used for auto-in-service mechanism. |
| `valid-to` | `local-certificate`, `peer-certificate`, `trusted-certificate` | The date after which the certificate is deemed to have expired. |
| `value` | `add`, `advanced-parameter`, `current-advanced-parameter`, `export`, `named-value-set`, `serdes`, `serdes-template-entry`, `set`, `show`, `show commit`, `template` | Value of the attribute to be initialized. |
| `variable` | `export` | Name of the variable; can be any alphanumeric name. |
| `vendor` | `inventory`, `third-party-app`, `third-party-fw`, `unprovisioned-inventory` | Part number for this equipment. |
| `vendor-compliance-code` | `inventory` | Vendor Compliance Code information for 3rd party TOMs. |
| `verify-client-cert` | `secure-application` | Enables or disables TLS Mutual Authentication. Controls client certificate verification behavior at TLS handshake:<br>• disabled - Indicates that client certifi |
| `verify-result` | `verify` | Result of the verification operation. |
| `version` | `gapt`, `gcmt`, `local-certificate`, `manifest`, `ospf-instance`, `peer-certificate`, `sw-component`, `sw-subcomponent`, `third-party-app`, `third-party-fw`, `trusted-certificate` | Table version. |
| `virtual-slot` | `supported-slot` | Describes whether this slot is virtual. |
| `voa-attenuation-actual-rx` | `osc` | Reports the actual VOA value as configured. The system returns not-applicable when the card or SFP is not actually equipped. |
| `voa-attenuation-target-rx` | `osc` | Target Rx VOA value in case of manual control mode. |
| `voa-control-mode` | `amplifier` | Type of VOA control mode:<br>• manual - Manual target attenuation.<br>• constant-power - Constant Power. |
| `vrf` | `bgp-instance`, `interface`, `ipv4-static-route`, `ipv6-static-route`, `ospf-instance`, `rib`, `show` | See vrf (p. 1348) for more information. |
| `wavelength` | `optical-carrier` | The wavelength of the optical carrier. |
| `wavelength-band` | `optical-switch` | Defines the wavelength band: o-band (1310) or c-band (1550). |
| `wavelength-duplication` | `adg` | Whether the SRG can handle duplicate wavelengths and if so to what extent. Only CDCs allow more than one instance of the same wavelength on the ADG. |
| `when` | `ntp-server-status` | Indicates time elapsed since last packet was received in seconds. |
| `white-listed` | `peer-certificate` | If true, the peer-certificate does not have an associated trust-chain, and was explicitly white-listed at import time. Otherwise, it has an associated trust-cha |
| `width` | `nmc`, `spectrum-control`, `spectrum-monitoring` | Network Media Channel frequency width; unit is MHz. The user must configure the 3 dB signal bandwidth. The value in GHz must be equivalent to the baud rate (GBd |
| `working-los-threshold` | `optical-switch` | Defines the Signal Fail (SF) threshold for the Working Path. It is represented in dBm. |
| `working-path-degree` | `optical-switch` | Displays the degree number of the working path degree. The value of zero denotes that the working path degree is not associated yet. |
| `working-pu` | `protection-group` | The working Protection uUnit (PU) associated with the protection group. |
| `working-switch-threshold` | `optical-switch` | Defines the Signal Degrade (SD) threshold for the Working Path. It is represented in dBm. |
| `writable-running` | `system-policies` | Disabling writable-running policy makes it impossible to do configure commands via running datastore, making it mandatory to use the candidate datastore. This i |
| `write-default` | `authorization` | In case only user configured access-rules are used, this policy defines what is the action to use if a given write operation does not match any rule. Write acce |
| `wss-less` | `degree`, `oms` | Indicates if there is a WSS component or not. The value is true if there is no WSS component in the Degree. The system sets autonomously this attribute to 'true |
| `wtr-timer` | `optical-switch`, `protection-group` | Trigger clearance soaking time before reverting to the working protection unit, measured and set in 1-second steps. Only applicable in revertive mode. |
| `xcon` | `add`, `delete`, `set`, `show` | Layer 1 digital services that are currently provisioned in the system. This includes pre-provisioned XCONs too. See xcon (p. 1350) for more information. |
| `xcon-type` | `nw-xconnect` | The XCON type of this object:<br>• L1-ETH-TO-GCC0 - L1-ETH to GCC0 user channel cross-connection.<br>• L1-GCC0-TO-GCC0 - GCC0 to GCC0 user channel cross-connect |
| `ztp` | `show` | Zero Touch Provisioning (ZTP) status. See ztp (p. 1356) for more information. |
| `ztp mode` | `change-ztp-mode` | Enable or disable ztp. |
| `ztp-completion-status` | `ztp` | Summarized completion status of ZTP on the node. |
| `ztp-details` | `ztp` | Additional information on the current state. |
| `ztp-mode` | `ztp` | User-set mode of ZTP. This flag is set via change-ztp-mode RPC that is allowed even when NBI is locked. |
| `ztp-state` | `ztp` | Current state of ZTP service. |
