---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.55. commit'
source_lines: 7493-7557
---

## 6.55. commit

#### Command Description

This command is used to commit the contents of the candidate datastore. It provides an additional option when doing the commit, by explicitly providing a timeout parameter. If this mode is used, the candidate configuration is committed as usual, but if a confirmation doesn't occur until the timeout elapses, the configuration is rolled-back to the pre-commit version. So the confirmation acts as a 'persist' trigger. Additionally, user can select a Persistent Confirmed Commit that requires the commands to have an `id`parameter: All these commands need to be done while in the **Candidate Configuration mode**, entered with the `configure` command.

#### Command Syntax

```
commit
commit -m
commit confirmed [confirm-timeout=<timeout>] [-id=<id>]
commit persist
commit cancel
```

#### Command Usage Details

**Table 186: commit Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Candidate Configuration mode |

<!-- page 333 -->

#### Command Parameters

**Table 187: commit Command Flags**

| Parameter | Description |
| --- | --- |
| -m | When used with command commit -m=&lt;message&gt;which allows the user to provide a custom message to associate with the commit. |

**Table 188: commit Command Parameters**

| Parameter | Description |
| --- | --- |
| confirmed | Command parameter for initiating a confirmed commit. |
| persist | Command parameter for confirming the commit. |
| cancel | Command parameter for canceling the commit. |

**Table 189: commit confirmed Command Parameters**

| Parameter | Description |
| --- | --- |
| confirm-timeout | This parameter can be provided in this case in seconds, defining how long the commit will be pending before rollback. The default rollback time is 10 minutes. |
| -id | This command &lt;id&gt; defines the ID of the commit confirmed, commit persist and confirmed cancel commands. |

#### Examples

This example shows how to commit the contents of the candidate datastore:

```
commit
```

This example shows how to confirm the commit of the contents of the candidate datastore:

```
commit confirmed [-id=<id>]
```

<!-- page 334 -->
