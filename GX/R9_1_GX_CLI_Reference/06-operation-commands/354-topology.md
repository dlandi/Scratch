---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.354. topology'
source_lines: 26468-26568
---

## 6.354. topology

#### Command Description

The `clear topology` command, manually removes existing topology neighbor information. This action will remove existing topology information. The \<target\> may be:

- an lldp-neighbor instance, discovered via LLDP,
- a carrier-neighbor instance, discovered via ICMP, or
- an lldp-port-statistics instance, containing details associated with an LLDP enabled port.

**Tip:** Use 'clear topology \<tab\>' to get a list of valid topology objects.

The `show topology` is used to retrieve information about the topology.

#### Command Syntax

```
clear [-f] topology [target=]<value>
show topology
```

#### Command Usage Details

**Table 813: topology Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode (only for show command) |

<!-- page 1293 -->

#### Command Parameters

**Table 814: topology Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 815: topology Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| target | Target instance to be cleared. May be a lldp-neighbor, a carrier-neighbor or a lldp-port-statistics instance. | instance-identifier | n/a | clear |
| &lt; &gt; | Topology instance to be viewed:<br>• inci - Refer to for inci (p. 549) additional information on INCI parameters.<br>• links - Refer to links (p. 611) for additional information on links parameters.<br>• lldp - lldp information. Refer to lldp (p. 612) for additional information on LLDP parameters.<br>• icdp - icdp information. Refer to icdp (p. 530) for additional information on ICDP parameters.<br>• sndp | • inci<br>• links<br>• lldp<br>• icdp<br>• sndp | n/a | show |

<!-- page 1294 -->

#### Examples

This example shows how to clear the topology neighbor data on a 1830 GX G40 node:

```
clear topology lldp-neighbor-ethernet-1-4-T1-1
```

This example shows how to view the topology information:

```
show topology
```

The following output is displayed:

```
show topology
  topology
  lldp
  icdp
```

This example shows how to view the LLDP topology information:

```
show topology lldp
```

The following output is displayed:

```
show topology lldp
  lldp
  hold-on-timer     900 seconds
```

This example shows how to view the ICDP topology information:

```
show topology icdp
```

The following output is displayed:

```
show topology icdp
  icdp
  global-switch     true
```

<!-- page 1295 -->
