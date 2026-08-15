# Topic index - query vocabulary to commands

Maps the words people actually use to the commands that implement them. Each entry lists **search terms** (synonyms, acronyms and phrasings that should trigger it) and the commands to open.

57 topics covering 389 distinct commands. Associations were assigned by reading each command's description in the source; they are editorial, not extracted, so treat them as routing hints and confirm against the command page.

## AAA, TACACS+ and RADIUS

*Search terms:* `aaa`, `tacacs`, `radius`, `authentication`

| Command | What it does | File |
| --- | --- | --- |
| `aaa-server` | This command is used to add/edit/show an AAA server | [001-aaa-server.md](../06-operation-commands/001-aaa-server.md) |
| `aaa-statistics` | This command can be used to view the AAA statistics for AAA servers that use the TACACS+ protocol | [002-aaa-statistics.md](../06-operation-commands/002-aaa-statistics.md) |
| `authorization` | The commands described in this section are used to set or show the `authorization` attributes | [027-authorization.md](../06-operation-commands/027-authorization.md) |
| `auth-key` | This command is used to add, edit or show a authorization key | [026-auth-key.md](../06-operation-commands/026-auth-key.md) |
| `cert-to-name` | This command defines a prioritized set of rules used to map an X.509 client certificate to a... | [044-cert-to-name.md](../06-operation-commands/044-cert-to-name.md) |

## Access control lists and rules

*Search terms:* `acl`, `access control`, `access rule`, `ace`, `permit`, `deny`, `rule`, `packet filter`, `filter rule`, `firewall`

| Command | What it does | File |
| --- | --- | --- |
| `acl` | These commands are used to add/delete an access control list (ACL) and set/show attributes... | [007-acl.md](../06-operation-commands/007-acl.md) |
| `ace` | This command is used to add/set attributes associated with every access control entry (ACE) | [006-ace.md](../06-operation-commands/006-ace.md) |
| `access-control-list` | This command is used to show access control list | [003-access-control-list.md](../06-operation-commands/003-access-control-list.md) |
| `access-rule` | The commands described in this section are used to add, set or show the `access-rule` attributes | [004-access-rule.md](../06-operation-commands/004-access-rule.md) |
| `access-rule-list` | The commands described in this section are used to add, set or show the `access-rule-list` attributes | [005-access-rule-list.md](../06-operation-commands/005-access-rule-list.md) |
| `security` | The command described in this section is used to show the top level security container | [279-security.md](../06-operation-commands/279-security.md) |
| `security-policies` | The commands described in this section are used to edit or show security-policies | [280-security-policies.md](../06-operation-commands/280-security-policies.md) |
| `authorization` | The commands described in this section are used to set or show the `authorization` attributes | [027-authorization.md](../06-operation-commands/027-authorization.md) |

## Alarms and conditions

*Search terms:* `alarm`, `condition`, `severity`, `acknowledge`

| Command | What it does | File |
| --- | --- | --- |
| `alarm` | This command is used to clear alarms that have no auto criteria to be cleared and to show currently... | [014-alarm.md](../06-operation-commands/014-alarm.md) |
| `current-alarms` | The command is used to show the list of currently raised alarms | [066-current-alarms.md](../06-operation-commands/066-current-alarms.md) |
| `alarm-control` | The commands described in this section are used to set or show the parameters related with alarm... | [015-alarm-control.md](../06-operation-commands/015-alarm-control.md) |
| `alarm-inventory` | The command described in this section is used to show the inventory with all possible alarm types... | [016-alarm-inventory.md](../06-operation-commands/016-alarm-inventory.md) |
| `alarm-severity-profile` | This command is used to set or show the alarm severity for a alarm profile | [018-alarm-severity-profile.md](../06-operation-commands/018-alarm-severity-profile.md) |
| `alarm-severity-entry` | The commands described in this section are used to set or show the individual entry in... | [017-alarm-severity-entry.md](../06-operation-commands/017-alarm-severity-entry.md) |
| `set-alarm-state` | The set-alarm-state changes the operator state of an alarm | [288-set-alarm-state.md](../06-operation-commands/288-set-alarm-state.md) |
| `get-conditions` | This command is used to retrieve conditions | [118-get-conditions.md](../06-operation-commands/118-get-conditions.md) |
| `simulate` | This command is used to trigger simulated events in the system (alarms, equipment, etc).... | [293-simulate.md](../06-operation-commands/293-simulate.md) |

## Amplifiers, gain and Raman pumps

*Search terms:* `amplifier`, `gain`, `raman`, `pump`, `tilt`

| Command | What it does | File |
| --- | --- | --- |
| `amplifier` | These commands are used to set or show the amplifier object attributes | [019-amplifier.md](../06-operation-commands/019-amplifier.md) |
| `amplifier-raman` | These commands are used to set or show the amplifier object attributes | [020-amplifier-raman.md](../06-operation-commands/020-amplifier-raman.md) |
| `pump` | These commands are used set up a pump | [255-pump.md](../06-operation-commands/255-pump.md) |
| `pump-power` | These commands are used to set up a Raman pump | [256-pump-power.md](../06-operation-commands/256-pump-power.md) |
| `raman-calibration` | The commands described in this section are used to add, delete, set or show the `raman-calibration`... | [257-raman-calibration.md](../06-operation-commands/257-raman-calibration.md) |
| `calibrate` | The command described in this section is used to calibrate the Raman gain | [036-calibrate.md](../06-operation-commands/036-calibrate.md) |
| `supported-gain-range` | This command is used to display the supported gain range | [324-supported-gain-range.md](../06-operation-commands/324-supported-gain-range.md) |
| `rsc` | These commands are used to set attributes for or show an RSC, Raman card Pilot Tone facility | [271-rsc.md](../06-operation-commands/271-rsc.md) |
| `ne-function` | This command is used to show the Network Element (NE) function | [187-ne-function.md](../06-operation-commands/187-ne-function.md) |

## BGP routing

*Search terms:* `bgp`, `peer`, `autonomous system`, `advertise`, `announce`, `upstream as`, `as `

| Command | What it does | File |
| --- | --- | --- |
| `bgp-instance` | This command is used to add/edit/show a bgp instance | [029-bgp-instance.md](../06-operation-commands/029-bgp-instance.md) |
| `bgp-neighbor` | This command is used to add/edit/show a BGP neighbor | [030-bgp-neighbor.md](../06-operation-commands/030-bgp-neighbor.md) |
| `bgp-network` | This command is used to add/edit/show a bgp network | [031-bgp-network.md](../06-operation-commands/031-bgp-network.md) |

## CLI navigation, output filtering and help

*Search terms:* `navigat`, `prompt`, `help`, `pipe`, `filter`, `tree`, `terminal`, `rows`, `columns`, `cli session`, `logged into`, `broadcast`, `alarm columns`, `create entity`, `managed entity`, `confirmation`, `hop to`, `long-running`, `which entities`, `creatable`

| Command | What it does | File |
| --- | --- | --- |
| `edit` | The edit command is used to navigate the managed entity hierarchy | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#42-edit) |
| `top` | The `top` command is used to bring the current path to the top of the managed entity hierarchy [ne] | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#44-top) |
| `up` | The `up` command is used to bring the current path up by one path level in the managed entity hierarchy | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#47-up) |
| `tree` | The `tree` command is used to display the managed entity hierarchy in a tree-like format | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#45-tree) |
| `history` | The `history` command is used to display the current session's command history as a numbered list | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#43-history) |
| `alias` | The `alias` command is used to define a more user-friendly alphanumeric string for one or more... | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `unalias` | The `unalias` command is used to remove an alias previously defined.. When using `unalias` command,... | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `help` | Displays help for a command, container, or attribute. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `?` | Contextual help: displays what can be typed at the current prompt. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `begin` | The `begin` command is used to display the output of the previous command starting from a specified word | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#51-begin) |
| `display` | The `display` command is used to allows to customize the output of the previous command, i.e., to... | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#52-display) |
| `exclude` | The `exclude` command is used to filter the output that contains a defined word or string (i.e.,... | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#53-exclude) |
| `grep` | The `grep` command is used to filter the output based on a defined word or string (i.e., only... | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#54-grep) |
| `highlight` | The `highlight` command is used to visually markup a word or set of words in the output of a given... | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#55-highlight) |
| `include` | The `include` command is used to filter the output to a defined word or string (i.e., only displays... | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#56-include) |
| `linenum` | The `linenum` command is used to add line numbers to output of the previous command | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#57-linenum) |
| `more` | The `more` command is used to display long outputs incrementally, page by page | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#58-more) |
| `sort` | The `sort` command is used to reorder the output of a command according to specified criteria | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#59-sort) |
| `until` | The `until` command is used to display the output of the previous command ending at a specified word | [05-piped-commands.md](../05-piped-commands/05-piped-commands.md#510-until) |
| `tic` | Starts a timer for the typed command. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `toc` | Displays the elapsed time since the timer was started. | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `cli` | These commands are used to set or show the configuration of the Command Line Interface (CLI)... | [050-cli.md](../06-operation-commands/050-cli.md) |
| `cli-session-config` | These commands are used to set or show the configuration of the Command Line Interface (CLI)... | [051-cli-session-config.md](../06-operation-commands/051-cli-session-config.md) |
| `add` | The `add` command is used to create a new managed entity | [010-add.md](../06-operation-commands/010-add.md) |
| `clear` | The clear command clears entries for the specified entity | [049-clear.md](../06-operation-commands/049-clear.md) |
| `connect` | The `connect` command described in this section is used to establish a ssh session directly from CLI | [058-connect.md](../06-operation-commands/058-connect.md) |
| `message` | This command is used to send a message to other CLI sessions | [180-message.md](../06-operation-commands/180-message.md) |
| `terminate` | This command is used to terminate a running operation. **Location led test Termination** By... | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `exit` | This command is used to logout of the current CLI mode | [100-exit.md](../06-operation-commands/100-exit.md) |

## CableID fiber verification

*Search terms:* `cableid`, `cable-id`, `verify`, `fibre verification`, `fiber verification`, `progress`

| Command | What it does | File |
| --- | --- | --- |
| `cable-id` | The commands described in this section are used to show the `cable-id` entities and terminate a... | [033-cable-id.md](../06-operation-commands/033-cable-id.md) |
| `cable-id-path` | The commands described in this section are used to show the `cable-id-path` attributes | [034-cable-id-path.md](../06-operation-commands/034-cable-id-path.md) |
| `cable-id-status` | The command described in this section is used to show the `cable-id-status` attributes | [035-cable-id-status.md](../06-operation-commands/035-cable-id-status.md) |
| `verify` | The command verify is used to trigger CableID-based fiber connections verification. **Fiber... | [371-verify.md](../06-operation-commands/371-verify.md) |
| `cid-ptp` | The commands described in this section are used to manage `cid-ptp` facility and its attributes | [048-cid-ptp.md](../06-operation-commands/048-cid-ptp.md) |

## Candidate configuration, commit and rollback

*Search terms:* `candidate`, `commit`, `rollback`, `datastore`, `discard`, `staged`, `configuration mode`, `exclusive`, `lock`, `throw away`, `abandon`, `changing configuration`, `write access`, `mastership`

| Command | What it does | File |
| --- | --- | --- |
| `configure` | This command is used to change to Candidate Configuration mode in order to edit a candidate datastore | [057-configure.md](../06-operation-commands/057-configure.md) |
| `commit` | This command is used to commit the contents of the candidate datastore | [055-commit.md](../06-operation-commands/055-commit.md) |
| `show commit` | The `show commit` retrieves the commit record information from the system | [292-show-commit.md](../06-operation-commands/292-show-commit.md) |
| `rollback` | The `rollback commit` must be executed using the commit parameter, and optionally a specific... | [268-rollback.md](../06-operation-commands/268-rollback.md) |
| `discard-changes` | This command will discard all candidate datastore content and CLI return to operational mode | [082-discard-changes.md](../06-operation-commands/082-discard-changes.md) |
| `validate` | This command is used to validate the contents of the specified configuration | [370-validate.md](../06-operation-commands/370-validate.md) |
| `diff` | This command is used to perform a diff comparison between a candidate configuration and the current... | [080-diff.md](../06-operation-commands/080-diff.md) |
| `lock` | This command is used to lock the database access to the current session | [162-lock.md](../06-operation-commands/162-lock.md) |
| `unlock` | This command will release a previously locked database (achieved by using the 'lock' command) | [360-unlock.md](../06-operation-commands/360-unlock.md) |
| `system-policies` | The commands described in this section are used to set or show the `system-policies` attributes.The... | [341-system-policies.md](../06-operation-commands/341-system-policies.md) |
| `config` | The `show config` displays the system's configuration | [056-config.md](../06-operation-commands/056-config.md) |

## Cards, slots, chassis and pluggables

*Search terms:* `card`, `slot`, `chassis`, `tom`, `pluggable`, `fru`, `equipment`, `console`, `baud`, `craft`, `standby controller`, `controller`

| Command | What it does | File |
| --- | --- | --- |
| `card` | These commands are used to add, edit, show or delete a card-base object | [040-card.md](../06-operation-commands/040-card.md) |
| `slot` | These commands are used to show the slot equipment holder details | [295-slot.md](../06-operation-commands/295-slot.md) |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `equipment` | This command is used to display installed equipment information | [092-equipment.md](../06-operation-commands/092-equipment.md) |
| `controller-card` | This command is used to display the configuration of a controller card | [061-controller-card.md](../06-operation-commands/061-controller-card.md) |
| `port` | These commands is used are set/show port attributes | [246-port.md](../06-operation-commands/246-port.md) |
| `tom` | These commands are used to add, set, show or delete a TOM (Tunable/non-tunable Optical Module) pluggable | [352-tom.md](../06-operation-commands/352-tom.md) |
| `tom-type` | This command is used to show the capabilities of the supported TOM (Tunable/non-tunable Optical... | [353-tom-type.md](../06-operation-commands/353-tom-type.md) |
| `sub-component` | This command is used to show the sub-component details or card resources | [314-sub-component.md](../06-operation-commands/314-sub-component.md) |
| `fru-info` | This command is used to display the packaged FRU information associated to a particular equipment-type | [114-fru-info.md](../06-operation-commands/114-fru-info.md) |
| `led` | These commands are used to show the representation of a LED in a FRU. Object exists even if FRU is... | [152-led.md](../06-operation-commands/152-led.md) |
| `usb` | This command shows the USB function attributes of the port | [366-usb.md](../06-operation-commands/366-usb.md) |
| `console` | These commands are used to set or show console attributes | [060-console.md](../06-operation-commands/060-console.md) |
| `serial-console` | These commands are used to set or show the global configuration of all serial console ports in the system | [285-serial-console.md](../06-operation-commands/285-serial-console.md) |
| `resources` | This command is used to show system or card resources | [264-resources.md](../06-operation-commands/264-resources.md) |
| `equipment-policies` | These commands are used to set or show the equipment policies attributes | [093-equipment-policies.md](../06-operation-commands/093-equipment-policies.md) |
| `controller-card` | This command is used to display the configuration of a controller card | [061-controller-card.md](../06-operation-commands/061-controller-card.md) |
| `usb` | This command shows the USB function attributes of the port | [366-usb.md](../06-operation-commands/366-usb.md) |
| `slot` | These commands are used to show the slot equipment holder details | [295-slot.md](../06-operation-commands/295-slot.md) |
| `sub-component` | This command is used to show the sub-component details or card resources | [314-sub-component.md](../06-operation-commands/314-sub-component.md) |

## Certificates and PKI

*Search terms:* `certificate`, `x509`, `csr`, `crl`, `ocsp`, `est`, `ca`, `revocation`, `signing request`, `distribution point`

| Command | What it does | File |
| --- | --- | --- |
| `certificate` | This command is used to delete already imported local/trusted/peer X509v3 certificates and to show... | [045-certificate.md](../06-operation-commands/045-certificate.md) |
| `local-certificate` | These commands are used to set or show the attributes of the X.509v3 end-entity certificate that... | [159-local-certificate.md](../06-operation-commands/159-local-certificate.md) |
| `trusted-certificate` | These commands are used to set or show the X509v3 CA (Root and Intermediate) certificate trusted by... | [359-trusted-certificate.md](../06-operation-commands/359-trusted-certificate.md) |
| `peer-certificate` | These commands are used to set or show the attributes of the X509v3 end-entity certificate that... | [234-peer-certificate.md](../06-operation-commands/234-peer-certificate.md) |
| `import-certificate` | This command allows to import one or more certificates in PEM format into the NE | [131-import-certificate.md](../06-operation-commands/131-import-certificate.md) |
| `display-cert` | This command is used to show the details of a certificate or CSR | [083-display-cert.md](../06-operation-commands/083-display-cert.md) |
| `csr-gen` | This command is used to generate a Certificate Signing Request based on user provided information | [064-csr-gen.md](../06-operation-commands/064-csr-gen.md) |
| `cert-gen` | This command is used to generate a self-signed certificate | [043-cert-gen.md](../06-operation-commands/043-cert-gen.md) |
| `crl` | This command is used to show one or all Certificate Revocation Lists (CRLs) presently on the... | [063-crl.md](../06-operation-commands/063-crl.md) |
| `cdp` | This command is used to manage manually configured CRL Distribution Points (CDPs) | [042-cdp.md](../06-operation-commands/042-cdp.md) |
| `ocsp-server` | These commands are used to add, edit delete or show the attributes of an Online Certificate Status... | [206-ocsp-server.md](../06-operation-commands/206-ocsp-server.md) |
| `est` | The Enrollment over Secure Transport (EST) protocol enables robust and automated certificate... | [095-est.md](../06-operation-commands/095-est.md) |
| `est-ca` | This command is used to represent a Certificate Authority (CA) which is set for Enrollment over... | [096-est-ca.md](../06-operation-commands/096-est-ca.md) |
| `est-server` | This command is used to configure the Enrollment over Secure Transport (EST) server settings | [097-est-server.md](../06-operation-commands/097-est-server.md) |
| `cert-to-name` | This command defines a prioritized set of rules used to map an X.509 client certificate to a... | [044-cert-to-name.md](../06-operation-commands/044-cert-to-name.md) |

## Configuration templates and defaults

*Search terms:* `template`, `default`, `named-value`, `advanced parameter`, `extended config`, `non-standard`

| Command | What it does | File |
| --- | --- | --- |
| `template` | These commands are used to add, set, show and delete the template entry that is defined by an... | [345-template.md](../06-operation-commands/345-template.md) |
| `template-group` | These commands are used to add and show the configuration that defines the data model for system... | [346-template-group.md](../06-operation-commands/346-template-group.md) |
| `templates` | This command is used to show the configuration that defines the data model for system templates | [347-templates.md](../06-operation-commands/347-templates.md) |
| `apply-template` | This command is used to apply templates of multiple types | [023-apply-template.md](../06-operation-commands/023-apply-template.md) |
| `default` | This command can be used to assign default value(s) for the targeted entities | [075-default.md](../06-operation-commands/075-default.md) |
| `named-value-set` | These commands are used to add/set/show and delete the `named-value-set` attributes | [184-named-value-set.md](../06-operation-commands/184-named-value-set.md) |
| `advanced-parameter` | The commands described in this section are used to add, configure, show, or delete advanced parameters | [013-advanced-parameter.md](../06-operation-commands/013-advanced-parameter.md) |
| `current-advanced-parameter` | This command is used to show the current values of the advanced parameters, which are running on... | [065-current-advanced-parameter.md](../06-operation-commands/065-current-advanced-parameter.md) |
| `golden-advanced-parameter` | This command is used to show the `golden-advanced-parameter` attributes | [119-golden-advanced-parameter.md](../06-operation-commands/119-golden-advanced-parameter.md) |
| `gapt` | This command is used to list the golden advanced parameters from the Golden Advanced Parameters... | [116-gapt.md](../06-operation-commands/116-gapt.md) |
| `extended-config` | The commands described in this section are used to add, delete or show the `extended-config` attributes | [103-extended-config.md](../06-operation-commands/103-extended-config.md) |
| `property` | These commands are used to set or show a type specific property, auto instantiated by the system,... | [249-property.md](../06-operation-commands/249-property.md) |
| `equipment-templates` | These commands are used to enable and view the serdes templates setting associated with equipment | [094-equipment-templates.md](../06-operation-commands/094-equipment-templates.md) |

## DNS and DHCP

*Search terms:* `dns`, `dhcp`, `domain name`, `relay`, `name server`

| Command | What it does | File |
| --- | --- | --- |
| `dns` | These commands are used to edit or show the domain name service instance | [084-dns.md](../06-operation-commands/084-dns.md) |
| `dns-server` | These commands are used to add, edit or show a Domain Name Server (DNS) server in the configuration | [085-dns-server.md](../06-operation-commands/085-dns-server.md) |
| `dhcp-relay` | These commands allow to edit or view the dhcp relay mode and server address | [078-dhcp-relay.md](../06-operation-commands/078-dhcp-relay.md) |
| `if-dhcp-relay` | The commands described in this section are used to set or show the `if-dhcp-relay` attributes | [126-if-dhcp-relay.md](../06-operation-commands/126-if-dhcp-relay.md) |

## Data path / Layer 1 encryption

*Search terms:* `encryption`, `secure entity`, `secure application`, `data-path`, `encrypted wavelength`, `digital identity`, `presents`, `mutual`

| Command | What it does | File |
| --- | --- | --- |
| `data-path-encryption` | This command is used to show datapath encryption attributes | [071-data-path-encryption.md](../06-operation-commands/071-data-path-encryption.md) |
| `secure-entity` | These commands are used to add, edit or show a secure entity | [277-secure-entity.md](../06-operation-commands/277-secure-entity.md) |
| `secure-entity-sa-proposal` | The command described in this section is used to show the `secure-entity-sa-proposal` attributes | [278-secure-entity-sa-proposal.md](../06-operation-commands/278-secure-entity-sa-proposal.md) |
| `secure-application` | The commands described in this section are used to edit or show a secure-application or show... | [276-secure-application.md](../06-operation-commands/276-secure-application.md) |
| `peer-certificate` | These commands are used to set or show the attributes of the X509v3 end-entity certificate that... | [234-peer-certificate.md](../06-operation-commands/234-peer-certificate.md) |
| `local-certificate` | These commands are used to set or show the attributes of the X.509v3 end-entity certificate that... | [159-local-certificate.md](../06-operation-commands/159-local-certificate.md) |

## Database backup, snapshot and restore

*Search terms:* `snapshot`, `database`, `recovery`, `migrate`, `replacement controller`, `chassis storage`, `restore`, `backup`

| Command | What it does | File |
| --- | --- | --- |
| `database` | The `show database` command is used to show the list of the databases in the system | [072-database.md](../06-operation-commands/072-database.md) |
| `take-snapshot` | This command is used to create a local database snapshot | [342-take-snapshot.md](../06-operation-commands/342-take-snapshot.md) |
| `activate-snapshot` | This command is used to activate an available database snapshot | [009-activate-snapshot.md](../06-operation-commands/009-activate-snapshot.md) |
| `db-migrate` | The command described in this section is used to show the `db-migrate` attributes | [073-db-migrate.md](../06-operation-commands/073-db-migrate.md) |
| `db-protection-scheme` | The command described in this section is used to show the `db-protection-scheme` attributes | [074-db-protection-scheme.md](../06-operation-commands/074-db-protection-scheme.md) |
| `recovery` | These commands are used configure and display the status of system recovery from chassis storage | [261-recovery.md](../06-operation-commands/261-recovery.md) |
| `recover-mode` | The `clear recover-mode` command, clears recover-mode flag | [260-recover-mode.md](../06-operation-commands/260-recover-mode.md) |

## Environmental: fans, temperature, voltage

*Search terms:* `fan`, `temperature`, `voltage`, `environment`, `cooling`, `locator`, `led`, `lamp test`

| Command | What it does | File |
| --- | --- | --- |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `card` | These commands are used to add, edit, show or delete a card-base object | [040-card.md](../06-operation-commands/040-card.md) |
| `pm` | The `clear pm` command, removes or resets PM data | [236-pm.md](../06-operation-commands/236-pm.md) |
| `status` | This command is used to display multiple dashboard-type outputs | [312-status.md](../06-operation-commands/312-status.md) |
| `trib-ptp` | These commands are used to set or show configuration of the tributary client physical termination... | [358-trib-ptp.md](../06-operation-commands/358-trib-ptp.md) |
| `led` | These commands are used to show the representation of a LED in a FRU. Object exists even if FRU is... | [152-led.md](../06-operation-commands/152-led.md) |
| `activate` | The `activate` command is used to: activate a software image, activate a database, activate... | [008-activate.md](../06-operation-commands/008-activate.md) |
| `terminate` | This command is used to terminate a running operation. **Location led test Termination** By... | [348-terminate.md](../06-operation-commands/348-terminate.md) |

## Equipment protection and switchover

*Search terms:* `protect`, `switchover`, `switch`, `redundan`, `working`, `standby`, `y-cable`, `sncp`

| Command | What it does | File |
| --- | --- | --- |
| `protection` | This command is used to show protection | [250-protection.md](../06-operation-commands/250-protection.md) |
| `protection-group` | These commands are used to add, set and show a protection group | [251-protection-group.md](../06-operation-commands/251-protection-group.md) |
| `protection-switch` | This is the operating command for protection group switching | [252-protection-switch.md](../06-operation-commands/252-protection-switch.md) |
| `protection-unit` | These commands are used to set or show a protection unit | [253-protection-unit.md](../06-operation-commands/253-protection-unit.md) |
| `manual-switchover` | This command is used to perform a manual switchover | [177-manual-switchover.md](../06-operation-commands/177-manual-switchover.md) |

## Ethernet and client facilities

*Search terms:* `ethernet`, `client`, `fibre channel`, `interlaken`, `stm`, `zr`, `sdh`, `sonet`, `termination point`, `auto in service`, `tributary`, `bridge`, `l2-bridge`

| Command | What it does | File |
| --- | --- | --- |
| `ethernet` | These commands are used to set/show ethernet facility attributes | [099-ethernet.md](../06-operation-commands/099-ethernet.md) |
| `eth-zr` | These commands are used to add/edit/show/delete an Ethernet ZR facility | [098-eth-zr.md](../06-operation-commands/098-eth-zr.md) |
| `fc` | The commands described in this section are used to set or show the `fc` attributes | [106-fc.md](../06-operation-commands/106-fc.md) |
| `interlaken` | The commands described in this section are used to set or show the SPN2 `interlaken` attributes | [136-interlaken.md](../06-operation-commands/136-interlaken.md) |
| `stm` | This command is used to set or show STM attributes | [313-stm.md](../06-operation-commands/313-stm.md) |
| `trib-ptp` | These commands are used to set or show configuration of the tributary client physical termination... | [358-trib-ptp.md](../06-operation-commands/358-trib-ptp.md) |
| `line-ptp` | These commands are used to add/set/show/delete a line ptp | [153-line-ptp.md](../06-operation-commands/153-line-ptp.md) |
| `facilities` | This command is used to show system facilities | [105-facilities.md](../06-operation-commands/105-facilities.md) |

## Factory reset, wipe and decommissioning

*Search terms:* `factory reset`, `factory-reset`, `wipe`, `full-wipe`, `decommission`, `erase`, `secure wipe`

| Command | What it does | File |
| --- | --- | --- |
| `system` | The `set system` command is used to set system attributes including the following: | [340-system.md](../06-operation-commands/340-system.md) |
| `database` | The `show database` command is used to show the list of the databases in the system | [072-database.md](../06-operation-commands/072-database.md) |
| `clear` | The clear command clears entries for the specified entity | [049-clear.md](../06-operation-commands/049-clear.md) |
| `recovery` | These commands are used configure and display the status of system recovery from chassis storage | [261-recovery.md](../06-operation-commands/261-recovery.md) |
| `take-snapshot` | This command is used to create a local database snapshot | [342-take-snapshot.md](../06-operation-commands/342-take-snapshot.md) |

## File transfer and file servers

*Search terms:* `download`, `upload`, `file server`, `transfer`, `sftp`, `directory`, `proxy`, `export`, `diagnostic log`, `debug log`, `pull onto`, `get off`

| Command | What it does | File |
| --- | --- | --- |
| `download` | This command is used to download a file from an external location (a file server) to the NE. The... | [086-download.md](../06-operation-commands/086-download.md) |
| `upload` | This command is used to upload files to a remote server | [364-upload.md](../06-operation-commands/364-upload.md) |
| `transfer` | These commands are used to display information about file transfers | [356-transfer.md](../06-operation-commands/356-transfer.md) |
| `transfer-status` | The `show transfer-status` displays information associated with file transfer | [357-transfer-status.md](../06-operation-commands/357-transfer-status.md) |
| `file` | This command is used to perform basic file and directory operations | [108-file.md](../06-operation-commands/108-file.md) |
| `file-operation` | This command is used to perform basic file and directory operations | [109-file-operation.md](../06-operation-commands/109-file-operation.md) |
| `file-server` | These commands are used to add, edit or show user-configurable file servers (e.g | [110-file-server.md](../06-operation-commands/110-file-server.md) |
| `http-file-server` | These commands are used to set/show HTTP file server attributes | [124-http-file-server.md](../06-operation-commands/124-http-file-server.md) |
| `file-type` | This command is used to display file-type transfer information | [111-file-type.md](../06-operation-commands/111-file-type.md) |
| `downloads` | This command is used to show a list of downloads | [088-downloads.md](../06-operation-commands/088-downloads.md) |

## Firmware (FW) management

*Search terms:* `firmware`, `fw`

| Command | What it does | File |
| --- | --- | --- |
| `current-fw` | These commands are used to show the list of current firmware available in the cards | [067-current-fw.md](../06-operation-commands/067-current-fw.md) |
| `packaged-fw` | These commands are used to show the Firmware version included in this software-load | [232-packaged-fw.md](../06-operation-commands/232-packaged-fw.md) |
| `third-party-fw` | This command is used to show third-party firmware information | [350-third-party-fw.md](../06-operation-commands/350-third-party-fw.md) |
| `activate` | The `activate` command is used to: activate a software image, activate a database, activate... | [008-activate.md](../06-operation-commands/008-activate.md) |

## IP addressing and interfaces

*Search terms:* `ip address`, `ipv4`, `ipv6`, `interface`, `subnet`, `mtu`, `management ethernet`, `network service`, `source address`, `negotiat`, `bridge`, `use-as-source`, `uses as its source`, `duplex`

| Command | What it does | File |
| --- | --- | --- |
| `interface` | These commands are used to add/set/show/delete an interface and related attributes | [134-interface.md](../06-operation-commands/134-interface.md) |
| `ipv4-address` | These commands are used to add/show/delete an IPv4 address on the interface | [143-ipv4-address.md](../06-operation-commands/143-ipv4-address.md) |
| `ipv6-address` | These commands are used to add/show/delete an IPv6 address to the interface | [145-ipv6-address.md](../06-operation-commands/145-ipv6-address.md) |
| `supporting-interface` | This command is used to show supporting interface information | [331-supporting-interface.md](../06-operation-commands/331-supporting-interface.md) |
| `networking` | These commands are used to show/set networking information | [190-networking.md](../06-operation-commands/190-networking.md) |
| `networking-services` | This command is used to show the list of network services | [191-networking-services.md](../06-operation-commands/191-networking-services.md) |
| `comm-eth` | These commands are used to set or show the communication Ethernet port attributes | [054-comm-eth.md](../06-operation-commands/054-comm-eth.md) |
| `comm-channel` | These commands are used to add, set or show communications channel attributes | [053-comm-channel.md](../06-operation-commands/053-comm-channel.md) |
| `L2-bridge` | The commands described in this section are used to set or show the `L2-bridge` attributes | [170-l2-bridge.md](../06-operation-commands/170-l2-bridge.md) |
| `ip-monitoring` | These commands are used to add, edit or show Monitoring instance configuration and state | [138-ip-monitoring.md](../06-operation-commands/138-ip-monitoring.md) |

## IPsec and IKEv2

*Search terms:* `ipsec`, `ikev2`, `ike`, `security association`, `traffic selector`, `re-key`, `security policy`, `spd`, `matching traffic`, `tunnel`

| Command | What it does | File |
| --- | --- | --- |
| `ike-sa-proposal` | This command is used to add, edit or show a common set of attributes for IKEv2 used across... | [127-ike-sa-proposal.md](../06-operation-commands/127-ike-sa-proposal.md) |
| `ikev2` | This command is used to set ikev2 | [128-ikev2.md](../06-operation-commands/128-ikev2.md) |
| `ikev2-local-instance` | These commands are used to set and show an ikev2 local instance | [129-ikev2-local-instance.md](../06-operation-commands/129-ikev2-local-instance.md) |
| `ikev2-peer` | These commands are used to add, edit or show an ikev2 peers associated with this local IKE instance | [130-ikev2-peer.md](../06-operation-commands/130-ikev2-peer.md) |
| `ipsec-sa-proposal` | This command is used to add, edit or show an ipsec sa proposal | [139-ipsec-sa-proposal.md](../06-operation-commands/139-ipsec-sa-proposal.md) |
| `ipsec-sa-re-key` | This command is used to add, edit or show ipsec sa re key | [140-ipsec-sa-re-key.md](../06-operation-commands/140-ipsec-sa-re-key.md) |
| `ipsec-spd-entry` | These commands are used to add, edit or show ipsec Security Policy Database entry | [141-ipsec-spd-entry.md](../06-operation-commands/141-ipsec-spd-entry.md) |
| `ipsec-traffic-selector` | This command is used to add, edit or show ipsec traffic selector | [142-ipsec-traffic-selector.md](../06-operation-commands/142-ipsec-traffic-selector.md) |
| `security-policy-database` | These commands are used to add, edit or show the security database | [281-security-policy-database.md](../06-operation-commands/281-security-policy-database.md) |
| `local-subnet` | This command is used to add or show a local subnet | [161-local-subnet.md](../06-operation-commands/161-local-subnet.md) |
| `remote-subnet` | This command is used to add or show a remote subnet | [263-remote-subnet.md](../06-operation-commands/263-remote-subnet.md) |
| `local-ports` | This command is used to add or show local ports | [160-local-ports.md](../06-operation-commands/160-local-ports.md) |
| `remote-ports` | This command is used to add or show a remote port | [262-remote-ports.md](../06-operation-commands/262-remote-ports.md) |
| `re-key` | This command is used to perform a re-key operation including on-demand re-keying of a data path... | [259-re-key.md](../06-operation-commands/259-re-key.md) |
| `re-auth` | This command is used to perform a re-authentication operation of IKEv2 security associations | [258-re-auth.md](../06-operation-commands/258-re-auth.md) |
| `additional-key-exchange` | Users can configure additional key exchange algorithms (for example, classic, PQC, or hybrid with... | [011-additional-key-exchange.md](../06-operation-commands/011-additional-key-exchange.md) |
| `encryption-algorithm` | This command is used to add or show encryption-algorithm attributes | [091-encryption-algorithm.md](../06-operation-commands/091-encryption-algorithm.md) |
| `ospfv3-ipsec-security-association` | This command is used to add/set/show an OSPF version 3 security association | [222-ospfv3-ipsec-security-association.md](../06-operation-commands/222-ospfv3-ipsec-security-association.md) |

## Image signing and root keys

*Search terms:* `image signing`, `root key`, `isk`, `krk`, `key replacement`

| Command | What it does | File |
| --- | --- | --- |
| `ISK` | The show command is used to view the Image Signing Key (ISK) resources from the system | [147-isk.md](../06-operation-commands/147-isk.md) |
| `KRK` | These commands are used to show the list of Image Root Keys (KRKs) list and KRK information | [150-krk.md](../06-operation-commands/150-krk.md) |
| `key-replacement-package` | This command is used to show key replacement package (KRP) attributes | [148-key-replacement-package.md](../06-operation-commands/148-key-replacement-package.md) |

## Inventory and capabilities

*Search terms:* `inventory`, `capabilit`, `supported`, `supports`, `part number`, `card type`, `chassis type`, `form factor`, `breakout`, `phy mode`, `blank`, `compatib`, `redundancy supported`, `capable of`

| Command | What it does | File |
| --- | --- | --- |
| `inventory` | These commands are used to show the inventory data for a present FRU | [137-inventory.md](../06-operation-commands/137-inventory.md) |
| `unprovisioned-inventory` | This command is used to show a .ist of detected inventory but not yet accepted by the Node... | [361-unprovisioned-inventory.md](../06-operation-commands/361-unprovisioned-inventory.md) |
| `capabilities` | This command is used to retrieve information about a cards capabilities | [039-capabilities.md](../06-operation-commands/039-capabilities.md) |
| `supported-card` | This command is used to show the capability information for supported card | [321-supported-card.md](../06-operation-commands/321-supported-card.md) |
| `supported-chassis` | This command is used to show the capability information for supported chassis | [323-supported-chassis.md](../06-operation-commands/323-supported-chassis.md) |
| `supported-slot` | This command is used to show the capability for each slot within each supported chassis | [327-supported-slot.md](../06-operation-commands/327-supported-slot.md) |
| `supported-port` | This command is used to display the capabilities for each port in each supported card | [325-supported-port.md](../06-operation-commands/325-supported-port.md) |
| `supported-tom` | This command is used to display the capability information for supported TOM (Tunable/non-tunable... | [328-supported-tom.md](../06-operation-commands/328-supported-tom.md) |
| `supported-tom-power` | The command described in this section is used to show `supported-tom-power` attributes | [329-supported-tom-power.md](../06-operation-commands/329-supported-tom-power.md) |
| `tom-type` | This command is used to show the capabilities of the supported TOM (Tunable/non-tunable Optical... | [353-tom-type.md](../06-operation-commands/353-tom-type.md) |
| `fru-info` | This command is used to display the packaged FRU information associated to a particular equipment-type | [114-fru-info.md](../06-operation-commands/114-fru-info.md) |
| `l0-capabilities` | The command described in this section is used to show the capabilities details related with node's... | [151-l0-capabilities.md](../06-operation-commands/151-l0-capabilities.md) |
| `oadm-capabilities` | This command is used to show OADM capabilities | [200-oadm-capabilities.md](../06-operation-commands/200-oadm-capabilities.md) |
| `subtype-constraint` | This command is used to show software subtype-constraint information | [318-subtype-constraint.md](../06-operation-commands/318-subtype-constraint.md) |

## LLDP and neighbor discovery

*Search terms:* `lldp`, `neighbor`, `neighbour`, `discovery`, `tlv`, `icdp`, `sndp`, `management address`, `advertising`, `inci`, `discover`, `far end`, `stale`

| Command | What it does | File |
| --- | --- | --- |
| `lldp` | These commands are used to set or show the LLDP hold on timer | [155-lldp.md](../06-operation-commands/155-lldp.md) |
| `lldp-local-info` | This command is used to show the LLDP local system information sent on lldp-port | [156-lldp-local-info.md](../06-operation-commands/156-lldp-local-info.md) |
| `lldp-neighbor` | This command is used to show the LLDP remote system discovered by lldp-port | [157-lldp-neighbor.md](../06-operation-commands/157-lldp-neighbor.md) |
| `lldp-port-statistics` | This command is used to show LLDP frame reception statistics for a particular port and direction | [158-lldp-port-statistics.md](../06-operation-commands/158-lldp-port-statistics.md) |
| `custom-tlv` | This command is used to show a list of Organizational Specific TLVs (Type-Lengh-Value) parameters... | [069-custom-tlv.md](../06-operation-commands/069-custom-tlv.md) |
| `icdp` | These commands are used to set or show Nokia Carrier Discovery Protocol | [125-icdp.md](../06-operation-commands/125-icdp.md) |
| `sndp` | The commands described in this section are used to set or show the `sndp` attributes | [296-sndp.md](../06-operation-commands/296-sndp.md) |
| `carrier-neighbor` | This command is used to show a Local carrier instance that has discovered this neighbor node | [041-carrier-neighbor.md](../06-operation-commands/041-carrier-neighbor.md) |
| `interface-neighbor` | The commands described in this section are used to set or show the `interface-neighbor` attributes | [135-interface-neighbor.md](../06-operation-commands/135-interface-neighbor.md) |
| `inci` | These commands are used to edit or show INCI which is Inter-NE Communication Interface information... | [132-inci.md](../06-operation-commands/132-inci.md) |
| `inci-neighbor` | These commands are used to add, edit or show an INCI which is Inter-NE Communication Interface neighbor | [133-inci-neighbor.md](../06-operation-commands/133-inci-neighbor.md) |
| `management-address` | This command is used to retrieve management address information about a particular chassis component | [174-management-address.md](../06-operation-commands/174-management-address.md) |
| `management-address-local` | This command is used to retrieve management address information about a particular chassis component | [175-management-address-local.md](../06-operation-commands/175-management-address-local.md) |
| `topology` | The `clear topology` command, manually removes existing topology neighbor information | [354-topology.md](../06-operation-commands/354-topology.md) |
| `carrier-neighbor` | This command is used to show a Local carrier instance that has discovered this neighbor node | [041-carrier-neighbor.md](../06-operation-commands/041-carrier-neighbor.md) |

## Laser shutdown and transmit control

*Search terms:* `laser`, `shutdown`, `propagate-shutdown`, `als`, `squelch`

| Command | What it does | File |
| --- | --- | --- |
| `optical-carrier` | These commands are used to add, edit and show the attributes of an optical carrier | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `optical-ptp` | This command is used to edit, or show an optical ptp attributes | [213-optical-ptp.md](../06-operation-commands/213-optical-ptp.md) |
| `fc` | The commands described in this section are used to set or show the `fc` attributes | [106-fc.md](../06-operation-commands/106-fc.md) |
| `trib-ptp` | These commands are used to set or show configuration of the tributary client physical termination... | [358-trib-ptp.md](../06-operation-commands/358-trib-ptp.md) |
| `ots` | These commands are used to set or show the Optical Transport Section (OTS) facility attributes | [225-ots.md](../06-operation-commands/225-ots.md) |
| `otdr` | The commands described in this section are used to add, delete, set or show the OTDR function | [223-otdr.md](../06-operation-commands/223-otdr.md) |

## Loopbacks, test signals and BERT

*Search terms:* `loopback`, `test signal`, `bert`, `prbs`, `diagnostic`

| Command | What it does | File |
| --- | --- | --- |
| `bert` | The commands described in this section are used to start/ stop/get/delete the attributes associated... | [028-bert.md](../06-operation-commands/028-bert.md) |
| `odu-diagnostics` | These commands are used to add, set, show or delete a set of attributes associated with ODU diagnostics | [208-odu-diagnostics.md](../06-operation-commands/208-odu-diagnostics.md) |
| `otu-diagnostics` | These commands are used to set or show the attributes associated with OTU diagnostics | [230-otu-diagnostics.md](../06-operation-commands/230-otu-diagnostics.md) |
| `ots-diagnostics` | This command is used to set or show the attributes associated with OTS diagnostics | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |
| `stm` | This command is used to set or show STM attributes | [313-stm.md](../06-operation-commands/313-stm.md) |
| `ethernet` | These commands are used to set/show ethernet facility attributes | [099-ethernet.md](../06-operation-commands/099-ethernet.md) |
| `odu` | These commands are used to add, set, show an ODU facility | [207-odu.md](../06-operation-commands/207-odu.md) |

## MACsec

*Search terms:* `macsec`, `mka`, `secure channel`, `sak`

| Command | What it does | File |
| --- | --- | --- |
| `macsec-entity` | The commands described in this section are used add, set, show and delete a macsec-entity | [171-macsec-entity.md](../06-operation-commands/171-macsec-entity.md) |
| `macsec-mka` | The commands described in this section are used to add, set, and show and delete a macsec-mka attributes | [172-macsec-mka.md](../06-operation-commands/172-macsec-mka.md) |
| `mka-policy` | The commands described in this section are used add, set, show and delete a mka-policy MACsec Key... | [173-mka-policy.md](../06-operation-commands/173-mka-policy.md) |
| `sc-rx` | The commands described in this section are used to show the Receiving Secure Channel (`sc-rx`) attributes | [273-sc-rx.md](../06-operation-commands/273-sc-rx.md) |
| `sc-tx` | The commands described in this section are used to show the Transmitting Secure Channel (`sc-tx`)... | [274-sc-tx.md](../06-operation-commands/274-sc-tx.md) |

## Multi-chassis and node controller

*Search terms:* `multi-chassis`, `node controller`, `nct`, `chassis`

| Command | What it does | File |
| --- | --- | --- |
| `nct-connection` | This command is used to show NCT connectivity information, providing existing links between NCT... | [185-nct-connection.md](../06-operation-commands/185-nct-connection.md) |
| `unprovisioned-inventory` | This command is used to show a .ist of detected inventory but not yet accepted by the Node... | [361-unprovisioned-inventory.md](../06-operation-commands/361-unprovisioned-inventory.md) |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `management-address` | This command is used to retrieve management address information about a particular chassis component | [174-management-address.md](../06-operation-commands/174-management-address.md) |
| `management-address-local` | This command is used to retrieve management address information about a particular chassis component | [175-management-address-local.md](../06-operation-commands/175-management-address-local.md) |

## NETCONF, RESTCONF, gNMI and YANG

*Search terms:* `netconf`, `restconf`, `grpc`, `gnmi`, `yang`, `data model`

| Command | What it does | File |
| --- | --- | --- |
| `netconf` | These commands are used to set or show NETCONF management protocol attributes | [188-netconf.md](../06-operation-commands/188-netconf.md) |
| `restconf` | These commands are used to set or show configuration of the RESTCONF management protocol | [266-restconf.md](../06-operation-commands/266-restconf.md) |
| `grpc` | These commands are used to enable or show gNMI/gRPC management protocol | [121-grpc.md](../06-operation-commands/121-grpc.md) |
| `data-model` | These commands are used to enable or show the attributes of the available YANG Data models for... | [070-data-model.md](../06-operation-commands/070-data-model.md) |
| `cli` | These commands are used to set or show the configuration of the Command Line Interface (CLI)... | [050-cli.md](../06-operation-commands/050-cli.md) |
| `convert` | This command is used to convert a CLI command into a request for another northbound protocol | [062-convert.md](../06-operation-commands/062-convert.md) |

## NTP and system time

*Search terms:* `ntp`, `time`, `clock`, `timezone`

| Command | What it does | File |
| --- | --- | --- |
| `ntp` | These commands are used to configure and show the Network Time Protocol | [195-ntp.md](../06-operation-commands/195-ntp.md) |
| `ntp-key` | These commands are used to add, configure, show and delete NTP keys to be used for NTP authentication | [196-ntp-key.md](../06-operation-commands/196-ntp-key.md) |
| `ntp-server` | These commands are used to add, set or show the NTP server attributes | [197-ntp-server.md](../06-operation-commands/197-ntp-server.md) |
| `ntp-server-status` | These commands are used to configure and show the NTP server status | [198-ntp-server-status.md](../06-operation-commands/198-ntp-server-status.md) |
| `clock` | These commands are used to set or show the system clock | [052-clock.md](../06-operation-commands/052-clock.md) |
| `set-time` | The `set-time` command changes the system time | [289-set-time.md](../06-operation-commands/289-set-time.md) |
| `time` | This command is used to display the system's time | [351-time.md](../06-operation-commands/351-time.md) |

## Node, NE-level settings and status

*Search terms:* `network element`, `system`, `status`, `uptime`, `restart`, `node type`, `node-type`, `ila`, `oadm`, `xpdr`, `in-line amplifier`, `load average`, `ne-name`, `site`, `location`

| Command | What it does | File |
| --- | --- | --- |
| `ne` | These commands are used to set/show network element attributes | [186-ne.md](../06-operation-commands/186-ne.md) |
| `ne-function` | This command is used to show the Network Element (NE) function | [187-ne-function.md](../06-operation-commands/187-ne-function.md) |
| `system` | The `set system` command is used to set system attributes including the following: | [340-system.md](../06-operation-commands/340-system.md) |
| `status` | This command is used to display multiple dashboard-type outputs | [312-status.md](../06-operation-commands/312-status.md) |
| `uptime` | This command displays the system uptime and load average | [365-uptime.md](../06-operation-commands/365-uptime.md) |
| `restart` | Restarts a specific resource of the system | [265-restart.md](../06-operation-commands/265-restart.md) |
| `resources` | This command is used to show system or card resources | [264-resources.md](../06-operation-commands/264-resources.md) |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | [047-chassis.md](../06-operation-commands/047-chassis.md) |

## OSPF routing

*Search terms:* `ospf`, `area`, `lsa`, `adjacency`

| Command | What it does | File |
| --- | --- | --- |
| `ospf` | The `clear ospf` command is used to remove and restart an ospf-instance | [216-ospf.md](../06-operation-commands/216-ospf.md) |
| `ospf-instance` | These commands are used to add, set, show and delete an OSPF protocol instance | [219-ospf-instance.md](../06-operation-commands/219-ospf-instance.md) |
| `ospf-area` | These commands are used to add, set, show or delete an OSPF protocol area | [217-ospf-area.md](../06-operation-commands/217-ospf-area.md) |
| `ospf-area-range` | These commands are used to add, set, show or delete an OSPF area range instance | [218-ospf-area-range.md](../06-operation-commands/218-ospf-area-range.md) |
| `ospf-interface` | These commands are used to add, set, show or delete an OSPF interface | [220-ospf-interface.md](../06-operation-commands/220-ospf-interface.md) |
| `ospf-neighbor` | The command described in this section is used to show the `ospf-neighbor` attributes | [221-ospf-neighbor.md](../06-operation-commands/221-ospf-neighbor.md) |
| `ospfv3-ipsec-security-association` | This command is used to add/set/show an OSPF version 3 security association | [222-ospfv3-ipsec-security-association.md](../06-operation-commands/222-ospfv3-ipsec-security-association.md) |

## OTDR and fiber diagnostics

*Search terms:* `otdr`, `reflect`, `fiber`, `fibre`, `trace`, `scan`

| Command | What it does | File |
| --- | --- | --- |
| `otdr` | The commands described in this section are used to add, delete, set or show the OTDR function | [223-otdr.md](../06-operation-commands/223-otdr.md) |
| `otdr-ptp` | These commands are used to add, delete set or show the OTDR ptp | [224-otdr-ptp.md](../06-operation-commands/224-otdr-ptp.md) |
| `ots-r-auto-otdr` | The commands described in this section are used to add or delete automatic OTDR `ots-r-auto-otdr`... | [228-ots-r-auto-otdr.md](../06-operation-commands/228-ots-r-auto-otdr.md) |
| `ots-diagnostics` | This command is used to set or show the attributes associated with OTS diagnostics | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |

## OTN: ODU, OTU and cross-connects

*Search terms:* `odu`, `otu`, `otn`, `cross connect`, `xcon`, `tti`, `trace`, `sapi`, `dapi`, `flexo`, `gcc`, `mismatched`

| Command | What it does | File |
| --- | --- | --- |
| `odu` | These commands are used to add, set, show an ODU facility | [207-odu.md](../06-operation-commands/207-odu.md) |
| `otu` | These commands are used to add, edit or show an OTU. The delete command is used to remove an OTU... | [229-otu.md](../06-operation-commands/229-otu.md) |
| `odu-diagnostics` | These commands are used to add, set, show or delete a set of attributes associated with ODU diagnostics | [208-odu-diagnostics.md](../06-operation-commands/208-odu-diagnostics.md) |
| `otu-diagnostics` | These commands are used to set or show the attributes associated with OTU diagnostics | [230-otu-diagnostics.md](../06-operation-commands/230-otu-diagnostics.md) |
| `xcon` | These commands are used to add, edit or show Layer 1 digital services that are currently... | [373-xcon.md](../06-operation-commands/373-xcon.md) |
| `nw-xconnect` | The commands described in this section are used to add, set or show the `nw-xconnect` attributes | [199-nw-xconnect.md](../06-operation-commands/199-nw-xconnect.md) |
| `network-xconnect` | This command is used to show the list of services of multiple user cross connections commissioned... | [189-network-xconnect.md](../06-operation-commands/189-network-xconnect.md) |
| `flexo` | The commands described in this section are used to set or show the `flexo` attributes | [112-flexo.md](../06-operation-commands/112-flexo.md) |
| `flexo-group` | These commands are used to add/set/show/delete a flexo-group | [113-flexo-group.md](../06-operation-commands/113-flexo-group.md) |

## Optical power control and profiles

*Search terms:* `power`, `attenuation`, `profile`, `target`

| Command | What it does | File |
| --- | --- | --- |
| `profile-control` | The `profile-control` command allows the user to read or write per-slice power or attenuation... | [248-profile-control.md](../06-operation-commands/248-profile-control.md) |
| `supported-power-profile` | This command is used to show the supported power-profile attributes for the specified card-type | [326-supported-power-profile.md](../06-operation-commands/326-supported-power-profile.md) |
| `supported-tom-power` | The command described in this section is used to show `supported-tom-power` attributes | [329-supported-tom-power.md](../06-operation-commands/329-supported-tom-power.md) |
| `spectrum-control` | The commands described in this section are used to set or show the `spectrum-control` object attributes | [304-spectrum-control.md](../06-operation-commands/304-spectrum-control.md) |
| `amplifier` | These commands are used to set or show the amplifier object attributes | [019-amplifier.md](../06-operation-commands/019-amplifier.md) |

## Optical sections OTS / OMS / OPS / OSC

*Search terms:* `ots`, `oms`, `ops`, `osc`, `supervisory`, `section`

| Command | What it does | File |
| --- | --- | --- |
| `ots` | These commands are used to set or show the Optical Transport Section (OTS) facility attributes | [225-ots.md](../06-operation-commands/225-ots.md) |
| `ots-r` | These commands are used to enable, add, set or show the attributes associated with Optical... | [227-ots-r.md](../06-operation-commands/227-ots-r.md) |
| `oms` | These commands are used to set or show the Optical Multiplex Section (OMS) facility attributes | [209-oms.md](../06-operation-commands/209-oms.md) |
| `ops` | These commands are used to set or show the Optical Physical Section (OPS) facility attributes | [210-ops.md](../06-operation-commands/210-ops.md) |
| `osc` | These commands are used to set or show the Optical Supervisory Channel (OSC) facility attributes | [215-osc.md](../06-operation-commands/215-osc.md) |
| `ots-diagnostics` | This command is used to set or show the attributes associated with OTS diagnostics | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |

## Performance monitoring, PM bins and thresholds

*Search terms:* `pm`, `performance`, `threshold`, `tca`, `bin`, `counter`, `limit`, `errored second`, `supervision`, `gauge`

| Command | What it does | File |
| --- | --- | --- |
| `pm` | The `clear pm` command, removes or resets PM data | [236-pm.md](../06-operation-commands/236-pm.md) |
| `pm-catalog` | This command is used to show the contents of PM catalog | [237-pm-catalog.md](../06-operation-commands/237-pm-catalog.md) |
| `pm-control` | These commands are used to set or show configuration for currently existing resources in the system... | [238-pm-control.md](../06-operation-commands/238-pm-control.md) |
| `pm-control-entry` | These commands are used to set or show the PM configuration for one particular resource, for one... | [239-pm-control-entry.md](../06-operation-commands/239-pm-control-entry.md) |
| `pm-parameter` | This command is used to show pm parameter information | [240-pm-parameter.md](../06-operation-commands/240-pm-parameter.md) |
| `pm-profile` | These commands are used to set or show a PM profile which contains information on all resources... | [241-pm-profile.md](../06-operation-commands/241-pm-profile.md) |
| `pm-profile-entry` | These commands are used to set or show the PM configuration per resource type | [242-pm-profile-entry.md](../06-operation-commands/242-pm-profile-entry.md) |
| `pm-resource` | These commands are used to set or show the PM configuration per resource instance | [243-pm-resource.md](../06-operation-commands/243-pm-resource.md) |
| `pm-threshold` | These commands are used to add, set, show or delete a PM threshold | [244-pm-threshold.md](../06-operation-commands/244-pm-threshold.md) |
| `pm-threshold-profile` | These commands are used to set or show PM configuration per parameter, for this resource type | [245-pm-threshold-profile.md](../06-operation-commands/245-pm-threshold-profile.md) |
| `statistics` | The command described in this section is used to clear the event counters (statistics) for the... | [311-statistics.md](../06-operation-commands/311-statistics.md) |
| `high-speed-monitoring` | The commands described in this section are used to set or show the `high-speed-monitoring` attributes | [123-high-speed-monitoring.md](../06-operation-commands/123-high-speed-monitoring.md) |

## ROADM degrees, add/drop groups and switching

*Search terms:* `degree`, `adg`, `add/drop`, `roadm`, `oadm`, `switch`, `cross connection`, `ase`, `idler`, `photonic`, `layer 0`, `l0`

| Command | What it does | File |
| --- | --- | --- |
| `degree` | These commands are used to add, delete a degree and to set or show the degree attributes | [076-degree.md](../06-operation-commands/076-degree.md) |
| `adg` | These commands are used to add, delete an Add/Drop Group (ADG) and to set or show the ADG attributes | [012-adg.md](../06-operation-commands/012-adg.md) |
| `modules-degree` | These commands are used to add, delete modules to a degree and to set or show the object attributes | [182-modules-degree.md](../06-operation-commands/182-modules-degree.md) |
| `modules-adg` | These commands are used to add, delete modules to an ADG and to set or show the object attributes | [181-modules-adg.md](../06-operation-commands/181-modules-adg.md) |
| `oadm-capabilities` | This command is used to show OADM capabilities | [200-oadm-capabilities.md](../06-operation-commands/200-oadm-capabilities.md) |
| `optical-switch` | The commands described in this section are used to set or show the `optical-switch` attributes | [214-optical-switch.md](../06-operation-commands/214-optical-switch.md) |
| `oxcon` | These commands are used to add, delete the Optical Cross Connection (OXcon), and set or show the... | [231-oxcon.md](../06-operation-commands/231-oxcon.md) |
| `direction` | These commands are used to add/edit or show the directions on a multi-rail ILA node | [081-direction.md](../06-operation-commands/081-direction.md) |
| `l0-capabilities` | The command described in this section is used to show the capabilities details related with node's... | [151-l0-capabilities.md](../06-operation-commands/151-l0-capabilities.md) |
| `ase-idler-service` | The commands described in this section are used to add/delete `ase-idler-service` or set/show the... | [024-ase-idler-service.md](../06-operation-commands/024-ase-idler-service.md) |
| `ase-idler-source` | The commands described in this section are used to set or show the `ase-idler-source` attributes | [025-ase-idler-source.md](../06-operation-commands/025-ase-idler-source.md) |

## Reachability tests

*Search terms:* `ping`, `traceroute`, `echo`, `monitoring`, `reachability`, `trace`, `hops`, `unreachable`, `far end`

| Command | What it does | File |
| --- | --- | --- |
| `ping` | This command sends an echo message to another TCP/IP node to determine if the node is visible on... | [235-ping.md](../06-operation-commands/235-ping.md) |
| `traceroute` | This command is used to track the route packets taken from an IP network on their way to a given host | [355-traceroute.md](../06-operation-commands/355-traceroute.md) |
| `ip-monitoring` | These commands are used to add, edit or show Monitoring instance configuration and state | [138-ip-monitoring.md](../06-operation-commands/138-ip-monitoring.md) |

## Restart, reboot and recovery

*Search terms:* `reboot`, `restart`, `reset`, `reload`, `recover`

| Command | What it does | File |
| --- | --- | --- |
| `restart` | Restarts a specific resource of the system | [265-restart.md](../06-operation-commands/265-restart.md) |
| `recover-mode` | The `clear recover-mode` command, clears recover-mode flag | [260-recover-mode.md](../06-operation-commands/260-recover-mode.md) |
| `recovery` | These commands are used configure and display the status of system recovery from chassis storage | [261-recovery.md](../06-operation-commands/261-recovery.md) |
| `manual-switchover` | This command is used to perform a manual switchover | [177-manual-switchover.md](../06-operation-commands/177-manual-switchover.md) |
| `card` | These commands are used to add, edit, show or delete a card-base object | [040-card.md](../06-operation-commands/040-card.md) |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | [047-chassis.md](../06-operation-commands/047-chassis.md) |

## SNMP

*Search terms:* `snmp`, `community`, `trap`, `v3`

| Command | What it does | File |
| --- | --- | --- |
| `snmp` | These commands are used to set or show the configuration of the SNMP management protocol | [297-snmp.md](../06-operation-commands/297-snmp.md) |
| `snmp-community` | These commands are used to add, set, show or delete an SNMP community | [298-snmp-community.md](../06-operation-commands/298-snmp-community.md) |
| `snmp-target` | These commands are used to add, set, show or delete a list of SNMP targets (trap listeners) | [299-snmp-target.md](../06-operation-commands/299-snmp-target.md) |
| `snmpv3-user` | These commands are used to add, set, show or delete a list of SNMP V3 user | [300-snmpv3-user.md](../06-operation-commands/300-snmpv3-user.md) |

## SSH keys and known hosts

*Search terms:* `ssh`, `key pair`, `known host`, `host key`, `authorized key`, `public key`, `log in with`, `keygen`

| Command | What it does | File |
| --- | --- | --- |
| `ssh` | These commands are used to set or show attributes of secure shell access | [306-ssh.md](../06-operation-commands/306-ssh.md) |
| `ssh-keygen` | This command is used to generate a ssh private/public key pair | [309-ssh-keygen.md](../06-operation-commands/309-ssh-keygen.md) |
| `ssh-host-key` | This command is used to show global (for server and client side SSHv2 based apps) SSHv2 host keys | [308-ssh-host-key.md](../06-operation-commands/308-ssh-host-key.md) |
| `ssh-authorized-key` | These commands are used to add, set, show an ssh authorized key | [307-ssh-authorized-key.md](../06-operation-commands/307-ssh-authorized-key.md) |
| `ssh-known-host` | These commands are used to add, set, show or delete an SSHv2 known hosts entry | [310-ssh-known-host.md](../06-operation-commands/310-ssh-known-host.md) |

## Scripting, tasks and automation

*Search terms:* `script`, `task`, `schedule`, `variable`, `alias`, `expect`, `property`, `card property`, `on demand`, `card-level`

| Command | What it does | File |
| --- | --- | --- |
| `run` | This command is used to execute a previously configured/defined/scheduled task or a script | [272-run.md](../06-operation-commands/272-run.md) |
| `task` | These commands are used to add, set, show or delete a user configurable scheduled task | [343-task.md](../06-operation-commands/343-task.md) |
| `scheduled-task` | These commands are used to add/set or show a set of individual user-configurable scheduled commands | [275-scheduled-task.md](../06-operation-commands/275-scheduled-task.md) |
| `expect` | This command is used to ensure that an attribute matches the expected value | [101-expect.md](../06-operation-commands/101-expect.md) |
| `export` | This command is used to define variables to use in CLI. Variables can be referenced with... | [102-export.md](../06-operation-commands/102-export.md) |
| `sleep` | This command is used to specify a delay for a specified amount of time | [294-sleep.md](../06-operation-commands/294-sleep.md) |
| `message` | This command is used to send a message to other CLI sessions | [180-message.md](../06-operation-commands/180-message.md) |
| `alias` | The `alias` command is used to define a more user-friendly alphanumeric string for one or more... | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `unalias` | The `unalias` command is used to remove an alias previously defined.. When using `unalias` command,... | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `simulate` | This command is used to trigger simulated events in the system (alarms, equipment, etc).... | [293-simulate.md](../06-operation-commands/293-simulate.md) |
| `property` | These commands are used to set or show a type specific property, auto instantiated by the system,... | [249-property.md](../06-operation-commands/249-property.md) |

## SerDes and TOM templates

*Search terms:* `serdes`, `template`, `tom`

| Command | What it does | File |
| --- | --- | --- |
| `serdes` | These commands are used to add, edit or show serdes | [282-serdes.md](../06-operation-commands/282-serdes.md) |
| `serdes-template` | This command is used to auto-configure serdes for 3rd party TOMs. serdes-templates are created by... | [283-serdes-template.md](../06-operation-commands/283-serdes-template.md) |
| `serdes-template-entry` | These commands are used to enter an individual entry to the serdes-template | [284-serdes-template-entry.md](../06-operation-commands/284-serdes-template-entry.md) |
| `equipment-templates` | These commands are used to enable and view the serdes templates setting associated with equipment | [094-equipment-templates.md](../06-operation-commands/094-equipment-templates.md) |
| `supported-tom` | This command is used to display the capability information for supported TOM (Tunable/non-tunable... | [328-supported-tom.md](../06-operation-commands/328-supported-tom.md) |
| `supported-tom-power` | The command described in this section is used to show `supported-tom-power` attributes | [329-supported-tom-power.md](../06-operation-commands/329-supported-tom-power.md) |

## Serial numbers, CLEI and FRU identification

*Search terms:* `serial number`, `serial-number`, `clei`, `part number`, `manufactur`

| Command | What it does | File |
| --- | --- | --- |
| `inventory` | These commands are used to show the inventory data for a present FRU | [137-inventory.md](../06-operation-commands/137-inventory.md) |
| `unprovisioned-inventory` | This command is used to show a .ist of detected inventory but not yet accepted by the Node... | [361-unprovisioned-inventory.md](../06-operation-commands/361-unprovisioned-inventory.md) |
| `fru-info` | This command is used to display the packaged FRU information associated to a particular equipment-type | [114-fru-info.md](../06-operation-commands/114-fru-info.md) |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `card` | These commands are used to add, edit, show or delete a card-base object | [040-card.md](../06-operation-commands/040-card.md) |
| `sub-component` | This command is used to show the sub-component details or card resources | [314-sub-component.md](../06-operation-commands/314-sub-component.md) |

## Signal quality: OSNR, FEC and error counts

*Search terms:* `osnr`, `fec`, `pre-fec`, `ber`, `q-factor`, `error`, `signal quality`

| Command | What it does | File |
| --- | --- | --- |
| `status` | This command is used to display multiple dashboard-type outputs | [312-status.md](../06-operation-commands/312-status.md) |
| `pm` | The `clear pm` command, removes or resets PM data | [236-pm.md](../06-operation-commands/236-pm.md) |
| `pm-parameter` | This command is used to show pm parameter information | [240-pm-parameter.md](../06-operation-commands/240-pm-parameter.md) |
| `bert` | The commands described in this section are used to start/ stop/get/delete the attributes associated... | [028-bert.md](../06-operation-commands/028-bert.md) |
| `otu-diagnostics` | These commands are used to set or show the attributes associated with OTU diagnostics | [230-otu-diagnostics.md](../06-operation-commands/230-otu-diagnostics.md) |
| `odu-diagnostics` | These commands are used to add, set, show or delete a set of attributes associated with ODU diagnostics | [208-odu-diagnostics.md](../06-operation-commands/208-odu-diagnostics.md) |
| `eth-zr` | These commands are used to add/edit/show/delete an Ethernet ZR facility | [098-eth-zr.md](../06-operation-commands/098-eth-zr.md) |
| `optical-carrier` | These commands are used to add, edit and show the attributes of an optical carrier | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `dsc-group` | The commands described in this section are used to add, delete, set or show the `dsc-group` attributes | [090-dsc-group.md](../06-operation-commands/090-dsc-group.md) |

## Software upgrade

*Search terms:* `upgrade`, `software load`, `activate`, `swversion`, `software version`, `software label`, `prepare`, `install`, `running software`, `software on`, `what software`, `versions`

| Command | What it does | File |
| --- | --- | --- |
| `prepare-upgrade` | This command is used to prepare the network element software for upgrade | [247-prepare-upgrade.md](../06-operation-commands/247-prepare-upgrade.md) |
| `activate` | The `activate` command is used to: activate a software image, activate a database, activate... | [008-activate.md](../06-operation-commands/008-activate.md) |
| `cancel-upgrade` | This command is used to cancel any active upgrade in progress | [038-cancel-upgrade.md](../06-operation-commands/038-cancel-upgrade.md) |
| `upgrade-status` | This command displays all the SW versions being installed in the system, and their installation status | [363-upgrade-status.md](../06-operation-commands/363-upgrade-status.md) |
| `swversion` | This command is used to retrieve the active, inactive and/or installable versions of the software... | [338-swversion.md](../06-operation-commands/338-swversion.md) |
| `software-load` | These commands are used to show the information on the Software Load present in the system | [301-software-load.md](../06-operation-commands/301-software-load.md) |
| `software-location` | This command is used to retrieve information about the location of software | [302-software-location.md](../06-operation-commands/302-software-location.md) |
| `sw-management` | This command is used to show information about software locations, activity and downloads | [335-sw-management.md](../06-operation-commands/335-sw-management.md) |
| `downloaded-image` | This command is used to retrieve information about downloaded image files | [087-downloaded-image.md](../06-operation-commands/087-downloaded-image.md) |
| `manifest` | These commands are used to show the downloaded manifest file and it's information | [176-manifest.md](../06-operation-commands/176-manifest.md) |
| `subtype-constraint` | This command is used to show software subtype-constraint information | [318-subtype-constraint.md](../06-operation-commands/318-subtype-constraint.md) |
| `sw-component` | This command is used to show the software load component details | [332-sw-component.md](../06-operation-commands/332-sw-component.md) |
| `sw-subcomponent` | These commands are used to show the software load subcomponent details | [337-sw-subcomponent.md](../06-operation-commands/337-sw-subcomponent.md) |
| `software-location` | This command is used to retrieve information about the location of software | [302-software-location.md](../06-operation-commands/302-software-location.md) |
| `prepare-upgrade` | This command is used to prepare the network element software for upgrade | [247-prepare-upgrade.md](../06-operation-commands/247-prepare-upgrade.md) |
| `swversion` | This command is used to retrieve the active, inactive and/or installable versions of the software... | [338-swversion.md](../06-operation-commands/338-swversion.md) |

## Span loss, dispersion and optical line characterisation

*Search terms:* `span loss`, `span-loss`, `span`, `dispersion`, `cd`, `pmd`, `reach`, `fiber type`, `fibre`, `lossy`

| Command | What it does | File |
| --- | --- | --- |
| `ots` | These commands are used to set or show the Optical Transport Section (OTS) facility attributes | [225-ots.md](../06-operation-commands/225-ots.md) |
| `ots-r` | These commands are used to enable, add, set or show the attributes associated with Optical... | [227-ots-r.md](../06-operation-commands/227-ots-r.md) |
| `amplifier` | These commands are used to set or show the amplifier object attributes | [019-amplifier.md](../06-operation-commands/019-amplifier.md) |
| `otdr` | The commands described in this section are used to add, delete, set or show the OTDR function | [223-otdr.md](../06-operation-commands/223-otdr.md) |
| `otdr-ptp` | These commands are used to add, delete set or show the OTDR ptp | [224-otdr-ptp.md](../06-operation-commands/224-otdr-ptp.md) |
| `ots-r-auto-otdr` | The commands described in this section are used to add or delete automatic OTDR `ots-r-auto-otdr`... | [228-ots-r-auto-otdr.md](../06-operation-commands/228-ots-r-auto-otdr.md) |
| `optical-carrier` | These commands are used to add, edit and show the attributes of an optical carrier | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `raman-calibration` | The commands described in this section are used to add, delete, set or show the `raman-calibration`... | [257-raman-calibration.md](../06-operation-commands/257-raman-calibration.md) |

## Spectrum, wavelength and channels

*Search terms:* `spectrum`, `frequency`, `wavelength`, `channel`, `carrier`, `thz`

| Command | What it does | File |
| --- | --- | --- |
| `spectrum` | The commands described in this section are used to set or show the spectrum facility attributes | [303-spectrum.md](../06-operation-commands/303-spectrum.md) |
| `spectrum-control` | The commands described in this section are used to set or show the `spectrum-control` object attributes | [304-spectrum-control.md](../06-operation-commands/304-spectrum-control.md) |
| `spectrum-monitoring` | The command described in this section are used to show the `spectrum-monitoring` attributes | [305-spectrum-monitoring.md](../06-operation-commands/305-spectrum-monitoring.md) |
| `mc` | These commands are used to add, delete the Media Channel (MC), and set or show the MC facility attributes | [178-mc.md](../06-operation-commands/178-mc.md) |
| `mc-f` | This command is used to show the Media Channel Filler (NMC-F) facility attributes | [179-mc-f.md](../06-operation-commands/179-mc-f.md) |
| `nmc` | These commands are used to add, delete the Network Media Channel (NMC), and set or show the NMC... | [193-nmc.md](../06-operation-commands/193-nmc.md) |
| `nmc-f` | This command is used to show the Network Media Channel Filler (NMC-F) facility attributes | [194-nmc-f.md](../06-operation-commands/194-nmc-f.md) |
| `oc` | This command is used to enable, edit or show the attributes of an optical carrier | [201-oc.md](../06-operation-commands/201-oc.md) |
| `optical-carrier` | These commands are used to add, edit and show the attributes of an optical carrier | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `optical-channel` | These commands are used to edit, and show optical channel attributes | [212-optical-channel.md](../06-operation-commands/212-optical-channel.md) |
| `optical-ptp` | This command is used to edit, or show an optical ptp attributes | [213-optical-ptp.md](../06-operation-commands/213-optical-ptp.md) |
| `super-channel` | This command is used to display Super Channel configuration attributes | [319-super-channel.md](../06-operation-commands/319-super-channel.md) |
| `super-channel-group` | This command is used to add, set or show super-channel-group attributes | [320-super-channel-group.md](../06-operation-commands/320-super-channel-group.md) |
| `monitored-channel` | The command described in this section is used to show the **monitored-channel** attributes | [183-monitored-channel.md](../06-operation-commands/183-monitored-channel.md) |
| `ocm-channel` | The commands described in this section are used to set or show the `ocm-channel` attributes | [203-ocm-channel.md](../06-operation-commands/203-ocm-channel.md) |
| `ocm-mp` | The commands described in this section are used to set or show the `ocm-mp` attributes | [204-ocm-mp.md](../06-operation-commands/204-ocm-mp.md) |
| `ocm-ptp` | The commands described in this section are used to set or show the `ocm-ptp` attributes | [205-ocm-ptp.md](../06-operation-commands/205-ocm-ptp.md) |
| `ochm` | The commands described in this section are used to set or show the `ochm` (optical channel... | [202-ochm.md](../06-operation-commands/202-ochm.md) |
| `supported-carrier-mode` | This command is used to display a list of supported carrier modes | [322-supported-carrier-mode.md](../06-operation-commands/322-supported-carrier-mode.md) |
| `golden-carrier-mode` | This command is used to retrieve configuration information from the system | [120-golden-carrier-mode.md](../06-operation-commands/120-golden-carrier-mode.md) |
| `gadt` | This command is used to retrieve information about golden carrier application information  | [115-gadt.md](../06-operation-commands/115-gadt.md) |
| `gcmt` | This command is used to retrieve information about the golden carrier mode | [117-gcmt.md](../06-operation-commands/117-gcmt.md) |

## Static routes, RIB and VRF

*Search terms:* `static route`, `route`, `rib`, `vrf`, `next hop`, `routing information base`, `routing instance`, `address family`, `forwarding table`

| Command | What it does | File |
| --- | --- | --- |
| `ipv4-static-route` | These commands are used to add/show/delete a list of IPv4 static routes to the interface | [144-ipv4-static-route.md](../06-operation-commands/144-ipv4-static-route.md) |
| `ipv6-static-route` | These commands are used to add/show/delete a list of static routes to the interface | [146-ipv6-static-route.md](../06-operation-commands/146-ipv6-static-route.md) |
| `route` | This command is used to show the list of system routes from various sources, such as dynamic... | [269-route.md](../06-operation-commands/269-route.md) |
| `rib` | This command is used to show RIB entries | [267-rib.md](../06-operation-commands/267-rib.md) |
| `routing` | This command is used to show routing information | [270-routing.md](../06-operation-commands/270-routing.md) |
| `next-hop` | This command is used to show the next hop in a route | [192-next-hop.md](../06-operation-commands/192-next-hop.md) |
| `vrf` | This command shows the Virtual Routing and Forwarding (VRF) instance | [372-vrf.md](../06-operation-commands/372-vrf.md) |

## Syslog and logging

*Search terms:* `log`, `syslog`, `facility`, `severity`, `console`, `informational`, `debug`, `collector`, `forward`, `message`, `rotation`

| Command | What it does | File |
| --- | --- | --- |
| `log` | This command is used to retrieve log files . If no <logname> is provided, the list of available... | [163-log.md](../06-operation-commands/163-log.md) |
| `syslog` | These commands are used to set or show the configuration for logging functionality via syslog | [339-syslog.md](../06-operation-commands/339-syslog.md) |
| `log-file` | These commands are used to add/set/show/delete local syslog files supported to the system | [166-log-file.md](../06-operation-commands/166-log-file.md) |
| `log-file-facility-filter` | These commands are used to add/set/show a selector that filters messages based on their source... | [167-log-file-facility-filter.md](../06-operation-commands/167-log-file-facility-filter.md) |
| `log-server` | This command is used to group or show the configuration parameters for log forwarding | [168-log-server.md](../06-operation-commands/168-log-server.md) |
| `log-server-facility-filter` | These commands allow to filter log messages based on their source facilities and severities | [169-log-server-facility-filter.md](../06-operation-commands/169-log-server-facility-filter.md) |
| `log-console` | These commands are used to set or show the attributes of the console logging supported by the system | [164-log-console.md](../06-operation-commands/164-log-console.md) |
| `log-console-facility-filter` | These commands are used to add, set or show a selector that filters messages based on their source... | [165-log-console-facility-filter.md](../06-operation-commands/165-log-console-facility-filter.md) |

## Telemetry and subscriptions

*Search terms:* `telemetry`, `subscri`, `stream`, `dial-out`, `call home`, `dial out`, `management system`, `retry timer`

| Command | What it does | File |
| --- | --- | --- |
| `telemetry` | This command is used to configure persistent and dynamic telemetry | [344-telemetry.md](../06-operation-commands/344-telemetry.md) |
| `subscriptions` | This command is used to show a list of subscriptions | [317-subscriptions.md](../06-operation-commands/317-subscriptions.md) |
| `current-subscription` | This command is used to show a list representation of telemetry subscriptions that are configured... | [068-current-subscription.md](../06-operation-commands/068-current-subscription.md) |
| `subscription-path` | These commands are used to retrieve information subscription-paths | [316-subscription-path.md](../06-operation-commands/316-subscription-path.md) |
| `dial-out-server` | These commands are used to add/edit or show the dial-out-server | [079-dial-out-server.md](../06-operation-commands/079-dial-out-server.md) |
| `call-home` | This command is used to execute a manual connection trigger to a configured dial-out-server | [037-call-home.md](../06-operation-commands/037-call-home.md) |

## Third-party applications and containers

*Search terms:* `third party`, `third-party`, `app`, `container`, `shell`, `service`, `software service`, `restarting`, `daemon`

| Command | What it does | File |
| --- | --- | --- |
| `app` | The `clear app` command, clears third party apps | [021-app.md](../06-operation-commands/021-app.md) |
| `appctl` | This command is used to control third-party applications | [022-appctl.md](../06-operation-commands/022-appctl.md) |
| `third-party-app` | This command is used to set or show a third party application | [349-third-party-app.md](../06-operation-commands/349-third-party-app.md) |
| `third-party-fw` | This command is used to show third-party firmware information | [350-third-party-fw.md](../06-operation-commands/350-third-party-fw.md) |
| `gshell` | This command is used to launch a Linux bash shell inside a Guest Container from within the CLI. The... | [122-gshell.md](../06-operation-commands/122-gshell.md) |
| `shell` | This command is used to launch a Linux bash shell from within the CLI. The shell will be launched... | [290-shell.md](../06-operation-commands/290-shell.md) |
| `sw-container` | This command is used to show the list of OS-level containers | [333-sw-container.md](../06-operation-commands/333-sw-container.md) |
| `sw-service` | These commands are used to show the software service running in the system | [336-sw-service.md](../06-operation-commands/336-sw-service.md) |
| `sw-control-rule` | These commands are used to add, set or show option service-specific custom rules to override the... | [334-sw-control-rule.md](../06-operation-commands/334-sw-control-rule.md) |

## Topology and fiber connections

*Search terms:* `topology`, `fiber`, `fibre`, `cable`, `link`, `connection`, `connectiv`

| Command | What it does | File |
| --- | --- | --- |
| `topology` | The `clear topology` command, manually removes existing topology neighbor information | [354-topology.md](../06-operation-commands/354-topology.md) |
| `links` | This command is used to show the links container within the topology | [154-links.md](../06-operation-commands/154-links.md) |
| `fiber-connection` | These commands are used to add, set, show or delete a fiber-connection in an OADM/ILA topology | [107-fiber-connection.md](../06-operation-commands/107-fiber-connection.md) |
| `external-fiber-connection` | These commands are used to add, set, show or delete an external fiber connection | [104-external-fiber-connection.md](../06-operation-commands/104-external-fiber-connection.md) |
| `supporting-fiber-connection` | The commands described in this section are used to show the list of fiber connections | [330-supporting-fiber-connection.md](../06-operation-commands/330-supporting-fiber-connection.md) |
| `connection-ports` | This command is used to show connection ports | [059-connection-ports.md](../06-operation-commands/059-connection-ports.md) |
| `submarine-link` | The commands described in this section are used to add or delete `submarine-link` object and set or... | [315-submarine-link.md](../06-operation-commands/315-submarine-link.md) |
| `nct-connection` | This command is used to show NCT connectivity information, providing existing links between NCT... | [185-nct-connection.md](../06-operation-commands/185-nct-connection.md) |

## Users, passwords and user groups

*Search terms:* `user`, `password`, `group`, `privilege`, `login`, `lockout`, `account`, `complexity`, `minimum length`

| Command | What it does | File |
| --- | --- | --- |
| `user` | These commands are used to add, set, show or delete users and attributes | [367-user.md](../06-operation-commands/367-user.md) |
| `user-group` | These commands are used to add, set or show user groups and attributes | [369-user-group.md](../06-operation-commands/369-user-group.md) |
| `user-data` | The commands described in this section are used to show the `user-data` | [368-user-data.md](../06-operation-commands/368-user-data.md) |
| `password` | This command allows a user to change its own password in an interactive way. **Changing own... | [233-password.md](../06-operation-commands/233-password.md) |
| `authorization` | The commands described in this section are used to set or show the `authorization` attributes | [027-authorization.md](../06-operation-commands/027-authorization.md) |
| `session` | This command is used to show the list of currently established management layer sessions | [286-session.md](../06-operation-commands/286-session.md) |
| `kill-session` | This command is used to close any established session, independently on the type of the session... | [149-kill-session.md](../06-operation-commands/149-kill-session.md) |
| `security-policies` | The commands described in this section are used to edit or show security-policies | [280-security-policies.md](../06-operation-commands/280-security-policies.md) |

## Zero touch provisioning (ZTP)

*Search terms:* `ztp`, `zero touch`, `bootstrap`, `commissioning`, `brand new node`, `first admin`, `neighbour node`

| Command | What it does | File |
| --- | --- | --- |
| `ztp` | This command shows the Zero Touch Provisioning (ZTP) status | [374-ztp.md](../06-operation-commands/374-ztp.md) |
| `change-ztp-mode` | This command is used to toggle the Zero Touch Provisioning (ZTP) mode, deactivating it or reactivating it | [046-change-ztp-mode.md](../06-operation-commands/046-change-ztp-mode.md) |
| `bootstrap` | The command described in this section is used to bootstrap a neighbor NE by establishing a TLS... | [032-bootstrap.md](../06-operation-commands/032-bootstrap.md) |
