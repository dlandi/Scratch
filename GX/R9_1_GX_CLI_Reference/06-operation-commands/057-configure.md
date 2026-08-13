---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.57. configure'
source_lines: 7583-7635
---

## 6.57. configure

#### Command Description

This command is used to change to Candidate Configuration mode in order to edit a candidate datastore. This command changes from operational mode to one of the supported config modes.

- exclusive mode - only this session can make changes to the candidate configuration
- shared mode - multiples sessions can be make changes to the candidate configuration

This command also supports the parameters to configure the initialization of the Candidate Datastore starting point for the following:

- the Candidate Datastore starts from a copy of the Running Datastore.
- using `from-default`- A blank config, implies configuration from scratch.
- using `from-script=<script>`- Imported configuration from a text file in the form of CLI commands.
- using `from-commit=<commit-id>`- Imported configuration from a previous commit-record.

#### Command Syntax

```
configure <target> ([from-default] | [[from-script=]<value>] | [[from-commit=]<value>])
```

#### Command Usage Details

**Table 191: configure Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

<!-- page 336 -->

#### Command Parameters

**Table 192: configure Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| target | • exclusive - only this session can make changes to the candidate configuration<br>• shared - multiples sessions can be make changes to the candidate configuration | exclusive, shared | exclusive |
| from-default | This parameter allows user to start the Candidate configuration from an empty slate, effectively removing all non-default configurations present in the system from the Candidate Datastore. | - | - |
| from-script=&lt;script&gt; | This parameter allows users to use a CLI configuration script as source for the Candidate Configuration, effectively replacing the Running Configuration with whatever the script contains. | - | - |
| from-commit=&lt;commit-id&gt; | This parameter allows to leverage the to initialize the candidate from the configuration associated with a past commit. This option is only available when the Commit Repository feature is available (meaning, commit-tracking system-policy is enabled). | - | - |

#### Examples

This example shows how to enter Candidate Configuration mode in exclusive mode:

```
configure exclusive
```

<!-- page 337 -->
