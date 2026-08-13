---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.110. file-server'
source_lines: 11156-11204
---

## 6.110. file-server

#### Command Description

These commands are used to add, edit or show user-configurable file servers (e.g. SFTP server), to be used for transfer operations (upload/download). The delete command is used to delete a file-server.

#### Command Syntax

```
add file-server-<name> server-address <value> protocol <value> [server-port <value>] [user-name <value>] [password <value>] [initial-path
<value>] [label <value>]
set file-server-<name> [server-address <value>] [server-port <value>] [protocol <value>] [user-name <value>] [password <value>] [initial-path
<value>] [label <value>]
show file-server-<name> [server-address] [server-port] [protocol] [user-name] [password] [initial-path] [label]
show file-servers
delete file-server-<name>
```

#### Command Usage Details

**Table 306: file-server Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

#### Command Parameters

**Table 307: file-server Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| name | Name of the server, as usable in download/ upload commands. | String (0...64 characters) | n/a | add, set, show, delete |
| server-address | Address of the file-server. The ipv4-address type represents an IPv4 address in dotted-quad notation. The IPv4 address may include a zone index, separated by a % sign. The zone index is used to disambiguate identical address values. For link-local addresses, the zone index will typically be the interface index number or the name of an interface. If the zone index is not present, the default zone of the device will be used. The canonical format for the zone index is the numerical format. The ipv6-address type represents an IPv6 address in full, mixed, shortened, and shortened-mixed notation. The IPv6 address may include a zone index, separated by a % sign. The zone index is used to disambiguate identical address values. For link-local addresses, the zone index will typically be the interface index number or the name of an interface. If the zone index is not present, the default zone of the device will be used. The canonical format of IPv6 addresses uses the textual representation defined in Section 4 of RFC 5952. The canonical format for the zone index is the numerical format as described in Section 11.2 of RFC 4007. The domain-name type represents a DNS domain name. The name MUST be fully qualified whenever possible. Internet domain names are only loosely specified. Section 3.5 of RFC 1034 recommends a syntax (modified in Section 2.1 of RFC 1123). The pattern above is intended to allow for current practice in domain name use, and some possible future expansion. It is designed to hold various types of domain names, including names used for A or AAAA records (host names) and other records, such as SRV records. Note that Internet host names have a stricter syntax (described in RFC 952) than the DNS recommendations in RFCs 1034 and 1123, and that systems that want to store host names in schema nodes using the domain-name type are recommended to adhere to this stricter standard to ensure interoperability. The encoding of DNS names in the DNS protocol is limited to 255 characters. Since the encoding consists of labels prefixed by a length bytes and there is a trailing NULL byte, only 253 characters can appear in the textual dotted notation. The description clause of schema nodes using the domain-name type MUST describe when and how these names are resolved to IP addresses. Note that the resolution of a domain-name value may require to query multiple DNS records (e.g., A for IPv4 and AAAA for IPv6). The order of the resolution process and which DNS record takes precedence can either be defined explicitly or may depend on the configuration of the resolver. Domain-name values use the US-ASCII encoding. Their canonical format uses lowercase US-ASCII characters. Internationalized domain names MUST be A-labels as per RFC 5890. | String (IPv4, IPv6, domain name) | n/a | add, set, show |
| server-port | Port used for file transfer; if not provided, default will be used according with selected protocol. | Number | n/a | add, set, show |
| protocol | The file transfer protocol that this server supports.<br>• sftp: Represents sftp transfer protocol.<br>• ftp: Represents ftp transfer protocol.<br>• scp: Represents scp transfer protocol.<br>• http: Represents http transfer protocol.<br>• https: Represents https transfer protocol.<br>• file: Represents local storage, including USB storage. Requires initial-path to be provided. Tip: The ftp and http are applicable when the secure-mode is set to false. | • file<br>• ftp<br>• sftp<br>• scp<br>• http<br>• https | n/a | add, set, show |
| user-name | User name credentials for the remote file server. | String (0..64 characters) | n/a | add, set, show |
| password | Password credentials for the remote file server. | String (0...128 characters) | n/a | add, set, show |
| initial-path | The directory in the file server that is used as source/destination. | String (0...256 characters) | n/a | add, set, show |
| label | User-defined label for the server. | String (0...256 characters) | n/a | add, set, show |

#### Examples

```
add file-server-139 server-address '10.220.227.139' protocol 'scp' user-name 'root' password 'Nokia' initial-path '/root'
```

<!-- page 500 -->
