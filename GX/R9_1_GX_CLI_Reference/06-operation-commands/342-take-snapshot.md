---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.342. take-snapshot'
source_lines: 25837-25907
---

## 6.342. take-snapshot

#### Command Description

This command is used to create a local database snapshot. It stores the current state of the Configuration database into one of the available backup slots. The command generates a database snapshot and stores it locally in the NE. The system is able to hold multiple snapshots, some of them generated automatically in a periodic way, others triggered manually with this command. It is possible to provide an optional description. Existing database snapshots can be visualized with `show database` command. Later, a user can perform the additional actions associated with database:

- activate a database snapshot with 'activate database'.
- upload a database snapshot with 'upload database'.

**Note:** The system will only accept this command if the db-passphrase (used for DB snapshot encryption) is configured, either in global configuration (as part of the security-policies), or locally as a parameter of this command.

#### Command Syntax

```
take-snapshot -h
take-snapshot [[db-passphrase=]<value>] [[db-instance=]<value>] [[description=]<value>]
```

#### Command Usage Details

**Table 786: take-snapshot Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

<!-- page 1263 -->

#### Command Parameters

**Table 787: take-snapshot Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 788: take-snapshot Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| type | Location where the snapshot will be stored:<br>• db-backup - Stores the current state of the Configuration database into one of the available backup slots.<br>• system-backup - Performs a system backup into the chassis storage. | db-backup system-backup | db-backup |
| db-instance | The database instance to be captured:<br>• temp : used as temporary storage for all manual backup and restore operations (default).<br>• manual : used as permanent storage to keep important configurations. Automatic options:<br>• onehour - Automatically created db instance taken every hour<br>• oneday - Automatically created db instance taken every day<br>• oneweek - Automatically created db instance taken every week | • onehour<br>• oneday<br>• oneweek<br>• temp<br>• manual<br>• rollback | temp |
| db-passphrase | Passphrase used for encrypting and decrypting DB snapshots. For each command associated with DB snapshots (backup, restore, etc), this db-passphrase will be used, except when it is directly provided in each command. Automatic DB snapshots will not be enabled until this parameter is set. | • A minimum of 40 characters.<br>• Passphrase must not contain any dictionary words.<br>• Special character are allowed in accordance with the 85-character set known as Z85. | n/a |
| description | Optional description for the generated snapshot. | string (length 0...128) | n/a |

#### Examples

The following example shows how to generate a 'temp' snapshot from the current database:

```
take-snapshot
```

The following example shows how to generate a 'manual' snapshot with a description:

```
take-snapshot manual description='Recover point'
```

The following example shows how to generate a 'oneweek' snapshot:

```
take-snapshot oneweek
```

**Note:** If the snapshot oneweek already exists, this command will overwrite the existing file.

**Note:** If db-passphrase is not set here, it has to be set previously using:`set security-policies db-passphrase` using the db-passphrase argument in the example `take-snapshot db-passphrase=***`

<!-- page 1265 -->
