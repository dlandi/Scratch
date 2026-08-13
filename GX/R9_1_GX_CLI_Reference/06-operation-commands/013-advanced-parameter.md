---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.13. advanced-parameter'
source_lines: 4939-5024
---

## 6.13. advanced-parameter

#### Command Description

The commands described in this section are used to add, configure, show, or delete advanced parameters. These can be executed on the node only after the `enable-advanced-parameters` on the optical carrier is set to `true`. For a detailed procedure to configure advanced parameters, refer to the *1830 GX Management Interfaces User Guide*.

**Note:** If no advanced parameter is configured or the advanced parameter is deleted, running `show advanced-parameter` will report `ERROR: object` `does not exist`.

**Note:** If an advanced parameter is configured with an invalid value, the configuration is not rejected; running `show advanced-parameter` will report the `failed` status for the advanced parameter.

#### Command Syntax

```
add advanced-parameter-[<card-name>-<port-name>|<optical-carrier-name>]/<advanced-parameter-name> value <value>
set advanced-parameter-[<card-name>-<port-name>|<optical-carrier-name>]/<advanced-parameter-name> [value <value>]
show advanced-parameter-[<card-name>-<port-name>|<optical-carrier-name>]/<advanced-parameter-name> [value] [status]
delete advanced-parameter-[<card-name>-<port-name>|<optical-carrier-name>]/<advanced-parameter-name>
```

#### Command Usage Details

**Table 95: advanced-parameter Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 176 -->

#### Command Parameters

**Table 96: advanced-parameter Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| card-name | The name of the card supporting the advanced parameter. | string | n/a | add, set, delete, show |
| port-name | The name of the port supporting the advance parameter. | string | n/a | add, set, delete, show |
| optical-carrier-name | The name of the optical carrier. | string | n/a | add, set, delete, show |
| advanced-parameter-name | The name of the advanced parameter. | string length 0..256 For the list of advanced parameters that are supported on CHM6 and CHM7/7X, refer to 1830 GX System Description Guide. | n/a | add, set, delete, show |
| value | The value set for the advanced parameter. | For the list of values for advanced parameters that are supported on CHM6 and CHM7/7X, refer to 1830 GX System Description Guide. | For the default values for advanced parameters that are supported on CHM6 and CHM7/7X, refer to 1830 GX System Description Guide. | add, set, delete, show |
| status | The current state of the advanced parameter. | • failed<br>• in-progress<br>• not-supported<br>• set<br>• unknown | unknown | show |

#### Examples

This example shows how to add an advanced parameter on a CHM6 optical carrier:

<!-- page 177 -->

```
add advanced-parameter-1-6-L1-1/FFCRAvgN value 3
```

This example shows how to add an advanced parameter on a CHM7/7X optical carrier:

```
add advanced-parameter-1-4-L1-1/EEPNNLMitigation value "0 1"
```

This example shows how to modify advanced parameter settings on a CHM7/7X optical carrier:

```
set advanced-parameter-1-4-L1-1/EEPNNLMitigation value "1 8"
```

These examples show how to view the configured advanced parameter(s):

```
show advanced-parameter
```

Example of an output retrieved from the system:

```
advanced-parameter                            value  status
--------------------------------------------  -----  ------
advanced-parameter-1-4-L1-1/EEPNNLMitigation  1 8    set
```

This example shows how to delete an advanced parameter:

```
delete advanced-parameter-1-4-L1-1/EEPNNLMitigation
```

<!-- page 178 -->
