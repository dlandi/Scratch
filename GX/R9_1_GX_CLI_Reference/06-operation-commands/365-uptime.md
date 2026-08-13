---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.365. uptime'
source_lines: 27287-27326
---

## 6.365. uptime

#### Command Description

This command displays the system uptime and load average. The load average, also called average system load, is an important metric that indicates if there are multiple tasks in queue on the system (on the card). The load average can be high or low, depending on: the number of cores system has, how many CPUs are integrated into the system (card), and the load average number itself. A load average value is considered to be high when it’s greater than the number of CPUs on the card.

#### Command Syntax

```
uptime
```

#### Command Usage Details

**Table 839: uptime Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

None.

#### Examples

The following example shows how to view the uptime retrieved output from one NE:

```
uptime
```

The following output is displayed:

```
21:24:09 up 41 days, 15:56, 1 user, load average: 0.10, 0.35, 0.39
```

<!-- page 1333 -->
