---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.254. protocols'
source_lines: 19585-19725
---

## 6.254. protocols

#### Command Description

This command is used to show protocol information.

#### Command Syntax

```
show protocols [ssh|cli|serial-console|netconf|grpc|snmp|restconf|http-file-server|data-model-openconfig]
```

#### Command Usage Details

**Table 600: protocols Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 601: protocols Command Parameters**

| Parameter | Description | Values |
| --- | --- | --- |
| ssh | Show ssh protocols | ssh |
| cli | Show cli protocols | cli |
| serial-console | Show serial console protocols | serial-console |
| netconf | Show netconf protocols | netconf |
| restconf | Show restconf protocols | restconf |
| grpc | Show grpc protocols | grpc |
| snmp | Show snmp protocols | snmp |
| http-file-server | Show http-file-server protocols | http-file-server |
| data-model-openconfig | Show data-model-openconfig protocols | data-model-openconfig |

#### Examples

This example shows hows to show protocols information. The following output is displayed:

```
show protocols
  protocols
  ssh
  cli
  serial-console
  netconf
  restconf
  grpc
  snmp
  http-file-server
  data-model-openconfig
[ ne ]
admin@GX> show protocols ssh
  ssh
  ssh-host-key-ecdsa-sha2-nistp521
  ssh-host-key-ssh-rsa4096
  enabled                                       false
  port                                          8022
  pre-login-message                             ''
  post-login-message                            '******************************
                                               ************ Warning ***********
                                               *******************************
                                               This system is restricted to
                                                authorized users for business
                                                purposes. Unauthorized access
                                                is a
                                               violation of the law. This
                                                service may be monitored for
                                                administrative and security
                                                reasons.
                                               By proceeding, you consent to
                                                this monitoring.
                                               ********************************
                                               ********************************
                                               *****************************
                                               '
[ ne ]
admin@GX> show protocols cli
  cli
  cli-session-config-10.220.117.112:65049
  cli-session-config-10.220.117.112:65053
  enabled                                              true
  port                                                 22
  script-dir                                           '/storage/scripts'
[ ne ]
admin@GX> show protocols serial-console
  serial-console
  global-switch               enabled
  global-timeout              60 minutes
[ ne ]
admin@GX> show protocols netconf
  netconf
  enabled                  true
  port                     830
  annotate-cli-name        false
  static-info-in-notifs
[ ne ]
admin@GX> show protocols restconf
  restconf
  enabled               true
  http-enabled          false
  https-enabled         true
  http-port             8080
  https-port            8181
  cookie-timeout        5 minutes
  api-root              '/restconf'
[ ne ]
admin@GX> show protocols grpc
  grpc
  enabled           true
  port              50051
[ ne ]
admin@GX> show protocols snmp
  snmp
  enabled           true
  port              161
  snmp-engine-id    '0x80001f8804c117ce1531cd6c61'
[ ne ]
admin@GX> show protoocols http-file-server
ERROR: unknown element 'protoocols'
[ ne ]
admin@GX> show protocols http-file-server
  http-file-server
  enabled                       true
  http-enabled                  false
  https-enabled                 true
  http-port                     8980
  https-port                    8981
  url-base                      '/transfer'
[ ne ]
admin@GX> show protocols data-model-openconfig
  data-model-openconfig
  description                        'OPENCONFIG model'
  enabled                            true
[ ne ]
admin@GX>
```

<!-- page 992 -->
