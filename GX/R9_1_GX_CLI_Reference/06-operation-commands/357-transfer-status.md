---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.357. transfer-status'
source_lines: 26664-26737
---

## 6.357. transfer-status

#### Command Description

The `show transfer-status` displays information associated with file transfer.

#### Command Syntax

```
show transfer-status-<filetype>/<operation> [last-completion-status] [last-transfer] [last-duration] [transfer-type] [session-id]
[session-user-name] [filename][transfer-progress] [bytes-transferred] [total-bytes] [details]
```

**Table 820: transfer-status Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 821: transfer-status Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| filetype | File transfer status per filetype | string | n/a | show |
| operation | Last transfer operation:<br>• upload - last operation upload.<br>• download - last operation download. | • upload<br>• download | n/a | show |
| last-completion-status | Success indicates a successful operation. The state could be "unknown", i.e. it may occur that system gets rebooted while the file transfer is "in-progress". In this case, when the system starts up, the state will be showed as "unknown". | String (length "0..128") (for example, success, fail, in-progress, unknown) | n/a | show |
| last-transfer | Last transfer Start Timestamp. | date-and-time | n/a | show |
| last-duration | Last transfer duration. It is a time interval using the following syntax: '[xw] [xd] [xh] [xm] [xs]' where: w(eeks), d(ays), h(ours), m(inutes), s(seconds). Examples:<br>• 2w - two weeks<br>• 5d 12h - 5 days and 12 hours<br>• 1h 7m 30s - 1 hour and 7 minutes and 30 seconds | string (length 0..32) with pattern '(((1000)\|(0*\d{1,3}))w)? (((1000)\| (0\d{1,3}))d)? (((1000)\|(0\d{1,3} ))h)? (((1000)\|(0\d{1,3}))m)? (((1000)\|(0\d{1,3}))s)?' | n/a | show |
| transfer-type | Last transfer type:<br>• sync - last transfer type sync.<br>• async - last transfer type async | • sync<br>• async | n/a | show |
| session-id | Last transfer session-id. | string (length 0..100) | n/a | show |
| session-user-name | Last transfer session-user-name. | string (length 1..64) with pattern '[a-zA-Z .][a-zA-Z0-9 -.]*[$]?' _ _ | n/a | show |
| filename | Last transferred file URL. | string (length 0..1024) | n/a | show |
| transfer-progress | Transfer completion percentage. | 0-100 percent | 0 | show |
| bytes-transferred | Bytes that have been transferred so far. | integer (uint64) unit: bytes | 0 | show |
| total-bytes | Total file size in bytes. Zero until known at the transfer start. | integer (uint64) unit: bytes | 0 | show |
| details | Details of transfer phase | string (length 0..140)<br>• completed<br>• Failed<br>• idle<br>• preparation<br>• transfer | idle | show |

#### Examples

The following command shows how to show the file transfer status:

```
show transfer-status
```

The following is an example output:

```
transfer-status                               last-completion-status  last-transfer              last-duration  transfer-type  session-id
--------------------------------------------  ----------------------  -------------------------  -------------  -------------  -----------
transfer-status-local-certificate/download    Success                 2026-05-20T19:45:48+05:30  3s             sync           0.0.0.0:485
transfer-status-trusted-certificate/download  Success                 2026-05-20T19:42:31+05:30  1s             async          0.0.0.0:485
transfer-status                               session-user-name  filename                              transfer-progress (percent)
--------------------------------------------  -----------------  ------------------------------------  ---------------------------
transfer-status-local-certificate/download    sd                 http://192.168.0.1:8088/myclient.pfx  100
transfer-status-trusted-certificate/download  sd                 http://192.168.0.1:8088/rootCA.pem    100
transfer-status                               bytes-transferred (bytes)  total-bytes (bytes)  details
--------------------------------------------  -------------------------  -------------------  ---------
transfer-status-local-certificate/download    3293                       3293                 Completed
transfer-status-trusted-certificate/download  1131                       1131                 Completed
```

<!-- page 1302 -->

The following command shows how to show the file download status of the trusted certificate:

```
show transfer-status-trusted-certificate/download
```

<!-- page 1303 -->
