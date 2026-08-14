# GX CLI Reference - master command index

Routing index for the 1830 GX Release 9.1 CLI Reference Guide, split from `../R9_1_GX_CLI_Command_Reference_Guide_001P4.md`.
**395 commands** (374 operation, 7 navigation, 10 piped, 4 auxiliary) grouped into 16 functional domains.

## How to use this index

1. Match the user's query against the **domain headings** below, then the command rows. If the query uses vocabulary that is not a command name ("wavelength", "laser shutdown", "upgrade", "loopback"), start from [index/topics.md](index/topics.md) instead.
2. Open the file in the `File` column. Every operation command is a complete, self-contained page: description, syntax, access mode, full parameter table, and usually examples (median 53 lines, so reading the whole file is cheap).
3. For a parameter or attribute name, use [index/parameters.md](index/parameters.md). For an AID like `card-1-1` or `port-1-1-DCN`, use [index/entities.md](index/entities.md).
4. Page citations in the text ("refer to pm (p. 934)") resolve through [index/pages.tsv](index/pages.tsv); table and figure numbers through [index/tables.tsv](index/tables.tsv).

This file is large; grep it for a command or domain rather than reading it end to end.

`Mode` column: `oper` = Operational mode, `cand` = Candidate Configuration mode, `-` = the source states no access mode for this command (13 commands: all 10 piped plus 3 operation commands whose section has no usage table). A trailing `*` means the source qualifies the mode (for example "only for show command") - check the command page. `Verbs` is empty where the command is not invoked as `<verb> <entity>`.

## Domains

- [CLI, sessions and scripting](#cli-and-session) - 47 commands
- [Candidate config, commit, database and templates](#config-datastore) - 26 commands
- [Equipment, cards, ports, pluggables and inventory](#equipment-inventory) - 29 commands
- [Protection and switchover](#protection-redundancy) - 5 commands
- [Layer 0 photonics: spectrum, amplifiers, degrees, OTDR](#optical-layer0) - 55 commands
- [Layer 1 transport: OTN, Ethernet and client facilities](#transport-layer1) - 21 commands
- [Topology, fiber connections and neighbor discovery](#topology-discovery) - 23 commands
- [IP interfaces, routing protocols and DCN](#ip-networking) - 35 commands
- [Users, AAA and access control](#security-access-control) - 15 commands
- [Certificates, PKI and SSH keys](#certificates-pki) - 23 commands
- [Encryption: IPsec/IKEv2, MACsec and secure entities](#encryption-ipsec-macsec) - 26 commands
- [Management protocols, telemetry and third-party apps](#management-protocols) - 17 commands
- [Alarms, conditions and logging](#fault-alarms-logging) - 17 commands
- [Performance monitoring and statistics](#performance-monitoring) - 10 commands
- [Software, firmware, file transfer and ZTP](#software-firmware-files) - 33 commands
- [Node-level system, time and status](#system-node-time) - 13 commands

## CLI, sessions and scripting

<a id="cli-and-session"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `?` | Contextual help: displays what can be typed at the current prompt. | ? | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `add` | Layer 1 digital services that are currently provisioned in the system | add | oper+cand | [010-add.md](06-operation-commands/010-add.md) |
| `alias` | Value to replace the alias name with | alias | oper | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `begin` | Line to begin with |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#51-begin) |
| `clear` | Clears event counters (statistics) for the specified objects | clear | oper | [049-clear.md](06-operation-commands/049-clear.md) |
| `cli` | Columns to display in the output of 'show alarm' CLI command | set/show | oper+cand | [050-cli.md](06-operation-commands/050-cli.md) |
| `cli-session-config` | Determines if the current timestamp is printed on every CLI command | set/show | oper+cand | [051-cli-session-config.md](06-operation-commands/051-cli-session-config.md) |
| `connect` | Optional port | connect | oper | [058-connect.md](06-operation-commands/058-connect.md) |
| `convert` | CLI command; should be enclosed in quotes; if multiple commands are to be converted, they should be separated... | convert | oper+cand | [062-convert.md](06-operation-commands/062-convert.md) |
| `default` | Attribute names to be defaulted | default | oper+cand | [075-default.md](06-operation-commands/075-default.md) |
| `delete` | Layer 1 digital services that are currently provisioned in the system | delete | - | [077-delete.md](06-operation-commands/077-delete.md) |
| `display` | The display mode to be selected |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#52-display) |
| `edit` | Instance ID of the entity to be addressed | edit | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#42-edit) |
| `exclude` | Text to be filtered |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#53-exclude) |
| `exit` | Forces the command without confirmation | exit | oper+cand | [100-exit.md](06-operation-commands/100-exit.md) |
| `expect` | The expected value | expect | oper+cand | [101-expect.md](06-operation-commands/101-expect.md) |
| `export` | Value to replace variable with; can be any supported character, including spaces | export | oper+cand | [102-export.md](06-operation-commands/102-export.md) |
| `grep` | The following options are supported for grep:<br>• -a=&lt;n&gt; - Number of lines of context to show after... |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#54-grep) |
| `gshell` | Command to execute inside the Guest Container | gshell | oper | [122-gshell.md](06-operation-commands/122-gshell.md) |
| `help` | Displays help for a command, container, or attribute. | help | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `highlight` | Any word to highlight |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#55-highlight) |
| `history` | Displays help for this command | history | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#43-history) |
| `include` | Text to be filtered |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#56-include) |
| `kill-session` | An existing session-id | kill-session | oper | [149-kill-session.md](06-operation-commands/149-kill-session.md) |
| `linenum` | Any display command such as tree or show |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#57-linenum) |
| `message` | The CLI sessions to which the message will be sent | message | oper | [180-message.md](06-operation-commands/180-message.md) |
| `more` | Any display command such as tree or show |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#58-more) |
| `property` | The property to be set | show/set | oper+cand | [249-property.md](06-operation-commands/249-property.md) |
| `run` | Optional arguments to the script | run | oper+cand | [272-run.md](06-operation-commands/272-run.md) |
| `scheduled-task` | Output of the previous task run | add/set/show/delete | oper+cand | [275-scheduled-task.md](06-operation-commands/275-scheduled-task.md) |
| `session` | Name of the dial-out-server associated with this session | show | oper+cand | [286-session.md](06-operation-commands/286-session.md) |
| `set` | Layer 1 digital services that are currently provisioned in the system | set | oper+cand | [287-set.md](06-operation-commands/287-set.md) |
| `shell` | Displays help for this command | shell | oper | [290-shell.md](06-operation-commands/290-shell.md) |
| `show` | Zero Touch Provisioning (ZTP) status | show | oper+cand | [291-show.md](06-operation-commands/291-show.md) |
| `simulate` | The location of the simulated alarm | simulate | oper | [293-simulate.md](06-operation-commands/293-simulate.md) |
| `sleep` | Duration of delay in seconds | sleep | oper+cand | [294-sleep.md](06-operation-commands/294-sleep.md) |
| `sort` | Any attribute name that exists in the context of the output |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#59-sort) |
| `task` | Output of the previous task run | add/set/show/delete | oper+cand | [343-task.md](06-operation-commands/343-task.md) |
| `terminate` | Specific NMC entity for terminating the WSS loopback | terminate | oper | [348-terminate.md](06-operation-commands/348-terminate.md) |
| `tic` | Starts a timer for the typed command. | tic | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `toc` | Displays the elapsed time since the timer was started. | toc | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `top` | Displays help for this command | top | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#44-top) |
| `tree` | Instance ID of the entity to be displayed in the tree | tree | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#45-tree) |
| `unalias` | Name of the alias to remove | unalias | oper | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `until` | Line to end with |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#510-until) |
| `up` | Displays help for this command | up | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#47-up) |
| `update` | Instance(s) for the required update | update | oper | [362-update.md](06-operation-commands/362-update.md) |

## Candidate config, commit, database and templates

<a id="config-datastore"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `activate-snapshot` | Action to override the sanity check | activate-snapshot | oper | [009-activate-snapshot.md](06-operation-commands/009-activate-snapshot.md) |
| `advanced-parameter` | The current state of the advanced parameter | add/set/show/delete | oper+cand | [013-advanced-parameter.md](06-operation-commands/013-advanced-parameter.md) |
| `apply-template` | Applicable TOMS | apply-template | oper+cand | [023-apply-template.md](06-operation-commands/023-apply-template.md) |
| `commit` | This command &lt;id&gt; defines the ID of the commit confirmed, commit persist and confirmed cancel commands | commit | cand | [055-commit.md](06-operation-commands/055-commit.md) |
| `config` | Entity type to retrieve the configuration | show | - | [056-config.md](06-operation-commands/056-config.md) |
| `configure` | This parameter allows to leverage the to initialize the candidate from the configuration associated with a... | configure | oper | [057-configure.md](06-operation-commands/057-configure.md) |
| `current-advanced-parameter` | The value of the advanced parameter, which is running on the system | show | oper+cand | [065-current-advanced-parameter.md](06-operation-commands/065-current-advanced-parameter.md) |
| `database` | The password for the new-admin-user that is auto-configured after the database is wiped | clear/show | oper+cand | [072-database.md](06-operation-commands/072-database.md) |
| `db-migrate` | defines the protection mode to be configured | db-migrate | oper | [073-db-migrate.md](06-operation-commands/073-db-migrate.md) |
| `db-protection-scheme` | Current Protection Scheme of DB. Can be changed via 'db-migrate' RPC | show | oper+cand | [074-db-protection-scheme.md](06-operation-commands/074-db-protection-scheme.md) |
| `diff` | It is a system generated commit-id | diff | oper+cand | [080-diff.md](06-operation-commands/080-diff.md) |
| `discard-changes` | This command will discard all candidate datastore content and CLI return to operational mode | discard-changes | cand | [082-discard-changes.md](06-operation-commands/082-discard-changes.md) |
| `extended-config` | Displays the description of the extended-config provided by the system and its effect in the system | add/delete/show | oper+cand | [103-extended-config.md](06-operation-commands/103-extended-config.md) |
| `golden-advanced-parameter` | Identifies if applying this parameter change causes service impact | show | oper+cand | [119-golden-advanced-parameter.md](06-operation-commands/119-golden-advanced-parameter.md) |
| `lock` | Displays help for this command | lock | oper | [162-lock.md](06-operation-commands/162-lock.md) |
| `named-value-set` | Value item | add/set/delete/show | oper+cand | [184-named-value-set.md](06-operation-commands/184-named-value-set.md) |
| `recovery` | Timestamp for the next backup to be performed | set/show | oper+cand | [261-recovery.md](06-operation-commands/261-recovery.md) |
| `rollback` | This CLI command will revert the current Datastore either Running or Candidate, depending on operational mode... | rollback | oper+cand | [268-rollback.md](06-operation-commands/268-rollback.md) |
| `show commit` | Filter (&lt;attribute&gt;=&lt;value&gt;) | show | oper+cand | [292-show-commit.md](06-operation-commands/292-show-commit.md) |
| `system-policies` | Disabling writable-running policy makes it impossible to do configure commands via running datastore, making... | set/show | oper+cand | [341-system-policies.md](06-operation-commands/341-system-policies.md) |
| `take-snapshot` | Optional description for the generated snapshot | take-snapshot | oper | [342-take-snapshot.md](06-operation-commands/342-take-snapshot.md) |
| `template` | Represents the condition to apply on the template (e.g. service-type=OTU4)- optional | add/set/show/delete/apply-template | oper+cand | [345-template.md](06-operation-commands/345-template.md) |
| `template-group` | Represents the label to apply on the template - optional | add/show/delete | oper+cand | [346-template-group.md](06-operation-commands/346-template-group.md) |
| `templates` | This command is used to show the configuration that defines the data model for system templates | show | oper+cand | [347-templates.md](06-operation-commands/347-templates.md) |
| `unlock` | Displays help for this command | unlock | oper | [360-unlock.md](06-operation-commands/360-unlock.md) |
| `validate` | The command to validate | validate | oper+cand | [370-validate.md](06-operation-commands/370-validate.md) |

## Equipment, cards, ports, pluggables and inventory

<a id="equipment-inventory"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `capabilities` | the name of the card | show | oper+cand | [039-capabilities.md](06-operation-commands/039-capabilities.md) |
| `card` | List of sub-cards associated with this card | add/set/show/delete | oper+cand | [040-card.md](06-operation-commands/040-card.md) |
| `chassis` | Indicates if the chassis power consumption is limited by reducing max fan speed. i Note: This attribute is... | add/delete/set/show | oper+cand | [047-chassis.md](06-operation-commands/047-chassis.md) |
| `console` | Current status of the console for this card | set/show | oper+cand | [060-console.md](06-operation-commands/060-console.md) |
| `controller-card` | Additional details for synchronization status | show | oper+cand | [061-controller-card.md](06-operation-commands/061-controller-card.md) |
| `equipment` | The equipment to be viewed | show | oper+cand | [092-equipment.md](06-operation-commands/092-equipment.md) |
| `equipment-policies` | Physical location of the communication Ethernet ports | set/show | oper+cand | [093-equipment-policies.md](06-operation-commands/093-equipment-policies.md) |
| `equipment-templates` | Whether serdes-templates are globally enabled or not | set/show | oper+cand | [094-equipment-templates.md](06-operation-commands/094-equipment-templates.md) |
| `fru-info` | Type of the equipment | show | oper+cand | [114-fru-info.md](06-operation-commands/114-fru-info.md) |
| `inventory` | not-applicable - Card doesn't have upgradeable firmware. current - All components have current firmware.... | show | oper+cand | [137-inventory.md](06-operation-commands/137-inventory.md) |
| `led` | The state of the LED, that is, the current color status of the LED: not-available - LED status not available.... | show | oper+cand | [152-led.md](06-operation-commands/152-led.md) |
| `port` | Port usage type | set/show | oper+cand | [246-port.md](06-operation-commands/246-port.md) |
| `resources` | Available bandwidth for the paired slot connection. i Note: This parameter is applicable only for SPN2/SPN2C... | show | oper+cand | [264-resources.md](06-operation-commands/264-resources.md) |
| `serdes` | State of the advanced parameter (as observable on the system) once it is configured | add/set/show/delete | oper+cand | [282-serdes.md](06-operation-commands/282-serdes.md) |
| `serdes-template` | The list of ports to which this template is applicable, or 'all' if all ports are to be considered (default) | add/set/delete/show | oper+cand | [283-serdes-template.md](06-operation-commands/283-serdes-template.md) |
| `serdes-template-entry` | Value of the serdes parameter | add/set/delete/show | oper+cand | [284-serdes-template-entry.md](06-operation-commands/284-serdes-template-entry.md) |
| `serial-console` | Serial console inactivity timeout | set/show | oper+cand | [285-serial-console.md](06-operation-commands/285-serial-console.md) |
| `slot` | Name of the equipment that is currently required in this slot | show | oper+cand | [295-slot.md](06-operation-commands/295-slot.md) |
| `sub-component` | A user configurable description of the sub-component | show | oper+cand | [314-sub-component.md](06-operation-commands/314-sub-component.md) |
| `supported-card` | Supported features; may be empty if no features are not supported | show | oper+cand | [321-supported-card.md](06-operation-commands/321-supported-card.md) |
| `supported-chassis` | Supported features | show | oper+cand | [323-supported-chassis.md](06-operation-commands/323-supported-chassis.md) |
| `supported-port` | Indicates if TOMs that are plugged on this port type are auto migrated according with the equipment-policies... | show | oper+cand | [325-supported-port.md](06-operation-commands/325-supported-port.md) |
| `supported-slot` | List of LEDs available in the slot | show | oper+cand | [327-supported-slot.md](06-operation-commands/327-supported-slot.md) |
| `supported-tom` | The phy-mode that is used by default in this TOM for this card | show | oper+cand | [328-supported-tom.md](06-operation-commands/328-supported-tom.md) |
| `supported-tom-power` | Maximum power in watts the host port allows for this pluggable type under supported-power-class | show | oper+cand | [329-supported-tom-power.md](06-operation-commands/329-supported-tom-power.md) |
| `tom` | Specifies if the TOM is configured to function in the low power mode | add/set/show/delete | oper+cand | [352-tom.md](06-operation-commands/352-tom.md) |
| `tom-type` | 3rd party subtype for this TOM | show | oper+cand | [353-tom-type.md](06-operation-commands/353-tom-type.md) |
| `unprovisioned-inventory` | Timestamp with the last time the unprovisioned equipment was detected by the Node Controller | show | oper+cand | [361-unprovisioned-inventory.md](06-operation-commands/361-unprovisioned-inventory.md) |
| `usb` | Local filesystem path on where this USB file-system is mounted; this can be used as a target/ source for file... | show | oper+cand | [366-usb.md](06-operation-commands/366-usb.md) |

## Protection and switchover

<a id="protection-redundancy"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `manual-switchover` | The object to be manually switched | manual-switchover | oper | [177-manual-switchover.md](06-operation-commands/177-manual-switchover.md) |
| `protection` | This command is used to show protection | show | oper+cand | [250-protection.md](06-operation-commands/250-protection.md) |
| `protection-group` | Specifies the last reason that triggered a protection switchover | add/set/show/delete | oper+cand | [251-protection-group.md](06-operation-commands/251-protection-group.md) |
| `protection-switch` | The target of the switch command | protection-switch | oper | [252-protection-switch.md](06-operation-commands/252-protection-switch.md) |
| `protection-unit` | Protection unit role | set/show | oper+cand | [253-protection-unit.md](06-operation-commands/253-protection-unit.md) |

## Layer 0 photonics: spectrum, amplifiers, degrees, OTDR

<a id="optical-layer0"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `adg` | List of bands supported by an ADG, with dependence on supported cards.<br>• not-applicable -Transmission band... | set/show/delete | oper+cand | [012-adg.md](06-operation-commands/012-adg.md) |
| `amplifier` | Control speed factor for the DGE power control algorithm | set/show | oper+cand | [019-amplifier.md](06-operation-commands/019-amplifier.md) |
| `amplifier-raman` | Indicates the current state of the power control adjustment for the preamplifier:<br>• unknown : default... | set/show | oper+cand | [020-amplifier-raman.md](06-operation-commands/020-amplifier-raman.md) |
| `ase-idler-service` | • enabled: ASE idler signal filling on the unused and nmc-failed portions of the band spectrum is... | add/delete/set/show | oper+cand | [024-ase-idler-service.md](06-operation-commands/024-ase-idler-service.md) |
| `ase-idler-source` | ASE pump output power required (if manually configured) | set/show | oper+cand | [025-ase-idler-source.md](06-operation-commands/025-ase-idler-source.md) |
| `calibrate` | Select the entity to be calibrated | calibrate | oper | [036-calibrate.md](06-operation-commands/036-calibrate.md) |
| `degree` | List of bands supported by a degree, with dependence on supported cards.<br>• not-applicable -Transmission... | add/delete/set/show | oper+cand | [076-degree.md](06-operation-commands/076-degree.md) |
| `direction` | Instance of the card's port hosting this direction (index) | add/delete/set/show | oper+cand | [081-direction.md](06-operation-commands/081-direction.md) |
| `dsc` | Flag indicating if alarm the reporting is allowed | add/delete/set/show | oper+cand | [089-dsc.md](06-operation-commands/089-dsc.md) |
| `dsc-group` | The threshold to raise the DGD- OORH alarm (in ps) | add/delete/set/show | oper+cand | [090-dsc-group.md](06-operation-commands/090-dsc-group.md) |
| `gadt` | Detailed description of application ID | show | oper+cand | [115-gadt.md](06-operation-commands/115-gadt.md) |
| `gapt` | The managed resource type(s) that are applicable for this particular advanced parameter | show | oper+cand | [116-gapt.md](06-operation-commands/116-gapt.md) |
| `gcmt` | table version | show | oper+cand | [117-gcmt.md](06-operation-commands/117-gcmt.md) |
| `golden-carrier-mode` | Subtypes for which this carrier mode has candidate status | show | oper+cand | [120-golden-carrier-mode.md](06-operation-commands/120-golden-carrier-mode.md) |
| `l0-capabilities` | The command described in this section is used to show the capabilities details related with node's Layer 0... | show | oper+cand | [151-l0-capabilities.md](06-operation-commands/151-l0-capabilities.md) |
| `mc` | When enabled, the system may auto-delete this MC once it has no associated NMC. When disabled, the MC stays... | add/set/show/delete | oper+cand | [178-mc.md](06-operation-commands/178-mc.md) |
| `mc-f` | Slot width, as calculated by the system, from upper-frequency - lower-frequency | show | oper+cand | [179-mc-f.md](06-operation-commands/179-mc-f.md) |
| `modules-adg` | Set upon creation, cannot be changed after supported-card being assigned | add/set/show/delete | oper+cand | [181-modules-adg.md](06-operation-commands/181-modules-adg.md) |
| `modules-degree` | Instance of card or subcard that belongs to the degree | add/set/show/delete | oper+cand | [182-modules-degree.md](06-operation-commands/182-modules-degree.md) |
| `monitored-channel` | Carrier (channel) width configured at the NMC within the oxcon source/ destination, in MHz | show | oper+cand | [183-monitored-channel.md](06-operation-commands/183-monitored-channel.md) |
| `nmc` | When enabled, the system may auto-delete this NMC once it has no associated OXcon | add/set/show/delete | oper+cand | [193-nmc.md](06-operation-commands/193-nmc.md) |
| `nmc-f` | Network Media Channel attenuation adjustment applied by auto-controls to do power targeting in mux direction | set/show | oper+cand | [194-nmc-f.md](06-operation-commands/194-nmc-f.md) |
| `oadm-capabilities` | Maximum number of ADGs (Add/ Drop Group(s)); 0 if not supported | show | oper+cand | [200-oadm-capabilities.md](06-operation-commands/200-oadm-capabilities.md) |
| `oc` | System configured circuit id | set/show | oper+cand | [201-oc.md](06-operation-commands/201-oc.md) |
| `ochm` | DGE VOA attenuation of channel | set/show | oper+cand | [202-ochm.md](06-operation-commands/202-ochm.md) |
| `ocm-channel` | Yields 'true' if the channel is configured (involved in an oxcon) | show | oper+cand | [203-ocm-channel.md](06-operation-commands/203-ocm-channel.md) |
| `ocm-mp` | System reports 'enabled' when OMS reference exists | set/show | oper+cand | [204-ocm-mp.md](06-operation-commands/204-ocm-mp.md) |
| `ocm-ptp` | System reports 'enabled' when complete connectivity at AD is established, and OCM measurement is possible | set/show | oper+cand | [205-ocm-ptp.md](06-operation-commands/205-ocm-ptp.md) |
| `oms` | System reports this attribute to indicate whether the OMS is intended to be in use (instead of simply being... | set/show | oper+cand | [209-oms.md](06-operation-commands/209-oms.md) |
| `ops` | Intended for Y-cable expansion | set/show | oper+cand | [210-ops.md](06-operation-commands/210-ops.md) |
| `optical-carrier` | Controls enabling/disabling sop data collection, providing the collection interval in ms | set/show | oper+cand | [211-optical-carrier.md](06-operation-commands/211-optical-carrier.md) |
| `optical-channel` | Describes whether this facility was system created or not | set/show | oper+cand | [212-optical-channel.md](06-operation-commands/212-optical-channel.md) |
| `optical-ptp` | Fiber patch cord length between the Raman DWDM port and the base card DWDM line port | set/show | oper+cand | [213-optical-ptp.md](06-operation-commands/213-optical-ptp.md) |
| `optical-switch` | SD threshold hysteresis (in dB) | set/show | oper+cand | [214-optical-switch.md](06-operation-commands/214-optical-switch.md) |
| `osc` | Represents the actual received OSC power value measured at DWDM Line port input | set/show | oper+cand | [215-osc.md](06-operation-commands/215-osc.md) |
| `otdr` | Displays which pre-defined OTDR measurement profile is in progress:<br>• none: Indicates automatic otdr scan... | add/delete/set/show | oper+cand | [223-otdr.md](06-operation-commands/223-otdr.md) |
| `otdr-ptp` | The last OTDR measurement the generated .sor file | add/delete/set/show | oper+cand | [224-otdr-ptp.md](06-operation-commands/224-otdr-ptp.md) |
| `ots` | Currently this attribute is applicable to SLTE only | set/show | oper+cand | [225-ots.md](06-operation-commands/225-ots.md) |
| `ots-diagnostics` | The port-id in OTS TTI is the AID of the port but limited to 32 printable characters | set/show | oper+cand | [226-ots-diagnostics.md](06-operation-commands/226-ots-diagnostics.md) |
| `ots-r` | Connected Reference | set/show | oper+cand | [227-ots-r.md](06-operation-commands/227-ots-r.md) |
| `ots-r-auto-otdr` | Displays the status of the automatic OTDR execution for the corresponding OTS-R facility:<br>•... | add/delete/set/show | oper+cand | [228-ots-r-auto-otdr.md](06-operation-commands/228-ots-r-auto-otdr.md) |
| `oxcon` | Path/ service name of optical cross-connection | add/set/show/delete | oper+cand | [231-oxcon.md](06-operation-commands/231-oxcon.md) |
| `profile-control` | Profile data to be inputted | profile-control | oper | [248-profile-control.md](06-operation-commands/248-profile-control.md) |
| `pump` | Describes whether this facility was system created or not | set/show | oper+cand | [255-pump.md](06-operation-commands/255-pump.md) |
| `pump-power` | The actual values which are currently measured in each pump | set/show | oper+cand | [256-pump-power.md](06-operation-commands/256-pump-power.md) |
| `raman-calibration` | Indicates any information for troubleshooting when the calibration-state is fail or out-dated | add/delete/set/show | oper+cand | [257-raman-calibration.md](06-operation-commands/257-raman-calibration.md) |
| `rsc` | The transmitted Pilot Tone integrated power | set/show | oper+cand | [271-rsc.md](06-operation-commands/271-rsc.md) |
| `spectrum` | Unique attenuation value for entire spectrum [dB] | set/show | oper+cand | [303-spectrum.md](06-operation-commands/303-spectrum.md) |
| `spectrum-control` | The intended target output power for the spectra | add/delete/set/show | oper+cand | [304-spectrum-control.md](06-operation-commands/304-spectrum-control.md) |
| `spectrum-monitoring` | Currently calculated PSD. The Power Spectral Density does not depend on the spectra width | show | oper+cand | [305-spectrum-monitoring.md](06-operation-commands/305-spectrum-monitoring.md) |
| `super-channel` | Theoretical total TX power at Faceplate calculated based on per carrier Target TX power value | add/show/delete | oper+cand | [319-super-channel.md](06-operation-commands/319-super-channel.md) |
| `super-channel-group` | -m | add/set/show | oper+cand | [320-super-channel-group.md](06-operation-commands/320-super-channel-group.md) |
| `supported-carrier-mode` | Subtypes that each carrier mode supports | show | oper+cand | [322-supported-carrier-mode.md](06-operation-commands/322-supported-carrier-mode.md) |
| `supported-gain-range` | The maximum settable gain-target for this type of range ('standard'/ 'low'/ 'high') | show | oper+cand | [324-supported-gain-range.md](06-operation-commands/324-supported-gain-range.md) |
| `supported-power-profile` | Whether is the default value or not | show | oper+cand | [326-supported-power-profile.md](06-operation-commands/326-supported-power-profile.md) |

## Layer 1 transport: OTN, Ethernet and client facilities

<a id="transport-layer1"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `bert` | specifies the duration of the test is run in seconds | bert | oper | [028-bert.md](06-operation-commands/028-bert.md) |
| `cid-ptp` | It is true when CableID functionality is supported | set/show | oper+cand | [048-cid-ptp.md](06-operation-commands/048-cid-ptp.md) |
| `eth-zr` | Loopback on modem interface | add/set/show/delete | oper+cand | [098-eth-zr.md](06-operation-commands/098-eth-zr.md) |
| `ethernet` | System configured circuit ID | set/show | oper+cand | [099-ethernet.md](06-operation-commands/099-ethernet.md) |
| `facilities` | This command is used to show system facilities | show | oper+cand | [105-facilities.md](06-operation-commands/105-facilities.md) |
| `fc` | System configured circuit ID, present in XCONs and associated facilities | set/show | oper+cand | [106-fc.md](06-operation-commands/106-fc.md) |
| `flexo` | The received iid on the FlexO interface | set/show | oper+cand | [112-flexo.md](06-operation-commands/112-flexo.md) |
| `flexo-group` | Indicates the interface group instance that the FlexO-x interface is a member of |  | oper+cand | [113-flexo-group.md](06-operation-commands/113-flexo-group.md) |
| `high-speed-monitoring` | User configurable port | set/show | oper+cand | [123-high-speed-monitoring.md](06-operation-commands/123-high-speed-monitoring.md) |
| `interlaken` | Total capacity for the interlaken interface | set/show | oper+cand | [136-interlaken.md](06-operation-commands/136-interlaken.md) |
| `L2-bridge` | Description of the bridge and its intended purpose | set/show | oper+cand | [170-l2-bridge.md](06-operation-commands/170-l2-bridge.md) |
| `line-ptp` | Provide an aggregate view of all used resources on the DSP | add/set/show/delete | oper+cand | [153-line-ptp.md](06-operation-commands/153-line-ptp.md) |
| `network-xconnect` | This command is used to show the list of services of multiple user cross connections commissioned in this NE | show | oper+cand | [189-network-xconnect.md](06-operation-commands/189-network-xconnect.md) |
| `nw-xconnect` | Maximum bandwidth rate of the user channel (in Mbps units) | add/delete/set/show | oper+cand | [199-nw-xconnect.md](06-operation-commands/199-nw-xconnect.md) |
| `odu` | Provides an aggregate view of used resources in the DSP | add/set/show/delete | oper+cand | [207-odu.md](06-operation-commands/207-odu.md) |
| `odu-diagnostics` | Monitor the incoming test signals for diagnostics | add/set/show/delete | oper+cand | [208-odu-diagnostics.md](06-operation-commands/208-odu-diagnostics.md) |
| `otu` | Time slots of the ODU | add/set/show/delete | oper+cand | [229-otu.md](06-operation-commands/229-otu.md) |
| `otu-diagnostics` | The threshold in percentage of block errors versus total blocks at which a degrade-interval number of seconds... | set/show | oper+cand | [230-otu-diagnostics.md](06-operation-commands/230-otu-diagnostics.md) |
| `stm` | The system configured circuit ID | set/show | oper+cand | [313-stm.md](06-operation-commands/313-stm.md) |
| `trib-ptp` | -m | add/set/show/delete | oper+cand | [358-trib-ptp.md](06-operation-commands/358-trib-ptp.md) |
| `xcon` | List of resources being used by this XCON besides the two main source/destination end-points | add/set/show/delete | oper+cand | [373-xcon.md](06-operation-commands/373-xcon.md) |

## Topology, fiber connections and neighbor discovery

<a id="topology-discovery"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `cable-id` | The commands described in this section are used to show the `cable-id` entities and terminate a CableID... | show/terminate | oper+cand* | [033-cable-id.md](06-operation-commands/033-cable-id.md) |
| `cable-id-path` | Displays a list of supporting-fiber-connections | show | oper+cand | [034-cable-id-path.md](06-operation-commands/034-cable-id-path.md) |
| `cable-id-status` | Display the cable-id test progress | show | oper+cand | [035-cable-id-status.md](06-operation-commands/035-cable-id-status.md) |
| `carrier-neighbor` | IPv6 loopback address of the neighbor; may be empty if not configured | show | oper+cand | [041-carrier-neighbor.md](06-operation-commands/041-carrier-neighbor.md) |
| `connection-ports` | The dwdm-line port of RD or ILAx card | show | oper+cand | [059-connection-ports.md](06-operation-commands/059-connection-ports.md) |
| `custom-tlv` | The sub-type identifier of the TLV in the scope of the OUI The firmware name | show | oper+cand | [069-custom-tlv.md](06-operation-commands/069-custom-tlv.md) |
| `external-fiber-connection` | Type of the fiber connection | add/set/show/delete | oper+cand | [104-external-fiber-connection.md](06-operation-commands/104-external-fiber-connection.md) |
| `fiber-connection` | Type of the fiber connection | add/set/show/delete | oper+cand | [107-fiber-connection.md](06-operation-commands/107-fiber-connection.md) |
| `icdp` | Flag to enable icdp | set/show | oper+cand | [125-icdp.md](06-operation-commands/125-icdp.md) |
| `inci` | Switch to enable INCI | set/show | oper+cand | [132-inci.md](06-operation-commands/132-inci.md) |
| `inci-neighbor` | The operational state of this object | add/set/show/delete | oper+cand | [133-inci-neighbor.md](06-operation-commands/133-inci-neighbor.md) |
| `interface-neighbor` | Resource Access Identifier (AID) | set/show | oper+cand | [135-interface-neighbor.md](06-operation-commands/135-interface-neighbor.md) |
| `links` | This command is used to show the links container within the topology | show | oper+cand | [154-links.md](06-operation-commands/154-links.md) |
| `lldp` | Time to keep neighbor information, in case neighbor does not have an explicit Time-To-Live (TTL) TLV | set/show | oper+cand | [155-lldp.md](06-operation-commands/155-lldp.md) |
| `lldp-local-info` | This attribute describes the remote system enabled capabilities | show | oper+cand | [156-lldp-local-info.md](06-operation-commands/156-lldp-local-info.md) |
| `lldp-neighbor` | Remote system info Time-To-Live (TTL) | show | oper+cand | [157-lldp-neighbor.md](06-operation-commands/157-lldp-neighbor.md) |
| `lldp-port-statistics` | This counter provides a count of all TLVs not recognized by the receiving LLDP local agent | show | oper+cand | [158-lldp-port-statistics.md](06-operation-commands/158-lldp-port-statistics.md) |
| `nct-connection` | The state of the dst-chassis | show | oper+cand | [185-nct-connection.md](06-operation-commands/185-nct-connection.md) |
| `sndp` | This is a switch to control the sndp feature | set/show | oper+cand | [296-sndp.md](06-operation-commands/296-sndp.md) |
| `submarine-link` | Allocated spectrum blocks for the link configured as a set of start frequency, end frequency pairs | add/delete/set/show | oper+cand | [315-submarine-link.md](06-operation-commands/315-submarine-link.md) |
| `supporting-fiber-connection` | Supported fiber connection path | show | oper+cand | [330-supporting-fiber-connection.md](06-operation-commands/330-supporting-fiber-connection.md) |
| `topology` | Topology instance to be viewed:<br>• inci - Refer to for inci (p. 549) additional information on INCI... | clear/show | oper+cand* | [354-topology.md](06-operation-commands/354-topology.md) |
| `verify` | Result of the verification operation | verify | oper | [371-verify.md](06-operation-commands/371-verify.md) |

## IP interfaces, routing protocols and DCN

<a id="ip-networking"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `bgp-instance` | Specifies the router ID. 0.0.0.0/0 is not supported for IPv4 and 0::0.0 is not supported for IPv6 | add/delete/set/show | oper+cand | [029-bgp-instance.md](06-operation-commands/029-bgp-instance.md) |
| `bgp-neighbor` | Current BGP Session state errors if any ASCII format | add/delete/set/show | oper+cand | [030-bgp-neighbor.md](06-operation-commands/030-bgp-neighbor.md) |
| `bgp-network` | Specifies the network prefix | add/delete/show | oper+cand | [031-bgp-network.md](06-operation-commands/031-bgp-network.md) |
| `comm-channel` | Indicates the mode of operation of control channel | add/delete/set/show | oper+cand | [053-comm-channel.md](06-operation-commands/053-comm-channel.md) |
| `comm-eth` | The operational state of this object | set/show | oper+cand | [054-comm-eth.md](06-operation-commands/054-comm-eth.md) |
| `dhcp-relay` | DHCP server ip-addresses; when enabled at least one IP address should be configured | set/show | oper+cand | [078-dhcp-relay.md](06-operation-commands/078-dhcp-relay.md) |
| `dns` | DNS-search-suffix name | set/show | oper+cand | [084-dns.md](06-operation-commands/084-dns.md) |
| `dns-server` | DNS address assignment method, the user can convert DHCP configured DNS entry into a manual configured by... | add/delete/set/show | oper+cand | [085-dns-server.md](06-operation-commands/085-dns-server.md) |
| `if-dhcp-relay` | Enables dhcp-relay function on this interface | set/show | oper+cand | [126-if-dhcp-relay.md](06-operation-commands/126-if-dhcp-relay.md) |
| `interface` | User defined label | add/set/show/delete | oper+cand | [134-interface.md](06-operation-commands/134-interface.md) |
| `ip-monitoring` | Controls the reporting of alarms for this particular object | add/set/show/delete | oper+cand | [138-ip-monitoring.md](06-operation-commands/138-ip-monitoring.md) |
| `ipv4-address` | IPv4 address assignment method. static: Indicates that the address has been statically\n configured - for... | add/show/delete | oper+cand | [143-ipv4-address.md](06-operation-commands/143-ipv4-address.md) |
| `ipv4-static-route` | The routes to be advertised to external AS must exist in the forwarding table installed by an Interior... | add/set/show/delete | oper+cand | [144-ipv4-static-route.md](06-operation-commands/144-ipv4-static-route.md) |
| `ipv6-address` | IPv6 address assignment method. static: Indicates that the address has been statically\n configured - for... | add/show/delete | oper+cand | [145-ipv6-address.md](06-operation-commands/145-ipv6-address.md) |
| `ipv6-static-route` | The routes to be advertised to external AS must exist in the forwarding table installed by an Interior... | add/set/show/delete | oper+cand | [146-ipv6-static-route.md](06-operation-commands/146-ipv6-static-route.md) |
| `management-address` | The Object Identifier (OID) value used to identify the type of hardware component or protocol entity... | show | oper+cand | [174-management-address.md](06-operation-commands/174-management-address.md) |
| `management-address-local` | The Object Identifier (OID) value used to identify the type of hardware component or protocol entity... | show | oper+cand | [175-management-address-local.md](06-operation-commands/175-management-address-local.md) |
| `networking` | Interface to use as source address | set/show | oper+cand | [190-networking.md](06-operation-commands/190-networking.md) |
| `networking-services` | This command is used to show the list of network services | show | oper+cand | [191-networking-services.md](06-operation-commands/191-networking-services.md) |
| `next-hop` | IP address of the next-hop | show | oper+cand | [192-next-hop.md](06-operation-commands/192-next-hop.md) |
| `ospf` | The id of the ospf-instance needs to be provided as &lt;instance&gt; | clear | oper | [216-ospf.md](06-operation-commands/216-ospf.md) |
| `ospf-area` | OSPF Router Area Type | add/set/show/delete | oper+cand | [217-ospf-area.md](06-operation-commands/217-ospf-area.md) |
| `ospf-area-range` | Advertise or hide | add/set/show/delete | oper+cand | [218-ospf-area-range.md](06-operation-commands/218-ospf-area-range.md) |
| `ospf-instance` | Flag to indicate router-id is loopback IP or manual configured | add/set/show/delete | oper+cand | [219-ospf-instance.md](06-operation-commands/219-ospf-instance.md) |
| `ospf-interface` | Authentication key string in ASCII format | add/set/show/delete | oper+cand | [220-ospf-interface.md](06-operation-commands/220-ospf-interface.md) |
| `ospf-neighbor` | OSPF router priority | show | oper | [221-ospf-neighbor.md](06-operation-commands/221-ospf-neighbor.md) |
| `ospfv3-ipsec-security-association` | Indicates IPsec mode | add/set/show/delete | oper+cand | [222-ospfv3-ipsec-security-association.md](06-operation-commands/222-ospfv3-ipsec-security-association.md) |
| `ping` | IP address of the destination of the ICMP ECHO REQUEST datagram. _ | ping | oper+cand | [235-ping.md](06-operation-commands/235-ping.md) |
| `protocols` | Show data-model-openconfig protocols | show | oper+cand | [254-protocols.md](06-operation-commands/254-protocols.md) |
| `rib` | Address family | show | oper+cand | [267-rib.md](06-operation-commands/267-rib.md) |
| `route` | Source protocol for the route entry | show | oper+cand | [269-route.md](06-operation-commands/269-route.md) |
| `routing` | This command is used to show routing information | show | oper+cand | [270-routing.md](06-operation-commands/270-routing.md) |
| `supporting-interface` | A reference to the IPv4/IPv6 interface | show | oper+cand | [331-supporting-interface.md](06-operation-commands/331-supporting-interface.md) |
| `traceroute` | IP address of the destination of the ICMP ECHO REQUEST datagram. _ | traceroute | oper+cand | [355-traceroute.md](06-operation-commands/355-traceroute.md) |
| `vrf` | Associated chassis name to this VRF | show | oper+cand | [372-vrf.md](06-operation-commands/372-vrf.md) |

## Users, AAA and access control

<a id="security-access-control"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `aaa-server` | Specifies the number of attempted Access-Request messages to a single AAA server before failing authentication | add/set/show/delete | oper+cand | [001-aaa-server.md](06-operation-commands/001-aaa-server.md) |
| `aaa-statistics` | Displays the number of accounting requests | show | oper+cand | [002-aaa-statistics.md](06-operation-commands/002-aaa-statistics.md) |
| `access-control-list` | This command is used to show access control list | show | oper+cand | [003-access-control-list.md](06-operation-commands/003-access-control-list.md) |
| `access-rule` | A user-configurable description about this access rule | add/delete/set/show | oper+cand | [004-access-rule.md](06-operation-commands/004-access-rule.md) |
| `access-rule-list` | A generic description of this access-rule-list | add/delete/set/show | oper+cand | [005-access-rule-list.md](06-operation-commands/005-access-rule-list.md) |
| `ace` | User-configurable label | add/set/show/delete | oper+cand | [006-ace.md](06-operation-commands/006-ace.md) |
| `acl` | Indicates the top-level type of ACL, i.e., what fields from the associated IPv4 or IPv6 headers this ACL matches on | add/set/show/delete | oper+cand | [007-acl.md](06-operation-commands/007-acl.md) |
| `auth-key` | Indicates whether the integrity key is ASCII or hexadecimal encoded | add/set/show/delete | oper+cand | [026-auth-key.md](06-operation-commands/026-auth-key.md) |
| `authorization` | Number of times since the system last restarted that a notification was dropped for a subscription because... | set/show | oper+cand | [027-authorization.md](06-operation-commands/027-authorization.md) |
| `password` | The the new password inline with the command | password | oper | [233-password.md](06-operation-commands/233-password.md) |
| `security` | The command described in this section is used to show the top level security container | show | oper+cand | [279-security.md](06-operation-commands/279-security.md) |
| `security-policies` | This policy defines whether OCSP responders can be consulted for certificate revocation checking | set/show | oper+cand | [280-security-policies.md](06-operation-commands/280-security-policies.md) |
| `user` | User defined label | add/set/show/delete | oper+cand | [367-user.md](06-operation-commands/367-user.md) |
| `user-data` | The commands described in this section are used to show the `user-data` | show | oper+cand | [368-user-data.md](06-operation-commands/368-user-data.md) |
| `user-group` | Long description of the user group | add/delete/set/show | oper+cand | [369-user-group.md](06-operation-commands/369-user-group.md) |

## Certificates, PKI and SSH keys

<a id="certificates-pki"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `cdp` | Result of the most recent CRL update | add/set/show/delete | oper+cand | [042-cdp.md](06-operation-commands/042-cdp.md) |
| `cert-gen` | Auto-assign certificate to any secure-application without active certificate | cert-gen | oper | [043-cert-gen.md](06-operation-commands/043-cert-gen.md) |
| `cert-to-name` | Specifies the user label | add/delete/set/show | oper+cand | [044-cert-to-name.md](06-operation-commands/044-cert-to-name.md) |
| `certificate` | Certificate ID. The id must match a currently installed but unused certificate of the provided type | clear/show | oper | [045-certificate.md](06-operation-commands/045-certificate.md) |
| `crl` | The HTTP URI from which this CRL was auto-downloaded | clear/show | oper+cand* | [063-crl.md](06-operation-commands/063-crl.md) |
| `csr-gen` | The Extended Key Usage type(s) for the certificate |  | oper | [064-csr-gen.md](06-operation-commands/064-csr-gen.md) |
| `display-cert` | Defines the requested type of display operation | display-cert | oper+cand | [083-display-cert.md](06-operation-commands/083-display-cert.md) |
| `est` | The credentials used to authenticate a user when accessing resources protected by the HTTP protocol | est | oper+cand | [095-est.md](06-operation-commands/095-est.md) |
| `est-ca` | Specifies the number of days before expiration at which re-enrollment will be performed for all leaf... | set/show | oper+cand | [096-est-ca.md](06-operation-commands/096-est-ca.md) |
| `est-server` | Specifies an optional label added to the EST base url | add/delete/set | oper+cand | [097-est-server.md](06-operation-commands/097-est-server.md) |
| `import-certificate` | Import any intermediate certificates present in a PEM string bundle | import-certificate | oper | [131-import-certificate.md](06-operation-commands/131-import-certificate.md) |
| `ISK` | Signature Generation Time | clear/show | oper+cand* | [147-isk.md](06-operation-commands/147-isk.md) |
| `key-replacement-package` | Indicates if this KRP has been installed in the system | show | oper+cand | [148-key-replacement-package.md](06-operation-commands/148-key-replacement-package.md) |
| `KRK` | Key Payload (hex format) | show | oper+cand | [150-krk.md](06-operation-commands/150-krk.md) |
| `local-certificate` | User defined label | set/show | oper+cand | [159-local-certificate.md](06-operation-commands/159-local-certificate.md) |
| `ocsp-server` | Timestamp of last successful query | add/set/delete/show | oper+cand | [206-ocsp-server.md](06-operation-commands/206-ocsp-server.md) |
| `peer-certificate` | User-defined label | set/show | oper+cand | [234-peer-certificate.md](06-operation-commands/234-peer-certificate.md) |
| `ssh` | Welcome message displayed after user login | set/show | oper+cand | [306-ssh.md](06-operation-commands/306-ssh.md) |
| `ssh-authorized-key` | User defined label | add/set/show/delete | oper+cand | [307-ssh-authorized-key.md](06-operation-commands/307-ssh-authorized-key.md) |
| `ssh-host-key` | Fingerprint string as a sequence of pairs of hex digits | show | oper+cand | [308-ssh-host-key.md](06-operation-commands/308-ssh-host-key.md) |
| `ssh-keygen` | Specify type of key to generate | ssh-keygen | oper | [309-ssh-keygen.md](06-operation-commands/309-ssh-keygen.md) |
| `ssh-known-host` | User defined label | add/set/show/delete | oper+cand | [310-ssh-known-host.md](06-operation-commands/310-ssh-known-host.md) |
| `trusted-certificate` | User defined label | set/show | oper+cand | [359-trusted-certificate.md](06-operation-commands/359-trusted-certificate.md) |

## Encryption: IPsec/IKEv2, MACsec and secure entities

<a id="encryption-ipsec-macsec"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `additional-key-exchange` | A list of IKE SA Diffie-Hellman groups + advertised to the far-end IKE peer | add/set/show/delete | oper+cand | [011-additional-key-exchange.md](06-operation-commands/011-additional-key-exchange.md) |
| `data-path-encryption` | This command is used to show datapath encryption attributes | show | oper+cand | [071-data-path-encryption.md](06-operation-commands/071-data-path-encryption.md) |
| `encryption-algorithm` | The IKE SA encryption algorithm key length | add/show/delete | oper+cand | [091-encryption-algorithm.md](06-operation-commands/091-encryption-algorithm.md) |
| `ike-sa-proposal` | A list of protocol proposals when negotiating the IKE SA + with the far-end IKE peer | add/set/show/delete | oper+cand | [127-ike-sa-proposal.md](06-operation-commands/127-ike-sa-proposal.md) |
| `ikev2` | A global, L1 encryption-specific policy that indicates whether the NE must validate Certificate subject... | set/show | oper+cand | [128-ikev2.md](06-operation-commands/128-ikev2.md) |
| `ikev2-local-instance` | Local IPv4 address for IKEv2 channel with prefix-length 32 | set/show | oper+cand | [129-ikev2-local-instance.md](06-operation-commands/129-ikev2-local-instance.md) |
| `ikev2-peer` | Indicates whether PPK use is mandatory or optional for the IKEv2 peer. i Note: If this parameter is set to... | add/set/show/delete | oper+cand | [130-ikev2-peer.md](06-operation-commands/130-ikev2-peer.md) |
| `ipsec-sa-proposal` | Extended Sequence Number (ESN) support | add/set/show/delete | oper+cand | [139-ipsec-sa-proposal.md](06-operation-commands/139-ipsec-sa-proposal.md) |
| `ipsec-sa-re-key` | The rekeying frequency for the IPsec child security association with the far-end peer based on amount of... | add/set/show/delete | oper+cand | [140-ipsec-sa-re-key.md](06-operation-commands/140-ipsec-sa-re-key.md) |
| `ipsec-spd-entry` | Indicates whether dynamic traffic selector is enabled in this SPD entry | add/set/show/delete | oper+cand | [141-ipsec-spd-entry.md](06-operation-commands/141-ipsec-spd-entry.md) |
| `ipsec-traffic-selector` | Indicates the inner protocol (upper layer), obtained from the IPv4 protocol or the IPv6 next header field | add/set/show/delete | oper+cand | [142-ipsec-traffic-selector.md](06-operation-commands/142-ipsec-traffic-selector.md) |
| `local-ports` | The values for the stopping port | add/show/delete | oper+cand | [160-local-ports.md](06-operation-commands/160-local-ports.md) |
| `local-subnet` | This is a list of ranges of IPv4/IPv6 addresses (unicast, broadcast (IPv4 only)) | add/show/delete | oper+cand | [161-local-subnet.md](06-operation-commands/161-local-subnet.md) |
| `macsec-entity` | Number of packets to consider for replay protection window | add/show/set/delete | oper+cand | [171-macsec-entity.md](06-operation-commands/171-macsec-entity.md) |
| `macsec-mka` | Indicates whether PSK lifetime notification is enabled or disabled | set/show | oper+cand | [172-macsec-mka.md](06-operation-commands/172-macsec-mka.md) |
| `mka-policy` | Secure Association Key(SAK) rekey interval in seconds | add/show/set/delete | oper+cand | [173-mka-policy.md](06-operation-commands/173-mka-policy.md) |
| `re-auth` | A reference to the IKE peer object (IKE SA) | re-auth | oper | [258-re-auth.md](06-operation-commands/258-re-auth.md) |
| `re-key` | Points to secure entity object (Child SA) | re-key | oper | [259-re-key.md](06-operation-commands/259-re-key.md) |
| `remote-ports` | The values for the stopping port | add/show/delete | oper+cand | [262-remote-ports.md](06-operation-commands/262-remote-ports.md) |
| `remote-subnet` | This is a list of ranges of IPv4/IPv6 addresses (unicast, broadcast (IPv4 only)) | add/show/delete | oper+cand | [263-remote-subnet.md](06-operation-commands/263-remote-subnet.md) |
| `sc-rx` | State of the secure channel returned by MKA stack: • in-use: Indicates Secure Association(s) under this... | show | oper+cand | [273-sc-rx.md](06-operation-commands/273-sc-rx.md) |
| `sc-tx` | State of the secure channel returned by MKA stack: • in-use: Indicates Secure Association(s) under this... | show | oper+cand | [274-sc-tx.md](06-operation-commands/274-sc-tx.md) |
| `secure-application` | Enables or disables TLS Mutual Authentication | set/show | oper+cand | [276-secure-application.md](06-operation-commands/276-secure-application.md) |
| `secure-entity` | If the re-key fail policy is set to KILL-TRAFFIC, this attribute indicates the amount of time the system... | add/set/show/delete | oper+cand | [277-secure-entity.md](06-operation-commands/277-secure-entity.md) |
| `secure-entity-sa-proposal` | Secure entity SA Diffie-Hellman group advertised to the far-end secure entity peer | show | oper+cand | [278-secure-entity-sa-proposal.md](06-operation-commands/278-secure-entity-sa-proposal.md) |
| `security-policy-database` | List of all SPD entries associated with this far-end peer for which IKE negotiates security associations (keys) | add/set/show/delete | oper+cand | [281-security-policy-database.md](06-operation-commands/281-security-policy-database.md) |

## Management protocols, telemetry and third-party apps

<a id="management-protocols"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `app` | Third party app name | clear | oper+cand | [021-app.md](06-operation-commands/021-app.md) |
| `appctl` | Optional parameters to be passed in the command with max-elements 50 | appctl | oper+cand | [022-appctl.md](06-operation-commands/022-appctl.md) |
| `call-home` | The pre-configured name of the dial-out server | call-home | oper | [037-call-home.md](06-operation-commands/037-call-home.md) |
| `current-subscription` | Username in order to resolve paths according to user access | show | oper+cand | [068-current-subscription.md](06-operation-commands/068-current-subscription.md) |
| `data-model` | Allows to load/unload this data model | set/show | oper+cand | [070-data-model.md](06-operation-commands/070-data-model.md) |
| `dial-out-server` | Connection state to the dial-out-server | add/delete/set/show | oper+cand | [079-dial-out-server.md](06-operation-commands/079-dial-out-server.md) |
| `grpc` | Allows to configure the granularity of data in gNMI Get responses, when encoded with JSON. • per-path - puts... | set/show | oper+cand | [121-grpc.md](06-operation-commands/121-grpc.md) |
| `netconf` | List of YANG identifiers that are statically included in notifications | set/show | oper+cand | [188-netconf.md](06-operation-commands/188-netconf.md) |
| `restconf` | Root of the RESTCONF API | set/show | oper+cand | [266-restconf.md](06-operation-commands/266-restconf.md) |
| `snmp` | SNMP engine boot count | set/show | oper+cand | [297-snmp.md](06-operation-commands/297-snmp.md) |
| `snmp-community` | SNMP access right of this community string | add/set/show/delete | oper+cand | [298-snmp-community.md](06-operation-commands/298-snmp-community.md) |
| `snmp-target` | Type of transport for the SNMP target | add/set/show/delete | oper+cand | [299-snmp-target.md](06-operation-commands/299-snmp-target.md) |
| `snmpv3-user` | Specifies the SNMPv3 privacy pass phrase | add/set/show/delete | oper+cand | [300-snmpv3-user.md](06-operation-commands/300-snmpv3-user.md) |
| `subscription-path` | Boolean flag to control suppression of redundant telemetry updates to the collector platform | show | oper+cand | [316-subscription-path.md](06-operation-commands/316-subscription-path.md) |
| `subscriptions` | This command is used to show a list of subscriptions | show | oper+cand | [317-subscriptions.md](06-operation-commands/317-subscriptions.md) |
| `telemetry` | Persistent and dynamic telemetry | show/set | oper+cand | [344-telemetry.md](06-operation-commands/344-telemetry.md) |
| `third-party-app` | Third-party-app enabled state | set/show | oper+cand | [349-third-party-app.md](06-operation-commands/349-third-party-app.md) |

## Alarms, conditions and logging

<a id="fault-alarms-logging"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `alarm` | Timestamp when the alarm was last changed by operator | clear/show | oper | [014-alarm.md](06-operation-commands/014-alarm.md) |
| `alarm-control` | System -wide alarm-soaking-behavior switch:<br>• automatic: soaking time used is defined in FM profile.<br>•... | set/show | oper+cand | [015-alarm-control.md](06-operation-commands/015-alarm-control.md) |
| `alarm-inventory` | Information on whether this alarm is service affecting or not | show | oper+cand | [016-alarm-inventory.md](06-operation-commands/016-alarm-inventory.md) |
| `alarm-severity-entry` | Possible alarm service affecting category | set/show | oper+cand | [017-alarm-severity-entry.md](06-operation-commands/017-alarm-severity-entry.md) |
| `alarm-severity-profile` | The assigned severity of the profile | set/show | oper+cand | [018-alarm-severity-profile.md](06-operation-commands/018-alarm-severity-profile.md) |
| `current-alarms` | Timestamp of the last change in the current alarm list (either a raise or clear event) | show | oper+cand | [066-current-alarms.md](06-operation-commands/066-current-alarms.md) |
| `get-conditions` | Resource Access Identifier (AID) | get-conditions | oper | [118-get-conditions.md](06-operation-commands/118-get-conditions.md) |
| `log` | The name of the log file to have it's contents removed | clear/show | oper+cand* | [163-log.md](06-operation-commands/163-log.md) |
| `log-console` | Switches on and off the console logging | set/show | oper+cand | [164-log-console.md](06-operation-commands/164-log-console.md) |
| `log-console-facility-filter` | Describes the option to specify how the severity comparison is performed | add/set/show/delete | oper+cand | [165-log-console-facility-filter.md](06-operation-commands/165-log-console-facility-filter.md) |
| `log-file` | Whether the local file has logs include sensitive data | add/set/show/delete | oper+cand | [166-log-file.md](06-operation-commands/166-log-file.md) |
| `log-file-facility-filter` | Describes the option to specify how the severity comparison is performed | add/set/show/delete | oper+cand | [167-log-file-facility-filter.md](06-operation-commands/167-log-file-facility-filter.md) |
| `log-server` | Flag indicating if alarm the reporting is allowed | add/set/show/delete | oper+cand | [168-log-server.md](06-operation-commands/168-log-server.md) |
| `log-server-facility-filter` | Describes the option to specify how the severity comparison is performed | add/set/show/delete | oper+cand | [169-log-server-facility-filter.md](06-operation-commands/169-log-server-facility-filter.md) |
| `set-alarm-state` | Optional text that will be stored in the alarm | set-alarm-state | oper | [288-set-alarm-state.md](06-operation-commands/288-set-alarm-state.md) |
| `statistics` | Objects that will have their event counter statistics cleared | clear | oper | [311-statistics.md](06-operation-commands/311-statistics.md) |
| `syslog` | User defined label | set/show | oper+cand | [339-syslog.md](06-operation-commands/339-syslog.md) |

## Performance monitoring and statistics

<a id="performance-monitoring"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `pm` | Resource Access Identifier (AID) | clear/show | oper+cand* | [236-pm.md](06-operation-commands/236-pm.md) |
| `pm-catalog` | The catalog name | show | oper+cand | [237-pm-catalog.md](06-operation-commands/237-pm-catalog.md) |
| `pm-control` | Real-time data supervision for this resource | set/show | oper+cand | [238-pm-control.md](06-operation-commands/238-pm-control.md) |
| `pm-control-entry` | TCA supervision for this resource | set/show | oper+cand | [239-pm-control-entry.md](06-operation-commands/239-pm-control-entry.md) |
| `pm-parameter` | Type of PM parameter, it can be either a counter or a gauge | show | oper+cand | [240-pm-parameter.md](06-operation-commands/240-pm-parameter.md) |
| `pm-profile` | This parameter provides a way to globally enable PM data-supervision without having to toggle it... | set/show | oper+cand | [241-pm-profile.md](06-operation-commands/241-pm-profile.md) |
| `pm-profile-entry` | For newly created resources of this type, whether they have PM threshold crossing supervision automatically... | set/show | oper+cand | [242-pm-profile-entry.md](06-operation-commands/242-pm-profile-entry.md) |
| `pm-resource` | Date and time of the last real time data reset for this resource | set/show | oper+cand | [243-pm-resource.md](06-operation-commands/243-pm-resource.md) |
| `pm-threshold` | Configured high threshold value for resources that have this parameter | add/set/show/delete | oper+cand | [244-pm-threshold.md](06-operation-commands/244-pm-threshold.md) |
| `pm-threshold-profile` | Maximum value for this parameter | set/show | oper+cand | [245-pm-threshold-profile.md](06-operation-commands/245-pm-threshold-profile.md) |

## Software, firmware, file transfer and ZTP

<a id="software-firmware-files"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `activate` | Specific entity in the system for activating the loopback | activate | oper | [008-activate.md](06-operation-commands/008-activate.md) |
| `bootstrap` | Password for the new administrator account on the neighbor NE. Can be provided as a password hash ( format... | bootstrap | oper+cand | [032-bootstrap.md](06-operation-commands/032-bootstrap.md) |
| `cancel-upgrade` | Displays help for this command | cancel-upgrade | oper | [038-cancel-upgrade.md](06-operation-commands/038-cancel-upgrade.md) |
| `change-ztp-mode` | Enable or disable ztp | change-ztp-mode | oper | [046-change-ztp-mode.md](06-operation-commands/046-change-ztp-mode.md) |
| `current-fw` | Status for this particular firmware. current - Current firmware is up-to-date. not-current - Current firmware... | show | oper+cand | [067-current-fw.md](06-operation-commands/067-current-fw.md) |
| `download` | The password for the new-admin-user that is auto-configured after the database is wiped | download | oper | [086-download.md](06-operation-commands/086-download.md) |
| `downloaded-image` | Downloaded software image file signature | show | oper+cand | [087-downloaded-image.md](06-operation-commands/087-downloaded-image.md) |
| `downloads` | This command is used to show a list of downloads | show | oper+cand | [088-downloads.md](06-operation-commands/088-downloads.md) |
| `file` | Filepath of the file to be deleted | clear/file | oper | [108-file.md](06-operation-commands/108-file.md) |
| `file-operation` | The path to the file | file-operation | oper | [109-file-operation.md](06-operation-commands/109-file-operation.md) |
| `file-server` | User-defined label for the server | add/set/show/delete | oper+cand | [110-file-server.md](06-operation-commands/110-file-server.md) |
| `file-type` | Last transfer operation | show | oper+cand | [111-file-type.md](06-operation-commands/111-file-type.md) |
| `http-file-server` | The base URL used to redirect to the file transfer application | set/show | oper+cand | [124-http-file-server.md](06-operation-commands/124-http-file-server.md) |
| `manifest` | Included version of the firmware | show | oper+cand | [176-manifest.md](06-operation-commands/176-manifest.md) |
| `packaged-fw` | Included version of the firmware | show | oper+cand | [232-packaged-fw.md](06-operation-commands/232-packaged-fw.md) |
| `prepare-upgrade` | The password for the new-admin-user that is auto-configured after the database is wiped | prepare-upgrade | oper | [247-prepare-upgrade.md](06-operation-commands/247-prepare-upgrade.md) |
| `recover-mode` | Forces the command without confirmation | clear | oper | [260-recover-mode.md](06-operation-commands/260-recover-mode.md) |
| `software-load` | Software load package type | show | oper+cand | [301-software-load.md](06-operation-commands/301-software-load.md) |
| `software-location` | Location of the equipment | show | oper+cand | [302-software-location.md](06-operation-commands/302-software-location.md) |
| `subtype-constraint` | Subtype description | show | oper+cand | [318-subtype-constraint.md](06-operation-commands/318-subtype-constraint.md) |
| `sw-component` | Package information | show | oper+cand | [332-sw-component.md](06-operation-commands/332-sw-component.md) |
| `sw-container` | Time since the container started | show | oper+cand | [333-sw-container.md](06-operation-commands/333-sw-container.md) |
| `sw-control-rule` | The action to be taken. • default-action - performs the policy of restarting the service, then rebooting the... | add/set/show/delete | oper+cand | [334-sw-control-rule.md](06-operation-commands/334-sw-control-rule.md) |
| `sw-management` | Shows inactive software | show | oper+cand | [335-sw-management.md](06-operation-commands/335-sw-management.md) |
| `sw-service` | The number of times a service has restarted | show | oper+cand | [336-sw-service.md](06-operation-commands/336-sw-service.md) |
| `sw-subcomponent` | Package information | show | oper+cand | [337-sw-subcomponent.md](06-operation-commands/337-sw-subcomponent.md) |
| `swversion` | This command is used to retrieve the active, inactive and/or installable versions of the software present on... | swversion | oper+cand | [338-swversion.md](06-operation-commands/338-swversion.md) |
| `third-party-fw` | List of resources that this firmware can be applied to | show | oper+cand | [350-third-party-fw.md](06-operation-commands/350-third-party-fw.md) |
| `transfer` | List of keywords associated with optional content to be selected for debug-log upload | set/show | oper+cand | [356-transfer.md](06-operation-commands/356-transfer.md) |
| `transfer-status` | Details of transfer phase | show | - | [357-transfer-status.md](06-operation-commands/357-transfer-status.md) |
| `upgrade-status` | Details on the current upgrade | show | oper+cand | [363-upgrade-status.md](06-operation-commands/363-upgrade-status.md) |
| `upload` | X509v3 local/peer/trusted certificate name to be uploaded | upload | oper | [364-upload.md](06-operation-commands/364-upload.md) |
| `ztp` | Summarized completion status of ZTP on the node | show | oper+cand | [374-ztp.md](06-operation-commands/374-ztp.md) |

## Node-level system, time and status

<a id="system-node-time"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `clock` | Indicates last system time jump in the format '&lt;time1&gt; to &lt;time2&gt;'. Time jumps of less than 10... | set/show | oper+cand | [052-clock.md](06-operation-commands/052-clock.md) |
| `ne` | Controls the reporting of alarms for this particular object.<br>• allowed - Alarm reporting is allowed.<br>•... | set/show | oper+cand | [186-ne.md](06-operation-commands/186-ne.md) |
| `ne-function` | This command is used to show the Network Element (NE) function | show | oper+cand | [187-ne-function.md](06-operation-commands/187-ne-function.md) |
| `ntp` | The system contains manual and dhcp configured values | set/show | oper+cand | [195-ntp.md](06-operation-commands/195-ntp.md) |
| `ntp-key` | Indicates a trusted NTP key | add/set/show/delete | oper+cand | [196-ntp-key.md](06-operation-commands/196-ntp-key.md) |
| `ntp-server` | Controls the reporting of alarms for this particular object. allowed - Alarm reporting is allowed. inhibited... | add/set/show/delete | oper+cand | [197-ntp-server.md](06-operation-commands/197-ntp-server.md) |
| `ntp-server-status` | Condition of NTP server | show | oper+cand | [198-ntp-server-status.md](06-operation-commands/198-ntp-server-status.md) |
| `restart` | Card HW or SW sub-component to restart | restart | oper | [265-restart.md](06-operation-commands/265-restart.md) |
| `set-time` | Time to set in the system | set-time | oper | [289-set-time.md](06-operation-commands/289-set-time.md) |
| `status` | For some dashboards, allows to specify an AID filter, reducing the scope of the output | status | oper | [312-status.md](06-operation-commands/312-status.md) |
| `system` | The attribute of the object-id | show/set/clear | oper+cand* | [340-system.md](06-operation-commands/340-system.md) |
| `time` | This command is used to display the system's time | time | oper+cand | [351-time.md](06-operation-commands/351-time.md) |
| `uptime` | This command displays the system uptime and load average | uptime | oper+cand | [365-uptime.md](06-operation-commands/365-uptime.md) |
