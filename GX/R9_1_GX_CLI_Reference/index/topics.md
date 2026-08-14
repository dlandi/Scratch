# Topic index - query vocabulary to commands

Maps the words people actually use to the commands that implement them. Each entry lists **search terms** (synonyms, acronyms and phrasings that should trigger it) and the commands to open.

50 topics covering 380 distinct commands. Associations were assigned by reading each command's description in the source; they are editorial, not extracted, so treat them as routing hints and confirm against the command page.

## AAA, TACACS+ and RADIUS

*Search terms:* `aaa`, `tacacs`, `radius`, `authentication`

| Command | What it does | File |
| --- | --- | --- |
| `aaa-server` | Specifies the number of attempted Access-Request messages to a single AAA server before failing... | [001-aaa-server.md](../06-operation-commands/001-aaa-server.md) |
| `aaa-statistics` | Displays the number of accounting requests | [002-aaa-statistics.md](../06-operation-commands/002-aaa-statistics.md) |
| `authorization` | Number of times since the system last restarted that a notification was dropped for a subscription... | [027-authorization.md](../06-operation-commands/027-authorization.md) |
| `auth-key` | Indicates whether the integrity key is ASCII or hexadecimal encoded | [026-auth-key.md](../06-operation-commands/026-auth-key.md) |
| `cert-to-name` | Specifies the user label | [044-cert-to-name.md](../06-operation-commands/044-cert-to-name.md) |

## Access control lists and rules

*Search terms:* `acl`, `access control`, `access rule`, `ace`, `permit`, `deny`

| Command | What it does | File |
| --- | --- | --- |
| `acl` | Indicates the top-level type of ACL, i.e., what fields from the associated IPv4 or IPv6 headers... | [007-acl.md](../06-operation-commands/007-acl.md) |
| `ace` | User-configurable label | [006-ace.md](../06-operation-commands/006-ace.md) |
| `access-control-list` | This command is used to show access control list | [003-access-control-list.md](../06-operation-commands/003-access-control-list.md) |
| `access-rule` | A user-configurable description about this access rule | [004-access-rule.md](../06-operation-commands/004-access-rule.md) |
| `access-rule-list` | A generic description of this access-rule-list | [005-access-rule-list.md](../06-operation-commands/005-access-rule-list.md) |
| `security` | The command described in this section is used to show the top level security container | [279-security.md](../06-operation-commands/279-security.md) |
| `security-policies` | This policy defines whether OCSP responders can be consulted for certificate revocation checking | [280-security-policies.md](../06-operation-commands/280-security-policies.md) |

## Alarms and conditions

*Search terms:* `alarm`, `condition`, `severity`, `acknowledge`

| Command | What it does | File |
| --- | --- | --- |
| `alarm` | Timestamp when the alarm was last changed by operator | [014-alarm.md](../06-operation-commands/014-alarm.md) |
| `current-alarms` | Timestamp of the last change in the current alarm list (either a raise or clear event) | [066-current-alarms.md](../06-operation-commands/066-current-alarms.md) |
| `alarm-control` | System -wide alarm-soaking-behavior switch:<br>• automatic: soaking time used is defined in FM... | [015-alarm-control.md](../06-operation-commands/015-alarm-control.md) |
| `alarm-inventory` | Information on whether this alarm is service affecting or not | [016-alarm-inventory.md](../06-operation-commands/016-alarm-inventory.md) |
| `alarm-severity-profile` | The assigned severity of the profile | [018-alarm-severity-profile.md](../06-operation-commands/018-alarm-severity-profile.md) |
| `alarm-severity-entry` | Possible alarm service affecting category | [017-alarm-severity-entry.md](../06-operation-commands/017-alarm-severity-entry.md) |
| `set-alarm-state` | Optional text that will be stored in the alarm | [288-set-alarm-state.md](../06-operation-commands/288-set-alarm-state.md) |
| `get-conditions` | Resource Access Identifier (AID) | [118-get-conditions.md](../06-operation-commands/118-get-conditions.md) |
| `simulate` | The location of the simulated alarm | [293-simulate.md](../06-operation-commands/293-simulate.md) |

## Amplifiers, gain and Raman pumps

*Search terms:* `amplifier`, `gain`, `raman`, `pump`, `tilt`

| Command | What it does | File |
| --- | --- | --- |
| `amplifier` | Control speed factor for the DGE power control algorithm | [019-amplifier.md](../06-operation-commands/019-amplifier.md) |
| `amplifier-raman` | Indicates the current state of the power control adjustment for the preamplifier:<br>• unknown :... | [020-amplifier-raman.md](../06-operation-commands/020-amplifier-raman.md) |
| `pump` | Describes whether this facility was system created or not | [255-pump.md](../06-operation-commands/255-pump.md) |
| `pump-power` | The actual values which are currently measured in each pump | [256-pump-power.md](../06-operation-commands/256-pump-power.md) |
| `raman-calibration` | Indicates any information for troubleshooting when the calibration-state is fail or out-dated | [257-raman-calibration.md](../06-operation-commands/257-raman-calibration.md) |
| `calibrate` | Select the entity to be calibrated | [036-calibrate.md](../06-operation-commands/036-calibrate.md) |
| `supported-gain-range` | The maximum settable gain-target for this type of range ('standard'/ 'low'/ 'high') | [324-supported-gain-range.md](../06-operation-commands/324-supported-gain-range.md) |
| `rsc` | The transmitted Pilot Tone integrated power | [271-rsc.md](../06-operation-commands/271-rsc.md) |

## BGP routing

*Search terms:* `bgp`, `peer`, `autonomous system`

| Command | What it does | File |
| --- | --- | --- |
| `bgp-instance` | Specifies the router ID. 0.0.0.0/0 is not supported for IPv4 and 0::0.0 is not supported for IPv6 | [029-bgp-instance.md](../06-operation-commands/029-bgp-instance.md) |
| `bgp-neighbor` | Current BGP Session state errors if any ASCII format | [030-bgp-neighbor.md](../06-operation-commands/030-bgp-neighbor.md) |
| `bgp-network` | Specifies the network prefix | [031-bgp-network.md](../06-operation-commands/031-bgp-network.md) |

## CLI navigation, output filtering and help

*Search terms:* `navigat`, `prompt`, `help`, `pipe`, `filter`, `tree`

| Command | What it does | File |
| --- | --- | --- |
| `edit` | Instance ID of the entity to be addressed | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#42-edit) |
| `top` | Displays help for this command | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#44-top) |
| `up` | Displays help for this command | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#47-up) |
| `tree` | Instance ID of the entity to be displayed in the tree | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#45-tree) |
| `history` | Displays help for this command | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#43-history) |
| `alias` | Value to replace the alias name with | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `unalias` | Name of the alias to remove | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `help` | Displays help for a command, container, or attribute. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `?` | Contextual help: displays what can be typed at the current prompt. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `begin` | Line to begin with | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#51-begin) |
| `display` | The display mode to be selected | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#52-display) |
| `exclude` | Text to be filtered | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#53-exclude) |
| `grep` | The following options are supported for grep:<br>• -a=&lt;n&gt; - Number of lines of context to... | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#54-grep) |
| `highlight` | Any word to highlight | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#55-highlight) |
| `include` | Text to be filtered | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#56-include) |
| `linenum` | Any display command such as tree or show | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#57-linenum) |
| `more` | Any display command such as tree or show | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#58-more) |
| `sort` | Any attribute name that exists in the context of the output | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#59-sort) |
| `until` | Line to end with | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#510-until) |
| `tic` | Starts a timer for the typed command. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `toc` | Displays the elapsed time since the timer was started. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |

## CableID fiber verification

*Search terms:* `cableid`, `cable-id`, `verify`

| Command | What it does | File |
| --- | --- | --- |
| `cable-id` | The commands described in this section are used to show the `cable-id` entities and terminate a... | [033-cable-id.md](../06-operation-commands/033-cable-id.md) |
| `cable-id-path` | Displays a list of supporting-fiber-connections | [034-cable-id-path.md](../06-operation-commands/034-cable-id-path.md) |
| `cable-id-status` | Display the cable-id test progress | [035-cable-id-status.md](../06-operation-commands/035-cable-id-status.md) |
| `verify` | Result of the verification operation | [371-verify.md](../06-operation-commands/371-verify.md) |
| `cid-ptp` | It is true when CableID functionality is supported | [048-cid-ptp.md](../06-operation-commands/048-cid-ptp.md) |

## Candidate configuration, commit and rollback

*Search terms:* `candidate`, `commit`, `rollback`, `datastore`, `discard`

| Command | What it does | File |
| --- | --- | --- |
| `configure` | This parameter allows to leverage the to initialize the candidate from the configuration associated... | [057-configure.md](../06-operation-commands/057-configure.md) |
| `commit` | This command &lt;id&gt; defines the ID of the commit confirmed, commit persist and confirmed cancel... | [055-commit.md](../06-operation-commands/055-commit.md) |
| `show commit` | Filter (&lt;attribute&gt;=&lt;value&gt;) | [292-show-commit.md](../06-operation-commands/292-show-commit.md) |
| `rollback` | This CLI command will revert the current Datastore either Running or Candidate, depending on... | [268-rollback.md](../06-operation-commands/268-rollback.md) |
| `discard-changes` | This command will discard all candidate datastore content and CLI return to operational mode | [082-discard-changes.md](../06-operation-commands/082-discard-changes.md) |
| `validate` | The command to validate | [370-validate.md](../06-operation-commands/370-validate.md) |
| `diff` | It is a system generated commit-id | [080-diff.md](../06-operation-commands/080-diff.md) |
| `lock` | Displays help for this command | [162-lock.md](../06-operation-commands/162-lock.md) |
| `unlock` | Displays help for this command | [360-unlock.md](../06-operation-commands/360-unlock.md) |
| `system-policies` | Disabling writable-running policy makes it impossible to do configure commands via running... | [341-system-policies.md](../06-operation-commands/341-system-policies.md) |
| `config` | Entity type to retrieve the configuration | [056-config.md](../06-operation-commands/056-config.md) |

## Cards, slots, chassis and pluggables

*Search terms:* `card`, `slot`, `chassis`, `tom`, `pluggable`, `fru`, `equipment`

| Command | What it does | File |
| --- | --- | --- |
| `card` | List of sub-cards associated with this card | [040-card.md](../06-operation-commands/040-card.md) |
| `slot` | Name of the equipment that is currently required in this slot | [295-slot.md](../06-operation-commands/295-slot.md) |
| `chassis` | Indicates if the chassis power consumption is limited by reducing max fan speed. i Note: This... | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `equipment` | The equipment to be viewed | [092-equipment.md](../06-operation-commands/092-equipment.md) |
| `controller-card` | Additional details for synchronization status | [061-controller-card.md](../06-operation-commands/061-controller-card.md) |
| `port` | Port usage type | [246-port.md](../06-operation-commands/246-port.md) |
| `tom` | Specifies if the TOM is configured to function in the low power mode | [352-tom.md](../06-operation-commands/352-tom.md) |
| `tom-type` | 3rd party subtype for this TOM | [353-tom-type.md](../06-operation-commands/353-tom-type.md) |
| `sub-component` | A user configurable description of the sub-component | [314-sub-component.md](../06-operation-commands/314-sub-component.md) |
| `fru-info` | Type of the equipment | [114-fru-info.md](../06-operation-commands/114-fru-info.md) |
| `led` | The state of the LED, that is, the current color status of the LED: not-available - LED status not... | [152-led.md](../06-operation-commands/152-led.md) |
| `usb` | Local filesystem path on where this USB file-system is mounted; this can be used as a target/... | [366-usb.md](../06-operation-commands/366-usb.md) |
| `console` | Current status of the console for this card | [060-console.md](../06-operation-commands/060-console.md) |
| `serial-console` | Serial console inactivity timeout | [285-serial-console.md](../06-operation-commands/285-serial-console.md) |
| `resources` | Available bandwidth for the paired slot connection. i Note: This parameter is applicable only for... | [264-resources.md](../06-operation-commands/264-resources.md) |
| `equipment-policies` | Physical location of the communication Ethernet ports | [093-equipment-policies.md](../06-operation-commands/093-equipment-policies.md) |

## Certificates and PKI

*Search terms:* `certificate`, `x509`, `csr`, `crl`, `ocsp`, `est`, `ca`

| Command | What it does | File |
| --- | --- | --- |
| `certificate` | Certificate ID. The id must match a currently installed but unused certificate of the provided type | [045-certificate.md](../06-operation-commands/045-certificate.md) |
| `local-certificate` | User defined label | [159-local-certificate.md](../06-operation-commands/159-local-certificate.md) |
| `trusted-certificate` | User defined label | [359-trusted-certificate.md](../06-operation-commands/359-trusted-certificate.md) |
| `peer-certificate` | User-defined label | [234-peer-certificate.md](../06-operation-commands/234-peer-certificate.md) |
| `import-certificate` | Import any intermediate certificates present in a PEM string bundle | [131-import-certificate.md](../06-operation-commands/131-import-certificate.md) |
| `display-cert` | Defines the requested type of display operation | [083-display-cert.md](../06-operation-commands/083-display-cert.md) |
| `csr-gen` | The Extended Key Usage type(s) for the certificate | [064-csr-gen.md](../06-operation-commands/064-csr-gen.md) |
| `cert-gen` | Auto-assign certificate to any secure-application without active certificate | [043-cert-gen.md](../06-operation-commands/043-cert-gen.md) |
| `crl` | The HTTP URI from which this CRL was auto-downloaded | [063-crl.md](../06-operation-commands/063-crl.md) |
| `cdp` | Result of the most recent CRL update | [042-cdp.md](../06-operation-commands/042-cdp.md) |
| `ocsp-server` | Timestamp of last successful query | [206-ocsp-server.md](../06-operation-commands/206-ocsp-server.md) |
| `est` | The credentials used to authenticate a user when accessing resources protected by the HTTP protocol | [095-est.md](../06-operation-commands/095-est.md) |
| `est-ca` | Specifies the number of days before expiration at which re-enrollment will be performed for all... | [096-est-ca.md](../06-operation-commands/096-est-ca.md) |
| `est-server` | Specifies an optional label added to the EST base url | [097-est-server.md](../06-operation-commands/097-est-server.md) |
| `cert-to-name` | Specifies the user label | [044-cert-to-name.md](../06-operation-commands/044-cert-to-name.md) |

## Configuration templates and defaults

*Search terms:* `template`, `default`, `named-value`, `advanced parameter`

| Command | What it does | File |
| --- | --- | --- |
| `template` | Represents the condition to apply on the template (e.g. service-type=OTU4)- optional | [345-template.md](../06-operation-commands/345-template.md) |
| `template-group` | Represents the label to apply on the template - optional | [346-template-group.md](../06-operation-commands/346-template-group.md) |
| `templates` | This command is used to show the configuration that defines the data model for system templates | [347-templates.md](../06-operation-commands/347-templates.md) |
| `apply-template` | Applicable TOMS | [023-apply-template.md](../06-operation-commands/023-apply-template.md) |
| `default` | Attribute names to be defaulted | [075-default.md](../06-operation-commands/075-default.md) |
| `named-value-set` | Value item | [184-named-value-set.md](../06-operation-commands/184-named-value-set.md) |
| `advanced-parameter` | The current state of the advanced parameter | [013-advanced-parameter.md](../06-operation-commands/013-advanced-parameter.md) |
| `current-advanced-parameter` | The value of the advanced parameter, which is running on the system | [065-current-advanced-parameter.md](../06-operation-commands/065-current-advanced-parameter.md) |
| `golden-advanced-parameter` | Identifies if applying this parameter change causes service impact | [119-golden-advanced-parameter.md](../06-operation-commands/119-golden-advanced-parameter.md) |
| `gapt` | The managed resource type(s) that are applicable for this particular advanced parameter | [116-gapt.md](../06-operation-commands/116-gapt.md) |
| `extended-config` | Displays the description of the extended-config provided by the system and its effect in the system | [103-extended-config.md](../06-operation-commands/103-extended-config.md) |
| `property` | The property to be set | [249-property.md](../06-operation-commands/249-property.md) |
| `equipment-templates` | Whether serdes-templates are globally enabled or not | [094-equipment-templates.md](../06-operation-commands/094-equipment-templates.md) |

## DNS and DHCP

*Search terms:* `dns`, `dhcp`, `domain name`, `relay`

| Command | What it does | File |
| --- | --- | --- |
| `dns` | DNS-search-suffix name | [084-dns.md](../06-operation-commands/084-dns.md) |
| `dns-server` | DNS address assignment method, the user can convert DHCP configured DNS entry into a manual... | [085-dns-server.md](../06-operation-commands/085-dns-server.md) |
| `dhcp-relay` | DHCP server ip-addresses; when enabled at least one IP address should be configured | [078-dhcp-relay.md](../06-operation-commands/078-dhcp-relay.md) |
| `if-dhcp-relay` | Enables dhcp-relay function on this interface | [126-if-dhcp-relay.md](../06-operation-commands/126-if-dhcp-relay.md) |

## Data path / Layer 1 encryption

*Search terms:* `encryption`, `secure entity`, `secure application`, `data-path`

| Command | What it does | File |
| --- | --- | --- |
| `data-path-encryption` | This command is used to show datapath encryption attributes | [071-data-path-encryption.md](../06-operation-commands/071-data-path-encryption.md) |
| `secure-entity` | If the re-key fail policy is set to KILL-TRAFFIC, this attribute indicates the amount of time the... | [277-secure-entity.md](../06-operation-commands/277-secure-entity.md) |
| `secure-entity-sa-proposal` | Secure entity SA Diffie-Hellman group advertised to the far-end secure entity peer | [278-secure-entity-sa-proposal.md](../06-operation-commands/278-secure-entity-sa-proposal.md) |
| `secure-application` | Enables or disables TLS Mutual Authentication | [276-secure-application.md](../06-operation-commands/276-secure-application.md) |
| `peer-certificate` | User-defined label | [234-peer-certificate.md](../06-operation-commands/234-peer-certificate.md) |
| `local-certificate` | User defined label | [159-local-certificate.md](../06-operation-commands/159-local-certificate.md) |

## Database backup, snapshot and restore

*Search terms:* `snapshot`, `database`, `recovery`, `migrate`

| Command | What it does | File |
| --- | --- | --- |
| `database` | The password for the new-admin-user that is auto-configured after the database is wiped | [072-database.md](../06-operation-commands/072-database.md) |
| `take-snapshot` | Optional description for the generated snapshot | [342-take-snapshot.md](../06-operation-commands/342-take-snapshot.md) |
| `activate-snapshot` | Action to override the sanity check | [009-activate-snapshot.md](../06-operation-commands/009-activate-snapshot.md) |
| `db-migrate` | defines the protection mode to be configured | [073-db-migrate.md](../06-operation-commands/073-db-migrate.md) |
| `db-protection-scheme` | Current Protection Scheme of DB. Can be changed via 'db-migrate' RPC | [074-db-protection-scheme.md](../06-operation-commands/074-db-protection-scheme.md) |
| `recovery` | Timestamp for the next backup to be performed | [261-recovery.md](../06-operation-commands/261-recovery.md) |
| `recover-mode` | Forces the command without confirmation | [260-recover-mode.md](../06-operation-commands/260-recover-mode.md) |

## Equipment protection and switchover

*Search terms:* `protection`, `switchover`, `switch`, `redundan`

| Command | What it does | File |
| --- | --- | --- |
| `protection` | This command is used to show protection | [250-protection.md](../06-operation-commands/250-protection.md) |
| `protection-group` | Specifies the last reason that triggered a protection switchover | [251-protection-group.md](../06-operation-commands/251-protection-group.md) |
| `protection-switch` | The target of the switch command | [252-protection-switch.md](../06-operation-commands/252-protection-switch.md) |
| `protection-unit` | Protection unit role | [253-protection-unit.md](../06-operation-commands/253-protection-unit.md) |
| `manual-switchover` | The object to be manually switched | [177-manual-switchover.md](../06-operation-commands/177-manual-switchover.md) |

## Ethernet and client facilities

*Search terms:* `ethernet`, `client`, `fibre channel`, `interlaken`, `stm`, `zr`

| Command | What it does | File |
| --- | --- | --- |
| `ethernet` | System configured circuit ID | [099-ethernet.md](../06-operation-commands/099-ethernet.md) |
| `eth-zr` | Loopback on modem interface | [098-eth-zr.md](../06-operation-commands/098-eth-zr.md) |
| `fc` | System configured circuit ID, present in XCONs and associated facilities | [106-fc.md](../06-operation-commands/106-fc.md) |
| `interlaken` | Total capacity for the interlaken interface | [136-interlaken.md](../06-operation-commands/136-interlaken.md) |
| `stm` | The system configured circuit ID | [313-stm.md](../06-operation-commands/313-stm.md) |
| `trib-ptp` | -m | [358-trib-ptp.md](../06-operation-commands/358-trib-ptp.md) |
| `line-ptp` | Provide an aggregate view of all used resources on the DSP | [153-line-ptp.md](../06-operation-commands/153-line-ptp.md) |
| `facilities` | This command is used to show system facilities | [105-facilities.md](../06-operation-commands/105-facilities.md) |

## File transfer and file servers

*Search terms:* `download`, `upload`, `file server`, `transfer`, `sftp`, `directory`

| Command | What it does | File |
| --- | --- | --- |
| `download` | The password for the new-admin-user that is auto-configured after the database is wiped | [086-download.md](../06-operation-commands/086-download.md) |
| `upload` | X509v3 local/peer/trusted certificate name to be uploaded | [364-upload.md](../06-operation-commands/364-upload.md) |
| `transfer` | List of keywords associated with optional content to be selected for debug-log upload | [356-transfer.md](../06-operation-commands/356-transfer.md) |
| `transfer-status` | Details of transfer phase | [357-transfer-status.md](../06-operation-commands/357-transfer-status.md) |
| `file` | Filepath of the file to be deleted | [108-file.md](../06-operation-commands/108-file.md) |
| `file-operation` | The path to the file | [109-file-operation.md](../06-operation-commands/109-file-operation.md) |
| `file-server` | User-defined label for the server | [110-file-server.md](../06-operation-commands/110-file-server.md) |
| `http-file-server` | The base URL used to redirect to the file transfer application | [124-http-file-server.md](../06-operation-commands/124-http-file-server.md) |
| `file-type` | Last transfer operation | [111-file-type.md](../06-operation-commands/111-file-type.md) |
| `downloads` | This command is used to show a list of downloads | [088-downloads.md](../06-operation-commands/088-downloads.md) |

## Firmware (FW) management

*Search terms:* `firmware`, `fw`

| Command | What it does | File |
| --- | --- | --- |
| `current-fw` | Status for this particular firmware. current - Current firmware is up-to-date. not-current -... | [067-current-fw.md](../06-operation-commands/067-current-fw.md) |
| `packaged-fw` | Included version of the firmware | [232-packaged-fw.md](../06-operation-commands/232-packaged-fw.md) |
| `third-party-fw` | List of resources that this firmware can be applied to | [350-third-party-fw.md](../06-operation-commands/350-third-party-fw.md) |
| `activate` | Specific entity in the system for activating the loopback | [008-activate.md](../06-operation-commands/008-activate.md) |

## IP addressing and interfaces

*Search terms:* `ip address`, `ipv4`, `ipv6`, `interface`, `subnet`, `mtu`

| Command | What it does | File |
| --- | --- | --- |
| `interface` | User defined label | [134-interface.md](../06-operation-commands/134-interface.md) |
| `ipv4-address` | IPv4 address assignment method. static: Indicates that the address has been statically\n configured... | [143-ipv4-address.md](../06-operation-commands/143-ipv4-address.md) |
| `ipv6-address` | IPv6 address assignment method. static: Indicates that the address has been statically\n configured... | [145-ipv6-address.md](../06-operation-commands/145-ipv6-address.md) |
| `supporting-interface` | A reference to the IPv4/IPv6 interface | [331-supporting-interface.md](../06-operation-commands/331-supporting-interface.md) |
| `networking` | Interface to use as source address | [190-networking.md](../06-operation-commands/190-networking.md) |
| `networking-services` | This command is used to show the list of network services | [191-networking-services.md](../06-operation-commands/191-networking-services.md) |
| `comm-eth` | The operational state of this object | [054-comm-eth.md](../06-operation-commands/054-comm-eth.md) |
| `comm-channel` | Indicates the mode of operation of control channel | [053-comm-channel.md](../06-operation-commands/053-comm-channel.md) |
| `L2-bridge` | Description of the bridge and its intended purpose | [170-l2-bridge.md](../06-operation-commands/170-l2-bridge.md) |

## IPsec and IKEv2

*Search terms:* `ipsec`, `ikev2`, `ike`, `security association`, `traffic selector`, `re-key`

| Command | What it does | File |
| --- | --- | --- |
| `ike-sa-proposal` | A list of protocol proposals when negotiating the IKE SA + with the far-end IKE peer | [127-ike-sa-proposal.md](../06-operation-commands/127-ike-sa-proposal.md) |
| `ikev2` | A global, L1 encryption-specific policy that indicates whether the NE must validate Certificate... | [128-ikev2.md](../06-operation-commands/128-ikev2.md) |
| `ikev2-local-instance` | Local IPv4 address for IKEv2 channel with prefix-length 32 | [129-ikev2-local-instance.md](../06-operation-commands/129-ikev2-local-instance.md) |
| `ikev2-peer` | Indicates whether PPK use is mandatory or optional for the IKEv2 peer. i Note: If this parameter is... | [130-ikev2-peer.md](../06-operation-commands/130-ikev2-peer.md) |
| `ipsec-sa-proposal` | Extended Sequence Number (ESN) support | [139-ipsec-sa-proposal.md](../06-operation-commands/139-ipsec-sa-proposal.md) |
| `ipsec-sa-re-key` | The rekeying frequency for the IPsec child security association with the far-end peer based on... | [140-ipsec-sa-re-key.md](../06-operation-commands/140-ipsec-sa-re-key.md) |
| `ipsec-spd-entry` | Indicates whether dynamic traffic selector is enabled in this SPD entry | [141-ipsec-spd-entry.md](../06-operation-commands/141-ipsec-spd-entry.md) |
| `ipsec-traffic-selector` | Indicates the inner protocol (upper layer), obtained from the IPv4 protocol or the IPv6 next header field | [142-ipsec-traffic-selector.md](../06-operation-commands/142-ipsec-traffic-selector.md) |
| `security-policy-database` | List of all SPD entries associated with this far-end peer for which IKE negotiates security... | [281-security-policy-database.md](../06-operation-commands/281-security-policy-database.md) |
| `local-subnet` | This is a list of ranges of IPv4/IPv6 addresses (unicast, broadcast (IPv4 only)) | [161-local-subnet.md](../06-operation-commands/161-local-subnet.md) |
| `remote-subnet` | This is a list of ranges of IPv4/IPv6 addresses (unicast, broadcast (IPv4 only)) | [263-remote-subnet.md](../06-operation-commands/263-remote-subnet.md) |
| `local-ports` | The values for the stopping port | [160-local-ports.md](../06-operation-commands/160-local-ports.md) |
| `remote-ports` | The values for the stopping port | [262-remote-ports.md](../06-operation-commands/262-remote-ports.md) |
| `re-key` | Points to secure entity object (Child SA) | [259-re-key.md](../06-operation-commands/259-re-key.md) |
| `re-auth` | A reference to the IKE peer object (IKE SA) | [258-re-auth.md](../06-operation-commands/258-re-auth.md) |
| `additional-key-exchange` | A list of IKE SA Diffie-Hellman groups + advertised to the far-end IKE peer | [011-additional-key-exchange.md](../06-operation-commands/011-additional-key-exchange.md) |
| `encryption-algorithm` | The IKE SA encryption algorithm key length | [091-encryption-algorithm.md](../06-operation-commands/091-encryption-algorithm.md) |
| `ospfv3-ipsec-security-association` | Indicates IPsec mode | [222-ospfv3-ipsec-security-association.md](../06-operation-commands/222-ospfv3-ipsec-security-association.md) |

## Image signing and root keys

*Search terms:* `image signing`, `root key`, `isk`, `krk`, `key replacement`

| Command | What it does | File |
| --- | --- | --- |
| `ISK` | Signature Generation Time | [147-isk.md](../06-operation-commands/147-isk.md) |
| `KRK` | Key Payload (hex format) | [150-krk.md](../06-operation-commands/150-krk.md) |
| `key-replacement-package` | Indicates if this KRP has been installed in the system | [148-key-replacement-package.md](../06-operation-commands/148-key-replacement-package.md) |

## Inventory and capabilities

*Search terms:* `inventory`, `capabilit`, `supported`, `part number`

| Command | What it does | File |
| --- | --- | --- |
| `inventory` | not-applicable - Card doesn't have upgradeable firmware. current - All components have current... | [137-inventory.md](../06-operation-commands/137-inventory.md) |
| `unprovisioned-inventory` | Timestamp with the last time the unprovisioned equipment was detected by the Node Controller | [361-unprovisioned-inventory.md](../06-operation-commands/361-unprovisioned-inventory.md) |
| `capabilities` | the name of the card | [039-capabilities.md](../06-operation-commands/039-capabilities.md) |
| `supported-card` | Supported features; may be empty if no features are not supported | [321-supported-card.md](../06-operation-commands/321-supported-card.md) |
| `supported-chassis` | Supported features | [323-supported-chassis.md](../06-operation-commands/323-supported-chassis.md) |
| `supported-slot` | List of LEDs available in the slot | [327-supported-slot.md](../06-operation-commands/327-supported-slot.md) |
| `supported-port` | Indicates if TOMs that are plugged on this port type are auto migrated according with the... | [325-supported-port.md](../06-operation-commands/325-supported-port.md) |
| `supported-tom` | The phy-mode that is used by default in this TOM for this card | [328-supported-tom.md](../06-operation-commands/328-supported-tom.md) |
| `fru-info` | Type of the equipment | [114-fru-info.md](../06-operation-commands/114-fru-info.md) |
| `l0-capabilities` | The command described in this section is used to show the capabilities details related with node's... | [151-l0-capabilities.md](../06-operation-commands/151-l0-capabilities.md) |
| `oadm-capabilities` | Maximum number of ADGs (Add/ Drop Group(s)); 0 if not supported | [200-oadm-capabilities.md](../06-operation-commands/200-oadm-capabilities.md) |

## LLDP and neighbor discovery

*Search terms:* `lldp`, `neighbor`, `discovery`, `tlv`, `icdp`, `sndp`

| Command | What it does | File |
| --- | --- | --- |
| `lldp` | Time to keep neighbor information, in case neighbor does not have an explicit Time-To-Live (TTL) TLV | [155-lldp.md](../06-operation-commands/155-lldp.md) |
| `lldp-local-info` | This attribute describes the remote system enabled capabilities | [156-lldp-local-info.md](../06-operation-commands/156-lldp-local-info.md) |
| `lldp-neighbor` | Remote system info Time-To-Live (TTL) | [157-lldp-neighbor.md](../06-operation-commands/157-lldp-neighbor.md) |
| `lldp-port-statistics` | This counter provides a count of all TLVs not recognized by the receiving LLDP local agent | [158-lldp-port-statistics.md](../06-operation-commands/158-lldp-port-statistics.md) |
| `custom-tlv` | The sub-type identifier of the TLV in the scope of the OUI The firmware name | [069-custom-tlv.md](../06-operation-commands/069-custom-tlv.md) |
| `icdp` | Flag to enable icdp | [125-icdp.md](../06-operation-commands/125-icdp.md) |
| `sndp` | This is a switch to control the sndp feature | [296-sndp.md](../06-operation-commands/296-sndp.md) |
| `carrier-neighbor` | IPv6 loopback address of the neighbor; may be empty if not configured | [041-carrier-neighbor.md](../06-operation-commands/041-carrier-neighbor.md) |
| `interface-neighbor` | Resource Access Identifier (AID) | [135-interface-neighbor.md](../06-operation-commands/135-interface-neighbor.md) |
| `inci` | Switch to enable INCI | [132-inci.md](../06-operation-commands/132-inci.md) |
| `inci-neighbor` | The operational state of this object | [133-inci-neighbor.md](../06-operation-commands/133-inci-neighbor.md) |

## Loopbacks, test signals and BERT

*Search terms:* `loopback`, `test signal`, `bert`, `prbs`, `diagnostic`

| Command | What it does | File |
| --- | --- | --- |
| `bert` | specifies the duration of the test is run in seconds | [028-bert.md](../06-operation-commands/028-bert.md) |
| `odu-diagnostics` | Monitor the incoming test signals for diagnostics | [208-odu-diagnostics.md](../06-operation-commands/208-odu-diagnostics.md) |
| `otu-diagnostics` | The threshold in percentage of block errors versus total blocks at which a degrade-interval number... | [230-otu-diagnostics.md](../06-operation-commands/230-otu-diagnostics.md) |
| `ots-diagnostics` | The port-id in OTS TTI is the AID of the port but limited to 32 printable characters | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |
| `stm` | The system configured circuit ID | [313-stm.md](../06-operation-commands/313-stm.md) |
| `ethernet` | System configured circuit ID | [099-ethernet.md](../06-operation-commands/099-ethernet.md) |
| `odu` | Provides an aggregate view of used resources in the DSP | [207-odu.md](../06-operation-commands/207-odu.md) |

## MACsec

*Search terms:* `macsec`, `mka`, `secure channel`, `sak`

| Command | What it does | File |
| --- | --- | --- |
| `macsec-entity` | Number of packets to consider for replay protection window | [171-macsec-entity.md](../06-operation-commands/171-macsec-entity.md) |
| `macsec-mka` | Indicates whether PSK lifetime notification is enabled or disabled | [172-macsec-mka.md](../06-operation-commands/172-macsec-mka.md) |
| `mka-policy` | Secure Association Key(SAK) rekey interval in seconds | [173-mka-policy.md](../06-operation-commands/173-mka-policy.md) |
| `sc-rx` | State of the secure channel returned by MKA stack: • in-use: Indicates Secure Association(s) under... | [273-sc-rx.md](../06-operation-commands/273-sc-rx.md) |
| `sc-tx` | State of the secure channel returned by MKA stack: • in-use: Indicates Secure Association(s) under... | [274-sc-tx.md](../06-operation-commands/274-sc-tx.md) |

## Multi-chassis and node controller

*Search terms:* `multi-chassis`, `node controller`, `nct`, `chassis`

| Command | What it does | File |
| --- | --- | --- |
| `nct-connection` | The state of the dst-chassis | [185-nct-connection.md](../06-operation-commands/185-nct-connection.md) |
| `unprovisioned-inventory` | Timestamp with the last time the unprovisioned equipment was detected by the Node Controller | [361-unprovisioned-inventory.md](../06-operation-commands/361-unprovisioned-inventory.md) |
| `chassis` | Indicates if the chassis power consumption is limited by reducing max fan speed. i Note: This... | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `management-address` | The Object Identifier (OID) value used to identify the type of hardware component or protocol... | [174-management-address.md](../06-operation-commands/174-management-address.md) |
| `management-address-local` | The Object Identifier (OID) value used to identify the type of hardware component or protocol... | [175-management-address-local.md](../06-operation-commands/175-management-address-local.md) |

## NETCONF, RESTCONF, gNMI and YANG

*Search terms:* `netconf`, `restconf`, `grpc`, `gnmi`, `yang`, `data model`

| Command | What it does | File |
| --- | --- | --- |
| `netconf` | List of YANG identifiers that are statically included in notifications | [188-netconf.md](../06-operation-commands/188-netconf.md) |
| `restconf` | Root of the RESTCONF API | [266-restconf.md](../06-operation-commands/266-restconf.md) |
| `grpc` | Allows to configure the granularity of data in gNMI Get responses, when encoded with JSON. •... | [121-grpc.md](../06-operation-commands/121-grpc.md) |
| `data-model` | Allows to load/unload this data model | [070-data-model.md](../06-operation-commands/070-data-model.md) |
| `cli` | Columns to display in the output of 'show alarm' CLI command | [050-cli.md](../06-operation-commands/050-cli.md) |
| `convert` | CLI command; should be enclosed in quotes; if multiple commands are to be converted, they should be... | [062-convert.md](../06-operation-commands/062-convert.md) |

## NTP and system time

*Search terms:* `ntp`, `time`, `clock`, `timezone`

| Command | What it does | File |
| --- | --- | --- |
| `ntp` | The system contains manual and dhcp configured values | [195-ntp.md](../06-operation-commands/195-ntp.md) |
| `ntp-key` | Indicates a trusted NTP key | [196-ntp-key.md](../06-operation-commands/196-ntp-key.md) |
| `ntp-server` | Controls the reporting of alarms for this particular object. allowed - Alarm reporting is allowed.... | [197-ntp-server.md](../06-operation-commands/197-ntp-server.md) |
| `ntp-server-status` | Condition of NTP server | [198-ntp-server-status.md](../06-operation-commands/198-ntp-server-status.md) |
| `clock` | Indicates last system time jump in the format '&lt;time1&gt; to &lt;time2&gt;'. Time jumps of less... | [052-clock.md](../06-operation-commands/052-clock.md) |
| `set-time` | Time to set in the system | [289-set-time.md](../06-operation-commands/289-set-time.md) |
| `time` | This command is used to display the system's time | [351-time.md](../06-operation-commands/351-time.md) |

## Node, NE-level settings and status

*Search terms:* `network element`, `system`, `status`, `uptime`, `restart`

| Command | What it does | File |
| --- | --- | --- |
| `ne` | Controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is... | [186-ne.md](../06-operation-commands/186-ne.md) |
| `ne-function` | This command is used to show the Network Element (NE) function | [187-ne-function.md](../06-operation-commands/187-ne-function.md) |
| `system` | The attribute of the object-id | [340-system.md](../06-operation-commands/340-system.md) |
| `status` | For some dashboards, allows to specify an AID filter, reducing the scope of the output | [312-status.md](../06-operation-commands/312-status.md) |
| `uptime` | This command displays the system uptime and load average | [365-uptime.md](../06-operation-commands/365-uptime.md) |
| `restart` | Card HW or SW sub-component to restart | [265-restart.md](../06-operation-commands/265-restart.md) |
| `resources` | Available bandwidth for the paired slot connection. i Note: This parameter is applicable only for... | [264-resources.md](../06-operation-commands/264-resources.md) |

## OSPF routing

*Search terms:* `ospf`, `area`, `lsa`, `adjacency`

| Command | What it does | File |
| --- | --- | --- |
| `ospf` | The id of the ospf-instance needs to be provided as &lt;instance&gt; | [216-ospf.md](../06-operation-commands/216-ospf.md) |
| `ospf-instance` | Flag to indicate router-id is loopback IP or manual configured | [219-ospf-instance.md](../06-operation-commands/219-ospf-instance.md) |
| `ospf-area` | OSPF Router Area Type | [217-ospf-area.md](../06-operation-commands/217-ospf-area.md) |
| `ospf-area-range` | Advertise or hide | [218-ospf-area-range.md](../06-operation-commands/218-ospf-area-range.md) |
| `ospf-interface` | Authentication key string in ASCII format | [220-ospf-interface.md](../06-operation-commands/220-ospf-interface.md) |
| `ospf-neighbor` | OSPF router priority | [221-ospf-neighbor.md](../06-operation-commands/221-ospf-neighbor.md) |
| `ospfv3-ipsec-security-association` | Indicates IPsec mode | [222-ospfv3-ipsec-security-association.md](../06-operation-commands/222-ospfv3-ipsec-security-association.md) |

## OTDR and fiber diagnostics

*Search terms:* `otdr`, `reflect`, `fiber`, `trace`

| Command | What it does | File |
| --- | --- | --- |
| `otdr` | Displays which pre-defined OTDR measurement profile is in progress:<br>• none: Indicates automatic... | [223-otdr.md](../06-operation-commands/223-otdr.md) |
| `otdr-ptp` | The last OTDR measurement the generated .sor file | [224-otdr-ptp.md](../06-operation-commands/224-otdr-ptp.md) |
| `ots-r-auto-otdr` | Displays the status of the automatic OTDR execution for the corresponding OTS-R facility:<br>•... | [228-ots-r-auto-otdr.md](../06-operation-commands/228-ots-r-auto-otdr.md) |
| `ots-diagnostics` | The port-id in OTS TTI is the AID of the port but limited to 32 printable characters | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |

## OTN: ODU, OTU and cross-connects

*Search terms:* `odu`, `otu`, `otn`, `cross connect`, `xcon`, `tti`

| Command | What it does | File |
| --- | --- | --- |
| `odu` | Provides an aggregate view of used resources in the DSP | [207-odu.md](../06-operation-commands/207-odu.md) |
| `otu` | Time slots of the ODU | [229-otu.md](../06-operation-commands/229-otu.md) |
| `odu-diagnostics` | Monitor the incoming test signals for diagnostics | [208-odu-diagnostics.md](../06-operation-commands/208-odu-diagnostics.md) |
| `otu-diagnostics` | The threshold in percentage of block errors versus total blocks at which a degrade-interval number... | [230-otu-diagnostics.md](../06-operation-commands/230-otu-diagnostics.md) |
| `xcon` | List of resources being used by this XCON besides the two main source/destination end-points | [373-xcon.md](../06-operation-commands/373-xcon.md) |
| `nw-xconnect` | Maximum bandwidth rate of the user channel (in Mbps units) | [199-nw-xconnect.md](../06-operation-commands/199-nw-xconnect.md) |
| `network-xconnect` | This command is used to show the list of services of multiple user cross connections commissioned... | [189-network-xconnect.md](../06-operation-commands/189-network-xconnect.md) |
| `flexo` | The received iid on the FlexO interface | [112-flexo.md](../06-operation-commands/112-flexo.md) |
| `flexo-group` | Indicates the interface group instance that the FlexO-x interface is a member of | [113-flexo-group.md](../06-operation-commands/113-flexo-group.md) |

## Optical power control and profiles

*Search terms:* `power`, `attenuation`, `profile`, `target`

| Command | What it does | File |
| --- | --- | --- |
| `profile-control` | Profile data to be inputted | [248-profile-control.md](../06-operation-commands/248-profile-control.md) |
| `supported-power-profile` | Whether is the default value or not | [326-supported-power-profile.md](../06-operation-commands/326-supported-power-profile.md) |
| `supported-tom-power` | Maximum power in watts the host port allows for this pluggable type under supported-power-class | [329-supported-tom-power.md](../06-operation-commands/329-supported-tom-power.md) |
| `spectrum-control` | The intended target output power for the spectra | [304-spectrum-control.md](../06-operation-commands/304-spectrum-control.md) |
| `amplifier` | Control speed factor for the DGE power control algorithm | [019-amplifier.md](../06-operation-commands/019-amplifier.md) |

## Optical sections OTS / OMS / OPS / OSC

*Search terms:* `ots`, `oms`, `ops`, `osc`, `supervisory`, `section`

| Command | What it does | File |
| --- | --- | --- |
| `ots` | Currently this attribute is applicable to SLTE only | [225-ots.md](../06-operation-commands/225-ots.md) |
| `ots-r` | Connected Reference | [227-ots-r.md](../06-operation-commands/227-ots-r.md) |
| `oms` | System reports this attribute to indicate whether the OMS is intended to be in use (instead of... | [209-oms.md](../06-operation-commands/209-oms.md) |
| `ops` | Intended for Y-cable expansion | [210-ops.md](../06-operation-commands/210-ops.md) |
| `osc` | Represents the actual received OSC power value measured at DWDM Line port input | [215-osc.md](../06-operation-commands/215-osc.md) |
| `ots-diagnostics` | The port-id in OTS TTI is the AID of the port but limited to 32 printable characters | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |

## Performance monitoring, PM bins and thresholds

*Search terms:* `pm`, `performance`, `threshold`, `tca`, `bin`, `counter`

| Command | What it does | File |
| --- | --- | --- |
| `pm` | Resource Access Identifier (AID) | [236-pm.md](../06-operation-commands/236-pm.md) |
| `pm-catalog` | The catalog name | [237-pm-catalog.md](../06-operation-commands/237-pm-catalog.md) |
| `pm-control` | Real-time data supervision for this resource | [238-pm-control.md](../06-operation-commands/238-pm-control.md) |
| `pm-control-entry` | TCA supervision for this resource | [239-pm-control-entry.md](../06-operation-commands/239-pm-control-entry.md) |
| `pm-parameter` | Type of PM parameter, it can be either a counter or a gauge | [240-pm-parameter.md](../06-operation-commands/240-pm-parameter.md) |
| `pm-profile` | This parameter provides a way to globally enable PM data-supervision without having to toggle it... | [241-pm-profile.md](../06-operation-commands/241-pm-profile.md) |
| `pm-profile-entry` | For newly created resources of this type, whether they have PM threshold crossing supervision... | [242-pm-profile-entry.md](../06-operation-commands/242-pm-profile-entry.md) |
| `pm-resource` | Date and time of the last real time data reset for this resource | [243-pm-resource.md](../06-operation-commands/243-pm-resource.md) |
| `pm-threshold` | Configured high threshold value for resources that have this parameter | [244-pm-threshold.md](../06-operation-commands/244-pm-threshold.md) |
| `pm-threshold-profile` | Maximum value for this parameter | [245-pm-threshold-profile.md](../06-operation-commands/245-pm-threshold-profile.md) |
| `statistics` | Objects that will have their event counter statistics cleared | [311-statistics.md](../06-operation-commands/311-statistics.md) |
| `high-speed-monitoring` | User configurable port | [123-high-speed-monitoring.md](../06-operation-commands/123-high-speed-monitoring.md) |

## ROADM degrees, add/drop groups and switching

*Search terms:* `degree`, `adg`, `add/drop`, `roadm`, `oadm`, `switch`, `cross connection`

| Command | What it does | File |
| --- | --- | --- |
| `degree` | List of bands supported by a degree, with dependence on supported cards.<br>• not-applicable... | [076-degree.md](../06-operation-commands/076-degree.md) |
| `adg` | List of bands supported by an ADG, with dependence on supported cards.<br>• not-applicable... | [012-adg.md](../06-operation-commands/012-adg.md) |
| `modules-degree` | Instance of card or subcard that belongs to the degree | [182-modules-degree.md](../06-operation-commands/182-modules-degree.md) |
| `modules-adg` | Set upon creation, cannot be changed after supported-card being assigned | [181-modules-adg.md](../06-operation-commands/181-modules-adg.md) |
| `oadm-capabilities` | Maximum number of ADGs (Add/ Drop Group(s)); 0 if not supported | [200-oadm-capabilities.md](../06-operation-commands/200-oadm-capabilities.md) |
| `optical-switch` | SD threshold hysteresis (in dB) | [214-optical-switch.md](../06-operation-commands/214-optical-switch.md) |
| `oxcon` | Path/ service name of optical cross-connection | [231-oxcon.md](../06-operation-commands/231-oxcon.md) |
| `direction` | Instance of the card's port hosting this direction (index) | [081-direction.md](../06-operation-commands/081-direction.md) |
| `l0-capabilities` | The command described in this section is used to show the capabilities details related with node's... | [151-l0-capabilities.md](../06-operation-commands/151-l0-capabilities.md) |
| `ase-idler-service` | • enabled: ASE idler signal filling on the unused and nmc-failed portions of the band spectrum is... | [024-ase-idler-service.md](../06-operation-commands/024-ase-idler-service.md) |
| `ase-idler-source` | ASE pump output power required (if manually configured) | [025-ase-idler-source.md](../06-operation-commands/025-ase-idler-source.md) |

## Reachability tests

*Search terms:* `ping`, `traceroute`, `echo`, `monitoring`

| Command | What it does | File |
| --- | --- | --- |
| `ping` | IP address of the destination of the ICMP ECHO REQUEST datagram. _ | [235-ping.md](../06-operation-commands/235-ping.md) |
| `traceroute` | IP address of the destination of the ICMP ECHO REQUEST datagram. _ | [355-traceroute.md](../06-operation-commands/355-traceroute.md) |
| `ip-monitoring` | Controls the reporting of alarms for this particular object | [138-ip-monitoring.md](../06-operation-commands/138-ip-monitoring.md) |

## SNMP

*Search terms:* `snmp`, `community`, `trap`, `v3`

| Command | What it does | File |
| --- | --- | --- |
| `snmp` | SNMP engine boot count | [297-snmp.md](../06-operation-commands/297-snmp.md) |
| `snmp-community` | SNMP access right of this community string | [298-snmp-community.md](../06-operation-commands/298-snmp-community.md) |
| `snmp-target` | Type of transport for the SNMP target | [299-snmp-target.md](../06-operation-commands/299-snmp-target.md) |
| `snmpv3-user` | Specifies the SNMPv3 privacy pass phrase | [300-snmpv3-user.md](../06-operation-commands/300-snmpv3-user.md) |

## SSH keys and known hosts

*Search terms:* `ssh`, `key pair`, `known host`, `host key`

| Command | What it does | File |
| --- | --- | --- |
| `ssh` | Welcome message displayed after user login | [306-ssh.md](../06-operation-commands/306-ssh.md) |
| `ssh-keygen` | Specify type of key to generate | [309-ssh-keygen.md](../06-operation-commands/309-ssh-keygen.md) |
| `ssh-host-key` | Fingerprint string as a sequence of pairs of hex digits | [308-ssh-host-key.md](../06-operation-commands/308-ssh-host-key.md) |
| `ssh-authorized-key` | User defined label | [307-ssh-authorized-key.md](../06-operation-commands/307-ssh-authorized-key.md) |
| `ssh-known-host` | User defined label | [310-ssh-known-host.md](../06-operation-commands/310-ssh-known-host.md) |

## Scripting, tasks and automation

*Search terms:* `script`, `task`, `schedule`, `variable`, `alias`, `expect`

| Command | What it does | File |
| --- | --- | --- |
| `run` | Optional arguments to the script | [272-run.md](../06-operation-commands/272-run.md) |
| `task` | Output of the previous task run | [343-task.md](../06-operation-commands/343-task.md) |
| `scheduled-task` | Output of the previous task run | [275-scheduled-task.md](../06-operation-commands/275-scheduled-task.md) |
| `expect` | The expected value | [101-expect.md](../06-operation-commands/101-expect.md) |
| `export` | Value to replace variable with; can be any supported character, including spaces | [102-export.md](../06-operation-commands/102-export.md) |
| `sleep` | Duration of delay in seconds | [294-sleep.md](../06-operation-commands/294-sleep.md) |
| `message` | The CLI sessions to which the message will be sent | [180-message.md](../06-operation-commands/180-message.md) |
| `alias` | Value to replace the alias name with | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `unalias` | Name of the alias to remove | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `simulate` | The location of the simulated alarm | [293-simulate.md](../06-operation-commands/293-simulate.md) |

## SerDes and TOM templates

*Search terms:* `serdes`, `template`, `tom`

| Command | What it does | File |
| --- | --- | --- |
| `serdes` | State of the advanced parameter (as observable on the system) once it is configured | [282-serdes.md](../06-operation-commands/282-serdes.md) |
| `serdes-template` | The list of ports to which this template is applicable, or 'all' if all ports are to be considered... | [283-serdes-template.md](../06-operation-commands/283-serdes-template.md) |
| `serdes-template-entry` | Value of the serdes parameter | [284-serdes-template-entry.md](../06-operation-commands/284-serdes-template-entry.md) |
| `equipment-templates` | Whether serdes-templates are globally enabled or not | [094-equipment-templates.md](../06-operation-commands/094-equipment-templates.md) |
| `supported-tom` | The phy-mode that is used by default in this TOM for this card | [328-supported-tom.md](../06-operation-commands/328-supported-tom.md) |
| `supported-tom-power` | Maximum power in watts the host port allows for this pluggable type under supported-power-class | [329-supported-tom-power.md](../06-operation-commands/329-supported-tom-power.md) |

## Software upgrade

*Search terms:* `upgrade`, `software load`, `activate`, `swversion`

| Command | What it does | File |
| --- | --- | --- |
| `prepare-upgrade` | The password for the new-admin-user that is auto-configured after the database is wiped | [247-prepare-upgrade.md](../06-operation-commands/247-prepare-upgrade.md) |
| `activate` | Specific entity in the system for activating the loopback | [008-activate.md](../06-operation-commands/008-activate.md) |
| `cancel-upgrade` | Displays help for this command | [038-cancel-upgrade.md](../06-operation-commands/038-cancel-upgrade.md) |
| `upgrade-status` | Details on the current upgrade | [363-upgrade-status.md](../06-operation-commands/363-upgrade-status.md) |
| `swversion` | This command is used to retrieve the active, inactive and/or installable versions of the software... | [338-swversion.md](../06-operation-commands/338-swversion.md) |
| `software-load` | Software load package type | [301-software-load.md](../06-operation-commands/301-software-load.md) |
| `software-location` | Location of the equipment | [302-software-location.md](../06-operation-commands/302-software-location.md) |
| `sw-management` | Shows inactive software | [335-sw-management.md](../06-operation-commands/335-sw-management.md) |
| `downloaded-image` | Downloaded software image file signature | [087-downloaded-image.md](../06-operation-commands/087-downloaded-image.md) |
| `manifest` | Included version of the firmware | [176-manifest.md](../06-operation-commands/176-manifest.md) |
| `subtype-constraint` | Subtype description | [318-subtype-constraint.md](../06-operation-commands/318-subtype-constraint.md) |
| `sw-component` | Package information | [332-sw-component.md](../06-operation-commands/332-sw-component.md) |
| `sw-subcomponent` | Package information | [337-sw-subcomponent.md](../06-operation-commands/337-sw-subcomponent.md) |

## Spectrum, wavelength and channels

*Search terms:* `spectrum`, `frequency`, `wavelength`, `channel`, `carrier`, `thz`

| Command | What it does | File |
| --- | --- | --- |
| `spectrum` | Unique attenuation value for entire spectrum [dB] | [303-spectrum.md](../06-operation-commands/303-spectrum.md) |
| `spectrum-control` | The intended target output power for the spectra | [304-spectrum-control.md](../06-operation-commands/304-spectrum-control.md) |
| `spectrum-monitoring` | Currently calculated PSD. The Power Spectral Density does not depend on the spectra width | [305-spectrum-monitoring.md](../06-operation-commands/305-spectrum-monitoring.md) |
| `mc` | When enabled, the system may auto-delete this MC once it has no associated NMC. When disabled, the... | [178-mc.md](../06-operation-commands/178-mc.md) |
| `mc-f` | Slot width, as calculated by the system, from upper-frequency - lower-frequency | [179-mc-f.md](../06-operation-commands/179-mc-f.md) |
| `nmc` | When enabled, the system may auto-delete this NMC once it has no associated OXcon | [193-nmc.md](../06-operation-commands/193-nmc.md) |
| `nmc-f` | Network Media Channel attenuation adjustment applied by auto-controls to do power targeting in mux... | [194-nmc-f.md](../06-operation-commands/194-nmc-f.md) |
| `oc` | System configured circuit id | [201-oc.md](../06-operation-commands/201-oc.md) |
| `optical-carrier` | Controls enabling/disabling sop data collection, providing the collection interval in ms | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `optical-channel` | Describes whether this facility was system created or not | [212-optical-channel.md](../06-operation-commands/212-optical-channel.md) |
| `optical-ptp` | Fiber patch cord length between the Raman DWDM port and the base card DWDM line port | [213-optical-ptp.md](../06-operation-commands/213-optical-ptp.md) |
| `super-channel` | Theoretical total TX power at Faceplate calculated based on per carrier Target TX power value | [319-super-channel.md](../06-operation-commands/319-super-channel.md) |
| `super-channel-group` | -m | [320-super-channel-group.md](../06-operation-commands/320-super-channel-group.md) |
| `monitored-channel` | Carrier (channel) width configured at the NMC within the oxcon source/ destination, in MHz | [183-monitored-channel.md](../06-operation-commands/183-monitored-channel.md) |
| `ocm-channel` | Yields 'true' if the channel is configured (involved in an oxcon) | [203-ocm-channel.md](../06-operation-commands/203-ocm-channel.md) |
| `ocm-mp` | System reports 'enabled' when OMS reference exists | [204-ocm-mp.md](../06-operation-commands/204-ocm-mp.md) |
| `ocm-ptp` | System reports 'enabled' when complete connectivity at AD is established, and OCM measurement is possible | [205-ocm-ptp.md](../06-operation-commands/205-ocm-ptp.md) |
| `ochm` | DGE VOA attenuation of channel | [202-ochm.md](../06-operation-commands/202-ochm.md) |
| `supported-carrier-mode` | Subtypes that each carrier mode supports | [322-supported-carrier-mode.md](../06-operation-commands/322-supported-carrier-mode.md) |
| `golden-carrier-mode` | Subtypes for which this carrier mode has candidate status | [120-golden-carrier-mode.md](../06-operation-commands/120-golden-carrier-mode.md) |
| `gadt` | Detailed description of application ID | [115-gadt.md](../06-operation-commands/115-gadt.md) |
| `gcmt` | table version | [117-gcmt.md](../06-operation-commands/117-gcmt.md) |

## Static routes, RIB and VRF

*Search terms:* `static route`, `route`, `rib`, `vrf`, `next hop`

| Command | What it does | File |
| --- | --- | --- |
| `ipv4-static-route` | The routes to be advertised to external AS must exist in the forwarding table installed by an... | [144-ipv4-static-route.md](../06-operation-commands/144-ipv4-static-route.md) |
| `ipv6-static-route` | The routes to be advertised to external AS must exist in the forwarding table installed by an... | [146-ipv6-static-route.md](../06-operation-commands/146-ipv6-static-route.md) |
| `route` | Source protocol for the route entry | [269-route.md](../06-operation-commands/269-route.md) |
| `rib` | Address family | [267-rib.md](../06-operation-commands/267-rib.md) |
| `routing` | This command is used to show routing information | [270-routing.md](../06-operation-commands/270-routing.md) |
| `next-hop` | IP address of the next-hop | [192-next-hop.md](../06-operation-commands/192-next-hop.md) |
| `vrf` | Associated chassis name to this VRF | [372-vrf.md](../06-operation-commands/372-vrf.md) |

## Syslog and logging

*Search terms:* `log`, `syslog`, `facility`, `severity`

| Command | What it does | File |
| --- | --- | --- |
| `log` | The name of the log file to have it's contents removed | [163-log.md](../06-operation-commands/163-log.md) |
| `syslog` | User defined label | [339-syslog.md](../06-operation-commands/339-syslog.md) |
| `log-file` | Whether the local file has logs include sensitive data | [166-log-file.md](../06-operation-commands/166-log-file.md) |
| `log-file-facility-filter` | Describes the option to specify how the severity comparison is performed | [167-log-file-facility-filter.md](../06-operation-commands/167-log-file-facility-filter.md) |
| `log-server` | Flag indicating if alarm the reporting is allowed | [168-log-server.md](../06-operation-commands/168-log-server.md) |
| `log-server-facility-filter` | Describes the option to specify how the severity comparison is performed | [169-log-server-facility-filter.md](../06-operation-commands/169-log-server-facility-filter.md) |
| `log-console` | Switches on and off the console logging | [164-log-console.md](../06-operation-commands/164-log-console.md) |
| `log-console-facility-filter` | Describes the option to specify how the severity comparison is performed | [165-log-console-facility-filter.md](../06-operation-commands/165-log-console-facility-filter.md) |

## Telemetry and subscriptions

*Search terms:* `telemetry`, `subscription`, `dial-out`, `call home`

| Command | What it does | File |
| --- | --- | --- |
| `telemetry` | Persistent and dynamic telemetry | [344-telemetry.md](../06-operation-commands/344-telemetry.md) |
| `subscriptions` | This command is used to show a list of subscriptions | [317-subscriptions.md](../06-operation-commands/317-subscriptions.md) |
| `current-subscription` | Username in order to resolve paths according to user access | [068-current-subscription.md](../06-operation-commands/068-current-subscription.md) |
| `subscription-path` | Boolean flag to control suppression of redundant telemetry updates to the collector platform | [316-subscription-path.md](../06-operation-commands/316-subscription-path.md) |
| `dial-out-server` | Connection state to the dial-out-server | [079-dial-out-server.md](../06-operation-commands/079-dial-out-server.md) |
| `call-home` | The pre-configured name of the dial-out server | [037-call-home.md](../06-operation-commands/037-call-home.md) |

## Third-party applications and containers

*Search terms:* `third party`, `third-party`, `app`, `container`, `shell`

| Command | What it does | File |
| --- | --- | --- |
| `app` | Third party app name | [021-app.md](../06-operation-commands/021-app.md) |
| `appctl` | Optional parameters to be passed in the command with max-elements 50 | [022-appctl.md](../06-operation-commands/022-appctl.md) |
| `third-party-app` | Third-party-app enabled state | [349-third-party-app.md](../06-operation-commands/349-third-party-app.md) |
| `third-party-fw` | List of resources that this firmware can be applied to | [350-third-party-fw.md](../06-operation-commands/350-third-party-fw.md) |
| `gshell` | Command to execute inside the Guest Container | [122-gshell.md](../06-operation-commands/122-gshell.md) |
| `shell` | Displays help for this command | [290-shell.md](../06-operation-commands/290-shell.md) |
| `sw-container` | Time since the container started | [333-sw-container.md](../06-operation-commands/333-sw-container.md) |

## Topology and fiber connections

*Search terms:* `topology`, `fiber`, `cable`, `link`, `connection`

| Command | What it does | File |
| --- | --- | --- |
| `topology` | Topology instance to be viewed:<br>• inci - Refer to for inci (p. 549) additional information on... | [354-topology.md](../06-operation-commands/354-topology.md) |
| `links` | This command is used to show the links container within the topology | [154-links.md](../06-operation-commands/154-links.md) |
| `fiber-connection` | Type of the fiber connection | [107-fiber-connection.md](../06-operation-commands/107-fiber-connection.md) |
| `external-fiber-connection` | Type of the fiber connection | [104-external-fiber-connection.md](../06-operation-commands/104-external-fiber-connection.md) |
| `supporting-fiber-connection` | Supported fiber connection path | [330-supporting-fiber-connection.md](../06-operation-commands/330-supporting-fiber-connection.md) |
| `connection-ports` | The dwdm-line port of RD or ILAx card | [059-connection-ports.md](../06-operation-commands/059-connection-ports.md) |
| `submarine-link` | Allocated spectrum blocks for the link configured as a set of start frequency, end frequency pairs | [315-submarine-link.md](../06-operation-commands/315-submarine-link.md) |

## Users, passwords and user groups

*Search terms:* `user`, `password`, `group`, `privilege`, `login`

| Command | What it does | File |
| --- | --- | --- |
| `user` | User defined label | [367-user.md](../06-operation-commands/367-user.md) |
| `user-group` | Long description of the user group | [369-user-group.md](../06-operation-commands/369-user-group.md) |
| `user-data` | The commands described in this section are used to show the `user-data` | [368-user-data.md](../06-operation-commands/368-user-data.md) |
| `password` | The the new password inline with the command | [233-password.md](../06-operation-commands/233-password.md) |
| `authorization` | Number of times since the system last restarted that a notification was dropped for a subscription... | [027-authorization.md](../06-operation-commands/027-authorization.md) |
| `session` | Name of the dial-out-server associated with this session | [286-session.md](../06-operation-commands/286-session.md) |
| `kill-session` | An existing session-id | [149-kill-session.md](../06-operation-commands/149-kill-session.md) |

## Zero touch provisioning (ZTP)

*Search terms:* `ztp`, `zero touch`, `bootstrap`

| Command | What it does | File |
| --- | --- | --- |
| `ztp` | Summarized completion status of ZTP on the node | [374-ztp.md](../06-operation-commands/374-ztp.md) |
| `change-ztp-mode` | Enable or disable ztp | [046-change-ztp-mode.md](../06-operation-commands/046-change-ztp-mode.md) |
| `bootstrap` | Password for the new administrator account on the neighbor NE. Can be provided as a password hash (... | [032-bootstrap.md](../06-operation-commands/032-bootstrap.md) |
