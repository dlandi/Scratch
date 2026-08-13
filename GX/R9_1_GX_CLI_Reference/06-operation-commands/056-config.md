---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.56. config'
source_lines: 7558-7582
---

## 6.56. config

#### Command Description

The `show config` displays the system's configuration. It displays non-default configurations (configurations that have their default values are skipped). The displayed configuration is fully recursive from the current CLI scope, so doing this command at the top of the CLI hierarchy will provide the complete system configuration. Alternatively, an \<entity-id\> can be provided to limit the scope of the output. If all entities of a given type are relevant, it is possible to provide the \<entity-type\> instead.

**Tip:** A good way to create a CLI script that can be used to restore the system configuration is by using `show config | display commands` and storing the result for later usage. See display (p. 113) command for details.

#### Command Syntax

```
show config [(<entity-id>|<entity-type>)]
```

#### Command Parameters

**Table 190: config Command Parameters**

| Parameter | Description |
| --- | --- |
| entity-id | Instance ID of the entity to retrieve the configuration. |
| entity-type | Entity type to retrieve the configuration. |

<!-- page 335 -->
