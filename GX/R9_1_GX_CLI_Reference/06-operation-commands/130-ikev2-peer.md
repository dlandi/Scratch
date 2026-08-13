---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.130. ikev2-peer'
source_lines: 12125-12236
---

## 6.130. ikev2-peer

#### Command Description

These commands are used to add, edit or show an ikev2 peers associated with this local IKE instance. The delete command is used to delete an ikev2 peer.

#### Command Syntax

```
add ikev2-peer-<ikev2-local-instance-name>/<ikev2-peer-name> destination <value> ppk-id <value> ppk-key <value> pre-shared-key-type
<value> psk-ascii <value> psk-hex <value> psk-hash <value> [dpd-delay <value>] [keying-tries <value>] [label <value>] [admin-state <value>]
[local-identity-type <value>] [local-identity <value>] [peer-identity-type <value>] [peer-identity <value>] [authentication-scheme <value>]
[re-key-frequency <value>] [re-auth-frequency <value>] [alarm-report-control <value>] [post-quantum-preshared-key-scheme <value>] [ppk-required
<value>] [sms-operation <value>] [interface <value>] [psk-lifetime <value>] [psk-expiration-warning <value>] [psk-lifetime-enable <value>]
[local-certificate <value>] [peer-certificate <value>] [re-key-fail-policy <value>] [re-key-traffic-kill-offset <value>] [re-auth-fail-policy
<value>] [re-auth-traffic-kill-offset <value>]
set ikev2-peer-<ikev2-local-instance-name>/<ikev2-peer-name> [destination <value>] [dpd-delay <value>] [keying-tries <value>] [label
<value>] [admin-state <value>] [local-identity-type <value>] [local-identity <value>] [peer-identity-type <value>] [peer-identity
<value>] [authentication-scheme <value>] [re-key-frequency <value>] [re-auth-frequency <value>] [alarm-report-control <value>]
[post-quantum-preshared-key-scheme <value>] [ppk-required <value>] [ppk-id <value>] [ppk-key <value>] [sms-operation <value>]
[pre-shared-key-type <value>] [psk-ascii <value>] [psk-hex <value>] [psk-hash <value>] [interface <value>] [psk-lifetime <value>]
[psk-expiration-warning <value>] [psk-lifetime-enable <value>] [local-certificate <value>] [peer-certificate <value>] [re-key-fail-policy
<value>] [re-key-traffic-kill-offset <value>] [re-auth-fail-policy <value>] [re-auth-traffic-kill-offset <value>]
show ikev2-peer-<ikev2-local-instance-name>/<ikev2-peer-name> [destination] [port] [dpd-delay] [keying-tries] [AID] [label] [oper-state]
[admin-state] [local-identity-type] [local-identity] [peer-identity-type] [peer-identity] [authentication-scheme] [re-key-frequency]
[re-auth-frequency] [alarm-report-control] [post-quantum-preshared-key-scheme] [ppk-required] [ppk-id] [ppk-key] [sms-state] [sms-operation]
[pre-shared-key-type] [psk-ascii] [psk-hex] [psk-hash] [interface] [psk-configured-timestamp] [psk-lifetime] [psk-expiration-warning]
[psk-lifetime-enable] [local-certificate] [peer-certificate] [last-used-local-certificate] [last-used-peer-certificate] [re-key-fail-policy]
[re-key-traffic-kill-offset] [re-auth-fail-policy] [re-auth-traffic-kill-offset]
delete ikev2-peer-<ikev2-local-instance-name>/<ikev2-peer-name>
```

<!-- page 539 -->

#### Command Usage Details

**Table 347: ikev2-peer Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 348: ikev2-peer Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| ikev2-local-instance-name | The name (ID) of the local IKE protocol daemon instance. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| ikev2-peer-name | A unique identifier for each IKE peer association. | string (length 1..64; pattern '([A- Za-z0-9 \-.,]*)') _ | n/a | add, set, show, delete |
| destination | The IPv4/IPv6 address or the domain name of the far-end IKE peer. | IPv4/IPv6 address/domain name | n/a | add, set, show |
| port | The IKEv2 UDP listen port. | port-number | 500 | show |
| dpd-delay | The interval to check the liveness of a peer actively. Only of relevance for scope management IPsec and name not global. | uint32 | 30 | add, set, show |
| keying-tries | The number of rekeying attempts once a peer is considered dead. Only of relevance for scope management IPsec and name not global. | • uint32, range (1..max)<br>• infinite - Indicates that the Strongswan will perform infinite keying tries. | infinite | add, set, show |
| AID | Resource Access Identifier (AID). Identifies an instance within a specific resource type. | string (length 1..64 characters) | n/a | show |
| label | User configurable label | string | n/a | add, set, show |
| admin-state | The administrative state of the managed object. | lock, maintenance, unlock | unlock | add, set, show |
| oper-state | The operational state of this object. | enabled, disabled | disabled | show |
| alarm-report-control | Controls the reporting of alarms for this particular object. | allowed, inhibited | Inhibited | add, set, show |
| interface | A reference to a supported IPv4/IPv6 interface. Only of relevance for scope management IPsec and name not global. | path "../../../../../../networking/ interface/if-name" | n/a | add, set, show |
| local-identity-type | Type of local identity | ike-identity-type | id-type | add, set, show |
| local-identity | Identity of local IKE instance. | string (length 1-255) | n/a | add, set, show |
| peer-identity-type | Type of peer identity. | ike-identity-type | id-type | add, set, show |
| peer-identity | Identity of remote IKE instance. | string (length 1-255) | n/a | add, set, show |
| authentication-scheme | IKEv2 authentication mechanism with the peer. | x.509-certificate, pre-shared-key | x.509-certificate | add, set, show |
| pre-shared-key-type | The type of pre-shared key scheme. | ascii, hex | ascii | add, set, show |
| psk-ascii | Plain-text ASCII value for the PSK. | string (length 8..128) | n/a | add, set, show |
| psk-hex | Binary, hexadecimal value for the PSK. | string (length 8-256) | n/a | add, set, show |
| psk-configured-timestamp | Local NE timestamp when the PSK was configured. | date-and-time | n/a | show |
| psk-lifetime | Absolute time duration in days after which the PSK will expire. | 7..180 days | 90 | add, set, show |
| psk-expiration-warning | An absolute time duration (in days) at which the network element provides a warning when the PSK is about to expire. | 1..173 days | 14 | add, set, show |
| local-certificate | The locally installed certificates that the local IKEv2 instance uses with this particular IKE peer for purposes of authentication. Customers can pick one or more certificates from the list of locally installed certificates to use during IKE authentication with this far-end IKE peer. This attribute is a 'list' that allows for multiple certificates to be added This helps in rotating the local certificate. | "../../../../../certificates/ localcertificate/ id | n/a | add, set, show |
| peer-certificate | The locally installed list of peer certificates that the instance uses to authenticate the far-end IKE peer. These certificates indicate the identity of this far-end peer. Customers can indicate multiple certificates from the list of locally installed 'peer' certificates to use during IKE authentication with this far-end IKE peer. This attribute is a 'list' that allows for multiple certificates to be added This helps in certificate rotation and revocation. | "../../../../../certificates/ peercertificate/ id | n/a | add, set, show |
| re-key-frequency | re-key frequency for the IKE security association with the far-end IKE peer. Range and default values may be context-specific. | 3600..86400 seconds | 28800 | add, set, show |
| re-key-fail-policy | If the re-key fail policy is set to KILL- TRAFFIC, this attribute indicates the amount of time the system waits before killing all encrypted data security associations that are tied to this IKE SA. | kill traffic, continue traffic | continue-traffic | add, set, show |
| re-key-traffic-kill-offset | If the re-key fail policy is set to KILL- TRAFFIC, this attribute indicates the amount of time the system waits before killing all encrypted data security associations that are tied to this IKE SA. | 0..86400 seconds | 0 | add, set, show |
| re-auth-frequency | The re-authentication frequency for the IKE security association with the far-end IKE peer. Range and default values may be context-specific. | 3600..604800 seconds | 43200 | add, set, show |
| re-auth-fail-policy | Bring down the data path encrypted service if re-authentication was unsuccessful. | ../../../scope='data-path-encryption' | n/a | add, set, show |
| re-auth-traffic-kill-offset | If the re-authentication fail policy is set to KILL-TRAFFIC, this attribute indicates the amount of time the system waits before killing all Child SAs that are associated with this IKE SA. | 0..86400 seconds | 0 | add, set, show |
| last-used-local-certificate | A reference to the specific local entity leaf certificate that was last used during the IKE authentication with the far-end peer. | "../../../../../certificates/ localcertificate/ id | n/a | add, set, show |
| last-used-peer-certificate | A reference to the specific peer leaf certificate that was last used to authenticate the far-end IKE peer. | "../../../../../certificates/ peercertificate/ id | n/a | add, set,show |
| ppk-id | Specifies the PPK ID. | String (length 1-31) | n/a | add,set,show |
| ppk-key | Specifies the PPK Key. i Note: It is recommended to set the ppk-key to 256 bits to provide 128 bits of PQC security. | Binary( length min/max is 256 bit (i.e., 32 byte)) | n/a | add,set,show |
| post-quantum-preshared-key-scheme | Specifies the Post Quantum Preshared key scheme. If this value is set to Disabled, then PPK is disabled. If this option is set to Manual, then PPK must be manually configured. If this option is set to SMS, PPK is enabled, and the authentication-scheme is always set to pre-shared-key. i Note: SMS option is not supported for IPSec IKEv2. | disabled, manual, SMS | Disabled | add, set,show |
| ppk-required | Indicates whether PPK use is mandatory or optional for the IKEv2 peer. i Note: If this parameter is set to true and the peer does not support PPK, the connection will be terminated. For Datapath Encryption, this parameter must be always set to true as single administrator manages both ends of network. For IPsec for IKEv2, this parameter must be always set to false for backward compatibility. | True, False | True | add,set,show |

**Note:** sms-operation and sms-state parameters are not supported in Release 9.1, even though it is displayed in the interface.

**Note:** Re-key-frequency and re-auth-frequency values must not be multiples of each other. Re-key-frequency and re-auth-frequency must have a difference of a few minutes to ensure a significant interval between re-authentication and re-keying.

#### Examples

These examples provide the commands to add an ikev2-peer:

```
add ikev2-peer-1-6/test peer-identity-type id-key peer-identity NE202-1-4 authentication-scheme x.509-certificate local-certificate local
 peer-certificate client
add ikev2-peer-ipsec/GX2 destination 102.20.20.2 authentication-scheme pre-shared-key psk-ascii test12345 local-identity 'GX1ALEX'
 local-identity-type fqdn peer-identity 'GX2ALEX' peer-identity-type fqdn
```

<!-- page 545 -->

The following example provide an example to modify the IKEv2 peer and configure PPK parameters:

```
set -f ikev2-peer-1-6/NEB post-quantum-preshared-key-scheme manual  ppk-id ppkid1 ppk-key
 1234567890ABCDEF1234567890ABCDEF1234567890ABCDEF1234567890ABCDE1
```

<!-- page 546 -->
