---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.290. shell'
source_lines: 21787-21877
---

## 6.290. shell

#### Command Description

**Note:** User access to the shell command can be limited to users in the NA, NE, and TT user-groups.

This command is used to launch a Linux bash shell from within the CLI. The shell will be launched using the currently logged in user, and will allow command accessible to the current user. The Linux shell can be closed the typical way (for example, with the 'exit' command), and the shell will return to the CLI prompt. The command also allows execution of a single shell command inside the CLI session. Certain commands/scripts can be launched with elevated privileges. To do so, prefix the command with 'sudo'. Possible commands executed with sudo include:

- /bin/ping
- /sbin/reboot
- /sbin/shutdown
- /bin/netstat
- /bin/df
- /usr/bin/du
- /sbin/dhclient
- /bin/bash /opt/infinera/thanos/local/bin/clear\_downloads.sh
- /bin/bash /opt/infinera/thanos/local/bin/config\_dns.sh
- /bin/bash /opt/infinera/thanos/local/bin/config\_ip.sh
- /bin/bash /opt/infinera/thanos/local/bin/download.sh
- /bin/bash /opt/infinera/thanos/local/bin/install\_os.sh
- /bin/bash /opt/infinera/thanos/local/bin/show\_dns.sh
- /bin/bash /opt/infinera/thanos/local/bin/show\_downloads.sh
- /bin/bash /opt/infinera/thanos/local/bin/show\_ip.sh
<!-- page 1090 -->
- /bin/bash /opt/infinera/thanos/local/bin/show\_route.sh
- /bin/bash /opt/infinera/thanos/local/bin/tunnelctl.sh
- /bin/bash /opt/infinera/thanos/local/bin/install\_app.sh
- /bin/bash /opt/infinera/thanos/local/bin/remove\_app.sh
- /bin/bash /opt/infinera/thanos/local/bin/CopyLogsToUSB.sh
- /bin/bash /opt/infinera/thanos/local/bin/docker.sh

**Note:** Various standard packages are included with the Debian distribution providing common Linux utilities.

#### Command Syntax

```
shell [-h] [shell command]
```

#### Command Usage Details

**Table 676: shell Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 677: shell Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Examples

This example shows how to launch the Linux Bash shell:

```
shell
```

<!-- page 1091 -->

This example shows how to launch the predefined shell editor:

```
shell editor
```

This example shows how to launch vi editor:

```
shell vi
```

This example shows how to executes ifconfig command:

```
shell ifconfig
```

The following example runs netstat with elevated privileges and pipes the output to grep to find all listen ports opened for sshd. Note the use of single quotes to allow more complex shell commands involving multiple composed commands, piping, etc.:

```
shell 'sudo netstat -alnp | grep sshd'
```

<!-- page 1092 -->
