# Access control index - user groups, objects and commands

Derived from Chapter 2 Tables 32-34 ([2.2.10 User groups and access privilege](../02-using-the-cli/02-using-the-cli.md#2210-user-groups-and-access-privilege)). A user may belong to several groups; the highest permission wins.

## User groups

| Code | Group | Privilege summary |
| --- | --- | --- |
| MA | Monitoring Access | Read-only across equipment and traffic model |
| NA | Network Administrator | Read/write system, DCN, software and firmware |
| SA | Security Administrator | Read/write all security, AAA and certificates |
| PR | Provisioning | Facility endpoints and service provisioning |
| NE | Network Engineer | Equipment, facility endpoints and cross-connections |
| EA | Encryption Administrator | Data and control plane encryption |
| TT | Test and Turn up | Turn-up and troubleshooting |

## Command execution access (Table 34)

47 commands have an explicit execution-access entry. Commands not listed here are governed by object access below.

| Command | Sub-command | Conditions | Groups | Notes | File |
| --- | --- | --- | --- | --- | --- |
| `activate` | activate-file | swimage/database | NA | - | [008-activate.md](../06-operation-commands/008-activate.md) |
| `activate` | eqpt-fw | - | NA | - | [008-activate.md](../06-operation-commands/008-activate.md) |
| `activate` | location-led | - | NA,NE | - | [008-activate.md](../06-operation-commands/008-activate.md) |
| `activate` | krp | - | SA | - | [008-activate.md](../06-operation-commands/008-activate.md) |
| `add` | - | - | all | - | [010-add.md](../06-operation-commands/010-add.md) |
| `alias` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#41-alias) |
| `call-home` | - | - | NA,NE | - | [037-call-home.md](../06-operation-commands/037-call-home.md) |
| `cert-gen` | - | - | SA | - | [043-cert-gen.md](../06-operation-commands/043-cert-gen.md) |
| `change-ztp-mode` | - | - | SA | - | [046-change-ztp-mode.md](../06-operation-commands/046-change-ztp-mode.md) |
| `clear` | app | - | NA | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | certificate | - | SA | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | database | - | NA | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | diagnostics | - | NA,NE | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | file | swimage | NA | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | isk | (other) - | NA,NE SA | - - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | log | - | NA,NE | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | ospf | - | NA,PR,TT | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | pm | - | NA,NE | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | recover-mode | - | SA | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | system | - | NA | - | [049-clear.md](../06-operation-commands/049-clear.md) |
| `clear` | topology | - | NA,PR,TT | Clears LLDP neighbor info. | [049-clear.md](../06-operation-commands/049-clear.md) |
| `convert` | - | - | all | - | [062-convert.md](../06-operation-commands/062-convert.md) |
| `default` | - | &lt;same as target parameter&gt; |  | - | [075-default.md](../06-operation-commands/075-default.md) |
| `delete` | - | - | all | - | [077-delete.md](../06-operation-commands/077-delete.md) |
| `download` | - | swimage | NA | - | [086-download.md](../06-operation-commands/086-download.md) |
| `download` | - | script | NA | - | [086-download.md](../06-operation-commands/086-download.md) |
| `download` | - | database | NA | - | [086-download.md](../06-operation-commands/086-download.md) |
| `download` | - | certificate | SA | - | [086-download.md](../06-operation-commands/086-download.md) |
| `download` | - | krp | SA | - | [086-download.md](../06-operation-commands/086-download.md) |
| `download` | - | (other) | NA,SA,NE | - | [086-download.md](../06-operation-commands/086-download.md) |
| `edit` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#42-edit) |
| `exit` | - | - | all | - | [100-exit.md](../06-operation-commands/100-exit.md) |
| `export` | - | - | all | - | [102-export.md](../06-operation-commands/102-export.md) |
| `file` | - | - | NA,NE,TT | - | [108-file.md](../06-operation-commands/108-file.md) |
| `gshell` | - | - | NA,NE,TT | - | [122-gshell.md](../06-operation-commands/122-gshell.md) |
| `help` | - | - | all | - | [03-auxiliary-and-help-commands.md](../03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md#3-auxiliary-and-help-commands) |
| `history` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#43-history) |
| `kill-session` | - | - | SA | - | [149-kill-session.md](../06-operation-commands/149-kill-session.md) |
| `lock` | - | - | SA,NA,EA,NE,TT,PR | - | [162-lock.md](../06-operation-commands/162-lock.md) |
| `password` | - | - | all | (changes own password) | [233-password.md](../06-operation-commands/233-password.md) |
| `ping` | - | - | NA,PR,TT | - | [235-ping.md](../06-operation-commands/235-ping.md) |
| `prepare-upgrade` | - | - | NA | - | [247-prepare-upgrade.md](../06-operation-commands/247-prepare-upgrade.md) |
| `restart` | - | - | NA,NE | Includes warm/cold/ shutdown for cards/ toms | [265-restart.md](../06-operation-commands/265-restart.md) |
| `run` | script task | - - | all NA,NE,TT | (script content will be limited by current user credentials) - | [272-run.md](../06-operation-commands/272-run.md) |
| `set` | - | - | all | - | [287-set.md](../06-operation-commands/287-set.md) |
| `set-alarm-state` | - | - | NA,NE,PR,TT | - | [288-set-alarm-state.md](../06-operation-commands/288-set-alarm-state.md) |
| `set-time` | - | - | NA,NE,TT | - | [289-set-time.md](../06-operation-commands/289-set-time.md) |
| `shell` | - | - | NA,NE,TT | - | [290-shell.md](../06-operation-commands/290-shell.md) |
| `show` | - | - | all | - | [291-show.md](../06-operation-commands/291-show.md) |
| `simulate` | - | - | NA,NE | For equipment simulation | [293-simulate.md](../06-operation-commands/293-simulate.md) |
| `sleep` | - | - | all | - | [294-sleep.md](../06-operation-commands/294-sleep.md) |
| `ssh-keygen` | - | - | SA | - | [309-ssh-keygen.md](../06-operation-commands/309-ssh-keygen.md) |
| `swversion` | - | - | all | - | [338-swversion.md](../06-operation-commands/338-swversion.md) |
| `take-snapshot` | - | - | NA | - | [342-take-snapshot.md](../06-operation-commands/342-take-snapshot.md) |
| `terminate` | - | - | all | - | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `time` | - | - | all | - | [351-time.md](../06-operation-commands/351-time.md) |
| `top` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#44-top) |
| `traceroute` | - | - | NA,PR,TT | - | [355-traceroute.md](../06-operation-commands/355-traceroute.md) |
| `tree` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#45-tree) |
| `unalias` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#46-unalias) |
| `unlock` | - | - | SA,NA,EA,NE,TT,PR | - | [360-unlock.md](../06-operation-commands/360-unlock.md) |
| `up` | - | - | all | - | [04-navigation-and-display-commands.md](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#47-up) |
| `upgrade-status` | - | - | all | - | [363-upgrade-status.md](../06-operation-commands/363-upgrade-status.md) |
| `upload` | - | database | NA | - | [364-upload.md](../06-operation-commands/364-upload.md) |
| `upload` | - | debug-log | NA,NE | - | [364-upload.md](../06-operation-commands/364-upload.md) |
| `upload` | - | logs | NA,NE,TT | - | [364-upload.md](../06-operation-commands/364-upload.md) |
| `upload` | - | pm-logs | NA,NE,TT | - | [364-upload.md](../06-operation-commands/364-upload.md) |
| `uptime` | - | - | all | - | [365-uptime.md](../06-operation-commands/365-uptime.md) |
| `validate` | - | - | all | - | [370-validate.md](../06-operation-commands/370-validate.md) |

## Data-model object access (Table 33)

70 objects. `Write` lists the groups that may create, update or delete; all groups may read unless stated.

| Object | Write access | Read access | Command page |
| --- | --- | --- | --- |
| `aaa-server` | SA | all | [001-aaa-server.md](../06-operation-commands/001-aaa-server.md) |
| `ace` | SA | all | [006-ace.md](../06-operation-commands/006-ace.md) |
| `acl` | SA | all | [007-acl.md](../06-operation-commands/007-acl.md) |
| `alarm` | NA | all | [014-alarm.md](../06-operation-commands/014-alarm.md) |
| `alarm-control` | NA | all | [015-alarm-control.md](../06-operation-commands/015-alarm-control.md) |
| `asap` | NA | all | - |
| `card` | NA,NE | all | [040-card.md](../06-operation-commands/040-card.md) |
| `certificates` | SA | all | [045-certificate.md](../06-operation-commands/045-certificate.md) |
| `chassis` | NA,NE | all | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `cli` | SA,NA | all | [050-cli.md](../06-operation-commands/050-cli.md) |
| `clock` | NA,NE,TT | all | [052-clock.md](../06-operation-commands/052-clock.md) |
| `command` | Whatever user-group is able to do this command | all | - |
| `community-string` | SA,NA | all | [298-snmp-community.md](../06-operation-commands/298-snmp-community.md) |
| `connect` | NA,NE | all | [058-connect.md](../06-operation-commands/058-connect.md) |
| `data-model` | SA,NA | all | [070-data-model.md](../06-operation-commands/070-data-model.md) |
| `data-path-encryption` | EA | all | [071-data-path-encryption.md](../06-operation-commands/071-data-path-encryption.md) |
| `dial-out-server` | NA,NE | all | [079-dial-out-server.md](../06-operation-commands/079-dial-out-server.md) |
| `dns` | NA,NE,TT | all | [084-dns.md](../06-operation-commands/084-dns.md) |
| `equipment` | NA,NE | all | [092-equipment.md](../06-operation-commands/092-equipment.md) |
| `eth-zr` | NA,PR,TT | all | [098-eth-zr.md](../06-operation-commands/098-eth-zr.md) |
| `ethernet` | NA,PR,TT | all | [099-ethernet.md](../06-operation-commands/099-ethernet.md) |
| `facilities` | NA,PR,TT | all | [105-facilities.md](../06-operation-commands/105-facilities.md) |
| `file-server` | NA,NE,TT | all | [110-file-server.md](../06-operation-commands/110-file-server.md) |
| `flexo` | NA,PR,TT | all | [112-flexo.md](../06-operation-commands/112-flexo.md) |
| `flexo-group` | NA,PR,TT | all | [113-flexo-group.md](../06-operation-commands/113-flexo-group.md) |
| `grpc` | SA,NA | all | [121-grpc.md](../06-operation-commands/121-grpc.md) |
| `interface` | NA,NE,TT | all | [134-interface.md](../06-operation-commands/134-interface.md) |
| `leds` | NA,NE | all | [152-led.md](../06-operation-commands/152-led.md) |
| `lldp` | NA,PR,TT | all | [155-lldp.md](../06-operation-commands/155-lldp.md) |
| `log-console` | NA,NE,TT | all | [164-log-console.md](../06-operation-commands/164-log-console.md) |
| `log-file` | NA,NE,TT | all | [166-log-file.md](../06-operation-commands/166-log-file.md) |
| `log-server` | NA,NE,TT | all | [168-log-server.md](../06-operation-commands/168-log-server.md) |
| `ne` | NA,NE,TT | all | [186-ne.md](../06-operation-commands/186-ne.md) |
| `netconf` | SA,NA | all | [188-netconf.md](../06-operation-commands/188-netconf.md) |
| `networking` | NA,NE,TT | all | [190-networking.md](../06-operation-commands/190-networking.md) |
| `ntp` | NA,NE,TT | all | [195-ntp.md](../06-operation-commands/195-ntp.md) |
| `ntp-key` | SA | all | [196-ntp-key.md](../06-operation-commands/196-ntp-key.md) |
| `ntp-server` | NA,NE,TT | all | [197-ntp-server.md](../06-operation-commands/197-ntp-server.md) |
| `odu` | NA,PR,TT | all | [207-odu.md](../06-operation-commands/207-odu.md) |
| `optical-carrier` | NA,PR,TT | all | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `ospf` | NA,NE,TT | all | [216-ospf.md](../06-operation-commands/216-ospf.md) |
| `pm` | NA,NE,TT | all | [236-pm.md](../06-operation-commands/236-pm.md) |
| `pm-control` | NA,NE,TT | all | [238-pm-control.md](../06-operation-commands/238-pm-control.md) |
| `pm-profile` | NA,NE,TT | all | [241-pm-profile.md](../06-operation-commands/241-pm-profile.md) |
| `protocols` | SA,NA | all | [254-protocols.md](../06-operation-commands/254-protocols.md) |
| `routing` | NA,NE,TT | all | [270-routing.md](../06-operation-commands/270-routing.md) |
| `secure-applications` | SA | all | [276-secure-application.md](../06-operation-commands/276-secure-application.md) |
| `security` | SA | all | [279-security.md](../06-operation-commands/279-security.md) |
| `security-policies` | SA | all | [280-security-policies.md](../06-operation-commands/280-security-policies.md) |
| `services` | NA,PR | all | - |
| `session` | SA | SA + other users can read themselves only | [286-session.md](../06-operation-commands/286-session.md) |
| `snmp` | SA,NA | all | [297-snmp.md](../06-operation-commands/297-snmp.md) |
| `snmp-target` | SA,NA | all | [299-snmp-target.md](../06-operation-commands/299-snmp-target.md) |
| `snmpv3-user` | SA,NA | all | [300-snmpv3-user.md](../06-operation-commands/300-snmpv3-user.md) |
| `ssh` | SA,NA | all | [306-ssh.md](../06-operation-commands/306-ssh.md) |
| `ssh-authorized-keys` | SA | all | [307-ssh-authorized-key.md](../06-operation-commands/307-ssh-authorized-key.md) |
| `ssh-known-host` | SA | all | [310-ssh-known-host.md](../06-operation-commands/310-ssh-known-host.md) |
| `static-route` | NA,NE,TT | all | - |
| `sw-control-rule` | NA | all | [334-sw-control-rule.md](../06-operation-commands/334-sw-control-rule.md) |
| `sw-services` | NA | all | [336-sw-service.md](../06-operation-commands/336-sw-service.md) |
| `syslog` | NA,NE,TT | all | [339-syslog.md](../06-operation-commands/339-syslog.md) |
| `system` | NA,NE,TT | all | [340-system.md](../06-operation-commands/340-system.md) |
| `tasks` | NA,NE,TT | all | [343-task.md](../06-operation-commands/343-task.md) |
| `tom` | NA,NE | all | [352-tom.md](../06-operation-commands/352-tom.md) |
| `topology` | NA,PR,TT | all | [354-topology.md](../06-operation-commands/354-topology.md) |
| `trib-ptp` | NA,PR,TT | all | [358-trib-ptp.md](../06-operation-commands/358-trib-ptp.md) |
| `user` | SA | SA + other users can read their sessions only | [367-user.md](../06-operation-commands/367-user.md) |
| `user-group` | SA | all | [369-user-group.md](../06-operation-commands/369-user-group.md) |
| `xcon` | NA,PR | all | [373-xcon.md](../06-operation-commands/373-xcon.md) |
| `ztp` | SA | all | [374-ztp.md](../06-operation-commands/374-ztp.md) |

## Source data quality

The following rows are ambiguous **in the source document**: the PDF to Markdown conversion collapsed what were two table rows into one, so a single cell holds two values. They are reproduced verbatim above rather than split, because the pairing cannot be recovered with certainty. Check the guide's own Table 34 before relying on them.

| Command | Sub-command | Conditions | Groups | Notes |
| --- | --- | --- | --- | --- |
| `clear` | isk | (other) - | NA,NE SA | - - |
| `default` | - | &lt;same as target parameter&gt; | (none given) | - |
| `run` | script task | - - | all NA,NE,TT | (script content will be limited by current user credentials) - |

Objects with no single matching command page: `asap`, `command`, `services`, `static-route` (container or model-level names rather than CLI commands).
