---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.348. terminate'
source_lines: 26138-26240
---

## 6.348. terminate

#### Command Description

This command is used to terminate a running operation. **Location led test Termination** By providing the keyword 'location-led' as a parameter, this command can terminate a location led test that was previously started by the 'activate' command. The target for the location led test can be either a chassis, or a particular card. Invoking this command when no location led test is active for that location will have no effect. **OTDR** This command will stop an ongoing OTDR measurement for the provided port. **OTDR Fiber Check** This command will stop an ongoing OTDR fiber check for the provided entity. It is possible to terminate an ongoing automatic OTDR test at any time during the execution of the automatic OTDR test. OTDR scan is aborted abruptly. otdr ne function attributes restored to the values before the manually triggered OTDR based Raman fiber check. Logs and available OTDR result files are saved. ots-r-auto-otdr auto-otdr-state is reverted back to the state before the manually triggered OTDR based Raman fiber check, and if the state was *fail*, PUMPS-DISABLED-OTDR-TEST-FAILED alarm is raised again. **Loopback** This command will terminate WSS loopback for the provided entity. All channels under the loopback in a degree must be terminated. User cannot add/remove channels from an active loopback request. **Terminate CableID verification test** After the CableID verification test has started, the user can stop the test before the test is run to its completion. As the CableID verification is performed on port-pair basis, the verification test can be terminated/stopped only after the current port-pair verification test is completed, and before starting the test for the next port-pair. This means that when the test of a port-pair has started, the test for this port-pair will run to its completion, but the test will exit before starting the test for another port-pair. The maximum delay of to terminate is one port-pair verification time.

<!-- page 1275 -->

The CableID verification test is known to be terminated once the cable-id-status **cable-id-state** changes to *idle*. After the command is issued, the user can use the `show cable-id cable-id-status` command to confirm the cable-id entity is back to the *idle* state, meaning the test has been stopped.

#### Command Syntax

```
terminate -h
terminate location-led <entity>
terminate otdr [entity=]<value>
terminate otdr-fiber-check [entity=]<value>
terminate loopback [entity=]<value> [, <value>]*
terminate cable-id
```

#### Command Usage Details

**Table 798: terminate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 799: terminate Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 800: terminate location-led Command Parameters**

| Parameter | Description |
| --- | --- |
| entity | Specific entity in the system for enabling its location led test. It can be a chassis or a card. By providing the keyword 'led' as a parameter, this command can terminate a LED test that was previously started by the 'activate' command. Invoking this command when no led test is active will have no effect. For the entity-id see 1830 GX Management Entity AIDs (p. 43). |

**Table 801: terminate otdr Command Parameters**

| Parameter | Description |
| --- | --- |
| entity | Specific entity (port) in the system for terminating the OTDR measurement. |

**Table 802: terminate otdr-fiber-check Command Parameters**

| Parameter | Description |
| --- | --- |
| entity | Specific OTS-R entity for terminating the manually triggered OTDR based Raman fiber check. |

**Table 803: terminate loopback Command Parameters**

| Parameter | Description |
| --- | --- |
| entity | Specific NMC entity for terminating the WSS loopback. |

#### Examples

The following command shows how to terminate the ongoing location led test on chassis-1:

```
terminate location-led chassis-1
```

The following command shows how to terminate the ongoing location led test on card-1-1:

```
terminate location-led card-1-1
```

The following command shows how to terminate the ongoing OTDR measurement on port 1-1.3-7:

```
terminate otdr 1-1.3-7
```

The following command shows how to terminate all the running cableID verification tests:

<!-- page 1277 -->

```
terminate cable-id
```

The following command shows how to terminate an ongoing OTDR fiber check on ots-r-1-1-dwdm-line:

```
terminate otdr-fiber-check ots-r-1-1-dwdm-line
```

The following command shows how to terminate WSS loopback on NMC:

```
terminate loopback nmc-RD66-1-8-ad1-191337500
```

<!-- page 1278 -->
