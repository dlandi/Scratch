---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.86. download'
source_lines: 9492-9820
---

## 6.86. download

#### Command Description

This command is used to download a file from an external location (a file server) to the NE. The type of file and the external location must be provided. The file types to be downloaded are:

- crl : downloads a Certificate Revocation List (CRL) file in PEM format
- database : downloads a previously uploaded database.
- file : downloads any file to the file-system.
- krp : downloads a new key replacement package (KRP).
- local-certificate : imports passphrase protected local-certificate file to add to X.509v3 end-entity certificates. Either an X.509 certificate in PKCS#12 format (with password-protected private key), or PKCS#7 format (if certificate is in pending-import).
- peer-certificate : imports peer-certificate file to add to X.509v3 end-entity certificates. An X.509v3 certificate in PKCS#12 format (with password-protected private key).
- script : downloads a CLI script from a remote server.
- swimage : downloads a manifest file, and then downloads all files belonging to a SW image.
- trusted-certificate : imports trusted-certificate file to add X.509v3 Root/Intermediate CA certificates. X.509v3 PKCS#7 trusted certificate, either Root or Intermediate CA.

The source is provided as a SFTP or SCP URL in the format :

- sftp://user@hostname/directorypath/filename
- scp://user@hostname/directorypath/filename

Non-secure protocols are only usable if secure-mode is disabled. When necessary, the user must insert the password as a separate input (SFTP and SCP). It is not possible to provide the password in the main command line (for security reasons). **Remote file location** The source location can be provided in one of two ways:

<!-- page 412 -->

1. Based on a previously configured file-server and an associated remote path:

  - The path is mandatory for downloads.
  - If the path is relative, it will be concatenated with the file-server initial-path.
  - If the path is absolute, the file-server initial-path is ignored.

2. Based on a URL in the format: \<protocol\>://[\<user\>@]\<address\>[:\<port\>]/\<path\>/\<filename\>:

  - Protocol may be ftp, sftp, scp, http, https, file (non-secure protocols - ftp/http - are only usable if secure-mode policy is disabled).
  - 'file' represents access to the local file system; this is particularly useful for mounted USB drives, which can still be used as a source for downloads; when using a USB drive, no address/port/user/password is required.

**Tip:** Use 'show usb' to see where a particular USB drive is mounted in.

  - Server port is optional. If not provided, the default port for that protocol is used.
  - Address may be an ipv4/ipv6/hostname **▪**For providing ipv6 addresses, use 'user@[ip:port]' (ex: scp://user@[2620:38:4::8:4000:238]/path/file). **▪**Path represents an absolute path; it is not possible to use relative paths. **▪**For sftp/ftp/scp, a username and a password need to be provided. **▪**The password can be provided inline with the command, or alternatively will be prompted to the user.

**Tip:** The system will automatically store the downloaded file in an appropriate place; there is no need for the user to provide a local location for storage.

**Generic file Download** By providing 'file' as file-type, any file can be downloaded in the file-system. The \<destination\> parameter will define where the file will be located. This parameter can be:

- omitted: means file is downloaded to the default directory with the original file-name;
- a file-name only: uses default directory with the new file-name;
<!-- page 413 -->
- a relative path: uses the default directory as starting path, plus relative path;
- an absolute path: Absolute path for the user accessible directories can be used.

It is necessary for the user to have write access to the destination path for the download to succeed.

**Tip:** Use 'show transfer' to see what is the default storage directory.

For generic file transfer, no further activity occurs after download, so the 'unattended' flag will be ignored. **Asynchronous Download** The download may be triggered as a background task with the -a flag (asynchronous). If so, the user can visualize the current state with the command: `show` `transfer-status filetype`.

**Note:** The transfer status MO is persistent if upgrading to a new build without an empty database or if installing an image using shell scripts. As so, the output of the transfer-status may contain some garbage value as the database was not cleared. To avoid this behavior use one of the following options:

  - Create a new VM to ensure the proper output of the transfer-status.
  - Upgrade the image with an empty database.
  - Run a clear database command from CLI: `clear database clear-type=full`.

**Skip secure verification for Download** The download may be triggered with -s flag. If so, HTTPS file transfers skip TLS verification and SCP/SFTP transfers skip ssh known host verification. If flag not set, verification is according to current security policy. **File activation and Unattended Download** Files, once downloaded, may be activated explicitly with the 'activate' command, or instead the download command may have the -u flag (for unattended download) to automatically activate the file after transfer. The following filetypes support being activated (-u flag will be ignored for other files):

<!-- page 414 -->
- database : activation of a database implies a reboot, where the system comes up with the restored DB
- swimage : activation of a swimage implies that the upgrade will be prepared and executed, implying a system reboot;

**Tip:** If an unattended activation is triggered, the additional parameters provided in the activate will not be available, and defaults will be used.

**Table 255: Additional Parameters per filetype parameter**

| Parameter | File-type(s) | Notes |
| --- | --- | --- |
| passphrase | trusted-certificate, local-certificate | Needs to be provided inline or interactively. |
| certificate-name | trusted-certificate, local-certificate, peer-certificate | Name with which this certificate will be known in the system. |
| intermediate-import (-i) | trusted-certificate, local-certificate, peer-certificate | Accepts a bundle of certificates in PEM format. Any intermediate certs present will be imported. NOTE: Root certificates need to be installed individually If no certificate-name is given, certificate name will be derived from top-most certificate issuer /CN, plus a numeric suffix. |
| sanity-check-override | database | Boolean flag that allows to skip sanity check for databases. |
| db-passphrase | database | Name of the database passphrase |
| db-action | swimage | Specify the expected database operation during activating software image. |

#### Command Syntax

```
download [-f] [-i] [-u] [-a] [-s] [filetype=]<value> ([source=]<value> | [file-server=]<value> [path=]<value>) [[passphrase=]<value>]
[certificate-name=]<value> [sanity-check-override] [[destination=]<value>] [password=]<value> [[db-passphrase=]<value>] [[db-action=]<value>]
[[clear-type=]<value>] [script=]<value> [new-admin-user=]<value> [new-admin-password=]<value>
```

<!-- page 415 -->

#### Command Usage Details

**Table 256: download Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 257: download Command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -f | Force command without confirmation. |
| -u | Auto prepare and auto activate file after a successful download. Only some files support 'activation'; others just ignore this flag. |
| -a | Download asynchronously. Tip: In synchronous file transfer mode, a textual progress bar is rendered live in the CLI during the transfer. In asynchronous file transfer mode, the download or upload operation is performed in the background; to check the progress, use the show transfer-status command. |
| -s | For HTTPS transfers, it skips TLS verification. For SCP/SFTP transfers, it skips ssh known host checking. If the flag is not set, the verification is done according with current security-policy. |
| -i | Import any intermediate certificates present in a certificate file bundle, in addition to the leaf certificate. For files with a single certificate, this enables importing any intermediate certificates specified through the AIA extension. i Note: Root certificates need to be installed individually. If no certificate-name is given, the certificate name will be derived from the topmost certificate issuer /CN, plus a numeric suffix. |

**Table 258: download Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| filetype | Predefined file type available for download. | • crl<br>• database<br>• file<br>• krp<br>• local-certificate<br>• peer-certificate<br>• script<br>• swimage<br>• trusted-certificate | n/a |
| source | Source of the download as a URL ([sftp\|scp\|http\|https\|ftp]://[user@]hostna me/directorypath/filename). | string (length 1..1024; pattern '((ftp\|sftp\|scp\|http\|https\|file):/)?/[^\s/$.?#].[^ \s]*') | n/a |
| destination | Allows user to provide the destination for the downloaded file, including directory and/ or filename. This is only applicable when file-type is 'file', representing a generic file transfer. The parameter can be: • omitted: means file is downloaded to the default directory with the original file-name<br>• a file-name only: uses default directory with the new file-name<br>• a relative path: uses the default directory as starting path, plus relative path<br>• an absolute path: Absolute path for the user accessible directories can be used It is necessary for the user to have write access to the destination path for the download to succeed. Tip: Use 'show transfer' to see what is the default storage directory. For generic file transfer, no further activity occurs after download, so the 'unattended' flag will be ignored."; | string | n/a |
| password | The password for the source. | string (length 1..255) | n/a |
| file-server | Name of a pre-configured file-server to be used for this download. | string | n/a |
| path | In case a file-server was provided, selects the (directory and filename) of the remote file. | path | n/a |
| db-action | Specify the expected database operation during activating software image, valid for unattended upgrade only. The following options are available: • empty-db: Activate the software image with empty database.<br>• upgrade-db: Activate the software image with upgrading the current database.<br>• rollback: Rollback to the previous active software image. | empty-db, upgrade-db, rollback | upgrade-db |
| sanity-check-override | If true, skips the sanity check override when downloading a database snapshot. | true, false | false |
| certificate-name | A custom type for certificate name. X.509v3 local/trusted/peer certificate id. | string (length 1..128; pattern '([A-Za-z0-9 .,/ _ @][A-Za-z0-9 \-.,/@]*)') _ | n/a |
| passphrase | To decode encrypted input files. Applicable for filetypes 'local-certificate' or 'peer-certificate'. | string (length 1..1024) | n/a |
| db-passphrase | Passphrase used for encrypting and decrypting DB snapshots. | string (length 40..200; pattern '[0-9a-zA-Z.\-: +=^!/*?&&lt;&gt;()\[\]{}@%$#]*')) | n/a |
| 3 clear-type | The type of clear action to be performed on the database.<br>• full: Full wipe of DB contents is to be performed; the database is to be reset to factory defaults.<br>• keep-networking: Full wipe of DB contents is to be performed, but network configurations are to be kept. In this case, new-admin-user and new-admin-password must be provided for the | • full<br>• keep-networking<br>• initialize-from-script | full |

\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_\_ 3 Only valid for an unattended operation with db-action set to empty-db.
| script | The script to execute after clearing the database. The script parameter may be an absolute path for a .cli file, or just the filename if the script is present in the default script directory (/storage/scripts). The script must always include the .cli extension and reference a CLI script (.cli). This script needs to match the criteria already covered by the run script command. The file needs to exist, and needs to be readable by users. i Note: This parameter is mandatory for users to clear database with clear-type set to initialize-from-script. | string Examples:<br>• /tmp/my script.cli _<br>• my script.cli _ | n/a |
| new-admin-user | The user-name that is auto-configured after the database is wiped. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | String (0..64 characters) | n/a |
| new-admin-password | The password for the new-admin-user that is auto-configured after the database is wiped. The password can be provided as a password hash ( format: $&lt;id&gt;$&lt;salt&gt; $&lt;hash&gt;; only id 6 (SHA512) is supported; salt size is between 2 and 16 chars), or as plain text. i Note: This parameter is mandatory for users to clear database with clear-type set to keep-networking or initialize-from-script. | string pattern: "$6$[A-Za-z0-9./]{2,16}$[A-Za-z0-9./]+" | n/a |

<!-- page 421 -->

#### Examples

**Tip:** In synchronous file transfer mode, a textual progress bar is rendered live in the CLI during the transfer. In asynchronous file transfer mode, the download or upload operation is performed in the background; to check the progress, use the `show transfer-status` command.

This example shows the commandhow to download a local certificate (leaf certificate) where Passphrase is the password to decript myclient.pfx:

```
download filetype=local-certificate
source=scp://testuser@10.220.225.176:/home/testuser/anantheshv/certs/myclient.pfx certificate-name=client
passphrase=infinera2 password=infinera
10%]
20%]
30%]
40%]
50%]
60%]
70%]
80%]
90%]
100%]
Successdownload filetype=local-certificate source=http://192.168.0.1:8088/myclient.pfx passphrase=*** certificate-name=myLeafCert2
/myclient.pfx
```

System output indicating the total bytes to transfer and the file transfer progress:

```
Total Bytes to Transfer = 3293
[======================================================================================100%]
Success
```

This example shows how to download a scpgeneric file:\<codeblock id="codeblock\_p2z\_mny\_3dc" class="+ topic/pre pr-d/codeblock "\>download filetype=file source=scp://forest@172.22.19.10:/home/lokesh/RSA\_2048.pem password=trees \</codeblock\>

```
download filetype=file source=http://192.168.0.1:8088/bigfile5g.bin
```

System output indicating the total bytes to transfer and the file transfer progress:

<!-- page 422 -->

```
/bigfile5g.bin
Total Bytes to Transfer = 5368709120
[==================================.......................................................................37%]
[=====================================================================================....................77%]
[=========================================================================================================100%]
Success
```

This example shows how to download new SW manifest:

```
download swimage sftp://joe@server/imagedir/newSW.manifest
```

This example shows how to use a preconfigured file-server with an absolute path:

```
download swimage serverX /imagedir/newSW.manifest
```

This example shows how to use a preconfigured file-server with a relative path:

```
download swimage serverX newSW.manifest.
```

This example shows how to download database skipping the sanity check:

```
download database sftp://tom@1.2.3.4/backup.db sanity-check-override=true
```

This example shows how to import passphrase protected local-certificate file:

```
download local-certificate sftp://tom@1.2.3.4/server_id.p12 certificate-name=local-cert-1
```

This example shows how to import passphrase protected peer-certificate file:

```
download peer-certificate sftp://tom@1.2.3.4/peer.p12 certificate-name=peer-cert1
```

This example shows how to import passphrase protected peer-certificate file, while implicitly trusting the certificate (white-listing it).

```
download peer-certificate sftp://tom@1.2.3.4/peer.p12 certificate-name=peer-cert1 -w
```

This example shows how to import trusted-certificate file:

```
download trusted-certificate sftp://tom@1.2.3.4/cacert.p7 certificate-name=trusted-cert-1
```

This example shows how to use unattended download from a USB drive:

```
download -u swimage file://media/5-U1/newSW.manifest
```

This example shows how to use asynchronous download with a file-server:

<!-- page 423 -->

```
download -a swimage serverX /imagedir/newSW.manifest
```

This example shows how to downloads a generic file (could be any filetype):

```
download file sftp://joe@server/imagedir/newSW.manifest /storage/
```

This example shows how to downloads a CRL file:

```
download crl scp://test@1.2.3.4:/download/crl/rvk_ca.crl password=fileserver1234
```

This example shows how to download a database:

```
download database sftp://gxtrain@10.120.19.21/home/gxtrain/D Bbackup3-GX42-md-scripty-csim8-14-05-20 21-12.52.17.tar.gz password=infinera
 db-passphrase=<40 to 200 characters>
```

The db-passphrase associated with the database being downloaded is mandatory to download the database. Otherwise it will fail and downloading a database stores the database on the node as a db-instance=temp type database. If a temp database already exists, it will be overwritten. This example shows how to download a manifest using http:

```
download swimage http://bar@server/imagedir/newSW.manifest
```

To download with http or ftp, secure mode must be set to false with "set security-policies secure-mode false". To download with http or https, a dns server must be set with "add -m dns-server-10.10.10.10". This example shows how to download a manifest using scp with password inline:

```
download swimage scp://bar@server/imagedir/newSW.manifest password=infinera
```

This example shows how to download a manifest using scp unattended:

```
download -u swimage scp://bar@server/imagedir/newSW.manifest
```

Downloading unattended automatically initiates prepare for upgrade validation and prepare for upgrade apply. The software is then upgraded automatically as well as the database. This example shows how to download a manifest using asynchronously:

```
download -a swimage scp://bar@server/imagedir/newSW.manifest
```

This example shows how to download a manifest using ftp:

<!-- page 424 -->

```
download swimage ftp://bar@server/imagedir/newSW.manifest
```

To download with http or ftp, secure mode must be set to false with "set security-policies secure-mode false". This example shows the commandhow to adddownload a trusted certificate asynchronously:

```
download filetype=trusted-certificate
source=scp://testuser@10.220.225.176:/home/testuser/anantheshv/certs/myCA.p7b certificate-name=root
password=infinera
10%]
20%]
30%]
40%]
50%]
60%]
70%]
80%]
90%]
100%]
Success  download -a filetype=trusted-certificate source=http://192.168.0.1:8088/rootCA.pem certificate-name=myRootCA
```

This example show the commandhow to download a certificate for secure applications:

```
download filetype=local-certificate source=scp:// testuser@10.220.225.176:/home/testuser/anantheshv/certs/myclient.pfx certificate-name=client
 PASSPHRASE: PASSWORD: myclient.pfx z
```

<!-- page 425 -->
