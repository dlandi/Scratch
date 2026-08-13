---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.312. status'
source_lines: 23823-24107
---

## 6.312. status

#### Command Description

This command is used to display multiple dashboard-type outputs. The term dashboard refers to a summary view of a specific type of information about the systemobjects presented in a text-based format. Using these dashboards facilitates retrieving related system information with one command. The followingFive dashboards are available: System, Equipment, L0-oxcon, and L0-ocm, L0-spectrum and L1-traffic.

**System Dashboard** The System dashboard is a summary of relevant high level information about the system. This dashboard provides important system details in an easy to read format, including:

- NE properties (name, ID, type)
- SW labels
- Uptime
- Management info (IP config)
- Alarm summary (number of alarms, per severity)
- Equipment summary (number of chassis, cards, toms, per types)
- Services summary (number of xcons, per type, number of oxcons)

This dashboard can be invoked with the '**status system**' command, or simply with the '**status**' command alone (this acts as the default dashboard).

**Equipment Dashboard** The Equipment dashboard provides a summary of all equipment in the system, together with relevant details like temperature and power usage. The default for this dashboard is the entire network element. This dashboard type allows the user to specify the scope of relevant equipment at the chassis level that can be scoped by port AID with wildcard support. This dashboard provides an equipment view of the system, including:

<!-- page 1174 -->
- list of chassis
- list of cards per chassis and their subtype
- list of toms per card
- relevant data per equipment, including: **▪**temperature sensors **▪**power values
- generic status **▪**presence or absence **▪**oper state **▪**alarms

For large NEs, the output of this dashboard may be quite large and it is therefore possible to filter it per chassis.

- This can be done by providing the chassis id as a parameter (e.g. **status equipment 5**).
- Only existing chassis ids are accepted; wildcards are not supported.
- This means that either all chassis are displayed (without AID filter), or just one (with AID filter).

**L0-oxcon Dashboard** (1830 GX G30 only) This dashboard provides a view of system's L0 Optical Cross Connections (OXcons), entities that are specific for L0 setups. An OXcon relates two NMC objects (Network Media Channel), so this dashboard provides an aggregated view of both the OXcon properties, as well as information associated with the cards and ports of the end points of the connection, together with some power values. The output is a table, where the first column will have the name of the parameters, and each additional column will represent a single OXcon; the table is split if the screen width is not enough, so that each OXcon is present in a single table, and not split. Given that the source and destination of the OXcon contains multiple interesting details, they contain a small tree structure, as visible in the example below. This dashboard supports a *filter* parameter, where a degree number can be optionally provided (ex: **status L0-oxcon 2** for displaying all OXcons related with degree 2).

<!-- page 1175 -->

Not applicable parameters appear with '---', and power values will appear in green, yellow or red in order to reflect positive or negative status (relative to the values it is presenting).

**L0-ocm Dashboard** (1830 GX G30 only) This dashboard provides a view from the L0 OCM (Optical Channel Monitor) point of view. It lists, per degree, information regarding Line In and Line Out directions, as well as listing related NMCs per direction and associated parameters. This dashboard supports a *filter* parameter, where a degree number can be optionally provided (ex: **status L0-ocm 3** for displaying OCM status related with degree 3).

**L0-spectrum Dashboard** (1830 GX G30 only)

The L0-spectrum dashboard provides the spectrum power in a horizontal line, using an ASCII character-set. It is applicable to RD66TM and G2PBAL cards (type: PBAx) card types only. \<p class="- topic/p "\>The L0-spectrum dashboard provides the spectrum power in an horizontal line, using an ASCII character-set. It is applicable to RD66TM and G2PBAL cards only.\</p\> The following command syntax is used:

```
status L0-spectrum "<degree> [<lower-frequency> <upper-frequency> [units]]"
```

If the lower/ upper-frequency is not provided, OCM results in the active frequency ranges are shown for the entire spectrum, in 6.25 GHz steps (results with values above -20.00 dBm). The optional attribute **units** can be set to *dBm* (default value) or to *mW*. \<p class="- topic/p "\>If the lower/ upper-frequency is not provided, OCM results in the active frequency ranges are shown for the entire spectrum, in 6.25 GHz steps (results with values above -20.00 dBm). The optional attribute \<b class="+ topic/ph hi-d/b "\>units\</b\> can be set to \<i class="+ topic/ph hi-d/i "\>dBm\</i\> (default value) or to \<i class="+ topic/ph hi-d/i "\>mW\</i\>.\</p\> **L1-traffic Port Dashboards** The L1-traffic Port dashboard is a summary table showing all traffic ports in the system and relevant traffic details per port. The default for this dashboard is the entire network element. This dashboard type allows the user to specify the scope of relevant ports that can be scoped by port AID with wildcard support.

<!-- page 1176 -->

This dashboard can be invoked with the **status L1-traffic [AID- filter]** command. The AID filter can optionally be provided to limit the amount of ports displayed in the output. If no AID-filter is provided, all traffic ports in the system are displayed. The filter can be:

- an exact port AID (e.g. **status L1-traffic 1-4-T1** (for 1830 GX G40), **status L1-traffic 2-1-1** (for 1830 GX G30))
- an AID with wildcard (\*) at the right side of the expression: **▪status L1-traffic 2-\*** - this command gets all ports in chassis 2. **▪status L1-traffic 4-6-\*** - this command gets all ports in card-4-6. **▪status L1-traffic 1-4-T\*** - this command gets all T ports (e.g. tributary ports) in 1830 GX G40 card-1-4

**Note:** Wildcards in the middle of the AID (e.g. \*-4-T1 or 1-\*-L1 (1830 GX G40 example)) are not supported - the '\*' needs to be at the end/right side of the AID filter field.

**Note:** Starting from R9.0, (+) is used in the **status L1-traffic** output to indicate that more than one value exists for the parameter with (+) on the port. This applies to SPN2/SPN2C ports in line XR mode when one port has multiple dsc-groups, each one of which has its own PM data. In this case, only the first instance of the PM value (when associated with dsc-groups) is shown. For additional details available, use a standard **show pm** command. The output example (p. 1180) shows an example output, where (+) is shown.

**Usage of** ✔**/**✘**/**🔒❌ **Symbols** For attributes associated with the keywords 'ok'/'not ok'/'none', the display includes some Unicode symbols that simplifies the visual parsing of the information.

- ✔❌ is used for oper-state enabled, admin-state unlocked or no alarms.
- ✘❌ is used for oper-state disabled
- 🔒❌ is used for admin-state lock
<!-- page 1177 -->

#### Command Syntax

```
status [[dashboard=]<system | equipment | L0-oxcon | L0-ocm | L1-traffic>] [[filter=]<value>]
```

#### Command Usage Details

**Table 725: status Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 726: status Command Parameters**

| Parameter | Description | Default |
| --- | --- | --- |
| system | Retrieves the system dashboard which provides a summary of relevant high level information about the system. | entire system |
| equipment | Retrieves the Equipment dashboard which provides a summary of all equipment in the system, together with relevant details like temperature and power usage. | all equipment in the system |
| L0-oxcon | (1830 GX G30 only) Retrieves the OXcon dashboard which provides a view of system's Optical Cross Connections (OXcons), entities that are specific for L0 setups. L0-oxcon represents the optical connections between NMCs and its target versus actual output powers. | all OXcons in the system |
| L0-ocm | (1830 GX G30 only) Retrieves the OCM dashboard which provides a view from an OCM (Optical Channel Monitor) point of view. It lists, per degree, information regarding Line In and Line Out directions, as well as listing related NMCs per direction and associated parameters. L0-ocm represents the NMCs power in Pre-amp and Booster direction. | all OCMs in the system |
| L0-spectrum | (1830 GX G30 only) Retrieves the spectrum power in an horizontal line, using an ASCII character-set. It is applicable to RD66TM and G2PBALPBAx card types. &lt;p class="- topic/p "&gt;Both ingress and egress powers are reported. In noise-loaded systems such as HSC OLS, the ASE Idler noise is also exposed.&lt;/p&gt; Both ingress and egress powers are reported. In noise-loaded systems such as HSC OLS, the ASE Idler noise is also exposed. The attribute units is optional and can be set to dBm (default value) or to mW. | - |
| L1-traffic | Retrieves the L1 traffic dashboard which provides a table containing all configured L1 traffic ports in the system, and information associated with each port. By 'configured', it means that there are facilities configured on that port. Example: a tributary port without a configured TOM is not considered as a configured port; same for a CHM6 line port which does not have a super-channel associated with it. | all ports |
| filter | For some dashboards, allows to specify an AID filter, reducing the scope of the output. For the 'equipment' dashboard, the filter needs to be an existing chassis id. For the 'L1-traffic' dashboard, the filter can be a specific port AID, or a wildcard based AID, where the * needs to be the last character. For the 'L0-oxcon' and 'L0-ocm' dashboards, the filter needs to be a specific degree number. If filter is not provided, all applicable instances are provided in the dashboard output. | the field is empty by default |

#### Examples

This example shows how to display the L1-traffic dashboard from chassis 2, slot 1:

```
status L1-traffic 2-1-*
```

This example shows how to display the L1-traffic dashboard from chassis 1, slot 4 in a 1830 GX G40 node:

```
status L1-traffic 1-4-*
Port Summary               1-4-L1      1-4-T1    1-4-T2    1-4-T3
-------------------------  ----------  --------  --------  ----------
mode                       700M.95P    100GBE    100GBE    100GBE
```

`alarms`                     ✔❌           ✔❌         ✔❌        `ADMIN-LOCK` `oper-state`                 ✔❌           ✘❌         ✘❌        ✘ `admin-state`                ✔❌           ✔❌         ✔❌        🔒

```
rx-power            (dBm)  ---         ---       ---       ---
rx-frequency        (MHz)  0           ---       ---       ---
osnr                 (dB)  ---         ---       ---       ---
pre-fec-ber          (dB)  ---         ---       ---       ---
pre-fec-q            (dB)  ---         ---       ---       ---
corrected-words   (words)  ---         ---       ---       ---
uncorrected-words (words)  ---         ---       ---       ---
cd                (ps/nm)  ---         ---       ---       ---
dgd                  (ps)  ---         ---       ---       ---
tx-power            (dBm)  ---         ---       ---       ---
tx-frequency        (MHz)  0           ---       ---       ---
bit-rate           (Gbps)  700         ---       ---       ---
baud-rate         (Gbaud)  95.2965202  ---       ---       ---
fec-type                   ---         ---       ---       ---
modulation-format          700M.95P    ---       ---       ---
```

This example shows the L1-traffic dashboard output where (+) is used:

```
Port Summary               1-1-1             1-1-2               1-6-3
-------------------------  ----------------  ------------------  -------------------------
mode                       DP-16QAM-400G-EX  DP-16QAM-PS-400G    DP-16QAM-400G
```

`alarms`                     ✔❌                 ✔❌                   ✔ `oper-state`                 ✘❌                 ✘❌                   ✘ `admin-state`                ✔❌                 ✔❌                   ✔

```
rx-power            (dBm)  32.24             96.29               74.52
rx-frequency        (MHz)  0                 0                   0
osnr                 (dB)  ---               66.0102005          65.3855514(+)
pre-fec-ber          (dB)  ---               7.2036854775808e-5  2.00372036854775808e-1(+)
pre-fec-q            (dB)  ---               63.6650390          89.3812713(+)
corrected-words   (words)  ---               26                  ---
uncorrected-words (words)  ---               27                  ---
cd                (ps/nm)  ---               18.6181583          75.1764373(+)
dgd                  (ps)  ---               95.1263504          32.7877349(+)
tx-power            (dBm)  42.83             52.77               17.71
tx-frequency        (MHz)  0                 0                   0
bit-rate           (Gbps)  400.000           400.000             400.000
baud-rate         (Gbaud)  ---               ---                 ---
fec-type                   ---               ---                 ---
modulation-format          DP-16QAM          DP-16QAM-PS         DP-16QAM
Note: (+) indicates that more than one value exists for that parameter in that port.
```

This example shows how to display the system dashboard:

```
status
```

The following output displays a 1830 GX G40 system dashboard example:

<!-- page 1181 -->

```
status
##################################################################
  ### System Status at 2023-09-13T21:51:21Z ####
  G40 NE   name=GX   id=GXABCDEFGH55
  Active   SW : R7.0.0   G40_ADV-R7.0.0-F-2023.09.13_07_17-x-sim-1096
  Inactive SW :
  Uptime: 1 days 00:37:14
  Management:
    DCN:       ipv4 192.168.122.6 (static) /  ipv6 fd17:625c:f037:a87a:ee3b:3845:5666:e27 (dhcp)
  13 current alarms  (1 critical, 5 major, 2 not-reported, 5 minor)
  Equipment:
        2 chassis     (2 G42)
        2 line-cards  (1 CHM6L, 1 CHM6)
        3 toms        (3 QSFP28)
  Services:
        1 XCONs       (1 100GBE)
##################################################################
```

This example shows how to display the equipment dashboard:

```
status equipment
```

This example shows how to display the equipment dashboard from a 1830 GX G40 node:

```
status equipment
equipment                     type             state                alarms                     stats
------------------------  ------------  -------------------  ---------------------  ----------------------------
│
```

`├───chassis-1               G42 (NC)`    ✔❌ `ok/eqpt discovery                         temp 25ºC | power 2.1/4400 W` `│   ├───card-1-1           XMM4 (Act)`            ✔❌                                           `temp 25ºC` `│   ├───card-1-2            IOPANEL`              ✔

<!-- page 1182 -->

|  | │ ├───card-1-3 ACTIVE-BLANK-MISSING |  |
| --- | --- | --- |
|  | │ ├───card-1-4 CHM6L-L6 ✔ ❌ EQPTCPUR |  |
|  | │ │ ├───tom-1-4-T1 QSFP28 ❌ missing |  |
|  | │ │ ├───tom-1-4-T2 QSFP28 ❌ missing |  |
|  | │ │ └───tom-1-4-T3 QSFP28 ✔ |  |
|  | │ ├───card-1-5 CHM6 🔒❌ ❌ missing ACTIVE-BLANK-MISSING+ |  |
|  | │ ├───card-1-6 ACTIVE-BLANK-MISSING |  |
|  | │ ├───card-1-7 ACTIVE-BLANK-MISSING |  |
|  | │ ├───card-1-FAN-1 FAN ✔ ❌ speed 41% |  |
|  | │ ├───card-1-FAN-2 FAN ✔ ❌ speed 42% |  |
|  | │ ├───card-1-FAN-3 FAN ✔ ❌ speed 43% |  |
|  | │ ├───card-1-FAN-4 FAN ✔ ❌ speed 44% |  |
|  | │ ├───card-1-FAN-5 FAN ✔ ❌ speed 45% |  |
|  | │ ├───card-1-FAN-6 XMM4-FAN ✔ ❌ speed 46% |  |
|  | │ ├───card-1-FAN-7 XMM4-FAN ✔ ❌ speed 47% |  |
|  | │ ├───card-1-FANCTRL-1 FAN-CTRL ✔ ❌ temp 25ºC |  |
|  | │ ├───card-1-PEM-1 PEM-DC ✔ ❌ PWRUV temp 2ºC |  |
|  | │ ├───card-1-PEM-2 PEM-DC ✔ ❌ PWRUV temp 2ºC |  |
|  | │ ├───card-1-PEM-3 PEM-DC ✔ ❌ PWRUV temp 2ºC |  |
|  | │ └───card-1-PEM-4 PEM-DC ✔ ❌ PWRUV temp 2ºC |  |
|  | │ |  |
|  | └───chassis-2 G42 (SC) ❌ missing |  |
|  | ├───card-2-1 |  |
|  | ├───card-2-2 IOPANEL  |  |
|  | ├───card-2-3 |  |
|  | ├───card-2-4 |  |
|  | ├───card-2-5 |  |
|  | ├───card-2-6 |  |
|  | ├───card-2-7 |  |
|  | ├───card-2-FAN-1 FAN  |  |
|  | ├───card-2-FAN-2 FAN  |  |
|  | ├───card-2-FAN-3 FAN  |  |
|  | ├───card-2-FAN-4 FAN  |  |
|  | ├───card-2-FAN-5 FAN  |  |
|  | ├───card-2-FAN-6 XMM4-FAN  |  |
|  | ├───card-2-FAN-7 XMM4-FAN  |  |
|  | ├───card-2-FANCTRL-1 FAN-CTRL  |  |

<!-- page 1183 -->

`├───card-2-PEM-1         PEM-DC`               `├───card-2-PEM-2         PEM-DC`               `├───card-2-PEM-3         PEM-DC`               `└───card-2-PEM-4         PEM-DC`              

✔❌ `oper-state enabled` ❌ `oper-state disabled` 🔒❌ `admin-locked`

This example shows how to display the OXcon dashboard in degree #2 of a 1830 GX G30 node:

```
status L0-oxcon 2
```

This example shows the command to display the L0-ocm dashboard degree#2 of a 1830 GX G30 node:

```
status L0-ocm 2
```

This example shows the command to display the L0-spectrum dashboard for degree#1 with frequency range 193 to 194.5194.025 to 194.175 THz (power in dBm) of a 1830 GX G30 node:

```
status L0-spectrum "1 193000000 194500000194025000 194175000"
```

Figure 8: Showing Spectrum Power (p. 1184) shows the output in a horizontal line, using an ASCII character set.

<!-- page 1184 -->

**Figure 8: Showing Spectrum Power**

![Figure from page 1184](images/figure-p1184-1.png)

<!-- page 1185 -->
