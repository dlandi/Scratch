---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.49. clear'
source_lines: 7053-7120
---

## 6.49. clear

#### Command Description

The clear command clears entries for the specified entity. This command is used to clear the entry/entries for the specified entity/entities. Each sub command handles a specific type of clear operation. In all cases, the 'clear' command requires a user confirmation. The -f flag can be used to force the command without confirmation. A confirmation prompt will be displayed if the -f flag is not provided.

**Note:** The `clear system factory-reset` with the `shutdown` option is not supported on the L0 cards.

#### Command Syntax

```
clear -h
clear [-f] pm [-i=<value>] [data-type=]<value> [[period=]<value>] [[direction=]<value>] [[location=]<value>] [[resource=]<value>]
[[resource-type=]<value>] [[AID=]<value>]
clear [-f] topology [target=]<value>
clear [-f] log [[clear-target=]<value>] [log-file-name=]<value> [[target-entity=]<value>]
clear [-f] ospf [instance=]<value>
clear [-f] database [[clear-type=]<value>] [script=]<value> [new-admin-user=]<value> [new-admin-password=]<value>
clear [-f] isk [key-name=]<value>
clear [-f] file [filetype=]<value> [target-file=]<value>
clear [-f] app [app-name=]<value>
clear [-f] certificate [type=]<value> [id=]<value>
clear [-f] crl [[clear-target=]<value>] [crl-name=]<value>
clear [-f] recover-mode
clear [-f] system [type=]<value> [[target=]<value>] [[restart-behavior=]<value>] [[action=]<value>]
clear [-f] alarm [alarm-type=]<value> [[resource=]<value>[,<value>]*]
clear [-f] dns [[target=]<value>]
clear [-f] statistics [target=]<value>[,<value>]*
```

#### Command Usage Details

**Table 172: clear Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 173: clear Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Forces the command without confirmation. |

**Table 174: clear Command Parameters**

| Subcommands | Description |
| --- | --- |
| alarm | Clear alarms that have no auto criteria to be cleared. For additional details, refer to alarm (p. 178). |
| app | Clears installed apps. For additional details, refer to app (p. 213). |
| certificate | Clears installed x509 certificates. For additional details, refer to certificate (p. 287). |
| crl | Clears one or more installed Certificate Revocation Lists (CRLs) from the system. For additional details, refer to crl (p. 349). |
| database | Set NE database to default and reboots the system. For additional details, refer to database (p. 369). |
| file | Removes a particular file from the system. For additional details, refer to file (p. 491). |
| isk | Deletes Image Signing Key (ISK) from the system. For additional details, refer to ISK (p. 591). |
| log | Removes content for a specific log-file. For additional details, refer to log (p. 633). |
| ospf | Clears an ospf-instance asynchronously. For additional details, refer to ospf (p. 837). |
| pm | Removes or resets PM data. For additional details, refer to pm (p. 934). |
| recover-mode | Clears recover-mode flag For additional details, refer to recover-mode (p. 1004). |
| system | Wipes the system/specific instance and resets to the factory configurations. For additional details, refer to system (p. 1256). |
| topology | Manually removes existing topology neighbor information. For additional details, refer to topology (p. 1292). |
| statistics | Clears event counters (statistics) for the specified objects. For additional details, refer to statistics (p. 1171). |

<!-- page 310 -->
