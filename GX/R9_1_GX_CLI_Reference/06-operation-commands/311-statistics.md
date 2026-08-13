---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.311. statistics'
source_lines: 23766-23822
---

## 6.311. statistics

#### Command Description

The command described in this section is used to clear the event counters (statistics) for the specified objects.

**Note:** Currently, the supported object type is aaa-server.

**Note:** AAA statistics are supported for TACACS+ servers but not for RADIUS servers.

#### Command Syntax

```
clear [-f] statistics [target=]<value>[,<value>]*
```

#### Command Usage Details

**Table 722: statistics Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 723: statistics Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Forces the command without confirmation. |

<!-- page 1172 -->

**Table 724: statistics Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| target | Objects that will have their event counter statistics cleared. | instance-identifier | n/a | clear |

#### Examples

The following command shows an example on how to clear the statistics for all AAA servers:

```
clear statistics aaa-server
```

The following command shows an example on how to clear the statistics for AAA server `MyServer`:

```
clear statistics aaa-server-MyServer
```

<!-- page 1173 -->
