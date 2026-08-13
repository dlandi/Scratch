---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.35. cable-id-status'
source_lines: 6228-6274
---

## 6.35. cable-id-status

#### Command Description

The command described in this section is used to show the `cable-id-status` attributes. This is the container that holds the process status and progress information of a CableID-based fiber connection verification. A user is allowed to issue the `show cable-id cable-id-status` command anytime to query the progress.

#### Command Syntax

```
show cable-id-status [cable-id-state] [test-progress]
```

#### Command Usage Details

**Table 141: cable-id-status Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 142: cable-id-status Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| cable-id-state | Display the cable-id state:<br>• idle - cable-id verification is not running.<br>• running-incl-switching - cable-id verification is running for both active and protected paths.<br>• running-no-switching - cable-id verification is running only for active path. | • idle<br>• running-incl-switching<br>• running-no-switching | idle | show |
| test-progress | Display the cable-id test progress. It uses a string to show the progress:<br>• "Not applicable" - If cable-id-state is disabled.<br>• "N out of M completed" - If cable-id-state is "running", where: ▪ M = total number of port-pairs to be tested ▪ N = total number of port-pairs with testing completed | string (length 0..255) | 'None' | show |

#### Examples

The following command shows an example on how to view the CableID verification test status:

```
show cable-id-status
```

with the following result:

```
  cable-id-status
  cable-id-state               idle
  test-progress                '1 out of 1 completed.'
```

<!-- page 259 -->
