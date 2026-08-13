---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.67. current-fw'
source_lines: 8217-8283
---

## 6.67. current-fw

#### Command Description

These commands are used to show the list of current firmware available in the cards.

#### Command Syntax

```
show current-fw-<card-name>-<port-name>/<fw-name> [fw-version] [expected-fw-version] [fw-status]
show current-fw-<card-name>.<slot-name>/<fw-name> [fw-version] [expected-fw-version] [fw-status]
show current-fw-<chassis-name>-<slot-name>/<fw-name> [fw-version] [expected-fw-version] [fw-status]
show current-fw-<name>/<fw-name> [fw-version] [expected-fw-version] [fw-status]
```

#### Command Usage Details

**Table 213: current-fw Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 214: current-fw Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | The name of the object. | String | n/a | show |
| chassis-name | The name of the chassis. | String (length 0..64) | n/a | show |
| card-name | The name of the card. | This object has parameters that are common to all existing card types (controller, fan, etc). | n/a | show |
| port-name | The name of the port object. | String | n/a | show |
| fw-name | Name of the firmware. | String (length 0..32) | n/a | show |
| fw-version | Current version of the firmware. | String (length 0..32) | n/a | show |
| expected-fw-version | Expected version of the firmware. | String (length 0..32) | n/a | show |
| fw-status | Status for this particular firmware. current - Current firmware is up-to-date. not-current - Current firmware is not up-to-date against the expected one. unavailable - Information on firmware status is currently unavailable | current not-current unavailable | unavailable | show |

#### Examples

This example shows how to display the current firmware available in a card's component:

```
show current-fw-1-3/DCO_1
```

This example shows how to display the firmware information:

```
show current-fw
```

The following output is displayed in a 1830 GX G40 environment:

```
current-fw                      fw-version  expected-fw-version  fw-status
------------------------------  ----------  -------------------  -----------
current-fw-1-1/CORE_BOOT        0x20        0x28                 not-current
current-fw-1-1/FCP_FPGA         0x29        0x29                 current
current-fw-1-1/SecMCU_MG        0x0         0x30015              not-current
current-fw-1-FANCTRL-1/FANCTRL  0xb         0xb                  current
current-fw-1-PEM-3/PEM_AC       0x10101     0x10103              not-current
current-fw-1-PEM-4/PEM_AC       0x10101     0x10103              not-current
```

<!-- page 364 -->
