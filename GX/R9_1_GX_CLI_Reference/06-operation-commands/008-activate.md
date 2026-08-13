---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.8. activate'
source_lines: 4398-4644
---

## 6.8. activate

#### Command Description

The `activate` command is used to: activate a software image, activate a database, activate location LED test, activate a new FW image in a given resource, or activate/install a Key Replacement Package (KRP). **Software Image Activation** Providing `swimage` as a parameter, the command performs an activation of the currently installable software image. When using the `activate swimage` command, take into consideration the following information:

- A successful swimage activation implies a system restart.
- The optional parameter `db-action` can specify the action to apply to the database upon swimage activation. The database may be upgraded or may be erased or may be rolled back. If not provided, the system will decide automatically.
- Unless the -f option is provided, the user must confirm the activation when prompted.
- If a `db-passphrase` is not specified, then the `db-passphrase` specified in security policies is used for uploading the snapshot. However, if a `db-passphrase` is specified in the command, then, it must be the same `db-passphrase` used for taking the snapshot and the same should be used for downloading and activating this specific snapshot. Refer to `set default db-passphrase`.

**Database Activation** The `database` parameter is used for activating the currently inactive database. When using the `activate database` command, take into consideration the following information:

- A successful database activation implies a system restart.
- The optional parameter `restart-type` is used to select the type of system restart, which can be *cold* or *warm*. A cold restart may be needed if the target database deletes objects from the current one.
- Unless the -f option is provided, the user must confirm the activation when prompted.
- **Database Backup** involves the creation of one or more snapshots and exporting them to external server or storing the data on local network element. **Database activation** involves restoring the database snapshot copied from external server or the snapshot available on the local network element. Up to three database versions (including the current one) can be stored on the network element at a time.
<!-- page 147 -->
- Database back up and restore are always encrypted. The encryption is based on a database passphrase `db-passphrase`. The `db-passphrase` needs to be configured for any database operations to be successful. Until the `db-passphrase` is configured, automatic database snapshots are not taken. The `db-passphrase` does not have a default value and needs to be explicitly specified.
- The following parameters are checked before activating the database snapshot: **▪**Network element name **▪**Shelf Serial Number **▪**Software Version
- The database snapshot can be activated only when it is for correct network element and it is compatible with the existing software on the network element. This can be overridden by setting the `sanity-check-override` attribute to `true`.

**Location LED test Activation** By providing the keyword `location-led`, this command can start a location led test. The target for the location led test can be either a chassis, or a particular card. The location led test can be immediately disabled with the `terminate` command. The `location-led` command supports the LED location and lamp test functions. The LED location function is used to easily identify a targeted LED entity, which could be the chassis or an FRU (i.e. SLED, system controller, Fan, IOP). The lamp test function is used to check the chassis's LEDs. When the lamp test function is enabled, all the LEDs in the chassis lights in amber color. Both LED location and lamp test functions can be terminated by using the command `terminate location-led [entity=]<value>` or by configuring a timer within the range of 0 to 120 seconds. By default, the timer is set to 0 seconds, which means the function does not have a timeout and is terminated when the command `terminate location-led [entity=]<value>` is issued.

**Note:** The `timeout` parameter does not take effect in 1830 GX G30 R5.1. To terminate the LED location and lamp test functions, disable these functions using `terminate` command. For details, refer to terminate (p. 1274).

**Tip:** Only a single location led test can be running at a time.

**Tip:** Not all cards support the Location LED test Activation functionality.

<!-- page 148 -->

**Equipment Firmware Activation** This command starts equipment firmware. The `eqpt-fw` parameter is used to activate a new FW image in a given equipment resource. Both the equipment resource and the **fw-image** path need to be provided.

**Tip:** A user can visualize the list of available **fw-images** with command `show third-party-fw`.

**krp Activation** The `krp` parameter installs a key replacement package (KRP).

**OTDR Activation** This command initiates an OTDR (Optical Time Domain Reflectometer) measurement for the target OTDR port (provided as an AID). The configuration for the measurement is provided as a standard configuration within the otdr-ptp object. **OTDR Fiber Check Activation** This command initiates a manual OTDR based fiber check. This command can be used when the automatic OTDR fiber check is disabled and a check on the fiber connection quality using OTDR tests is required. If a manual OTDR scan or automatic OTDR based Raman fiber check is in progress, this command returns an error message. If the request is accepted, up to 3 OTDR tests are executed with intermediate and final results. Once the OTDR fiber check is completed:

- ots-r-auto-otdr **external-attenuation-rx-measured** value is updated.
- ots-r-auto-otdr **total-reflectance-rx-measured** value is updated. The user judges if the total reflectance is too high to enable the pumps or not.
- No alarm will be raised if the total reflectance value fails against the pass/fail threshold.
- ots-r-auto-otdr **auto-otdr-state** is reverted back to the state before the manually triggered OTDR based Raman fiber check.

**Loopback Activation** This command activates WSS loopback on the NMC. Power received on the tributary NMC Rx port is transmitted back out of the same tributary NMC Tx port. This can be used for local NMC connectivity verification. To activate WSS Loopback, the following pre conditions must be met:

<!-- page 149 -->
- Other active Loopback requests must not be present on the same Degree card.​
- Number of NMCs is limited to a maximum amount. In Release 9.0, Loopback can be activated only on one NMC.
- An optical cross connect must be associated with the NMC and the Optical Cross Connect activation request must be set to deactivate.​
- Administrative state of NMC must not be locked.

#### Command Syntax

```
activate -h
activate activate-file [-v] [filetype=]<value> [[db-action=]<value>] [[clear-type=]<value>] [script=]<value> [new-admin-user=]<value>
[new-admin-password=]<value> [[label=]<value>] [[db-passphrase=]<value>] [[db-instance=]<value>] [sanity-check-override]
activate [-f] swimage [label <string>] [db-action <empty-db|upgrade-db|rollback|auto>]
activate [-f] database <string> [db-instance <manual | oneday | onehour| oneweek| rollback|temp>] [db-passphrase <string>] [restart-type
<cold,warm>] [sanity-check-override=<true | false>]
activate location-led [entity=]<value> [[timeout=]<value>] [[led-mode=]<value>]
activate loopback [entity=]<value>[,<value>]*
activate eqpt-fw [resource=]<value>[,<value>]* [fw-image-name=]<value>
activate krp
activate otdr [entity=]<value> [[otdr-file-prefix=]<value>]
activate otdr-fiber-check [entity=]<value> [[otdr-file-prefix=]<value>]
```

#### Command Usage Details

**Table 74: activate Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

<!-- page 150 -->

#### Command Parameters

**Table 75: activate activate-file Command Flags**

| Parameter | Description |
| --- | --- |
| -v | Validates the command. |

**Table 76: activate command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| activate-file | Command parameter for activating a file. For activate-file specific parameters, refer to Table 77: activate activate-file Command Parameters (p. 151). |
| swimage | Command parameter for activating the currently installable software image. For swimage specific parameters, refer to Table 78: activate swimage command parameters (p. 153). |
| database | Command parameter for activating the currently inactive database. For database specific parameters, refer to Table 79: activate database command parameters (p. 154). |
| location-led | Command parameter for starting a location LED test. For location-led specific parameters, refer to Table 80: activate location-led command parameters (p. 156). |
| eqpt-fw | Command parameter for activating a new FW image in a given resource. For eqpt-fw specific parameters, refer to Table 81: activate eqpt-fw command parameters (p. 156). |
| krp | Command parameter for installing a Key Replacement Package (KPR). |
| otdr | Command parameter for triggering an OTDR measurement. For otdr specific parameters, refer to Table 82: activate otdr command parameters (p. 157). |
| otdr-fiber-check | Command parameter for triggering an automatic OTDR measurement. For otdr specific parameters, refer to Table 83: activate otdr-fiber-check command parameters (p. 157). |
| loopback | Command parameter for activating WSS Loopback. |

<!-- page 151 -->

**Table 77: activate activate-file Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| file-type | The type of file to activate. | - | n/a |
| db-action | Specifies the expected database operation:<br>• empty-db: Activate the software image with empty database.<br>• upgrade-db: Activate the software image with upgrading the current database.<br>• rollback: Rollback to the previous active software image. | • empty-db<br>• upgrade-db<br>• rollback | upgrade-db |
| 2 clear-type | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory defaults.<br>• keep-networking: Full wipe of DB contents is to be performed, but network configurations are to be kept. In this case, new-admin-user and new-admin-password must be provided for the system to auto-create the new admin user after clearing the database.<br>• initialize-from-script: Full wipe of DB contents is to be performed, but the database is to be initialized from the pre-defined script. The script must be pre-stored in the system. Additionally, new-admin-user and new-admin-password must be provided for the system to auto-create the new admin user. i Note: The keep-networking and initialize-from-script options require the new-admin-user and new-admin-password parameters to be provided and they can only be triggered by the SA user. | • full<br>• keep-networking<br>• initialize-from-script | full |
| script | The script to execute after clearing the database. The script parameter may be an absolute path for a .cli file, or just the filename if the script is present in | string Examples: | n/a |

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 2 Only valid for an unattended operation with db-action set to empty-db.
| new-admin-user | The user-name that is auto-configured after the database is wiped. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | String (0..64 characters) | n/a |
| new-admin-password | The password for the new-admin-user that is auto-configured after the database is wiped. The password can be provided as a password hash ( format: $&lt;id&gt;$&lt;salt&gt;$&lt;hash&gt;; only id 6 (SHA512) is supported; salt size is between 2 and 16 chars), or as plain text. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | string pattern: "$6$[A-Za-z0-9./]{2,16}$[A-Za-z0-9./]+" | n/a |
| label | User defined label. | string | n/a |
| db-passphrase | Passphrase used for encrypting and decrypting the file. The following rules must be used: • A minimum of 40 characters.<br>• Passphrase must not contain any dictionary words.<br>• Special characters are allowed in accordance with the 85-character set known as Z85. | string | n/a |
| db-instance | The database backup instance. | • manual<br>• oneday<br>• onehour<br>• oneweek<br>• rollback<br>• temp | n/a |
| sanity-check-override | Specifies if the sanity check needs to be skipped. | • false<br>• true | n/a |

**Table 78: activate swimage command parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| -f | Forces the command without confirmation. | - | n/a |
| -v | Force validate check again, only applicable for activation of swimage. | - | n/a |
| swimage | The currently installable software image. | string | n/a |
| db-action | Specify the expected database operation during activating software image. | • auto - activates the software image by processing the database with the system default behavior.<br>• empty-db - activates the software image with an empty database.<br>• rollback - rollback to the previous active software image.<br>• upgrade-db - activates the software image by upgrading the current database. | auto |
| label | User defined label. In the activate command label is used to specify the sw image/manifest file name. If not specified the currently Installable image is used. | string | n/a |

**Table 79: activate database command parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| -h | Displays the help for this command. | - | - |
| -f | Forces the command without confirmation. | - | - |
| database | The database to activate. | string | n/a |
| db-instance | Optional parameter. It defines the database instance name to be activated. | • active<br>• manual<br>• oneday • onehour<br>• oneweek<br>• rollback<br>• temp | n/a |
| db-passphrase | Optional parameter. The passphrase used for encrypting and decrypting DB snapshots. The following rules must be used:<br>• A minimum of 40 characters.<br>• Passphrase must not contain any dictionary words.<br>• Special characters are allowed in accordance with the 85-character set known as Z85. For each command associated with DB snapshots (backup, restore, etc), this db-passphrase will be used, except when it is directly provided in each command. Automatic DB snapshots will not be enabled until this parameter is set. | string (length 40..200). | n/a |
| restart-type | Specifies the type of system restart. | • cold<br>• warm | cold |
| sanity-check-override | Optional parameter. It is used to bypass the sanity check if set to 'true'. | • false<br>• true | false |

**Table 80: activate location-led command parameters**

| Parameter | Description | Value | Default |
| --- | --- | --- | --- |
| entity | Specific entity in the system for enabling its location led test. It can be a chassis or a card. | • &lt;chassis&gt;<br>• &lt;card&gt; | n/a |
| led-mode | Indicates the LED mode behavior:<br>• flash (default value) - for an amber light flashing/blinking at the frequency of 1Hz (usually used for LED location)<br>• solid - for a solid/steady amber light (usually used for lamp test) | • flash<br>• solid | flash |
| timeout | Specify a timer to terminate the LED location or the lamp test function, in seconds. By default, the timeout value is 0 seconds, which means there is no timer to terminate the LED location or the lamp test function. | Number (range: 0 .. 120 seconds) | 0 |

**Table 81: activate eqpt-fw command parameters**

| Parameter | Description | Value | Default |
| --- | --- | --- | --- |
| fw-image-name | The firmware file name. | string | n/a |
| resource | List of equipment to be activated that allows activating a 3rd party firmware. | • &lt;chassis&gt;<br>• &lt;card&gt;<br>• &lt;tom&gt; | n/a |

**Table 82: activate otdr command parameters**

| Parameter | Description | Value | Default |
| --- | --- | --- | --- |
| entity | Specific entity in the system for activating an OTDR measurement. | &lt;port&gt; | n/a |
| otdr-file-prefix | Specifies/indicates the optional user-defined file name prefix of the current OTDR measurement result files. | &lt;string&gt; | n/a |

**Table 83: activate otdr-fiber-check command parameters**

| Parameter | Description | Value | Default |
| --- | --- | --- | --- |
| entity | Specific entity in the system for activating an automatic OTDR measurement. | &lt;ots-r-id&gt; | n/a |
| otdr-file-prefix | Specifies/indicates the optional user-defined file name prefix of the current OTDR measurement result files. If not specified by the user, the file prefix is defined according with the following convention: otdr-file-prefix = -&lt;auto/user&gt; &lt;triggerCondition&gt; &lt;testProfi _ _ le&gt; • &lt; auto/user &gt; : string to identify if the OTDR test is user triggered or automatically triggered<br>• &lt; triggerCondition &gt; : string to identify the OTDR triggering condition, possible values: ▪ manual: if the previous value is ‘user’ ▪ fiber fault: if the OTDR test is _ automatically triggered upon fiber break of failure event detection ▪ fiber repair: if the OTDR test is _ automatically triggered upon fiber repair, i.e. clearing of APSD condition ▪ raman: if the OTDR test is automatically triggered prior to Raman ▪ activation baseline: if the trace is tagged as baseline by the system or user<br>• &lt; testProfile &gt; : string to identify the OTDR configuration profile, possible values: custom, short, medium, long, pass1/2/3. | &lt;string&gt; | n/a |

**Table 84: activate loopback command parameters**

| Parameter | Description | Value | Default |
| --- | --- | --- | --- |
| entity | Specific entity in the system for activating the loopback | &lt;nmc-ID&gt; | n/a |

<!-- page 159 -->

#### Examples

This example shows how to activate the currently inactive database:

```
activate database
```

This example shows how to activate the currently installable software image with empty DB instead of upgrading it:

```
activate swimage db-action=empty-db
```

This example shows how to install a key replacement package (KRP):

```
activate krp
```

This example shows how to activate the location led test on chassis-1:

```
activate location-led chassis-1
```

This example shows how to activate the location led test on card-1-1:

```
activate location-led card-1-1
```

This example shows how to activate the currently installable software image (force without confirmation):

```
activate -f swimage
```

This example shows how to update the tom-1-3-1 with the firmware 'xpto':

```
activate eqpt-fw tom-1-3-1 xpto
```

This example shows how to activate OTDR test on this port:

```
activate otdr 1-1.3-7
```

This example shows how to activate the automatic OTDR fiber check:

```
activate otdr-fiber-check ots-r-1-1-dwdm-line
```

<!-- page 160 -->

This example shows how to activate the WSS loopback on NMC:

```
activate loopback nmc-RD66-1-8-ad1-191337500
```

<!-- page 161 -->
