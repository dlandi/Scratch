---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 05-piped-commands
section: '5. Piped Commands'
source_lines: 3540-4012
---

# 5. Piped Commands

This section provides information and examples on how the piped commands can be used while operating the 1830 GX G30/1830 GX G40.

**Tip:** Unless stated otherwise, always assume the root command prompt [ne] for all the examples in this section.

## 5.1. begin

#### Command Description

The `begin` command is used to display the output of the previous command starting from a specified word. The output begins with the line that matches the word. It can be used together with 'until' piped command to retrieve a sub-set of an output.

#### Command Syntax

```
<command> | begin -h
<command> | begin <words>
```

#### Command Parameters

**Table 51: begin command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;words&gt; | Line to begin with. May contain spaces if in quotes. |

#### Examples

This example shows how to display system information beginning with line containing ne-location:

```
show | begin ne-location
```

<!-- page 113 -->

## 5.2. display

#### Command Description

The `display` command is used to allows to customize the output of the previous command, i.e., to display only the attributes' values or attributes' names and values. Starting from GX 9.0, CLI provides an enhancement with CLI interactive display where it uses an open source tool to display data with an interactive mechanism. You can view the data and navigate by using the keyboard and mouse and search or navigate on the data in a number of ways in order to find the required information. It is a user friendly way where in you can collapse and expand parts of the tree/ node.

#### Command Syntax

```
<command> | display -h
<command> | display <mode>
```

#### Command Parameters

**Table 52: display command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any display command such as tree or show. |
| &lt;mode&gt; | The display mode to be selected. The supported modes for the show command are:<br>• list - The default display mode. Displays the results as a standard list, one value per line.<br>• table - Displays the results in table format, where rows represent instances, and columns represent attributes.<br>• commands - Displays the configurations as CLI 'set' and 'add' commands. It is useful for creating CLI scripts from the current configuration.<br>• csv - Displays output in comma separated values format; useful for pasting into a spreadsheet. • keys-table - It is similar to the table mode, with the difference that the instance representation is split per individual keys.<br>• single-table - It is similar to the table mode, with the difference that it merges all tables into one. It is useful when retrieving the same data in multiple entities.<br>• only-values - Displays only the value of the requested attributes. It is useful for script building.<br>• xml - Displays the output in xml format (similar to NETCONF responses).<br>• json - Displays the output in json format (similar to RESTCONF response).<br>• xpath - Displays the output in xpath.<br>• interactive - displays output in JSON format in an interactive viewer using the 'jless' library. Inside jless, use 'q' or CTRL-C to leave or ':help' to obtain help. |

#### Examples

This example shows how to display CLI script from all system configurations:

```
show -r | display commands
```

This example shows how to display PM data in csv format:

```
show pm | display csv
```

This example shows how to display this entity in xml format:

```
show card-1-1 | display xml
```

This example shows how to display alarms in json format:

```
show alarm | display json
```

This example show command with pipe display interactive is used in order to display the CLI outputs in an interactive way:

```
show -r | display interactive
```

<!-- page 115 -->

## 5.3. exclude

#### Command Description

The `exclude` command is used to filter the output that contains a defined word or string (i.e., does not display output that includes a given word or string). It is a piped command to be called against the result of a previous command (separated by \|). The command will exclude lines with \<filter\> from the output. When using the `exclude` command, take into consideration the following information:

- The filter may be multiple words separated by spaces, if enclosed by quotes.
- The filter supports regex (regular expressions).
- Multiple piped commands can be used in a single command.
- Option \<filter\> will do a case insensitive comparison, e.g. the following two commands are equivalent:

#### ▪> show | exclude abc

#### ▪> show | exclude ABC

Related commands:

- `include` - filters the output that includes the filtering text.
- `grep` - filters the output that includes the filtering text (same as 'include').

#### Command Syntax

```
<command> | exclude -h
<command> | exclude <filter>
```

#### Command Parameters

**Table 53: exclude command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any normal command that generates output. |
| &lt;filter&gt; | Text to be filtered. It can have spaces if enclosed by quotes and supports regex (regular expressions). |

#### Examples

This example shows how to display the show command without any lines containing 'ne':

```
show | exclude ne
```

This example shows how to display the show command without any lines containing 'ne oper':

```
show | exclude 'ne oper'
```

This example shows how to display all leds excluding status leds:

```
show led | exclude status
```

<!-- page 117 -->

## 5.4. grep

#### Command Description

The `grep` command is used to filter the output based on a defined word or string (i.e., only displays output that includes a given word or string). Unlike `include`, the `grep` command has several additional filtering options. It is a piped command to be called against the result of a previous command (separated by \|). When using `grep` command, take into consideration the following characteristics:

- It will only display lines matching \<filter\>.
- This command is the same as the `include` command.
- Multiple piped commands can be used in a single command.
- The filter may be multiple words separated by spaces, if enclosed by quotes. Filter supports regex.
- The \<filter\> will do a case insensitive comparison, e.g., the following two commands are equivalent: **▪**`> show | grep abc` **▪**`> show | grep ABC`

Related commands:

- `include` - filters output that includes the filtering text (the same as `grep`).
- `exclude` - filters output that excludes the filtering text.
- `begin` - selects output starting from the line that includes the filtering text.
- `until` - selects output until the line that includes the filtering text.

#### Command Syntax

```
<command> | grep -h
<command> | grep [<option>] <filter>
```

<!-- page 118 -->

#### Command Parameters

**Table 54: grep command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any normal command that generates output. |
| &lt;filter&gt; | Displays help for this command. |
| &lt;option&gt; | The following options are supported for grep:<br>• -a=&lt;n&gt; - Number of lines of context to show after the actual match.<br>• -b=&lt;n&gt; - Number of lines of context to show before the actual match.<br>• -n - Displays line numbers in result. |

#### Examples

This example shows how to display only lines with 'ne' from the show command output:

```
show | grep ne
```

This example shows how to display lines from the a 1830 GX G40 tree output that include that regex:

```
tree | grep '1-.-T[4-8]'
```

<!-- page 119 -->

## 5.5. highlight

#### Command Description

The `highlight` command is used to visually markup a word or set of words in the output of a given command. This command will highlight a word in the output of the previous command. More than one word can be provided, either within quotes (to search for a sentence) or without quotes (to search for words separately).

#### Command Syntax

```
<command> | highlight -h
<command> | highlight <word> [<word>]
```

#### Command Parameters

**Table 55: highlight command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any normal command that generates output. |
| &lt;word&gt; | Any word to highlight. May contain spaces, if enclosed by quotes. |

#### Examples

This example shows how to highlight the word 'odu4' in the show result:

```
show odu | highlight odu4
```

This example shows how to highlight the words 'odu4' and 'oduflex' in the show result:

```
show odu | hightlight odu4 oduflex
```

This example shows how to highlight the sequence of words 'idle lower-layer-down' and # the word 'odu4' in the show result:

```
show odu | highlight 'idle lower-layer-down' odu4
```

<!-- page 120 -->

## 5.6. include

#### Command Description

The `include` command is used to filter the output to a defined word or string (i.e., only displays output that includes a given word or string). It is a piped command to be called against the result of a previous command (separated by \|). When using the `include` command, take into consideration the following information:

- It will only display lines matching \<filter\>.
- This command is the same as the 'grep' command.
- Multiple piped commands can be used in a single command.
- The filter may be multiple words separated by spaces, if enclosed by quotes.
- The filter also supports regex.
- \<filter\> will do a case insensitive comparison. The following two commands are equivalent: **▪**`> show | include abc` **▪**`> show | include ABC`

Related commands:

- `grep` - filters output that includes the filtering text (the same as `include`).
- `exclude` - filters output that excludes the filtering text.
- `begin` - selects output starting from the line that includes the filtering text.
- `until` - selects output until the line that includes the filtering text.

#### Command Syntax

```
<command> | include -h
<command> | include <filter>
```

<!-- page 121 -->

#### Command Parameters

**Table 56: include command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any normal command that generates output. |
| &lt;filter&gt; | Text to be filtered. It can have spaces if enclosed by quotes and supports regex (regular expressions). |

#### Examples

This example shows how to display only lines with 'ne' from the show command output:

```
show | include ne
```

This example shows how to display lines from the tree output that include that regex:

```
tree | include '1-.-T[4-8]'
```

## 5.7. linenum

#### Command Description

The `linenum` command is used to add line numbers to output of the previous command.

#### Command Syntax

```
<command> | linenum -h
<command> | linenum
```

<!-- page 122 -->

#### Command Parameters

**Table 57: linenum command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any display command such as tree or show. |

#### Examples

This example shows how to display an output with enumerated lines:

```
show | linenum
```

<!-- page 123 -->

## 5.8. more

#### Command Description

The `more` command is used to display long outputs incrementally, page by page. When using `more` command, take into consideration the following rules:

- The size of the page will match exactly the size of the CLI session window. That is, the size of the page is related with the size of the terminal, except for the serial port case, where the terminal size is fixed.
- When the page limit is reached, the information display is paused. It is resumed by pressing any key or [CTRL]+[C] to terminate the display and return to prompt. The \<enter\> shows the next line, while pressing any other key (including \<space\>, \<tab\>, etc) shows the next page.
- Multiple piped commands can be used sequentially, but `more` needs to be the last one in the command line.

#### Command Syntax

```
<command> | more [-h]
```

#### Command Parameters

**Table 58: more command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;command&gt; | Any display command such as tree or show. |

#### Examples

This example shows how to display the output of recursive show page by page:

```
show -r | more
```

This example shows how to display the output of previous 'include' command page by page:

```
tree | include led | more
```

<!-- page 124 -->

## 5.9. sort

#### Command Description

The `sort` command is used to reorder the output of a command according to specified criteria. It is a piped command that can reorder the output of a `show` command based on specified criteria. When using the `sort` command, take into consideration the following information:

- Sort without parameters will display the output sorted by object instance (same as default display).
- A list of attributes can be provided as parameters, which are used for sorting .
- If an unrecognized attribute name is used, it will be ignored.
- The -i will invert the order in the output, and can be used on its own, or together with an attribute list.

#### Command Syntax

```
<show command> | sort [-h]
<show command> | sort [-i] [<attribute>*]
```

#### Command Parameters

**Table 59: sort command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -i | Inverts the order. |
| &lt;show command&gt; | show command. |
| &lt;attribute&gt; | Any attribute name that exists in the context of the output. |

#### Examples

This example shows how to display cards sorted by instance (same as show card alone):

<!-- page 125 -->

```
show card | sort
```

This example shows how to display ports sorted by port-type:

```
show port | sort port-type
```

This example shows how to display users sorted by user-group, then by last-login date, inverted:

```
show user | sort -i user-group last-login-date
```

This example shows how to display odus in inverted order:

```
show odu | sort -i
```

## 5.10. until

#### Command Description

The `until` command is used to display the output of the previous command ending at a specified word. The output ends with the line that matches the word. It can be used together with 'begin' piped command to retrieve a sub-set of an output.

#### Command Syntax

```
<command> | until -h
<command> | until <words>
```

#### Command Parameters

**Table 60: until command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;words&gt; | Line to end with. May contain spaces if in quotes. |

<!-- page 126 -->

#### Examples

This example shows how to display the output until line containing ne-location:

```
show | until ne-location
```

<!-- page 127 -->
