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
| `add` | The `add` command is used to create a new managed entity | add | oper+cand | [010-add.md](06-operation-commands/010-add.md) |
| `alias` | The `alias` command is used to define a more user-friendly alphanumeric string for one or more commands,... | alias | oper | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `begin` | The `begin` command is used to display the output of the previous command starting from a specified word |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#51-begin) |
| `clear` | The clear command clears entries for the specified entity | clear | oper | [049-clear.md](06-operation-commands/049-clear.md) |
| `cli` | These commands are used to set or show the configuration of the Command Line Interface (CLI) management protocol | set/show | oper+cand | [050-cli.md](06-operation-commands/050-cli.md) |
| `cli-session-config` | These commands are used to set or show the configuration of the Command Line Interface (CLI) session attributes | set/show | oper+cand | [051-cli-session-config.md](06-operation-commands/051-cli-session-config.md) |
| `connect` | The `connect` command described in this section is used to establish a ssh session directly from CLI | connect | oper | [058-connect.md](06-operation-commands/058-connect.md) |
| `convert` | This command is used to convert a CLI command into a request for another northbound protocol | convert | oper+cand | [062-convert.md](06-operation-commands/062-convert.md) |
| `default` | This command can be used to assign default value(s) for the targeted entities | default | oper+cand | [075-default.md](06-operation-commands/075-default.md) |
| `delete` | The `delete` command is used to delete an existing managed entity from the database | delete | - | [077-delete.md](06-operation-commands/077-delete.md) |
| `display` | The `display` command is used to allows to customize the output of the previous command, i.e., to display... |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#52-display) |
| `edit` | The edit command is used to navigate the managed entity hierarchy | edit | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#42-edit) |
| `exclude` | The `exclude` command is used to filter the output that contains a defined word or string (i.e., does not... |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#53-exclude) |
| `exit` | This command is used to logout of the current CLI mode | exit | oper+cand | [100-exit.md](06-operation-commands/100-exit.md) |
| `expect` | This command is used to ensure that an attribute matches the expected value | expect | oper+cand | [101-expect.md](06-operation-commands/101-expect.md) |
| `export` | This command is used to define variables to use in CLI. Variables can be referenced with ${variable} in any... | export | oper+cand | [102-export.md](06-operation-commands/102-export.md) |
| `grep` | The `grep` command is used to filter the output based on a defined word or string (i.e., only displays output... |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#54-grep) |
| `gshell` | This command is used to launch a Linux bash shell inside a Guest Container from within the CLI. The shell can... | gshell | oper | [122-gshell.md](06-operation-commands/122-gshell.md) |
| `help` | Displays help for a command, container, or attribute. | help | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `highlight` | The `highlight` command is used to visually markup a word or set of words in the output of a given command |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#55-highlight) |
| `history` | The `history` command is used to display the current session's command history as a numbered list | history | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#43-history) |
| `include` | The `include` command is used to filter the output to a defined word or string (i.e., only displays output... |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#56-include) |
| `kill-session` | This command is used to close any established session, independently on the type of the session (CLI, NETCONF, etc) | kill-session | oper | [149-kill-session.md](06-operation-commands/149-kill-session.md) |
| `linenum` | The `linenum` command is used to add line numbers to output of the previous command |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#57-linenum) |
| `message` | This command is used to send a message to other CLI sessions | message | oper | [180-message.md](06-operation-commands/180-message.md) |
| `more` | The `more` command is used to display long outputs incrementally, page by page |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#58-more) |
| `property` | These commands are used to set or show a type specific property, auto instantiated by the system, but... | show/set | oper+cand | [249-property.md](06-operation-commands/249-property.md) |
| `run` | This command is used to execute a previously configured/defined/scheduled task or a script | run | oper+cand | [272-run.md](06-operation-commands/272-run.md) |
| `scheduled-task` | These commands are used to add/set or show a set of individual user-configurable scheduled commands | add/set/show/delete | oper+cand | [275-scheduled-task.md](06-operation-commands/275-scheduled-task.md) |
| `session` | This command is used to show the list of currently established management layer sessions | show | oper+cand | [286-session.md](06-operation-commands/286-session.md) |
| `set` | The `set` assigns values to the specified attributes | set | oper+cand | [287-set.md](06-operation-commands/287-set.md) |
| `shell` | This command is used to launch a Linux bash shell from within the CLI. The shell will be launched using the... | shell | oper | [290-shell.md](06-operation-commands/290-shell.md) |
| `show` | The `show` retrieves information from the system | show | oper+cand | [291-show.md](06-operation-commands/291-show.md) |
| `simulate` | This command is used to trigger simulated events in the system (alarms, equipment, etc). **Equipment... | simulate | oper | [293-simulate.md](06-operation-commands/293-simulate.md) |
| `sleep` | This command is used to specify a delay for a specified amount of time | sleep | oper+cand | [294-sleep.md](06-operation-commands/294-sleep.md) |
| `sort` | The `sort` command is used to reorder the output of a command according to specified criteria |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#59-sort) |
| `task` | These commands are used to add, set, show or delete a user configurable scheduled task | add/set/show/delete | oper+cand | [343-task.md](06-operation-commands/343-task.md) |
| `terminate` | This command is used to terminate a running operation. **Location led test Termination** By providing the... | terminate | oper | [348-terminate.md](06-operation-commands/348-terminate.md) |
| `tic` | Starts a timer for the typed command. | tic | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `toc` | Displays the elapsed time since the timer was started. | toc | oper+cand | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `top` | The `top` command is used to bring the current path to the top of the managed entity hierarchy [ne] | top | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#44-top) |
| `tree` | The `tree` command is used to display the managed entity hierarchy in a tree-like format | tree | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#45-tree) |
| `unalias` | The `unalias` command is used to remove an alias previously defined.. When using `unalias` command, take into... | unalias | oper | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `until` | The `until` command is used to display the output of the previous command ending at a specified word |  | - | [05-piped-commands.md](05-piped-commands/05-piped-commands.md#510-until) |
| `up` | The `up` command is used to bring the current path up by one path level in the managed entity hierarchy | up | oper+cand | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md#47-up) |
| `update` | This command is used to update a specific object attribute(s), with dependence on the provided <type> | update | oper | [362-update.md](06-operation-commands/362-update.md) |

## Candidate config, commit, database and templates

<a id="config-datastore"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `activate-snapshot` | This command is used to activate an available database snapshot | activate-snapshot | oper | [009-activate-snapshot.md](06-operation-commands/009-activate-snapshot.md) |
| `advanced-parameter` | The commands described in this section are used to add, configure, show, or delete advanced parameters | add/set/show/delete | oper+cand | [013-advanced-parameter.md](06-operation-commands/013-advanced-parameter.md) |
| `apply-template` | This command is used to apply templates of multiple types | apply-template | oper+cand | [023-apply-template.md](06-operation-commands/023-apply-template.md) |
| `commit` | This command is used to commit the contents of the candidate datastore | commit | cand | [055-commit.md](06-operation-commands/055-commit.md) |
| `config` | The `show config` displays the system's configuration | show | - | [056-config.md](06-operation-commands/056-config.md) |
| `configure` | This command is used to change to Candidate Configuration mode in order to edit a candidate datastore | configure | oper | [057-configure.md](06-operation-commands/057-configure.md) |
| `current-advanced-parameter` | This command is used to show the current values of the advanced parameters, which are running on the system | show | oper+cand | [065-current-advanced-parameter.md](06-operation-commands/065-current-advanced-parameter.md) |
| `database` | The `show database` command is used to show the list of the databases in the system | clear/show | oper+cand | [072-database.md](06-operation-commands/072-database.md) |
| `db-migrate` | The command described in this section is used to show the `db-migrate` attributes | db-migrate | oper | [073-db-migrate.md](06-operation-commands/073-db-migrate.md) |
| `db-protection-scheme` | The command described in this section is used to show the `db-protection-scheme` attributes | show | oper+cand | [074-db-protection-scheme.md](06-operation-commands/074-db-protection-scheme.md) |
| `diff` | This command is used to perform a diff comparison between a candidate configuration and the current system... | diff | oper+cand | [080-diff.md](06-operation-commands/080-diff.md) |
| `discard-changes` | This command will discard all candidate datastore content and CLI return to operational mode | discard-changes | cand | [082-discard-changes.md](06-operation-commands/082-discard-changes.md) |
| `extended-config` | The commands described in this section are used to add, delete or show the `extended-config` attributes | add/delete/show | oper+cand | [103-extended-config.md](06-operation-commands/103-extended-config.md) |
| `golden-advanced-parameter` | This command is used to show the `golden-advanced-parameter` attributes | show | oper+cand | [119-golden-advanced-parameter.md](06-operation-commands/119-golden-advanced-parameter.md) |
| `lock` | This command is used to lock the database access to the current session | lock | oper | [162-lock.md](06-operation-commands/162-lock.md) |
| `named-value-set` | These commands are used to add/set/show and delete the `named-value-set` attributes | add/set/delete/show | oper+cand | [184-named-value-set.md](06-operation-commands/184-named-value-set.md) |
| `recovery` | These commands are used configure and display the status of system recovery from chassis storage | set/show | oper+cand | [261-recovery.md](06-operation-commands/261-recovery.md) |
| `rollback` | The `rollback commit` must be executed using the commit parameter, and optionally a specific commit-id (if... | rollback | oper+cand | [268-rollback.md](06-operation-commands/268-rollback.md) |
| `show commit` | The `show commit` retrieves the commit record information from the system | show | oper+cand | [292-show-commit.md](06-operation-commands/292-show-commit.md) |
| `system-policies` | The commands described in this section are used to set or show the `system-policies` attributes.The commands... | set/show | oper+cand | [341-system-policies.md](06-operation-commands/341-system-policies.md) |
| `take-snapshot` | This command is used to create a local database snapshot | take-snapshot | oper | [342-take-snapshot.md](06-operation-commands/342-take-snapshot.md) |
| `template` | These commands are used to add, set, show and delete the template entry that is defined by an object and... | add/set/show/delete/apply-template | oper+cand | [345-template.md](06-operation-commands/345-template.md) |
| `template-group` | These commands are used to add and show the configuration that defines the data model for system template-group | add/show/delete | oper+cand | [346-template-group.md](06-operation-commands/346-template-group.md) |
| `templates` | This command is used to show the configuration that defines the data model for system templates | show | oper+cand | [347-templates.md](06-operation-commands/347-templates.md) |
| `unlock` | This command will release a previously locked database (achieved by using the 'lock' command) | unlock | oper | [360-unlock.md](06-operation-commands/360-unlock.md) |
| `validate` | This command is used to validate the contents of the specified configuration | validate | oper+cand | [370-validate.md](06-operation-commands/370-validate.md) |

## Equipment, cards, ports, pluggables and inventory

<a id="equipment-inventory"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `capabilities` | This command is used to retrieve information about a cards capabilities | show | oper+cand | [039-capabilities.md](06-operation-commands/039-capabilities.md) |
| `card` | These commands are used to add, edit, show or delete a card-base object | add/set/show/delete | oper+cand | [040-card.md](06-operation-commands/040-card.md) |
| `chassis` | These commands are used to add, delete, edit or show the chassis attributes | add/delete/set/show | oper+cand | [047-chassis.md](06-operation-commands/047-chassis.md) |
| `console` | These commands are used to set or show console attributes | set/show | oper+cand | [060-console.md](06-operation-commands/060-console.md) |
| `controller-card` | This command is used to display the configuration of a controller card | show | oper+cand | [061-controller-card.md](06-operation-commands/061-controller-card.md) |
| `equipment` | This command is used to display installed equipment information | show | oper+cand | [092-equipment.md](06-operation-commands/092-equipment.md) |
| `equipment-policies` | These commands are used to set or show the equipment policies attributes | set/show | oper+cand | [093-equipment-policies.md](06-operation-commands/093-equipment-policies.md) |
| `equipment-templates` | These commands are used to enable and view the serdes templates setting associated with equipment | set/show | oper+cand | [094-equipment-templates.md](06-operation-commands/094-equipment-templates.md) |
| `fru-info` | This command is used to display the packaged FRU information associated to a particular equipment-type | show | oper+cand | [114-fru-info.md](06-operation-commands/114-fru-info.md) |
| `inventory` | These commands are used to show the inventory data for a present FRU | show | oper+cand | [137-inventory.md](06-operation-commands/137-inventory.md) |
| `led` | These commands are used to show the representation of a LED in a FRU. Object exists even if FRU is not... | show | oper+cand | [152-led.md](06-operation-commands/152-led.md) |
| `port` | These commands is used are set/show port attributes | set/show | oper+cand | [246-port.md](06-operation-commands/246-port.md) |
| `resources` | This command is used to show system or card resources | show | oper+cand | [264-resources.md](06-operation-commands/264-resources.md) |
| `serdes` | These commands are used to add, edit or show serdes | add/set/show/delete | oper+cand | [282-serdes.md](06-operation-commands/282-serdes.md) |
| `serdes-template` | This command is used to auto-configure serdes for 3rd party TOMs. serdes-templates are created by the user... | add/set/delete/show | oper+cand | [283-serdes-template.md](06-operation-commands/283-serdes-template.md) |
| `serdes-template-entry` | These commands are used to enter an individual entry to the serdes-template | add/set/delete/show | oper+cand | [284-serdes-template-entry.md](06-operation-commands/284-serdes-template-entry.md) |
| `serial-console` | These commands are used to set or show the global configuration of all serial console ports in the system | set/show | oper+cand | [285-serial-console.md](06-operation-commands/285-serial-console.md) |
| `slot` | These commands are used to show the slot equipment holder details | show | oper+cand | [295-slot.md](06-operation-commands/295-slot.md) |
| `sub-component` | This command is used to show the sub-component details or card resources | show | oper+cand | [314-sub-component.md](06-operation-commands/314-sub-component.md) |
| `supported-card` | This command is used to show the capability information for supported card | show | oper+cand | [321-supported-card.md](06-operation-commands/321-supported-card.md) |
| `supported-chassis` | This command is used to show the capability information for supported chassis | show | oper+cand | [323-supported-chassis.md](06-operation-commands/323-supported-chassis.md) |
| `supported-port` | This command is used to display the capabilities for each port in each supported card | show | oper+cand | [325-supported-port.md](06-operation-commands/325-supported-port.md) |
| `supported-slot` | This command is used to show the capability for each slot within each supported chassis | show | oper+cand | [327-supported-slot.md](06-operation-commands/327-supported-slot.md) |
| `supported-tom` | This command is used to display the capability information for supported TOM (Tunable/non-tunable Optical... | show | oper+cand | [328-supported-tom.md](06-operation-commands/328-supported-tom.md) |
| `supported-tom-power` | The command described in this section is used to show `supported-tom-power` attributes | show | oper+cand | [329-supported-tom-power.md](06-operation-commands/329-supported-tom-power.md) |
| `tom` | These commands are used to add, set, show or delete a TOM (Tunable/non-tunable Optical Module) pluggable | add/set/show/delete | oper+cand | [352-tom.md](06-operation-commands/352-tom.md) |
| `tom-type` | This command is used to show the capabilities of the supported TOM (Tunable/non-tunable Optical Module)... | show | oper+cand | [353-tom-type.md](06-operation-commands/353-tom-type.md) |
| `unprovisioned-inventory` | This command is used to show a .ist of detected inventory but not yet accepted by the Node Controller in... | show | oper+cand | [361-unprovisioned-inventory.md](06-operation-commands/361-unprovisioned-inventory.md) |
| `usb` | This command shows the USB function attributes of the port | show | oper+cand | [366-usb.md](06-operation-commands/366-usb.md) |

## Protection and switchover

<a id="protection-redundancy"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `manual-switchover` | This command is used to perform a manual switchover | manual-switchover | oper | [177-manual-switchover.md](06-operation-commands/177-manual-switchover.md) |
| `protection` | This command is used to show protection | show | oper+cand | [250-protection.md](06-operation-commands/250-protection.md) |
| `protection-group` | These commands are used to add, set and show a protection group | add/set/show/delete | oper+cand | [251-protection-group.md](06-operation-commands/251-protection-group.md) |
| `protection-switch` | This is the operating command for protection group switching | protection-switch | oper | [252-protection-switch.md](06-operation-commands/252-protection-switch.md) |
| `protection-unit` | These commands are used to set or show a protection unit | set/show | oper+cand | [253-protection-unit.md](06-operation-commands/253-protection-unit.md) |

## Layer 0 photonics: spectrum, amplifiers, degrees, OTDR

<a id="optical-layer0"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `adg` | These commands are used to add, delete an Add/Drop Group (ADG) and to set or show the ADG attributes | set/show/delete | oper+cand | [012-adg.md](06-operation-commands/012-adg.md) |
| `amplifier` | These commands are used to set or show the amplifier object attributes | set/show | oper+cand | [019-amplifier.md](06-operation-commands/019-amplifier.md) |
| `amplifier-raman` | These commands are used to set or show the amplifier object attributes | set/show | oper+cand | [020-amplifier-raman.md](06-operation-commands/020-amplifier-raman.md) |
| `ase-idler-service` | The commands described in this section are used to add/delete `ase-idler-service` or set/show the... | add/delete/set/show | oper+cand | [024-ase-idler-service.md](06-operation-commands/024-ase-idler-service.md) |
| `ase-idler-source` | The commands described in this section are used to set or show the `ase-idler-source` attributes | set/show | oper+cand | [025-ase-idler-source.md](06-operation-commands/025-ase-idler-source.md) |
| `calibrate` | The command described in this section is used to calibrate the Raman gain | calibrate | oper | [036-calibrate.md](06-operation-commands/036-calibrate.md) |
| `degree` | These commands are used to add, delete a degree and to set or show the degree attributes | add/delete/set/show | oper+cand | [076-degree.md](06-operation-commands/076-degree.md) |
| `direction` | These commands are used to add/edit or show the directions on a multi-rail ILA node | add/delete/set/show | oper+cand | [081-direction.md](06-operation-commands/081-direction.md) |
| `dsc` | The commands described in this section are used to add, delete, set or show the `dsc` attributes | add/delete/set/show | oper+cand | [089-dsc.md](06-operation-commands/089-dsc.md) |
| `dsc-group` | The commands described in this section are used to add, delete, set or show the `dsc-group` attributes | add/delete/set/show | oper+cand | [090-dsc-group.md](06-operation-commands/090-dsc-group.md) |
| `gadt` | This command is used to retrieve information about golden carrier application information  | show | oper+cand | [115-gadt.md](06-operation-commands/115-gadt.md) |
| `gapt` | This command is used to list the golden advanced parameters from the Golden Advanced Parameters Table (GAPT) | show | oper+cand | [116-gapt.md](06-operation-commands/116-gapt.md) |
| `gcmt` | This command is used to retrieve information about the golden carrier mode | show | oper+cand | [117-gcmt.md](06-operation-commands/117-gcmt.md) |
| `golden-carrier-mode` | This command is used to retrieve configuration information from the system | show | oper+cand | [120-golden-carrier-mode.md](06-operation-commands/120-golden-carrier-mode.md) |
| `l0-capabilities` | The command described in this section is used to show the capabilities details related with node's Layer 0... | show | oper+cand | [151-l0-capabilities.md](06-operation-commands/151-l0-capabilities.md) |
| `mc` | These commands are used to add, delete the Media Channel (MC), and set or show the MC facility attributes | add/set/show/delete | oper+cand | [178-mc.md](06-operation-commands/178-mc.md) |
| `mc-f` | This command is used to show the Media Channel Filler (NMC-F) facility attributes | show | oper+cand | [179-mc-f.md](06-operation-commands/179-mc-f.md) |
| `modules-adg` | These commands are used to add, delete modules to an ADG and to set or show the object attributes | add/set/show/delete | oper+cand | [181-modules-adg.md](06-operation-commands/181-modules-adg.md) |
| `modules-degree` | These commands are used to add, delete modules to a degree and to set or show the object attributes | add/set/show/delete | oper+cand | [182-modules-degree.md](06-operation-commands/182-modules-degree.md) |
| `monitored-channel` | The command described in this section is used to show the **monitored-channel** attributes | show | oper+cand | [183-monitored-channel.md](06-operation-commands/183-monitored-channel.md) |
| `nmc` | These commands are used to add, delete the Network Media Channel (NMC), and set or show the NMC facility attributes | add/set/show/delete | oper+cand | [193-nmc.md](06-operation-commands/193-nmc.md) |
| `nmc-f` | This command is used to show the Network Media Channel Filler (NMC-F) facility attributes | set/show | oper+cand | [194-nmc-f.md](06-operation-commands/194-nmc-f.md) |
| `oadm-capabilities` | This command is used to show OADM capabilities | show | oper+cand | [200-oadm-capabilities.md](06-operation-commands/200-oadm-capabilities.md) |
| `oc` | This command is used to enable, edit or show the attributes of an optical carrier | set/show | oper+cand | [201-oc.md](06-operation-commands/201-oc.md) |
| `ochm` | The commands described in this section are used to set or show the `ochm` (optical channel monitoring) attributes | set/show | oper+cand | [202-ochm.md](06-operation-commands/202-ochm.md) |
| `ocm-channel` | The commands described in this section are used to set or show the `ocm-channel` attributes | show | oper+cand | [203-ocm-channel.md](06-operation-commands/203-ocm-channel.md) |
| `ocm-mp` | The commands described in this section are used to set or show the `ocm-mp` attributes | set/show | oper+cand | [204-ocm-mp.md](06-operation-commands/204-ocm-mp.md) |
| `ocm-ptp` | The commands described in this section are used to set or show the `ocm-ptp` attributes | set/show | oper+cand | [205-ocm-ptp.md](06-operation-commands/205-ocm-ptp.md) |
| `oms` | These commands are used to set or show the Optical Multiplex Section (OMS) facility attributes | set/show | oper+cand | [209-oms.md](06-operation-commands/209-oms.md) |
| `ops` | These commands are used to set or show the Optical Physical Section (OPS) facility attributes | set/show | oper+cand | [210-ops.md](06-operation-commands/210-ops.md) |
| `optical-carrier` | These commands are used to add, edit and show the attributes of an optical carrier | set/show | oper+cand | [211-optical-carrier.md](06-operation-commands/211-optical-carrier.md) |
| `optical-channel` | These commands are used to edit, and show optical channel attributes | set/show | oper+cand | [212-optical-channel.md](06-operation-commands/212-optical-channel.md) |
| `optical-ptp` | This command is used to edit, or show an optical ptp attributes | set/show | oper+cand | [213-optical-ptp.md](06-operation-commands/213-optical-ptp.md) |
| `optical-switch` | The commands described in this section are used to set or show the `optical-switch` attributes | set/show | oper+cand | [214-optical-switch.md](06-operation-commands/214-optical-switch.md) |
| `osc` | These commands are used to set or show the Optical Supervisory Channel (OSC) facility attributes | set/show | oper+cand | [215-osc.md](06-operation-commands/215-osc.md) |
| `otdr` | The commands described in this section are used to add, delete, set or show the OTDR function | add/delete/set/show | oper+cand | [223-otdr.md](06-operation-commands/223-otdr.md) |
| `otdr-ptp` | These commands are used to add, delete set or show the OTDR ptp | add/delete/set/show | oper+cand | [224-otdr-ptp.md](06-operation-commands/224-otdr-ptp.md) |
| `ots` | These commands are used to set or show the Optical Transport Section (OTS) facility attributes | set/show | oper+cand | [225-ots.md](06-operation-commands/225-ots.md) |
| `ots-diagnostics` | This command is used to set or show the attributes associated with OTS diagnostics | set/show | oper+cand | [226-ots-diagnostics.md](06-operation-commands/226-ots-diagnostics.md) |
| `ots-r` | These commands are used to enable, add, set or show the attributes associated with Optical Transport Section... | set/show | oper+cand | [227-ots-r.md](06-operation-commands/227-ots-r.md) |
| `ots-r-auto-otdr` | The commands described in this section are used to add or delete automatic OTDR `ots-r-auto-otdr` entity on... | add/delete/set/show | oper+cand | [228-ots-r-auto-otdr.md](06-operation-commands/228-ots-r-auto-otdr.md) |
| `oxcon` | These commands are used to add, delete the Optical Cross Connection (OXcon), and set or show the OXcon attributes | add/set/show/delete | oper+cand | [231-oxcon.md](06-operation-commands/231-oxcon.md) |
| `profile-control` | The `profile-control` command allows the user to read or write per-slice power or attenuation profiles... | profile-control | oper | [248-profile-control.md](06-operation-commands/248-profile-control.md) |
| `pump` | These commands are used set up a pump | set/show | oper+cand | [255-pump.md](06-operation-commands/255-pump.md) |
| `pump-power` | These commands are used to set up a Raman pump | set/show | oper+cand | [256-pump-power.md](06-operation-commands/256-pump-power.md) |
| `raman-calibration` | The commands described in this section are used to add, delete, set or show the `raman-calibration` attributes | add/delete/set/show | oper+cand | [257-raman-calibration.md](06-operation-commands/257-raman-calibration.md) |
| `rsc` | These commands are used to set attributes for or show an RSC, Raman card Pilot Tone facility | set/show | oper+cand | [271-rsc.md](06-operation-commands/271-rsc.md) |
| `spectrum` | The commands described in this section are used to set or show the spectrum facility attributes | set/show | oper+cand | [303-spectrum.md](06-operation-commands/303-spectrum.md) |
| `spectrum-control` | The commands described in this section are used to set or show the `spectrum-control` object attributes | add/delete/set/show | oper+cand | [304-spectrum-control.md](06-operation-commands/304-spectrum-control.md) |
| `spectrum-monitoring` | The command described in this section are used to show the `spectrum-monitoring` attributes | show | oper+cand | [305-spectrum-monitoring.md](06-operation-commands/305-spectrum-monitoring.md) |
| `super-channel` | This command is used to display Super Channel configuration attributes | add/show/delete | oper+cand | [319-super-channel.md](06-operation-commands/319-super-channel.md) |
| `super-channel-group` | This command is used to add, set or show super-channel-group attributes | add/set/show | oper+cand | [320-super-channel-group.md](06-operation-commands/320-super-channel-group.md) |
| `supported-carrier-mode` | This command is used to display a list of supported carrier modes | show | oper+cand | [322-supported-carrier-mode.md](06-operation-commands/322-supported-carrier-mode.md) |
| `supported-gain-range` | This command is used to display the supported gain range | show | oper+cand | [324-supported-gain-range.md](06-operation-commands/324-supported-gain-range.md) |
| `supported-power-profile` | This command is used to show the supported power-profile attributes for the specified card-type | show | oper+cand | [326-supported-power-profile.md](06-operation-commands/326-supported-power-profile.md) |

## Layer 1 transport: OTN, Ethernet and client facilities

<a id="transport-layer1"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `bert` | The commands described in this section are used to start/ stop/get/delete the attributes associated with the... | bert | oper | [028-bert.md](06-operation-commands/028-bert.md) |
| `cid-ptp` | The commands described in this section are used to manage `cid-ptp` facility and its attributes | set/show | oper+cand | [048-cid-ptp.md](06-operation-commands/048-cid-ptp.md) |
| `eth-zr` | These commands are used to add/edit/show/delete an Ethernet ZR facility | add/set/show/delete | oper+cand | [098-eth-zr.md](06-operation-commands/098-eth-zr.md) |
| `ethernet` | These commands are used to set/show ethernet facility attributes | set/show | oper+cand | [099-ethernet.md](06-operation-commands/099-ethernet.md) |
| `facilities` | This command is used to show system facilities | show | oper+cand | [105-facilities.md](06-operation-commands/105-facilities.md) |
| `fc` | The commands described in this section are used to set or show the `fc` attributes | set/show | oper+cand | [106-fc.md](06-operation-commands/106-fc.md) |
| `flexo` | The commands described in this section are used to set or show the `flexo` attributes | set/show | oper+cand | [112-flexo.md](06-operation-commands/112-flexo.md) |
| `flexo-group` | These commands are used to add/set/show/delete a flexo-group |  | oper+cand | [113-flexo-group.md](06-operation-commands/113-flexo-group.md) |
| `high-speed-monitoring` | The commands described in this section are used to set or show the `high-speed-monitoring` attributes | set/show | oper+cand | [123-high-speed-monitoring.md](06-operation-commands/123-high-speed-monitoring.md) |
| `interlaken` | The commands described in this section are used to set or show the SPN2 `interlaken` attributes | set/show | oper+cand | [136-interlaken.md](06-operation-commands/136-interlaken.md) |
| `L2-bridge` | The commands described in this section are used to set or show the `L2-bridge` attributes | set/show | oper+cand | [170-l2-bridge.md](06-operation-commands/170-l2-bridge.md) |
| `line-ptp` | These commands are used to add/set/show/delete a line ptp | add/set/show/delete | oper+cand | [153-line-ptp.md](06-operation-commands/153-line-ptp.md) |
| `network-xconnect` | This command is used to show the list of services of multiple user cross connections commissioned in this NE | show | oper+cand | [189-network-xconnect.md](06-operation-commands/189-network-xconnect.md) |
| `nw-xconnect` | The commands described in this section are used to add, set or show the `nw-xconnect` attributes | add/delete/set/show | oper+cand | [199-nw-xconnect.md](06-operation-commands/199-nw-xconnect.md) |
| `odu` | These commands are used to add, set, show an ODU facility | add/set/show/delete | oper+cand | [207-odu.md](06-operation-commands/207-odu.md) |
| `odu-diagnostics` | These commands are used to add, set, show or delete a set of attributes associated with ODU diagnostics | add/set/show/delete | oper+cand | [208-odu-diagnostics.md](06-operation-commands/208-odu-diagnostics.md) |
| `otu` | These commands are used to add, edit or show an OTU. The delete command is used to remove an OTU from the... | add/set/show/delete | oper+cand | [229-otu.md](06-operation-commands/229-otu.md) |
| `otu-diagnostics` | These commands are used to set or show the attributes associated with OTU diagnostics | set/show | oper+cand | [230-otu-diagnostics.md](06-operation-commands/230-otu-diagnostics.md) |
| `stm` | This command is used to set or show STM attributes | set/show | oper+cand | [313-stm.md](06-operation-commands/313-stm.md) |
| `trib-ptp` | These commands are used to set or show configuration of the tributary client physical termination layer which... | add/set/show/delete | oper+cand | [358-trib-ptp.md](06-operation-commands/358-trib-ptp.md) |
| `xcon` | These commands are used to add, edit or show Layer 1 digital services that are currently provisioned in the system | add/set/show/delete | oper+cand | [373-xcon.md](06-operation-commands/373-xcon.md) |

## Topology, fiber connections and neighbor discovery

<a id="topology-discovery"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `cable-id` | The commands described in this section are used to show the `cable-id` entities and terminate a CableID... | show/terminate | oper+cand* | [033-cable-id.md](06-operation-commands/033-cable-id.md) |
| `cable-id-path` | The commands described in this section are used to show the `cable-id-path` attributes | show | oper+cand | [034-cable-id-path.md](06-operation-commands/034-cable-id-path.md) |
| `cable-id-status` | The command described in this section is used to show the `cable-id-status` attributes | show | oper+cand | [035-cable-id-status.md](06-operation-commands/035-cable-id-status.md) |
| `carrier-neighbor` | This command is used to show a Local carrier instance that has discovered this neighbor node | show | oper+cand | [041-carrier-neighbor.md](06-operation-commands/041-carrier-neighbor.md) |
| `connection-ports` | This command is used to show connection ports | show | oper+cand | [059-connection-ports.md](06-operation-commands/059-connection-ports.md) |
| `custom-tlv` | This command is used to show a list of Organizational Specific TLVs (Type-Lengh-Value) parameters information | show | oper+cand | [069-custom-tlv.md](06-operation-commands/069-custom-tlv.md) |
| `external-fiber-connection` | These commands are used to add, set, show or delete an external fiber connection | add/set/show/delete | oper+cand | [104-external-fiber-connection.md](06-operation-commands/104-external-fiber-connection.md) |
| `fiber-connection` | These commands are used to add, set, show or delete a fiber-connection in an OADM/ILA topology | add/set/show/delete | oper+cand | [107-fiber-connection.md](06-operation-commands/107-fiber-connection.md) |
| `icdp` | These commands are used to set or show Nokia Carrier Discovery Protocol | set/show | oper+cand | [125-icdp.md](06-operation-commands/125-icdp.md) |
| `inci` | These commands are used to edit or show INCI which is Inter-NE Communication Interface information related to... | set/show | oper+cand | [132-inci.md](06-operation-commands/132-inci.md) |
| `inci-neighbor` | These commands are used to add, edit or show an INCI which is Inter-NE Communication Interface neighbor | add/set/show/delete | oper+cand | [133-inci-neighbor.md](06-operation-commands/133-inci-neighbor.md) |
| `interface-neighbor` | The commands described in this section are used to set or show the `interface-neighbor` attributes | set/show | oper+cand | [135-interface-neighbor.md](06-operation-commands/135-interface-neighbor.md) |
| `links` | This command is used to show the links container within the topology | show | oper+cand | [154-links.md](06-operation-commands/154-links.md) |
| `lldp` | These commands are used to set or show the LLDP hold on timer | set/show | oper+cand | [155-lldp.md](06-operation-commands/155-lldp.md) |
| `lldp-local-info` | This command is used to show the LLDP local system information sent on lldp-port | show | oper+cand | [156-lldp-local-info.md](06-operation-commands/156-lldp-local-info.md) |
| `lldp-neighbor` | This command is used to show the LLDP remote system discovered by lldp-port | show | oper+cand | [157-lldp-neighbor.md](06-operation-commands/157-lldp-neighbor.md) |
| `lldp-port-statistics` | This command is used to show LLDP frame reception statistics for a particular port and direction | show | oper+cand | [158-lldp-port-statistics.md](06-operation-commands/158-lldp-port-statistics.md) |
| `nct-connection` | This command is used to show NCT connectivity information, providing existing links between NCT ports in a... | show | oper+cand | [185-nct-connection.md](06-operation-commands/185-nct-connection.md) |
| `sndp` | The commands described in this section are used to set or show the `sndp` attributes | set/show | oper+cand | [296-sndp.md](06-operation-commands/296-sndp.md) |
| `submarine-link` | The commands described in this section are used to add or delete `submarine-link` object and set or show the... | add/delete/set/show | oper+cand | [315-submarine-link.md](06-operation-commands/315-submarine-link.md) |
| `supporting-fiber-connection` | The commands described in this section are used to show the list of fiber connections | show | oper+cand | [330-supporting-fiber-connection.md](06-operation-commands/330-supporting-fiber-connection.md) |
| `topology` | The `clear topology` command, manually removes existing topology neighbor information | clear/show | oper+cand* | [354-topology.md](06-operation-commands/354-topology.md) |
| `verify` | The command verify is used to trigger CableID-based fiber connections verification. **Fiber Connection... | verify | oper | [371-verify.md](06-operation-commands/371-verify.md) |

## IP interfaces, routing protocols and DCN

<a id="ip-networking"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `bgp-instance` | This command is used to add/edit/show a bgp instance | add/delete/set/show | oper+cand | [029-bgp-instance.md](06-operation-commands/029-bgp-instance.md) |
| `bgp-neighbor` | This command is used to add/edit/show a BGP neighbor | add/delete/set/show | oper+cand | [030-bgp-neighbor.md](06-operation-commands/030-bgp-neighbor.md) |
| `bgp-network` | This command is used to add/edit/show a bgp network | add/delete/show | oper+cand | [031-bgp-network.md](06-operation-commands/031-bgp-network.md) |
| `comm-channel` | These commands are used to add, set or show communications channel attributes | add/delete/set/show | oper+cand | [053-comm-channel.md](06-operation-commands/053-comm-channel.md) |
| `comm-eth` | These commands are used to set or show the communication Ethernet port attributes | set/show | oper+cand | [054-comm-eth.md](06-operation-commands/054-comm-eth.md) |
| `dhcp-relay` | These commands allow to edit or view the dhcp relay mode and server address | set/show | oper+cand | [078-dhcp-relay.md](06-operation-commands/078-dhcp-relay.md) |
| `dns` | These commands are used to edit or show the domain name service instance | set/show | oper+cand | [084-dns.md](06-operation-commands/084-dns.md) |
| `dns-server` | These commands are used to add, edit or show a Domain Name Server (DNS) server in the configuration | add/delete/set/show | oper+cand | [085-dns-server.md](06-operation-commands/085-dns-server.md) |
| `if-dhcp-relay` | The commands described in this section are used to set or show the `if-dhcp-relay` attributes | set/show | oper+cand | [126-if-dhcp-relay.md](06-operation-commands/126-if-dhcp-relay.md) |
| `interface` | These commands are used to add/set/show/delete an interface and related attributes | add/set/show/delete | oper+cand | [134-interface.md](06-operation-commands/134-interface.md) |
| `ip-monitoring` | These commands are used to add, edit or show Monitoring instance configuration and state | add/set/show/delete | oper+cand | [138-ip-monitoring.md](06-operation-commands/138-ip-monitoring.md) |
| `ipv4-address` | These commands are used to add/show/delete an IPv4 address on the interface | add/show/delete | oper+cand | [143-ipv4-address.md](06-operation-commands/143-ipv4-address.md) |
| `ipv4-static-route` | These commands are used to add/show/delete a list of IPv4 static routes to the interface | add/set/show/delete | oper+cand | [144-ipv4-static-route.md](06-operation-commands/144-ipv4-static-route.md) |
| `ipv6-address` | These commands are used to add/show/delete an IPv6 address to the interface | add/show/delete | oper+cand | [145-ipv6-address.md](06-operation-commands/145-ipv6-address.md) |
| `ipv6-static-route` | These commands are used to add/show/delete a list of static routes to the interface | add/set/show/delete | oper+cand | [146-ipv6-static-route.md](06-operation-commands/146-ipv6-static-route.md) |
| `management-address` | This command is used to retrieve management address information about a particular chassis component | show | oper+cand | [174-management-address.md](06-operation-commands/174-management-address.md) |
| `management-address-local` | This command is used to retrieve management address information about a particular chassis component | show | oper+cand | [175-management-address-local.md](06-operation-commands/175-management-address-local.md) |
| `networking` | These commands are used to show/set networking information | set/show | oper+cand | [190-networking.md](06-operation-commands/190-networking.md) |
| `networking-services` | This command is used to show the list of network services | show | oper+cand | [191-networking-services.md](06-operation-commands/191-networking-services.md) |
| `next-hop` | This command is used to show the next hop in a route | show | oper+cand | [192-next-hop.md](06-operation-commands/192-next-hop.md) |
| `ospf` | The `clear ospf` command is used to remove and restart an ospf-instance | clear | oper | [216-ospf.md](06-operation-commands/216-ospf.md) |
| `ospf-area` | These commands are used to add, set, show or delete an OSPF protocol area | add/set/show/delete | oper+cand | [217-ospf-area.md](06-operation-commands/217-ospf-area.md) |
| `ospf-area-range` | These commands are used to add, set, show or delete an OSPF area range instance | add/set/show/delete | oper+cand | [218-ospf-area-range.md](06-operation-commands/218-ospf-area-range.md) |
| `ospf-instance` | These commands are used to add, set, show and delete an OSPF protocol instance | add/set/show/delete | oper+cand | [219-ospf-instance.md](06-operation-commands/219-ospf-instance.md) |
| `ospf-interface` | These commands are used to add, set, show or delete an OSPF interface | add/set/show/delete | oper+cand | [220-ospf-interface.md](06-operation-commands/220-ospf-interface.md) |
| `ospf-neighbor` | The command described in this section is used to show the `ospf-neighbor` attributes | show | oper | [221-ospf-neighbor.md](06-operation-commands/221-ospf-neighbor.md) |
| `ospfv3-ipsec-security-association` | This command is used to add/set/show an OSPF version 3 security association | add/set/show/delete | oper+cand | [222-ospfv3-ipsec-security-association.md](06-operation-commands/222-ospfv3-ipsec-security-association.md) |
| `ping` | This command sends an echo message to another TCP/IP node to determine if the node is visible on the network.... | ping | oper+cand | [235-ping.md](06-operation-commands/235-ping.md) |
| `protocols` | This command is used to show protocol information | show | oper+cand | [254-protocols.md](06-operation-commands/254-protocols.md) |
| `rib` | This command is used to show RIB entries | show | oper+cand | [267-rib.md](06-operation-commands/267-rib.md) |
| `route` | This command is used to show the list of system routes from various sources, such as dynamic protocols and... | show | oper+cand | [269-route.md](06-operation-commands/269-route.md) |
| `routing` | This command is used to show routing information | show | oper+cand | [270-routing.md](06-operation-commands/270-routing.md) |
| `supporting-interface` | This command is used to show supporting interface information | show | oper+cand | [331-supporting-interface.md](06-operation-commands/331-supporting-interface.md) |
| `traceroute` | This command is used to track the route packets taken from an IP network on their way to a given host | traceroute | oper+cand | [355-traceroute.md](06-operation-commands/355-traceroute.md) |
| `vrf` | This command shows the Virtual Routing and Forwarding (VRF) instance | show | oper+cand | [372-vrf.md](06-operation-commands/372-vrf.md) |

## Users, AAA and access control

<a id="security-access-control"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `aaa-server` | This command is used to add/edit/show an AAA server | add/set/show/delete | oper+cand | [001-aaa-server.md](06-operation-commands/001-aaa-server.md) |
| `aaa-statistics` | This command can be used to view the AAA statistics for AAA servers that use the TACACS+ protocol | show | oper+cand | [002-aaa-statistics.md](06-operation-commands/002-aaa-statistics.md) |
| `access-control-list` | This command is used to show access control list | show | oper+cand | [003-access-control-list.md](06-operation-commands/003-access-control-list.md) |
| `access-rule` | The commands described in this section are used to add, set or show the `access-rule` attributes | add/delete/set/show | oper+cand | [004-access-rule.md](06-operation-commands/004-access-rule.md) |
| `access-rule-list` | The commands described in this section are used to add, set or show the `access-rule-list` attributes | add/delete/set/show | oper+cand | [005-access-rule-list.md](06-operation-commands/005-access-rule-list.md) |
| `ace` | This command is used to add/set attributes associated with every access control entry (ACE) | add/set/show/delete | oper+cand | [006-ace.md](06-operation-commands/006-ace.md) |
| `acl` | These commands are used to add/delete an access control list (ACL) and set/show attributes associated with... | add/set/show/delete | oper+cand | [007-acl.md](06-operation-commands/007-acl.md) |
| `auth-key` | This command is used to add, edit or show a authorization key | add/set/show/delete | oper+cand | [026-auth-key.md](06-operation-commands/026-auth-key.md) |
| `authorization` | The commands described in this section are used to set or show the `authorization` attributes | set/show | oper+cand | [027-authorization.md](06-operation-commands/027-authorization.md) |
| `password` | This command allows a user to change its own password in an interactive way. **Changing own password** Every... | password | oper | [233-password.md](06-operation-commands/233-password.md) |
| `security` | The command described in this section is used to show the top level security container | show | oper+cand | [279-security.md](06-operation-commands/279-security.md) |
| `security-policies` | The commands described in this section are used to edit or show security-policies | set/show | oper+cand | [280-security-policies.md](06-operation-commands/280-security-policies.md) |
| `user` | These commands are used to add, set, show or delete users and attributes | add/set/show/delete | oper+cand | [367-user.md](06-operation-commands/367-user.md) |
| `user-data` | The commands described in this section are used to show the `user-data` | show | oper+cand | [368-user-data.md](06-operation-commands/368-user-data.md) |
| `user-group` | These commands are used to add, set or show user groups and attributes | add/delete/set/show | oper+cand | [369-user-group.md](06-operation-commands/369-user-group.md) |

## Certificates, PKI and SSH keys

<a id="certificates-pki"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `cdp` | This command is used to manage manually configured CRL Distribution Points (CDPs) | add/set/show/delete | oper+cand | [042-cdp.md](06-operation-commands/042-cdp.md) |
| `cert-gen` | This command is used to generate a self-signed certificate | cert-gen | oper | [043-cert-gen.md](06-operation-commands/043-cert-gen.md) |
| `cert-to-name` | This command defines a prioritized set of rules used to map an X.509 client certificate to a specific user identity | add/delete/set/show | oper+cand | [044-cert-to-name.md](06-operation-commands/044-cert-to-name.md) |
| `certificate` | This command is used to delete already imported local/trusted/peer X509v3 certificates and to show a list of... | clear/show | oper | [045-certificate.md](06-operation-commands/045-certificate.md) |
| `crl` | This command is used to show one or all Certificate Revocation Lists (CRLs) presently on the system, and... | clear/show | oper+cand* | [063-crl.md](06-operation-commands/063-crl.md) |
| `csr-gen` | This command is used to generate a Certificate Signing Request based on user provided information |  | oper | [064-csr-gen.md](06-operation-commands/064-csr-gen.md) |
| `display-cert` | This command is used to show the details of a certificate or CSR | display-cert | oper+cand | [083-display-cert.md](06-operation-commands/083-display-cert.md) |
| `est` | The Enrollment over Secure Transport (EST) protocol enables robust and automated certificate management,... | est | oper+cand | [095-est.md](06-operation-commands/095-est.md) |
| `est-ca` | This command is used to represent a Certificate Authority (CA) which is set for Enrollment over Secure... | set/show | oper+cand | [096-est-ca.md](06-operation-commands/096-est-ca.md) |
| `est-server` | This command is used to configure the Enrollment over Secure Transport (EST) server settings | add/delete/set | oper+cand | [097-est-server.md](06-operation-commands/097-est-server.md) |
| `import-certificate` | This command allows to import one or more certificates in PEM format into the NE | import-certificate | oper | [131-import-certificate.md](06-operation-commands/131-import-certificate.md) |
| `ISK` | The show command is used to view the Image Signing Key (ISK) resources from the system | clear/show | oper+cand* | [147-isk.md](06-operation-commands/147-isk.md) |
| `key-replacement-package` | This command is used to show key replacement package (KRP) attributes | show | oper+cand | [148-key-replacement-package.md](06-operation-commands/148-key-replacement-package.md) |
| `KRK` | These commands are used to show the list of Image Root Keys (KRKs) list and KRK information | show | oper+cand | [150-krk.md](06-operation-commands/150-krk.md) |
| `local-certificate` | These commands are used to set or show the attributes of the X.509v3 end-entity certificate that represents... | set/show | oper+cand | [159-local-certificate.md](06-operation-commands/159-local-certificate.md) |
| `ocsp-server` | These commands are used to add, edit delete or show the attributes of an Online Certificate Status Protocol... | add/set/delete/show | oper+cand | [206-ocsp-server.md](06-operation-commands/206-ocsp-server.md) |
| `peer-certificate` | These commands are used to set or show the attributes of the X509v3 end-entity certificate that represents a... | set/show | oper+cand | [234-peer-certificate.md](06-operation-commands/234-peer-certificate.md) |
| `ssh` | These commands are used to set or show attributes of secure shell access | set/show | oper+cand | [306-ssh.md](06-operation-commands/306-ssh.md) |
| `ssh-authorized-key` | These commands are used to add, set, show an ssh authorized key | add/set/show/delete | oper+cand | [307-ssh-authorized-key.md](06-operation-commands/307-ssh-authorized-key.md) |
| `ssh-host-key` | This command is used to show global (for server and client side SSHv2 based apps) SSHv2 host keys | show | oper+cand | [308-ssh-host-key.md](06-operation-commands/308-ssh-host-key.md) |
| `ssh-keygen` | This command is used to generate a ssh private/public key pair | ssh-keygen | oper | [309-ssh-keygen.md](06-operation-commands/309-ssh-keygen.md) |
| `ssh-known-host` | These commands are used to add, set, show or delete an SSHv2 known hosts entry | add/set/show/delete | oper+cand | [310-ssh-known-host.md](06-operation-commands/310-ssh-known-host.md) |
| `trusted-certificate` | These commands are used to set or show the X509v3 CA (Root and Intermediate) certificate trusted by the system | set/show | oper+cand | [359-trusted-certificate.md](06-operation-commands/359-trusted-certificate.md) |

## Encryption: IPsec/IKEv2, MACsec and secure entities

<a id="encryption-ipsec-macsec"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `additional-key-exchange` | Users can configure additional key exchange algorithms (for example, classic, PQC, or hybrid with an... | add/set/show/delete | oper+cand | [011-additional-key-exchange.md](06-operation-commands/011-additional-key-exchange.md) |
| `data-path-encryption` | This command is used to show datapath encryption attributes | show | oper+cand | [071-data-path-encryption.md](06-operation-commands/071-data-path-encryption.md) |
| `encryption-algorithm` | This command is used to add or show encryption-algorithm attributes | add/show/delete | oper+cand | [091-encryption-algorithm.md](06-operation-commands/091-encryption-algorithm.md) |
| `ike-sa-proposal` | This command is used to add, edit or show a common set of attributes for IKEv2 used across Management IPsec | add/set/show/delete | oper+cand | [127-ike-sa-proposal.md](06-operation-commands/127-ike-sa-proposal.md) |
| `ikev2` | This command is used to set ikev2 | set/show | oper+cand | [128-ikev2.md](06-operation-commands/128-ikev2.md) |
| `ikev2-local-instance` | These commands are used to set and show an ikev2 local instance | set/show | oper+cand | [129-ikev2-local-instance.md](06-operation-commands/129-ikev2-local-instance.md) |
| `ikev2-peer` | These commands are used to add, edit or show an ikev2 peers associated with this local IKE instance | add/set/show/delete | oper+cand | [130-ikev2-peer.md](06-operation-commands/130-ikev2-peer.md) |
| `ipsec-sa-proposal` | This command is used to add, edit or show an ipsec sa proposal | add/set/show/delete | oper+cand | [139-ipsec-sa-proposal.md](06-operation-commands/139-ipsec-sa-proposal.md) |
| `ipsec-sa-re-key` | This command is used to add, edit or show ipsec sa re key | add/set/show/delete | oper+cand | [140-ipsec-sa-re-key.md](06-operation-commands/140-ipsec-sa-re-key.md) |
| `ipsec-spd-entry` | These commands are used to add, edit or show ipsec Security Policy Database entry | add/set/show/delete | oper+cand | [141-ipsec-spd-entry.md](06-operation-commands/141-ipsec-spd-entry.md) |
| `ipsec-traffic-selector` | This command is used to add, edit or show ipsec traffic selector | add/set/show/delete | oper+cand | [142-ipsec-traffic-selector.md](06-operation-commands/142-ipsec-traffic-selector.md) |
| `local-ports` | This command is used to add or show local ports | add/show/delete | oper+cand | [160-local-ports.md](06-operation-commands/160-local-ports.md) |
| `local-subnet` | This command is used to add or show a local subnet | add/show/delete | oper+cand | [161-local-subnet.md](06-operation-commands/161-local-subnet.md) |
| `macsec-entity` | The commands described in this section are used add, set, show and delete a macsec-entity | add/show/set/delete | oper+cand | [171-macsec-entity.md](06-operation-commands/171-macsec-entity.md) |
| `macsec-mka` | The commands described in this section are used to add, set, and show and delete a macsec-mka attributes | set/show | oper+cand | [172-macsec-mka.md](06-operation-commands/172-macsec-mka.md) |
| `mka-policy` | The commands described in this section are used add, set, show and delete a mka-policy MACsec Key Agreement... | add/show/set/delete | oper+cand | [173-mka-policy.md](06-operation-commands/173-mka-policy.md) |
| `re-auth` | This command is used to perform a re-authentication operation of IKEv2 security associations | re-auth | oper | [258-re-auth.md](06-operation-commands/258-re-auth.md) |
| `re-key` | This command is used to perform a re-key operation including on-demand re-keying of a data path encryption... | re-key | oper | [259-re-key.md](06-operation-commands/259-re-key.md) |
| `remote-ports` | This command is used to add or show a remote port | add/show/delete | oper+cand | [262-remote-ports.md](06-operation-commands/262-remote-ports.md) |
| `remote-subnet` | This command is used to add or show a remote subnet | add/show/delete | oper+cand | [263-remote-subnet.md](06-operation-commands/263-remote-subnet.md) |
| `sc-rx` | The commands described in this section are used to show the Receiving Secure Channel (`sc-rx`) attributes | show | oper+cand | [273-sc-rx.md](06-operation-commands/273-sc-rx.md) |
| `sc-tx` | The commands described in this section are used to show the Transmitting Secure Channel (`sc-tx`) attributes | show | oper+cand | [274-sc-tx.md](06-operation-commands/274-sc-tx.md) |
| `secure-application` | The commands described in this section are used to edit or show a secure-application or show secure-applications | set/show | oper+cand | [276-secure-application.md](06-operation-commands/276-secure-application.md) |
| `secure-entity` | These commands are used to add, edit or show a secure entity | add/set/show/delete | oper+cand | [277-secure-entity.md](06-operation-commands/277-secure-entity.md) |
| `secure-entity-sa-proposal` | The command described in this section is used to show the `secure-entity-sa-proposal` attributes | show | oper+cand | [278-secure-entity-sa-proposal.md](06-operation-commands/278-secure-entity-sa-proposal.md) |
| `security-policy-database` | These commands are used to add, edit or show the security database | add/set/show/delete | oper+cand | [281-security-policy-database.md](06-operation-commands/281-security-policy-database.md) |

## Management protocols, telemetry and third-party apps

<a id="management-protocols"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `app` | The `clear app` command, clears third party apps | clear | oper+cand | [021-app.md](06-operation-commands/021-app.md) |
| `appctl` | This command is used to control third-party applications | appctl | oper+cand | [022-appctl.md](06-operation-commands/022-appctl.md) |
| `call-home` | This command is used to execute a manual connection trigger to a configured dial-out-server | call-home | oper | [037-call-home.md](06-operation-commands/037-call-home.md) |
| `current-subscription` | This command is used to show a list representation of telemetry subscriptions that are configured in the... | show | oper+cand | [068-current-subscription.md](06-operation-commands/068-current-subscription.md) |
| `data-model` | These commands are used to enable or show the attributes of the available YANG Data models for loading/unloading | set/show | oper+cand | [070-data-model.md](06-operation-commands/070-data-model.md) |
| `dial-out-server` | These commands are used to add/edit or show the dial-out-server | add/delete/set/show | oper+cand | [079-dial-out-server.md](06-operation-commands/079-dial-out-server.md) |
| `grpc` | These commands are used to enable or show gNMI/gRPC management protocol | set/show | oper+cand | [121-grpc.md](06-operation-commands/121-grpc.md) |
| `netconf` | These commands are used to set or show NETCONF management protocol attributes | set/show | oper+cand | [188-netconf.md](06-operation-commands/188-netconf.md) |
| `restconf` | These commands are used to set or show configuration of the RESTCONF management protocol | set/show | oper+cand | [266-restconf.md](06-operation-commands/266-restconf.md) |
| `snmp` | These commands are used to set or show the configuration of the SNMP management protocol | set/show | oper+cand | [297-snmp.md](06-operation-commands/297-snmp.md) |
| `snmp-community` | These commands are used to add, set, show or delete an SNMP community | add/set/show/delete | oper+cand | [298-snmp-community.md](06-operation-commands/298-snmp-community.md) |
| `snmp-target` | These commands are used to add, set, show or delete a list of SNMP targets (trap listeners) | add/set/show/delete | oper+cand | [299-snmp-target.md](06-operation-commands/299-snmp-target.md) |
| `snmpv3-user` | These commands are used to add, set, show or delete a list of SNMP V3 user | add/set/show/delete | oper+cand | [300-snmpv3-user.md](06-operation-commands/300-snmpv3-user.md) |
| `subscription-path` | These commands are used to retrieve information subscription-paths | show | oper+cand | [316-subscription-path.md](06-operation-commands/316-subscription-path.md) |
| `subscriptions` | This command is used to show a list of subscriptions | show | oper+cand | [317-subscriptions.md](06-operation-commands/317-subscriptions.md) |
| `telemetry` | This command is used to configure persistent and dynamic telemetry | show/set | oper+cand | [344-telemetry.md](06-operation-commands/344-telemetry.md) |
| `third-party-app` | This command is used to set or show a third party application | set/show | oper+cand | [349-third-party-app.md](06-operation-commands/349-third-party-app.md) |

## Alarms, conditions and logging

<a id="fault-alarms-logging"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `alarm` | This command is used to clear alarms that have no auto criteria to be cleared and to show currently raised... | clear/show | oper | [014-alarm.md](06-operation-commands/014-alarm.md) |
| `alarm-control` | The commands described in this section are used to set or show the parameters related with alarm management control | set/show | oper+cand | [015-alarm-control.md](06-operation-commands/015-alarm-control.md) |
| `alarm-inventory` | The command described in this section is used to show the inventory with all possible alarm types for the... | show | oper+cand | [016-alarm-inventory.md](06-operation-commands/016-alarm-inventory.md) |
| `alarm-severity-entry` | The commands described in this section are used to set or show the individual entry in alarm-severity-entry | set/show | oper+cand | [017-alarm-severity-entry.md](06-operation-commands/017-alarm-severity-entry.md) |
| `alarm-severity-profile` | This command is used to set or show the alarm severity for a alarm profile | set/show | oper+cand | [018-alarm-severity-profile.md](06-operation-commands/018-alarm-severity-profile.md) |
| `current-alarms` | The command is used to show the list of currently raised alarms | show | oper+cand | [066-current-alarms.md](06-operation-commands/066-current-alarms.md) |
| `get-conditions` | This command is used to retrieve conditions | get-conditions | oper | [118-get-conditions.md](06-operation-commands/118-get-conditions.md) |
| `log` | This command is used to retrieve log files . If no <logname> is provided, the list of available logs is displayed | clear/show | oper+cand* | [163-log.md](06-operation-commands/163-log.md) |
| `log-console` | These commands are used to set or show the attributes of the console logging supported by the system | set/show | oper+cand | [164-log-console.md](06-operation-commands/164-log-console.md) |
| `log-console-facility-filter` | These commands are used to add, set or show a selector that filters messages based on their source facilities... | add/set/show/delete | oper+cand | [165-log-console-facility-filter.md](06-operation-commands/165-log-console-facility-filter.md) |
| `log-file` | These commands are used to add/set/show/delete local syslog files supported to the system | add/set/show/delete | oper+cand | [166-log-file.md](06-operation-commands/166-log-file.md) |
| `log-file-facility-filter` | These commands are used to add/set/show a selector that filters messages based on their source facilities and... | add/set/show/delete | oper+cand | [167-log-file-facility-filter.md](06-operation-commands/167-log-file-facility-filter.md) |
| `log-server` | This command is used to group or show the configuration parameters for log forwarding | add/set/show/delete | oper+cand | [168-log-server.md](06-operation-commands/168-log-server.md) |
| `log-server-facility-filter` | These commands allow to filter log messages based on their source facilities and severities | add/set/show/delete | oper+cand | [169-log-server-facility-filter.md](06-operation-commands/169-log-server-facility-filter.md) |
| `set-alarm-state` | The set-alarm-state changes the operator state of an alarm | set-alarm-state | oper | [288-set-alarm-state.md](06-operation-commands/288-set-alarm-state.md) |
| `statistics` | The command described in this section is used to clear the event counters (statistics) for the specified objects | clear | oper | [311-statistics.md](06-operation-commands/311-statistics.md) |
| `syslog` | These commands are used to set or show the configuration for logging functionality via syslog | set/show | oper+cand | [339-syslog.md](06-operation-commands/339-syslog.md) |

## Performance monitoring and statistics

<a id="performance-monitoring"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `pm` | The `clear pm` command, removes or resets PM data | clear/show | oper+cand* | [236-pm.md](06-operation-commands/236-pm.md) |
| `pm-catalog` | This command is used to show the contents of PM catalog | show | oper+cand | [237-pm-catalog.md](06-operation-commands/237-pm-catalog.md) |
| `pm-control` | These commands are used to set or show configuration for currently existing resources in the system that... | set/show | oper+cand | [238-pm-control.md](06-operation-commands/238-pm-control.md) |
| `pm-control-entry` | These commands are used to set or show the PM configuration for one particular resource, for one particular... | set/show | oper+cand | [239-pm-control-entry.md](06-operation-commands/239-pm-control-entry.md) |
| `pm-parameter` | This command is used to show pm parameter information | show | oper+cand | [240-pm-parameter.md](06-operation-commands/240-pm-parameter.md) |
| `pm-profile` | These commands are used to set or show a PM profile which contains information on all resources that support... | set/show | oper+cand | [241-pm-profile.md](06-operation-commands/241-pm-profile.md) |
| `pm-profile-entry` | These commands are used to set or show the PM configuration per resource type | set/show | oper+cand | [242-pm-profile-entry.md](06-operation-commands/242-pm-profile-entry.md) |
| `pm-resource` | These commands are used to set or show the PM configuration per resource instance | set/show | oper+cand | [243-pm-resource.md](06-operation-commands/243-pm-resource.md) |
| `pm-threshold` | These commands are used to add, set, show or delete a PM threshold | add/set/show/delete | oper+cand | [244-pm-threshold.md](06-operation-commands/244-pm-threshold.md) |
| `pm-threshold-profile` | These commands are used to set or show PM configuration per parameter, for this resource type | set/show | oper+cand | [245-pm-threshold-profile.md](06-operation-commands/245-pm-threshold-profile.md) |

## Software, firmware, file transfer and ZTP

<a id="software-firmware-files"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `activate` | The `activate` command is used to: activate a software image, activate a database, activate location LED... | activate | oper | [008-activate.md](06-operation-commands/008-activate.md) |
| `bootstrap` | The command described in this section is used to bootstrap a neighbor NE by establishing a TLS connection... | bootstrap | oper+cand | [032-bootstrap.md](06-operation-commands/032-bootstrap.md) |
| `cancel-upgrade` | This command is used to cancel any active upgrade in progress | cancel-upgrade | oper | [038-cancel-upgrade.md](06-operation-commands/038-cancel-upgrade.md) |
| `change-ztp-mode` | This command is used to toggle the Zero Touch Provisioning (ZTP) mode, deactivating it or reactivating it | change-ztp-mode | oper | [046-change-ztp-mode.md](06-operation-commands/046-change-ztp-mode.md) |
| `current-fw` | These commands are used to show the list of current firmware available in the cards | show | oper+cand | [067-current-fw.md](06-operation-commands/067-current-fw.md) |
| `download` | This command is used to download a file from an external location (a file server) to the NE. The type of file... | download | oper | [086-download.md](06-operation-commands/086-download.md) |
| `downloaded-image` | This command is used to retrieve information about downloaded image files | show | oper+cand | [087-downloaded-image.md](06-operation-commands/087-downloaded-image.md) |
| `downloads` | This command is used to show a list of downloads | show | oper+cand | [088-downloads.md](06-operation-commands/088-downloads.md) |
| `file` | This command is used to perform basic file and directory operations | clear/file | oper | [108-file.md](06-operation-commands/108-file.md) |
| `file-operation` | This command is used to perform basic file and directory operations | file-operation | oper | [109-file-operation.md](06-operation-commands/109-file-operation.md) |
| `file-server` | These commands are used to add, edit or show user-configurable file servers (e.g | add/set/show/delete | oper+cand | [110-file-server.md](06-operation-commands/110-file-server.md) |
| `file-type` | This command is used to display file-type transfer information | show | oper+cand | [111-file-type.md](06-operation-commands/111-file-type.md) |
| `http-file-server` | These commands are used to set/show HTTP file server attributes | set/show | oper+cand | [124-http-file-server.md](06-operation-commands/124-http-file-server.md) |
| `manifest` | These commands are used to show the downloaded manifest file and it's information | show | oper+cand | [176-manifest.md](06-operation-commands/176-manifest.md) |
| `packaged-fw` | These commands are used to show the Firmware version included in this software-load | show | oper+cand | [232-packaged-fw.md](06-operation-commands/232-packaged-fw.md) |
| `prepare-upgrade` | This command is used to prepare the network element software for upgrade | prepare-upgrade | oper | [247-prepare-upgrade.md](06-operation-commands/247-prepare-upgrade.md) |
| `recover-mode` | The `clear recover-mode` command, clears recover-mode flag | clear | oper | [260-recover-mode.md](06-operation-commands/260-recover-mode.md) |
| `software-load` | These commands are used to show the information on the Software Load present in the system | show | oper+cand | [301-software-load.md](06-operation-commands/301-software-load.md) |
| `software-location` | This command is used to retrieve information about the location of software | show | oper+cand | [302-software-location.md](06-operation-commands/302-software-location.md) |
| `subtype-constraint` | This command is used to show software subtype-constraint information | show | oper+cand | [318-subtype-constraint.md](06-operation-commands/318-subtype-constraint.md) |
| `sw-component` | This command is used to show the software load component details | show | oper+cand | [332-sw-component.md](06-operation-commands/332-sw-component.md) |
| `sw-container` | This command is used to show the list of OS-level containers | show | oper+cand | [333-sw-container.md](06-operation-commands/333-sw-container.md) |
| `sw-control-rule` | These commands are used to add, set or show option service-specific custom rules to override the default... | add/set/show/delete | oper+cand | [334-sw-control-rule.md](06-operation-commands/334-sw-control-rule.md) |
| `sw-management` | This command is used to show information about software locations, activity and downloads | show | oper+cand | [335-sw-management.md](06-operation-commands/335-sw-management.md) |
| `sw-service` | These commands are used to show the software service running in the system | show | oper+cand | [336-sw-service.md](06-operation-commands/336-sw-service.md) |
| `sw-subcomponent` | These commands are used to show the software load subcomponent details | show | oper+cand | [337-sw-subcomponent.md](06-operation-commands/337-sw-subcomponent.md) |
| `swversion` | This command is used to retrieve the active, inactive and/or installable versions of the software present on... | swversion | oper+cand | [338-swversion.md](06-operation-commands/338-swversion.md) |
| `third-party-fw` | This command is used to show third-party firmware information | show | oper+cand | [350-third-party-fw.md](06-operation-commands/350-third-party-fw.md) |
| `transfer` | These commands are used to display information about file transfers | set/show | oper+cand | [356-transfer.md](06-operation-commands/356-transfer.md) |
| `transfer-status` | The `show transfer-status` displays information associated with file transfer | show | - | [357-transfer-status.md](06-operation-commands/357-transfer-status.md) |
| `upgrade-status` | This command displays all the SW versions being installed in the system, and their installation status | show | oper+cand | [363-upgrade-status.md](06-operation-commands/363-upgrade-status.md) |
| `upload` | This command is used to upload files to a remote server | upload | oper | [364-upload.md](06-operation-commands/364-upload.md) |
| `ztp` | This command shows the Zero Touch Provisioning (ZTP) status | show | oper+cand | [374-ztp.md](06-operation-commands/374-ztp.md) |

## Node-level system, time and status

<a id="system-node-time"></a>

| Command | What it does | Verbs | Mode | File |
| --- | --- | --- | --- | --- |
| `clock` | These commands are used to set or show the system clock | set/show | oper+cand | [052-clock.md](06-operation-commands/052-clock.md) |
| `ne` | These commands are used to set/show network element attributes | set/show | oper+cand | [186-ne.md](06-operation-commands/186-ne.md) |
| `ne-function` | This command is used to show the Network Element (NE) function | show | oper+cand | [187-ne-function.md](06-operation-commands/187-ne-function.md) |
| `ntp` | These commands are used to configure and show the Network Time Protocol | set/show | oper+cand | [195-ntp.md](06-operation-commands/195-ntp.md) |
| `ntp-key` | These commands are used to add, configure, show and delete NTP keys to be used for NTP authentication | add/set/show/delete | oper+cand | [196-ntp-key.md](06-operation-commands/196-ntp-key.md) |
| `ntp-server` | These commands are used to add, set or show the NTP server attributes | add/set/show/delete | oper+cand | [197-ntp-server.md](06-operation-commands/197-ntp-server.md) |
| `ntp-server-status` | These commands are used to configure and show the NTP server status | show | oper+cand | [198-ntp-server-status.md](06-operation-commands/198-ntp-server-status.md) |
| `restart` | Restarts a specific resource of the system | restart | oper | [265-restart.md](06-operation-commands/265-restart.md) |
| `set-time` | The `set-time` command changes the system time | set-time | oper | [289-set-time.md](06-operation-commands/289-set-time.md) |
| `status` | This command is used to display multiple dashboard-type outputs | status | oper | [312-status.md](06-operation-commands/312-status.md) |
| `system` | The `set system` command is used to set system attributes including the following: | show/set/clear | oper+cand* | [340-system.md](06-operation-commands/340-system.md) |
| `time` | This command is used to display the system's time | time | oper+cand | [351-time.md](06-operation-commands/351-time.md) |
| `uptime` | This command displays the system uptime and load average | uptime | oper+cand | [365-uptime.md](06-operation-commands/365-uptime.md) |
