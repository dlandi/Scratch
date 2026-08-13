---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.83. display-cert'
source_lines: 9276-9402
---

## 6.83. display-cert

#### Command Description

This command is used to show the details of a certificate or CSR.

#### Command Syntax

```
display-cert [certificate=]<value> [display-type=] <value>
```

#### Command Usage Details

**Table 249: display-cert Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration mode |

#### Command Parameters

**Table 250: display-cert Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| certificate = &lt;value&gt; | The certificate to display. | string | n/a |
| display-type | Defines the requested type of display operation. | • all-certificate-hierarchy<br>• certificate-details<br>• certificate-hierarchy | all-certificate-hierarchy |

#### Examples

This example shows how to display the details of a certificate:

```
e.g.
temproot@GX> display-cert
Filtered attributes completion:
  certificate=
certificate completion:
  local-certificate-pace_client    local-certificate-server_cert    trusted-certificate-ca_root      trusted-certificate-pace_root
[ ne ]
temproot@GX> display-cert trusted-certificate-ca_root
Certificate:
    Data:
        Version: 3 (0x2)
        Serial Number:
            40:47:cb:92:d2:4e:89:6c:ab:40:33:69:8c:b2:2e:01:4a:e2:0d:e6
        Signature Algorithm: sha256WithRSAEncryption
        Issuer: C = IN, ST = KAR, L = BLR, O = INFN, OU = GX, CN = caCA1674507249
        Validity
            Not Before: Jan 23 20:54:17 2023 GMT
            Not After : Jan 20 20:54:17 2033 GMT
        Subject: C = IN, ST = KAR, L = BLR, O = INFN, OU = GX, CN = caCA1674507249
        Subject Public Key Info:
            Public Key Algorithm: rsaEncryption
                RSA Public-Key: (4096 bit)
                Modulus:
                    00:f2:2d:3a:ea:2f:bb:71:ec:75:0c:00:ac:ce:05:
                    6f:25:ff:58:59:38:11:7b:32:ca:4b:00:10:d0:b1:
                    74:ed:c7:07:9e:20:3e:34:0e:c3:46:f4:a4:46:e5:
                    ca:05:81:3a:ac:35:39:59:95:bf:14:88:e6:76:a4:
                    e1:cb:37:6a:2c:ea:89:5e:3b:f4:da:ec:18:db:70:
                    92:56:4c:1b:cd:e4:aa:35:93:ff:0e:b9:0e:91:c8:
                    97:e1:b0:5b:03:36:17:85:24:7d:ad:c2:f8:fe:81:
                    5f:2a:82:e7:13:fa:cf:15:95:46:b4:00:f2:3c:63:
                    29:23:e2:8c:d0:8e:6a:05:d9:80:cc:e5:f3:92:0f:
                    75:2c:e5:ca:61:74:3a:b7:ca:8a:77:a6:f4:2a:f0:
                    68:2a:eb:b4:58:fc:d6:74:93:51:bb:ad:2a:e7:0b:
                    3e:dd:71:62:3c:02:cc:cc:f8:46:5a:d4:54:37:b5:
                    ca:fb:4d:af:2f:6e:5d:36:6d:91:1a:83:f8:3b:81:
                    6d:23:4a:08:cb:73:db:7d:80:d1:19:28:08:c9:11:
                    ab:90:88:74:ac:63:fb:27:aa:be:c1:0e:f1:d2:49:
                    06:8d:1e:39:3f:f0:25:f2:cf:24:84:77:12:0c:d2:
                    79:8f:e2:3a:f3:6a:c5:94:d2:2f:3d:dd:96:a6:04:
                    b9:43:38:f5:85:37:28:e0:37:1d:fb:88:23:3c:23:
                    32:70:4a:f9:05:f6:43:c1:b3:10:5c:82:c3:b7:79:
                    ff:7f:3f:ef:0d:1d:26:db:a2:58:63:ef:ea:7b:a6:
                    42:0d:ef:de:eb:39:8c:7e:43:6f:1e:7c:65:5a:59:
                    30:fe:72:a9:23:33:90:9c:11:3f:87:f7:fe:b0:b6:
                    43:20:c8:b1:92:83:ab:e3:ee:e9:28:a0:2b:23:a7:
                    96:76:96:ff:3e:3c:06:09:01:c4:b0:cc:65:49:09:
                    f0:c8:d1:09:6c:6b:15:0e:52:6a:db:4d:08:6f:34:
                    ae:54:a0:b3:23:eb:5e:cd:a7:e5:0e:fe:e3:c2:8b:
                    fb:23:e5:72:03:f6:c8:50:67:d5:33:06:ae:a4:5c:
                    6b:ae:ef:7e:0d:cb:02:db:d6:30:1d:9d:09:62:6f:
                    46:7c:ac:9a:e7:20:5e:32:4c:27:00:24:d2:55:03:
                    28:e2:4e:23:ef:e8:54:82:22:cd:3f:cc:ff:40:5f:
                    16:cb:dc:09:70:31:02:45:a5:b1:6b:7b:59:4b:da:
                    67:87:fa:ab:2c:a0:c1:1f:e3:d9:4d:f9:43:f5:f3:
                    b3:f9:c6:76:af:6f:85:b0:f1:7f:9b:70:12:b8:b7:
                    d5:f0:6c:56:9f:a7:03:f7:22:f0:31:f8:b3:da:50:
                    21:ee:59
                Exponent: 65537 (0x10001)
        X509v3 extensions:
            X509v3 Subject Key Identifier:
                2B:6D:5C:A9:CA:F2:5F:93:8A:C7:E8:84:26:90:AB:9B:5D:21:78:5D
            X509v3 Authority Key Identifier:
                keyid:2B:6D:5C:A9:CA:F2:5F:93:8A:C7:E8:84:26:90:AB:9B:5D:21:78:5D
            X509v3 Basic Constraints: critical
                CA:TRUE
    Signature Algorithm: sha256WithRSAEncryption
         e8:6f:0a:89:d1:f3:f8:80:08:f1:a4:5d:37:f3:6d:28:29:c4:
         69:f6:50:e9:38:f8:98:30:fd:21:6d:76:ec:91:4c:9f:90:c5:
         46:ca:3a:76:63:80:84:59:8c:92:90:61:97:49:5f:fe:18:69:
         8e:3a:c8:87:e3:b0:e4:60:e9:31:17:b1:59:43:82:1c:82:ff:
         a1:84:21:9f:ff:12:88:ed:b2:dc:b8:89:75:9b:7d:b1:21:8d:
         9f:c0:05:8a:f2:c8:e4:6a:55:90:61:82:79:a0:4a:23:4c:b3:
         eb:34:60:f5:8a:ee:e2:d5:37:76:a1:75:6d:19:0f:5a:86:19:
         65:11:6d:98:29:3c:9b:c0:68:18:12:c7:ca:57:28:d5:6b:02:
         8c:f8:7e:30:3b:a9:83:b1:3b:97:da:28:96:53:e8:1a:09:9d:
         df:d3:48:d4:8a:aa:ac:44:41:fe:51:ec:37:5e:c8:3e:2c:b7:
         71:30:9a:9f:70:c9:df:85:4d:23:c5:65:d9:fd:e1:a0:9a:bf:
         6f:c0:cf:2d:54:6f:99:ad:3d:6a:e7:00:27:28:ad:9a:b4:45:
         a1:93:0f:f6:aa:56:92:a9:f1:03:1c:e1:6d:85:8c:27:90:b9:
         80:f1:f0:0d:db:4c:46:b4:b7:5b:bb:b7:70:f0:d9:43:d1:b9:
         e5:5a:cc:c8:ff:95:6a:78:27:25:9c:b9:88:e9:61:f0:3f:3d:
         ef:04:39:9c:54:53:67:53:52:4b:c9:49:2a:24:f3:1a:b5:f9:
         98:38:0d:d4:59:e5:37:91:7e:9d:9f:b3:f6:f2:1e:de:56:9d:
         9e:12:cd:78:a4:01:4c:a4:84:af:b2:18:d4:a7:13:31:58:3a:
         15:2c:5d:ef:ef:7d:5f:cb:06:8c:4e:e8:4c:f4:45:75:77:b0:
         20:a3:67:1b:6b:70:28:86:a5:b0:8c:2b:74:3f:06:80:ca:24:
         4a:12:eb:e0:c9:1f:5f:01:23:29:95:70:0a:5d:23:f0:3a:dd:
         60
```

<!-- page 407 -->
