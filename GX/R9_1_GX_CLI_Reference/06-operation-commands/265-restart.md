---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.265. restart'
source_lines: 20210-20308
---

## 6.265. restart

#### Command Description

Restarts a specific resource of the system. This command restarts a specific managed entity that supports being restarted. The 'restart' command requires a confirmation, unless the -f flag is provided. If the \<resource-id\> parameter is not provided, the node controller is restarted. If the \<type\> parameter is not provided, a warm start is done by default. Entities that can be restarted:

- card (cold, warm, shutdown) - restarts an individual card.
- card sub-component (cold, warm) - restarts a sub-component of a single card.
- tom (cold) - restarts an individual tom.

**Note:** Not all cards support all restart types.

**Tip:** A list of card sub-components can be viewed with: `show card resources`.

#### Command Syntax

```
restart [-h] [-f] [<resource-id>] [<type>] [<sub-component>]
```

#### Command Usage Details

**Table 622: restart Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

<!-- page 1017 -->

#### Command Parameters

**Table 623: restart Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Forces command without confirmation. |

**Table 624: restart Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| resource-id | Entity to restart. | • AID of card<br>• AID of tom | If resource is not mentioned in the command argument, then it will restart the active controller card. |
| type | Restart type (cold, warm, shutdown):<br>• cold - Reboots all components and sub-components and is service affecting. System regains connectivity within five minutes. Cold reboot of XMM4 is not service affecting but visibility is lost. Cold reboot of CHM6, UCM4, etc., is service affecting.<br>• warm - Reboots the processor of the CHM6, UCM4, etc., and is non service affecting. System regains connectivity within five minutes. Reboot of XMM4 is non-service affecting but visibility is lost for 3 minutes.<br>• shutdown - Gracefully shuts down the CHM6, UCM4, etc., card. | • cold<br>• warm<br>• shutdown | warm |
| sub-component | Card HW or SW sub-component to restart. | string | n/a |

#### Examples

This example shows how to warm restart main controller:

```
restart
```

This example shows how to restart card 1-4:

```
restart card-1-4
```

Since the \<type\> parameter is not provided, a warm start is performed on card 1-4. This example shows how to cold restart the DCO sub-component of card 1-4:

```
restart card-1-4 DCO cold
```

This example shows how to restart (warm, cold) the DCO sub-component of card 1-6:

```
restart card-1-6 DCO cold
restart card-1-6 DCO warm
```

This example shows how to cold restart the CHM6, UCM4, etc., in slot 1-4 with confirmation :

```
restart card-1-4 cold
System will reboot and will temporarily lose connectivity. Are you sure? [y/n] y
```

This example shows how to warm restart the CHM6, UCM4, etc., in slot 1-6 with confirmation :

```
restart card-1-6 warm
System will reboot and will temporarily lose connectivity. Are you sure? [y/n] y
```

This example shows how to perform a graceful shutdown on CHM6, UCM4, etc., in slot 1-6 with confirmation :

<!-- page 1019 -->

```
restart card-1-6 shutdown
System will shutdown and become unreacheable. Are you sure? [y/n] y
```

<!-- page 1020 -->
