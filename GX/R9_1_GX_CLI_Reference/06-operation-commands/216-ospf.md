---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.216. ospf'
source_lines: 17181-17226
---

## 6.216. ospf

#### Command Description

The `clear ospf` command is used to remove and restart an ospf-instance. This operation is asynchronous. The id of the ospf-instance needs to be provided as \<instance\>.

#### Command Syntax

```
clear [-f] ospf [instance=]<value>
```

#### Command Usage Details

**Table 518: ospf Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 519: ospf Command Flags**

| Parameter | Description |
| --- | --- |
| -f | Forces the command without confirmation. |

**Table 520: ospf Command Parameters**

| Parameter | Description | Values | Default |
| --- | --- | --- | --- |
| instance | The id of the ospf-instance needs to be provided as &lt;instance&gt;. | string | n/a |

#### Examples

This example shows how to clear an ospf instance:

<!-- page 838 -->

```
clear ospf 1
```

<!-- page 839 -->
