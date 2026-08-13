---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.52. clock'
source_lines: 7239-7285
---

## 6.52. clock

#### Command Description

These commands are used to set or show the system clock.

#### Command Syntax

```
set clock [timezone <value>]
show clock [current-time] [universal-time] [timezone] [uptime] [uptime-seconds] [time-source] [DST-active] [last-time-jump]
```

**Note:** If the DNS server is not properly configured, it may take up to 2 minutes to retrieve the response from the `show clock` command.

#### Command Usage Details

**Table 179: clock Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 180: clock Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| current-time | Indicates the current Date and Time of this NE. | String | n/a | show |
| universal-time | Indicates the UTC Date and Time of this NE. | String | n/a | show |
| timezone | Indicates the Name of the Time Zone of this NE. | International Date Line West[GMT-12:00] _ _ _ Midway Island-Samoa[GMT-11:00] _ Hawaii[GMT-10:00] Alaska[GMT-09:00] Pacific Time[US and Canada][GMT-08:00] _ _ _ Arizona[GMT-07:00] Mountain Time[US and Canada][GMT-07:00] _ _ _ CentralAmerica[GMT-06:00] Central Time[US and Canada][GMT-06:00] _ _ _ Mexico City-Tegucigalpa[GMT-06:00] _ Saskatchewan[GMT-06:00] Bagota-Lima-Quito[GMT-05:00] Eastern Time[US and Canada][GMT-05:00] _ _ _ Indiana[East][GMT-05:00] Caracas-La Paz[GMT-04:30] _ Atlantic Time[Canada][GMT-04:00] _ Santiago[GMT-04:00] Newfoundland[GMT-03:30] Brasilia[GMT-03:00] Buenos Aires-Georgetown[GMT-03:00] _ Greenland[GMT-03:00] Mid-Atlantic[GMT-02:00] Azores[GMT-01:00] Cape Verde Is.[GMT-01:00] _ _ Casablanca-Monrovia[GMT] Greenwich Mean Time:Dublin-Edinburgh-Lisbon-London[GMT] _ _ Amsterdam-Copenhagen-Madrid-ParisVilnius[GMT+01:00] Belgrade-Sarajevo-Skopje-Sofija-Zargreb[GMT+01:00] Bratislava-Budapest-Ljublijana-Prague-Wasaw[GMT+01:00] Brussels-Berlin-Bern-Rome-Stockholm-Vienna[GMT+01:00] West Central Africa[GMT+01:00] _ _ Athens-Istanbul-Minsk[GMT+02:00] Bucharest[GMT+02:00] Cairo[GMT+02:00] Harare-Pretoria[GMT+02:00] Helsinki-Riga-Tallinn[GMT+02:00] Jerusalem[GMT+02:00] Israel[GMT+02:00] Baghdad[GMT+03:00] Kuwait-Riyadh[GMT+03:00] Moscow-St.Petersburg-Volgograd[GMT+03:00] Nairobi[GMT+03:00] Tehran[GMT+03:30] Abu Dhabi-Muscat[GMT+04:00] _ Baku[GMT+04:00] Tbilisi[GMT+04:00] Kabul[GMT+04:30] Ekaterinburg[GMT+05:00] Islamabad-Karachi-Tashkent[GMT+05:00] Mumbai-Calcutta-Chennai-New Delhi[GMT+05:30] _ Colombo[GMT+05:30] Kathmandu[GMT+05:45] Dhaka[GMT+06:00] Almaty[GMT+06:00] Rangoon[GMT+06:30] Bangkok-Hanoi-Jakarta[GMT+07:00] Beijing-Chongqing-Hong Kong-Urumqi[GMT+08:00] _ Perth[GMT+08:00] Singapore-Kuala Lumpur[GMT+08:00] _ Taipei[GMT+08:00] Osaka-Sapporo-Tokyo[GMT+09:00] "Seoul[GMT+09:00] Yakutsk[GMT+09:00] Adelaide[GMT+09:30] Darwin[GMT+09:30] Brisbane[GMT+10:00] "Canberra-Melbourne-Sydney[GMT+10:00] Guam-Port Moresby[GMT+10:00] _ Hobart[GMT+10:00] Vladivostok[GMT+10:00] Magadan-Solomon Is.-New Caledonia[GMT+11:00] _ _ Auckland-Wellington[GMT+12:00] Fiji-Kamchatka-Marshall Is.[GMT+12:00] _ Eniwetok-Kwajalein[GMT+12:00] Nuku alofa[GMT+13:00] _ Kiritimati[GMT+14:00] Universal-Time-Coordinated | Universal-Time- Coordinated | set, show |
| uptime | Indicates how long the system has been running. | String (length 0..200) | n/a | show |
| uptime-seconds | Indicates how long the system has been running, in seconds. | uint32 | n/a | show |
| time-source | Indicates the source of the system current time. ntp - Indicates that NE uses NTP for synchronization. manual - Indicates that NE uses NE internal clock for Synchronization. | ntp, manual | manual | show |
| DST-active | Whether daylight saving is active. | true, false | false | show |
| last-time-jump | Indicates last system time jump in the format '&lt;time1&gt; to &lt;time2&gt;'. Time jumps of less than 10 seconds are ignored. | string (length 0..200) | n/a | show |

#### Examples

```
show clock  #shows system's clock attributes.
set clock timezone Beijing-Chongqing-Hong_Kong-Urumqi[GMT+08:00]  #sets the system clock timezone.
```

<!-- page 320 -->
