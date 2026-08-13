---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.62. convert'
source_lines: 7872-7972
---

## 6.62. convert

#### Command Description

This command is used to convert a CLI command into a request for another northbound protocol. This command is able to convert a normal CLI command into a NETCONF or RESTCONF command. It is mainly intended as an auxiliary tool to generate complex commands using other protocols. It also has the ability to generate Python code that performs the CLI command either via NETCONF or RESTCONF. The output of this command will depend on the \<target-representation\> parameter:

- netconf-xml : generates an entire NETCONF xml payload.
- netconf-python : generates Python code that performs a NETCONF request from the CLI command.
- restconf-json : generates a RESTCONF command (HTTP Method, URI, headers and body) with JSON payloads.
- restconf-xml : same as 'restconf-json', but uses XML payloads.
- restconf-python : generates Python code that performs a RESTCONF request from the CLI command.
- plaintext-to-encrypted: converts a plain text into an encrypted string; the NE will return encrypted value based on security-policies/csp-passphrase.
- snmp-oids: returns the OIDs corresponding to the objects requested in a 'show' CLI command.
- netsnmp-cmd: returns a shell command that performs a SNMP request from the CLI command.

Multiple CLI commands can be provided if separated with ';'. The payloads for each command will be merged if possible (for example, two CLI 'set' commands will be merged into a single edit-config NETCONF rpc). When it is not possible to merge the commands, multiple payloads will be generated (for example, a CLI 'set' and a 'show' command will generate two separate payloads). The generated Python code is compatible with both Python 2 or 3. It requires the following Python libraries:

- for NETCONF: ncclient (https://github.com/ncclient/ncclient)
- for RESTCONF: requests (http://python-requests.org)

The generated code will have the host, IP and user-name auto filled in, only the password needs to be manually entered.

<!-- page 347 -->

#### Command Syntax

```
convert -h
convert [target-representation=]<value> [command=]<value> [plaintext=]<value>[encoded-text=]<value>
```

#### Command Usage Details

**Table 202: convert Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 203: convert Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

**Table 204: convert Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| target-representation | Protocol to convert the command to. | • netconf-xml<br>• netconf-python<br>• restconf-json<br>• restconf-xml<br>• restconf-python<br>• plaintext-to-encrypted<br>• snmp-oids<br>• netsnmp-cmd | n/a |
| command | CLI command; should be enclosed in quotes; if multiple commands are to be converted, they should be separated by semi-colon (;) | String | n/a |

#### Examples

This example shows how to convert the ping request to a restconf python script:

```
convert restconf-python 'ping 1.23.151.23'
```

This example shows how to convert two CLI commands into one NETCONF request in 1830 GX G30 environment:

```
convert netconf-xml 'set card-1-4 admin-state lock ; set chassis-1 admin-state maintenace'
```

This example shows how to convert the create-card-services command to a RESTCONF request in Python:

```
convert restconf-python "add file-server-139 server-address '10.220.227.139' protocol 'scp' user-name 'root' password 'infinera' initial-path
 '/root'; add snmp-community-mycommunity community-string public community-string-access read-only user-group NA,SA enabled true"
```

This example shows how to convert two CLI commands into one NETCONF request in 1830 GX G40 environment:

```
convert netconf-xml 'set port-1-4-T1 admin-state maintenance ; set optical-carrier-1-4-L1-1 tx-power -4'
```

This example shows how to encrypt a value:

```
convert plaintext-to-encrypted 'my-password'
```

Output will be the encrypted value:

```
l8gKFUUbyD28L6NMHfOqO35mLLyvV2LTPdQH0hz+EBkAqhgPwL55
```

This example shows how to convert a restconf-json command:

```
convert restconf-json command="add ospf-area-range-1/9.9.9.9/10.220.0.0/16"
```

<!-- page 349 -->
