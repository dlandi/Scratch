---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.76. degree'
source_lines: 8697-8751
---

## 6.76. degree

#### Command Description

These commands are used to add, delete a degree and to set or show the degree attributes.

#### Command Syntax

```
add degree-<degree-number> [label <value>]
delete degree-<degree-number>
set degree-<degree-number> [label <value>]
```

\<p class="- topic/p "\>\<userinput class="+ topic/ph sw-d/userinput "\>show degree-&lt;degree-number&gt; [label] [is-foadm] [wss-less]\</userinput\>\</p\>

```
show degree-<degree-number> [label] [is-foadm] [wss-less] [slot-width-granularity] [center-freq-granularity] [min-slots] [max-slots]
[bands-supported]
```

#### Command Usage Details

**Table 233: degree Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 234: degree Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| degree-number | Degree number should be greater than zero and not greater than max-degrees. | integer in the range [1..20] | n/a | add, set, show, delete |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| is-foadm | True if there is no WSS component at the Degree (at 'modules-degree') and PAx assigned to the degree appropriately. | true, false | false | show |
| wss-less | Indicates if there is a WSS component or not. The value is true if there is no WSS component in the Degree. The system sets autonomously this attribute to 'true' when supporting required-type card is PAx. | true, false | true | show |
| slot-width-granularity | Width of a slot (measured in GHz). | value in MHz | 6250 | show |
| center-freq-granularity | Granularity of allowed center frequencies. The base frequency for this computation is 193.1 THz (G.694.1). | value in MHz | 3125 | show |
| min-slots | Minimum number of slots permitted to be joined together to form a media channel. Must be less than or equal to the max-slots. | integer | 8 | show |
| max-slots | Maximum number of slots permitted to be joined together to form a media channel. Must be greater than or equal to the min-slots. | integer | 32 | show |
| bands-supported | List of bands supported by a degree, with dependence on supported cards.<br>• not-applicable -Transmission band not applicable.<br>• standardC-band - Standard C-band (4.85 THz).<br>• superC-band - SuperC-band (6.1 THz).<br>• standardL-band - Standard L-band (4.85 THz).<br>• standardC-standardL-band - Standard C or Standard L band. | • not-applicable<br>• standardC-band<br>• superC-band<br>• standardL-band<br>• standardC-standardL-band | standardC-band | show |

#### Examples

This example shows how to add a degree to the OADM topology:

```
add degree-1
```

<!-- page 383 -->
