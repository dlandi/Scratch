---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.64. csr-gen'
source_lines: 8034-8104
---

## 6.64. csr-gen

#### Command Description

This command is used to generate a Certificate Signing Request based on user provided information. The consequence of this command is the creation of a local-certificate in the 'pending-import' state, and the output of a CSR in PKCS#10 PEM format. This CSR can then be used with an external Certificate Authority to produce a full certificate, which can then be downloaded into the system to produce a complete local-certificate.";

#### Command Syntax

\<userinput class="+ topic/ph sw-d/userinput "\>csr-gen [[key-algorithm=]&lt;value&gt;] \| [[key-from-certificate=]&lt;value&gt;]) [[SAN=]&lt;value&gt;] [[signature-hash-algorithm=]&lt;value&gt;] [certificate-name=]&lt;value&gt; [[key-usage=]&lt;value&gt;[,&lt;value&gt;]\*] [metadata-from-certificate=]&lt;value&gt; [[metadata-template=]&lt;value&gt;] [metadata-from-download cnf=]&lt;value&gt; [[extended-key-usage=]&lt;value&gt;[,&lt;value&gt;]\*] [subject=]&lt;value&gt;\</userinput\>`csr-gen [certificate-name=]<value> ([[key-algorithm=]<value>] | [[key-from-certificate=]<value>])`

```
[[signature-hash-algorithm=]<value>] [[metadata-template=]<value>] [metadata-from-certificate=]<value> [metadata-from-cnf=]<value>
[[subject=]<value>] [[SAN=]<value>] [[key-usage=]<value>[,<value>]*] [[extended-key-usage=]<value>[,<value>]*]
```

#### Command Usage Details

**Table 207: csr-gen Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 208: csr-gen Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| certificate-name | The name of the certificate. | string | n/a |
| key-algorithm | Specifies the algorithm to be used for a new key pair for this CSR. | rsa4096--RSA (Rivest-Shamir-Adleman) public-key cryptosystem algorithm with key size 4096. rsa3072 --RSA (Rivest-Shamir-Adleman) public-key cryptosystem algorithm with key size 3072. rsa2048 --RSA (Rivest-Shamir-Adleman) public-key cryptosystem algorithm with key size 2048. eccp128 --ECC (Elliptic Curve Cryptography) 128-bit prime field Weierstrass curve - secp128r1 eccp256 --ECC (Elliptic Curve Cryptography) 256-bit prime field Weierstrass curve - prime256v1. eccp384 -- ECC (Elliptic Curve Cryptography) 384-bit prime field Weierstrass curve - secp384r1. eccp521 -- ECC (Elliptic Curve Cryptography) 521-bit prime field Weierstrass curve - ecp521r1 | eccp256 |
| key-from-certificate | Allows to reuse the key pair from an existing local-certificate. | String of the existing certificate name: /ioa-ne:ne/ioa-ne:system/ioa-ne:security/ioa-ne:certificates/ioa-ne:local-certificate/ioa-ne:id | n/a |
| signature-hash-algorithm | Hash algorithm to be used. Default value depends on the selected key-algorithm. | sha256 -- Secure Hash Algorithm 2, digest size 256 bits sha384--Secure Hash Algorithm 2, digest size 384 bits. sha512--Secure Hash Algorithm 2, digest size 512 bits. | • sha256 - if key-algorithm is 'rsa2048','rsa3072', 'eccp256'.<br>• sha384 - if key-algorithm is 'eccp384'.<br>• sha512 - if key-algorithm is 'rsa4096', 'eccp521'. |
| metadata-template | Selects the possible sources for the CSR metadata, including reusing it from an existing certificate, loading from an openssl cnf file, or using a generic template which defines the metadata defaults. In all cases except for 'from-openssl-cnf', it is possible to override the metadata individual parameters by providing the metadata parameters (subject, SAN, etc) explicitly. | from-existing-certificate --Metadata is provided/copied from existing a certificate. from-openssl-cnf --Metadata is provided from an openssl .cnf file. generic --Metadata intended for a large variety of applications and scenarios generic-tls-server --Metadata intended for a server secure-application generic-tls-client -- Metadata intended for a client secure-application. generic-ikev2-identity --Metadata intended for ikev2 node identity. | generic |
| metadata-from-certificate | A local-certificate id to be used as metadata source. Metadata details can be overridden separately. | String of the existing certificate name: /ioa-ne:ne/ioa-ne:system/ioa-ne:security/ioa-n/ane:certificates/ioa-ne:local-certificate/ioa-ne:id | n/a |
| metadata-from-cnf | Multi-line string input of cnf with metadata. Metadata details can be overridden separately. OpenSSL CSR request configuration for metadata-template from-openssl-cnf. Two forms are supported:<br>• Inline configuration: a multi-line string whose first character is not '/'. This embeds the full metadata .cnf content in the command or RPC with \n used to represent new lines.<br>• File path: an absolute path that starts with '/', pointing to a readable file on the local file system. | string length 0..4096 Examples: • metadata-from-cnf='[req] \n distinguished name = _ req distinguished name \n _ _ req extensions = v3 req \n prompt _ _ = no \n [req distinguished name] _ _ \n CN = InfineraViaCNF.com \n [v3 req] \n keyUsage = _ digitalSignature,keyEncipherment \n extendedKeyUsage = serverAuth,clientAuth \n subjectAltName = @alt names \n [alt names] \n IP.1 _ _ = 127.0.0.1 \n IP.2 = 172.29.202.81 \n DNS.1 = ne81.g31ne.com \n DNS.2 = ne83.g31ne.com'<br>• metadata-from-cnf=/tmp/mycsr.cnf | n/a |
| subject | The certificate subject. The common name (CN) RDN is mandatory. Each relative DN must have a prefix slash (/). | string length 0..1024 Examples:<br>• A minimal valid subject, which contains CN only: '/CN=Nokia'<br>• With all supported RDN fields: '/CN=NokiaLeaf/C=US/ST=California/ L=Sunnyvale/O=NokiaCorporation/ OU=NokiaR&D' |  |
| SAN | The certificate SAN (Subject Alternate Name) fields. SANs are specified as Type-Value comma separated list. The only valid types are 'IP' and 'DNS'. | Example: 'IP:127.0.0.1,DNS:localhost | n/a |
| key-usage | The Key Usage type(s) for the certificate. | Default is derived from the metadata-template parameter. The key-usage defines the purpose of the certificate. When the csr is newly generated, it can be configured with the following options: cRLSign - Allows public key to verify signature of revocation information. dataEncipherment - Allows public key usage to encrypt user data. decipherOnly For keyEncipherment - Allows the public key to be use for decryption only. digitalSignature Allows using public key with a digital sign mechanism. encipherOnly For keyEncipherment - Allows the public key to be use for encryption only. keyAgreement - Allows deriving of a session key from the public key. keyCertSign Allows public key to verify signature of certificates. keyEncipherment - Allows usage with a protocol that uses encryption keys from public key. nonRepudiation - Allows using public key for verifying digital signatures. When the csr is generated from existing certificate or template, it's derived from the existing object. | digitalSignature,keyAgreement. |
| extended-key-usage | The Extended Key Usage type(s) for the certificate. | Default is derived from the metadata-template parameter. When the csr is newly generated, it can be configured to the following options: OCSPSigning OCSP Signing. clientAuth TLS WWW Client Authentication. codeSigning Code Signing. emailProtection E-mail Protection (S/MIME). serverAuth TLS WWW Server Authentication. timeStamping Trusted Timestamping. When the csr is generated from existing certificate or template, it's derived from the existing object. | serverAuth,clientAuth. |

#### Examples

The following example shows minimal CSR generation evocation, for a resulting certificate 'myCertificate':

```
csr-gen myCertificate subject='/CN=Nokia'
```

The following example shows how to generate a CSR, but reusing an existing key from certificate 'cert1':

<!-- page 358 -->

```
csr-gen myCertificate key-from-certificate=cert1 subject='/CN=Nokia'
```

The following example shows how to generate a CSR, while providing the full subject:

```
csr-gen myCertificate subject='/CN=InfineraRoot/C=US/ST=California/L=Sunnyvale/O=InfineraCorporation/OU=InfineraR&D'
```

The following example shows how to generate a CSR, using the template specific for TLS client:

```
csr-gen myCertificate metadata-template=generic-tls-client subject='/CN=Nokia'
```

<!-- page 359 -->
