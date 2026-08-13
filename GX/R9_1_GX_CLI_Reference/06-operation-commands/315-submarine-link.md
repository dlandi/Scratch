---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.315. submarine-link'
source_lines: 24219-24352
---

## 6.315. submarine-link

#### Command Description

The commands described in this section are used to add or delete `submarine-link` object and set or show the submarine link topology including Branching Units (BU) and subsea link attributes.

#### Command Syntax

```
add submarine-link-<name> src-node-id <value> src-port-name <value> dst-node-id <value> dst-port-name <value> degree-target-tx-power <value>
[label <value>] [src-card-name <value>] [dst-card-name <value>] [fiber-connection-type <value>] [link-name <value>] [fiber-pair-id <value>]
[fiber-length <value>] [segment-list <value>] [bu-segment-index <value>] [rx-fiber-type <value>] [tx-fiber-type <value>] [gsnr <value>]
[degree-expected-rx-power <value>] [commissioning-snr-margin <value>] [launch-condition <value>] [allocated-spectrum-list <value>]
delete submarine-link-<name>
set submarine-link-<name> [label <value>] [src-node-id <value>] [src-card-name <value>] [src-port-name <value>] [dst-node-id <value>]
[dst-card-name <value>] [dst-port-name <value>] [fiber-connection-type <value>] [link-name <value>] [fiber-pair-id <value>] [fiber-length
<value>] [segment-list <value>] [bu-segment-index <value>] [rx-fiber-type <value>] [tx-fiber-type <value>] [gsnr <value>] [degree-target-tx-power
<value>] [degree-expected-rx-power <value>] [commissioning-snr-margin <value>] [launch-condition <value>] [allocated-spectrum-list <value>]
show submarine-link-<name> [label] [src-node-id] [src-card-name] [src-port-name] [dst-node-id] [dst-card-name] [dst-port-name]
[fiber-connection-type] [link-name] [fiber-pair-id] [fiber-length] [segment-list] [bu-segment-index] [rx-fiber-type] [tx-fiber-type] [gsnr]
[degree-target-tx-power] [degree-expected-rx-power] [commissioning-snr-margin] [launch-condition] [allocated-spectrum-list]
```

#### Command Usage Details

**Table 731: submarine-link Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

<!-- page 1191 -->

#### Command Parameters

**Table 732: submarine-link Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the entity. | String (length 0..64) | n/a | add, set, show, delete |
| label | User defined label. | String (length: 0..256) | n/a | add, set, show |
| src-node-id | Indicates the "NE name" of the source node that originates the current submarine link. This is a mandatory attribute. Note that in a subsea topology, the same src-node-id can connect to multiple dst-node-ids indicating the presence of intervening BUs. | String (length 0..256) | n/a | add, set, show |
| src-card-name | The user defined name for the card that hosts the line facing OTS at the source node. | String (length 0..64) | n/a | add, set, show |
| src-port-name | Identifies the OTS port (dwdm-line) of the degree of the source node endpoint from the link. This is a mandatory attribute. | String (length 0..128) | n/a | add, set, show |
| dst-node-id | Indicates the "NE name" of the destination node associated with the current submarine link. This is a mandatory attribute. | String (length 0..256) | n/a | add, set, show |
| dst-card-name | The user defined name for the card that hosts the line facing OTS at the destination node. | String (length 0..64) | n/a | add, set, show |
| dst-port-name | Identifies the OTS port (dwdm-line) of the degree of the destination node endpoint from the link. It is a mandatory attribute. | String (length 0..128) | n/a | add, set, show |
| fiber-connection-type | This attribute indicates the type of fiber connection for the submarine link. It is two-way only for submarine links. | • two-way<br>• one-way | two-way | add, set, show |
| link-name | Defines the name of the link. | String (length 0..256) | n/a | add, set, show |
| fiber-pair-id | Defines the fiber pair ID of the fiber in the fiber bundle that is associated with the link. | String (length 0..128) | n/a | add, set, show |
| fiber-length | Defines the fiber length, in km, of the associated fiber pair ID. This does not include the length of the branch segments. | decimal64 (range: 0..25000 km) | 0 | add, set, show |
| segment-list | Defines the list of fiber segments that make up the submarine link. | String (length 0..512) | n/a | add, set, show |
| bu-segment-index | Defines the index of the segment location associated to the BU. Defines how many segments away the branching unit is located from the branch node. This is 1 in most cases where the terminating branch node is 1 fiber segment away to the associated BU. In cases where there are hierarchical branching units present on the same fiber, it represents the number of segments to reach the BU associated to the trunk fiber. It is 0 for the end nodes. | uint16 | 0 | add, set, show |
| rx-fiber-type | Indicates the Rx fiber type of the link. | String (length 0..64) | n/a | add, set, show |
| tx-fiber-type | Indicates the Tx fiber type of the link. | String (length 0..64) | n/a | add, set, show |
| gsnr | Indicates the expected GSNR of the link (dB). | decimal64 | n/a | add, set, show |
| degree-target-tx-power | Indicates the target transmit power for the degree. It is for the launch power at the ROADM into the primary fiber link. This is a mandatory parameter for the link. | decimal64 | n/a | set, show |
| degree-expected-rx-power | Indicates the expected receive power at the degree. It is for the total expected receive power at the degree and not for the individual submarine links. | decimal64 | n/a | add, set, show |
| commissioning-snr-margin | SNR margin at the time of commissioning. | decimal64 | n/a | add, set, show |
| launch-condition | Defines the launch option for the Tx pre-emphasis. | • flat-tx<br>• pfib | pfib | add, set, show |
| allocated-spectrum-list | Allocated spectrum blocks for the link configured as a set of start frequency, end frequency pairs. It is a list of frequencies defined in a (start-freq, end-freq ) tuple fashion indicating the chunks of bandwidth that this submarine-link will be able to utilize for L0 services. | list of frequencies (max 32 elements) | n/a | add, set, show |

#### Examples

The following command shows how to retrieve all submarine-link related entities and parameters:

```
show submarine-link
```

For example:

```
submarine-link            label  src-node-id  src-card-name  src-port-name  dst-node-id
------------------------  -----  -----------  -------------  -------------  -----------
submarine-link-MD-DLS-01         GX-CLS1      1-3            dwdm-line      GX-CLS2
submarine-link-MD-DLS-02         GX-CLS1      1-3            dwdm-line      GX-Branch
submarine-link            dst-card-name  dst-port-name  fiber-connection-type  link-name
------------------------  -------------  -------------  ---------------------  ---------
submarine-link-MD-DLS-01  1-3            dwdm-line      two-way                MD-DLS-01
submarine-link-MD-DLS-02  1-3            dwdm-line      two-way                MD-DLS-02
submarine-link            fiber-pair-id  fiber-length (km)  segment-list  bu-segment-index
------------------------  -------------  -----------------  ------------  ----------------
submarine-link-MD-DLS-01  MD-FP02        9600.000                         0
submarine-link-MD-DLS-02  MD-FP02        9600.000                         0
submarine-link            rx-fiber-type  tx-fiber-type  gsnr (dB)  degree-target-tx-power (dBm)
------------------------  -------------  -------------  ---------  ----------------------------
submarine-link-MD-DLS-01  ULL            ULL            12.00      14.00
submarine-link-MD-DLS-02  ULL            ULL            12.00      14.00
submarine-link            degree-expected-rx-power (dBm)  commissioning-snr-margin (dB)
------------------------  ------------------------------  -----------------------------
submarine-link-MD-DLS-01  5.00                            0.50
submarine-link-MD-DLS-02  5.00                            0.50
submarine-link            launch-condition  allocated-spectrum-list (MHz)
------------------------  ----------------  ---------------------------------------
submarine-link-MD-DLS-01  flat-tx           191800000,194000000,195000000,196000000
submarine-link-MD-DLS-02  flat-tx           194000000,195000000
```

The following command shows an example on how to retrieve the parameters from submarine-link-MD-DLS-01:

```
show submarine-link-MD-DLS-01
```

The output is as follows:

```
  submarine-link-MD-DLS-01
  label                                 ''
  src-node-id                           'GX-CLS1'
  src-card-name                         '1-3'
  src-port-name                         'dwdm-line'
  dst-node-id                           'GX-CLS2'
  dst-card-name                         '1-3'
  dst-port-name                         'dwdm-line'
  fiber-connection-type                 two-way
  link-name                             'MD-DLS-01'
  fiber-pair-id                         'MD-FP02'
  fiber-length                          9600.000 km
  segment-list                          ''
  bu-segment-index                      0
  rx-fiber-type                         'ULL'
  tx-fiber-type                         'ULL'
  gsnr                                  12.00 dB
  degree-target-tx-power                14.00 dBm
  degree-expected-rx-power              5.00 dBm
  commissioning-snr-margin              0.50 dB
  launch-condition                      flat-tx
  allocated-spectrum-list               191800000,194000000,195000000,196000000 MHz
```

<!-- page 1196 -->
