---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.240. pm-parameter'
source_lines: 18800-18859
---

## 6.240. pm-parameter

#### Command Description

This command is used to show pm parameter information. The `show pm-parameter` command displays a list of pm parameters.

#### Command Syntax

```
show pm-parameter-<parameter> [units] [type]
```

#### Command Usage Details

**Table 572: pm-parameter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 573: pm-parameter Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| parameter | PM parameter identifier (can be a counter or a gauge). | identityref (pm parameter) | n/a | show |
| units | Units for the parameter. | na dBm ms ps ps/nm dB seconds packets events octets bits blocks times percent bit-ratio C frames W V A rpm ps2 mA words cw nm bytes errors MHz | n/a | show |
| type | Type of PM parameter, it can be either a counter or a gauge. | counter gauge | n/a | show |

#### Examples

This example shows how to view a list of pm parameters:

```
show pm-parameter
```

This example shows how to view pm parameters with parameter=severely-errored-seconds:

```
show pm-parameter-severely-errored-seconds
```

This example shows how to view the pm parameter information:

```
show pm-parameter pm-parameter-undersized
```

The following output is displayed:

```
  pm-parameter-undersized
  units                                packets
  type                                 counter
```

<!-- page 949 -->
