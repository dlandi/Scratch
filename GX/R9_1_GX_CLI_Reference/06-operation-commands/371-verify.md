---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.371. verify'
source_lines: 27604-27678
---

## 6.371. verify

#### Command Description

The command verify is used to trigger CableID-based fiber connections verification. **Fiber Connection Verification (verify fiber-connection)** The `verify` command triggers the system to run CableID verification for the ports contained in the 'cable-id' entity, when used with the following syntax:

```
verify fiber-connection <port-id>| all <allow-switching>
```

where:

- **port-id** attribute is the target port of the CableID-capable sled which the verification is to be performed: **▪**If the port is involved in multiple CableID paths, all the CableID paths are included in the check. **▪**If **allow-switching** is set to *true*, the verification includes active and inactive paths (system initiated switching). **▪**If **allow-switching** is set to *false*, the verification includes active paths only (no system initiated switching).
- **allow-switching** attribute is of boolean type: **▪**If *true*, the CableID verification is allowed to initiate switching on OPSM to complete the optical path for verification. **▪**If *false*, the CableID verification is not allowed to initiate switching on the OPSM (e.g., system is in service) to complete the optical path for verification. **▪**By default the attribute is set to *false*.

The command prompts a confirmation message which must be confirmed by the user.

**Tip:** The command supports the -f flag option which force the command to run without the confirmation prompt.

**Note:** IPM uses the CLI command to trigger CableID verification.

Preconditions:

- The equipment-policies **cable-id-control** attribute is set to *enabled*.
<!-- page 1346 -->

Consequent actions:

- If the request is accepted, the system raises the alarm **CID-verification-in-progress** against the NE entity. The alarm is raised until all queued test requests are completed.

The command is non-blocking. Once the command is triggered, the system responds with the message: "Verification started. Use show cable-id-status command to obtain latest verification status". If the verification request is accepted, the system proceeds to execute the verification.

#### Command Syntax

```
verify [-f] [type-select=]<value> [[target-select=]<value>] [allow-switching]
```

#### Command Usage Details

**Table 849: verify Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 850: verify Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| Input Parameters |  |  |  |
| type-select | Type of verification. Currently only fiber-connection verification is supported. | fiber-connection | n/a |
| target-select | For fiber-connection verification, the attribute can be:<br>• &lt;port-id&gt; - port instance-identifier of the cable-id capable card to be verified.<br>• If the &lt;port-id&gt; is not specified, the verification is done on all paths. | • instance-identifier | n/a |
| allow-switching | In protection scenarios, it specifies whether the Cable-id function is allowed to initiate switching on OPSM to verify working and protection paths:<br>• true - the verification includes active and inactive paths (system initiated switching).<br>• false - the verification includes active paths only (no system initiated switching). | • false<br>• true | false |
| Output Parameters |  |  |  |
| verify-result | Result of the verification operation. | string | n/a |

#### Examples

The following command shows how to verify the fiber connection at ADE11 port of RD20TM equipped at chassis 1 slot 6:

```
verify fiber-connection port-1-6-ade11
```

The following command shows how to verify of the fiber connection at ADE3 port of RD20TM equipped at chassis 1 slot 6. Both active and inactive optical path is verified:

```
verify fiber-connection port-1-6-ade3 allow-switching=true
```

<!-- page 1348 -->
