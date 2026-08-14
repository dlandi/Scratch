# GX CLI Reference - LLM unit tests

376 single-command and 5 multi-command tests. Generated for reading; the machine-readable source is in `tests/`.

## Single-command tests

### pm-export-csv

*performance-monitoring / how-to*

**Q.** How can I get 15-minute performance counters out of the node in a form I can open in a spreadsheet?

**A.** Use the show pm command with a display filter. The guide states that `show pm <command flag> | display csv` outputs CSV data importable to a spreadsheet, and that xml and json are also available. PM data is either real-time (continuously updated) or binned into time windows, with bin 0 being current data and bins 1..n the history; the node keeps both 15-minute (pm-15min) and 24-hour (pm-24h) granularities. Filter with -sbin/-ebin for a bin range or -st/-et for timestamps.

Source: `06-operation-commands/236-pm.md`

### pm-catalog-lookup

*performance-monitoring / which-command*

**Q.** I have a counter name and want to know what it measures. Where do I look it up?

**A.** Use `show pm-catalog <string>`, where the string is the catalog name. The output gives the parameter with its units and type, for example pm-parameter-undersized shows units 'packets' and type 'counter'. The same information is available per parameter through show pm-parameter.

Source: `06-operation-commands/237-pm-catalog.md`

### pm-control-disable-resource

*performance-monitoring / how-to-default*

**Q.** How do I stop the node collecting real-time performance data for one particular card?

**A.** Set data-supervision to false on that resource with pm-control, for example `set pm-control pm-resource-card-1-1 data-supervision false`. The default for data-supervision is true, so collection is on unless you turn it off.

Source: `06-operation-commands/238-pm-control.md`

### pm-control-entry-scope

*performance-monitoring / parameter-values*

**Q.** Can I turn threshold-crossing supervision on for only the 15-minute ingress near-end counters of one resource?

**A.** Yes. pm-control-entry is addressed per resource, period, direction and location, so the instance identifies exactly that scope: set pm-control-entry-<resource>/<period>/<direction>/<location> tca-supervision true. Direction accepts ingress, egress, all or na (default all) and location accepts all, na, near-end or far-end (default all). The same entry also carries data-supervision.

Source: `06-operation-commands/239-pm-control-entry.md`

### pm-parameter-units-type

*performance-monitoring / enumeration*

**Q.** How do I tell whether a performance parameter is a counter or a gauge, and what units it reports?

**A.** Use show pm-parameter-<parameter> with the units and type attributes. Type is either counter or gauge. Units come from a fixed list that includes na, dBm, ms, ps, ps/nm, dB, seconds, packets, events, octets, bits, blocks, times, percent, bit-ratio, C, frames, W, V, A, rpm, ps2, mA, words, cw, nm, bytes, errors and MHz.

Source: `06-operation-commands/240-pm-parameter.md`

### pm-profile-global-supervision

*performance-monitoring / how-to-default*

**Q.** Is there a way to switch performance data collection on everywhere at once instead of resource by resource?

**A.** Yes, through pm-profile. `set pm-profile global-data-supervision auto-enabled` globally enables PM data supervision without toggling it individually. The other value is manual, which is the default, and leaves the flag controlled per pm-profile-entry or directly per pm-control-entry. The pm-profile object is system managed and cannot be manually deleted.

Source: `06-operation-commands/241-pm-profile.md`

### pm-profile-entry-defaults

*performance-monitoring / how-to*

**Q.** How do I make newly created facilities of a given type come up with performance monitoring already enabled?

**A.** Use pm-profile-entry, which holds the PM configuration per resource type. Set default-data-supervision and default-tca-supervision on pm-profile-entry-<resource-type>/<direction>/<location>/<period>. Those defaults apply to newly created resources of that type; they do not retroactively change existing ones.

Source: `06-operation-commands/242-pm-profile-entry.md`

### pm-resource-last-reset

*performance-monitoring / which-attribute*

**Q.** How can I find out when a port's real-time performance counters were last cleared?

**A.** Show the pm-resource for that port and read real-time-data-last-reset, which gives the date and time of the last real-time data reset; if the data was never reset it shows the resource's creation time. The same object carries real-time-supervision (true or false, default true), resource-type and AID.

Source: `06-operation-commands/243-pm-resource.md`

### pm-threshold-set-one-instance

*performance-monitoring / how-to-default*

**Q.** How do I put a high limit on errored seconds for one specific facility?

**A.** Add or set a pm-threshold on that instance: pm-threshold-<resource>/<period>/<direction>/<location>/<parameter> with high-threshold (and optionally low-threshold). Both accept a number or the keywords na and not-supported, and both default to na. Delete the pm-threshold instance to remove the limit.

Source: `06-operation-commands/244-pm-threshold.md`

### pm-threshold-profile-defaults

*performance-monitoring / which-attribute*

**Q.** Where can I see the system's own default limits for a performance parameter, and the range I am allowed to configure?

**A.** Show the pm-threshold-profile for that resource type and parameter. Besides the configured low-threshold and high-threshold it exposes default-low-threshold and default-high-threshold, which are the system defined defaults, and min-value and max-value, which bound what the parameter can be. It is addressed per resource-type, direction, location, period and parameter.

Source: `06-operation-commands/245-pm-threshold-profile.md`

### clock-timezone-and-source

*system-node-time / how-to*

**Q.** How do I change the node's timezone, and how can I check whether it is taking its time from NTP or from itself?

**A.** Set the timezone with `set clock timezone <value>`, for example Beijing-Chongqing-Hong_Kong-Urumqi[GMT+08:00]. Read the source with show clock: the time-source attribute is ntp when the NE synchronises via NTP and manual when it uses its internal clock, with manual as the default. show clock also reports current-time, universal-time, uptime, DST-active and last-time-jump. Note the guide warns that if DNS is not properly configured show clock can take up to 2 minutes to respond.

Source: `06-operation-commands/052-clock.md`

### ne-node-type-change

*system-node-time / pre-condition*

**Q.** Can I convert a G34c node from an in-line amplifier into an add/drop node, and does anything have to be cleared first?

**A.** Yes, by setting node-type on the ne object; node-type can only be set or changed for the G34c chassis, which defaults to ILA. The valid values are ILA, OADM and XPDR. To move to OADM the guide requires that no OMS monitoring-mode in the system is 'ila-with-equalization' or 'not-monitored' (that is, no spectrum or ochm objects exist) and that no direction-* exists. Moving the other way, to ILA, requires that no dwdm-line port has OMS monitoring-mode 'intrusive' (no RD or PAx card), and that no degree, oxcon or nmc exists. G31 and G32 support only OADM.

Source: `06-operation-commands/186-ne.md`

### ne-function-list

*system-node-time / which-command*

**Q.** Which command lists the node's directions and amplifiers in one place?

**A.** `show ne-function` displays the Network Element generic functions. On a G30 node the output lists the direction-N entries followed by the amplifier and amplifier-raman instances. The command takes no parameters and is available in both operational and candidate configuration mode.

Source: `06-operation-commands/187-ne-function.md`

### ntp-enable-with-auth

*system-node-time / how-to-default*

**Q.** How do I turn on time synchronisation and require it to be authenticated?

**A.** Set both flags on the ntp object: `set ntp ntp-enabled true ntp-auth-enabled true`. ntp-enabled defaults to true and ntp-auth-enabled defaults to false, so authentication is the one you have to turn on. assignment-method controls whether manually configured servers, DHCP-supplied ones, or both are used, and defaults to both. The ntp object is system managed and cannot be added or deleted manually.

Source: `06-operation-commands/195-ntp.md`

### ntp-key-algorithms

*system-node-time / enumeration*

**Q.** What hash algorithms are available for NTP authentication keys, and what are the limits on key id and key value?

**A.** ntp-key supports key-type sha-1, aes-cmac, sha-256 and md5. The key-id is a number in the range 1 to 65534 and forms the instance name. key-value is a string of 8 to 40 characters. is-trusted marks the key as trusted and defaults to false, so a newly added key is not trusted unless you say so.

Source: `06-operation-commands/196-ntp-key.md`

### ntp-server-dhcp-to-manual

*system-node-time / how-to*

**Q.** Our time server was handed to the node by DHCP. Can I keep it but manage it ourselves?

**A.** Yes. The origin attribute on ntp-server records how the address was assigned, dhcp or manual, and the guide states that a user can convert a DHCP configured NTP entry into a manually configured one by changing this attribute: `set ntp-server-<ip-address> origin manual`. The instance is keyed by IP address and accepts IPv4, IPv6 or a DNS name. auth-key-id ties the server to an ntp-key and defaults to not-applicable.

Source: `06-operation-commands/197-ntp-server.md`

### ntp-server-status-reachability

*system-node-time / troubleshooting*

**Q.** How can I tell whether the node is actually reaching its time server?

**A.** Show ntp-server-status for that server address. The reach attribute is an 8-bit shift register with the most recent probe in the 2^0 position, and the value 377 means all recent probes were answered. Alongside it, when gives the seconds since the last packet, poll the polling interval, delay, offset and jitter the path measurements, auth-status one of ok, yes, bad or none, and condition a string such as sys.peer, reject or candidate. stratum shows the remote peer's stratum.

Source: `06-operation-commands/198-ntp-server-status.md`

### restart-warm-vs-cold

*system-node-time / comparison*

**Q.** What is the difference between a warm and a cold reboot of a card, and which one happens if I don't say?

**A.** Warm is the default when the type parameter is omitted. Warm reboots the processor of the card and is non service affecting, with connectivity regained within five minutes (for XMM4 it is non-service affecting but visibility is lost for about 3 minutes). Cold reboots all components and sub-components and is service affecting, again regaining connectivity within five minutes; cold reboot of XMM4 is not service affecting but visibility is lost. Shutdown gracefully shuts the card down. If no resource-id is given the active controller card is restarted, and the command asks for confirmation unless -f is supplied. Not all cards support all restart types.

Source: `06-operation-commands/265-restart.md`

### set-time-requires-manual-source

*system-node-time / troubleshooting*

**Q.** Why is the node refusing to let me set the clock by hand?

**A.** Because manual time setting only applies when the time source is manual, that is when NTP is not enabled. The guide states set-time is only applicable when time-source is manual. The value uses an ISO 8601 derived format combining date and time, such as 2021-02-06T11:16:58Z, where Z is UTC; a non-UTC offset can be given as +/-hh:mm instead. Read the current time back with the time command, which reports it in the system configured timezone rather than the timezone you supplied.

Source: `06-operation-commands/289-set-time.md`

### status-equipment-dashboard

*system-node-time / which-command*

**Q.** Is there one command that summarises temperature and power for all the equipment in the node?

**A.** Yes, the equipment dashboard: `status equipment`. The status command displays dashboard-style summaries; the equipment dashboard lists chassis, the cards per chassis and their subtype, the toms per card, and per-equipment data including temperature sensors and power values along with presence, operational state and alarms. On a large NE the output can be filtered to one chassis by giving the chassis id, for example status equipment 5; only existing chassis ids are accepted and wildcards are not supported there. Called with no argument, status shows the system dashboard.

Source: `06-operation-commands/312-status.md`

### system-factory-reset

*system-node-time / how-to*

**Q.** How do I wipe a node back to factory configuration, and what are the limits on that?

**A.** Use `clear system factory-reset`. The clear system command wipes the system or a specific instance back to factory configuration and does a secure wipe of the system data; it stops target traffic services and removes files and user configurations, which may mean loss of connectivity. The guide notes factory-reset does not take effect for G30 optical carrier cards (OCC2T, OCC2E) and is supported for the G30 controller card (FRCU), and that factory-reset with the shutdown option is not supported on L0 cards. A stronger option, full-wipe, cleans the entire system and reinstalls software, after which a base software reinstall from ONIE is needed to recover. The target can be the whole system or a chassis or card by AID, and restart-behavior can be a standard restart (default) or shutdown. For a simple database wipe the guide points to clear database instead.

Source: `06-operation-commands/340-system.md`

### time-current

*system-node-time / minimal-command  (weak: the source section is thin)*

**Q.** What is the quickest way to print the node's current date and time?

**A.** Just run `time`. It takes no parameters, works in operational and candidate configuration mode, and prints a single ISO 8601 timestamp such as 2020-06-29T22:29:37+00:00. For the timezone, source and uptime as well, use show clock instead.

Source: `06-operation-commands/351-time.md`

### uptime-load-average

*system-node-time / interpretation*

**Q.** The node reports a load average. How do I know whether the number is bad?

**A.** The uptime command shows system uptime and load average, for example '21:24:09 up 41 days, 15:56, 1 user, load average: 0.10, 0.35, 0.39'. The guide states a load average is considered high when it is greater than the number of CPUs on the card, and that whether a value is high or low depends on the number of cores, how many CPUs are integrated into the card, and the value itself.

Source: `06-operation-commands/365-uptime.md`

### manual-switchover-impact

*protection-redundancy / consequence*

**Q.** If I switch over to the standby controller, will I lose my management session?

**A.** Yes. manual-switchover prompts 'Controller will switchover and connection to the management interface will be lost. Do you want to continue? [y/n]' and you confirm to proceed. The command takes the object to be switched as an AID, for example manual-switchover card-1-3, and accepts -f to force it without the confirmation. It runs in operational mode only.

Source: `06-operation-commands/177-manual-switchover.md`

### protection-show

*protection-redundancy / minimal-command  (weak: the source section is thin)*

**Q.** Which command shows the node's protection container?

**A.** `show protection`. It takes no parameters and is available in operational and candidate configuration mode. For the detail you almost certainly want, look at protection-group (configuration and state of each group) and protection-unit (the working and protection members).

Source: `06-operation-commands/250-protection.md`

### protection-group-timers

*protection-redundancy / parameter-values*

**Q.** What are the default switching mode and wait-to-restore time on a protection group, and what range can the timer take?

**A.** switching-mode is unidirectional or bidirectional and defaults to unidirectional. wtr-timer is the trigger clearance soaking time before reverting to the working unit, set in 1-second steps, range 60 to 720 seconds, default 300, and it only applies in revertive mode. reversion-mode itself is revertive or non-revertive and defaults to non-revertive, so by default there is no automatic reversion. Separately, hold-off-timer is the switching trigger soaking time before switching, in 1-millisecond steps, range 0 to 10000, default 0. protection-type is y-cable or snc-n, default y-cable.

Source: `06-operation-commands/251-protection-group.md`

### protection-switch-force

*protection-redundancy / how-to*

**Q.** How do I force traffic onto the protection path of a group?

**A.** Use the protection-switch operating command: `protection-switch protection-group-test operation-type=force switch-target=protection`. operation-type is one of force, lockout, manual or release. switch-target names what to switch to and is not needed for the release and lockout operations. protection-group identifies the group. It runs in operational mode and -f forces it without confirmation.

Source: `06-operation-commands/252-protection-switch.md`

### protection-unit-which-is-active

*protection-redundancy / which-attribute*

**Q.** How do I tell which member of a protected pair is currently carrying the traffic?

**A.** Show the protection-unit, addressed as protection-unit-<protection-group-name>/<protection-unit-name>. The state attribute is active, standby, available or unknown, and role is working or protection, so the unit with state active is the one carrying traffic. transport-entity gives the instance identifier of the underlying transport entity. Note alarm-report-control on a protection unit defaults to inhibited.

Source: `06-operation-commands/253-protection-unit.md`

### alarm-manual-clear

*fault-alarms-logging / how-to*

**Q.** A few alarms are stuck up and never clear by themselves. How do I get rid of them?

**A.** Most alarms have both an automatic raise and an automatic clear criteria, but a small subset have no clear criteria and require the operator to acknowledge and clear them manually with `clear alarm [alarm-type=]<value>`. To find which ones those are, list the alarm inventory filtered on the can-be-cleared-by-user flag: `show alarm-inventory can-be-cleared-by-user=true`. This mechanism applies only to system wide alarms that are not associated with any particular resource. The command runs in operational mode and takes -f to skip confirmation.

Source: `06-operation-commands/014-alarm.md`

### alarm-control-arc-behavior

*fault-alarms-logging / parameter-values*

**Q.** When we inhibit alarm reporting on something, do the alarms that are already raised disappear or stay up?

**A.** That is controlled system wide by arc-behavior on alarm-control. leave-alarms, which is the default, leaves current alarms in a raised mode when ARC is set to inhibit; clear-alarms clears them instead. The same object carries alarm-soaking-behavior, either automatic, where the soaking time comes from the FM profile, or no-soak, defaulting to automatic.

Source: `06-operation-commands/015-alarm-control.md`

### alarm-inventory-meaning

*fault-alarms-logging / which-command*

**Q.** I have an alarm code I do not recognise. Where do I find out what it means and what to do about it?

**A.** Use `show alarm-inventory-<alarm-type>`, which holds static information for every alarm type the system can raise. alarm-type-description explains the alarm, corrective-action is the system provided guidance on how to correct the situation that triggered it, and default-severity lists the possible default severities (critical, major, minor, warning, not-reported, event). alarm-category places it as communication, facility, equipment, environmental, processing-error, software, quality-of-service or security, and service-affecting says whether it affects service (indeterminate, sa, nsa or sa-nsa). The same alarm can have different severities and service impact depending on the resource-type it applies to.

Source: `06-operation-commands/016-alarm-inventory.md`

### alarm-severity-entry-change

*fault-alarms-logging / how-to*

**Q.** Can I downgrade one particular alarm on one card type without touching anything else?

**A.** Yes. alarm-severity-entry configures the severity of one particular alarm and is addressed per resource-type, alarm-type, direction and location: `set alarm-severity-entry-<resource-type>/<alarm-type>/<direction>/<location> severity <value>`. Severity accepts critical, event, major, minor, not-reported and warning. Direction is na, ingress or egress; location is na, near-end or far-end. The object also shows service-affecting. It is system managed and cannot be manually deleted, and pressing ? after the command lists the available entities.

Source: `06-operation-commands/017-alarm-severity-entry.md`

### alarm-severity-profile-set

*fault-alarms-logging / comparison*

**Q.** How does alarm-severity-profile differ from setting the severity entry by entry?

**A.** alarm-severity-profile assigns a severity to a named profile entry in a single command: `set alarm-severity-profile <profile-entry> severity <critical|event|major|minor|not-reported|warning>`, for example set alarm-severity-profile alarm-severity-entry-trusted-certificate/CERTIFICATE-EXPIRED severity critical. show alarm-severity-profile lists them. Like alarm-severity-entry it is system managed and cannot be manually deleted, and pressing ? after the command displays the available profiles.

Source: `06-operation-commands/018-alarm-severity-profile.md`

### current-alarms-count

*fault-alarms-logging / which-attribute*

**Q.** How do I get a quick count of how many alarms are up right now and when the list last changed?

**A.** `show current-alarms` lists the currently raised alarms and exposes two attributes: number-of-alarms, the number of currently raised alarms, and last-changed, the timestamp of the last change in the current alarm list, meaning either a raise or a clear event. It is available in operational and candidate configuration mode.

Source: `06-operation-commands/066-current-alarms.md`

### get-conditions-hidden-alarms

*fault-alarms-logging / troubleshooting*

**Q.** I expected an alarm to be raised but it is not in the alarm list. Where else should I look?

**A.** Look at conditions, using get-conditions. A condition is an alarm that is not considered current, which happens when the alarm severity is configured as not-reported or not-alarmed, or the alarm is suppressed by alarm correlation, by ARC, or by AINS. get-conditions can be filtered by direction (all, na, ingress, egress; default all), resource, resource-type, alarm-type, location (all, na, near-end, far-end; default all) and AID. It runs in operational mode.

Source: `06-operation-commands/118-get-conditions.md`

### set-alarm-state-bulk-ack

*fault-alarms-logging / how-to*

**Q.** How do I acknowledge a batch of alarms and leave a note saying who looked at them?

**A.** Use set-alarm-state, which changes the operator state of an alarm. The state is mandatory and the user can select none, ack or close. Target either every raised alarm with the all-alarms option, or a specific set with alarm-id-list, which takes from 1 up to 10 alarm ids, for example `set-alarm-state ack 28872914984089790,17580406225060810165`. The optional acknowledge-text parameter stores a message of up to 256 characters in the alarm. Setting an alarm to the state it is already in is accepted, with only the text updated. It runs in operational mode.

Source: `06-operation-commands/288-set-alarm-state.md`

### statistics-clear-scope

*fault-alarms-logging / scope-limit*

**Q.** Can I reset the event counters on the node, and what can actually be reset?

**A.** Only partly. `clear statistics [target=]<value>` clears the event counters for the specified objects, but the guide states that currently the supported object type is aaa-server, for example `clear statistics aaa-server`. It also notes AAA statistics are supported for TACACS+ servers but not for RADIUS servers, so there is nothing to clear for a RADIUS server. The command runs in operational mode and takes -f to skip confirmation.

Source: `06-operation-commands/311-statistics.md`

### log-read-last-entries

*fault-alarms-logging / how-to*

**Q.** How do I read just the last few entries of the security log in something more readable than CSV?

**A.** Use `show [-n=<number>] [-t] log <logname>`, for example show -n=50 -t log security. By default log entries come out in CSV with each column named in the header; -t switches to an aligned table, which is easier to read but can make the table too wide, in which case unwanted columns are removed with exclude=<column> after the logname. Available lognames include messages, linecard, shell-command, netconf-command, apps, cli-command, alarm, configuration, kernel, user, event, security and ztp, and running show log with no logname lists what is available. Output can be piped, for example show log security | exclude john | include NETCONF. Note that clear log empties a log file but the security log may not be cleared.

Source: `06-operation-commands/163-log.md`

### log-console-enable

*fault-alarms-logging / how-to-default*

**Q.** Nothing is being logged to the console. Is that expected?

**A.** Yes, console logging is off by default. log-console has an enabled flag of true or false that defaults to false, so turn it on with `set log-console enabled true`. source-facilities selects which syslog facilities appear there, from a list including all, authentication, kernel, security, ntp, local1 through local7, system-daemons, user-level and others.

Source: `06-operation-commands/164-log-console.md`

### log-console-filter-severity

*fault-alarms-logging / parameter-values*

**Q.** How do I stop the console filling up with informational messages and show only serious ones?

**A.** Add a log-console-facility-filter for the facility and set its severity. The severity levels follow syslog: emergency is level 0, alert 1, critical 2, error 3, warning 4, notice 5, informational 6 and debug 7, and the attribute defaults to informational. compare-op decides how the comparison is applied, being equals, equals-or-higher or not-equals, and defaults to equals-or-higher. So severity critical with the default compare-op passes critical and anything more severe. The filter name identifies a single syslog facility, or all of them if the value is all.

Source: `06-operation-commands/165-log-console-facility-filter.md`

### log-file-rotation

*fault-alarms-logging / parameter-values*

**Q.** How much local log history does the node keep before it starts throwing entries away?

**A.** That is set per file on log-file. number-of-files is the maximum number of log files retained, range 1 to 20, default 10; when rotating because the maximum size is reached, the oldest files are discarded once the total exceeds that number. max-file-size is the maximum file size before rotation in megabytes, range 1 to 30, default 30. So by default up to ten files of up to 30 MB each. pattern-match can restrict entries to a regex, and sensitive-data selects none, both or only. The default system log file cannot be edited or deleted.

Source: `06-operation-commands/166-log-file.md`

### log-file-facility-filter-scope

*fault-alarms-logging / how-to*

**Q.** Can I make one of our local log files carry only authentication messages?

**A.** Yes, with a log-file-facility-filter, which is addressed per file: add log-file-facility-filter-<log-file-name>/<log-file-facility-filter-name>. The filter name identifies a single syslog facility, authentication among them, or all of them if the value is all. severity defaults to informational and compare-op defaults to equals-or-higher.

Source: `06-operation-commands/167-log-file-facility-filter.md`

### log-server-remote-forwarding

*fault-alarms-logging / parameter-values*

**Q.** We want to ship logs to a remote collector over TLS. What port and message format does the node use unless I say otherwise?

**A.** Configure a log-server with an address, a port and a transport. transport is tcp, udp or tls and defaults to udp, so TLS has to be set explicitly. port covers the range 1 to 65535 and defaults to 514. message-format is rfc3164 or rfc5424 and defaults to rfc5424. sensitive-data selects none, only or both and defaults to none. Other attributes include enabled, message-coalescence, pattern-match, source-facilities, destination-facility-override, origin (dhcp or manual) and alarm-report-control.

Source: `06-operation-commands/168-log-server.md`

### log-server-facility-filter-limit

*fault-alarms-logging / scope-limit*

**Q.** Can I filter what gets forwarded to one particular remote collector, and can that filter widen what it sends?

**A.** You can filter per server with a log-server-facility-filter, addressed as log-server-facility-filter-<log-server-name>/<log-server-facility-filter-name>, carrying severity and compare-op like the other filters. It cannot widen the selection: the guide states the filter is based on the source-facilities leaf-list and can only add a filter to the configured source facilities, so it narrows what the log-server is already sending.

Source: `06-operation-commands/169-log-server-facility-filter.md`

### syslog-disable-remote

*fault-alarms-logging / how-to-default*

**Q.** How do I switch off all remote logging in one go without deleting the collectors we have configured?

**A.** Set the master switch on syslog: `set syslog remote-logging-switch false`. The guide states that if false it disables all remote logging destinations; it defaults to true, and the configured log-servers are left in place. Related attributes on the same object are source-address, inserted into the HOST field of the log message and defaulting to localhost, log-file-message-coalescence (default true) which collapses repeated identical messages into a 'last message repeated n times' line, log-relay (default false) for forwarding from a shelf controller to a node controller, and assignment-method (manual, dhcp or both, default both).

Source: `06-operation-commands/339-syslog.md`

### aaa-server-shared-secret-length

*security-access-control / contradiction*

**Q.** How long can the shared secret for an authentication server be?

**A.** The guide is inconsistent on this point and you should treat 64 as the working limit. A note on the command states the maximum length of shared-secret is 64 characters and that any value longer than 64 is denied with an error message, while the parameter table gives the type as String (length 0...128). Take the note as authoritative for what the node accepts. The value is displayed as asterisks rather than in clear. Related defaults on the same object: protocol-supported is RADIUS or TACACSPLUS, defaulting to RADIUS; timeout is 1 to 90 seconds, default 5; and retry is 0 to 5.

Source: `06-operation-commands/001-aaa-server.md`

### aaa-statistics-counter-jump

*security-access-control / interpretation*

**Q.** Our TACACS+ authentication request counter goes up by two for a single login attempt. Is that a bug?

**A.** No, that is expected. The guide notes that for TACACS+ the default authentication protocol includes both PAP and CHAP, and the authentication requests counter tracks each retry and each authentication protocol attempted independently, so authentication requests may increase by 2 for each retry. show aaa-statistics-<server-name> also exposes connection-failures, which covers failures from unavailable servers and timeouts, plus authentication-rejects, authorization-requests, authorization-rejects and accounting-requests. The command only applies to servers using TACACS+.

Source: `06-operation-commands/002-aaa-statistics.md`

### ace-default-action-and-limits

*security-access-control / how-to-default*

**Q.** If I add a packet filter rule and do not specify what to do with a match, what happens, and how many rules can I have?

**A.** The action attribute defaults to drop, so an unspecified rule drops matching traffic; the other values are accept and reject. At least one of source IP, destination IP, L4 ports, TTL or protocol has to be given to add an ACE at all. On limits, a maximum of 16 ACEs can be created on an ACL if both upper-port and lower-port values are specified, and attempting a seventeenth in that form returns an error. sequence-id supports 1 to 100 for AUX and DCN-B interfaces and 1 to 256 for DCN-A, and the guide recommends using multiples of 5 so rules can be inserted later. direction defaults to input and logging-action to false.

Source: `06-operation-commands/006-ace.md`

### acl-interfaces-and-type

*security-access-control / enumeration*

**Q.** Which management interfaces can I attach a packet filter to, and does it handle IPv6?

**A.** An acl is attached to one interface, chosen from 1-AUX-1, 1-AUX-2, DCN, DCN-B, DCN-2 and DCN-2-B; the guide suggests `add acl-name interface ?` to see what is available on the node. type selects the top-level ACL type and is IPv4 or IPv6, so both are supported but a given ACL matches one of them. The name is up to 30 characters and admin-state is lock, maintenance or unlock, defaulting to unlock. Each ACL holds one or more ACEs.

Source: `06-operation-commands/007-acl.md`

### access-control-list-view

*security-access-control / minimal-command  (weak: the source section is thin)*

**Q.** Is there a single command that shows the access control lists on the node?

**A.** `show access-control-list` displays the access control list. It takes no parameters and runs in operational or candidate configuration mode. For the detail, use acl for each list's own attributes and ace for the individual entries within it.

Source: `06-operation-commands/003-access-control-list.md`

### access-rule-deny-path

*security-access-control / how-to*

**Q.** How do I write a rule that stops a group from writing to one part of the data model?

**A.** Add an access-rule inside an access-rule-list: `add access-rule-<access-rule-list-name>/<access-rule-name> action deny path <xpath>` plus the operation the rule covers. action is permit or deny and is mandatory whenever an access rule is created. path is the target object and may be the XPath of a YANG data node, notification, RPC or descendant, or an external command. module-name limits the rule to one YANG module and defaults to '*', meaning any module. operation lists the operations the rule applies to, with '*' meaning all operations as the default. sequence-id orders the rule within its list.

Source: `06-operation-commands/004-access-rule.md`

### access-rule-list-order

*security-access-control / interpretation*

**Q.** If I have several rule lists, which one wins, and how do I say which groups a list applies to?

**A.** An access-rule-list is a user created group of access-rules organised by the user-groups the rules apply to, and it is processed in order as given by the sequence-id parameter, with lower numbered ids processed first. If sequence-id is not provided it is set to the currently used latest id plus 1, so the new entry goes to the end of the list; ids can be changed later to re-sort entries. user-group is the list of groups the rules apply to, defaulting to '*', which is a match-all meaning it applies to all existing user-groups, and a maximum of 20 user-groups can be referenced.

Source: `06-operation-commands/005-access-rule-list.md`

### auth-key-is-ospf-scoped

*security-access-control / disambiguation*

**Q.** The auth-key command sounds generic. What is it actually for?

**A.** Despite the generic name it is OSPF specific. auth-key adds, edits or shows an authorization key scoped to an OSPF interface: the instance is auth-key-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi>, where instance-id is the OSPF instance ID in the range 0 to 255, ospf-area-id is the OSPF router area ID, ospf-if-name references the interface in the OSPF area, and spi is the security parameter index. It carries key and type, and the delete command removes it. For NTP authentication keys use ntp-key instead, and for user credentials use user and password.

Source: `06-operation-commands/026-auth-key.md`

### authorization-rules-only-mode

*security-access-control / parameter-values*

**Q.** How do I make the node obey only the access rules we have written, and deny anything they do not cover?

**A.** Set mode to rules-only on the authorization object. The three modes are static-only (only system defined static rules), static+rules (both, trying a user configured access-rule first and falling back to the system rules, and this is the default) and rules-only (only user defined access-rules, falling back to the global defaults when nothing matches). Those global defaults are read-default, write-default and exec-default, each permit or deny, and the guide notes they are only configurable when mode is rules-only. Their defaults are permit for read, deny for write and permit for exec, so writes are already denied by default in that mode. The object also counts denied-operations, denied-data-writes and denied-notifications.

Source: `06-operation-commands/027-authorization.md`

### password-change-own-vs-reset

*security-access-control / comparison*

**Q.** How does a user change their own password, and how would an administrator reset someone else's?

**A.** A user changes their own with the password command, supplying both the current and the new password. This is supported only for local users and a password may contain up to 200 characters. Input works two ways: inline as normal parameters, which is scriptable but shows the passwords on screen, or interactively, where they are prompted for and not echoed and the new one must be typed twice, cancellable with Ctrl+C. Inline input follows the standard string rules, so double quotes and single quotes must be escaped with a backslash and characters such as # ? | ; must be enclosed in quotes. For another user, an administrator does not use this command: the guide says to use set user-<username> force-password-change true. The command runs in operational mode.

Source: `06-operation-commands/233-password.md`

### security-container-view

*security-access-control / minimal-command  (weak: the source section is thin)*

**Q.** Which command shows the top level security settings container?

**A.** `show security` displays the top level security container. It takes no parameters and runs in operational or candidate configuration mode. The configurable policies underneath it are on security-policies, and the user and group objects are user, user-group and access-rule-list.

Source: `06-operation-commands/279-security.md`

### security-policies-password-rules

*security-access-control / parameter-values*

**Q.** What is the minimum password length on this platform, and how many old passwords does it remember?

**A.** Both are on security-policies. minimum-password-length is the configurable minimum length for user passwords, range 1 to 200, default 8, and it is checked when a password is changed. password-history-size is the number of passwords stored for reuse checking, range 1 to 200, default 5, used together with enforce-password-history-check. strict-password-check, when enabled, additionally enforces complexity rules including a minimum length of 8 characters and at least one lower case and one upper case letter. The same object holds secure-mode, which when enabled stops non-secure protocols such as HTTP being used, plus the SSH and TLS algorithm lists, default-user-group and disable-user-lockout.

Source: `06-operation-commands/280-security-policies.md`

### user-lockout-thresholds

*security-access-control / parameter-values*

**Q.** How many failed logins lock an account here, and how long does it stay locked?

**A.** On the user object, max-invalid-login is the maximum number of consecutive invalid login attempts before the account is suspended, range 0 to 255, default 5. suspension-time is how long the suspension lasts, in minutes, range 0 to 1440, default 5. Related defaults on the same object: timeout, the session time out interval, is 0 to 1440 minutes with a default of 60; password-aging-interval is 0 to 999 days with a default of 90; max-sessions is 1 to 20 with a default of 10; and force-password-change defaults to false. show user also exposes user-status, last-login-date and failed-logins.

Source: `06-operation-commands/367-user.md`

### user-data-view

*security-access-control / minimal-command  (weak: the source section is thin)*

**Q.** What does the user-data command give me?

**A.** `show user-data` displays the user-data container. The guide documents no parameters for it and gives no further detail, listing only the show form and the access mode, which is operational and candidate configuration. For actual user accounts and their attributes use user, and for groups use user-group.

Source: `06-operation-commands/368-user-data.md`

### user-group-create

*security-access-control / how-to*

**Q.** How do I create a group to hang permissions off, and what are the limits on the name?

**A.** Use `add user-group-<name> [description <value>]`. The name is a string of 1 to 64 characters and the description is 1 to 128 characters. Each user is associated with a list of groups and derives its permissions from them, and the delete command removes a group. To attach rules to the group, reference it from an access-rule-list's user-group attribute.

Source: `06-operation-commands/369-user-group.md`

### adg-numbering-and-duplication

*optical-layer0 / parameter-values*

**Q.** How many add/drop groups can a node have, and can the same wavelength appear twice in one of them?

**A.** The adg-number is an integer in the range 1 to 110, so up to 110 add/drop groups can be addressed. Duplication is governed by wavelength-duplication, which defaults to one-per-adg, meaning no duplication of frequencies within the ADG. The alternative, one-per-degree, allows duplicate frequencies in the ADG with only one at the degree, and the guide notes only CDCs allow more than one instance of the same wavelength on the ADG. bands-supported lists the transmission bands the ADG can carry and defaults to standardC-band. Add one with `add adg-1`.

Source: `06-operation-commands/012-adg.md`

### amplifier-gain-and-tilt-control

*optical-layer0 / parameter-values*

**Q.** What gain can an amplifier be asked for, and does the system control tilt by itself?

**A.** gain-target covers 0 to 40 dB and defaults to 0.0, with gain-adjustment allowing a trim of -20.00 to 20.00. gain-range-control is auto or manual, defaulting to auto, and gain-range-target is standard, low or high. Tilt is controlled by tilt-control-mode, which is auto by default, meaning the system implicitly controls amplifier tilt per the configured value; set it to manual to control tilt yourself. tilt-target and tilt-adjustment each accept -5 to 5 dB over the operating wavelength band and default to 0dB. Other useful defaults on the same object: amplifier-enable defaults to disabled, forced-shutdown to false, control-mode to auto-max-pw, span-loss-control to enabled and control-speed-factor to 1.00.

Source: `06-operation-commands/019-amplifier.md`

### amplifier-raman-gain-target

*optical-layer0 / parameter-values*

**Q.** What Raman gain can I ask for, and how do I know how many pumps the card has?

**A.** target-raman-gain accepts 0 or the range 5 to 30 dB and defaults to 0dB, so there is a gap between zero and the minimum usable gain. number-of-pumps reports 4 or 2. raman-state reads disabled, disabled-from-remote or enabled, and amplifier-enable is disable-local-and-remote (the default), disable-local or enabled, so a Raman amplifier can be held off from either end. control-mode is auto, manual or auto-planned, defaulting to auto. connected-amplifier references the partner EDFA in the range 1 to 20 or not-specified, and connected-amp-edfa-optimum-gain takes 1 to 55dB. actual-raman-signal-gain and actual-raman-osc-gain report what is being delivered, and total-pump-power runs from -99 to 99 dBm.

Source: `06-operation-commands/020-amplifier-raman.md`

### ase-idler-service-enable

*optical-layer0 / how-to-default*

**Q.** How do I switch on ASE loading, and how can I see whether it actually came up?

**A.** Set ase-idler-enable to enabled on the ase-idler-service, for example `set ase-idler-service-1-8 ase-idler-enable enabled`; it defaults to disabled. The result is reported by ase-idler-state, which reads ase-enabled, ase-partially-enabled, ase-faulted or ase-disabled, defaulting to ase-disabled, so ase-partially-enabled and ase-faulted are the states to watch for. The object is created and deleted with add and delete, and function is fixed at idler.

Source: `06-operation-commands/024-ase-idler-service.md`

### ase-idler-source-output-power

*optical-layer0 / parameter-values*

**Q.** What output power can the ASE source be set to, and is its pump on by default?

**A.** target-output-power accepts -3.00 to 20.50 dBm and defaults to 13. The pump is off by default: pump-enable defaults to disabled, and pump-state reports the actual state, also defaulting to disabled. A typical command is `set ase-idler-source-1-3.1-ase pump-enable 'enabled' target-output-power '15.00'`. admin-state is lock, unlock or maintenance with unlock as the default, and function is fixed at ase-idler-source.

Source: `06-operation-commands/025-ase-idler-source.md`

### calibrate-raman-gain

*optical-layer0 / how-to*

**Q.** How do I kick off a Raman gain calibration, and how do I stop one?

**A.** Use the calibrate command with three parameters: type, trigger and entity. type is raman, trigger is start or stop, and entity is the instance identifier to act on, for example `calibrate raman start ots-r-1-1-dwdm-line`. The same command with trigger stop halts it. calibrate runs in operational mode only.

Source: `06-operation-commands/036-calibrate.md`

### degree-grid-granularity

*optical-layer0 / parameter-values*

**Q.** How many degrees can a node have, and what spectral granularity does a degree use?

**A.** degree-number is an integer in the range 1 to 20. The spectral grid is described by slot-width-granularity, defaulting to 6250 MHz, and center-freq-granularity, defaulting to 3125 MHz, with min-slots defaulting to 8 and max-slots to 32. wss-less defaults to true and is-foadm to false. bands-supported defaults to standardC-band. Create one with `add degree-1`.

Source: `06-operation-commands/076-degree.md`

### direction-index-assignment

*optical-layer0 / interpretation*

**Q.** On a multi-rail in-line amplifier node, why do the directions come in odd and even pairs?

**A.** Because the system assigns them that way. Direction n and direction n+1 are automatically assigned to the DWDM Line1 and DWDM Line2 ports of the nth C2ILASGH/D2ILASGM card created, where n is an odd integer between 1 and 15. The DWDM Line1 port always gets an odd index and DWDM Line2 always gets an even index. The index itself is an integer in the range 1 to 16 and direction-port is port-<shelf>-<slot>-dwdm-line1 or dwdm-line2. Directions are created with add direction-<index> direction-port <value> and removed with delete, which takes -f.

Source: `06-operation-commands/081-direction.md`

### dsc-managed-by-system

*optical-layer0 / which-attribute  (weak: the source section is thin)*

**Q.** What can I configure on a dsc facility, and what does its managed-by attribute tell me?

**A.** add, set and delete accept label, admin-state and alarm-report-control only; everything else is read through show. managed-by reads system or user and defaults to system, which tells you whether the node created the facility itself or you did. The rest of the facility set is the usual one: admin-state (lock, unlock or maintenance, default unlock), oper-state (enabled or disabled, default disabled), avail-state, alarm-report-control (allowed or inhibited, default allowed), supporting-card, supporting-port and the supporting and supported facility lists. Note the guide never expands the abbreviation DSC nor says what the facility represents.

Source: `06-operation-commands/089-dsc.md`

### dsc-group-q-thresholds

*optical-layer0 / parameter-values*

**Q.** What signal degrade thresholds can I set on a carrier group, before and after FEC?

**A.** dsc-group carries both. pre-fec-q-sig-deg-threshold covers roughly 5.600 to 9.600 dB with pre-fec-q-sig-deg-hysteresis in the range 0.1 to 1.0 dB, default 0.5. post-fec-q-sig-deg-threshold runs 12.5 to 18.0 dB with a default of 18, and post-fec-q-sig-deg-hysteresis is again 0.1 to 1.0 dB, default 0.5. There is also dgd-high-threshold, in the range 25 to 400 ps, default 100. A group is created with carriers and rate mandatory, for example `add dsc-group-1-3-1-1 rate 100 carriers 1-3-1 group-id 1 instance-id 1`.

Source: `06-operation-commands/090-dsc-group.md`

### gadt-application-descriptions

*optical-layer0 / which-command*

**Q.** Where do I look up the golden carrier application information the node supports?

**A.** `show gadt` retrieves the golden carrier application information. It optionally takes one of application-description-H, application-description-P, application-description-S or application-description-U, for example show gadt application-description-H. Related lookups are gapt for the Golden Advanced Parameters Table and gcmt for golden carrier modes.

Source: `06-operation-commands/115-gadt.md`

### gapt-per-card-type

*optical-layer0 / which-command*

**Q.** What does the gapt command list and how is it scoped?

**A.** gapt lists the golden advanced parameters from the Golden Advanced Parameters Table. It is scoped per card type, `show gapt-<card-type> [version] [applicable-resource-type]`, and running show gapt with no card type lists what is there. The related per-instance objects are advanced-parameter, current-advanced-parameter and golden-advanced-parameter.

Source: `06-operation-commands/116-gapt.md`

### gcmt-carrier-mode-lookup

*optical-layer0 / which-command*

**Q.** How do I look up the details of a specific golden carrier mode such as 800M.95P?

**A.** `show gcmt [<card-type>] [version]` retrieves information about the golden carrier mode, for example show gcmt golden-carrier-mode-800M.95P. For the configured carrier mode of an instance, use golden-carrier-mode, which exposes actual-carrier-mode, capacity, client-mode, baud-rate, application, compatibility-id and status.

Source: `06-operation-commands/117-gcmt.md`

### golden-carrier-mode-status

*optical-layer0 / enumeration*

**Q.** How do I tell whether a carrier mode is fully supported or only experimental?

**A.** The status attribute on golden-carrier-mode reads supported, candidate, experimental, deprecated or diagnostic, so anything other than supported deserves a second look before you use it in service. The same object shows actual-carrier-mode, capacity, client-mode (ethernet or ethernet-otn), baud-rate, application, compatibility-id, sop-tracking-mode, supported-subtypes and candidate-subtypes. It is addressed as golden-carrier-mode-<card-type>/<carrier-mode>.

Source: `06-operation-commands/120-golden-carrier-mode.md`

### l0-capabilities-view

*optical-layer0 / minimal-command  (weak: the source section is thin)*

**Q.** Which command summarises what the node can do at the photonic layer?

**A.** `show l0-capabilities` shows the capabilities details related with the node's Layer 0 functions. It takes no parameters and runs in operational or candidate configuration mode. For the add/drop and degree limits specifically, oadm-capabilities reports max-degrees and max-adgs.

Source: `06-operation-commands/151-l0-capabilities.md`

### mc-create-frequency-bounds

*optical-layer0 / how-to*

**Q.** What do I have to supply to carve out a media channel?

**A.** Three things are mandatory on add: parent-oms, lower-frequency and upper-frequency, so the channel is defined by its spectral edges rather than a centre and width. Optional attributes are label, admin-state (lock, unlock or maintenance, default unlock), alarm-report-control (default allowed) and auto-delete, which defaults to disabled. managed-by shows whether the system or the user created it. The same edges can be changed later with set.

Source: `06-operation-commands/178-mc.md`

### mc-f-filler-readonly

*optical-layer0 / scope-limit*

**Q.** Can I configure the media channel filler, or is it read only?

**A.** The guide documents only a show form for mc-f: `show mc-f-<name>` with supporting-facilities, supported-facilities, supporting-card, supporting-port, AID, lower-frequency, upper-frequency and slot-width. There is no set, add or delete syntax listed, so treat it as read only and configure the surrounding media channel with mc instead. Note the description text calls it the Media Channel Filler but abbreviates it NMC-F.

Source: `06-operation-commands/179-mc-f.md`

### modules-adg-slots

*optical-layer0 / parameter-values*

**Q.** How many cards can I put into one add/drop group, and is channel monitoring on by default?

**A.** Up to four: modules-adg is addressed as modules-adg-<adg-number>/<index> where index is a number in the range 1 to 4, and adg-number covers 1 to 110. supported-card is mandatory on add, for example `add modules-adg-1/1 supported-card card-1-6`. ocm-monitoring defaults to true, so optical channel monitoring is on unless you disable it.

Source: `06-operation-commands/181-modules-adg.md`

### modules-degree-attach-card

*optical-layer0 / how-to*

**Q.** How do I associate a line card with a degree?

**A.** Add a modules-degree entry: `add modules-degree-<degree-number>/<index> supported-card <value>`, for example add modules-degree-1/1 supported-card card-1-1. degree-number is a number in the range 1 to 20. supported-card is the only attribute, and set changes it while delete removes the association. The equivalent for add/drop groups is modules-adg, which additionally carries ocm-monitoring.

Source: `06-operation-commands/182-modules-degree.md`

### monitored-channel-power

*optical-layer0 / which-attribute*

**Q.** How do I read the measured power of one channel at a monitoring point?

**A.** Show the monitored-channel for that point and frequency: the instance is monitored-channel-<name>/<frequency>, for example `show monitored-channel-1-1.2-ocm1-in/195987500`. It exposes monitored-optical-power, a decimal in the range -99.00 to 99.00 defaulting to -99, and monitored-width, defaulting to 0. A value of -99 therefore means nothing has been measured rather than a real reading.

Source: `06-operation-commands/183-monitored-channel.md`

### nmc-frequency-range-and-width

*optical-layer0 / parameter-values*

**Q.** What frequency range and channel width can a network media channel take?

**A.** center-frequency accepts values in the range 190650000 to 196700000 MHz and is mandatory on add along with parent-facility. width covers 15000 to 200000 MHz and defaults to 35000. A typical creation is `add nmc-cdc-1-4_192800000-34000 center-frequency 192800000 width 34000 parent-facility oms-1-4-ad1`. Power targets and attenuation are set through input-power-min, input-power-max, input-attenuation-target and output-attenuation-target, the last two defaulting to max, and ASE loading is controlled by ase-insertion-enable, which defaults to disabled with an ase-insertion-soak-timer of 5 seconds.

Source: `06-operation-commands/193-nmc.md`

### nmc-f-default-bandwidth

*optical-layer0 / how-to-default*

**Q.** What bandwidth does a network media channel filler occupy by default, and is it monitored?

**A.** alloc-bandwidth defaults to 75000 MHz, while actual-bandwidth reports what is really in use and starts at 0. monitoring-state defaults to enabled on nmc-f, unlike several other optical objects where it defaults to disabled. The filler is configurable to a limited extent: set accepts admin-state and alarm-report-control only, and everything else including the allocated and actual frequency edges and the tx and rx powers is read through show.

Source: `06-operation-commands/194-nmc-f.md`

### oadm-capabilities-limits

*optical-layer0 / which-command*

**Q.** How do I find the maximum number of degrees and add/drop groups this node supports?

**A.** `show oadm-capabilities` reports max-degrees and max-adgs. Those are the node's own limits, which sit inside the wider addressing ranges of 1 to 20 for degree-number and 1 to 110 for adg-number. For the broader photonic capability set use l0-capabilities.

Source: `06-operation-commands/200-oadm-capabilities.md`

### oc-loopback-and-test-signal

*optical-layer0 / parameter-values*

**Q.** What loopbacks and test patterns can I put on an optical carrier?

**A.** loopback accepts facility, terminal or none and defaults to none, with loopback-mode selecting facility or terminal. For test patterns, test-signal-type offers none, enumeration, PRBS31Q, PRBS13Q, scrambled-idles, PRBS9 and PRBS31, defaulting to none. test-signal-direction is ingress and test-signal-monitoring is true or false, defaulting to false. Trace identifiers are handled by tti-style (ITU-T-G709 by default, or proprietary), tx-tti and expected-tti, both tti-64, with tim-monitor defaulting to false.

Source: `06-operation-commands/201-oc.md`

### ochm-ila-derivation

*optical-layer0 / interpretation*

**Q.** On an in-line amplifier node, where does the optical channel monitoring information come from?

**A.** From the OSC. The guide states that in ILA nodes the OCHm represents the signalled optical channel from OSC, detected at the ILA OMS by using the OSC information. That means the OMS monitoring-mode matters: when it is set to not-monitored, or the required information is absent, the derivation cannot be made. The ochm object exposes direction (ingress or egress, default egress), power-actual, target-actual-power and attenuation-actual, all defaulting to -99 or n/a until measured, and set accepts only label, admin-state and alarm-report-control.

Source: `06-operation-commands/202-ochm.md`

### ocm-channel-detected-carriers

*optical-layer0 / which-command*

**Q.** How do I see which carriers the monitor is actually detecting inside a cross connection?

**A.** Use ocm-channel, which lists the detected carriers within the configured OXcon or OXcons. The instance is ocm-channel-<name>/<lower-frequency>/<upper-frequency>, so each entry is a spectral slice. It reports opm-pwr, a decimal from -99.00 to 99.00 defaulting to -99, and connected, true or false defaulting to false, which tells you whether the detected carrier is associated with a connection.

Source: `06-operation-commands/203-ocm-channel.md`

### ocm-mp-enabled-by-default

*optical-layer0 / how-to-default*

**Q.** Is optical channel monitoring on by default at a monitoring point?

**A.** Yes for ocm-mp: ocm-enable defaults to enabled and monitoring-state also defaults to enabled. That is worth noting because the closely named ocm-ptp defaults the other way, with ocm-enable and monitoring-state both disabled. ocm-mp also shows monitored-port, which is not-applicable until an instance identifier is bound, and ad-direction, which is ingress.

Source: `06-operation-commands/204-ocm-mp.md`

### ocm-ptp-ws04s-scope

*optical-layer0 / scope-limit*

**Q.** Which cards get a dedicated optical channel monitor, and is it enabled out of the box?

**A.** The guide states the ocm-ptp facility is available for WS04S cards within a CD-AD ADG and provides dedicated OCM monitoring, so it is card and configuration specific rather than general. It is off by default: ocm-enable defaults to disabled and monitoring-state to disabled. last-measurement reads never until a measurement is taken, ad-direction defaults to ingress, and adg-number covers 0 to 110.

Source: `06-operation-commands/205-ocm-ptp.md`

### oms-monitoring-mode-and-grid

*optical-layer0 / parameter-values*

**Q.** What spectral grid does a multiplex section use by default, and what does its monitoring mode change?

**A.** grid-mode defaults to flexible, with fixed alternatives including fixed-50G-96ch, fixed-100G-48ch, fixed-75G-64ch, fixed-75G-64ch-oif and fixed-50G-7100. The default spectrum runs from lower-frequency 191300000 to upper-frequency 196150000. monitoring-mode selects how the section is monitored, with intrusive as the default on all node types except ILA and not-monitored otherwise; not-monitored means non-intrusive monitoring with no OCM required, which is why it also affects whether ochm data can be derived. target-output-power accepts 5.00 to 20.00 dBm and defaults to 5, attenuation-control-mode-rx and -tx both default to auto, and control-speed-factor defaults to 1.00.

Source: `06-operation-commands/209-oms.md`

### ops-role-and-psd-profile

*optical-layer0 / which-attribute*

**Q.** What can I actually configure on an optical physical section?

**A.** set accepts label, admin-state, alarm-report-control, input-psd-profile, role and port-expansion. role is the section's configured role and supported-roles shows which roles the port can take, so check the latter before setting the former. input-psd-profile selects the expected input power spectral density profile. Everything else, including supporting-card, supporting-port, the supporting and supported facility lists, oper-state and avail-state, is read only. admin-state defaults to unlock.

Source: `06-operation-commands/210-ops.md`

### optical-carrier-frequency-and-power

*optical-layer0 / parameter-values*

**Q.** What frequency range and transmit power can I set on a coherent carrier, and can I trim the frequency slightly?

**A.** frequency covers roughly 191275000 to 196125000 MHz, and frequency-offset lets you trim it by -6000 to 6000 MHz with a default of 0. tx-power accepts -55.0 to 55.00 dBm, but the guide is explicit that the usable range depends on the module and pluggable, so check the per-module description rather than assuming the full range. grid-spacing offers 100, 75, 50, 33, 25, 12.5, 6.25 and 3.125, defaulting to 100. rx-frequency and actual-rx-frequency mirror the transmit side, and rx-attenuation covers 0.0 to 10.0 dBm.

Source: `06-operation-commands/211-optical-carrier.md`

### optical-carrier-shutdown-propagation

*optical-layer0 / how-to-default*

**Q.** Can I make the transmit laser shut down when the service in that direction fails?

**A.** Yes, that is propagate-shutdown on the optical carrier: when set to enabled the transmit laser is shut down if the whole service of the direction has signal failure. It defaults to disabled. propagate-shutdown-holdoff-timer delays the action by 0 to 2000 milliseconds and defaults to 0, so without a hold-off the shutdown is immediate. Related dispersion and polarisation settings on the same object are cd-compensation-mode (auto by default), cd-range-low and cd-range-high, fast-sop-mode (disabled by default) and sop-data-collection, which is disabled or a number from 10 to 500 ms.

Source: `06-operation-commands/211-optical-carrier.md`

### optical-channel-limited-config

*optical-layer0 / scope-limit  (weak: the source section is thin)*

**Q.** What can I change on an optical-channel object?

**A.** Very little. set accepts only label and admin-state; the guide lists no add or delete form, and everything else is read through show: supporting-card, supporting-port, the supporting and supported facility lists, AID, oper-state, avail-state and managed-by. admin-state defaults to unlock, oper-state to disabled and managed-by to system, so these objects are normally created by the node rather than by you. A typical command is `set optical-channel-channel1 admin-state unlock`.

Source: `06-operation-commands/212-optical-channel.md`

### optical-ptp-laser-and-attenuation

*optical-layer0 / which-attribute*

**Q.** How do I check whether the laser on a DWDM port is on, and can I add fixed pad on the receive side?

**A.** laser-state on the optical-ptp reads enabled or disabled and defaults to disabled, so it tells you directly whether the port is lasing. Fixed attenuation is set with fix-rx-attenuation and fix-tx-attenuation, each 0 to 30 dB defaulting to 0, for example `set -f optical-ptp-1-6-dwdm fix-rx-attenuation 10`. The measured levels are power-actual-rx and power-actual-tx, each -99.00 to 99.00 dBm defaulting to -99dBm, and actual-power-support tells you whether the port reports both directions, receive only, or neither. target-power-setting is auto, manual or auto-max, with the default depending on card and mode. ptp-type identifies the port as dwdm-line, dwdm, osc or others, and ase-source-connected flags whether an ASE source is attached.

Source: `06-operation-commands/213-optical-ptp.md`

### optical-switch-los-thresholds

*optical-layer0 / parameter-values*

**Q.** At what received power does an optical protection switch decide the path has failed, and can I change it?

**A.** working-los-threshold and protection-los-threshold both default to -23dBm within a range of -55.0 to 0 dBm, and facility-los-threshold defaults to -30.0dBm over -55 to 15 dBm. Separately, working-switch-threshold and protection-switch-threshold default to -18dBm and are only used when switch-threshold-enable is set to enabled, which it is not by default. Hysteresis is configurable: los-threshold-hysteresis defaults to 3dB and switch-threshold-hysteresis to 2dB, both adjustable from 0.5 to 5.0 dB in 0.1 dB steps. The switch reverts according to reversion-mode, which defaults to non-revertive, with wtr-timer defaulting to 300 seconds and hold-off-timer to 0.

Source: `06-operation-commands/214-optical-switch.md`

### osc-configurable-attributes

*optical-layer0 / which-attribute*

**Q.** What can I actually configure on the supervisory channel?

**A.** set on the osc facility accepts label, admin-state, alarm-report-control, l0-comm-interface-type, osc-control, target-output-power, tx-power-adjustment, voa-attenuation-target-rx and lof-soak-timer. That last one matters for alarm behaviour: the guide's alarm-control section notes the OSC LOF alarm uses this per-facility user-configurable lof-soak-timer on the PBAx card rather than the system-wide soaking setting. Read-only attributes include monitoring-mode, oscc-support, osc-mode, osc-wavelength and voa-attenuation-actual-rx. admin-state defaults to unlock.

Source: `06-operation-commands/215-osc.md`

### otdr-scan-state

*optical-layer0 / which-attribute*

**Q.** How do I tell whether a fibre scan is running, finished or failed?

**A.** Read otdr-state on the otdr object: it reads not-available, idle, measuring, finished or fail, defaulting to not-available. Alongside it, otdr-measurement-time reports how long the measurement took, otdr-error carries any error, otdr-laser-state shows whether the OTDR laser is enabled, otdr-measurement-port and otdr-measurement-direction (tx or rx) say what was scanned, and otdr-ongoing-measurement-profile names the profile in use from none, short, medium, long or the raman-precheck1 to raman-precheck3 variants. otdr-file-prefix-requested identifies the resulting trace file.

Source: `06-operation-commands/223-otdr.md`

### otdr-ptp-measurement-settings

*optical-layer0 / parameter-values*

**Q.** What can I tune about how far and how finely an OTDR measures?

**A.** otdr-range covers auto or 0 to 300.0 km, otdr-pulse-width auto or 10 to 20,000 ns, and otdr-resolution auto or 0.4 to 100.0; all three default to auto. otdr-measurement-speed selects fast, medium, slow, precision, auto or high-precision, again defaulting to auto, and peak-power is auto or 5 to 14.5. otdr-ior is auto or 1.0 to 2.0 and otdr-fiber-type can be auto, not-configured or a specific type. launching-fiber-length accepts 0 to 50 m and defaults to 0. A worked example is `set otdr-ptp-114-3.1-2 launching-fiber-length 20 otdr-fiber-type auto otdr-measurement-speed high-precision otdr-pulse-width auto otdr-range 100`. otdr-direction-mode defaults to counter-prop-in-service.

Source: `06-operation-commands/224-otdr-ptp.md`

### ots-span-and-fiber

*optical-layer0 / which-attribute*

**Q.** Where do I tell the node what fibre is on a span and how lossy it is?

**A.** On the ots facility. fiber-type-rx and fiber-type-tx declare the fibre, fiber-length-rx and fiber-length-tx its length, and fiber-spectral-attenuation-tilt-rx and -tx its tilt. Span loss is handled by span-loss-reference, span-loss-transmit, span-loss-receive and span-loss-aging-margin-rx, with external-attenuation-rx and external-attenuation-tx accounting for pads outside the node. raman-coefficient-rx and -tx feed the Raman calculations. Other settings include osc-compatibility (osc-g30 by default, or osc-7100), osc-less, enable-dcn-interworking (false by default) and target-power-setting.

Source: `06-operation-commands/225-ots.md`

### ots-diagnostics-auto-name

*optical-layer0 / interpretation*

**Q.** I did not create the OTS diagnostics object. Where does its name come from?

**A.** The system creates it. The guide states the ots-diagnostics name is auto generated by the system on line card provisioning, following the format ots-diagnostics-<chassis>-<slot>-<DWDM Line>. The object carries trace identifier fields for the section: rx-operator, tx-operator and expected-operator, all tti-32, where tx-operator defaults to the node name followed by -D00 and the other two start empty.

Source: `06-operation-commands/226-ots-diagnostics.md`

### ots-r-fiber-type-default

*optical-layer0 / how-to-default*

**Q.** What fibre type does the reduced-scope transport section assume, and what else can I declare about the receive span?

**A.** required-fiber-type-rx defaults to SSMF, with alternatives including AllWave, DrakaLL, DSF, LEAF, LS, PSLC, PureSilica, SMF-ULL and Teralight. fiber-length-rx is unspecified by default or a value from 0 to 500.0, and span-loss-receive defaults to 99, which is the unmeasured placeholder rather than a real loss. external-attenuation-rx accounts for an external pad, for example `set ots-r-11-2-dwdm-line external-attenuation-rx 2`, and delta-pointloss covers -1 to 3.5 dB or not-applicable. target-power-setting defaults to auto. The object also reports configured-fiber-type-rx, configured-fiber-length-rx and span-loss-at-amplifier.

Source: `06-operation-commands/227-ots-r.md`

### ots-r-auto-otdr-greenfield

*optical-layer0 / how-to-default*

**Q.** Does the node run OTDR automatically, and does that depend on whether the site is new?

**A.** Yes to both. automatic-otdr is enabled for green field deployments and disabled otherwise, so the default depends on the deployment type. Set it explicitly with `set ots-r-auto-otdr-1-1-dwdm-line automatic-otdr enabled`. loss-calibration-by-otdr chooses whether the measurement feeds loss calibration, with none, rx-only, tx-only or tx-rx, defaulting to none. Results appear in external-attenuation-rx-measured and total-reflectance-rx-measured, both not-applicable until a scan completes, and auto-otdr-state reads not-applicable, not-available, pass, in-progress, fail or aborted.

Source: `06-operation-commands/228-ots-r-auto-otdr.md`

### oxcon-create-and-activation

*optical-layer0 / how-to*

**Q.** How do I create an optical cross connection between two media channels, and will it come up on its own?

**A.** source and destination are mandatory on add and both reference network media channels, for example `add oxcon-1-80-192262500__1-3.1-dwdm-line-192262500 source 'nmc-1-80-192262500' destination 'nmc-1-3.1-dwdm-line-192262500'`. Whether it activates by itself depends on activation-mode, which is automatic, manual or activate-on-create, defaulting to automatic in standard mode and manual in SLTE mode. In manual mode you drive it with activation-request-fwd and activation-request-bwd (no-request, activate or deactivate) and watch activation-state-fwd and activation-state-bwd, which read activated, partially-activated, faulted or deactivated. direction defaults to two-way, target-output-power-src and -dst accept -18.00 to 15.00 dBm, and circuit-id carries up to 128 characters.

Source: `06-operation-commands/231-oxcon.md`

### profile-control-card-scope

*optical-layer0 / scope-limit*

**Q.** Which cards can I read or write per-slice power profiles on?

**A.** The guide restricts profile-control to the RD20TM. The command lets the user read or write per-slice power or attenuation profiles to and from the database; for a power profile the data can be retrieved from the hardware, while for an attenuation profile it is supplied by the user. It runs in operational mode only. For the per-card-type power profiles used in system power estimation, see supported-power-profile instead.

Source: `06-operation-commands/248-profile-control.md`

### pump-readonly-attributes

*optical-layer0 / scope-limit  (weak: the source section is thin)*

**Q.** What does the pump command let me change?

**A.** Only label, admin-state and alarm-report-control; there is no add or delete form. Everything else is read through show: supporting-card, supporting-port, the supporting and supported facility lists, AID, oper-state, avail-state, managed-by and pump-type. To set the actual power of a Raman pump use pump-power, which carries target-pump-power per pump-id.

Source: `06-operation-commands/255-pump.md`

### pump-power-target-and-limits

*optical-layer0 / which-attribute*

**Q.** How do I set the power of an individual Raman pump and find out how high it is allowed to go?

**A.** Each pump is addressed as pump-power-<name>/<pump-id> and carries target-pump-power, a value from -99.00 to 99.00 dBm or not-applicable, which is the default. The permitted window is reported by min-target-pump-power and max-target-pump-power, so read those before setting a target. configured-pump-power shows what has been configured and actual-pump-power what is being delivered, defaulting to -99 until there is a real reading.

Source: `06-operation-commands/256-pump-power.md`

### raman-calibration-state

*optical-layer0 / which-attribute*

**Q.** How do I check whether the Raman calibration on a span is current or stale?

**A.** calibration-state on the raman-calibration object reads not-available, in-progress, up-to-date, out-dated or fail, defaulting to not-available, so out-dated is the one that says a recalibration is due. last-calibration-timestamp reads never until one has run. gain-calibration-error and calibrated-delta-pointloss report the outcome, the latter within -3 to 3.5 dB, and intermediate-results and additional-info carry up to 1024 characters of detail. To run a calibration use the calibrate command with type raman.

Source: `06-operation-commands/257-raman-calibration.md`

### rsc-pilot-tone

*optical-layer0 / which-command*

**Q.** What is the rsc facility and what does it report?

**A.** rsc is the Raman card Pilot Tone facility. set accepts label, admin-state and alarm-report-control; show adds rsc-power-rx and rsc-power-tx along with the usual supporting-card, supporting-port, facility lists, AID, oper-state, avail-state and managed-by. Note this section states its states inconsistently with the rest of the guide, listing admin-state as Unlock, Locked, Maintenance or Unknown with a default of Unlocked, where other facilities use lock, unlock and maintenance.

Source: `06-operation-commands/271-rsc.md`

### spectrum-when-instantiated

*optical-layer0 / pre-condition*

**Q.** There is no spectrum object on my node. Why not?

**A.** Because it depends on how the underlying multiplex section is monitored. The guide states the spectrum facility is only instantiated by the system when the underlying server layer OMS monitoring-mode is configured either as non-intrusive or ila-with-equalization. So if the OMS is intrusive or not-monitored, no spectrum object appears. When it does exist, set accepts label, admin-state, alarm-report-control and attenuation-setting, the last being 0 to 30 dB defaulting to 0dB, and dge-in-use reports whether a dynamic gain equaliser is in use.

Source: `06-operation-commands/303-spectrum.md`

### spectrum-control-per-slice

*optical-layer0 / how-to*

**Q.** How do I set an attenuation or power target for one slice of spectrum in one direction?

**A.** Use spectrum-control, which is addressed per name, direction and centre frequency: `add spectrum-control-<name>/<direction>/<center-frequency> [attenuation-target <value>] [target-output-power <value>]`. direction is ingress or egress, defaulting to egress. attenuation-target covers 0 to 30 dB and defaults to 0, while target-output-power is not-specified by default or a value from -55 to 55 dBm. show adds width and attenuation-actual so you can compare target against achieved. The read-only counterpart, spectrum-monitoring, reports power-actual and psd-actual at the same granularity.

Source: `06-operation-commands/304-spectrum-control.md`

### spectrum-monitoring-readings

*optical-layer0 / which-attribute*

**Q.** How do I read the measured power and spectral density for a slice of spectrum?

**A.** Show spectrum-monitoring for that name, direction and centre frequency, for example `show spectrum-monitoring-1-7-dwdm-line2/egress/191968750`. It reports power-actual and target-actual-power, both defaulting to -99 or -99dBm until measured, psd-actual in nW/GHz to two digits or not-applicable, and the slice geometry through width, which defaults to 50000, plus lower-frequency and upper-frequency. It is read only; to set targets use spectrum-control.

Source: `06-operation-commands/305-spectrum-monitoring.md`

### super-channel-contention-check

*optical-layer0 / which-attribute*

**Q.** When I create a super channel, how do I know whether it clashed with something already in the spectrum?

**A.** contention-check-status reports it, with values pending, success, overridden and failk (spelled that way in the guide), defaulting to pending. Whether the check runs at all is governed by openwave-contention-check, which defaults to false, and line-system-mode, which defaults to openwave. carriers and carrier-mode are mandatory on add. The object also reports actual-carrier-mode, capacity, client-mode, baud-rate, application, sop-tracking-mode and spectral-bandwidth, and valid-signal-time defaults to 480 minutes within a range of 1 to 7200.

Source: `06-operation-commands/319-super-channel.md`

### super-channel-group-merge-flag

*optical-layer0 / how-to*

**Q.** The example for creating a super channel group uses a -m flag. What does that do?

**A.** -m performs a merge, described in the guide's system section as a best effort add: if the target entity does not exist it is created, and if it exists it is updated with the attributes present on the command. So `add -m super-channel-group-<name> ...` is safe to re-run. The group accepts label, admin-state (lock, unlock or maintenance, default unlock), auto-in-service-enabled, valid-signal-time, alarm-report-control, line-system-mode and openwave-contention-check.

Source: `06-operation-commands/320-super-channel-group.md`

### supported-carrier-mode-lookup

*optical-layer0 / which-command*

**Q.** How do I find out which carrier modes a particular card supports and what capacity each gives?

**A.** `show supported-carrier-mode-<name>/<carrier-mode>` lists the supported carrier modes and, per mode, capacity, client-mode (ethernet or ethernet-otn), baud-rate, application, compatibility-id, status and supported-subtypes. Use status to see whether a mode is fully supported or only a candidate. The related lookups are gcmt for the golden carrier mode table and golden-carrier-mode for a configured instance.

Source: `06-operation-commands/322-supported-carrier-mode.md`

### supported-gain-range-values

*optical-layer0 / which-command*

**Q.** How do I find the actual gain limits behind the low, standard and high gain range settings?

**A.** `show supported-gain-range-<name>/<gain-range-type>` gives gain-range-min and gain-range-max for each type, where gain-range-type is low, high or standard. Those are the same three values the amplifier's gain-range-target accepts, so this is where you look up what each label means in dB before setting gain-range-control to manual.

Source: `06-operation-commands/324-supported-gain-range.md`

### supported-power-profile-purpose

*optical-layer0 / interpretation*

**Q.** Why would a card have more than one power profile, and what does choosing one change?

**A.** The guide explains that different power profiles can be supported to reflect different scenarios when using a card, and that the user can define per card instance which profile is in effect, which has an impact on the power estimation for the system. `show supported-power-profile-<card-type>/<name>` lists them with profile-description, power-draw and a default flag showing which one applies unless you choose otherwise.

Source: `06-operation-commands/326-supported-power-profile.md`

### bgp-instance-router-id

*ip-networking / how-to-default*

**Q.** When I create a BGP instance, where does its router ID come from?

**A.** From the loopback by default. router-id-mode is use-loopback or manual and defaults to use-loopback, so you only supply router-id when you set the mode to manual. local-as is mandatory on add, for example `add bgp-instance-10 local-as 24 router-id-mode use-loopback`. The instance also carries description and shows which vrf it belongs to, and `show bgp` gives the overview.

Source: `06-operation-commands/029-bgp-instance.md`

### bgp-neighbor-timers-and-auth

*ip-networking / parameter-values*

**Q.** What are the default BGP timers, and can I authenticate a peering session?

**A.** hold-time defaults to 90 seconds within 3 to 65535, keepalive-interval to 30 seconds within 1 to 21845, and connect-retry-interval to 120 seconds within 1 to 65535. negotiated-hold-time shows what was actually agreed with the peer. For authentication, secure-session is none or TCP-MD5, defaulting to none, with the password attribute carrying the key. peer-as is mandatory on add, enabled defaults to true, and afi-safi is IPv4-unicast or IPv6-unicast, defaulting to match the remote address family. session-state reports Idle, Connect, Active, OpenSent, OpenConfirm and the rest of the BGP state machine.

Source: `06-operation-commands/030-bgp-neighbor.md`

### bgp-network-advertise-requires-igp

*ip-networking / pre-condition*

**Q.** I configured a network to advertise to our upstream AS but it is not being announced. What am I missing?

**A.** The route has to be in the forwarding table already. The guide states that routes to be advertised to an external AS must exist in the forwarding table installed by an Interior Gateway Protocol such as OSPF, or by static routes, but not by BGP itself; for routes not present in the IGP tables, blackhole static routes must be configured, using the special-next-hop parameter on the static route. A maximum of 100 bgp-network objects can be configured. The instance is addressed as bgp-network-<instance id>/<remote-address>/<network prefix>.

Source: `06-operation-commands/031-bgp-network.md`

### comm-channel-mtu-differs-by-platform

*ip-networking / contradiction*

**Q.** What MTU can a communications channel take?

**A.** It depends on the platform: the guide gives 1280 to 1500 octets for the G30 and 1280 to 9202 octets for the G40, with a default of 1500 and per-interface-type defaults on the G40 such as 1518 for DCN. So do not assume jumbo frames are available on a G30. Other attributes: type selects the channel technology from OFEC-CC, GCC0, GCC1, the OSCX1 to OSCX5 family, FCC1 and the 1GE-OSCX variants, mode is L1, L2 or L3 defaulting to L3, mru is 64 to 1500 defaulting to 1500, fcs-length is 16 or 32 bits defaulting to 16, restart-timer is 1 to 10 seconds defaulting to 3 and max-failure is 2 to 10 defaulting to 5. Note the add form works in merge mode only, so use -m.

Source: `06-operation-commands/053-comm-channel.md`

### comm-eth-lldp-and-negotiation

*ip-networking / how-to-default*

**Q.** Is LLDP running on the management Ethernet ports by default, and what speed do they negotiate?

**A.** LLDP is off: lldp-admin-status is tx-only, rx-only, tx-and-rx or disabled, and defaults to disabled, with lldp-transmit-interval defaulting to 30 seconds within 1 to 16383. Speed is negotiated: auto-negotiation defaults to enabled and rate defaults to maximum, with 1, 10, 100, 1000 and 10000 Mbit/s as explicit choices; operational-rate and operational-duplex-mode report what was actually agreed and read unknown until then. duplex-mode defaults to full, flow-control to disabled, mtu to 1500 within 1280 to 1500, and mode to L3. The object is system managed and cannot be manually deleted.

Source: `06-operation-commands/054-comm-eth.md`

### dhcp-relay-modes

*ip-networking / parameter-values*

**Q.** How do I turn on DHCP relay and point it at a server?

**A.** Set mode and server-address on the dhcp-relay object: `set dhcp-relay mode ipv4 server-address <address>`. mode is disabled, ipv4 or ipv6 and defaults to disabled, so relay is off until you choose an address family. That is the system-wide setting; to enable it on a particular interface use if-dhcp-relay, whose dhcp-relay-enabled flag also defaults to false.

Source: `06-operation-commands/078-dhcp-relay.md`

### dns-assignment-method

*ip-networking / how-to-default*

**Q.** Can the node take its name servers from DHCP as well as from what we configure?

**A.** Yes. assignment-method on the dns object is manual, dhcp or both and defaults to both, so DHCP-supplied and manually configured servers are used together unless you narrow it. enabled defaults to true and search sets the search domain list. Individual servers are added with dns-server, where each entry has its own origin of dhcp or manual.

Source: `06-operation-commands/084-dns.md`

### dns-server-origin

*ip-networking / how-to*

**Q.** How do I add a name server, and how can I tell which ones came from DHCP?

**A.** Add it by address: `add dns-server-<address> [origin <value>]`, for example add dns-server-10.100.210.243 origin dhcp. The origin attribute distinguishes a DNS address assigned to the system by a DHCP server from one that was manually configured, and defaults to manual. Whether DHCP-supplied servers are used at all is governed by assignment-method on the dns object.

Source: `06-operation-commands/085-dns-server.md`

### if-dhcp-relay-per-interface

*ip-networking / how-to-default*

**Q.** How do I enable DHCP relay on just one interface?

**A.** Use if-dhcp-relay, which is addressed per interface name: `set if-dhcp-relay-<if-name> dhcp-relay-enabled true`. The flag defaults to false, so relay is off per interface even when the system-wide dhcp-relay mode is set. A worked read is `show if-dhcp-relay-1-8-dwdm-line-1GE-OSCX1-MGMT dhcp-relay-enabled`.

Source: `06-operation-commands/126-if-dhcp-relay.md`

### interface-address-assignment

*ip-networking / parameter-values*

**Q.** When I create a management interface, is it static or DHCP addressed, and is it protected?

**A.** Static and protected by default. ipv4-address-assignment-method and ipv6-address-assignment-method are static or dhcp, both defaulting to static, while ipv4-enabled and ipv6-enabled both default to true, so both families are on. protection-mode is unknown, protected or unprotected and defaults to protected, with protection-state reporting the actual state. proxy-arp-enabled defaults to false and if-dhcp-relay to false. if-type is mandatory on add. Note that a set against the bare object can affect several interfaces at once and prompts for confirmation.

Source: `06-operation-commands/134-interface.md`

### ip-monitoring-withdraw-route

*ip-networking / interpretation*

**Q.** Can the node pull a static route automatically when the far end stops answering?

**A.** Yes, that is what ip-monitoring does: a monitoring instance periodically pings a destination and the result takes action on configured static routes. destination and next-hop are mandatory on add, static-route names the route to act on, and action is none or withdraw, defaulting to withdraw. probe-interval is 0 to 60 seconds defaulting to 5, and drop-rate is 1 to 10 defaulting to 1. enabled defaults to true, and monitoring-state reports the current result. The corresponding static route shows monitoring-state as unmonitored, ok or failed.

Source: `06-operation-commands/138-ip-monitoring.md`

### ipv4-address-add-netmask

*ip-networking / how-to*

**Q.** How do I put an IPv4 address on a management port?

**A.** Add it against the interface name: `add ipv4-address-<if-name>/<ip> netmask <value>`, for example add ipv4-address-1-AUX-1/200.20.20.186 netmask '255.255.240.0'. netmask is mandatory and given in dotted form rather than as a prefix length, which differs from IPv6 where prefix-length is used. origin reads static, dhcp or auto-config and defaults to static. The guide suggests pressing ? after ipv4-address- to list the available interfaces. Remove it with delete.

Source: `06-operation-commands/143-ipv4-address.md`

### ipv4-static-route-distance-and-blackhole

*ip-networking / parameter-values*

**Q.** What administrative distance does a static route get, and how do I create a discard route?

**A.** distance covers 1 to 255 and defaults to 1, so a static route is preferred over most dynamic sources unless you raise it. A discard route is made with special-next-hop, whose value is blackhole; that is also what the BGP section requires for advertising prefixes not present in an IGP. A route is added as ipv4-static-route-<ipv4-destination-prefix>/<vrf>, for example add ipv4-static-route-10.220.0.0/16/MGMT next-hop-address 10.220.225.165. advertised defaults to false, origin is manual or dhcp defaulting to manual, and monitoring-state reads unmonitored, ok or failed depending on any ip-monitoring instance attached. Note set only accepts label, so other attributes require delete and re-add.

Source: `06-operation-commands/144-ipv4-static-route.md`

### ipv6-address-prefix-length

*ip-networking / comparison*

**Q.** Is adding an IPv6 address the same as IPv4 on this platform?

**A.** Almost, but the mask is expressed differently: IPv6 uses prefix-length, a number from 1 to 128, where IPv4 uses a dotted netmask. So it is `add ipv6-address-<if-name>/<ip> prefix-length <value>`, for example add ipv6-address-1-AUX-1/AAAA::186 prefix-length 10. origin is the same set of static, dhcp or auto-config, defaulting to static. As with IPv4, pressing ? after the command lists the available interfaces.

Source: `06-operation-commands/145-ipv6-address.md`

### ipv6-static-route-dcn-b

*ip-networking / scope-limit*

**Q.** Can I point a static route out of the secondary DCN interface?

**A.** Only in one configuration: the guide notes static routes can be added with the DCN-B interface when DCN is operating in active-active mode. Otherwise the ipv6-static-route object mirrors its IPv4 counterpart, addressed as ipv6-static-route-<ipv6-destination-prefix>/<vrf> with next-hop-address, interface, distance in the range 1 to 255 defaulting to 1, advertised defaulting to false, origin of manual or dhcp, and special-next-hop of blackhole. A default route example is add ipv6-static-route-::/0/MGMT next-hop-address 2620:38:4::8:8000:1 interface DCN.

Source: `06-operation-commands/146-ipv6-static-route.md`

### management-address-remote

*ip-networking / which-command*

**Q.** How do I see the management addresses a neighbour is advertising to us?

**A.** Use management-address, which retrieves management address information about a particular chassis component and is keyed by lldp-port, direction, address-subtype and address, so it is LLDP-derived remote information. Each management address must have a distinct management address type. It reports if-subtype (unknown, if-index or system-port-number), if-id and address-oid. For the addresses this node advertises rather than receives, use management-address-local, which drops the direction from the key.

Source: `06-operation-commands/174-management-address.md`

### management-address-local-vs-remote

*ip-networking / disambiguation*

**Q.** What is the difference between management-address and management-address-local?

**A.** The key tells them apart. management-address-local is addressed as management-address-local-<lldp-port>/<address-subtype>/<address>, while management-address adds a direction to the key: management-address-<lldp-port>/<direction>/<address-subtype>/<address>. Both report if-subtype, if-id and address-oid, and both carry the same description text about management addresses configured on a remote system, so read the local variant as this node's own advertised addresses per LLDP port.

Source: `06-operation-commands/175-management-address-local.md`

### networking-use-as-source

*ip-networking / minimal-command  (weak: the source section is thin)*

**Q.** Is there a setting that picks which address the node uses as its source?

**A.** Yes, use-as-source is the only configurable attribute on the networking container: `set networking [use-as-source <value>]`, read back with show networking. The guide documents no values or default for it. The wider networking view is networking-services, which lists the network services, and protocols, which shows the management protocol objects.

Source: `06-operation-commands/190-networking.md`

### networking-services-list

*ip-networking / minimal-command  (weak: the source section is thin)*

**Q.** Which command lists the network services the node is running?

**A.** `show networking-services` shows the list of network services. It takes no parameters and runs in operational or candidate configuration mode. For the management protocols specifically, protocols gives ssh, cli, serial-console, netconf, grpc, snmp, restconf, http-file-server and data-model-openconfig.

Source: `06-operation-commands/191-networking-services.md`

### next-hop-per-rib

*ip-networking / interpretation*

**Q.** Why is next hop information organised per RIB rather than just per route?

**A.** Because a RIB holds one address family. The guide states each entry represents a RIB identified by the name key, all routes in a RIB belong to the same address family, and for each routing instance the system provides one system-controlled default RIB per supported address family. So next-hop is addressed as next-hop-<rib-name>/<destination-prefix>/<interface> and reports next-hop-address. rib itself shows the RIB with its vrf and address-family, and route lists the routes with their source-protocol.

Source: `06-operation-commands/192-next-hop.md`

### ospf-clear-instance

*ip-networking / consequence*

**Q.** How do I restart an OSPF process, and does the command wait for it to finish?

**A.** `clear ospf [instance=]<value>` removes and restarts an ospf-instance, for example clear ospf 1. The guide states the operation is asynchronous, so the command returns before the restart has completed. The instance id must be supplied. It runs in operational mode only and takes -f to skip confirmation. To delete the instance permanently rather than restart it, use delete on ospf-instance.

Source: `06-operation-commands/216-ospf.md`

### ospf-area-type-limited

*ip-networking / scope-limit*

**Q.** Can I configure a stub or NSSA area?

**A.** Not according to this guide: ospf-area-type has a single documented value, normal, which is also the default. So areas are created as normal areas with `add ospf-area-<instance-id>/<ospf-area-id> [ospf-area-type <value>]`, for example add ospf-area-1/0.0.0.0, where instance-id is 0 to 255 and the area id is in dotted form. Route summarisation is done separately with ospf-area-range, which the guide notes applies to Area Border Routers only.

Source: `06-operation-commands/217-ospf-area.md`

### ospf-area-range-abr-only

*ip-networking / scope-limit*

**Q.** How do I summarise routes out of an OSPF area, and does it work on any router?

**A.** Use ospf-area-range, which summarises routes for an OSPF area matching an address and mask. The guide states it is applicable to Area Border Routers only, so it has no effect on a router that is not an ABR. It is addressed as ospf-area-range-<instance-id>/<ospf-area-id>/<prefix> and carries advertise, true or false, defaulting to true; set advertise false to suppress the summary rather than announce it.

Source: `06-operation-commands/218-ospf-area-range.md`

### ospf-instance-version

*ip-networking / parameter-values*

**Q.** Does this platform run OSPFv3, and how is the router id chosen?

**A.** Yes: version is ospfv2 or ospfv3 and defaults to ospfv2, set at creation time. router-id-mode is manual or use-loopback and defaults to use-loopback; with manual you supply router-id, which is mandatory on add. instance-id runs 0 to 255. Two worked examples are add ospf-instance-1 description abc router-id-mode manual version ospfv2 router-id 100.100.1.1 and add ospf-instance-2 description xyz router-id-mode use-loopback version ospfv3. To restart an instance asynchronously use clear ospf.

Source: `06-operation-commands/219-ospf-instance.md`

### ospf-interface-timers

*ip-networking / parameter-values*

**Q.** What are the default OSPF hello and dead timers on an interface, and can I authenticate the adjacency?

**A.** hello-interval defaults to 10 and router-dead-interval to 40, with retransmission-interval 5 and transmit-delay 1. ospf-cost defaults to 10 and priority to 1, and ospf-network-type defaults to broadcast. Authentication is available: ospf-auth-enable defaults to false, ospf-auth-algorithm defaults to an HMAC SHA 256 variant, and ospf-auth-key carries the key, which is also modelled separately by the auth-key object. ospf-if-routing defaults to auto and enable to true. A full example is add -m ospf-interface-1/0.0.0.0/1-AUX-1 enable true hello-interval 6 router-dead-interval 18 retransmission-interval 2 transmit-delay 1.

Source: `06-operation-commands/220-ospf-interface.md`

### ospf-neighbor-states

*ip-networking / enumeration*

**Q.** My OSPF adjacency is stuck. What states can it be in and what do the roles mean?

**A.** show ospf-neighbor reports state and role. state runs through the OSPF machine starting at down, the initial state where no Hello packets are received from the neighbour, then init once Hellos are seen, and on through the remaining states. role reads drother (Designated Router Other), dr (Designated Router), bdr (Backup Designated Router) or ptp (point-to-point). It also shows address and priority. The object is read only and addressed as ospf-neighbor-<instance-id>/<ospf-area-id>/<ospf-if-name>/<router-id>, and it runs in operational mode only.

Source: `06-operation-commands/221-ospf-neighbor.md`

### ospfv3-ipsec-sa-algorithms

*ip-networking / parameter-values*

**Q.** How do I secure OSPFv3, and what integrity algorithms are available?

**A.** OSPFv3 uses an IPsec security association rather than the OSPFv2 style key. Add an ospfv3-ipsec-security-association keyed by instance-id, area id, interface name and spi, where spi runs from 256 to 4294967295. integrity-algorithm is mandatory and offers the AUTH_HMAC_SHA2_256_128, AUTH_HMAC_SHA2_384_192, AUTH_HMAC_SHA2_512_256 and AUTH_HMAC_SHA1_160 family. ipsec-protocol is ESP and ipsec-mode is transport, both fixed at those values. A worked example is add ospfv3-ipsec-security-association-1/0.0.0.0/DCN/256 integrity-algorithm AUTH_HMAC_SHA2_256_128.

Source: `06-operation-commands/222-ospfv3-ipsec-security-association.md`

### ping-options

*ip-networking / how-to*

**Q.** How do I test reachability from a particular interface or VRF?

**A.** ping takes -i=<interface> or -v=<vrf> as alternatives, so you pick one or the other rather than both: `ping [-c=<count>] [-w=<timeout>] [-s=<pktsize>] [-i=<interface> | -v=<vrf>] <ping-dest>`. The destination can be an IPv4 address, an IPv6 address or a domain name, for example ping 192.0.2.1. It uses the ICMP protocol's mandatory echo request datagram to elicit an echo response. -h shows help. For the path rather than reachability, traceroute takes the same -i and -v options.

Source: `06-operation-commands/235-ping.md`

### protocols-list

*ip-networking / enumeration*

**Q.** Which management protocols does the node expose, and how do I check them in one place?

**A.** `show protocols` takes an optional filter naming one of ssh, cli, serial-console, netconf, grpc, snmp, restconf, http-file-server or data-model-openconfig, which is effectively the list of management protocol objects the node models. Each has its own command for detail, for example netconf, restconf, grpc, snmp and ssh.

Source: `06-operation-commands/254-protocols.md`

### rib-address-family

*ip-networking / which-command*

**Q.** How do I look at the routing information base for one address family?

**A.** `show rib-<rib-name> [vrf] [address-family]`, for example show rib-IPv4. Each entry represents a RIB identified by the name key, all routes in a RIB belong to the same address family, and for each routing instance the system provides one system-controlled default RIB per supported address family. For the routes themselves use route, and for next hops use next-hop.

Source: `06-operation-commands/267-rib.md`

### route-source-protocol

*ip-networking / which-attribute*

**Q.** How do I tell whether a route came from OSPF, BGP or static configuration?

**A.** show route reports source-protocol, described as the source protocol for example OSPF, BGP or static. The command lists system routes from various sources, dynamic protocols and static routes alike, and is addressed as route-<rib-name>/<destination-prefix>. It also shows special-next-hop, which reads none, blackhole or unreachable, defaulting to none, so discard routes are visible here too.

Source: `06-operation-commands/269-route.md`

### routing-overview

*ip-networking / minimal-command  (weak: the source section is thin)*

**Q.** Is there a single command that gives the routing overview?

**A.** `show routing` shows routing information. It takes no parameters and runs in operational or candidate configuration mode. Beneath it sit rib for each routing information base, route for individual routes with their source-protocol, next-hop for next hop entries, and vrf for the virtual routing instances.

Source: `06-operation-commands/270-routing.md`

### supporting-interface-reference

*ip-networking / which-command  (weak: the source section is thin)*

**Q.** What does supporting-interface tell me?

**A.** It shows supporting interface information, addressed as supporting-interface-<name>/<interface>. The interface value is a path reference into the networking interface list, so the object records which configured interface underpins the named entity rather than holding settings of its own. It is read only and available in operational and candidate configuration mode.

Source: `06-operation-commands/331-supporting-interface.md`

### traceroute-defaults

*ip-networking / parameter-values*

**Q.** How many hops does a trace follow by default, and which VRF does it use?

**A.** The maximum hop count -m defaults to 30 within a range of 1 to 255, and -v, the VRF, defaults to MGMT. The per-probe timeout -w defaults to 2 seconds within 1 to 10, and pktsize defaults to 60 bytes for IPv4. The interface -i is otherwise selected according to the routing table, and it is an alternative to -v rather than a companion. The destination can be an IPv4 address, an IPv6 address or a domain name, for example traceroute 1.2.3.4. The command works by using the IP time to live field to elicit ICMP TIME_EXCEEDED responses along the path.

Source: `06-operation-commands/355-traceroute.md`

### vrf-instance-view

*ip-networking / which-command*

**Q.** How do I see the routing instances on the node and which chassis each belongs to?

**A.** `show vrf-<name> [type] [chassis-name] [description]`, for example show vrf-MGMT, which is also the default VRF used by traceroute. The object shows the Virtual Routing and Forwarding instance with its type, the chassis it is associated with and a description. VRF names also appear as part of the key of static routes, for example ipv4-static-route-10.220.0.0/16/MGMT.

Source: `06-operation-commands/372-vrf.md`

### activate-swimage-db-action

*software-firmware-files / consequence*

**Q.** What happens to the database when I activate a new software image, and does the node restart?

**A.** Yes, the guide states a successful swimage activation implies a system restart. What happens to the database is controlled by db-action, which can upgrade, empty or roll back the database; if it is not provided the system decides automatically. The documented values are empty-db, upgrade-db and rollback, with auto meaning the system default behaviour. Unless -f is given the user must confirm the activation when prompted. The same command also activates a database, a location LED test, a firmware image in a resource, or installs a Key Replacement Package.

Source: `06-operation-commands/008-activate.md`

### activate-location-led-limits

*software-firmware-files / scope-limit*

**Q.** Can I flash the locator LED on two cards at once, and how do I turn it off?

**A.** No: the guide states only a single location LED test can be running at a time, and also that not all cards support the Location LED test activation. led-mode is flash or solid, defaulting to flash, and timeout accepts 0 to 120 seconds. Be careful with timeout: the guide notes it does not take effect in G30 R5.1, and to stop the LED location and lamp test functions you should disable them with the terminate command rather than relying on the timer.

Source: `06-operation-commands/008-activate.md`

### bootstrap-neighbor-over-osc

*software-firmware-files / which-command*

**Q.** How do I create the first admin account on a brand new neighbour node that has no users yet?

**A.** Use bootstrap, which establishes a TLS connection over the OSC link to a neighbour NE and provisions the initial administrator account on it. The guide describes it as being for commissioning, to remotely create the first administrator user on an OSC peer NE that has no configured users yet. You identify the neighbour either by local-port or by neighbor-address, then supply new-admin-user and new-password, where the password follows a hashed pattern beginning $6$.

Source: `06-operation-commands/032-bootstrap.md`

### cancel-upgrade-simple

*software-firmware-files / minimal-command*

**Q.** An upgrade is running and I need to stop it. What do I type?

**A.** `cancel-upgrade`. It cancels any active upgrade in progress, takes no parameters other than -h for help, and runs in operational mode. To see what state the upgrade reached, use upgrade-status, whose status attribute reads idle, upgrade-in-progress, upgrade-complete, upgrade-partially-failed, upgrade-failed and the validate variants.

Source: `06-operation-commands/038-cancel-upgrade.md`

### change-ztp-mode-destructive

*software-firmware-files / consequence*

**Q.** What actually happens if I re-enable zero touch provisioning on a node that is already in service?

**A.** It is destructive. The guide states that `change-ztp-mode enabled` starts ZTP, reverts the database to the factory default and triggers a system reboot. `change-ztp-mode disabled` disables ZTP and stops it if it is already in progress. The command takes -f to skip confirmation, so use it carefully. To read the current state without changing anything, use show ztp.

Source: `06-operation-commands/046-change-ztp-mode.md`

### current-fw-status

*software-firmware-files / which-attribute*

**Q.** How do I tell whether a card is running the firmware it is supposed to be running?

**A.** Compare fw-version against expected-fw-version on the current-fw object, and read fw-status, which is current, not-current or unavailable, defaulting to unavailable. It is addressed several ways depending on the resource, for example show current-fw-<card-name>-<port-name>/<fw-name> or show current-fw-1-3/DCO_1. For what the installed software load contains rather than what is running, use packaged-fw.

Source: `06-operation-commands/067-current-fw.md`

### download-filetypes-and-storage

*software-firmware-files / enumeration*

**Q.** What kinds of file can I pull onto the node, and where do they end up?

**A.** filetype covers crl, database, file, krp, local-certificate, peer-certificate, script, swimage and trusted-certificate among others. You do not choose a local location: the guide states the system automatically stores the downloaded file in an appropriate place, and suggests `show transfer` to see the default storage directory. The source is either a URL, for example source=scp://user@host:/path, or a configured file-server plus a path. For certificates, certificate-name identifies the entry. db-action and clear-type apply when the download is a database or a script-driven initialisation.

Source: `06-operation-commands/086-download.md`

### downloaded-image-signature

*software-firmware-files / which-command*

**Q.** How do I check the signature of an image that has been downloaded to the node?

**A.** `show downloaded-image-<manifest-file>/<name> [signature]` retrieves information about downloaded image files including the signature, so it is keyed by the manifest the image belongs to. The related manifest command shows manifest-signature and downloaded-on for the manifest itself, and downloads lists what has been fetched.

Source: `06-operation-commands/087-downloaded-image.md`

### downloads-list

*software-firmware-files / minimal-command  (weak: the source section is thin)*

**Q.** Which command lists what has been downloaded to the node?

**A.** `show downloads` shows the list of downloads. It takes no parameters. A fuller view is `show sw-management downloads`, which covers software locations, activity and downloads together, and transfer-status gives per-filetype transfer progress and outcome.

Source: `06-operation-commands/088-downloads.md`

### file-operations-and-access

*software-firmware-files / scope-limit*

**Q.** Can I rename or delete any file on the node from the CLI?

**A.** Only within your own access. The guide states file system access is restricted to the current user's access and that not all files and directories are editable. The supported operations are rename, view and delete, for example `file rename /tmp/a.log /tmp/b.log`. There is also a `clear file [filetype=]<value> [target-file=]<value>` form where filetype is swimage, script or krp. file-operation is a near-identical command offering the same rename, delete and view operations.

Source: `06-operation-commands/108-file.md`

### file-operation-vs-file

*software-firmware-files / disambiguation  (weak: the source section is thin)*

**Q.** There is a file command and a file-operation command. What is the difference?

**A.** Very little in what they do: both perform basic file and directory operations with the same rename, delete and view operations. file-operation takes them as `file-operation [[operation=]<value>] [new-file-path=]<value> [file-path=]<value>`. The file command additionally documents the clear file form for removing a stored swimage, script or krp by filetype, and carries the guidance that file system access is restricted to the current user's access. Both run in operational mode.

Source: `06-operation-commands/109-file-operation.md`

### file-server-protocols

*software-firmware-files / enumeration*

**Q.** What transfer protocols can I configure a file server with, and what do I have to supply?

**A.** protocol offers file, ftp, sftp, scp, http and https, and both server-address and protocol are mandatory on add. Optional attributes are server-port, user-name, password, initial-path and label, for example `add file-server-139 server-address '10.220.227.139' protocol 'scp' user-name 'root' password 'Nokia' initial-path '/root'`. Once defined, download and upload can reference file-server plus path instead of a full URL, and if no path is given on upload the file-server initial-path is used.

Source: `06-operation-commands/110-file-server.md`

### file-type-last-transfer

*software-firmware-files / which-command*

**Q.** How do I see when a particular kind of file was last transferred and whether it worked?

**A.** `show filetype-<name>` reports last-completion-status, last-transfer, last-duration and last-operation, where last-operation is unknown, upload or download. Note the command is documented under the name file-type but the syntax uses filetype-<name>. For live progress on a transfer, transfer-status adds transfer-progress as a percentage, bytes-transferred, total-bytes and a details string.

Source: `06-operation-commands/111-file-type.md`

### http-file-server-ports

*software-firmware-files / how-to-default*

**Q.** Is plain HTTP available for file transfer off the node, and on what ports?

**A.** Not by default. http-file-server has enabled defaulting to true, but http-enabled defaults to false while https-enabled defaults to true, so only HTTPS is served unless you turn plain HTTP on. http-port defaults to 8980 and https-port to 8981, neither of them the usual 80 or 443. url-base defaults to /transfer. Note the security-policies secure-mode flag, when enabled, stops non-secure protocols including HTTP being used at all.

Source: `06-operation-commands/124-http-file-server.md`

### manifest-component-state

*software-firmware-files / which-command*

**Q.** How do I see what a downloaded software manifest contains and whether each part installed?

**A.** Three related shows. `show manifest-<manifest-file>` gives manifest-signature, downloaded-on and information. `show manifest-component-<manifest-file>/<equipment-type>/<name>` gives each component's state, version and description, where state reads installed, not-installed, installation-failed or unknown. `show manifest-firmware-<manifest-file>/<equipment-type>/<fw-name>` gives the firmware versions in the manifest. The same state vocabulary appears on sw-component and sw-subcomponent.

Source: `06-operation-commands/176-manifest.md`

### packaged-fw-per-equipment

*software-firmware-files / interpretation*

**Q.** Does a software load carry one firmware version for everything?

**A.** No. The guide states versions for the same firmware can be different per equipment-type, which is why packaged-fw is keyed by equipment-type and firmware name: show packaged-fw-<location-id>/<swload-state>/<equipment-type>/<fw-name> or the shorter show packaged-fw-<swload-state>/<equipment-type>/<fw-name>. It shows the firmware version included in the software load, as opposed to current-fw, which shows what a card is actually running.

Source: `06-operation-commands/232-packaged-fw.md`

### prepare-upgrade-validate-then-apply

*software-firmware-files / how-to*

**Q.** Is there a way to check a software label before committing to it?

**A.** Yes, that is what prepare-upgrade's two options are for: validate validates the software label and apply applies it, for example `prepare-upgrade validate <label>`. manifest names the manifest to use. db-action (empty-db, upgrade-db or rollback, default upgrade-db) and clear-type (full, keep-networking or initialize-from-script, default full) control what happens to the existing configuration, and script with new-admin-user and new-admin-password support an initialize-from-script path. It runs in operational mode and takes -f.

Source: `06-operation-commands/247-prepare-upgrade.md`

### recover-mode-clear-traffic-impact

*software-firmware-files / consequence*

**Q.** The node is in recover mode. What does clearing that flag actually do?

**A.** More than it sounds. The guide states clearing the recover-mode flag clears the current recover-mode state of the NE, confirming the current configuration as is, re-enabling communication with the line cards and potentially reconfiguring the traffic settings, and that as such it may be traffic impacting. The command is `clear [-f] recover-mode` and takes no parameters. Why the node entered recover mode is visible on the ne object through recover-mode, original-recover-mode-reason and recover-mode-reason.

Source: `06-operation-commands/260-recover-mode.md`

### software-load-states

*software-firmware-files / which-command*

**Q.** How do I see which software load is active and which is waiting to be used?

**A.** `show software-load-<swload-state>`, for example show software-load-active, where the state distinguishes active, inactive and installable loads. Per load it reports swload-version, swload-manifest, swload-prepared (true or false), swload-status, swload-information, swload-activation-type, swload-vendor, swload-product, swload-label, swload-delta-label and swload-pkg-type. It can also be scoped per location with software-load-<location-id>/<swload-state>. The quick summary is swversion, which retrieves the active, inactive and installable versions.

Source: `06-operation-commands/301-software-load.md`

### software-location-per-slot

*software-firmware-files / minimal-command  (weak: the source section is thin)*

**Q.** How do I look at software on one particular slot?

**A.** `show software-location-<location-id>`, for example show software-location-1-5, retrieves information about the location of software. The same location ids appear as the first key of software-load, sw-component and sw-subcomponent, so this is how per-slot software is scoped. sw-management also accepts software-location-<shelf>-<slot> with software-load-active, software-load-inactive or software-load-installable.

Source: `06-operation-commands/302-software-location.md`

### subtype-constraint-capacity

*software-firmware-files / which-command*

**Q.** Where do I find the capacity limits and supported applications for a card subtype?

**A.** `show subtype-constraint-<card-type>/<subtype>` reports min-capacity, max-capacity, supported-applications, unsupported-applications and a description. It is keyed by card type and subtype, so it answers what a particular licensed or configured subtype of a card is allowed to do. The related lookups for what a card can support in hardware terms are supported-card, supported-port and supported-tom.

Source: `06-operation-commands/318-subtype-constraint.md`

### sw-component-state

*software-firmware-files / which-attribute*

**Q.** How do I tell whether a part of the software load failed to install?

**A.** state on sw-component reads installed, not-installed, installation-failed or unknown, defaulting to unknown, so installation-failed is the value to look for. It is addressed as sw-component-<location-id>/<swload-state>/<name> or without the location, and also reports version and description. For the level below, sw-subcomponent uses the same state vocabulary keyed additionally by component name.

Source: `06-operation-commands/332-sw-component.md`

### sw-container-resource-usage

*software-firmware-files / which-command*

**Q.** How do I see how much CPU and memory the node's containers are using?

**A.** `show sw-container-<container-name>` lists the OS-level containers with cpu-usage and memory-usage, both percentages from 0 to 100, plus equipment, state, description and uptime. For the services running inside them, sw-service reports the same cpu-usage and memory-usage along with state, state-details, last-start-time and reboot-count, and show sw-services lists them all.

Source: `06-operation-commands/333-sw-container.md`

### sw-control-rule-fail-action

*software-firmware-files / how-to*

**Q.** Can I change what the node does when a particular software service dies?

**A.** Yes, that is what sw-control-rule is for: it adds service-specific custom rules to override the default action upon service failure. It is keyed by service name with a mandatory fail-action, for example `add sw-control-rule-xmm4-1-1_host_KeyManagement fail-action default-action`. Use delete to remove the override and return to the default behaviour. The service names come from sw-service.

Source: `06-operation-commands/334-sw-control-rule.md`

### sw-management-overview

*software-firmware-files / which-command*

**Q.** Is there one command that shows software locations, downloads and load state together?

**A.** Yes: `show sw-management` covers software locations, activity and downloads. It takes a filter of downloads, or software-location-<shelf>-<slot> optionally followed by software-load-active, software-load-inactive or software-load-installable, for example show sw-management downloads. The individual objects underneath are software-location, software-load, sw-component, sw-subcomponent and downloads.

Source: `06-operation-commands/335-sw-management.md`

### sw-service-restart-count

*software-firmware-files / which-attribute*

**Q.** How can I tell whether a software service has been restarting repeatedly?

**A.** sw-service reports reboot-count alongside last-start-time, so a high count with a recent start time indicates a service that keeps dying. It also shows state and state-details, equipment, location, cpu-usage, memory-usage and uptime. `show sw-services` lists them all. To change what happens when one fails, add a sw-control-rule with a fail-action for that service name.

Source: `06-operation-commands/336-sw-service.md`

### sw-subcomponent-detail

*software-firmware-files / which-command  (weak: the source section is thin)*

**Q.** What does sw-subcomponent add over sw-component?

**A.** One more level of key. sw-subcomponent is addressed as sw-subcomponent-<location-id>/<swload-state>/<sw-component-name>/<sw-subcomponent-name>, or without the location, so it identifies a part within a named component. It reports the same three attributes, state (installed, not-installed, installation-failed or unknown), version and description.

Source: `06-operation-commands/337-sw-subcomponent.md`

### swversion-quick-check

*software-firmware-files / minimal-command*

**Q.** What is the quickest way to see what software the node is running and what it could run?

**A.** `swversion`, with no parameters, retrieves the active, inactive and installable versions of the software present on the network element. For more detail on each, software-load reports swload-version, swload-status and swload-prepared per state, and sw-management ties locations, loads and downloads together.

Source: `06-operation-commands/338-swversion.md`

### third-party-fw-validity

*software-firmware-files / which-attribute*

**Q.** How do I check that a third-party firmware file on the node is intact and which equipment it applies to?

**A.** `show third-party-fw-<fw-name>` reports file-status as valid, invalid or missing, along with a crc, path, version, vendor and part-number. applicable-eqpt says what it can be used on and present-in-eqpt where it is already loaded. nsa-upgrade-version indicates the non-service-affecting upgrade version. For Nokia firmware, use current-fw for what is running and packaged-fw for what the load contains.

Source: `06-operation-commands/350-third-party-fw.md`

### transfer-http-proxy

*software-firmware-files / how-to*

**Q.** Our node has to go through a proxy to reach external servers. Where do I set that?

**A.** On the transfer object: `set transfer http-proxy http://1.2.3.4:1080`, where the format is [http://]<host>[:<port>]. show transfer also reports debug-log-optional-content, and the guide elsewhere points at show transfer to find the default storage directory used by download. Per-transfer outcomes live in transfer-status, and the guide notes transfer status only exists if at least one operation of that kind has been done for the filetype.

Source: `06-operation-commands/356-transfer.md`

### transfer-status-progress

*software-firmware-files / which-attribute*

**Q.** How do I watch how far along a file transfer is?

**A.** `show transfer-status-<filetype>/<operation>` where operation is upload or download. It reports transfer-progress as 0 to 100 percent, bytes-transferred and total-bytes, plus last-completion-status as a string such as success, fail, in-progress or unknown, last-transfer, last-duration, transfer-type of sync or async, session-id, session-user-name, filename and a details string that reads completed, Failed, idle, preparation or transfer, defaulting to idle. Note this section has no access mode stated in the guide.

Source: `06-operation-commands/357-transfer-status.md`

### upgrade-status-levels

*software-firmware-files / interpretation*

**Q.** An upgrade partly failed. How do I find out where and at which step?

**A.** `show upgrade-status-<resource>` reports at three levels, NE, chassis and card, with the resource parameter specifying which object the status is for. status reads idle, upgrade-in-progress, upgrade-complete, upgrade-partially-failed, upgrade-failed and the validate variants, so upgrade-partially-failed at NE level can be narrowed by querying the individual cards. step and step-start-time say where it got to, start-time and end-time bracket it, and details carries the explanation.

Source: `06-operation-commands/363-upgrade-status.md`

### upload-debug-log-and-certs

*software-firmware-files / how-to*

**Q.** How do I get diagnostic logs off the node, and is exporting certificates any different?

**A.** For logs use filetype=debug-log, which uploads logs for diagnostic troubleshooting including all other logs; the guide states debug-entity is a mandatory parameter for it. The upload command gathers the desired content into a single generated file and then transfers it, either to a destination URL or to a configured file-server plus path, and password is required for scp, sftp or ftp. Certificates differ: when uploading local, peer or trusted certificates a specific certificate must be named with certificate-name, except for trusted certificates where omitting the name exports all of them. Other filetypes include database, fdr-log, file and krp.

Source: `06-operation-commands/364-upload.md`

### ztp-state-machine

*software-firmware-files / enumeration*

**Q.** How do I check how far zero touch provisioning has got?

**A.** `show ztp` reports ztp-mode (disabled or enabled, defaulting to enabled), ztp-state, ztp-details and ztp-completion-status (not-completed or completed, defaulting to not-completed). ztp-state walks through ztp-init, dhcp-in-progress, image-download-in-progress, image-install-in-progress and custom-script-execution-in-progress among others, starting at ztp-init. To change the mode rather than read it, use change-ztp-mode, remembering that enabling ZTP reverts the database to factory default and reboots.

Source: `06-operation-commands/374-ztp.md`

### capabilities-per-card

*equipment-inventory / minimal-command  (weak: the source section is thin)*

**Q.** How do I see what one particular installed card is capable of?

**A.** `show capabilities-<name>` retrieves information about a card's capabilities. It takes no further attributes in the documented syntax. For capability information by card type rather than by installed instance, use supported-card, which reports node-type-compatibility, supported-subtype, card-mode options, power draw, LED and console support and supported bands.

Source: `06-operation-commands/039-capabilities.md`

### card-provision-required-type

*equipment-inventory / how-to*

**Q.** What do I have to supply to provision a card into a slot?

**A.** Three things are mandatory on add: required-type, chassis-name and slot-name, for example `add card-1-4 required-type CHM1R chassis-name 1 slot-name 2`. Optional attributes are required-subtype, card-mode, subslot-name, power-profile, alias-name, admin-state, alarm-report-control and label. chassis-name defaults to 1. Note that alarm-report-control on a card defaults to inhibited rather than allowed, so alarms from a newly provisioned card are suppressed until you change it. category reports what kind of card it is: line-card, fan, power-supply, carrier-card, blank or other.

Source: `06-operation-commands/040-card.md`

### chassis-ambient-temperature-zr

*equipment-inventory / pre-condition*

**Q.** A Native ZR pluggable will not provision and I am getting a port config mismatch alarm. What is wrong?

**A.** The chassis ambient temperature is probably not set. The guide states that on the G40, to bring up a Native ZR TOM the user needs to set the chassis ambient temperature to 40C, and that if this parameter is not set the system raises a port config mismatch alarm and the TOM cannot be provisioned. The attribute is configured-ambient-temperature on the chassis object. The same object also carries expected-pem-type (DC by default), expected-fan-type (counter-rotating by default), power-redundancy (one-plus-one by default), the PEM voltage thresholds and the dust filter maintenance settings.

Source: `06-operation-commands/047-chassis.md`

### console-baud-rate-default

*equipment-inventory / how-to-default*

**Q.** What baud rate does the craft console use, and can it work it out by itself?

**A.** The default differs by platform: 9600 on the G30 and 115200 on the G40. baud-rate also accepts auto-sensing, plus 19200, 38400 and 57600, and auto-sensing-state then reads sensing or locked, showing whether the node has settled on a rate. actual-baud-rate reports what is in use. local-switch is use-global-switch, force-enable or force-disable, defaulting to use-global-switch, so an individual console follows the system-wide serial-console global-switch unless forced.

Source: `06-operation-commands/060-console.md`

### controller-card-redundancy

*equipment-inventory / which-attribute*

**Q.** How do I check that the standby controller is synchronised and ready to take over?

**A.** `show controller-card-<name>` reports redundancy-status as active, standby or not-in-service, and redundancy-standby-status which distinguishes ready-synchronized, meaning the standby is in sync and ready, from not-ready-synchronizing, which is the default and means it is still catching up. It also gives number-of-switchover-events and time-of-last-switchover, so you can see how often the pair has switched. To force a switchover, use manual-switchover, which warns that the management connection will be lost.

Source: `06-operation-commands/061-controller-card.md`

### equipment-show-filter

*equipment-inventory / minimal-command*

**Q.** How do I list the equipment installed in the node, or just one card?

**A.** `show equipment [<option>]` displays installed equipment information, and the option narrows it to one object, for example show equipment card-1-1. For a dashboard-style view with temperature and power per card, use status equipment instead; for hardware detail such as serial numbers use inventory.

Source: `06-operation-commands/092-equipment.md`

### equipment-policies-auto-migration

*equipment-inventory / how-to-default*

**Q.** Will the node update a pluggable's subtype by itself when I swap hardware, and are degrees assigned automatically?

**A.** tom-auto-migration on equipment-policies controls automatic update of the TOM subtype based on the present equipment. Direction assignment is automatic by default: auto-assigned-directions defaults to enabled, while auto-assigned-degrees defaults to disabled, except that it defaults to enabled when l0-mode-op is hsc-ols. cable-id-control defaults to enabled when l0-mode-op is slte and disabled in standard mode. chassis-assignment-mode is manual, and comm-eth-location chooses between front and back DCN placement per chassis family.

Source: `06-operation-commands/093-equipment-policies.md`

### equipment-templates-serdes-switch

*equipment-inventory / how-to-default*

**Q.** Are SerDes templates applied automatically, or do I have to enable that?

**A.** You have to enable it. use-serdes-templates on equipment-templates is enabled or disabled and defaults to disabled, so `set equipment-templates use-serdes-templates enabled` turns on the association between equipment and the configured serdes templates. The templates themselves are defined with serdes-template per TOM part number, with their parameter values in serdes-template-entry.

Source: `06-operation-commands/094-equipment-templates.md`

### fru-info-packaged

*equipment-inventory / which-command*

**Q.** Where do I find the packaged FRU information for a given equipment type?

**A.** `show fru-info-<manifest-file>/<equipment-type>` displays the packaged FRU information associated with a particular equipment type. Being keyed by manifest file, it reflects what the software load knows about that FRU rather than what is physically installed; for the installed article use inventory, which gives serial-number, clei, vendor, part-number and manufacture-date.

Source: `06-operation-commands/114-fru-info.md`

### inventory-serial-and-clei

*equipment-inventory / which-command*

**Q.** Where do I get the serial number and CLEI code of an installed module?

**A.** From inventory, which shows the inventory data for a present FRU. `show inventory-<card-name>-<port-name>` or the slot form reports serial-number, clei, vendor, part-number, manufacture-date and insertion-date, plus hardware-version, actual-type, actual-subtype, sw-support-revision, PON, number-of-lanes, vendor-compliance-code, actual-power-class (1 to 8) and actual-max-power-draw. fw-status here reads not-applicable, current, not-current or unavailable. For hardware detected but not yet accepted in a multi-chassis node, use unprovisioned-inventory.

Source: `06-operation-commands/137-inventory.md`

### led-status-values

*equipment-inventory / enumeration*

**Q.** What colours and states can a module LED report, and does the object exist if the card is missing?

**A.** status reads not-available, off, yellow, flashing-yellow, green, flashing-green, red, flashing-red, amber, flashing-amber, cycling and cycling-with-off, defaulting to not-available. The guide states the object exists even if the FRU is not physically present, so a not-available status can simply mean nothing is fitted. It is addressed as led-<location>/<name>, and show leds lists them all.

Source: `06-operation-commands/152-led.md`

### port-types-and-usage

*equipment-inventory / interpretation*

**Q.** What kinds of port exist on this platform and where are they found?

**A.** The guide states that on the G40 there are three types of port: comm-eth, USB and trib/line. comm-eth and USB ports are on XMMs only and are found on Slot 1 XMMs, while the trib and line ports are on the other cards. Configurable attributes on a port are alias-name, admin-state, alarm-report-control, label, connected-to, external-connectivity, diverse-routing and port-usage; the guide suggests `set port ?` to list them. Read-only attributes include port-type, direction, parent-port, subport-list, hosted-interface, supported-type and installed-type.

Source: `06-operation-commands/246-port.md`

### resources-switch-bandwidth

*equipment-inventory / which-attribute*

**Q.** How do I see how much switching bandwidth is left on a card?

**A.** `show resources-<name>`, for example show resources-254-1, reports internal-cell-switch-total-bandwidth and internal-cell-switch-available-bandwidth, plus paired-slot-total-bandwidth and paired-slot-available-bandwidth. The guide notes the cell switch values default to 600.000 Gbit/s when the SPN2 or SPN2C card has a paired slot, and the paired-slot values default to 0 when it does not. The same object also lists supported-carriers, unassigned-carriers and supported-sub-components.

Source: `06-operation-commands/264-resources.md`

### serdes-parameter-status

*equipment-inventory / which-attribute*

**Q.** I set a SerDes parameter. How do I know it actually took effect?

**A.** Read status on the serdes object, which reads set when the parameter is applied, unknown, in-progress, failed or not-supported. It is addressed per port and parameter name as serdes-<card-name>-<port-name>/<serdes-name>, and value carries the setting. The default status is unknown. To apply a set of parameters automatically to third-party pluggables, define a serdes-template instead of setting each one.

Source: `06-operation-commands/282-serdes.md`

### serdes-template-scope

*equipment-inventory / interpretation*

**Q.** How do SerDes templates decide which pluggables and ports they apply to?

**A.** By TOM part number. The guide states serdes-templates are created by the user per tom-part-number and apply to all line cards that support serdes, and that when a TOM is plugged in with that part number the template is automatically applied. You can narrow the scope with ports-applicable, which accepts all or up to 20 port name strings of 1 to 16 characters, and card-types-applicable. The individual parameter values inside the template are added as serdes-template-entry records. The whole mechanism only takes effect if use-serdes-templates on equipment-templates is enabled.

Source: `06-operation-commands/283-serdes-template.md`

### serdes-template-entry-structure

*equipment-inventory / how-to*

**Q.** How do I put an individual parameter into a SerDes template?

**A.** Add a serdes-template-entry, which the guide describes as composed of a serdes parameter name and its associated value: `add serdes-template-entry-<serdes-template-name>/<serdes-template-entry-name> value <value>`. set changes the value, delete removes the entry, and the show form takes a further name key. Entries belong to a serdes-template, which is keyed by TOM part number.

Source: `06-operation-commands/284-serdes-template-entry.md`

### serial-console-global-timeout

*equipment-inventory / parameter-values*

**Q.** Is there a system-wide switch and idle timeout for the serial consoles?

**A.** Yes, on serial-console, which holds the global configuration of all serial console ports. global-switch is enabled or disabled and defaults to enabled, and global-timeout is a number of minutes defaulting to 60, for example `set serial-console global-switch enabled global-timeout 80`. Individual ports follow this through the console object's local-switch attribute unless it is set to force-enable or force-disable. serial-console is system managed and cannot be manually deleted.

Source: `06-operation-commands/285-serial-console.md`

### slot-current-equipment

*equipment-inventory / which-attribute*

**Q.** How do I find out what is currently sitting in a given slot and what could go there?

**A.** `show slot-<chassis-name>-<slot-name>` reports current-equipment, the name of the equipment presently in the holder, alongside supported-type and installed-type, plus AID, oper-state and avail-state. There is also a card-scoped form, show slot-<card-name>.<slot-name>, for slots within a card. For the full capability of a slot by chassis type, including possible-card-types and whether it requires a blank when empty, use supported-slot.

Source: `06-operation-commands/295-slot.md`

### sub-component-view

*equipment-inventory / minimal-command  (weak: the source section is thin)*

**Q.** How do I list the sub-components of a card?

**A.** `show sub-component-<card-name>/<sub-component-name>` shows the sub-component details or card resources, reporting AID and description. The restart command's guidance points at show card resources for the list of sub-components that can be restarted, and restart accepts a sub-component parameter to reboot just that part.

Source: `06-operation-commands/314-sub-component.md`

### supported-card-node-compatibility

*equipment-inventory / which-attribute*

**Q.** How do I check whether a card type can be used on an in-line amplifier node before I order it?

**A.** node-type-compatibility on supported-card reads all, ILA or OADM, so it says directly which node types accept the card. The same object reports supported-subtype, default-card-mode and supported-card-mode, card-width and card-height, is-field-replaceable, category, grid-mode-support (defaulting to general-c-band), max-power-draw, leds, location-led-support, console-port-support (no, yes-with-auto-sensing-baud-rate or yes-with-fixed-baud-rate, defaulting to no), default-console-baud-rate, support-serdes-config (default false), supported-bands and supported-features.

Source: `06-operation-commands/321-supported-card.md`

### supported-chassis-redundancy

*equipment-inventory / which-attribute*

**Q.** How do I tell whether a chassis type supports controller redundancy and how many slots it has?

**A.** `show supported-chassis-<chassis-type>` reports controller-redundancy-supported and power-control-supported as true or false, along with number-of-front-slots and number-of-rear-slots, depth and height, supported-subtype and default-subtype, supported-subchassis-type, leds and supported-features. It also covers physical maintenance: fan-adjustment-on-altitude, default false, and dust-filter-replacement, which reads not-applicable, optional-dust-filter or dust-filter-regularly-replaced, defaulting to optional-dust-filter. Chassis types include G31, G32, G34c and G42.

Source: `06-operation-commands/323-supported-chassis.md`

### supported-port-auto-migration

*equipment-inventory / which-attribute*

**Q.** How do I find out what a port on a given card type is for, and whether it is configured by me or by the system?

**A.** `show supported-port-<card-type>/<port-name>` reports port-type, direction (not-applicable, tx, rx or rxtx for a layer 0 port), faceplate-label, present (always or in-node-controller-only), default-tom, parent-port, subport-list, leds and allows-auto-migration, which defaults to true. configuration-mode distinguishes system-configured from user-configured, which tells you whether you provision the port yourself.

Source: `06-operation-commands/325-supported-port.md`

### supported-slot-blank-required

*equipment-inventory / which-attribute*

**Q.** Do empty slots need a blank fitted, and which cards can go where?

**A.** `show supported-slot-<chassis-type>/<slot-name>` answers both: requires-blank-when-empty says whether a filler is needed, and possible-card-types lists what the slot accepts, with default-card naming the usual occupant. It also reports slot-location, slot-vertical-position and slot-horizontal-position, auto-provision-capable, reset-power, leds, virtual-slot (default false) and configuration-mode of system-configured or user-configured. A card-scoped form exists for slots inside a card.

Source: `06-operation-commands/327-supported-slot.md`

### supported-tom-phy-modes

*equipment-inventory / enumeration*

**Q.** How do I find which breakout modes a pluggable supports on a given card and port?

**A.** `show supported-tom-<card-type>/<port-name>/<tom-type>/<tom-subtype-group>` reports supported-phy-modes and default-phy-mode. The phy modes include 100GE, 200GE, 400GE, 2x100GE, 4x100GE, 4x10GE, 40GE, 10GE, 1GE and their non-Ethernet equivalents, so the breakout options are visible here. tom-subtype-group is a string such as 4x100GE-breakout, and tom-type covers CFP2-DCO, QSFP28, QSFPDD and similar. The power side is a separate lookup, supported-tom-power, which gives supported-power-class from 1 to 8 and supported-max-power-draw.

Source: `06-operation-commands/328-supported-tom.md`

### supported-tom-power-class

*equipment-inventory / which-command*

**Q.** How much power is a pluggable allowed to draw in a given port?

**A.** `show supported-tom-power-<card-type>/<port-name>/<tom-type>` reports supported-power-class, an integer from 1 to 8, and supported-max-power-draw. The corresponding values for the module actually fitted appear on inventory as actual-power-class and actual-max-power-draw, and the tom object has power-class-override, disabled by default, for the cases where you need to force acceptance.

Source: `06-operation-commands/329-supported-tom-power.md`

### tom-create-and-side-effects

*equipment-inventory / consequence*

**Q.** When I provision a pluggable, does anything else get created with it?

**A.** Yes. The guide states the Tributary Port termination point is auto-created when the TOM is created, and automatically deleted when the TOM is deleted, so you should not create trib-ptp by hand. required-type is mandatory on add and covers CFP2-DCO, OSFP, QSFP28, QSFPDD, QSFPDD-ZR, QSFPPLUS, SFP and SFPPLUS, for example `add tom-1-4-T1 required-type QSFPDD`. Optional attributes include required-subtype, phy-mode, power-class-override (disabled by default), enable-serdes (false by default), power-mode (powered by default) and the usual alias, label and state fields. upgrade-status reports idle, in-progress, success or failed for a firmware update on the module.

Source: `06-operation-commands/352-tom.md`

### tom-type-third-party

*equipment-inventory / which-attribute*

**Q.** How do I tell whether a pluggable form factor accepts third-party modules?

**A.** `show tom-type-<tom-type>` reports support-third-party-toms as true or false, along with data-rate, description and generic-subtype. That flag matters because the SerDes template mechanism exists specifically to auto-configure third-party TOMs, keyed by tom-part-number.

Source: `06-operation-commands/353-tom-type.md`

### unprovisioned-inventory-multi-chassis

*equipment-inventory / which-command*

**Q.** We racked a new shelf and it is not showing in the inventory. Where does it appear first?

**A.** In unprovisioned-inventory, which shows detected inventory not yet accepted by the Node Controller in a multi-chassis configuration. It is keyed by chassis serial number and slot name and reports hardware-version, actual-type, actual-subtype, sw-support-revision, PON, serial-number, clei, vendor, part-number, manufacture-date and detection-timestamp, so you can identify hardware before it is provisioned. Once accepted it appears under inventory.

Source: `06-operation-commands/361-unprovisioned-inventory.md`

### usb-space-and-path

*equipment-inventory / which-command*

**Q.** How do I find where a USB stick is mounted and how much room is left on it?

**A.** `show usb-<card-name>-<port-name>`, for example show usb-1-5-U1, reports usb-path, available-space and total-space, along with type and present, which defaults to false when nothing is fitted. The download command's guidance points at show usb to see where a particular USB drive is mounted.

Source: `06-operation-commands/366-usb.md`

### activate-snapshot-passphrase

*config-datastore / how-to*

**Q.** How do I bring a saved database snapshot back into service?

**A.** `activate-snapshot db-instance <string> db-paraphrase <string> [sanity-check-override <true|false>]`. The db-instance names which backup slot to restore, defaulting to temp, and the passphrase is the one used to encrypt the snapshot. sanity-check-override defaults to false, so the system's checks apply unless you deliberately bypass them. The guide lists a pre-condition on this command: a snapshot must have been taken to activate it, which is done with take-snapshot. It runs in operational mode.

Source: `06-operation-commands/009-activate-snapshot.md`

### advanced-parameter-prerequisite

*config-datastore / pre-condition*

**Q.** I set an advanced parameter but nothing happened, and showing it gives an error. What is going on?

**A.** Two things to check. First, the guide states advanced parameters can only be executed on the node after enable-advanced-parameters on the optical carrier is set to true. Second, the error behaviour is documented: if no advanced parameter is configured, or it was deleted, show advanced-parameter reports that the object does not exist; and if one is configured with an invalid value the configuration is not rejected, but show reports a failed status. status reads failed, in-progress, not-supported, set or unknown. A parameter is added as advanced-parameter-<port or carrier>/<name> with a value, for example add advanced-parameter-1-6-L1-1/FFCRAvgN value 3.

Source: `06-operation-commands/013-advanced-parameter.md`

### apply-template-scope

*config-datastore / scope-limit*

**Q.** How do I push the SerDes templates onto pluggables that are already installed?

**A.** `apply-template [template-type=]<value> ([[applicable-tom=]<value>[,<value>]*])`. With template-type serdes-template it applies all existing serdes-templates to the TOMs named in applicable-tom; if the list is not provided, all system TOMs are considered. That is the manual path, since templates otherwise apply automatically when a TOM with a matching part number is plugged in, and only when use-serdes-templates is enabled on equipment-templates.

Source: `06-operation-commands/023-apply-template.md`

### commit-confirmed-timeout

*config-datastore / consequence*

**Q.** Is there a way to commit a change that undoes itself if I lose access to the node?

**A.** Yes, a confirmed commit. `commit confirmed [confirm-timeout=<timeout>] [-id=<id>]` commits the candidate as usual, but if a confirmation does not arrive before the timeout elapses the configuration is reverted. You confirm it with commit persist, or abandon it with commit cancel. A plain commit commits without that safety net, and commit -m is the merge form. The command runs in Candidate Configuration mode only. Note that the commit repository shown by show commit is available only when commit-tracking is enabled on system-policies, or while a pending confirmed commit exists.

Source: `06-operation-commands/055-commit.md`

### config-as-restore-script

*config-datastore / how-to*

**Q.** How do I capture the node's configuration in a form I could replay onto it later?

**A.** The guide's own tip is to use `show config | display commands` and store the result for later usage, which produces a CLI script that can restore the system configuration. show config displays only non-default configuration, skipping anything left at its default value, and is fully recursive from the current CLI scope, so running it at the top of the hierarchy gives the complete system configuration. It optionally takes an entity-id or entity-type to narrow the scope.

Source: `06-operation-commands/056-config.md`

### configure-exclusive-vs-shared

*config-datastore / comparison*

**Q.** When I enter configuration mode, can someone else edit at the same time?

**A.** That depends which mode you pick. exclusive means only this session can make changes to the candidate configuration; shared means multiple sessions can. exclusive is the default target. The command also seeds the candidate datastore: from-default starts from defaults, from-script initialises from a script, and from-commit starts from a previous commit. A typical entry is `configure exclusive`. Note that diff candidate must be run in Exclusive Candidate Configuration mode.

Source: `06-operation-commands/057-configure.md`

### current-advanced-parameter-running

*config-datastore / disambiguation*

**Q.** How do I see the advanced parameter values actually running on the system rather than what I configured?

**A.** `show current-advanced-parameter-<optical-carrier-name>/<current-advanced-parameter-name> [value]` shows the current values of the advanced parameters running on the system. That is distinct from advanced-parameter, which holds what you configured along with a status of set, failed, in-progress, not-supported or unknown, and from golden-advanced-parameter, which holds the system's own reference values.

Source: `06-operation-commands/065-current-advanced-parameter.md`

### database-clear-scope

*config-datastore / scope-limit*

**Q.** Does clearing the database wipe everything on the node?

**A.** No. The guide states clear database does not wipe logs, PM data and other non-configuration data, and points at clear system when a secure wipe or factory default is needed. What it does do is set the NE database to default and reboot the system, which is potentially traffic affecting; the guide also warns that transient alarms may be reported after the controller card reboot and recommends ignoring them. clear-type is full, keep-networking or initialize-from-script, defaulting to full, with script, new-admin-user and new-admin-password supporting the script path. show database lists the databases with database-state, database-version, backup-time and related details.

Source: `06-operation-commands/072-database.md`

### db-migrate-encryption-modes

*config-datastore / parameter-values*

**Q.** How do I move the database to a scheme that also protects integrity?

**A.** `db-migrate [-f] [type=]<value>` where type is encryption or encryption-with-integrity, for example db-migrate -f encryption-with-integrity. To see what is currently in force, show db-protection-scheme reports mode as encryption-only or encryption-with-integrity, together with encryption-algorithm, integrity-algorithm (hmac-sha2-512 or none) and integrity-status, which reads passed-on-bootup, failed-on-bootup or disabled. db-migrate runs in operational mode.

Source: `06-operation-commands/073-db-migrate.md`

### db-protection-scheme-integrity

*config-datastore / which-attribute*

**Q.** How do I check that the database integrity check passed at boot?

**A.** `show db-protection-scheme` reports integrity-status as passed-on-bootup, failed-on-bootup or disabled, so failed-on-bootup is the value that matters. It also shows mode (encryption-only or encryption-with-integrity), encryption-algorithm and integrity-algorithm, which is hmac-sha2-512 or none. To change the scheme, use db-migrate.

Source: `06-operation-commands/074-db-protection-scheme.md`

### diff-output-forms

*config-datastore / parameter-values*

**Q.** Before I commit, how do I see exactly what I changed, and can I get it as commands?

**A.** `diff [-t|-c] candidate` gives three presentations: the default normal diff style where + marks added objects or new values and - marks deleted objects or old values; a side-by-side diff with -t; and with -c, the CLI commands that would perform the same configuration as the candidate datastore. The guide states this command must be run in Exclusive Candidate Configuration mode. There is also `diff commit <id> [<id>]` for comparing commit records.

Source: `06-operation-commands/080-diff.md`

### discard-changes-effect

*config-datastore / consequence*

**Q.** How do I throw away everything I have staged and get back to normal mode?

**A.** `discard-changes` discards all candidate datastore content and returns the CLI to operational mode, so it both empties the candidate and exits configuration mode in one step. It takes no parameters and is available in Candidate Configuration mode only. If you want to keep the changes instead, use commit, or commit confirmed for a change that reverts itself unless confirmed.

Source: `06-operation-commands/082-discard-changes.md`

### extended-config-global-effect

*config-datastore / consequence*

**Q.** What is an extended config and how careful should I be with one?

**A.** Careful. The guide describes it as configuring a non-standard extended config that introduces exceptional behaviour globally in the system, and states it requires knowledge of the extended-config name on the user side, meaning there is no discovery of valid names from the CLI. The object supports only add, delete and show, with a description attribute; there is no set form, so an extended config is added or removed rather than edited.

Source: `06-operation-commands/103-extended-config.md`

### golden-advanced-parameter-impact

*config-datastore / which-attribute*

**Q.** How do I tell whether changing an advanced parameter will disturb traffic?

**A.** The golden-advanced-parameter entry for it carries both a configuration-impact and a service-impact. service-impact reads service-affecting or non-service-affecting, and configuration-impact reads no-change, no-reacquire, reacquire, full-config-pll-change or full-config-no-pll-change, so a reacquire or full-config value tells you the link will be disturbed. Those are the reference values from the system; what you have configured is on advanced-parameter and what is running is on current-advanced-parameter.

Source: `06-operation-commands/119-golden-advanced-parameter.md`

### lock-exclusive-write

*config-datastore / consequence*

**Q.** How do I stop anyone else changing configuration while I work, and what releases it?

**A.** `lock` grants exclusive write access to the current CLI session; while the database is locked another session that tries to configure receives an error. The guide states it is intended for ensuring configuration mastership for a small time. It is released either by the unlock command or by the locking session closing, whether the user closes it, an administrator closes it, or it times out through inactivity. Only the session that performed the lock can unlock it. Both commands run in operational mode.

Source: `06-operation-commands/162-lock.md`

### named-value-set-structure

*config-datastore / how-to  (weak: the source section is thin)*

**Q.** How is a named-value-set addressed and what does it hold?

**A.** It is keyed by database entry name and set name: `add named-value-set-<db-entry-name>/<named-value-set-name> [value <value>]`, with matching set, show and delete forms. The only attribute is value. The guide gives no further description of its purpose beyond that it adds, sets, shows and deletes the named-value-set attributes.

Source: `06-operation-commands/184-named-value-set.md`

### recovery-from-chassis-storage

*config-datastore / how-to-default*

**Q.** Will a replacement controller pick up the old configuration by itself?

**A.** That is governed by restore-from-chassis-storage on the recovery object, which is disabled, auto-restore or auto-in-service and defaults to auto-in-service. show recovery also reports restore-status, which walks through init, image-install-in-progress, db-restore-in-progress, check-completed, failed, disabled and waiting states, plus backup-status of successful, failed, in-progress or unknown, and the last-backup and next-backup timestamps, both of which read never until a backup has run.

Source: `06-operation-commands/261-recovery.md`

### rollback-commit-conditions

*config-datastore / pre-condition*

**Q.** Can I roll back to a previous commit while I have changes staged?

**A.** Not if the candidate has content. The guide states rollback works both in the Running Datastore, which is the default, and in the Candidate Datastore, but the rollback operation can be performed on the Candidate Datastore only if it is empty. The command must be executed with the commit parameter and optionally a specific commit-id; if no id is given the most recent commit record is used. The commit records themselves are visible through show commit, which requires commit-tracking to be enabled on system-policies or a pending confirmed commit to exist.

Source: `06-operation-commands/268-rollback.md`

### show-commit-availability

*config-datastore / pre-condition*

**Q.** Why does the commit history come back empty on our node?

**A.** Because the commit repository is conditional. The guide states show commit is available only if the commit-tracking policy is enabled, or if a pending confirmed commit exists. commit-tracking is set on system-policies and defaults to disabled, so on a default node there is no history to show. Once enabled, show commit takes an id, -s=<since> or -n=<number-of-records> to narrow the records returned.

Source: `06-operation-commands/292-show-commit.md`

### system-policies-commit-tracking

*config-datastore / how-to-default*

**Q.** How do I turn on commit history, and is the running datastore writable?

**A.** Both are flags on system-policies, and both default to disabled. commit-tracking enables the Commit Repository, without which show commit and much of the rollback history are unavailable; the guide notes the commands associated with the Commit Repository are only available if commit-tracking is enabled. writable-running controls whether the running datastore can be written directly rather than through the candidate. An example of enabling tracking is `set system-policies commit tracking enabled`.

Source: `06-operation-commands/341-system-policies.md`

### take-snapshot-passphrase-required

*config-datastore / pre-condition*

**Q.** My attempt to take a database backup was refused. What is missing?

**A.** Almost certainly the passphrase. The guide states the system will only accept the command if the db-passphrase used for snapshot encryption is configured, either globally as part of security-policies or locally as a parameter of the command. The passphrase must be a minimum of 40 characters, must not contain dictionary words, and allows special characters. db-instance selects the slot from onehour, oneday, oneweek, temp, manual or rollback, defaulting to temp, and the guide warns that if the oneweek snapshot already exists the command overwrites it. Snapshots are then activated with activate database or exported with upload database.

Source: `06-operation-commands/342-take-snapshot.md`

### template-entry-definition

*config-datastore / how-to*

**Q.** How do I make the node use my own default value for an attribute on newly created objects?

**A.** Define a template entry. A template is an object and attribute pair plus the value to be used as the default for that attribute, and the guide describes it as an individual rule for defining a default value for a given attribute. It is added inside a group: `add template-<template-group-name>/<template-name> object <value> attribute <value> value <value>`, with object, attribute and value all mandatory. sequence-id, from 1 to 65535, orders the rules, condition restricts when the rule applies, and label is free text. The group itself is created with template-group, and templates lists them.

Source: `06-operation-commands/345-template.md`

### template-group-enabled-exclusive

*config-datastore / how-to-default*

**Q.** Can I have two template groups active at the same time?

**A.** The guide's default suggests not: enabled on template-group defaults to true only if there is no other template-group enabled, which implies one enabled group at a time. A group is a configuration group containing a list of template entries, created with `add template-group-<name> [enabled <value>] [label <value>]`, for example add template-group-1. Note there is no set form documented, only add, show and delete. The individual rules inside it are template objects, and show templates displays the overall configuration.

Source: `06-operation-commands/346-template-group.md`

### templates-overview

*config-datastore / minimal-command  (weak: the source section is thin)*

**Q.** Which command gives the overall view of the templates configured on the node?

**A.** `show templates` shows the configuration that defines the data model for system templates. It takes no parameters. Underneath it, template-group holds each group with its enabled flag, and template holds the individual object, attribute and value rules with their sequence-id and condition.

Source: `06-operation-commands/347-templates.md`

### unlock-only-owner

*config-datastore / scope-limit*

**Q.** Someone locked the database and went home. Can I unlock it from my session?

**A.** No. The guide states only the session that performed the lock can trigger the unlock, and that trying to unlock a database that is not locked, or one locked by someone else, is an error case. After a successful unlock any session can configure again and can take a new lock. The other way a lock is released is by the locking session closing, whether by the user, by an administrator closing the session, or by inactivity timeout. unlock runs in operational mode.

Source: `06-operation-commands/360-unlock.md`

### validate-command-string

*config-datastore / how-to*

**Q.** Can I check that a configuration command is valid without actually applying it?

**A.** Yes: `validate candidate <candidate> command <string>` validates any CLI command used to edit a configuration datastore by creating, deleting, merging or replacing content, for example validate 'set ne altitude 600'. It is available in operational and candidate configuration mode. Note there is also a -v flag on several commands, described elsewhere in the guide as performing command validation only, where a valid command replies OK and the target entity is not created.

Source: `06-operation-commands/370-validate.md`

### additional-key-exchange-rounds

*encryption-ipsec-macsec / pre-condition*

**Q.** How many post-quantum key exchange rounds can I add to an IKE association, and what happens if the two ends disagree?

**A.** A maximum of 7 rounds of additional-key-exchange can be configured, each with a unique key exchange algorithm, and the guide states the rounds must be configured in the same order on both IKE peers; if the configuration does not match, an IKE-CONFIG-MISMATCH alarm is raised. Configuring them is optional, and port level key exchange for the Child SA is derived from IKE. dh-group covers the classic groups dhe-2048 through dhe-8192, the elliptic curve groups ecp-256, ecp-384, ecp-521, curve-25519 and curve-448, and the post-quantum ml-kem family, for example add additional-key-exchange-1-6/NEA/1/2 dh-group ml-kem-768.

Source: `06-operation-commands/011-additional-key-exchange.md`

### data-path-encryption-view

*encryption-ipsec-macsec / minimal-command  (weak: the source section is thin)*

**Q.** Which command shows the datapath encryption settings?

**A.** `show data-path-encryption` shows the datapath encryption attributes. It takes no parameters. The objects that actually carry the configuration are secure-entity for the encrypted entity itself, secure-application for the certificate identity, secure-entity-sa-proposal for the negotiated algorithms, and the ikev2 family for key exchange.

Source: `06-operation-commands/071-data-path-encryption.md`

### encryption-algorithm-choices

*encryption-ipsec-macsec / enumeration*

**Q.** Which ciphers and key lengths can I propose for an IPsec association?

**A.** algorithm offers null, aes-gcm-8, aes-gcm-12, aes-gcm-16, aes-ctr, aes-cbc, aes-ccm-8, aes-ccm-12, aes-ccm-16 and chacha20-poly1305. key-length is none, key-length-128, key-length-192 or key-length-256, defaulting to none. Both are part of the instance key rather than settable attributes, so a proposal is created by adding the full path, for example `add encryption-algorithm-ipsec/GX2/1/aes-gcm-8/key-length-192`, and removed with delete. There are two forms, one scoped to an SPD entry and one at peer level.

Source: `06-operation-commands/091-encryption-algorithm.md`

### ike-sa-proposal-required

*encryption-ipsec-macsec / how-to*

**Q.** What has to be specified when proposing IKEv2 parameters?

**A.** integrity-algorithm and dh-group are mandatory on add; prf is optional. integrity-algorithm offers none, hmac-sha2-256-128, hmac-sha2-384-192, hmac-sha2-512-256, hmac-sha1-160 and hmac-sha1-96, and prf offers hmac-sha2-256, hmac-sha2-384, hmac-sha2-512 and hmac-sha1. protocol-id is fixed at IKE. A worked example is `add ike-sa-proposal-ipsec/GX2/1 dh-group dhe-2048 prf hmac-sha2-256 integrity-algorithm hmac-sha2-384-192`. The cipher itself is a separate object, encryption-algorithm.

Source: `06-operation-commands/127-ike-sa-proposal.md`

### ikev2-san-id-match

*encryption-ipsec-macsec / which-attribute*

**Q.** What does the top-level ikev2 object actually configure?

**A.** Just one thing: data-path-encryption-san-ike-id-match, which is match or ignore, for example `set ikev2 data-path-encryption-san-ike-id-match match`. It decides whether the subject alternative name in the certificate has to match the IKE identity for data path encryption. Everything else lives on ikev2-local-instance, ikev2-peer and the proposal objects.

Source: `06-operation-commands/128-ikev2.md`

### ikev2-local-instance-scope

*encryption-ipsec-macsec / parameter-values*

**Q.** How do I tell whether a card can do encryption, and how is the local IKE address chosen?

**A.** host-card-encryption-capability on ikev2-local-instance reads yes, no or unknown, defaulting to unknown, so it answers the capability question directly. The address is automatic by default: local-address-assignment-method is auto or manual, defaulting to auto, and local-address defaults to 0.0.0.0 until assigned, for example `set ikev2-local-instance-1-3 local-address 1.1.180.132`. scope selects what the instance is for, including data-path-encryption and management-ipsec. Note alarm-report-control defaults to inhibited on this object.

Source: `06-operation-commands/129-ikev2-local-instance.md`

### ikev2-peer-rekey-vs-reauth

*encryption-ipsec-macsec / pre-condition*

**Q.** What re-key and re-authentication intervals should I use on an IKE peer, and is there a rule about them?

**A.** Yes, an explicit one. The guide states re-key-frequency and re-auth-frequency must not be multiples of each other, and must differ by a few minutes to ensure a significant interval between re-authentication and re-keying. re-key-frequency covers 3600 to 86400 seconds and defaults to 28800; re-auth-frequency covers 3600 to 604800 and defaults to 43200. re-key-fail-policy is kill traffic or continue traffic, defaulting to continue-traffic, with re-key-traffic-kill-offset and re-auth-traffic-kill-offset both defaulting to 0. Other defaults: dpd-delay 30, keying-tries infinite, port 500, authentication-scheme x.509-certificate, psk-lifetime 90 days with a 14 day expiry warning. The guide also notes sms-operation and sms-state are not supported in Release 9.1 even though they appear in the interface.

Source: `06-operation-commands/130-ikev2-peer.md`

### ipsec-sa-proposal-esp

*encryption-ipsec-macsec / how-to*

**Q.** What do I supply when proposing the child IPsec association parameters?

**A.** dh-group is mandatory on add and integrity-algorithm is optional, for example `add ipsec-sa-proposal-ipsec/GX2/dns/1 dh-group dhe-3072 integrity-algorithm hmac-sha2-512-256`. protocol-id is fixed at ESP, distinguishing this from the IKE proposal where it is IKE, and esn is fixed at esn. The integrity algorithms are the same set as for IKE: none, hmac-sha2-256-128, hmac-sha2-384-192, hmac-sha2-512-256, hmac-sha1-160 and hmac-sha1-96. Note the instance key includes the SPD entry name, so a proposal belongs to a policy entry.

Source: `06-operation-commands/139-ipsec-sa-proposal.md`

### ipsec-sa-re-key-limits

*encryption-ipsec-macsec / parameter-values*

**Q.** Can I make an IPsec association re-key on volume rather than on time?

**A.** Yes, on both. ipsec-sa-re-key carries frequency in seconds, 3600 to 86400 with a default of 14400, bytes from 1048576 upward with a default of 1073741824, which is one gigabyte, and packets, which is disabled by default but accepts a very large count. So by default the association re-keys every four hours or every gigabyte, whichever comes first, with no packet-based trigger.

Source: `06-operation-commands/140-ipsec-sa-re-key.md`

### ipsec-spd-entry-action

*encryption-ipsec-macsec / parameter-values*

**Q.** What can a security policy entry do with matching traffic, and does it tunnel by default?

**A.** action is protect, bypass or discard, defaulting to protect, and mode is tunnel or transport, defaulting to tunnel. priority is mandatory on add and orders the entries, for example `add ipsec-spd-entry-ipsec/GX2/dns priority 1 action protect`. ipsec-protocol is ESP. Replay protection is on: esn defaults to true and anti-replay-window covers 32 to 1024 with a default of 64. dynamic-ts defaults to disabled. Unusually for this domain, alarm-report-control here defaults to allowed rather than inhibited.

Source: `06-operation-commands/141-ipsec-spd-entry.md`

### ipsec-traffic-selector-purpose

*encryption-ipsec-macsec / which-command*

**Q.** How do I say which addresses and ports an IPsec policy applies to?

**A.** With an ipsec-traffic-selector, which hangs off the SPD entry and is then populated by four child objects: local-subnet and remote-subnet each add a prefix, and local-ports and remote-ports each add a start and stop port range. For example add local-subnet-ipsec/GX2/dns/ts1/101.10.10.1/32 and add remote-ports-ipsec/GX2/protect1/ts1/49/49. The port objects accept all or opaque as well as a number, so add remote-ports-ipsec/GX2/protect1/ts1/all selects everything.

Source: `06-operation-commands/142-ipsec-traffic-selector.md`

### local-ports-range-keys

*encryption-ipsec-macsec / interpretation*

**Q.** Why can I not edit a local port range on a traffic selector, only add and delete it?

**A.** Because the range is part of the instance key. local-ports is addressed as local-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>, so the start and stop values identify the object rather than being attributes of it; there is no set form, only add, show and delete. start accepts all, opaque or a port number, for example add local-ports-ipsec/GX2/tacacs/ts1/all. remote-ports works the same way.

Source: `06-operation-commands/160-local-ports.md`

### local-subnet-prefix-key

*encryption-ipsec-macsec / how-to*

**Q.** How do I add the local network that an IPsec tunnel should carry?

**A.** Add a local-subnet under the traffic selector, with the prefix as the last key element: `add local-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>`, for example add local-subnet-ipsec/GX2/dns/ts1/101.10.10.1/32. As with the port objects there is no set form, so changing the prefix means deleting and re-adding. The far side is remote-subnet, added the same way.

Source: `06-operation-commands/161-local-subnet.md`

### macsec-entity-must-secure

*encryption-ipsec-macsec / how-to-default*

**Q.** If MACsec fails to come up on a link, does traffic still pass?

**A.** Not by default. link-security-control is must-secure or should-secure and defaults to must-secure, so unsecured traffic is not allowed through unless you relax it to should-secure. enabled defaults to true and the entity is bound to a facility with supporting-facility, for example `add macsec-entity-6-3-3 supporting-facility ethernet-6-3-3`. replay-protection can be enabled or disabled with replay-protection-window defaulting to 0, and negotiated-cipher-suite reports what was agreed. alarm-report-control defaults to inhibited here.

Source: `06-operation-commands/171-macsec-entity.md`

### macsec-mka-cak

*encryption-ipsec-macsec / how-to*

**Q.** How do I configure the pre-shared key for MACsec key agreement, and how do I know which end is the key server?

**A.** Set connectivity-association-key and connectivity-association-key-name on macsec-mka, for example `set macsec-mka-6-3-3 connectivity-association-key 1122334411223344556677889900112233445566778899001122334455667788 connectivity-association-key-name aa`. is-key-server reports whether this end won the key server election, defaulting to false; the priority that decides it is key-server-priority on the referenced mka-policy. The key can be given a lifetime with psk-lifetime, psk-expiration-warning, which is 1 to 173 days defaulting to 14, and psk-lifetime-enable, with psk-configured-timestamp recording when it was set.

Source: `06-operation-commands/172-macsec-mka.md`

### mka-policy-attributes

*encryption-ipsec-macsec / which-attribute*

**Q.** Where do I set the MACsec cipher suite and how often the session key is refreshed?

**A.** On the mka-policy, which is the MACsec Key Agreement policy referenced by the macsec entity related to an Ethernet facility. It carries key-server-priority, macsec-cipher-suite, confidentiality-offset and sak-rekey-interval, the last covering roughly 30 to 65535. Be aware the guide's parameter table for this section looks corrupted: macsec-cipher-suite is documented with values true and false and confidentiality-offset with allowed and inhibited, which do not match their names, so verify against the node before relying on those.

Source: `06-operation-commands/173-mka-policy.md`

### re-auth-on-demand

*encryption-ipsec-macsec / how-to*

**Q.** How do I force an IKE peer to re-authenticate right now?

**A.** `re-auth [ikev2-peer=]<value>`, for example re-auth ikev2-peer=ikev2-NE202. It performs a re-authentication operation of IKEv2 security associations on demand, in operational mode. The scheduled equivalent is re-auth-frequency on the ikev2-peer, which defaults to 43200 seconds. To force new keys rather than re-authentication, use re-key.

Source: `06-operation-commands/258-re-auth.md`

### re-key-three-targets

*encryption-ipsec-macsec / enumeration*

**Q.** What can I force a re-key on?

**A.** Three things, and you name exactly one: a data path encryption secure entity with secure-entity=, an IKEv2 peer with ikev2-peer=, or an IPsec child security association, meaning a Security Policy Database entry, with ipsec-security-association=. For example `re-key secure-entity=NE202-1-4-L1-1`. It runs in operational mode. The scheduled equivalents are re-key-frequency on secure-entity and ikev2-peer, and the ipsec-sa-re-key object for child associations.

Source: `06-operation-commands/259-re-key.md`

### remote-ports-all-keyword

*encryption-ipsec-macsec / parameter-values*

**Q.** How do I match every port on the far side of an IPsec traffic selector?

**A.** Use the keyword all in place of the start value: `add remote-ports-ipsec/GX2/protect1/ts1/all`. start accepts all, opaque or a port number, while stop is a port number defaulting to 0, so a single port is expressed by repeating it, as in add remote-ports-ipsec/GX2/protect1/ts1/49/49. The near side is local-ports, addressed identically.

Source: `06-operation-commands/262-remote-ports.md`

### remote-subnet-add

*encryption-ipsec-macsec / how-to*

**Q.** How do I declare the far-end network for an IPsec policy?

**A.** Add a remote-subnet with the prefix as the final key element: `add remote-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>`, for example add remote-subnet-ipsec/GX2/dns/ts1/102.20.20.2/32. Only add, show and delete exist, so a change means deleting and re-adding. The near side equivalent is local-subnet.

Source: `06-operation-commands/263-remote-subnet.md`

### sc-rx-secure-channel

*encryption-ipsec-macsec / which-attribute*

**Q.** How do I check which MACsec receive channel is currently in use?

**A.** `show sc-rx-<name>/<index>` reports state as in-use or not-in-use, along with sci-rx, the secure channel identifier, association-number, key-identifier and next-packet-number. The transmit direction is the mirror object sc-tx, with sci-tx and the same attributes. Both are read only; the configuration lives on macsec-entity and macsec-mka.

Source: `06-operation-commands/273-sc-rx.md`

### sc-tx-transmit-channel

*encryption-ipsec-macsec / which-attribute*

**Q.** What does sc-tx report and can I configure it?

**A.** It is read only. `show sc-tx-<name>/<index>` gives sci-tx, state (in-use or not-in-use), association-number, key-identifier and next-packet-number for the transmitting secure channel. There is no set, add or delete form. Its receive counterpart is sc-rx, and the configurable objects behind both are macsec-entity, macsec-mka and mka-policy.

Source: `06-operation-commands/274-sc-tx.md`

### secure-application-certificate

*encryption-ipsec-macsec / interpretation*

**Q.** How does the node decide which certificate an application presents, and can it require one from clients?

**A.** A secure application represents an application which uses an X509v3 certificate as its digital identity, and active-certificate-id selects which certificate it presents. verify-client-cert is disabled or required, defaulting to disabled, so mutual authentication is off unless you set it to required. show reports type as server or client, in-use, and status as enabled or disabled. `show secure-applications` lists them all. The certificates themselves are managed with local-certificate, peer-certificate and trusted-certificate.

Source: `06-operation-commands/276-secure-application.md`

### secure-entity-rekey-policy

*encryption-ipsec-macsec / parameter-values*

**Q.** If a re-key fails on an encrypted wavelength, does the node drop the traffic?

**A.** By default no: re-key-fail-policy is kill-traffic or continue-traffic and defaults to continue-traffic. If you choose kill-traffic, traffic-kill-offset delays the kill by 0 to 86400 seconds, defaulting to 0, for example `add secure-entity-NE202-1-4-L1-1 supporting-facility optical-carrier-1-4-L1-1 remote-secure-entity '1-4-L1-1' re-key-fail-policy kill-traffic traffic-kill-offset 300`. re-key-frequency covers 3600 to 86400 seconds and defaults to 28800. Note enabled defaults to false, so a newly added secure entity is not encrypting until you enable it, and supporting-facility and remote-secure-entity are both mandatory.

Source: `06-operation-commands/277-secure-entity.md`

### secure-entity-sa-proposal-fixed

*encryption-ipsec-macsec / scope-limit*

**Q.** Can I choose the algorithms used for layer 1 wavelength encryption?

**A.** No. secure-entity-sa-proposal is read only and every value is fixed: number is 1, encryption-algorithm is aes-gcm-16, encryption-key-length is key-length-256, integrity-algorithm is none, since GCM provides its own integrity, and dh-group is ecp-521. So the proposal exists to be inspected rather than negotiated. Contrast this with management IPsec, where ike-sa-proposal, ipsec-sa-proposal and encryption-algorithm all offer real choices.

Source: `06-operation-commands/278-secure-entity-sa-proposal.md`

### security-policy-database-link

*encryption-ipsec-macsec / how-to*

**Q.** How is an IKE peer tied to the encrypted entity it protects?

**A.** Through security-policy-database, which is keyed by local instance and peer name and carries associated-secure-entity, for example `add security-policy-database-ikev2-local-instance-1-6/ikev2-NE202-1-4 associated-secure-entity secure-entity-1-6-L1-1`. That association is what links the key exchange to the data path encryption entity. For management IPsec the equivalent per-policy detail lives in ipsec-spd-entry with its traffic selectors.

Source: `06-operation-commands/281-security-policy-database.md`

### cdp-auto-refresh

*certificates-pki / how-to-default*

**Q.** Can the node fetch a revocation list by itself on a schedule?

**A.** Yes, that is what a CRL Distribution Point is for. Each cdp supports download and auto-refreshing of a specified CRL. url is mandatory on add, for example an http URL ending .crl, and refresh-interval accepts compound values such as 2w for two weeks or 5d 12h. enabled defaults to false, so a newly added CDP does not refresh until you enable it. show cdp reports next-update-time, last-update-time and last-update-result so you can see whether the last fetch worked. Note there are two delete forms, delete cdp and delete cdp-<name>.

Source: `06-operation-commands/042-cdp.md`

### cert-gen-self-signed

*certificates-pki / how-to-default*

**Q.** How do I create a self-signed certificate for testing, and how long is it valid?

**A.** `cert-gen [certificate-name=]<value>` generates a self-signed certificate. days defaults to 365, and auto-install defaults to true, so the certificate is installed unless you say otherwise. You can supply org-name, common-name, a full subject and SAN. A worked example is `cert-gen -f certificate-name=self-signed-cert1 days=10 org-name=testorg common-name=testcert auto-install=true`. For a CA-signed certificate instead, use csr-gen to produce a signing request, or est to enrol automatically.

Source: `06-operation-commands/043-cert-gen.md`

### cert-to-name-mapping

*certificates-pki / how-to*

**Q.** How does the node work out which user a client certificate belongs to?

**A.** Through cert-to-name, a prioritised set of rules that map an X.509 client certificate to a specific user identity. Each rule either extracts a username from a certificate field or maps it directly based on defined criteria, or on a specific issuer within the certificate's trust chain. map-type is extract or map; field selects common-name, san-any, san-dns-name, san-ip-address, san-rfc822-name, san-upn or san-uri; priority runs 1 to 255 and orders the rules; issuer restricts a rule to one issuer; enabled defaults to True. A minimal rule is `add cert-to-name-rule_name map-type extract field san-dns-name`.

Source: `06-operation-commands/044-cert-to-name.md`

### certificate-delete-and-list

*certificates-pki / how-to*

**Q.** How do I remove a certificate that was imported by mistake, and how do I list what is installed?

**A.** `clear certificate [type=]<value> [id=]<value>` deletes an imported local, trusted or peer X509v3 certificate, for example clear certificate local certX. To see what is there, show certificates displays all managed local, trusted and peer certificates that were imported by the download mechanism in a PKCS#12 or PKCS#7 secure bundle, and show certificate-revocation covers the revocations. The command runs in operational mode.

Source: `06-operation-commands/045-certificate.md`

### crl-purge-targets

*certificates-pki / parameter-values*

**Q.** How do I clear out stale revocation lists without removing the good ones?

**A.** clear crl takes a clear-target of single-crl, purge-invalid-crls, purge-cached-crls or purge-all-crls, so purge-invalid-crls removes only the ones that are no longer valid. A single named list is removed with `clear crl single-crl myCA-1`. show crl reports status as valid, expired or future and type as cached or manually, plus issuer, crl-number, effective-date, next-update, the signature key type and hash algorithm, last-used-time, associated-cdp and downloaded-from-uri. show crls lists them all. The lists covered include both manually downloaded CRLs and those retrieved automatically from a CDP.

Source: `06-operation-commands/063-crl.md`

### csr-gen-pending-import

*certificates-pki / consequence*

**Q.** What state is the node left in after I generate a signing request?

**A.** The guide is explicit: the consequence of csr-gen is the creation of a local-certificate in the pending-import state, plus the output of a CSR in PKCS#10 PEM format, which you then take to an external Certificate Authority. Defaults worth knowing: key-algorithm defaults to eccp256, with rsa2048, rsa3072, rsa4096, eccp384 and others available; signature-hash-algorithm defaults to sha256 for most key algorithms; metadata-template defaults to generic; key-usage defaults to digitalSignature and keyAgreement, and extended-key-usage to serverAuth and clientAuth. A minimal invocation is `csr-gen myCertificate subject='/CN=Nokia'`.

Source: `06-operation-commands/064-csr-gen.md`

### display-cert-hierarchy

*certificates-pki / parameter-values*

**Q.** How do I see a certificate's trust chain rather than just its own details?

**A.** `display-cert [certificate=]<value> [display-type=]<value>` where display-type is all-certificate-hierarchy, certificate-details or certificate-hierarchy. It defaults to all-certificate-hierarchy, so the chain is shown unless you narrow it to certificate-details. The command works on a certificate or a CSR, and runs in operational and candidate configuration mode.

Source: `06-operation-commands/083-display-cert.md`

### est-enroll-targets

*certificates-pki / enumeration*

**Q.** How do I get a certificate issued automatically instead of doing a CSR by hand?

**A.** Use EST, Enrollment over Secure Transport, which the guide describes as enabling automated certificate management over HTTPS through an EST client that talks to a dedicated EST server. target-command is cacerts to fetch the CA certificates, enroll for a first issue, or re-enroll for a renewal, for example `est enroll certificate-name=myNewCertificateA subject='/CN=NewCertA' http-user=<username> http-password=<password>`. key-algorithm covers eccp256, eccp384, rsa2048, rsa3072 and rsa4096, signature-hash-algorithm defaults to sha256 and metadata-template to generic. The CA is defined by est-ca and the server by est-server.

Source: `06-operation-commands/095-est.md`

### est-ca-auto-reenrollment

*certificates-pki / which-attribute*

**Q.** Can certificates issued through EST renew themselves, and how is the CA pinned?

**A.** est-ca carries auto-re-enrollment, which is what drives automatic renewal, alongside explicit-ca-root and root-fingerprint, which is how the CA root is pinned so the client can verify it. It also takes a label. The object is set and shown only, keyed by name, and the est command references it through its est-ca parameter, which defaults to 1.

Source: `06-operation-commands/096-est-ca.md`

### est-server-port-default

*certificates-pki / how-to-default*

**Q.** What port does the node use to reach an EST server, and can I define more than one?

**A.** server-port defaults to 443. You can define several: each est-server is keyed by CA name and server name, and priority runs 1 to 10 to order them, for example `add est-server-1/my-est-server server-address 10.23.55.123 server-port 8443`. enabled defaults to True. path-segment, up to 64 characters, appends a path to the EST URL for servers that need one.

Source: `06-operation-commands/097-est-server.md`

### import-certificate-pem

*certificates-pki / how-to*

**Q.** How do I paste a certificate straight into the node rather than downloading it?

**A.** `import-certificate {-i, <certificate type>} [<certificate-name>] <string in PEM format> [<passphrase>]` imports one or more certificates in PEM format into the NE, for example import-certificate local-certificate certificate-name=local-cert-1 followed by the PEM string and a passphrase. It runs in operational mode. The alternative is download with a filetype of local-certificate, peer-certificate or trusted-certificate, which fetches a PKCS#12 or PKCS#7 bundle from a server.

Source: `06-operation-commands/131-import-certificate.md`

### isk-in-use-flags

*certificates-pki / which-attribute*

**Q.** How do I tell whether an image signing key is actually being used before I remove it?

**A.** show ISK reports is-key-in-use and is-key-verified, both defaulting to false, plus being-deleted, so you can check all three before clearing. Removal is `clear isk [key-name=]<value>` and the name of the key to be deleted must be provided. Each ISK also shows CPU, key-name, key-serial-number, issuer-name, key-length, key-payload, the KRK-name it chains to, and the signature hash scheme, algorithm, payload and generation time. show ISKs lists them all.

Source: `06-operation-commands/147-isk.md`

### krk-root-keys

*certificates-pki / which-command*

**Q.** How do I list the image root keys on a node?

**A.** `show KRKs` lists them, and `show KRK-<name>` gives one, for example show KRK-1-5-0-KRK_E1. Each reports CPU, key-name, key-serial-number, issuer-name, key-length and key-payload. Root keys are replaced through a Key Replacement Package, which is downloaded with filetype krp and installed with activate; key-replacement-package shows its attributes. The image signing keys that chain to a root key are shown with ISK.

Source: `06-operation-commands/150-krk.md`

### key-replacement-package-view

*certificates-pki / minimal-command  (weak: the source section is thin)*

**Q.** Which command shows the key replacement package on the node?

**A.** `show key-replacement-package` shows the KRP attributes. The package itself is brought onto the node with download filetype=krp and installed with activate, which the activate section lists among its uses as activating or installing a Key Replacement Package. The keys it manages appear under KRK for root keys and ISK for image signing keys.

Source: `06-operation-commands/148-key-replacement-package.md`

### local-certificate-revocation-mode

*certificates-pki / parameter-values*

**Q.** Can I force the node to treat one of its own certificates as revoked?

**A.** Yes: revocation-mode on local-certificate is auto, force-revoked or force-unrevoked, defaulting to auto, so force-revoked overrides whatever the CRL or OCSP says. The object otherwise reports the certificate itself: version v3, serial-number, subject-name, issuer, trust-chain, valid-from and valid-to, status, public key length and type, signature key type and hash algorithm, certificate-bytes, key-usage, extended-key-usage, subject-alternative-names, self-signed and used-by. Only revocation-mode, alarm-report-control and label are settable.

Source: `06-operation-commands/159-local-certificate.md`

### ocsp-server-priority

*certificates-pki / how-to-default*

**Q.** How do I add an OCSP responder and control which one is tried first?

**A.** url and priority are both mandatory on add: `add ocsp-server-1 url http://1.2.3.4:8101 priority 3 enabled true`. priority runs 1 to 10. enabled defaults to false, so a responder added without it is inert. show ocsp-server reports last-query, which reads never until it has been used. Whether OCSP is consulted at all is governed by ocsp-based-revocation on security-policies, and the CRL alternative is configured with cdp and crl.

Source: `06-operation-commands/206-ocsp-server.md`

### peer-certificate-whitelist

*certificates-pki / disambiguation*

**Q.** What is a peer certificate here, and what does the white-listed flag mean?

**A.** A peer certificate is the X509v3 end-entity certificate representing a trusted remote peer for the L1 encryption secure application, so it is the far end's identity rather than the node's own. white-listed defaults to false and marks a peer certificate as explicitly accepted. Like a local certificate it exposes version, serial-number, subject-name, issuer, trust-chain, validity dates, status, key details, subject-alternative-names, key-usage and extended-key-usage, and only revocation-mode, alarm-report-control and label are settable. Compare local-certificate for the node's own identity and trusted-certificate for CA certificates.

Source: `06-operation-commands/234-peer-certificate.md`

### ssh-port-and-enabled

*certificates-pki / how-to-default*

**Q.** What port does SSH listen on, and is it on by default?

**A.** The port defaults to 8022, not 22, and enabled defaults to false, so SSH access has to be turned on explicitly with `set ssh enabled true`. port accepts 1 to 65535. The object also carries pre-login-message and post-login-message, the latter defaulting to a warning banner and accepting up to 1440 characters. The ssh object is system managed and cannot be manually deleted.

Source: `06-operation-commands/306-ssh.md`

### ssh-authorized-key-per-user

*certificates-pki / how-to*

**Q.** How do I let a user log in with a public key instead of a password?

**A.** Add an ssh-authorized-key for that user: `add ssh-authorized-key-<user-name>/<key-id> public-key <value> [label <value>]`, for example add ssh-authorized-key-admin/1 followed by the base64 public key. Each entry contains a trusted public key for SSHv2 user authentication, and a user can have several, distinguished by key-id. show reports public-key-algorithm and public-key. Whether key authentication is accepted at all is governed by ssh-authentication-method on security-policies.

Source: `06-operation-commands/307-ssh-authorized-key.md`

### ssh-host-key-per-algorithm

*certificates-pki / interpretation*

**Q.** Where does the node's own SSH host key come from, and can there be more than one?

**A.** The guide states there must be one host key per supported algorithm, that the system auto-generates a host key in the default database, and that additional host keys can be added or overwritten via the ssh-keygen RPC. `show ssh-host-key-<public-key-algorithm>` reports public-key, label, fingerprint-algorithm and fingerprint, which is what you compare when verifying the node from a client. These are the server side keys; keys the node trusts for outbound connections are ssh-known-host entries.

Source: `06-operation-commands/308-ssh-host-key.md`

### ssh-keygen-replaces-keys

*certificates-pki / consequence*

**Q.** If I regenerate the SSH keys, what happens to the existing ones?

**A.** They are replaced: the guide states the existing keys in the system will be replaced with the newly generated private and public key pair, so every client that had pinned the old host key will see a mismatch. The default type is RSA unless -t is given, with rsa, ecdsa and ed25519 available, and ED25519 support starts at R9.0. Key length is chosen with -b: 2048, 3072 or 4096 for RSA, 256, 384 or 521 for ECDSA, and 256 for ED25519; if not given the system picks a default for the type. The new public key is read back with show ssh-host-key.

Source: `06-operation-commands/309-ssh-keygen.md`

### ssh-known-host-algorithms

*certificates-pki / how-to*

**Q.** How do I pre-trust a server the node will connect out to over SSH?

**A.** Add an ssh-known-host entry with the address, the public key algorithm and the key: `add ssh-known-host-<id> address <value> public-key-algorithm <value> public-key <value>`, for example add ssh-known-host-Server_243 address 10.100.210.243 public-key-algorithm ecdsa-sha2-nistp256 followed by the key. public-key-algorithm covers ecdsa-sha2-nistp256, nistp384 and nistp521 and the ssh-rsa2048, ssh-rsa3072 and ssh-rsa4096 variants. Only label is settable afterwards. Whether unknown hosts are rejected is governed by ssh-strict-host-key-checking on security-policies.

Source: `06-operation-commands/310-ssh-known-host.md`

### trusted-certificate-scope

*certificates-pki / disambiguation*

**Q.** Which object holds the CA certificates the node trusts?

**A.** trusted-certificate, which the guide defines as the X509v3 CA root and intermediate certificate trusted by the system. Only alarm-report-control, label and revocation-mode are settable, the last being auto, force-revoked or force-unrevoked with auto as the default; everything else is read only, including trust-chain, valid-from, valid-to, status and the key and signature details. Removal is done with clear certificate rather than a delete on this object. For the node's own identity see local-certificate, and for a far-end peer see peer-certificate.

Source: `06-operation-commands/359-trusted-certificate.md`

### cable-id-terminate-test

*topology-discovery / which-command*

**Q.** How do I stop a CableID verification that is already running?

**A.** `terminate cable-id` stops it; the cable-id section documents both show cable-id and terminate cable-id, and points at the terminate command for details. To see the entities themselves use show cable-id, and to watch progress use cable-id-status, which reports cable-id-state as idle, running-incl-switching or running-no-switching plus a test-progress string. The test is started with verify.

Source: `06-operation-commands/033-cable-id.md`

### cable-id-path-cards

*topology-discovery / scope-limit*

**Q.** Which cards can source or terminate a CableID signal?

**A.** Only CAD10A and RD20TM. The guide states the CableID signal can only be sourced or terminated by those two cards and is transparently passed in the OPSM, and card-type-a and card-type-z accordingly accept only RD20TM or CAD10A. A path runs from the end A port to the end Z port. Per direction it reports a path-status of enabled or disabled and a last-test-status of not-verified, pass or fail, along with current-state, last-test-qualifier of up-to-date or out-dated, last-test-timestamp and additional-info.

Source: `06-operation-commands/034-cable-id-path.md`

### cable-id-status-progress

*topology-discovery / which-command*

**Q.** Can I check how far a fibre verification has got while it is running?

**A.** Yes. The guide states a user is allowed to issue the show cable-id cable-id-status command at any time to query the progress. cable-id-status is the container holding the process status and progress of a CableID-based fiber connection verification, reporting cable-id-state as idle, running-incl-switching or running-no-switching, and test-progress as a string that defaults to 'None'.

Source: `06-operation-commands/035-cable-id-status.md`

### carrier-neighbor-discovery-limit

*topology-discovery / interpretation*

**Q.** How many nodes can one carrier discover?

**A.** One. The guide states each carrier can discover up to one node, though it is possible for multiple collocated carriers to discover the same node several times, each connected to a different remote carrier. show carrier-neighbor reports last-update and age, local-carrier-id and remote-carrier-id, and the discovered ne-id, ne-name, ne-type plus its IPv4 and IPv6 loopback addresses. This discovery is via ICMP, which is why clear topology can target a carrier-neighbor instance.

Source: `06-operation-commands/041-carrier-neighbor.md`

### connection-ports-per-degree

*topology-discovery / which-command*

**Q.** How do I see which ports carry a given degree's connections?

**A.** `show connection-ports-<degree-number>/<index> [port-name]`, where degree-number runs 1 to 20 and index is 1 or 2, so each degree has at most two connection ports. A wildcard form such as show connection-ports* lists them all. The physical links themselves are modelled by fiber-connection within an NE and external-fiber-connection between NEs.

Source: `06-operation-commands/059-connection-ports.md`

### custom-tlv-organizational

*topology-discovery / which-command*

**Q.** How do I see vendor-specific LLDP TLVs received on a port?

**A.** `show custom-tlv-<lldp-port>/<direction>/<oui>/<subtype> [value]` lists Organizational Specific TLV parameter information, keyed by the organisationally unique identifier and subtype, so each vendor extension appears separately per port and direction. direction is ingress or egress, though the LLDP sections note the egress direction is not supported for neighbour and statistics data.

Source: `06-operation-commands/069-custom-tlv.md`

### external-fiber-connection-tnms

*topology-discovery / consequence*

**Q.** Should I configure fibre connections between nodes by hand?

**A.** The guide advises against it: the external-fiber-connection is set autonomously by TNMS, and although it is possible to configure it manually, it is not recommended. If you do, note that before R8.0 you were also expected to set the port's external-connectivity to yes. The object represents the physical link between two ports of L0 cards in different NEs, or in the same NE in disaggregated configurations. scope is general-purpose or cable-id, defaulting to general-purpose, and the cable-id value exists so CableID software can use the entry when building the CableID path topology. fiber-connection-type is one-way or two-way, defaulting to two-way. src-port-name and dst-port-name are mandatory on add.

Source: `06-operation-commands/104-external-fiber-connection.md`

### fiber-connection-within-ne

*topology-discovery / disambiguation*

**Q.** What is the difference between a fiber-connection and an external-fiber-connection?

**A.** Scope. A fiber-connection is the physical link representation of a connection between two distinct ports, or sub-ports, in the same NE, used in an OADM or ILA topology. An external-fiber-connection represents a connection between two ports of L0 cards in different NEs, or within one NE in disaggregated configurations. The internal one takes src-port and dst-port and can be freely set; the external one is normally written by TNMS and carries node ids and a scope attribute. Both have fiber-connection-type of one-way or two-way, defaulting to two-way.

Source: `06-operation-commands/107-fiber-connection.md`

### icdp-global-switch

*topology-discovery / how-to-default*

**Q.** Is the carrier discovery protocol on by default?

**A.** Yes. icdp, the Nokia Carrier Discovery Protocol, has a single attribute global-switch which is true or false and defaults to true, so discovery runs unless you turn it off with `set icdp global-switch false`. The object is system managed and cannot be manually deleted. Its results appear as carrier-neighbor entries.

Source: `06-operation-commands/125-icdp.md`

### inci-enabled-default

*topology-discovery / how-to-default*

**Q.** What is INCI and is it running by default?

**A.** INCI is the Inter-NE Communication Interface, which the guide describes as providing API-based communication infrastructure for control plane communication across different network elements, for example between a transponder such as a CHM6 and a line system. It is off by default: inci-enabled is true or false and defaults to false, so `set inci inci-enabled true` turns it on. Peers are then added as inci-neighbor entries.

Source: `06-operation-commands/132-inci.md`

### inci-neighbor-discovered-name

*topology-discovery / which-attribute*

**Q.** How do I tell whether the far end of an INCI link is the node I expected?

**A.** Compare configured-node-name, which you set, against discovered-node-name, which the node learns, and check discovered-node-id. connection-status and oper-state say whether the link is up. neighbor-address and configured-node-name are mandatory on add, and neighbor-port is reported. alarm-report-control defaults to inhibited on this object. The guide also notes digital trigger fault packets are H-MAC authenticated, and that digital trigger registration details are viewed with show digital-trigger-registration for the SCH AID.

Source: `06-operation-commands/133-inci-neighbor.md`

### interface-neighbor-discovery-timers

*topology-discovery / parameter-values*

**Q.** How often does the node look for a neighbour on a management interface, and how long before it gives up?

**A.** discovery-cycle-time covers 30 to 300 seconds and defaults to 30; discovery-timeout covers 300 to 1800 seconds and defaults to 300. discovery-enabled defaults to true, so discovery runs unless disabled. The result is neighbor-adjacency-state, which reads blackout, discovery, holding or unknown, together with the discovered neighbor-ne-id, neighbor-ne-name, neighbor-interface-name, neighbor-router-id and its IPv4 and IPv6 addresses, plus last-change-time and the associated-comm-channel.

Source: `06-operation-commands/135-interface-neighbor.md`

### links-dynamic

*topology-discovery / interpretation*

**Q.** Do I have to configure the links shown under topology?

**A.** No. The guide states these links are dynamically filled in by the system, allowing it to derive and display the NCT topology, so `show links` is a read-only view of what the node has worked out for itself. The same wording appears on nct-connection, which shows the links between NCT ports in a multi-chassis NE with their source and destination chassis and each chassis state of node controller, provisioned or unprovisioned.

Source: `06-operation-commands/154-links.md`

### lldp-hold-on-timer

*topology-discovery / how-to-default*

**Q.** How long does the node keep LLDP information before ageing it out?

**A.** hold-on-timer on the lldp object defaults to 900 seconds and is the only attribute it carries; the object is system managed and cannot be manually deleted. Note that neighbour data itself is described as kept indefinitely until the port is decommissioned or the data is manually cleared, which is done with clear topology targeting the lldp-neighbor instance.

Source: `06-operation-commands/155-lldp.md`

### lldp-local-info-system-string

*topology-discovery / which-command*

**Q.** What does this node advertise about itself over LLDP?

**A.** `show lldp-local-info-<lldp-port>` gives the local system information sent on that port: chassis-id and chassis-id-subtype, port-id and port-id-subtype, port-description, system-name, system-description, and the supported and enabled capabilities. The guide notes the GX uses a system-description string of the form 'Nokia Corporation.Converged OS, Version <release id>', which is a useful way to recognise a GX from the far end.

Source: `06-operation-commands/156-lldp-local-info.md`

### lldp-neighbor-egress-unsupported

*topology-discovery / scope-limit*

**Q.** Can I look at LLDP neighbours in the egress direction?

**A.** No. The guide states plainly that the egress direction is not supported, both for lldp-neighbor and for lldp-port-statistics, even though the instance key includes a direction. Neighbour data itself is kept indefinitely until the port is decommissioned or the data is manually cleared by the user, which is done with clear topology. The attributes include last-update, age, chassis-id, port-id, port-description, system-name, system-description, capabilities and ttl.

Source: `06-operation-commands/157-lldp-neighbor.md`

### lldp-port-statistics-persistence

*topology-discovery / interpretation*

**Q.** Do the LLDP frame counters reset when a neighbour ages out?

**A.** No. The guide states all counter values in a particular entry are maintained on a continuing basis and are not deleted upon expiration of the TTL timing counters associated with the LLDP neighbour information. The counters are total-frames-in, total-frames-out, total-discarded-frames, error-frames, total-discarded-tlvs, total-unrecognized-tlvs and total-ageouts, all starting at 0, plus last-change-time, which defaults to when the LLDP neighbour is formed, and last-clear-time. As with lldp-neighbor, the egress direction is not supported.

Source: `06-operation-commands/158-lldp-port-statistics.md`

### nct-connection-chassis-state

*topology-discovery / which-attribute*

**Q.** In a multi-chassis node, how do I see which shelf is the controller and which are not yet provisioned?

**A.** `show nct-connection-<src-port>/<dst-port>` reports src-chassis and dst-chassis with their states, where each state reads node controller, provisioned or unprovisioned. The links themselves are dynamically filled in by the system, allowing it to derive and display the NCT topology, so this is a discovered view rather than something you configure. Hardware seen but not yet accepted appears under unprovisioned-inventory.

Source: `06-operation-commands/185-nct-connection.md`

### sndp-enabled-default

*topology-discovery / how-to-default  (weak: the source section is thin)*

**Q.** What does the sndp command control?

**A.** It has a single attribute, sndp-enabled, which is true or false and defaults to true, read or written with `show sndp sndp-enabled` and set sndp. The acronym list expands SNDP as Simple Neighbor Discovery Protocol. The guide gives no further description in this section, so treat it as the on/off switch for that protocol; its neighbours surface through the topology views, and clear topology accepts sndp among its valid objects.

Source: `06-operation-commands/296-sndp.md`

### submarine-link-attributes

*topology-discovery / parameter-values*

**Q.** What do I have to supply to describe a subsea link, and how long can it be?

**A.** Five things are mandatory on add: src-node-id, src-port-name, dst-node-id, dst-port-name and degree-target-tx-power. fiber-length accepts 0 to 25000 km and defaults to 0. The object also models branching units through segment-list and bu-segment-index, and carries rx-fiber-type and tx-fiber-type, fiber-pair-id, link-name, gsnr, degree-expected-rx-power, commissioning-snr-margin and allocated-spectrum-list. launch-condition is flat-tx or pfib, defaulting to pfib, and fiber-connection-type defaults to two-way. Only add, delete, set and show exist.

Source: `06-operation-commands/315-submarine-link.md`

### supporting-fiber-connection-list

*topology-discovery / minimal-command  (weak: the source section is thin)*

**Q.** What does supporting-fiber-connection show?

**A.** `show supporting-fiber-connection-<name> [fiber-connection-list]` shows the list of fiber connections supporting the named entity. It is read only and carries a single attribute, fiber-connection-list. The connections themselves are configured as fiber-connection within an NE or external-fiber-connection between NEs.

Source: `06-operation-commands/330-supporting-fiber-connection.md`

### topology-clear-targets

*topology-discovery / enumeration*

**Q.** How do I wipe stale neighbour information, and what exactly can I clear?

**A.** `clear topology [target=]<value>` manually removes existing topology neighbour information. The target may be an lldp-neighbor instance discovered via LLDP, a carrier-neighbor instance discovered via ICMP, or an lldp-port-statistics instance holding details for an LLDP-enabled port, for example clear topology lldp-neighbor-ethernet-1-4-T1-1. The guide suggests pressing tab after clear topology to list the valid objects. show topology accepts inci, links, lldp, icdp and sndp as scopes.

Source: `06-operation-commands/354-topology.md`

### verify-allow-switching

*topology-discovery / how-to-default*

**Q.** How do I trigger a CableID fibre check, and will it disturb traffic?

**A.** `verify [-f] [type-select=]<value> [[target-select=]<value>] [allow-switching]` triggers CableID-based fiber connection verification, for example verify fiber-connection port-1-6-ade11, or all to cover every port in the cable-id entity. Whether it may switch is your choice: allow-switching is true or false and defaults to false, which is why cable-id-status distinguishes running-incl-switching from running-no-switching. -f runs it without the confirmation prompt. The guide notes IPM uses this CLI command to trigger CableID verification, and terminate cable-id stops a running test.

Source: `06-operation-commands/371-verify.md`

### bert-start-stop

*transport-layer1 / how-to*

**Q.** How do I run a bit error rate test on a client port and then stop it?

**A.** Start it with the bert command giving an operation of start plus a test-id and resource, for example `bert start test-id=TestX resource=ethernet-1-6-T1 test-signal-type=scrambled-idles test-signal-monitoring-type=scrambled-idles test-signal-direction=ingress test-signal-monitoring-direction=egress test-duration=10`. Stop it with bert stop test-id=TestX; the guide states test-id is mandatory for the stop command. test-duration defaults to 0, which means an indefinite test, and test-id is system generated if you do not supply one. bert get retrieves results. It runs in operational mode only.

Source: `06-operation-commands/028-bert.md`

### cid-ptp-auto-created

*transport-layer1 / interpretation*

**Q.** Where does the CableID facility come from and can I create one?

**A.** The node creates it. The guide states the cid-ptp facility is created when a card supporting the CableID function is created, for example an RD20TM or CAD10A, and that it supports the CableID SFP and its connection to the card via the CID port. Accordingly there is no add or delete form, only set for label and admin-state, and managed-by defaults to system. admin-state is documented with the single value unlock. The used flag, defaulting to false, shows whether the facility is in use.

Source: `06-operation-commands/048-cid-ptp.md`

### eth-zr-fec-and-degrade

*transport-layer1 / parameter-values*

**Q.** What FEC does a 400ZR interface use, and how are degrade thresholds set?

**A.** fec-type defaults to ofec, with cfec, noFEC, G709 and the older EFEC and SDFEC variants also listed. Degrade detection is off by default and split in two: fdd-monitoring and fed-monitoring both default to disabled. When enabled, fdd-threshold defaults to 0.0195 average BER with a clear threshold of 0.01062, and fed-threshold defaults to 0.0206 with a clear threshold of 0.01125; all four accept 0.000000001 to 0.1. link-degrade-indication then reads none, local-degraded, remote-degraded or local-and-remote-degraded. The facility is auto-instantiated when the ZR TOM is provisioned, and rate is fixed at 400.000 Gbit/s.

Source: `06-operation-commands/098-eth-zr.md`

### ethernet-max-packet-length

*transport-layer1 / contradiction*

**Q.** What is the largest frame an Ethernet client will accept?

**A.** It depends on the platform: max-packet-length covers 1280 to 18000 octets on the G30 and 1518 to 18000 octets on the G40, defaulting to 1518. Elsewhere the guide notes max-packet-length is configurable on card level only and applies to all interfaces on that card, and that it is used only for determining the undersized and oversized packet counts in 100GbE PMs, so it is a counting boundary rather than a hard MTU. Related settings include fec-mode, which defaults to disabled, tx-mapping-mode and expected-mapping-mode defaulting to GMP, timing-mode of retimed or transparent defaulting to transparent, and the fec-degraded-ser monitoring group whose activate threshold defaults to 0.00001 over a 10 second period.

Source: `06-operation-commands/099-ethernet.md`

### facilities-overview

*transport-layer1 / minimal-command  (weak: the source section is thin)*

**Q.** Which command lists the facilities configured on the node?

**A.** `show facilities` shows the system facilities. It takes no parameters and runs in operational or candidate configuration mode. The individual facility types beneath it include ethernet, eth-zr, fc, stm, odu, otu, flexo, interlaken and the optical ones such as ots, oms and oc.

Source: `06-operation-commands/105-facilities.md`

### fc-laser-toggling-tts

*transport-layer1 / which-attribute*

**Q.** What Fibre Channel specific settings does the node expose?

**A.** Beyond the usual label, admin-state, alarm-report-control, mapping modes and loopback, the fc facility carries tts and laser-toggling-for-tts, which relate to transmitter training. It also has the standard test signal group: test-signal-type, test-signal-direction and test-signal-monitoring. Everything else, including supporting-card, supporting-port and the facility lists, is read through show.

Source: `06-operation-commands/106-fc.md`

### flexo-foic-and-mode

*transport-layer1 / parameter-values*

**Q.** What FlexO interface types are available and what does resource-mode change?

**A.** foic-type covers foic1.2, foic1.4, foic2.4, foic2.8, foic3.6, foic4.8 and foic4.16, defaulting to foic4.8. resource-mode is ADM or XC, defaulting to ADM, and is set with a command such as `set flexo-1-5-L1-1 resource-mode 'XC'`. fec-type defaults to ofec with cfec, noFEC and not-applicable available. The degrade thresholds are fdd-raise-threshold, fdd-clear-threshold, fed-raise-threshold and fed-clear-threshold, and iid with accepted-iid and accepted-group-id cover the interface identifiers.

Source: `06-operation-commands/112-flexo.md`

### flexo-group-mandatory

*transport-layer1 / how-to*

**Q.** What do I need to supply to create a FlexO group?

**A.** Four mandatory values: carriers, rate, modulation-format and group-id, as in `add flexo-group-<name> carriers <value> rate <value> modulation-format <value> group-id <value>`. group-id is an integer from 1 to 1048575. Optional attributes are label, admin-state, alarm-report-control and fec-type, the last defaulting to ofec. Note that in this section the guide has folded the syntax block into the description text.

Source: `06-operation-commands/113-flexo-group.md`

### high-speed-monitoring-port

*transport-layer1 / how-to-default  (weak: the source section is thin)*

**Q.** What does high-speed-monitoring configure?

**A.** Two attributes only: enabled, which is true or false and defaults to false, and port, which defaults to 57500 and accepts 1 upward. So the feature is off by default and listens on a specific port when turned on with set high-speed-monitoring enabled true. The guide gives no further description of what the monitoring carries.

Source: `06-operation-commands/123-high-speed-monitoring.md`

### interlaken-capacity

*transport-layer1 / scope-limit*

**Q.** What is the Interlaken facility for and what can I change on it?

**A.** It represents the SPN2 Interlaken interface. Only label, admin-state, alarm-report-control and loopback are settable, for example `set interlaken-66-1-8 label interlaken_66_1`; loopback is none, facility or terminal, defaulting to none. capacity is fixed at 500 Gbit/s and managed-by is system, so the facility is created by the node rather than by you.

Source: `06-operation-commands/136-interlaken.md`

### line-ptp-valid-signal-time

*transport-layer1 / which-attribute*

**Q.** How does auto-in-service work on a line termination point?

**A.** line-ptp carries auto-in-service-enabled together with valid-signal-time, which is how long a good signal must persist before the facility is brought into service. The same pairing appears on trib-ptp and super-channel, where valid-signal-time covers 1 to 7200 minutes and defaults to 480, with remaining-valid-signal-time counting down. line-ptp also takes service-type, line-system-mode and the power-threshold-low-offset and power-threshold-high-offset trims.

Source: `06-operation-commands/153-line-ptp.md`

### network-xconnect-list

*transport-layer1 / disambiguation*

**Q.** What is the difference between network-xconnect and nw-xconnect?

**A.** network-xconnect is a read-only list: `show network-xconnect` shows the services of multiple user cross connections commissioned in the NE, with no other form. nw-xconnect is configurable and models a specific management cross connect: it is added with endpoint1 and endpoint2 mandatory, and carries xcon-type of L1-ETH-TO-GCC0, L1-GCC0-TO-GCC0, L1-ETH-TO-OSC or L1-OSC-TO-OSC, defaulting to L1-GCC0-TO-GCC0, plus rate in the range 1 to 20 Mbps. Neither is the same as xcon, which carries Layer 1 digital services.

Source: `06-operation-commands/189-network-xconnect.md`

### nw-xconnect-types

*transport-layer1 / parameter-values*

**Q.** How do I bridge a management channel between a GCC and an OSC?

**A.** With an nw-xconnect, choosing xcon-type from L1-ETH-TO-GCC0, L1-GCC0-TO-GCC0, L1-ETH-TO-OSC and L1-OSC-TO-OSC; it defaults to L1-GCC0-TO-GCC0. endpoint1 and endpoint2 are mandatory on add. rate is 1 to 20 Mbps, defaulting to 13 for L1-ETH-TO-GCC0 and 20 for the OSC variants. Note that the Ethernet side needs the comm-eth port in L1 mode, as in set comm-eth-1-12-ETH4 mode L1.

Source: `06-operation-commands/199-nw-xconnect.md`

### odu-high-order-auto

*transport-layer1 / interpretation*

**Q.** Do I have to create the high order ODU myself?

**A.** No. The guide states the high order ODU is automatically created when an SCH is created, and advises that when a CHM6 has an OTU configured on it you should use show odu to determine whether the ODU exists. What you create by hand are the low order ODUs: ODU4i at 100G or ODUflexi at 400G, which are what XCONs map into. class reads high-order, low-order or mapped, defaulting to low-order. odu-type is mandatory on add, and the mapping is described by trib-port-number (1 to 255), time-slots and opucn-time-slots, with expected-trib-port-number and expected-time-slots for the far end.

Source: `06-operation-commands/207-odu.md`

### odu-diagnostics-tti-per-direction

*transport-layer1 / which-attribute*

**Q.** Can I set a different trace identifier for each direction on an ODU?

**A.** Yes. odu-diagnostics is addressed as odu-diagnostics-<name>/<direction>, and the guide states each direction has its own values. Per direction you get monitoring-mode, tti-style, tti-mismatch-alarm-reporting, tx-tti and expected-tti along with the separate SAPI, DAPI and operator fields, and the read-back rx-tti, rx-sapi and rx-dapi in both text and hex. tim-act-enabled controls the consequent action on a trace mismatch, and degrade-interval and degrade-threshold govern signal degrade detection.

Source: `06-operation-commands/208-odu-diagnostics.md`

### otu-fec-defaults

*transport-layer1 / how-to-default*

**Q.** Is FEC on by default on an OTU, and which type?

**A.** Yes. fec-mode and fec-generation-mode both default to enabled, and fec-type defaults to ofec, with cfec, G709, noFEC, i4, i7, sdfec15, sdfec15nd, staircase7 and ufec7 also available. tx-mapping-mode and expected-mapping-mode default to none here, unlike the Ethernet facility where they default to GMP. loopback is none, facility or terminal with loopback-mode of loopback or loopback-and-continue. An example command is `set otu-OTUC4 alarm-report-control enabled`, although the documented values for that attribute are allowed and inhibited.

Source: `06-operation-commands/229-otu.md`

### otu-diagnostics-tim-reporting

*transport-layer1 / parameter-values*

**Q.** How do I make the node alarm only on a mismatched source trace rather than the whole trace string?

**A.** tti-mismatch-alarm-reporting on otu-diagnostics selects which part is compared: disabled, full-64-bytes, SAPI, DAPI, OPER or the combinations of those, defaulting to disabled. So `set otu-diagnostics-1-1-1-OTUC1/ingress tti-mismatch-alarm-reporting SAPI` alarms only on a source access point identifier mismatch. Each direction is configured separately. monitoring-mode is unused, intrusive, non-intrusive, limited-intrusive or limited-non-intrusive, defaulting to intrusive, and degrade-interval defaults to 7 seconds with degrade-threshold at 30%.

Source: `06-operation-commands/230-otu-diagnostics.md`

### stm-types-supported

*transport-layer1 / enumeration*

**Q.** Which SDH rates does the platform terminate?

**A.** stm-type reads STM-16 or STM-64. The facility carries the usual mapping and trace settings: tx-mapping-mode and expected-mapping-mode, tti-style of ITU-T-G709 or proprietary, tx-tti and expected-tti as tti-64, and tim-monitor defaulting to disabled. Test signals cover none, PRBS31Q, PRBS13Q, scrambled-idles, PRBS9 and PRBS31, with test-signal-direction fixed at ingress. An example is `set stm-1-5-T9.2 tx-mapping-mode BMP`. Note this section states admin-state as Lock, Unlock and Maintenance with initial capitals, unlike most other facilities.

Source: `06-operation-commands/313-stm.md`

### trib-ptp-disable-action

*transport-layer1 / parameter-values*

**Q.** What does the node do to a client port when the service behind it fails?

**A.** tributary-disable-action decides, and it defaults to laser-shut-off. The alternatives include none, odu-ais, send-ais-1, send-gais, send-idles and send-lf. tributary-disable-holdoff-timer delays it by 0 to 10000 milliseconds, defaulting to 0, and the guide notes it applies only when the action is laser turn-off, recommending you hold the optical signal until the timer expires before turning the laser off. Related settings are near-end-tda and tda-degrade-mode, both defaulting to disabled, and forward-defect-trigger, defaulting to true. Note also the warning that operating the ZXS-QDZRZZZZ-00 above 40 degrees C is not supported, in which case it goes to low power mode and a CFG-MSMT alarm is raised.

Source: `06-operation-commands/358-trib-ptp.md`

### xcon-create-and-protection

*transport-layer1 / how-to*

**Q.** How do I create a Layer 1 cross connect and what protection types can it have?

**A.** source and destination are mandatory on add, for example `add xcon-11 source odu-1-1-3-ODU4 destination odu-1-1-1-ODU4-1`. direction is documented with the single value two-way. protection-type reads y-cable, snc-n, snc-i or unprotected, defaulting to unprotected. type reads add, drop, add-drop or express, and payload-treatment reads transport, switching, transport-without-fec, regen or regen-switching. managed-by defaults to user here, unlike most facilities, since cross connects are provisioned rather than derived. Only label and circuit-id-suffix can be changed afterwards with set.

Source: `06-operation-commands/373-xcon.md`

### l2-bridge-attributes

*transport-layer1 / which-attribute  (weak: the source section is thin)*

**Q.** What can I configure on an L2-bridge, and what identifies one?

**A.** The bridge is identified by bridge-name, a string of up to 64 characters, and carries just two attributes: chassis-name, a reference to the chassis the bridge is associated with, and description, up to 255 characters describing the bridge and its intended purpose. Only set and show are documented, with no add or delete form, and it is available in operational and candidate configuration mode. The guide gives no further detail on what the bridge switches.

Source: `06-operation-commands/170-l2-bridge.md`

### app-clear-third-party

*management-protocols / minimal-command  (weak: the source section is thin)*

**Q.** How do I remove a third-party application from the node?

**A.** `clear app [app-name=]<value>` clears third party apps, and takes -f to skip confirmation. To control a running application rather than remove it, use appctl, and to see or set its attributes use third-party-app, whose state reads running, stopped or failed and whose enable flag defaults to false.

Source: `06-operation-commands/021-app.md`

### appctl-control-target

*management-protocols / how-to*

**Q.** How do I send a control command to a third-party application?

**A.** `appctl [app-name=]<value> [command=]<value> [[target=]<value>] [[parameters=]<value>[,<value>]*]`. app-name identifies the application and command is the action to perform, with optional parameters. target defaults to system. The related objects are third-party-app for the application's own attributes and state, sw-container for the OS-level container it runs in, and clear app to remove it.

Source: `06-operation-commands/022-appctl.md`

### call-home-forces-attempt

*management-protocols / consequence*

**Q.** Can I make the node dial out to the management system immediately instead of waiting for the retry timer?

**A.** Yes, that is exactly what call-home does. `call-home [dial-out-server-name=]<string>` forces a connection attempt to a configured dial-out-server, and the guide notes that if a dial-out-server is currently connecting, the command forces an immediate attempt rather than waiting before retrying. It runs in operational mode. The server itself is configured with dial-out-server, where retry-policy defaults to progressive-back-off.

Source: `06-operation-commands/037-call-home.md`

### current-subscription-encoding

*management-protocols / parameter-values*

**Q.** What encodings and transfer modes can a telemetry subscription use?

**A.** encoding covers json, bytes, proto, ascii and json-ietf, defaulting to json-ietf. transfer-mode is stream, where values are streamed by the target, once for a single set of values, or poll where values are sent in response to a request, defaulting to stream. session-type distinguishes gnmi-dial-in from the gnmi-dial-out variants including via tunnel, and session-protocol is gnmi. updates-only defaults to false. show current-subscription also reports related-session-id, related-dial-out-server and user-access.

Source: `06-operation-commands/068-current-subscription.md`

### data-model-enable

*management-protocols / how-to-default*

**Q.** Are all the YANG models loaded by default, and how do I turn one on?

**A.** No: enabled on data-model defaults to false, so a model is not loaded until you enable it with `set data-model-<name> enabled true`. show data-model lists the available models with a description and the enabled flag. The protocols that consume them are netconf, restconf and grpc, and show protocols includes data-model-openconfig among its filters.

Source: `06-operation-commands/070-data-model.md`

### dial-out-server-retry-policy

*management-protocols / parameter-values*

**Q.** What retry behaviour does a call-home server use, and on what port?

**A.** retry-policy is progressive-back-off, retry-then-stop or retry-forever, defaulting to progressive-back-off, with retry from 0 to 5 defaulting to 3 and timeout from 1 to 255 seconds defaulting to 10. port defaults to 4334 and protocol is netconf or restconf, defaulting to netconf. auto-connect defaults to true, so the node dials out by itself. connection-state reports connected or connecting among other values. Two worked examples are `set dial-out-server-callhome1 auto-connect false retry-policy retry-forever protocol netconf timeout 100` and a retry-then-stop variant on port 4889. alarm-report-control defaults to inhibited here.

Source: `06-operation-commands/079-dial-out-server.md`

### grpc-port-and-granularity

*management-protocols / how-to-default*

**Q.** Is gNMI enabled by default and on which port?

**A.** Yes: enabled on grpc defaults to true and port defaults to 50051. gnmi-get-encoding-granularity is per-path or per-object, defaulting to per-object, which controls how Get responses are encoded. The object is system managed and cannot be manually deleted. Contrast this with netconf on port 830 and restconf on 8080 and 8181.

Source: `06-operation-commands/121-grpc.md`

### netconf-hello-timeout

*management-protocols / parameter-values*

**Q.** What port does NETCONF use here and how long does it wait for a hello?

**A.** port defaults to 830 and hello-timeout defaults to just 2 seconds, within a range of 1 to 3600. enabled defaults to true. Two less obvious flags: annotate-cli-name, defaulting to false, adds the CLI name to the model output, and static-info-in-notifs controls whether static information is included in notifications. The object is set and shown only.

Source: `06-operation-commands/188-netconf.md`

### restconf-http-disabled

*management-protocols / how-to-default*

**Q.** Is plain HTTP available for RESTCONF, and how long do sessions last?

**A.** Not by default. enabled defaults to true, but http-enabled defaults to false while https-enabled defaults to true, so only HTTPS is served unless you enable HTTP explicitly. http-port defaults to 8080 and https-port to 8181. cookie-timeout is 1 to 300 minutes, defaulting to 5, for example `set restconf cookie-timeout 10`. show restconf also reports api-root. Note that secure-mode on security-policies stops non-secure protocols being used at all.

Source: `06-operation-commands/266-restconf.md`

### snmp-port-and-engine

*management-protocols / which-attribute*

**Q.** How do I check the SNMP engine identity of a node?

**A.** show snmp reports snmp-engine-id and engine-boot-count, the latter starting at 0, alongside enabled, which defaults to true, and port, which defaults to 161. The guide notes in two places that the trap-community-string is not here but on the snmp-target object. Read community strings are configured with snmp-community and v3 credentials with snmpv3-user.

Source: `06-operation-commands/297-snmp.md`

### snmp-community-read-only

*management-protocols / scope-limit*

**Q.** Can I create a read-write SNMP community?

**A.** No. community-string-access has a single documented value, read-only, which is also the default, so SNMP access here is read only. A community is added with `add snmp-community-mycommunity community-string public community-string-access read-only enabled true`, and enabled defaults to true. Note again that the trap community string used for notifications lives on snmp-target, not here.

Source: `06-operation-commands/298-snmp-community.md`

### snmp-target-version-and-port

*management-protocols / parameter-values*

**Q.** How do I add a trap receiver, and what version and community does it use by default?

**A.** snmpv3-user and target-address are mandatory on add. snmp-version is v2c or v3, defaulting to v2c, and trap-community-string defaults to the string infinera, which is worth changing. target-port defaults to 162 and target-transport is udp. A worked example is `add snmp-target-mytarget snmp-version v2c target-ip 10.220.225.10 target-port 162 target-transport udp trap-community-string public enabled true`, though note the example uses target-ip while the syntax gives target-address.

Source: `06-operation-commands/299-snmp-target.md`

### snmpv3-user-security-level

*management-protocols / how-to-default*

**Q.** What security level does a new SNMPv3 user get, and which privacy algorithms are available?

**A.** user-sec-level defaults to no-auth-no-priv, so a user created without setting it gets no authentication and no privacy; the alternatives are auth-no-priv and auth-priv. auth-protocol has one documented value, SHA. priv-protocol offers DES, AES128, AES192 and AES256, defaulting to AES128. Both auth-passphrase and priv-passphrase are mandatory on add, for example `add snmpv3-user-bob user-sec-level auth-priv auth-protocol SHA auth-passphrase public123 priv-protocol AES128 priv-passphrase private123`.

Source: `06-operation-commands/300-snmpv3-user.md`

### subscription-path-sampling

*management-protocols / parameter-values*

**Q.** How do I control whether telemetry is sent on change or at a fixed interval?

**A.** subscription-path-mode selects target-defined, on-change or sample, defaulting to target-defined, so the target decides unless you pick. sample-interval and heartbeat-interval are both in milliseconds and default to 0, the heartbeat being what forces a send even when nothing changed. suppress-redundant defaults to true, so unchanged values are not resent. The path itself and its origin are subscription-path and subscription-path-origin, keyed by subscription name and path name.

Source: `06-operation-commands/316-subscription-path.md`

### subscriptions-list

*management-protocols / minimal-command  (weak: the source section is thin)*

**Q.** Which command lists the telemetry subscriptions on the node?

**A.** `show subscriptions` shows the list of subscriptions, with no parameters. For the detail of each, current-subscription reports session type, protocol, encoding and transfer mode, and subscription-path reports the paths with their sampling mode and intervals. The top-level container is telemetry.

Source: `06-operation-commands/317-subscriptions.md`

### telemetry-persistent-dynamic

*management-protocols / which-command*

**Q.** Where is telemetry configured, and what is the difference between persistent and dynamic?

**A.** The telemetry object is the top-level container, described as configuring persistent and dynamic telemetry; it is system managed and cannot be manually deleted. Its documented syntax is show telemetry and set telemetry id <string>. The distinction between persistent and dynamic subscriptions is not elaborated further in this section: what you can inspect is current-subscription, whose session-type separates dial-in from dial-out sessions, and subscription-path for the paths and their sampling.

Source: `06-operation-commands/344-telemetry.md`

### third-party-app-state

*management-protocols / which-attribute*

**Q.** How do I tell whether a third-party application is running, and is it enabled by default?

**A.** state reads running, stopped or failed, defaulting to stopped, and enable defaults to false, so an application is neither enabled nor running until you say so. The object also carries version, vendor, product, label and information. A second form, `show third-party-app-info-<location-id>/<app-name>`, gives version, state and information per location. Use appctl to send it commands and clear app to remove it.

Source: `06-operation-commands/349-third-party-app.md`

### add-mandatory-attributes

*cli-and-session / how-to*

**Q.** How do I find out which entities I can create and what each one needs?

**A.** The guide's tip is to use `add <tab>`, which shows all entity types that are creatable at that time. Some entities have mandatory attributes that must be supplied at creation, and the entity id format differs per type. Note that entities managed by the system cannot be created with add at all. The flags are -v to validate only, -m to merge, which creates the entity if absent and updates it if present, and -f to force.

Source: `06-operation-commands/010-add.md`

### clear-requires-confirmation

*cli-and-session / consequence*

**Q.** Does clearing something always prompt me first?

**A.** Yes by default. The guide states the clear command requires a user confirmation in all cases, and that the -f flag forces it without confirmation. Each sub-command handles a specific type of clear operation, covering pm, topology, log, alarm, database, certificate, statistics, system, file, app, isk, ospf and recover-mode among others. It runs in operational mode. One documented restriction: clear system factory-reset with the shutdown option is not supported on the L0 cards.

Source: `06-operation-commands/049-clear.md`

### cli-port-and-alarm-columns

*cli-and-session / which-attribute*

**Q.** Can I change which columns the alarm display shows by default?

**A.** Yes, with show-alarm-columns on the cli object. It accepts a comma-separated list of columns, or the keyword default-columns, or default-columns plus additional ones, and defaults to default-columns. The same object carries enabled, defaulting to true, port, defaulting to 22, default-interactive-mode, defaulting to true, and a read-only script-dir. Per-session overrides live on cli-session-config.

Source: `06-operation-commands/050-cli.md`

### cli-session-config-size

*cli-and-session / parameter-values*

**Q.** How do I change how many rows my terminal output uses?

**A.** Set cli-lines on your own cli-session-config, which is keyed by session id in the form address:port, for example `set cli-session-config-10.19.204.27:52361 cli-lines 30`. cli-lines accepts 10 to 1000 and defaults to 40; cli-columns accepts 80 to 4000 and defaults to 80. interactive-mode defaults to true and display-timestamp to false, so timestamps are off unless you enable them. Find your session id with show session.

Source: `06-operation-commands/051-cli-session-config.md`

### connect-ssh-from-cli

*cli-and-session / how-to*

**Q.** Can I hop to another node without leaving the CLI?

**A.** Yes, `connect [target-address=]<value> [user-name=]<value> [[port=]<value>]` establishes an SSH session directly from the CLI, for example connect 10.41.24.55 admin port=8022. The target can be an IPv4 address, an IPv6 address or a hostname, and port defaults to 22, which is worth noting because the GX itself defaults its own SSH port to 8022. The first connection adds the far end to the known hosts list. It runs in operational mode.

Source: `06-operation-commands/058-connect.md`

### convert-to-netconf-python

*cli-and-session / enumeration*

**Q.** Is there a way to turn a CLI command into the equivalent NETCONF or RESTCONF request?

**A.** Yes, that is what convert is for. target-representation offers netconf-xml, which generates an entire NETCONF XML payload, netconf-python, which generates Python code performing the NETCONF request, restconf-json, which produces the HTTP method, URI, headers and body with a JSON payload, plus restconf-xml, restconf-python and the plaintext-to-encrypted conversions. For example `convert restconf-python 'ping 1.23.151.23'`. The guide describes it as mainly an auxiliary tool for generating complex commands in other protocols.

Source: `06-operation-commands/062-convert.md`

### default-not-everything-resets

*cli-and-session / scope-limit*

**Q.** If I reset an object to defaults, does everything go back?

**A.** No. The guide states some configuration attributes will not be reset to default, including mandatory parameters and attributes only settable at creation. If you name attributes, only those are reset. Multiple instances can be selected with a wildcard, for example default user-peter for one object. Two caveats: the command takes -f to skip confirmation, and the guide notes the default command is not supported on the G30 in Releases 5.0 and 5.1.

Source: `06-operation-commands/075-default.md`

### delete-best-effort-flag

*cli-and-session / parameter-values*

**Q.** How do I delete an object in a script without failing if it is already gone?

**A.** Use the -b best effort flag: the guide contrasts standard behaviour, where `delete <object>` fails if the object does not exist, with the new behaviour where `delete -b <object>` does not fail in that case. Deleting a managed entity instance removes the entity and all its sub-level objects, and a confirmation prompt appears unless -f is given. -v validates only. A simple example is delete card-1-4.

Source: `06-operation-commands/077-delete.md`

### exit-ctrl-d

*cli-and-session / consequence*

**Q.** What does exit actually do, and is there a shortcut?

**A.** It depends where you are: exit terminates the current CLI session if you are in operational mode, or leaves configuration mode if you are in candidate mode. A confirmation prompt appears unless -f is given. The guide notes the keyboard shortcut Ctrl+D has the same effect as `exit -f`. To leave candidate mode and throw away staged changes at the same time, use discard-changes instead.

Source: `06-operation-commands/100-exit.md`

### expect-silent-on-match

*cli-and-session / interpretation*

**Q.** How can a script check that an attribute has the value it should?

**A.** Use expect, which validates an existing attribute value against an expected value: `expect ne ne-name GX-NE-123`. The behaviour is deliberately quiet, since if the value matches no output is seen, and only a mismatch produces an ERROR message. That makes it suitable for scripts and automation where you want a guarantee rather than output to parse. The -r flag is available on the command.

Source: `06-operation-commands/101-expect.md`

### export-session-variables

*cli-and-session / scope-limit*

**Q.** Can I define variables to reuse in CLI commands, and do they survive logout?

**A.** You can define them, but they do not survive. The guide states variables are locally defined per session and are removed after the session is closed. Define one with `export SLOT_NUMBER=2` and reference it as ${SLOT_NUMBER} in any CLI command. The same command defines, deletes and views variables, and the value can be any supported character including spaces. It is particularly useful in CLI scripts.

Source: `06-operation-commands/102-export.md`

### gshell-guest-container

*cli-and-session / disambiguation*

**Q.** What is the difference between the shell and gshell commands?

**A.** Where the shell runs. gshell launches a Linux bash shell inside a Guest Container from within the CLI, while shell launches one on the node itself using the currently logged in user, with access limited to what that user can do. Both accept a single command as an argument and both return to the CLI prompt when closed with exit. Note the guide states access to shell can be limited to users in the NA, NE and TT user-groups.

Source: `06-operation-commands/122-gshell.md`

### kill-session-not-own

*cli-and-session / scope-limit*

**Q.** Can I use kill-session to log myself out?

**A.** No. The session-id must match an existing session but cannot match the id of the current session; the guide says to use a normal exit for that. The id takes the form address:port, for example `kill-session 10.24.11.25:56212`, and show session lists the ids. It closes any established session regardless of type, CLI, NETCONF or otherwise, and runs in operational mode.

Source: `06-operation-commands/149-kill-session.md`

### message-broadcast-target

*cli-and-session / how-to-default*

**Q.** How do I warn everyone logged into the node that I am about to reboot it?

**A.** `message "System will reboot in 5 minutes"` broadcasts to all CLI sessions, since target defaults to all. You can narrow it with local, meaning only serial console or CRAFT sessions, or remote for remote sessions, or target a specific session id or username. The guide notes the command can also be executed from NETCONF and RESTCONF, but the message is only ever delivered to CLI sessions. It runs in operational mode.

Source: `06-operation-commands/180-message.md`

### property-fast-client-recovery

*cli-and-session / which-attribute*

**Q.** What card-level properties can I set, and what are they for?

**A.** The property object is auto-instantiated by the system per card but configurable by you, addressed as property-<card-name>/<property-name>. Two are documented: fast-client-recovery, which is disabled or enabled and defaults to disabled, and max-packet-length, which accepts 1518 to 18000. For example `set property-1-5/fast-client-recovery value enabled`. The max-packet-length property is the card-level control that the Ethernet facility's own note refers to.

Source: `06-operation-commands/249-property.md`

### run-task-or-script

*cli-and-session / disambiguation*

**Q.** How do I execute a saved script or a defined task on demand?

**A.** The run command has two forms. `run task [[task-name=]<value>]` executes a previously configured or scheduled task, for example run task collect-diag. `run script [-q] [-y] [-e=<value>] [-r] [[script-name=]<value>] [[arguments=]<value>]` executes a script, where -q is quiet, -y answers prompts and arguments passes parameters. Tasks themselves are defined with task or scheduled-task.

Source: `06-operation-commands/272-run.md`

### scheduled-task-vs-task

*cli-and-session / disambiguation*

**Q.** There is a task command and a scheduled-task command. How do they differ?

**A.** They overlap heavily. Both add, set, show and delete user-configurable scheduled commands with enabled, command, command-type, frequency, start-time, end-time and persistent, and both default enabled and persistent to true. task additionally exposes number-of-runs, which is no-limit by default or 1 to 65535, alarm-report-control and label, and its task-status reads scheduled, disabled, finished or ongoing. scheduled-task exposes previous-result and previous-output, and its documented task-status values are true and false, which looks like a documentation error. Both express frequency in the same w, d, h, m, s form.

Source: `06-operation-commands/275-scheduled-task.md`

### session-visibility-by-role

*cli-and-session / scope-limit*

**Q.** Can I see everyone else's sessions on the node?

**A.** Only if you are a security administrator. The guide states only SA users can access the list of all sessions, and remaining users can only see their own. show session reports session-user, session-type, session-protocol, created-time, local-ip-address and dial-out-server-name. RESTCONF sessions appear in the CLI when cookie-based authentication is used, and the guide notes the session has a keep-alive of 5 minutes by default, changed with the restconf cookie-timeout attribute.

Source: `06-operation-commands/286-session.md`

### set-wildcard-forms

*cli-and-session / parameter-values*

**Q.** Can I change an attribute on several objects at once?

**A.** Yes, using wildcards in the entity id. The guide gives three forms: as a replacement of the whole instance key, for example card-*; as a replacement of a single instance key, for example port-1-4-* or port-1-*-3; and as a replacement of all following keys to the right, for example odu-1-4,5,6,7-*. The command needs at least one attribute-value pair and accepts several as long as they belong to the same managed entity, plus optional filters. -v validates only and -f forces. A simple example is `set ne-location London`, and the guide suggests using contextual help with ? for hints on the expected value type.

Source: `06-operation-commands/287-set.md`

### shell-user-group-limit

*cli-and-session / scope-limit*

**Q.** Who is allowed to drop to a Linux shell on the node?

**A.** The guide states user access to the shell command can be limited to users in the NA, NE and TT user-groups. The shell is launched as the currently logged in user and only allows commands accessible to that user, so it is not a privilege escalation. It closes with exit and returns to the CLI prompt, and a single command can be passed as an argument. The guide notes various standard Debian packages are included providing common Linux utilities. For a shell inside a Guest Container instead, use gshell.

Source: `06-operation-commands/290-shell.md`

### show-output-limit

*cli-and-session / scope-limit*

**Q.** Is there a limit on how much a single show command can return?

**A.** Yes. The guide states that if a user executes a show command which generates more than 50000 objects, an error message is displayed indicating the output is too large, so a broad query has to be narrowed with an entity id, entity type, attribute or filter. show has several modes: managed entities, show alarm, show config, show log and show pm, each documented in its own section. show -r is the recursive form.

Source: `06-operation-commands/291-show.md`

### simulate-triggers

*cli-and-session / scope-limit*

**Q.** Can I fake a card insertion or an alarm for testing?

**A.** Partly. simulate accepts triggers raise-alarm, clear-alarm, plug-in-fru and plug-out-fru, but the guide states plainly that plug-out-fru is not supported. For equipment simulation you give the holder-AID matching the card slot or TOM port plus a type, for example `simulate plug-in-fru 1-5`. For alarms you give alarmed-entity and alarm-type, with alarm-direction and alarm-location defaulting to auto. It runs in operational mode.

Source: `06-operation-commands/293-simulate.md`

### sleep-floating-point

*cli-and-session / minimal-command*

**Q.** Can I pause a script for a fraction of a second?

**A.** Yes. `sleep <time in seconds>` specifies a delay, and the guide states the sleep time may be an arbitrary floating point number, so sleep 0.5 is valid as well as sleep 1. It is available in operational and candidate configuration mode, which makes it usable inside CLI scripts alongside expect and export.

Source: `06-operation-commands/294-sleep.md`

### task-scheduling-fields

*cli-and-session / how-to*

**Q.** How do I schedule a database backup to run once at a specific time?

**A.** Add a task with the command to run and a start-time, for example `add task-db_backup_once command "upload database file-server=xfr1" start-time 2021-04-23T05:05:00+00:00 alarm-report-control allowed label "DB Backup once"`. command is mandatory. For a repeating task, frequency uses the form combining weeks, days, hours, minutes and seconds, such as 2w, and number-of-runs limits how many times it fires, defaulting to no-limit. enabled and persistent both default to true, end-time defaults to never, and task-status reads scheduled, disabled, finished or ongoing. Note the guide's precondition that the file server, xfr1 in the example, must be configured before the command is used.

Source: `06-operation-commands/343-task.md`

### terminate-what-can-stop

*cli-and-session / enumeration*

**Q.** Which long-running operations can I stop, and how do I confirm one has stopped?

**A.** terminate has four documented forms: location-led with an entity, for example `terminate location-led chassis-1`, otdr, otdr-fiber-check and loopback, the last taking a comma-separated list of entities. For the location LED test the target is a chassis or a particular card, and it stops a test started with activate. For CableID, the guide explains the verification test is known to have terminated once cable-id-status changes to idle, which you confirm with show cable-id cable-id-status. It runs in operational mode.

Source: `06-operation-commands/348-terminate.md`

### update-type-values

*cli-and-session / enumeration*

**Q.** What does the update command do that set does not?

**A.** It performs specific system-defined operations rather than assigning arbitrary values. type selects the operation from span-loss-alarm-threshold, filter-insertion-date-now, set-under-commissioning and clear-under-commissioning, and entity-id names the targets, which can use a wildcard for multiple instances. For example `update span-loss-alarm-threshold ots-1-1-dwdm-line` recalculates that threshold rather than setting a number. It runs in operational mode.

Source: `06-operation-commands/362-update.md`

## Multi-command tests

### multi-pm-threshold-crossing-chain

*performance-monitoring / which-commands-together*

**Q.** I want the node to raise a threshold crossing alert when errored seconds on one port go above a limit. Which commands are involved and what does each one contribute?

**A.** Four objects take part. pm-parameter identifies the counter itself and tells you whether it is a counter or a gauge and its units. pm-threshold sets the actual limit on one instance, addressed as pm-threshold-<resource>/<period>/<direction>/<location>/<parameter> with low-threshold and high-threshold; both default to na. pm-threshold-profile holds the per resource-type view and is where the system's own default-low-threshold, default-high-threshold, min-value and max-value are visible. pm-control-entry carries tca-supervision for that resource, period, direction and location, which is what enables threshold crossing supervision; the matching default for newly created resources of a type comes from pm-profile-entry's default-tca-supervision. All of these are available in operational and candidate configuration mode.

Source: `06-operation-commands/244-pm-threshold.md`, `06-operation-commands/245-pm-threshold-profile.md`, `06-operation-commands/239-pm-control-entry.md`, `06-operation-commands/240-pm-parameter.md`

Not stated by the document: The guide documents each object separately and does not state an ordering for configuring them. Any sequence given in an answer is inferred, not documented.

### multi-ntp-authenticated-setup

*system-node-time / which-commands-together*

**Q.** What does the node need in place to synchronise its clock from an authenticated time server, and how would I check it is working?

**A.** The ntp object carries the global switches: ntp-enabled (default true) and ntp-auth-enabled (default false), plus assignment-method (manual, dhcp or both, default both). ntp-key holds the authentication keys, keyed by key-id 1..65534, with key-type sha-1, aes-cmac, sha-256 or md5, a key-value of 8 to 40 characters, and is-trusted which defaults to false. ntp-server defines each server by IP address or DNS name and its auth-key-id points at the ntp-key to use; origin records whether the entry came from dhcp or was set manually. To verify, ntp-server-status reports reach, where 377 means all recent probes were answered, together with stratum, offset, jitter and auth-status (ok, yes, bad or none). show clock then shows time-source as ntp rather than manual once the node is synchronising.

Source: `06-operation-commands/195-ntp.md`, `06-operation-commands/196-ntp-key.md`, `06-operation-commands/197-ntp-server.md`, `06-operation-commands/198-ntp-server-status.md`, `06-operation-commands/052-clock.md`

Not stated by the document: The link from ntp-server.auth-key-id to an ntp-key instance is stated by the attribute description; the guide gives no end-to-end configuration procedure.

### multi-protection-switch-vs-switchover

*protection-redundancy / disambiguation*

**Q.** There seem to be two different switch commands. Which one do I use to move traffic to the protection path, and which one moves the controller?

**A.** They act on different things. protection-switch is the operating command for protection group switching: you give operation-type (force, lockout, manual or release), a switch-target and the protection-group. manual-switchover performs a manual switchover of an equipment object given by AID, for example manual-switchover card-1-3, and it warns that the controller will switch over and the connection to the management interface will be lost. So use protection-switch for traffic on a protection group, and manual-switchover for the controller card. Both are operational mode only and both accept -f to skip confirmation. The surrounding objects are protection-group, which holds the configuration and state of the group, and protection-unit, whose state (active, standby, available, unknown) and role (working, protection) tell you which member is carrying traffic.

Source: `06-operation-commands/252-protection-switch.md`, `06-operation-commands/177-manual-switchover.md`, `06-operation-commands/251-protection-group.md`, `06-operation-commands/253-protection-unit.md`

### multi-time-source-troubleshooting

*system-node-time / troubleshooting*

**Q.** The node's time is wrong and my attempt to set it by hand is rejected. What is going on and what should I check?

**A.** Manual time setting is only possible when the node is not synchronising from NTP: set-time is only applicable when time-source is manual, that is when NTP is not enabled. Check show clock, whose time-source attribute reads ntp or manual; if it reads ntp, the node is under NTP control and set-time does not apply. Check the ntp object's ntp-enabled flag, and ntp-server-status for whether the configured server is actually being reached, where reach 377 means all recent probes were answered. If you do set the time by hand, the format is ISO 8601 derived, for example 2021-02-06T11:16:58Z with Z for UTC or an explicit +/-hh:mm offset, and the time command reads the value back in the system configured timezone rather than the one you typed. Note also that show clock can take up to 2 minutes to respond if DNS is not properly configured, and that clock exposes last-time-jump, which records jumps larger than 10 seconds.

Source: `06-operation-commands/289-set-time.md`, `06-operation-commands/052-clock.md`, `06-operation-commands/195-ntp.md`, `06-operation-commands/351-time.md`

Not stated by the document: The causal link (NTP enabled therefore set-time rejected) is stated in set-time's description; the specific error text the CLI returns is not documented.

### multi-node-identity-and-restart

*system-node-time / which-commands-together*

**Q.** Before I reboot a card on an unfamiliar node, how do I find out what the node is, what is in it, and what the reboot will cost me?

**A.** ne holds the node's identity: ne-id, ne-name (default '1830 GX'), ne-type (G30 or G40), node-type (ILA, OADM or XPDR), plus site, location and contact fields, and equipment-discovery-ready which stays false until all equipment has been discovered at startup. status with no argument gives the system dashboard, summarising NE properties, software labels, uptime, management IP configuration, an alarm summary by severity and an equipment summary; status equipment lists chassis, cards and toms with temperature and power. uptime reports how long the system has been up along with a load average, which the guide says is high when it exceeds the number of CPUs on the card. restart then performs the reboot: warm is the default and is non service affecting, cold reboots all components and sub-components and is service affecting, and shutdown gracefully shuts the card down; with no resource-id it restarts the active controller card, it asks for confirmation unless -f is given, and not all cards support all restart types.

Source: `06-operation-commands/186-ne.md`, `06-operation-commands/312-status.md`, `06-operation-commands/365-uptime.md`, `06-operation-commands/265-restart.md`

Not stated by the document: The guide does not prescribe a pre-reboot checklist. Grouping these four commands as a sequence is editorial.
