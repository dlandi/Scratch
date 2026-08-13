---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.34. cable-id-path'
source_lines: 6118-6227
---

## 6.34. cable-id-path

#### Command Description

The commands described in this section are used to show the `cable-id-path` attributes. The CableID signal can only be sourced/terminated by CAD10A and RD20TM, the signal is transparently passed in the OPSM. The CableID path starts at *end A* port and terminates at *end Z* port, where the CableID signal is sourced/terminated (at CAD10A DWDM port or RD20TM ADE port).

#### Command Syntax

```
show cable-id-path-<name> [card-type-a] [port-a] [card-type-z] [port-z] [port-a-to-port-z-path-status] [port-z-to-port-a-path-status]
[port-a-to-port-z-last-test-status] [port-z-to-port-a-last-test-status] [current-state] [last-test-qualifier] [last-test-timestamp]
[additional-info]
show [-r] cable-id-path-<name>
```

#### Command Usage Details

**Table 138: cable-id-path Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 139: cable-id-path Command Flags**

| Parameter | Description |
| --- | --- |
| -r | When used with command show [-r] cable-id-path-&lt;name&gt;, it displays, for a single port, the results from the port-pair including all cable-id-path parameters and the supporting fiber connections. |

<!-- page 248 -->

**Table 140: cable-id-path Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | It is the cable-id result name. | string (length 0..255) | n/a | show |
| card-type-a | Displays the card type of the end A of the port pair:<br>• RD20TM<br>• CAD10A The card-type-A is a CableId capable sled. i Note: In R8.0.1, the sleds supported are CAD10A and RD20TM. The value is derived from the provisioned fiber-connection at port-A attribute. When a fiber-connection is created, the CableID function examines if the fiber connection is associated with a port on the CableID capable sled. If it is, the system captures this card type into the CableID path topology and assigns it to either cable-id-path card-type-A or cable-id-path card-type-Z. | • RD20TM<br>• CAD10A | n/a | show |
| port-a | Displays the instance identifier of the sled port for the port pair. It identifies one end of the CableId port-pair. The port-A is a port on a CableId capable sled. For R8.0.1, the sleds supported are CAD10A and RD20TM. | port AID | n/a | show |
| card-type-z | Displays the card type of the end Z of the port pair:<br>• RD20TM<br>• CAD10A The card-type-Z is a CableId capable sled. i Note: In R8.0.1, the sleds supported are CAD10A and RD20TM. The value is derived from the provisioned fiber-connection at port-Z attribute. When a fiber-connection is created, the CableID function examines if the fiber connection is associated with a port on the CableID capable sled. If it is, the system captures this card type into the CableID path topology and assigns it to either cable-id-path card-type-A or cable-id-path card-type-Z. | • RD20TM<br>• CAD10A |  | show |
| port-z | Displays the instance identifier of the sled port for the port pair. It identifies one end of the CableId port-pair. The port-Z is a port on a CableId capable sled. For R8.0.1, the sleds supported are CAD10A and RD20TM. | port AID | n/a | show |
| port-a-to-port-z-path-status | Display the protection path status for endpoints A-Z. It indicates if the port-A has optical continuity to the port-Z:<br>• enabled - all sleds supporting the cable-id path has been configured to provide optical continuity for the cable-id optical path.<br>• disabled - one or more sleds supporting the cable-id path has NOT been configured to provide optical continuity for the cable-id optical path. For direct connection between two CableID capable sled ports, the status is always 'enabled'. For port-A connection to port-Z via one or two OPSM, the status is "enabled" if all the transit OPSM(s) have the switch positions allowing completion of the CableID path. Otherwise, the status is "disabled" | • enabled<br>• disabled | disabled | show |
| port-z-to-port-a-path-status | Display the protection path status for endpoints A-Z. It indicates if the port-Z has optical continuity to the port-A:<br>• enabled - all sleds supporting the cable-id path has been configured to provide optical continuity for the cable-id optical path.<br>• disabled - one or more sleds supporting the cable-id path has NOT been configured to provide optical continuity for the cable-id optical path. For direct connection between two CableID capable sled ports, the status is always 'enabled'. For port-A connection to port-Z via one or two OPSM, the status is "enabled" if all the transit OPSM(s) have the switch positions allowing completion of the CableID path. Otherwise, the status is "disabled". | • enabled<br>• disabled | disabled | show |
| port-a-to-port-z-last-test-status | Display the cable id test results for endpoints A-Z:<br>• not-verified - cable-id verification is not initiated.<br>• pass - cable-id verification passed.<br>• fail - cable-id verification failed. | • not-verified<br>• pass<br>• fail | not-verified | show |
| port-z-to-port-a-last-test-status | Display the cable id test results for endpoints A-Z:<br>• not-verified - cable-id verification is not initiated.<br>• pass - cable-id verification passed.<br>• fail - cable-id verification failed. | • not-verified<br>• pass<br>• fail | not-verified | show |
| current-state | State of the CableID port-pair verification:<br>• idle (default value): CableID-based verification is not running.<br>• running-incl-switching: CableID-based verification is running for both active and protected paths. The verification is triggered with allow-switching set to true, which means system-initiated OPSM switching is allowed. • running-no-switching: CableID-based verification is running only for active path. The verification is triggered with allow-switching set to false, which means system-initiated OPSM switching is not allowed. i Note: The attribute does not persist over warm/ cold restart over SW upgrade. The attribute is re-initialized after any restart. | • idle<br>• running-incl-switching<br>• running-no-switching | idle | show |
| last-test-qualifier | Display last test status:<br>• up-to-date - Up to date, when cable-id test completed.<br>• out-dated - Out dated, when there is any fault on fiber. | • up-to-date<br>• out-dated | up-to-date | show |
| last-test-timestamp | Timestamp for the last cable-id verification for the port pair. | date-and-time | Null | show |
| additional-info | The additional-info field indicates any information for troubleshooting when the last-test-status is fail. When the last-test-status is fail, the additional-info string can be: • Cable-ID TxLOS: One of the sled does not send out the CableID signal. Check the CableID SFP or cable connecting the SFP to the cable-id port on the sled.<br>• Cable-ID RxLOS: One of the sled does not receive the CableID signal. Check the sled or warm restart the sled.<br>• Cable-ID-MISMATCH: The received CableID signature is not the expected. Check for misconnected fiber.<br>• Cable-ID-RxLOF: The received CableID signal has error. Check the sled or warm restart the sled.<br>• Power out-of-range: Cable-ID signal received power out-of-range. Check for mis-connected fiber or high insertion loss or the CableID SFP.<br>• timeout: the cable-ID signal is not received on the receiver side. Check if the fiber is connected on the wrong port, or if not properly inserted. | string (length 0..1024) | Null | show |
| supporting-fiber-connection | Container with the list of fiber connections (fiber-connection-list). The container is displayed when -r flag is used. | instance-identifier | n/a | show |
| fiber-connection-list | Displays a list of supporting-fiber-connections. It is displayed when -r flag is used. | instance-identifier | n/a | show |

#### Examples

The following command shows an example on how to view cable-id-path from RD20TM ADE port end (end A) to CAD10A DWDM port (end Z):

```
show cable-id-path-1-6-ade3-153-3-dwdm
```

The following output is retrieved:

```
  supporting-fiber-connection-1-6-ade3-153-3-dwdm
  cable-id-path-1-6-ade3-153-3-dwdm
  card-type-a                                                  RD20TM
  port-a                                                       port-1-6-ade3
  card-type-z                                                  CAD10A
  port-z                                                       port-153-3-dwdm
  port-a-to-port-z-path-status                                 disabled
  port-z-to-port-a-path-status                                 enabled
  port-a-to-port-z-last-test-status                            not-verified
  port-z-to-port-a-last-test-status                            pass
  current-state                                                idle
  last-test-qualifier                                          up-to-date
  last-test-timestamp                                          '2025-06-20T07:05:35Z'
  additional-info                                              '[a-z]:OPSM Switching failed due to priority pg state'
```

The following command shows an example on how to view cable-id-path from RD20TM ADE port (end A) to RD20TM ADE port (end Z) including the supported fiber-connections:

<!-- page 256 -->

```
show -r cable-id-path-1-6-ade6-1-8-ade6
```

The following output is retrieved:

```
  cable-id-path-1-6-ade6-1-8-ade6
  card-type-a                                                RD20TM
  port-a                                                     port-1-6-ade6
  card-type-z                                                RD20TM
  port-z                                                     port-1-8-ade6
  port-a-to-port-z-path-status                               enabled
  port-z-to-port-a-path-status                               enabled
  port-a-to-port-z-last-test-status                          fail
  port-z-to-port-a-last-test-status                          fail
  current-state                                              idle
  last-test-qualifier                                        up-to-date
  last-test-timestamp                                        '2025-06-24T10:01:15Z'
  additional-info                                            '[a-z]:Timeout; [z-a]:Timeout'
  supporting-fiber-connection-1-6-ade6-1-8-ade6
  fiber-connection-list                                      supporting-fiber-connection-R1
```

<!-- page 257 -->
