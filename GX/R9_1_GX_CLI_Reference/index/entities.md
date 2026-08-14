# Entity / AID index

Managed entities are addressed by an AID (Access Identifier) such as `card-1-1`, `port-1-1-DCN` or `odu-1-5-L1-1`. To find the command for an AID you have in hand, match the **longest AID prefix** below; the rest of the string is the instance key.

294 of 395 commands address a named entity; the remaining 101 are action commands (see the `kind` field in `commands.jsonl`).

Containment hierarchy (what lives under what) is documented in the source: see [1.3.6 Managed Objects (MO) Relationship](../01-introduction/01-introduction.md#136-managed-objects-mo-relationship) and the `tree` command output in [4.5. tree](../04-navigation-and-display-commands/04-navigation-and-display-commands.md#45-tree).

| AID prefix | Full pattern | Command | Domain | File |
| --- | --- | --- | --- | --- |
| `aaa-server` | `aaa-server-<server-name>` | `aaa-server` | security-access-control | [001-aaa-server.md](../06-operation-commands/001-aaa-server.md) |
| `aaa-statistics` | `aaa-statistics-<server-name>` | `aaa-statistics` | security-access-control | [002-aaa-statistics.md](../06-operation-commands/002-aaa-statistics.md) |
| `access-control-list` | `access-control-list` | `access-control-list` | security-access-control | [003-access-control-list.md](../06-operation-commands/003-access-control-list.md) |
| `access-rule` | `access-rule-<access-rule-list-name>/<access-rule-name>` | `access-rule` | security-access-control | [004-access-rule.md](../06-operation-commands/004-access-rule.md) |
| `access-rule-list` | `access-rule-list-<name>` | `access-rule-list` | security-access-control | [005-access-rule-list.md](../06-operation-commands/005-access-rule-list.md) |
| `ace` | `ace-<name>/<sequence-id>` | `ace` | security-access-control | [006-ace.md](../06-operation-commands/006-ace.md) |
| `acl` | `acl-<name>` | `acl` | security-access-control | [007-acl.md](../06-operation-commands/007-acl.md) |
| `additional-key-exchange` | `additional-key-exchange-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>/<additional-key-exchange-id>` | `additional-key-exchange` | encryption-ipsec-macsec | [011-additional-key-exchange.md](../06-operation-commands/011-additional-key-exchange.md) |
| `adg` | `adg-<adg-number>` | `adg` | optical-layer0 | [012-adg.md](../06-operation-commands/012-adg.md) |
| `alarm` | `alarm-<alarm-id>` | `alarm` | fault-alarms-logging | [014-alarm.md](../06-operation-commands/014-alarm.md) |
| `alarm-control` | `alarm-control` | `alarm-control` | fault-alarms-logging | [015-alarm-control.md](../06-operation-commands/015-alarm-control.md) |
| `alarm-inventory` | `alarm-inventory-<alarm-type>` | `alarm-inventory` | fault-alarms-logging | [016-alarm-inventory.md](../06-operation-commands/016-alarm-inventory.md) |
| `alarm-severity-entry` | `alarm-severity-entry-<resource-type>/<alarm-type>/<direction>/<location>` | `alarm-severity-entry` | fault-alarms-logging | [017-alarm-severity-entry.md](../06-operation-commands/017-alarm-severity-entry.md) |
| `alarm-severity-profile` | `alarm-severity-profile` | `alarm-severity-profile` | fault-alarms-logging | [018-alarm-severity-profile.md](../06-operation-commands/018-alarm-severity-profile.md) |
| `amplifier` | `amplifier-<name>` | `amplifier` | optical-layer0 | [019-amplifier.md](../06-operation-commands/019-amplifier.md) |
| `amplifier-raman` | `amplifier-raman-<name>` | `amplifier-raman` | optical-layer0 | [020-amplifier-raman.md](../06-operation-commands/020-amplifier-raman.md) |
| `ase-idler-service` | `ase-idler-service-<name>` | `ase-idler-service` | optical-layer0 | [024-ase-idler-service.md](../06-operation-commands/024-ase-idler-service.md) |
| `ase-idler-source` | `ase-idler-source-<name>` | `ase-idler-source` | optical-layer0 | [025-ase-idler-source.md](../06-operation-commands/025-ase-idler-source.md) |
| `auth-key` | `auth-key-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi>` | `auth-key` | security-access-control | [026-auth-key.md](../06-operation-commands/026-auth-key.md) |
| `authorization` | `authorization` | `authorization` | security-access-control | [027-authorization.md](../06-operation-commands/027-authorization.md) |
| `bgp-instance` | `bgp-instance-<instance-id>` | `bgp-instance` | ip-networking | [029-bgp-instance.md](../06-operation-commands/029-bgp-instance.md) |
| `bgp-neighbor` | `bgp-neighbor-<instance-id>/<remote-address>` | `bgp-neighbor` | ip-networking | [030-bgp-neighbor.md](../06-operation-commands/030-bgp-neighbor.md) |
| `bgp-network` | `bgp-network-<instance` | `bgp-network` | ip-networking | [031-bgp-network.md](../06-operation-commands/031-bgp-network.md) |
| `cable-id` | `cable-id` | `cable-id` | topology-discovery | [033-cable-id.md](../06-operation-commands/033-cable-id.md) |
| `cable-id-path` | `cable-id-path-<name>` | `cable-id-path` | topology-discovery | [034-cable-id-path.md](../06-operation-commands/034-cable-id-path.md) |
| `cable-id-status` | `cable-id-status` | `cable-id-status` | topology-discovery | [035-cable-id-status.md](../06-operation-commands/035-cable-id-status.md) |
| `capabilities` | `capabilities-<name>` | `capabilities` | equipment-inventory | [039-capabilities.md](../06-operation-commands/039-capabilities.md) |
| `card` | `card-<name>` | `card` | equipment-inventory | [040-card.md](../06-operation-commands/040-card.md) |
| `carrier-neighbor` | `carrier-neighbor-<local-carrier` | `carrier-neighbor` | topology-discovery | [041-carrier-neighbor.md](../06-operation-commands/041-carrier-neighbor.md) |
| `cdp` | `cdp-<name>` | `cdp` | certificates-pki | [042-cdp.md](../06-operation-commands/042-cdp.md) |
| `cert-to-name` | `cert-to-name-<id>` | `cert-to-name` | certificates-pki | [044-cert-to-name.md](../06-operation-commands/044-cert-to-name.md) |
| `chassis` | `chassis-<name>` | `chassis` | equipment-inventory | [047-chassis.md](../06-operation-commands/047-chassis.md) |
| `cid-ptp` | `cid-ptp-<name>` | `cid-ptp` | transport-layer1 | [048-cid-ptp.md](../06-operation-commands/048-cid-ptp.md) |
| `cli` | `cli` | `cli` | cli-and-session | [050-cli.md](../06-operation-commands/050-cli.md) |
| `cli-session-config` | `cli-session-config-<session-id>` | `cli-session-config` | cli-and-session | [051-cli-session-config.md](../06-operation-commands/051-cli-session-config.md) |
| `clock` | `clock` | `clock` | system-node-time | [052-clock.md](../06-operation-commands/052-clock.md) |
| `comm-channel` | `comm-channel-<name>` | `comm-channel` | ip-networking | [053-comm-channel.md](../06-operation-commands/053-comm-channel.md) |
| `comm-eth` | `comm-eth-<card-name>-<port-name>` | `comm-eth` | ip-networking | [054-comm-eth.md](../06-operation-commands/054-comm-eth.md) |
| `config` | `config` | `config` | config-datastore | [056-config.md](../06-operation-commands/056-config.md) |
| `connection-ports` | `connection-ports-<degree-number>/<index>` | `connection-ports` | topology-discovery | [059-connection-ports.md](../06-operation-commands/059-connection-ports.md) |
| `console` | `console-<name>` | `console` | equipment-inventory | [060-console.md](../06-operation-commands/060-console.md) |
| `controller-card` | `controller-card-<name>` | `controller-card` | equipment-inventory | [061-controller-card.md](../06-operation-commands/061-controller-card.md) |
| `crl` | `crl-<name>` | `crl` | certificates-pki | [063-crl.md](../06-operation-commands/063-crl.md) |
| `current-advanced-parameter` | `current-advanced-parameter-<optical-carrier-name>/<current-advanced-parameter-name>` | `current-advanced-parameter` | config-datastore | [065-current-advanced-parameter.md](../06-operation-commands/065-current-advanced-parameter.md) |
| `current-alarms` | `current-alarms` | `current-alarms` | fault-alarms-logging | [066-current-alarms.md](../06-operation-commands/066-current-alarms.md) |
| `current-fw` | `current-fw-<card-name>-<port-name>/<fw-name>` | `current-fw` | software-firmware-files | [067-current-fw.md](../06-operation-commands/067-current-fw.md) |
| `current-subscription` | `current-subscription-<subscription-name>` | `current-subscription` | management-protocols | [068-current-subscription.md](../06-operation-commands/068-current-subscription.md) |
| `custom-tlv` | `custom-tlv-<lldp-port>/<direction>/<oui>/<subtype>` | `custom-tlv` | topology-discovery | [069-custom-tlv.md](../06-operation-commands/069-custom-tlv.md) |
| `data-model` | `data-model-<name>` | `data-model` | management-protocols | [070-data-model.md](../06-operation-commands/070-data-model.md) |
| `data-path-encryption` | `data-path-encryption` | `data-path-encryption` | encryption-ipsec-macsec | [071-data-path-encryption.md](../06-operation-commands/071-data-path-encryption.md) |
| `database` | `database-<database-type>` | `database` | config-datastore | [072-database.md](../06-operation-commands/072-database.md) |
| `db-protection-scheme` | `db-protection-scheme` | `db-protection-scheme` | config-datastore | [074-db-protection-scheme.md](../06-operation-commands/074-db-protection-scheme.md) |
| `degree` | `degree-<degree-number>` | `degree` | optical-layer0 | [076-degree.md](../06-operation-commands/076-degree.md) |
| `dhcp-relay` | `dhcp-relay` | `dhcp-relay` | ip-networking | [078-dhcp-relay.md](../06-operation-commands/078-dhcp-relay.md) |
| `dial-out-server` | `dial-out-server-<name>` | `dial-out-server` | management-protocols | [079-dial-out-server.md](../06-operation-commands/079-dial-out-server.md) |
| `direction` | `direction-<index>` | `direction` | optical-layer0 | [081-direction.md](../06-operation-commands/081-direction.md) |
| `dns` | `dns` | `dns` | ip-networking | [084-dns.md](../06-operation-commands/084-dns.md) |
| `dns-server` | `dns-server-<address>` | `dns-server` | ip-networking | [085-dns-server.md](../06-operation-commands/085-dns-server.md) |
| `downloaded-image` | `downloaded-image-<manifest-file>/<name>` | `downloaded-image` | software-firmware-files | [087-downloaded-image.md](../06-operation-commands/087-downloaded-image.md) |
| `downloads` | `downloads` | `downloads` | software-firmware-files | [088-downloads.md](../06-operation-commands/088-downloads.md) |
| `dsc` | `dsc-<name>` | `dsc` | optical-layer0 | [089-dsc.md](../06-operation-commands/089-dsc.md) |
| `dsc-group` | `dsc-group-<name>` | `dsc-group` | optical-layer0 | [090-dsc-group.md](../06-operation-commands/090-dsc-group.md) |
| `encryption-algorithm` | `encryption-algorithm-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number>/<algorithm>/<key-length>` | `encryption-algorithm` | encryption-ipsec-macsec | [091-encryption-algorithm.md](../06-operation-commands/091-encryption-algorithm.md) |
| `equipment` | `equipment` | `equipment` | equipment-inventory | [092-equipment.md](../06-operation-commands/092-equipment.md) |
| `equipment-policies` | `equipment-policies` | `equipment-policies` | equipment-inventory | [093-equipment-policies.md](../06-operation-commands/093-equipment-policies.md) |
| `equipment-templates` | `equipment-templates` | `equipment-templates` | equipment-inventory | [094-equipment-templates.md](../06-operation-commands/094-equipment-templates.md) |
| `est-ca` | `est-ca-<name>` | `est-ca` | certificates-pki | [096-est-ca.md](../06-operation-commands/096-est-ca.md) |
| `est-server` | `est-server-<name>/<server-name>` | `est-server` | certificates-pki | [097-est-server.md](../06-operation-commands/097-est-server.md) |
| `eth-zr` | `eth-zr-<name>` | `eth-zr` | transport-layer1 | [098-eth-zr.md](../06-operation-commands/098-eth-zr.md) |
| `ethernet` | `ethernet-<name>` | `ethernet` | transport-layer1 | [099-ethernet.md](../06-operation-commands/099-ethernet.md) |
| `extended-config` | `extended-config-<name>` | `extended-config` | config-datastore | [103-extended-config.md](../06-operation-commands/103-extended-config.md) |
| `external-fiber-connection` | `external-fiber-connection-<name>` | `external-fiber-connection` | topology-discovery | [104-external-fiber-connection.md](../06-operation-commands/104-external-fiber-connection.md) |
| `facilities` | `facilities` | `facilities` | transport-layer1 | [105-facilities.md](../06-operation-commands/105-facilities.md) |
| `fc` | `fc-<name>` | `fc` | transport-layer1 | [106-fc.md](../06-operation-commands/106-fc.md) |
| `fiber-connection` | `fiber-connection-<name>` | `fiber-connection` | topology-discovery | [107-fiber-connection.md](../06-operation-commands/107-fiber-connection.md) |
| `file-server` | `file-server-<name>` | `file-server` | software-firmware-files | [110-file-server.md](../06-operation-commands/110-file-server.md) |
| `flexo` | `flexo-<name>` | `flexo` | transport-layer1 | [112-flexo.md](../06-operation-commands/112-flexo.md) |
| `fru-info` | `fru-info-<manifest-file>/<equipment-type>` | `fru-info` | equipment-inventory | [114-fru-info.md](../06-operation-commands/114-fru-info.md) |
| `gadt` | `gadt` | `gadt` | optical-layer0 | [115-gadt.md](../06-operation-commands/115-gadt.md) |
| `gapt` | `gapt-<card-type>` | `gapt` | optical-layer0 | [116-gapt.md](../06-operation-commands/116-gapt.md) |
| `gcmt` | `gcmt` | `gcmt` | optical-layer0 | [117-gcmt.md](../06-operation-commands/117-gcmt.md) |
| `golden-advanced-parameter` | `golden-advanced-parameter-<card-type>/<name>` | `golden-advanced-parameter` | config-datastore | [119-golden-advanced-parameter.md](../06-operation-commands/119-golden-advanced-parameter.md) |
| `golden-carrier-mode` | `golden-carrier-mode-<card-type>/<carrier-mode>` | `golden-carrier-mode` | optical-layer0 | [120-golden-carrier-mode.md](../06-operation-commands/120-golden-carrier-mode.md) |
| `grpc` | `grpc` | `grpc` | management-protocols | [121-grpc.md](../06-operation-commands/121-grpc.md) |
| `high-speed-monitoring` | `high-speed-monitoring` | `high-speed-monitoring` | transport-layer1 | [123-high-speed-monitoring.md](../06-operation-commands/123-high-speed-monitoring.md) |
| `http-file-server` | `http-file-server` | `http-file-server` | software-firmware-files | [124-http-file-server.md](../06-operation-commands/124-http-file-server.md) |
| `icdp` | `icdp` | `icdp` | topology-discovery | [125-icdp.md](../06-operation-commands/125-icdp.md) |
| `if-dhcp-relay` | `if-dhcp-relay-<if-name>` | `if-dhcp-relay` | ip-networking | [126-if-dhcp-relay.md](../06-operation-commands/126-if-dhcp-relay.md) |
| `ike-sa-proposal` | `ike-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<number>` | `ike-sa-proposal` | encryption-ipsec-macsec | [127-ike-sa-proposal.md](../06-operation-commands/127-ike-sa-proposal.md) |
| `ikev2` | `ikev2` | `ikev2` | encryption-ipsec-macsec | [128-ikev2.md](../06-operation-commands/128-ikev2.md) |
| `ikev2-local-instance` | `ikev2-local-instance-<name>` | `ikev2-local-instance` | encryption-ipsec-macsec | [129-ikev2-local-instance.md](../06-operation-commands/129-ikev2-local-instance.md) |
| `ikev2-peer` | `ikev2-peer-<ikev2-local-instance-name>/<ikev2-peer-name>` | `ikev2-peer` | encryption-ipsec-macsec | [130-ikev2-peer.md](../06-operation-commands/130-ikev2-peer.md) |
| `inci` | `inci` | `inci` | topology-discovery | [132-inci.md](../06-operation-commands/132-inci.md) |
| `inci-neighbor` | `inci-neighbor-<neighbor-id>` | `inci-neighbor` | topology-discovery | [133-inci-neighbor.md](../06-operation-commands/133-inci-neighbor.md) |
| `interface` | `interface-<if-name>` | `interface` | ip-networking | [134-interface.md](../06-operation-commands/134-interface.md) |
| `interface-neighbor` | `interface-neighbor-<local-interface>` | `interface-neighbor` | topology-discovery | [135-interface-neighbor.md](../06-operation-commands/135-interface-neighbor.md) |
| `interlaken` | `interlaken-<name>` | `interlaken` | transport-layer1 | [136-interlaken.md](../06-operation-commands/136-interlaken.md) |
| `inventory` | `inventory-<card-name>-<port-name>` | `inventory` | equipment-inventory | [137-inventory.md](../06-operation-commands/137-inventory.md) |
| `ip-monitoring` | `ip-monitoring-<name>` | `ip-monitoring` | ip-networking | [138-ip-monitoring.md](../06-operation-commands/138-ip-monitoring.md) |
| `ipsec-sa-proposal` | `ipsec-sa-proposal-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<number>` | `ipsec-sa-proposal` | encryption-ipsec-macsec | [139-ipsec-sa-proposal.md](../06-operation-commands/139-ipsec-sa-proposal.md) |
| `ipsec-sa-re-key` | `ipsec-sa-re-key-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>` | `ipsec-sa-re-key` | encryption-ipsec-macsec | [140-ipsec-sa-re-key.md](../06-operation-commands/140-ipsec-sa-re-key.md) |
| `ipsec-spd-entry` | `ipsec-spd-entry-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>` | `ipsec-spd-entry` | encryption-ipsec-macsec | [141-ipsec-spd-entry.md](../06-operation-commands/141-ipsec-spd-entry.md) |
| `ipsec-traffic-selector` | `ipsec-traffic-selector-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>` | `ipsec-traffic-selector` | encryption-ipsec-macsec | [142-ipsec-traffic-selector.md](../06-operation-commands/142-ipsec-traffic-selector.md) |
| `ipv4-address` | `ipv4-address-<if-name>/<ip>` | `ipv4-address` | ip-networking | [143-ipv4-address.md](../06-operation-commands/143-ipv4-address.md) |
| `ipv4-static-route` | `ipv4-static-route-<ipv4-destination-prefix>/<vrf>` | `ipv4-static-route` | ip-networking | [144-ipv4-static-route.md](../06-operation-commands/144-ipv4-static-route.md) |
| `ipv6-address` | `ipv6-address-<if-name>/<ip>` | `ipv6-address` | ip-networking | [145-ipv6-address.md](../06-operation-commands/145-ipv6-address.md) |
| `ipv6-static-route` | `ipv6-static-route-<ipv6-destination-prefix>/<vrf>` | `ipv6-static-route` | ip-networking | [146-ipv6-static-route.md](../06-operation-commands/146-ipv6-static-route.md) |
| `ISK` | `ISK-<name>` | `ISK` | certificates-pki | [147-isk.md](../06-operation-commands/147-isk.md) |
| `key-replacement-package` | `key-replacement-package` | `key-replacement-package` | certificates-pki | [148-key-replacement-package.md](../06-operation-commands/148-key-replacement-package.md) |
| `KRK` | `KRK-<name>` | `KRK` | certificates-pki | [150-krk.md](../06-operation-commands/150-krk.md) |
| `l0-capabilities` | `l0-capabilities` | `l0-capabilities` | optical-layer0 | [151-l0-capabilities.md](../06-operation-commands/151-l0-capabilities.md) |
| `L2-bridge` | `L2-bridge-<bridge-name>` | `L2-bridge` | transport-layer1 | [170-l2-bridge.md](../06-operation-commands/170-l2-bridge.md) |
| `led` | `led-<location>/<name>` | `led` | equipment-inventory | [152-led.md](../06-operation-commands/152-led.md) |
| `line-ptp` | `line-ptp-<name>` | `line-ptp` | transport-layer1 | [153-line-ptp.md](../06-operation-commands/153-line-ptp.md) |
| `links` | `links` | `links` | topology-discovery | [154-links.md](../06-operation-commands/154-links.md) |
| `lldp` | `lldp` | `lldp` | topology-discovery | [155-lldp.md](../06-operation-commands/155-lldp.md) |
| `lldp-local-info` | `lldp-local-info-<lldp-port>` | `lldp-local-info` | topology-discovery | [156-lldp-local-info.md](../06-operation-commands/156-lldp-local-info.md) |
| `lldp-neighbor` | `lldp-neighbor-<lldp-port>/<direction>` | `lldp-neighbor` | topology-discovery | [157-lldp-neighbor.md](../06-operation-commands/157-lldp-neighbor.md) |
| `lldp-port-statistics` | `lldp-port-statistics-<lldp-port>/<direction>` | `lldp-port-statistics` | topology-discovery | [158-lldp-port-statistics.md](../06-operation-commands/158-lldp-port-statistics.md) |
| `local-certificate` | `local-certificate-<id>` | `local-certificate` | certificates-pki | [159-local-certificate.md](../06-operation-commands/159-local-certificate.md) |
| `local-ports` | `local-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>` | `local-ports` | encryption-ipsec-macsec | [160-local-ports.md](../06-operation-commands/160-local-ports.md) |
| `local-subnet` | `local-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>` | `local-subnet` | encryption-ipsec-macsec | [161-local-subnet.md](../06-operation-commands/161-local-subnet.md) |
| `log-console` | `log-console` | `log-console` | fault-alarms-logging | [164-log-console.md](../06-operation-commands/164-log-console.md) |
| `log-console-facility-filter` | `log-console-facility-filter-<name>` | `log-console-facility-filter` | fault-alarms-logging | [165-log-console-facility-filter.md](../06-operation-commands/165-log-console-facility-filter.md) |
| `log-file` | `log-file-<name>` | `log-file` | fault-alarms-logging | [166-log-file.md](../06-operation-commands/166-log-file.md) |
| `log-file-facility-filter` | `log-file-facility-filter-<log-file-name>/<log-file-facility-filter-name>` | `log-file-facility-filter` | fault-alarms-logging | [167-log-file-facility-filter.md](../06-operation-commands/167-log-file-facility-filter.md) |
| `log-server` | `log-server-<name>` | `log-server` | fault-alarms-logging | [168-log-server.md](../06-operation-commands/168-log-server.md) |
| `log-server-facility-filter` | `log-server-facility-filter-<log-server-name>/<log-server-facility-filter-name>` | `log-server-facility-filter` | fault-alarms-logging | [169-log-server-facility-filter.md](../06-operation-commands/169-log-server-facility-filter.md) |
| `macsec-entity` | `macsec-entity-<name>` | `macsec-entity` | encryption-ipsec-macsec | [171-macsec-entity.md](../06-operation-commands/171-macsec-entity.md) |
| `macsec-mka` | `macsec-mka-<name>` | `macsec-mka` | encryption-ipsec-macsec | [172-macsec-mka.md](../06-operation-commands/172-macsec-mka.md) |
| `management-address` | `management-address-<lldp-port>/<direction>/<address-subtype>/<address>` | `management-address` | ip-networking | [174-management-address.md](../06-operation-commands/174-management-address.md) |
| `management-address-local` | `management-address-local-<lldp-port>/<address-subtype>/<address>` | `management-address-local` | ip-networking | [175-management-address-local.md](../06-operation-commands/175-management-address-local.md) |
| `manifest` | `manifest-<manifest-file>` | `manifest` | software-firmware-files | [176-manifest.md](../06-operation-commands/176-manifest.md) |
| `mc` | `mc-<name>` | `mc` | optical-layer0 | [178-mc.md](../06-operation-commands/178-mc.md) |
| `mc-f` | `mc-f-<name>` | `mc-f` | optical-layer0 | [179-mc-f.md](../06-operation-commands/179-mc-f.md) |
| `mka-policy` | `mka-policy-<name>` | `mka-policy` | encryption-ipsec-macsec | [173-mka-policy.md](../06-operation-commands/173-mka-policy.md) |
| `modules-adg` | `modules-adg-<adg-number>/<index>` | `modules-adg` | optical-layer0 | [181-modules-adg.md](../06-operation-commands/181-modules-adg.md) |
| `modules-degree` | `modules-degree-<degree-number>/<index>` | `modules-degree` | optical-layer0 | [182-modules-degree.md](../06-operation-commands/182-modules-degree.md) |
| `monitored-channel` | `monitored-channel-<name>/<frequency>` | `monitored-channel` | optical-layer0 | [183-monitored-channel.md](../06-operation-commands/183-monitored-channel.md) |
| `named-value-set` | `named-value-set-<db-entry-name>/<named-value-set-name>` | `named-value-set` | config-datastore | [184-named-value-set.md](../06-operation-commands/184-named-value-set.md) |
| `nct-connection` | `nct-connection-<src-port>/<dst-port>` | `nct-connection` | topology-discovery | [185-nct-connection.md](../06-operation-commands/185-nct-connection.md) |
| `ne` | `ne` | `ne` | system-node-time | [186-ne.md](../06-operation-commands/186-ne.md) |
| `ne-function` | `ne-function` | `ne-function` | system-node-time | [187-ne-function.md](../06-operation-commands/187-ne-function.md) |
| `netconf` | `netconf` | `netconf` | management-protocols | [188-netconf.md](../06-operation-commands/188-netconf.md) |
| `network-xconnect` | `network-xconnect` | `network-xconnect` | transport-layer1 | [189-network-xconnect.md](../06-operation-commands/189-network-xconnect.md) |
| `networking` | `networking` | `networking` | ip-networking | [190-networking.md](../06-operation-commands/190-networking.md) |
| `networking-services` | `networking-services` | `networking-services` | ip-networking | [191-networking-services.md](../06-operation-commands/191-networking-services.md) |
| `next-hop` | `next-hop-<rib-name>/<destination-prefix>/<interface>` | `next-hop` | ip-networking | [192-next-hop.md](../06-operation-commands/192-next-hop.md) |
| `nmc` | `nmc-<name>` | `nmc` | optical-layer0 | [193-nmc.md](../06-operation-commands/193-nmc.md) |
| `nmc-f` | `nmc-f-<name>` | `nmc-f` | optical-layer0 | [194-nmc-f.md](../06-operation-commands/194-nmc-f.md) |
| `ntp` | `ntp` | `ntp` | system-node-time | [195-ntp.md](../06-operation-commands/195-ntp.md) |
| `ntp-key` | `ntp-key-<key-id>` | `ntp-key` | system-node-time | [196-ntp-key.md](../06-operation-commands/196-ntp-key.md) |
| `ntp-server` | `ntp-server-<ip-address>` | `ntp-server` | system-node-time | [197-ntp-server.md](../06-operation-commands/197-ntp-server.md) |
| `ntp-server-status` | `ntp-server-status-<ip-address>` | `ntp-server-status` | system-node-time | [198-ntp-server-status.md](../06-operation-commands/198-ntp-server-status.md) |
| `nw-xconnect` | `nw-xconnect-<name>` | `nw-xconnect` | transport-layer1 | [199-nw-xconnect.md](../06-operation-commands/199-nw-xconnect.md) |
| `oadm-capabilities` | `oadm-capabilities` | `oadm-capabilities` | optical-layer0 | [200-oadm-capabilities.md](../06-operation-commands/200-oadm-capabilities.md) |
| `oc` | `oc-<name>` | `oc` | optical-layer0 | [201-oc.md](../06-operation-commands/201-oc.md) |
| `ochm` | `ochm-<name>` | `ochm` | optical-layer0 | [202-ochm.md](../06-operation-commands/202-ochm.md) |
| `ocm-channel` | `ocm-channel-<name>/<lower-frequency>/<upper-frequency>` | `ocm-channel` | optical-layer0 | [203-ocm-channel.md](../06-operation-commands/203-ocm-channel.md) |
| `ocm-mp` | `ocm-mp-<name>` | `ocm-mp` | optical-layer0 | [204-ocm-mp.md](../06-operation-commands/204-ocm-mp.md) |
| `ocm-ptp` | `ocm-ptp-<name>` | `ocm-ptp` | optical-layer0 | [205-ocm-ptp.md](../06-operation-commands/205-ocm-ptp.md) |
| `ocsp-server` | `ocsp-server-<name>` | `ocsp-server` | certificates-pki | [206-ocsp-server.md](../06-operation-commands/206-ocsp-server.md) |
| `odu` | `odu-<name>` | `odu` | transport-layer1 | [207-odu.md](../06-operation-commands/207-odu.md) |
| `odu-diagnostics` | `odu-diagnostics-<name>/<direction>` | `odu-diagnostics` | transport-layer1 | [208-odu-diagnostics.md](../06-operation-commands/208-odu-diagnostics.md) |
| `oms` | `oms-<name>` | `oms` | optical-layer0 | [209-oms.md](../06-operation-commands/209-oms.md) |
| `ops` | `ops-<name>` | `ops` | optical-layer0 | [210-ops.md](../06-operation-commands/210-ops.md) |
| `optical-carrier` | `optical-carrier-<name>` | `optical-carrier` | optical-layer0 | [211-optical-carrier.md](../06-operation-commands/211-optical-carrier.md) |
| `optical-channel` | `optical-channel-<name>` | `optical-channel` | optical-layer0 | [212-optical-channel.md](../06-operation-commands/212-optical-channel.md) |
| `optical-ptp` | `optical-ptp-<name>` | `optical-ptp` | optical-layer0 | [213-optical-ptp.md](../06-operation-commands/213-optical-ptp.md) |
| `optical-switch` | `optical-switch-<name>` | `optical-switch` | optical-layer0 | [214-optical-switch.md](../06-operation-commands/214-optical-switch.md) |
| `osc` | `osc-<name>` | `osc` | optical-layer0 | [215-osc.md](../06-operation-commands/215-osc.md) |
| `ospf-area` | `ospf-area-<instance-id>/<ospf-area-id>` | `ospf-area` | ip-networking | [217-ospf-area.md](../06-operation-commands/217-ospf-area.md) |
| `ospf-area-range` | `ospf-area-range-<instance-id>/<ospf-area-id>/<prefix>` | `ospf-area-range` | ip-networking | [218-ospf-area-range.md](../06-operation-commands/218-ospf-area-range.md) |
| `ospf-instance` | `ospf-instance-<instance-id>` | `ospf-instance` | ip-networking | [219-ospf-instance.md](../06-operation-commands/219-ospf-instance.md) |
| `ospf-interface` | `ospf-interface-<instance-id>/<ospf-area-id>/<ospf-if-name>` | `ospf-interface` | ip-networking | [220-ospf-interface.md](../06-operation-commands/220-ospf-interface.md) |
| `ospf-neighbor` | `ospf-neighbor-<instance-id>/<ospf-area-id>/<ospf-if-name>/<router-id>` | `ospf-neighbor` | ip-networking | [221-ospf-neighbor.md](../06-operation-commands/221-ospf-neighbor.md) |
| `ospfv3-ipsec-security-association` | `ospfv3-ipsec-security-association-<instance-id>/<ospf-area-id>/<ospf-if-name>/<spi>` | `ospfv3-ipsec-security-association` | ip-networking | [222-ospfv3-ipsec-security-association.md](../06-operation-commands/222-ospfv3-ipsec-security-association.md) |
| `otdr` | `otdr-<name>` | `otdr` | optical-layer0 | [223-otdr.md](../06-operation-commands/223-otdr.md) |
| `otdr-ptp` | `otdr-ptp-<name>` | `otdr-ptp` | optical-layer0 | [224-otdr-ptp.md](../06-operation-commands/224-otdr-ptp.md) |
| `ots` | `ots-<name>` | `ots` | optical-layer0 | [225-ots.md](../06-operation-commands/225-ots.md) |
| `ots-diagnostics` | `ots-diagnostics-<name>` | `ots-diagnostics` | optical-layer0 | [226-ots-diagnostics.md](../06-operation-commands/226-ots-diagnostics.md) |
| `ots-r` | `ots-r-<name>` | `ots-r` | optical-layer0 | [227-ots-r.md](../06-operation-commands/227-ots-r.md) |
| `ots-r-auto-otdr` | `ots-r-auto-otdr-<name>` | `ots-r-auto-otdr` | optical-layer0 | [228-ots-r-auto-otdr.md](../06-operation-commands/228-ots-r-auto-otdr.md) |
| `otu` | `otu-<name>` | `otu` | transport-layer1 | [229-otu.md](../06-operation-commands/229-otu.md) |
| `otu-diagnostics` | `otu-diagnostics-<name>/<direction>` | `otu-diagnostics` | transport-layer1 | [230-otu-diagnostics.md](../06-operation-commands/230-otu-diagnostics.md) |
| `oxcon` | `oxcon-<name>` | `oxcon` | optical-layer0 | [231-oxcon.md](../06-operation-commands/231-oxcon.md) |
| `packaged-fw` | `packaged-fw-<location-id>/<swload-state>/<equipment-type>/<fw-name>` | `packaged-fw` | software-firmware-files | [232-packaged-fw.md](../06-operation-commands/232-packaged-fw.md) |
| `pm` | `pm` | `pm` | performance-monitoring | [236-pm.md](../06-operation-commands/236-pm.md) |
| `pm-catalog` | `pm-catalog` | `pm-catalog` | performance-monitoring | [237-pm-catalog.md](../06-operation-commands/237-pm-catalog.md) |
| `pm-control` | `pm-control` | `pm-control` | performance-monitoring | [238-pm-control.md](../06-operation-commands/238-pm-control.md) |
| `pm-control-entry` | `pm-control-entry-<resource>/<period>/<direction>/<location>` | `pm-control-entry` | performance-monitoring | [239-pm-control-entry.md](../06-operation-commands/239-pm-control-entry.md) |
| `pm-parameter` | `pm-parameter-<parameter>` | `pm-parameter` | performance-monitoring | [240-pm-parameter.md](../06-operation-commands/240-pm-parameter.md) |
| `pm-profile` | `pm-profile` | `pm-profile` | performance-monitoring | [241-pm-profile.md](../06-operation-commands/241-pm-profile.md) |
| `pm-profile-entry` | `pm-profile-entry-<resource-type>/<direction>/<location>/<period>` | `pm-profile-entry` | performance-monitoring | [242-pm-profile-entry.md](../06-operation-commands/242-pm-profile-entry.md) |
| `pm-resource` | `pm-resource-<resource>` | `pm-resource` | performance-monitoring | [243-pm-resource.md](../06-operation-commands/243-pm-resource.md) |
| `pm-threshold` | `pm-threshold-<resource>/<period>/<direction>/<location>/<parameter>` | `pm-threshold` | performance-monitoring | [244-pm-threshold.md](../06-operation-commands/244-pm-threshold.md) |
| `pm-threshold-profile` | `pm-threshold-profile-<resource-type>/<direction>/<location>/<period>/<parameter>` | `pm-threshold-profile` | performance-monitoring | [245-pm-threshold-profile.md](../06-operation-commands/245-pm-threshold-profile.md) |
| `port` | `port-<card-name>-<port-name>` | `port` | equipment-inventory | [246-port.md](../06-operation-commands/246-port.md) |
| `property` | `property-<card-name>/<property-name>` | `property` | cli-and-session | [249-property.md](../06-operation-commands/249-property.md) |
| `protection` | `protection` | `protection` | protection-redundancy | [250-protection.md](../06-operation-commands/250-protection.md) |
| `protection-group` | `protection-group-<name>` | `protection-group` | protection-redundancy | [251-protection-group.md](../06-operation-commands/251-protection-group.md) |
| `protection-unit` | `protection-unit-<protection-group-name>/<protection-unit-name>` | `protection-unit` | protection-redundancy | [253-protection-unit.md](../06-operation-commands/253-protection-unit.md) |
| `protocols` | `protocols` | `protocols` | ip-networking | [254-protocols.md](../06-operation-commands/254-protocols.md) |
| `pump` | `pump-<name>` | `pump` | optical-layer0 | [255-pump.md](../06-operation-commands/255-pump.md) |
| `pump-power` | `pump-power-<name>/<pump-id>` | `pump-power` | optical-layer0 | [256-pump-power.md](../06-operation-commands/256-pump-power.md) |
| `raman-calibration` | `raman-calibration-<name>` | `raman-calibration` | optical-layer0 | [257-raman-calibration.md](../06-operation-commands/257-raman-calibration.md) |
| `recovery` | `recovery` | `recovery` | config-datastore | [261-recovery.md](../06-operation-commands/261-recovery.md) |
| `remote-ports` | `remote-ports-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<start>/<stop>` | `remote-ports` | encryption-ipsec-macsec | [262-remote-ports.md](../06-operation-commands/262-remote-ports.md) |
| `remote-subnet` | `remote-subnet-<ikev2-local-instance-name>/<ikev2-peer-name>/<ipsec-spd-entry-name>/<ipsec-traffic-selector-name>/<prefix>` | `remote-subnet` | encryption-ipsec-macsec | [263-remote-subnet.md](../06-operation-commands/263-remote-subnet.md) |
| `resources` | `resources-<name>` | `resources` | equipment-inventory | [264-resources.md](../06-operation-commands/264-resources.md) |
| `restconf` | `restconf` | `restconf` | management-protocols | [266-restconf.md](../06-operation-commands/266-restconf.md) |
| `rib` | `rib-<rib-name>` | `rib` | ip-networking | [267-rib.md](../06-operation-commands/267-rib.md) |
| `route` | `route-<rib-name>/<destination-prefix>` | `route` | ip-networking | [269-route.md](../06-operation-commands/269-route.md) |
| `routing` | `routing` | `routing` | ip-networking | [270-routing.md](../06-operation-commands/270-routing.md) |
| `rsc` | `rsc-<name>` | `rsc` | optical-layer0 | [271-rsc.md](../06-operation-commands/271-rsc.md) |
| `sc-rx` | `sc-rx-<name>/<index>` | `sc-rx` | encryption-ipsec-macsec | [273-sc-rx.md](../06-operation-commands/273-sc-rx.md) |
| `sc-tx` | `sc-tx-<name>/<index>` | `sc-tx` | encryption-ipsec-macsec | [274-sc-tx.md](../06-operation-commands/274-sc-tx.md) |
| `scheduled-task` | `scheduled-task-<name>` | `scheduled-task` | cli-and-session | [275-scheduled-task.md](../06-operation-commands/275-scheduled-task.md) |
| `secure-application` | `secure-application-<id>` | `secure-application` | encryption-ipsec-macsec | [276-secure-application.md](../06-operation-commands/276-secure-application.md) |
| `secure-entity` | `secure-entity-<name>` | `secure-entity` | encryption-ipsec-macsec | [277-secure-entity.md](../06-operation-commands/277-secure-entity.md) |
| `secure-entity-sa-proposal` | `secure-entity-sa-proposal-<name>/<number>` | `secure-entity-sa-proposal` | encryption-ipsec-macsec | [278-secure-entity-sa-proposal.md](../06-operation-commands/278-secure-entity-sa-proposal.md) |
| `security` | `security` | `security` | security-access-control | [279-security.md](../06-operation-commands/279-security.md) |
| `security-policies` | `security-policies` | `security-policies` | security-access-control | [280-security-policies.md](../06-operation-commands/280-security-policies.md) |
| `security-policy-database` | `security-policy-database-<ikev2-local-instance>/<ikev2-peer-name>` | `security-policy-database` | encryption-ipsec-macsec | [281-security-policy-database.md](../06-operation-commands/281-security-policy-database.md) |
| `serdes` | `serdes-<card-name>-<port-name>/<serdes-name>` | `serdes` | equipment-inventory | [282-serdes.md](../06-operation-commands/282-serdes.md) |
| `serdes-template` | `serdes-template-<tom-part-number>` | `serdes-template` | equipment-inventory | [283-serdes-template.md](../06-operation-commands/283-serdes-template.md) |
| `serdes-template-entry` | `serdes-template-entry-<serdes-template-name>/<serdes-template-entry-name>` | `serdes-template-entry` | equipment-inventory | [284-serdes-template-entry.md](../06-operation-commands/284-serdes-template-entry.md) |
| `serial-console` | `serial-console` | `serial-console` | equipment-inventory | [285-serial-console.md](../06-operation-commands/285-serial-console.md) |
| `session` | `session-<session-id>` | `session` | cli-and-session | [286-session.md](../06-operation-commands/286-session.md) |
| `slot` | `slot-<chassis-name>-<slot-name>` | `slot` | equipment-inventory | [295-slot.md](../06-operation-commands/295-slot.md) |
| `sndp` | `sndp` | `sndp` | topology-discovery | [296-sndp.md](../06-operation-commands/296-sndp.md) |
| `snmp` | `snmp` | `snmp` | management-protocols | [297-snmp.md](../06-operation-commands/297-snmp.md) |
| `snmp-community` | `snmp-community-<name>` | `snmp-community` | management-protocols | [298-snmp-community.md](../06-operation-commands/298-snmp-community.md) |
| `snmp-target` | `snmp-target-<target-name>` | `snmp-target` | management-protocols | [299-snmp-target.md](../06-operation-commands/299-snmp-target.md) |
| `snmpv3-user` | `snmpv3-user-<snmpv3-user-name>` | `snmpv3-user` | management-protocols | [300-snmpv3-user.md](../06-operation-commands/300-snmpv3-user.md) |
| `software-load` | `software-load-<location-id>/<swload-state>` | `software-load` | software-firmware-files | [301-software-load.md](../06-operation-commands/301-software-load.md) |
| `software-location` | `software-location-<location-id>` | `software-location` | software-firmware-files | [302-software-location.md](../06-operation-commands/302-software-location.md) |
| `spectrum` | `spectrum-<name>` | `spectrum` | optical-layer0 | [303-spectrum.md](../06-operation-commands/303-spectrum.md) |
| `spectrum-control` | `spectrum-control-<name>/<direction>/<center-frequency>` | `spectrum-control` | optical-layer0 | [304-spectrum-control.md](../06-operation-commands/304-spectrum-control.md) |
| `spectrum-monitoring` | `spectrum-monitoring-<name>/<direction>/<center-frequency>` | `spectrum-monitoring` | optical-layer0 | [305-spectrum-monitoring.md](../06-operation-commands/305-spectrum-monitoring.md) |
| `ssh` | `ssh` | `ssh` | certificates-pki | [306-ssh.md](../06-operation-commands/306-ssh.md) |
| `ssh-authorized-key` | `ssh-authorized-key-<user-name>/<key-id>` | `ssh-authorized-key` | certificates-pki | [307-ssh-authorized-key.md](../06-operation-commands/307-ssh-authorized-key.md) |
| `ssh-host-key` | `ssh-host-key-<public-key-algorithm>` | `ssh-host-key` | certificates-pki | [308-ssh-host-key.md](../06-operation-commands/308-ssh-host-key.md) |
| `ssh-known-host` | `ssh-known-host-<id>` | `ssh-known-host` | certificates-pki | [310-ssh-known-host.md](../06-operation-commands/310-ssh-known-host.md) |
| `stm` | `stm-<name>` | `stm` | transport-layer1 | [313-stm.md](../06-operation-commands/313-stm.md) |
| `sub-component` | `sub-component-<card-name>/<sub-component-name>` | `sub-component` | equipment-inventory | [314-sub-component.md](../06-operation-commands/314-sub-component.md) |
| `submarine-link` | `submarine-link-<name>` | `submarine-link` | topology-discovery | [315-submarine-link.md](../06-operation-commands/315-submarine-link.md) |
| `subscription-path` | `subscription-path-<subscription-name>/<subscription-path-name>` | `subscription-path` | management-protocols | [316-subscription-path.md](../06-operation-commands/316-subscription-path.md) |
| `subscriptions` | `subscriptions` | `subscriptions` | management-protocols | [317-subscriptions.md](../06-operation-commands/317-subscriptions.md) |
| `subtype-constraint` | `subtype-constraint-<card-type>/<subtype>` | `subtype-constraint` | software-firmware-files | [318-subtype-constraint.md](../06-operation-commands/318-subtype-constraint.md) |
| `super-channel` | `super-channel-<name>` | `super-channel` | optical-layer0 | [319-super-channel.md](../06-operation-commands/319-super-channel.md) |
| `super-channel-group` | `super-channel-group` | `super-channel-group` | optical-layer0 | [320-super-channel-group.md](../06-operation-commands/320-super-channel-group.md) |
| `supported-card` | `supported-card-<card-type>` | `supported-card` | equipment-inventory | [321-supported-card.md](../06-operation-commands/321-supported-card.md) |
| `supported-carrier-mode` | `supported-carrier-mode-<name>/<carrier-mode>` | `supported-carrier-mode` | optical-layer0 | [322-supported-carrier-mode.md](../06-operation-commands/322-supported-carrier-mode.md) |
| `supported-chassis` | `supported-chassis-<chassis-type>` | `supported-chassis` | equipment-inventory | [323-supported-chassis.md](../06-operation-commands/323-supported-chassis.md) |
| `supported-gain-range` | `supported-gain-range-<name>/<gain-range-type>` | `supported-gain-range` | optical-layer0 | [324-supported-gain-range.md](../06-operation-commands/324-supported-gain-range.md) |
| `supported-port` | `supported-port-<card-type>/<port-name>` | `supported-port` | equipment-inventory | [325-supported-port.md](../06-operation-commands/325-supported-port.md) |
| `supported-power-profile` | `supported-power-profile-<card-type>/<name>` | `supported-power-profile` | optical-layer0 | [326-supported-power-profile.md](../06-operation-commands/326-supported-power-profile.md) |
| `supported-slot` | `supported-slot-<card-type>/<slot-name>` | `supported-slot` | equipment-inventory | [327-supported-slot.md](../06-operation-commands/327-supported-slot.md) |
| `supported-tom` | `supported-tom-<card-type>/<port-name>/<tom-type>/<tom-subtype-group>` | `supported-tom` | equipment-inventory | [328-supported-tom.md](../06-operation-commands/328-supported-tom.md) |
| `supported-tom-power` | `supported-tom-power-<card-type>/<port-name>/<tom-type>` | `supported-tom-power` | equipment-inventory | [329-supported-tom-power.md](../06-operation-commands/329-supported-tom-power.md) |
| `supporting-fiber-connection` | `supporting-fiber-connection-<name>` | `supporting-fiber-connection` | topology-discovery | [330-supporting-fiber-connection.md](../06-operation-commands/330-supporting-fiber-connection.md) |
| `supporting-interface` | `supporting-interface-<name>/<interface>` | `supporting-interface` | ip-networking | [331-supporting-interface.md](../06-operation-commands/331-supporting-interface.md) |
| `sw-component` | `sw-component-<location-id>/<swload-state>/<name>` | `sw-component` | software-firmware-files | [332-sw-component.md](../06-operation-commands/332-sw-component.md) |
| `sw-container` | `sw-container-<container-name>` | `sw-container` | software-firmware-files | [333-sw-container.md](../06-operation-commands/333-sw-container.md) |
| `sw-control-rule` | `sw-control-rule-<service-name>` | `sw-control-rule` | software-firmware-files | [334-sw-control-rule.md](../06-operation-commands/334-sw-control-rule.md) |
| `sw-management` | `sw-management` | `sw-management` | software-firmware-files | [335-sw-management.md](../06-operation-commands/335-sw-management.md) |
| `sw-service` | `sw-service-<sv-name>` | `sw-service` | software-firmware-files | [336-sw-service.md](../06-operation-commands/336-sw-service.md) |
| `sw-subcomponent` | `sw-subcomponent-<location-id>/<swload-state>/<sw-component-name>/<sw-subcomponent-name>` | `sw-subcomponent` | software-firmware-files | [337-sw-subcomponent.md](../06-operation-commands/337-sw-subcomponent.md) |
| `syslog` | `syslog` | `syslog` | fault-alarms-logging | [339-syslog.md](../06-operation-commands/339-syslog.md) |
| `system` | `system` | `system` | system-node-time | [340-system.md](../06-operation-commands/340-system.md) |
| `system-policies` | `system-policies` | `system-policies` | config-datastore | [341-system-policies.md](../06-operation-commands/341-system-policies.md) |
| `task` | `task-<name>` | `task` | cli-and-session | [343-task.md](../06-operation-commands/343-task.md) |
| `telemetry` | `telemetry` | `telemetry` | management-protocols | [344-telemetry.md](../06-operation-commands/344-telemetry.md) |
| `template` | `template-<template-group-name>/<template-name>` | `template` | config-datastore | [345-template.md](../06-operation-commands/345-template.md) |
| `template-group` | `template-group-<name>` | `template-group` | config-datastore | [346-template-group.md](../06-operation-commands/346-template-group.md) |
| `templates` | `templates` | `templates` | config-datastore | [347-templates.md](../06-operation-commands/347-templates.md) |
| `third-party-app` | `third-party-app-<app-name>` | `third-party-app` | management-protocols | [349-third-party-app.md](../06-operation-commands/349-third-party-app.md) |
| `third-party-fw` | `third-party-fw-<fw-name>` | `third-party-fw` | software-firmware-files | [350-third-party-fw.md](../06-operation-commands/350-third-party-fw.md) |
| `tom` | `tom-<card-name>-<port-name>` | `tom` | equipment-inventory | [352-tom.md](../06-operation-commands/352-tom.md) |
| `tom-type` | `tom-type-<tom-type>` | `tom-type` | equipment-inventory | [353-tom-type.md](../06-operation-commands/353-tom-type.md) |
| `topology` | `topology` | `topology` | topology-discovery | [354-topology.md](../06-operation-commands/354-topology.md) |
| `transfer` | `transfer` | `transfer` | software-firmware-files | [356-transfer.md](../06-operation-commands/356-transfer.md) |
| `transfer-status` | `transfer-status-<filetype>/<operation>` | `transfer-status` | software-firmware-files | [357-transfer-status.md](../06-operation-commands/357-transfer-status.md) |
| `trib-ptp` | `trib-ptp-<name>` | `trib-ptp` | transport-layer1 | [358-trib-ptp.md](../06-operation-commands/358-trib-ptp.md) |
| `unprovisioned-inventory` | `unprovisioned-inventory-<chassis-serial-number>/<slot-name>` | `unprovisioned-inventory` | equipment-inventory | [361-unprovisioned-inventory.md](../06-operation-commands/361-unprovisioned-inventory.md) |
| `upgrade-status` | `upgrade-status-<resource>` | `upgrade-status` | software-firmware-files | [363-upgrade-status.md](../06-operation-commands/363-upgrade-status.md) |
| `usb` | `usb-<card-name>-<port-name>` | `usb` | equipment-inventory | [366-usb.md](../06-operation-commands/366-usb.md) |
| `user` | `user-<user-name>` | `user` | security-access-control | [367-user.md](../06-operation-commands/367-user.md) |
| `user-data` | `user-data` | `user-data` | security-access-control | [368-user-data.md](../06-operation-commands/368-user-data.md) |
| `user-group` | `user-group-<name>` | `user-group` | security-access-control | [369-user-group.md](../06-operation-commands/369-user-group.md) |
| `vrf` | `vrf-<name>` | `vrf` | ip-networking | [372-vrf.md](../06-operation-commands/372-vrf.md) |
| `xcon` | `xcon-<name>` | `xcon` | transport-layer1 | [373-xcon.md](../06-operation-commands/373-xcon.md) |
| `ztp` | `ztp` | `ztp` | software-firmware-files | [374-ztp.md](../06-operation-commands/374-ztp.md) |

## Sub-command keywords

These are literal keywords a command takes rather than addressable entities, so they do not appear above. They are listed because a query may name the keyword rather than the command.

| Keyword | Belongs to | File |
| --- | --- | --- |
| `ISKs` | `ISK` | [147-isk.md](../06-operation-commands/147-isk.md) |
| `KRKs` | `KRK` | [150-krk.md](../06-operation-commands/150-krk.md) |
| `activate-file` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `location-led` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `loopback` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `eqpt-fw` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `krp` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `otdr` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `otdr-fiber-check` | `activate` | [008-activate.md](../06-operation-commands/008-activate.md) |
| `db-instance` | `activate-snapshot` | [009-activate-snapshot.md](../06-operation-commands/009-activate-snapshot.md) |
| `alarms` | `alarm` | [014-alarm.md](../06-operation-commands/014-alarm.md) |
| `start` | `bert` | [028-bert.md](../06-operation-commands/028-bert.md) |
| `stop` | `bert` | [028-bert.md](../06-operation-commands/028-bert.md) |
| `get` | `bert` | [028-bert.md](../06-operation-commands/028-bert.md) |
| `delete` | `bert` | [028-bert.md](../06-operation-commands/028-bert.md) |
| `bgp` | `bgp-instance` | [029-bgp-instance.md](../06-operation-commands/029-bgp-instance.md) |
| `cdps` | `cdp` | [042-cdp.md](../06-operation-commands/042-cdp.md) |
| `certificate-revocation` | `certificate` | [045-certificate.md](../06-operation-commands/045-certificate.md) |
| `certificates` | `certificate` | [045-certificate.md](../06-operation-commands/045-certificate.md) |
| `confirmed` | `commit` | [055-commit.md](../06-operation-commands/055-commit.md) |
| `persist` | `commit` | [055-commit.md](../06-operation-commands/055-commit.md) |
| `cancel` | `commit` | [055-commit.md](../06-operation-commands/055-commit.md) |
| `crls` | `crl` | [063-crl.md](../06-operation-commands/063-crl.md) |
| `commit` | `diff` | [080-diff.md](../06-operation-commands/080-diff.md) |
| `file-servers` | `file-server` | [110-file-server.md](../06-operation-commands/110-file-server.md) |
| `filetype-<name>` | `file-type` | [111-file-type.md](../06-operation-commands/111-file-type.md) |
| `leds` | `led` | [152-led.md](../06-operation-commands/152-led.md) |
| `task` | `run` | [272-run.md](../06-operation-commands/272-run.md) |
| `script` | `run` | [272-run.md](../06-operation-commands/272-run.md) |
| `scheduled-tasks` | `scheduled-task` | [275-scheduled-task.md](../06-operation-commands/275-scheduled-task.md) |
| `secure-applications` | `secure-application` | [276-secure-application.md](../06-operation-commands/276-secure-application.md) |
| `commit` | `show commit` | [292-show-commit.md](../06-operation-commands/292-show-commit.md) |
| `sw-services` | `sw-service` | [336-sw-service.md](../06-operation-commands/336-sw-service.md) |
| `location-led` | `terminate` | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `otdr` | `terminate` | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `otdr-fiber-check` | `terminate` | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `loopback` | `terminate` | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `cable-id` | `terminate` | [348-terminate.md](../06-operation-commands/348-terminate.md) |
| `candidate` | `validate` | [370-validate.md](../06-operation-commands/370-validate.md) |
