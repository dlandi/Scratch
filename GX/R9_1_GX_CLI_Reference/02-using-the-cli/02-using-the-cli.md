---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 02-using-the-cli
section: '2. Using the Command Line Interface (CLI)'
source_lines: 1826-2990
---

# 2. Using the Command Line Interface (CLI)

This chapter provides general information to operate with the 1830 GX CLI and how to configure the CLI attributes.

## 2.1. Prerequisites

This section provides the user, system and hardware/software requirements in order to work properly with 1830 GX CLI. **User requirements** To work with 1830 GX, the user is required to have the following knowledge:

- Electrical, mechanical safety regulations and laser safety precautions as described in the *1830 GX* *G30 Hardware Installation Guide*.
- Optical system, protocols and specific 1830 GX functions as described in the *1830 GX G30 Hardware* *Description Guide*.
- Using a mouse/keyboard and SSHv2 functions.

**System requirements** The 1830 GX must have its hardware installed (racks, shelves and cards), cabled, and powered according to the *1830 GX G30 Hardware Installation Guide* instructions.

## 2.2. Launching a CLI Session

To launch the initial CLI session, the user creates a user name and password.

### 2.2.1. Opening a CLI Session using SSH

This section describes accessing the Command Line Interface on a standard SSH port. The section assumes that you have followed the instructions in the appropriate installation and configuration manual to configure the network element chassis, make a serial connection, configure the CLI terminal, power up the network element and perform initial commissioning.

With the serial connection in place, open a Microsoft Windows Command window.

**Step 1.**

Upon the first login, a new user is created. The created user has security administrator (SA) and

**Step 2.**

network administrator (NA) privileges. At the command prompt, enter the following command:

```
ssh -tt user1@deviceip
```

For more information about users and user-groups, see the *1830 GX Management Interfaces User* *Guide*. Example:

```
ssh -tt sysadmin@192.168.1.1
```

At the password prompt, set and confirm the password for this new user account. This password

**Step 3.**

can have from one to 200 characters and must not be blank. Using the ssh subsystems doesn't allocate a tty, so it is recommended that ssh session explicitly

**Step 4.**

requests for tty allocation. Using the command line ssh, this is done with the -t parameter:

```
ssh -tt sysadmin@deviceip
```

<!-- page 64 -->

The direct configuration mode prompt appears.

```
secadmin@GX>
```

At the prompt, enter `configure <cr>` to enter Candidate Configuration Mode. The Candidate

**Step 5.**

Configuration Mode prompt appears:

![Figure from page 64](images/figure-p64-2.png)

The Command Line Interface (CLI) is available via the NE's Ethernet port (**Eth1**) (for example, see Figure 5: 1830 GX G31 chassis - Location of the Eth1 port (p. 64), Figure 6: 1830 GX G32 chassis - Location of the Eth1 port (p. 64) and Figure 7: 1830 GX G34c chassis - Location of the Eth1 port (p. 65) for 1830 GX G30 nodes). For more information about 1830 GX startup procedures, refer to  *1830 GX Commissioning* *Guide*.

**Figure 5: 1830 GX G31 chassis - Location of the Eth1 port**

![Figure from page 64](images/figure-p64-1.png)

**Figure 6: 1830 GX G32 chassis - Location of the Eth1 port**

**Eth1/DCN port**

<!-- page 65 -->

![Figure from page 65](images/figure-p65-1.png)

Running a CLI session via **Eth1** port is only possible via an SSHv2 session.

**Note:** The **Eth1** port must be configured first.

The 1830 GX supports a maximum of 100 CLI sessions running simultaneously. The sessions can belong to the same user or different users. Each CLI session has a maximum dimension of 1000 lines and 4000 columns. To launch a CLI session via SSH using PuTTY, complete the following steps:

Connect an Ethernet cable between the laptop and the **Eth1** port of the desired 1830 GX chassis

**Step 1.**

main controller. Launch an SSH client such as PuTTY (refer to www.putty.org for further details).

**Step 2.**

Configure the SSH client with the following settings:

**Step 3.**

| IP address: | 10.220.227.208 |
| --- | --- |
| Port: | 22 |
| Remote character set: | UTF-8 |

Start the SSH client session. The CLI Login prompt is displayed.

**Step 4.**

Enter the user credentials.

**Step 5.**

**Note:** The user credentials must be created upon the first login.

<!-- page 66 -->

**Note:** When the user's password expires, the system reports the CREDENTIAL-AGING alarm and in the next login to the system, the user is requested to provide a new password. For example:

```
Connecting user administrator from 10.13.11.90:65168
Password has expired for user administrator, please input a new one!
Please provide the old password:
Please provide the new password:
Please confirm the new password:
****************************************** Warning ******************************************
This system is restricted to authorized users for business purposes. Unauthorized access is a
violation of the law. This service may be monitored for administrative and security reasons.
By proceeding, you consent to this monitoring.
*********************************************************************************************
```

The user's password expires when:

    - the password aging time is exceeded (by default, 90 days)
    - during Basic Commissioning, the system's clock is updated with a new date exceeding the password aging (by default, 90 days).

The CLI warning banner and the CLI path banner are displayed as in the following example:

```
****************************************** Warning ******************************************
This system is restricted to authorized users for business purposes. Unauthorized access is a
violation of the law. This service may be monitored for administrative and security reasons.
By proceeding, you consent to this monitoring.
*********************************************************************************************
Last login: 2021-10-01T13:04:07Z
[ ne ]
administrator@GX>
```

### 2.2.3. Closing a CLI Session

Any CLI session can be closed by the user in one of the following ways:

- Manually, when user terminates its own session by: Explicitly closing it with the exit command; Pressing Ctrl+d on an empty CLI prompt (same behavior as Linux shell); Abruptly interrupting the connection (for example, by closing the CLI window, or by losing network connectivity to the NE).
- Manually, when an administrator user (that has particular security management privileges) kills an existing session - even if not its own (true for both CLI and other protocol sessions).

Any CLI session can be closed automatically in one of the following ways:

- when session remains idle for long enough got the inactivity timer rlspdr (this timer is configurable per user)
- when system or management application is going to restart (for example, due to a controller warm start).
- upon controller switch-over.
<!-- page 67 -->

### 2.2.4. CLI Prompt

The CLI provides an interactive prompt. The prompt provides the current navigation context and login information and it is used to guide the user along the managed entity hierarchy. The prompt uses the following syntax:

`[Level]` refers to the current navigation context.

```
[Level]
```

`username` refers to the name of the user logged in to the CLI.

```
username@host>
```

`host` refers to the NE name. `@` is the symbol that separates the username and the host. `>` or `#` is the character indicating the current configuration mode:

    - `>` for normal/default mode.
    - `#` for candidate configuration mode.

**Tip:** At login, the current navigation context is always [ne], for example:`[ ne ] administrator@GX>`

### 2.2.5. CLI Configuration Attributes

Table 16: CLI configuration attributes (p. 67) lists all the CLI configuration attributes. These attributes are accessed under the “/NE/System/Protocols/CLI/CLI-Session-\<IP:Port\>-Config” managed entity hierarchy.

**Table 16: CLI configuration attributes**

| Attribute | Type | Value Range/ Value | Description |
| --- | --- | --- | --- |
| cli-columns | uint16 | [80... 4000] | Configurable number of columns to be used for display. The default value is 80. |
| cli-lines | uint16 | [10... 1000] | Configurable number of rows to be used for display before pausing the output. After pausing, pressing [SPACEBAR] will resume display. The default value is 40. |
| interactive-mode | boolean | true or false | Determines if the CLI shall issue interactive prompt (e.g., for prompting additional information, or for confirmation of user-initiated actions). This attribute can be set to:<br>• true (default value) = CLI will prompt user.<br>• false = CLI will suppress any prompt to the user. This parameter is set per CLI session and it is not persistent. |

For the CLI commands used to set or show the configuration of the Command Line Interface (CLI) session attributes, refer to cli-session-config (p. 313).

### 2.2.6. CLI Command Line Editor

The CLI provides a user friendly Command Line Editor. Some of the features supported by CLI Command Line Editor are described in the following sections.

<!-- page 68 -->

#### 2.2.6.1. CLI Command Line Cursor Movement

Use the keyboard shortcuts listed below to move the command line cursor.

**Table 17: CLI Command Line Cursor Movement**

| Shortcut command | Cursor Action |
| --- | --- |
| Left arrow | Moves the cursor backward one character |
| Right arrow | Moves the cursor forward one character |
| Backspace | Erases the character to the left of the cursor |

#### 2.2.6.2. Standard Keyboard Operations

Certain keys or key combinations can be used to perform standard operations. Table 18: Standard keyboard operations (p. 68) lists the supported keyboard operations.

**Table 18: Standard keyboard operations**

| Key or Key combination | Action |
| --- | --- |
| [any] | To continue display after the auto-pause when the line limit is reached, i.e., when the content occupies more than one page. |
| [TAB] | Auto-completion of current identifier if a unique identifier can be found. The identifier can be a command, a command option, a container-name, or an attribute. |
| [SPACEBAR] | Resumes the display after the maximum number of displayed lines per pages is reached. |
| [UpArrow] | Cycles through the command history buffer in an ascendant order. |
| [DownArrow] | Cycles through the command history buffer in a descendant order. |
| !!+[ENTER] | Repeats the last command. |
| !&lt;n&gt;+[ENTER] | Repeats a given command in the command history buffer. &lt;n&gt; corresponds to the order position in the history buffer. |
| [CTRL]+[C] | Terminates the current display when auto-paused or empties the current command line (^C is displayed). |
| [CTRL]+[D] | Automatically closes the current CLI session. |
| History+[ENTER] | Displays a numbered list of commands in the command history buffer (per CLI session). |
| cls+[ENTER] | Clears the screen (console window). All previously displayed input and output are cleared and the prompt is placed at the top of the page. |
| \| | The output of the first command serves as the input of the second command. It is used in piped commands. Example: show card \| grep up |
| * | Wild card key. Mainly used to represent multiple entities or attributes at the same time when using the “show”, “set”, or “delete” commands. Examples:<br>• as replacement of a single instance key: port-1-*<br>• for auto-completing attribute names: admin* instead of admin-status |
| [ ] | To represent ranges (card-1-[5..7]) or lists (interface-[CRAFT,DCN]) of manageable entities. |
| = | Used in conjunction with the “show” command provides a filtering function (a=b). Examples:<br>• to show all cards with “admin-state unlock”: show card admin-state=unlock<br>• to exclude a column name from a log table: show log-file \| exclude=&lt;column-name&gt; |
| \ | Used to escape special characters. Example: password old-password='Infinera4u#' new-password=N3w\'Password\' |
| ; | Used to separate multiple commands. Example: show card ; show port |
| ' and " | Limits the boundaries of a string but can also be used within a string. Examples:<br>• to limit a string: password old-password=Infinera4You! new-password='New4All#'<br>• to be used within a string: password old-password=Infinera4You! new-password=New"4"All |
| # | Commenting symbol. All text string that is typed afterwards is considered a comment. Useful while creating complex scripts. Tip: This symbol can also be used as a normal character (e.g., while defining a password or configuring an attribute). When used as character include single (') or double (") quotes at the beginning and end of the string. Example: set password "My#pass1234" |
| \n | To represent a line break in a string. Useful while creating complex scripts. |

**Note:** The 1830 GX G30/1830 GX G40 CLI also provides support for partial commands and identifiers, when only one option exists. For example, the `show user` command can be replaced by the short form `sho use`. Each command is automatically completed if there is only one option.

#### 2.2.6.3. CLI Input Command Format and Usage

The input command is issued by a user to the network element in order to accomplish a specific task. The command syntax consists of the command name, keywords, and/or argument(s). Using the CLI, a user has the ability to:

- Input commands character by character, or by copy/paste.
- Push multiple lines of text (representing multiple commands) via copy/paste. There is no specific limit on the size of the pasted text.
- Input commands with indefinite line length (minimum that the system should accept is 1k characters per command).
- Include comments in commands; everything after the character # is considered as a comment. The comment character is not configurable. In order to insert that character as part of a value, enclose the value in quotes.

Support for command line navigation and command execution includes:

- Left/right to navigate one character.
- Ctrl+left/right to navigate one word.
- Multiple commands can be accepted in a single line if separated by semi-colon (';') . An indefinite number of commands can be inputted this way.
- Command processing is independent; in the example "\<cmd1\> ; \<cmd2\>", \<cmd2\> is executed even if \<cmd1\> fails.
- Previous command can be repeated with '!!'.
- Last command starting with \<cmd\> can be repeated with !\<cmd\> .
- Home/End to go to beginning or end of command.
- At any point while entering a command, pressing Control+C prints ^C and resets the command line. For canceling ongoing commands.
<!-- page 71 -->

See Standard Keyboard Operations (p. 68) for the complete list of standard keyboard operations.

**Table 19: CLI Input Command Format**

| Parameter | Description |
| --- | --- |
| command name | Every CLI command must begin with a command name followed by up to two keywords and arguments if any. The command name defines the action to be performed such as showing information, configuring an option, etc. |
| keyword | Keywords define the object to which the command name (verb) is applied. They are also used to specify the application of the command verb. There can be one (primary) or more (secondary and tertiary) keywords. Based on the command, the keyword(s) may be a required or an optional parameter. |
| mo | The equipment / termination point identifier designates the name of an equipment or termination point within the network. It identifies the specific network entity being addressed by the command. It can consist of up to twenty (20) alphanumeric characters used to describe the exact identity of this network entity within the system. The valid alphanumeric characters include Letters (a to z\|A to Z). Spaces are not allowed. Based on the command, the equipment / termination point identifier may be a required or an optional parameter. Examples of valid equipment / termination point identifiers include pem, fan, tom etc. |
| AID | Access Identifier The AID parameter addresses a particular entity such as hardware, software, facility, etc. within a network element or terminating on the network element. The AID can be used to identify particular termination points, circuit packs, units, or sub-assemblies within the addressed network element. See 1830 GX Management Entity AIDs (p. 43) for the description of AIDs defined in network element. |
| argument | Arguments are place-holders in the command, for parameters that require user input. Arguments can be configurable parameters, identifiers for entities etc. Based on the command, the argument may be a required or an optional parameter. Examples of valid CLI arguments include ‘configurable parameters’ such as tributary-disable-action. |
| argument value | Argument values are user-entered values for arguments. User-selected values are those that are chosen and entered by the user, from a list of argument values displayed by the system. Example of valid CLI user-entered/defined argument values are SendLF. User-defined argument values are those that are defined and entered by the user, when the system does not display a list of argument values to select from. Example of a valid 1830 GX G40 CLI user-entered value is super-channel-group-1-4-L1. All argument values are case sensitive. |
| terminator | Commands are processed when &lt;enter&gt; key is pressed, or in copy/paste mode, when a new-line is detected. |

**Note:** CLI command names, keywords, arguments and user-entered argument values (that are displayed using the \<`Tab`\> key or `?`) are case-insensitive and can be entered in upper case, lower case or mixed cases. AIDs and user-defined argument values (that are not displayed using the \<`Tab`\> key or `?`) are case-sensitive.

#### 2.2.6.4. CLI Syntax Conventions and Notations

Table 20: CLI Syntax Conventions and Notations (p. 72) gives a description of notations, symbols, and conventions used when describing CLI command responses or CLI command formats.

<!-- page 72 -->

**Table 20: CLI Syntax Conventions and Notations**

| Convention | Notation |
| --- | --- |
| &lt;cr&gt; | Carriage Return |
| [a] | The keywords/arguments enclosed in square brackets [ ] are optional. Example: In the show interfaces ethernet &lt;ethernet identifier&gt; [client\|line] command, specifying the type of interface (client/line) is optional. Note: The values entered are case sensitive. |
| &lt;a&gt; | The keywords/arguments enclosed in angle brackets &lt;&gt; are mandatory. Absence of a bracket also indicates that the keywords/argument values are mandatory. Note: The values entered are for each of the parameters are case sensitive. |
| [&lt;&gt;] | The arguments enclosed in [&lt;&gt;] are mandatory or optional, depending on the value of keywords and other arguments in the command. Unless mentioned otherwise, not more than one of these arguments in [&lt;&gt;] can be specified in a single command. But, when an argument is specified, the corresponding value is mandatory.\ |
| Regular (non-italic) text | Regular text indicates literally entered command names, keywords, arguments and argument values. |
| italics | Text in italics indicates user-defined values for arguments. |

#### 2.2.6.5. CLI Command Line Shortcuts

The Nokia CLI supports several shortcuts, shown in Table 21: CLI Command Line Shortcuts (p. 72). To execute the shortcut action, enter the shortcut command at the command line prompt.

**Table 21: CLI Command Line Shortcuts**

| Shortcut commands | Action |
| --- | --- |
| “Command &lt;Tab&gt; key” | Lists the command and the command syntax. |
| “&lt;first letter of the command&gt; &lt;Tab key&gt;” | Lists all the commands that match with the first letter typed. |

#### 2.2.6.6. CLI Command History Buffer

CLI stores a per-session history of entered commands, and the ability to navigate between history commands with up/down keys. There is also the ability to visualize all commands in history. Each command is numbered and the last 500 commands are displayed. You can repeat a command from the history with !n , where n is the command number according with the history command list. Inputted commands with sensitive data (passwords, etc) that are automatically obfuscated will appear in history in its obfuscated format.

**Note:** The history buffer is associated with a given CLI session. History information is lost once a user logs out of CLI session.

#### 2.2.6.7. CLI Command Output

For CLI commands that are successful, and do not provide any useful response, no output is displayed. As such, the absence of an error message implies the command was successful. Examples of such commands include: add, set, delete.

<!-- page 73 -->

Other commands provide a useful response when successful. These commands provide output to the user, which should be optimized depending on the command itself. Examples of such commands include: show, and certain RPC-derived commands. Failed commands will have an explicit error message, in the format "ERROR: \<msg\>". The \<msg\> may be command specific, and should be specified on a per command/application basis. It is possible to chain commands by using the pipe ('\|') symbol between them, in which case the output of one command will be the input of the next (similar to Linux shell). Commands that can be used as piped commands do not necessarily make sense as primary commands, so they have a special category. Example: show interface \| sort if-type \| display xml would show all interface objects, sort them by if-type attribute, and finally display the result as XML instead as the normal CLI output. The supported piped commands are style specific, and are defined in dedicated requirements. If a command generates a useful response (for example, like the command show), the output content will be displayed in the default format as defined by the command. However, it is possible to transform the output into another format using the display piped command. The following are the supported formats that can be used against YANG data (meaning, against command results that are modeled as YANG data):

- Human optimized formats, including list (one attribute/value pair per line), or table (multiple objects organized in a table, with each attribute being a column, and each object instance being a row).
- Machine formats, including xml, json, csv and xpath.
- Commands format, which outputs results as a list of CLI commands, copy/paste ready.

**Note:** The most relevant command to apply this output transformation to is the show command, which typically provides YANG data as a result.

#### 2.2.6.8. CLI String Support

This section describes the 1830 GX G30/1830 GX G40 CLI string support.

##### 2.2.6.8.1. Input of String Values

String values are directly typed, just like any other non-string attributes. For example, the command `set` `ne-name NE1` sets the attribute `ne-name` to the string `NE1`. However, in the 1830 GX CLI, there are characters that require a special input mechanism when used in strings. Table 22: Characters usage in strings (p. 73) lists all the characters that can be used in strings.

**Table 22: Characters usage in strings**

| Character | Description | When used in a string value | Example (set &lt;attribute&gt; [...] ) |
| --- | --- | --- | --- |
| Alphanumeric | Normal [A-Za-z0-9] characters. | no special mechanism |  |
|  |  |  | mystring |
| Space /Tab | Space or tab characters. | enclose with quotes |  |
|  |  |  | 'my string' |
|  |  |  | "my string" |
| # | Hash character. Used elsewhere in a CLI command, it marks the | enclose with quotes |  |
|  |  |  | 'my#string' |
|  | start of a comment (for example:show # this is a comment) . |  | "my#string" |
| ; | Semi-colon character. Used elsewhere in a CLI command, it separates multiple commands (for example:show card ; show port). | enclose with quotes |  |
|  |  |  | 'my;string' |
|  |  |  | "my;string" |
| \| | Pipe character. Used elsewhere in a CLI command, it is considered as a pipe, sending output of the first command to the second command (for ex: show card \| grep up). | enclose with quotes |  |
|  |  |  | 'my\|string' |
|  |  |  | "my\|string" |
| ? | Question mark character. Used elsewhere in a CLI command, it will be considered as a request for contextual help. | enclose with quotes |  |
|  |  |  | 'my?string' |
|  |  |  | "my?string" |
| ' and " | Quote characters (single and double). Used normally to limit the boundaries of a string, but can be used within the string also. | escape the characters with \ |  |
|  |  |  | my\'string |
|  |  |  | my\"string |
|  |  | enclose with quotes of different type (python like) |  |
|  |  |  | 'my"string' |
|  |  |  | "my'string" |
| \ | Slash character. Used normally to escape special characters. | escape the characters with \ |  |
|  |  |  | my\\string |
| \n | New line character; creates multiline strings. | represent as \n |  |
|  |  |  | my\nstring |
| !$%&()*+,-./:&lt;=&gt;@[]^ `{}~ _ | Other symbol characters. | no special mechanism |  |
|  |  |  | my_-!$ %&()*+,./:&lt;=&gt;@[]^`{}~str ing |

<!-- page 75 -->

**Note:** When enclosing with quotes:

  - Single quoted strings can contain double quotes and double quoted strings can contain single quotes.
  - Any string can be enclosed with single/double quotes, even if not needed (example: 'abc').

**Note:** When escaping with \:

  - Only a few specific characters may be escaped with \ : **▪**\\ **▪**\' **▪**\" **▪**\(white space) **▪**\# **▪**\; **▪**\\| **▪**\? **▪**\\* **▪**\n (literal new line) **▪**\t (literal tab) **▪**\h (represents encoded start of hashed/encoded value)
  - If \ is used to escape a character that does not support it, it results in an error.

###### 2.2.6.8.1.1. Special Case: Password Input

A password is typically a string containing alphanumeric characters and symbols. As such some of the special characters listed above can be used in passwords.

- In the following example an old password *Infinera4u#* will be changed to a new one *N3w’Password’*.

```
password old-password='Infinera4u#' new-password=N3w\'Password\'
```

###### 2.2.6.8.1.2. Special Case: String in Keys

In YANG, a key is an identifier for an instance in a list. For example, in the instance “user-administrator”, “user” is the object, and “administrator” is the key representing the user-name. The keys can be of multiple types, including strings. The restrictions for key strings is different from normal attribute strings, in which most symbols are not supported. In keys, the list of supported characters is limited to alphanumeric characters, plus underscore (\_) and dash (-), in regex [A-Za-z0-9\_\-]\*. Some examples:

```
add user-mr_x
add snmp-target-x-y
```

<!-- page 76 -->

##### 2.2.6.8.2. Visualization of String Values

The display of strings in the CLI depends on the type of visualization. The following example is a test string literal, including slash, single quotes, hash and a new line:

```
My Location \ '#123'
here
```

Table 23: Visualization types (p. 76) lists all the visualization types and some application examples.

**Table 23: Visualization types**

| Type | Description | Example |
| --- | --- | --- |
| list view | Default view for the “show” command:<br>• Strings are enclosed with single quotes;<br>• Characters are not escaped;<br>• New line is resolved, and the string is displayed in multiple lines. |  |
|  |  | &gt; show ne-location |
|  |  | ne ne-location 'My Location \ '#123' here' |
| table view | View when displaying multiple object instances, or when “show -t” is used.:<br>• Strings are not enclosed in quotes;<br>• Characters are not escaped;<br>• New line is resolved, and the string is displayed in multiple lines. |  |
|  |  | &gt; show -t ne-location |
|  |  | ne ne-location -- -------------------------- ne My Location \ '#123' here' |
| as commands | View of configurations as CLI commands, by using “\| display commands”:<br>• Strings are enclosed with single quotes;<br>• Characters are escaped;<br>• New line is displayed as “\n”. The result is a valid CLI command (copy/paste ready). |  |
|  |  | &gt; show ne-location \| display commands set -f ne ne-location 'My Location \\ \'#123\'\nhere' |
| as xml | Typical view in NETCONF and RESTCONF. Can be viewed in CLI by using “\| display xml”. Encoded according to normal NETCONF xml rules:<br>• Strings are not enclosed with single quotes;<br>• Characters are not escaped;<br>• New line is resolved, and the string is displayed in multiple lines. The result is a valid xml. |  |
|  |  | &gt; show ne-location \| display xml &lt;?xml version="1.0" encoding="UTF-8" standalone="yes"?&gt; &lt;ne xmlns="http:// infinera.com/yang/os/ne"&gt; &lt;ne-location&gt;My Location \ '#123' here&lt;/ne-location&gt; &lt;/ne&gt; |
| as json | Typical view in RESTCONF. Can be viewed in CLI by using “\| display json”. Encoded according to normal RESTCONF json rules (see string encoding in http://json.org): |  |
|  | • Strings encoded as json string and enclosed with double quotes;<br>• Some characters are escaped, according to json rules;<br>• New line is displayed as “\n”. The result is a valid json. | &gt; show ne-location \| display json { "ne:ne": { "ne-location": "My Location \\ '#123'\nhere" } } |
| as csv | Comma Separated Values (CSV) display, can be viewed in CLI by using “\| display csv”:<br>• Strings are not enclosed by quotes, unless they contain the separator character “;” ;<br>• Characters are not escaped;<br>• New line is displayed as “\n”. |  |
|  |  | &gt; show ne-location \| display csv container;attribute;value;unit ne;ne-location;My Location \ '#123'\nhere; |
| as xpath | XPath representation of data can be visualized in CLI by using '\| display xpath':<br>• XPaths are presented per attribute in output;<br>• XPaths are simplified, e.g. they do not contain XML/YANG namespaces;<br>• Values appear after the xpath, separated by spaces. |  |
|  |  | &gt; show ne-location \| display xpath |
|  |  | /ne /ne/ne-location "My Location '#123'\nhere" |

### 2.2.7. CLI Help

The 1830 GX CLI has three mechanisms related with providing help information to the user:

- **Auto-complete \<tab\>:** can be triggered in the middle of a command to provide the available options.
- **Contextual Help '?':** can be used in the middle of a command to provide the available options and some related information.
- **Help command:** can be used to display information on commands, objects or attributes.

The CLI provides a help command that can be used to retrieve multiple types of information. The Table 24: Help Information Types lists the types of information that can be displayed.

**Table 24: Help Information Types**

| Type of Information | Provided Information | How to obtain the information |
| --- | --- | --- |
| Generic CLI Help | Overview of the CLI | help command without parameters |
| CLI commands | • Description of command<br>• Command Syntax<br>• Use-cases where command is useful<br>• Description of parameters • Generic tips associated with the command<br>• Examples | help &lt;command&gt; or &lt;command&gt; -h |
| YANG Attributes | Same as YANG Objects Data type information (possible values, integer ranges, etc) | help &lt;object&gt; &lt;attribute&gt; |
| YANG Objects | Description text taken from YANG Metadata details taken from YANG (is config or state, etc) Precondition rules | help &lt;object&gt; |

**Note:** Standard auto-complete of help command parameters is supported.

**Note:** Unless stated otherwise, always assume the root command prompt [ne] for all the examples in this section.

**Short Help at Login** Upon login, CLI provides a short help text. Press '\<tab\>' for a list of available commands and '[cmd] -h' for help on a specific command.

#### 2.2.7.1. Auto-complete

This function behaves as the standard Windows or Linux CLI auto-complete functionality. At any point while typing a command, \<tab\> can be pressed to retrieve the possible options at the current prompt location. In summary:

- \<tab\> in an empty command line, retrieves the CLI top level commands.
- \<tab\> in a non-empty line, after a space (e.g., at the beginning of a new word), retrieves available options which include all the possible keywords at that point.
- \<tab\> in a non-empty line, after a space and a dash (“ -”), retrieves flag options for that command (e.g., -f or -a).
- \<tab\> in a non-empty line, after a non-space character (e.g., in the middle of a word) retrieves options that start with the beginning of the current word.
- \<tab\> in a non empty line, after a pipe character ('\|'), retrieves options of all piped commands.

As a consequence of the auto-completion, the following may happen:

- If no auto-complete options exist, no output is provided, and the bell character is echoed, to signify 'no available options.' The bell character itself is not visible (although some ssh clients can be configured to react visually to the bell). If the next word does not support auto-complete (e.g., when the next word is expected to be an integer or string attribute value), no output is provided.

**Tip:** Some values support auto-complete, like enumerations or instance-identifiers.

- Not all scenarios have valid auto-complete options; for example, if the next keyword is expected to be an integer or a free string value, no options are provided.
<!-- page 79 -->
- If a single option exists, the option itself is auto-completed in the command line, followed by a space.
- If multiple options exist: if they all have a common beginning, the common part is auto-completed. if they do not have a shared prefix, they are displayed as a list below, where each option is grouped into categories; the list is wrapped across multiple lines if needed.
- After the list, the CLI returns to the prompt with the same command line content.

#### •

The \<tab\> auto-complete request is cached, to prevent the system getting blocked during multiple TAB requests. The initial \<tab\> completion request may take few seconds to retrieve the data, while the subsequent requests return results instantaneously from the cache by reducing the response time.

#### •

The \<tab\> process can now be interrupted by pressing **Ctrl+C** or by continuing to type while the response is being processed. This will unblock the CLI prompt so that the user can continue writing the command if user wants the auto-complete the result, they wait for the output without interrupting.

When auto-completing values of attributes while editing objects (creating or updating), when multiple options exist for the value, the current value (in the database) is highlighted with a \* character. For example, for the boolean ztp-enabled flag: \> set ztp-enabled \<tab\> Value completion: false true\* In this case, both true and false are possible values, but true is the current value in the database. The following example shows the usage of \<tab\> for command auto-complete:

```
% command auto-complete
> <tab>
Commands completion:
  activate             add                  alias                call-home            cancel-upgrade
  cert-gen             change-ztp-mode      clear                cls                  convert
  default              delete               download             edit                 exit
  expect               export               file                 gshell               help
  history              kill-session         lock                 password             ping
  prepare-upgrade      protection-switch    restart              run                  set
  set-alarm-state      set-time             shell                show                 simulate
  sleep                ssh-keygen           swversion            take-snapshot        terminate
  time                 top                  traceroute           tree                 unalias
  unlock               up                   upgrade-status       upload               uptime
  validate
```

The following example shows the usage of \<tab\> for word auto-complete:

```
% new word auto-complete
> show <tab>
Attribute completion:
  alarm-report-control            alarm-report-ready              altitude
  avail-state                     clli                            contact
  equipment-discovery-ready       label                           latitude
  longitude                       ne-id                           ne-location
  ne-name                         ne-site                         ne-sub-location
  ne-type                         ne-vendor                       node-controller-chassis-name
  oper-state
Keyword completion:
  alarm     config    log       pm        script    stats
Object completion:
  equipment              facilities             protection             services               system
  system-capabilities    topology
```

The following example shows the usage of \<tab\> for option auto-complete:

```
% option auto-complete
> show -<tab>
Option completion:
  -a    -c    -d    -h    -l    -n    -o    -r    -s    -t    -x
```

<!-- page 80 -->

The following example shows the usage of \<tab\> for multi-option current word auto-complete:

```
% multi-option current word auto-complete
> show ne<tab>
Attribute completion:
  ne-id              ne-location        ne-name            ne-site            ne-sub-location
   ne-type
  ne-vendor
Object completion:
  ne            netconf       networking    next-hop
```

The following example shows the usage of \<tab\> for multiple instance next word auto-complete:

```
% multiple instance next word auto-complete
> show chassis-1 slot-1<tab>
Object completion:
  slot-1     slot-10    slot-11
```

The following example shows the usage of \<tab\> for single option auto-complete:

```
% single option auto-complete, the prompt is completed in the same line
> set ne-loca<tab>
> set ne-location
```

The following example shows the usage of \<tab\> auto-complete of word that represents a string value:

```
% auto-complete of word that represents a string value
> set ne-location Lisb<tab>
> set ne-location Lisb  %nothing happens
```

The following example shows the usage of \<tab\> auto-complete of of invalid keyword:

```
% auto-complete of invalid keyword
> xpto
> xpto    %nothing happens
```

#### 2.2.7.2. Contextual Help

The contextual help and the auto-complete are very similar, however:

- Contextual help is triggered with the '?' key and provides useful information while writing a command. The contextual help and the auto-complete are very similar except auto-complete is triggered with the \<tab\> key and the contextual help is triggered with the '?' key.
- Contextual help shows all options (valid and invalid), and includes additional information on each option, while the auto-complete shows possible valid options. If there is only one option, auto-complete auto fills-in that option in the current command, while contextual help displays information on it.

**Tip:** The '?' is used for triggering the contextual help, however it can be also used as part of string values. In such cases, the '?' character does not trigger the contextual help.

##### 2.2.7.2.1. CLI Commands

For CLI commands, it retrieves a short description based on the YANG model. The following example shows the contextual help when typing part of a CLI command:

<!-- page 81 -->

```
> a?
Commands completion:
  activate    Activates an entity to take effect for the system
  add         Creates a new managed entity
  alias       Defines an alias to represent one or more commands
```

##### 2.2.7.2.2. CLI Command Flag Options

For CLI command flag options, it retrieves a short description of what each option does. If the option accepts values (e.g., it is not a simple boolean flag; example: -r option in show command) a description of the attribute type is displayed. The following example shows the contextual help for the flag option of CLI commands:

```
> show -?
Option completion:
  -a    display the attributes only (no container instances)
  -c    display the container instances only (no attributes)
  -d    include attributes default information
  -h    display help text
  -i    filter by chassis id
  -l    show long format
  -o    display state/read-only attributes
  -r    operate the command recursively for n levels down
  -s    display content in secure mode (no passwords showed)
  -t    display information in tabular format
  -x    display configuration/read-write attributes
> show -s?
Option completion:
  -s    display content in secure mode (no passwords showed)
```

##### 2.2.7.2.3. MO Keywords

For MO keywords, it retrieves the MO description and a separate line defining the MO representation, including all keys and each key's type. The following example shows the contextual help for for MO keywords:

```
> add card?
Object completion:
  card-    Description
          Card base object.This object has parameters that are common to all
           existing card types (controller, fan, etc).
          Syntax:
          add card-<name>
> set system networking interface?
Object completion:
  interface-1-AUX-1/    The list of configured interfaces on the device.
  interface-CRAFT/      The list of configured interfaces on the device.
  interface-DCN/        The list of configured interfaces on the device.
  interface-LO-MGMT/    The list of configured interfaces on the device.
```

<!-- page 82 -->

##### 2.2.7.2.4. Attribute Keywords

For attribute keywords, it retrieves the attribute description. Note the distinction between providing contextual help for the attribute name, versus the attribute value in the example below. The following example shows the contextual help for attribute keywords:

```
> show ne-?
Attribute completion:
  ne-id              Unique identifier of the NE defined by the system.
  ne-location        Name of the location of this particular NE.
  ne-name            User assigned name for this NE.
  ne-site            Name or CLLI of the site where this NE is located.
  ne-sub-location    Name of the secondary location of this particular NE.
  ne-type            Type of the NE.
  ne-vendor          Vendor name of the NE.
> add card-1-1 ?
Attribute completion:
  admin-state             The administrative state of the managed object.
  alarm-report-control    Controls the reporting of alarms for this particular
                          object.
  alias-name              User defined alias for this entity.
  chassis-name            Chassis where this card is located.
  label                   User label.
  required-type           Required card type.
  slot-name               Slot where this card is located.
```

##### 2.2.7.2.5. Values

For a value (or expected value), it retrieves the value type, possible values, etc. The following examples shows the help for values:

```
> set card-1-5 admin-state ?  % provides information on the possible attribute values (for enumeration)
Value completion:
  lock
  maintenance
  unlock
> set ne-location ?   % provides information on the attribute type
Value completion:
  String    (length 0..256)
> set altitude ?   % provides information on the attribute type
Value completion:
  Number    (meters)
> set altitude 4?   % ? is ignored for integer values.
>set ne-location Lisbo?   % ? is ignored.
> add user-?    % provides information on the key for user MO (user-name attribute) and command syntax.
Object completion:
  user-    Description
          User details. Can represent both locally configured users, as well
           as temporary remote users.
          Syntax:
          add user-<user-name>
> show user-?    % provides possible options for user MO completion (user-name attribute).
Object completion:
  user-admin/      User details. Can represent both locally configured users,
                   as well as temporary remote users.
  user-group       List of user groups, each one with its own access
                   permissions.
  user-na-user/    User details. Can represent both locally configured users,
                   as well as temporary remote users.
> add user-claudia user-group ?  % provides information on all the possible attribute values.
Value completion:
  EA
  MA
  NA
  NE
  PR
  SA
  TT
```

#### 2.2.7.3. Help Command

The CLI provides the `help` command to retrieve information on commands, objects or attributes. Table 25: Help command - Types of Information  (p. 83) lists all the type of information that can be retrieved using the `help` command.

**Table 25: Help command - Types of Information**

| Type of information | Provided information | How to obtain information |
| --- | --- | --- |
| Generic CLI help | Overview of CLI | help (without parameters) |
| CLI commands | • Description of command<br>• Command Syntax (BNF like)<br>• Use-cases where command is useful<br>• Description of parameters Generic tips associated with the command Examples | help &lt;command&gt; or &lt;command&gt; -h |
| YANG Objects | • Description text taken from YANG<br>• Metadata details taken from YANG (is config or state, etc)<br>• Precondition rules | help &lt;object&gt; |
| YANG Attributes | • Same as YANG Objects<br>• Data type information (possible values, integer ranges, etc) | help &lt;object&gt; &lt;attribute&gt; |
| System Features/Functions | • Generic description of a system feature from a user point of view. Includes references to common use-cases, useful commands, examples (examples of system features that might apply: 'software-upgrade', or 'chm6') | help &lt;topic&gt; |

**Tip:** This command supports auto-complete, to all existing keywords available in the current context. This includes all commands, all MOs and attributes in the current MO.

#### 2.2.7.4. Searching the CLI Help

It is possible to use the help command to search for commands that are related with specific search terms by using the following command:

<!-- page 84 -->

```
help -s <keyword > [<keyword> ...]
```

It allows to search for the provided keywords in all commands help text, and display the matches, in order of relevance Multiple keywords can be provided, and the result will include matches of commands that include either keyword. Partial keywords can be provided using a wildcard (example: help -s data\* to search for any word starting with data) Examples:

```
> help -s statistics # Get help text of commands containing the word 'statistics'
> help -s *config* # Get help text of commands containing the single word pattern '*config*',
 e.g. 'ifconfig', 'config', 'configuration'
> help -s database 'cold restart' # Get help text of commands containing the words 'database'
 or 'cold restart'
```

Results appear as a list of help commands that can be used to view the entire help text. Results include an excerpt of the help text, with the target keyword underlined.

**Note:** The search capability only provides matches in the scope of commands and system features; object and attribute matches are excluded.

### 2.2.8. CLI Wildcard support

The CLI deals with objects based on YANG data hierarchy, and in some cases those objects correspond to YANG lists. In order to apply CLI commands to multiple objects in one step, wildcards can be used when inputting commands. This functionality can be used for any command that supports it, which includes show, set, delete commands.

**Tip:** Certain commands do not support wildcards, as they explicitly require one single object (for example, the add command).

Wildcards are represented by the star character '\*', and can have multiple meanings, depending on how they are used. Refer to Table 26: CLI wilcard usage (p. 84) for more information.

**Table 26: CLI wilcard usage**

| Replacement type | Description | Representation with wildcard | Direct Representation |
| --- | --- | --- | --- |
| whole instance identifier | All objects of this type are selected. | card-* | card-1-4 , card-1-5, etc |
| single key in the instance identifier | All objects of this type are selected, if they have the provided partial key. | port-1-4-* | port-1-4-1, port-1-4-2, etc |
| partial string key | Same as single key in the instance identifier, but considers partial strings when filtering keys. | user-*admin* | user-administrator, user-cryptoadministrator |
| auto-completing attribute names | Selects all attributes that match the filter. | *power* | rx-power, tx-power |

<!-- page 85 -->

For configuration changes, using wildcards will affect multiple objects, and as such user has to confirm that the intention is indeed to perform a bulk change. In this case, a failure in one object cancels the entire command (all-or-nothing behavior). The confirmation prompt appears with options y/n/?, where yes and no accept the changes, and '?' displays the list of targeted objects.

#### 2.2.8.1. Object Filtering

In addition to wildcards, several other mechanisms can be used to filter groups of objects instead of single objects. Table 27: Object Filtering contains the information about each filter mechanism.

**Table 27: Object Filtering**

| Filter mechanism | Example | Description |
| --- | --- | --- |
| Keys in lists | user- [john,paul] | Selects both user-john and user-paul |
| Keys in ranges | slot-1-[5..10] | Selects all slots in chassis 1 from slot 5 to slot 10; applicable for number values only |
| Mix of lists and ranges | lot-1- [2,3,5..10] | Selects all slots in chassis 1 from slot 5 to slot 10, and also slots 2 and 3 |
| MO without keys | user | Selects all users; equivalent to user-* |

### 2.2.9. CLI Commands Overview

The 1830 GX CLI commands can be divided into the following main categories:

- Help and Auxiliary
- Navigation and Display
- Piped
- Operation

The following tables provide a summarized description of the 1830 GX CLI commands. Table 28: CLI help and auxiliary commands (p. 85) lists the CLI auxiliary and help commands.

**Table 28: CLI help and auxiliary commands**

| Command | Description |
| --- | --- |
|  | Starts a timer in seconds. |
| tic |  |
|  | Displays the elapsed time since “tic” command was triggered. |
| toc |  |
|  | Displays the help information for the parameter. The parameter can be a command, container, or the attribute of a container. |
| help |  |
|  | Displays the configurable/addressable attributes/ parameters at the current path. |
| ? |  |

**Tip:** Refer to Auxiliary and Help Commands (p. 95) for usage examples and a detailed list of all help/command combinations.

<!-- page 86 -->

Table 29: CLI navigation and display commands (p. 86) lists CLI navigation commands.

**Table 29: CLI navigation and display commands**

| Command | Description |
| --- | --- |
|  | Defines an alias to represent one or more, combinations of command, container-id, attribute-ids, attribute-value, or any free text string. The configured aliases are valid for the current CLI session and are not persistent. |
| alias |  |
|  | Allows navigation within the managed entity hierarchy. |
| edit |  |
|  | Displays a numbered list of commands in the command history buffer (per CLI session). |
| history |  |
|  | Jumps to the top level of the managed hierarchy. |
| top |  |
|  | Displays the managed hierarchy container in a tree view. |
| tree |  |
|  | Removes a configured alias. |
| unalias |  |
|  | Goes up one path level (from the current level) inside the managed hierarchy. |
| up |  |

**Tip:** Refer to Navigation and Display Commands (p. 97) for usage examples.

Table 30: CLI piped commands (p. 86) lists CLI piped commands.

**Table 30: CLI piped commands**

| Command | Description |
| --- | --- |
|  | The output begins with the line that matches a specified word. |
| begin |  |
|  | Formats the output results of a command. |
| display |  |
|  | Excludes a specified string (similar to grep -v). |
| exclude |  |
|  | General filtering. |
| grep |  |
|  | Visually highlights the selected output of a command. |
| highlight |  |
|  | Includes a specified string (similar to grep default). |
| include |  |
|  | Enumerate the lines in the output. |
| linenum |  |
|  | Pauses the output. |
| more |  |
|  | Reorders the output of a command according to a given criteria. |
| sort |  |
|  | The output ends with the line that matches a specified word. |
| until |  |

<!-- page 87 -->

**Note:** Piped commands allow post-processing a command output, and are created by using a “\|” as a separator from the previous command.

**Tip:** Refer to Piped Commands (p. 112) for usage examples.

Table 31: CLI operation commands (p. 87) lists CLI operation commands.

**Table 31: CLI operation commands**

| Command | Description |
| --- | --- |
|  | Activates the database/software file and defined operations. |
| activate |  |
|  | Adds a new managed entity. |
| add |  |
|  | Executes a manual connection trigger to a configured dial-out-server. |
| call-home |  |
|  | Cancels any active upgrade. |
| cancel-upgrade |  |
|  | Generates a new certificate. |
| cert-gen |  |
|  | Toggles the Zero Touch Provisioning (ZTP) mode, by deactivating it or reactivating it. |
| change-ztp-mode |  |
|  | Clears database, logs, PM bin values and test results. |
| clear |  |
|  | Clears the console screen. |
| cls |  |
|  | Converts a CLI command into a NETCONF or RESTCONF command. |
| convert |  |
|  | Sets the value of the attribute(s), pm control, and alarm profile to the default value(s). |
| default |  |
|  | Deletes an existing managed entity or file. |
| delete |  |
|  | Downloads a file from a designated source to the 1830 GX G30/1830 GX G40. |
| download |  |
|  | Exits current CLI mode. |
| exit |  |
|  | Ensures an attribute matches the expected value. |
| expect |  |
|  | Defines and manages CLI variables. |
| export |  |
|  | Command for basic file and directory operations. |
| file |  |
|  | Launch a bash shell inside a Guest Container. |
| gshell |  |
|  | Closes any established session, independently on the type of the session (CLI, NETCONF, etc). |
| kill-session |  |
|  | Locks the database access to the current session. |
| lock | Changes the user's password. |
| password |  |
|  | Sends an echo request packet to test the destination system reachability. |
| ping |  |
|  | Prepares the NE for upgrade. |
| prepare-upgrade |  |
|  | Restarts a managed entity. |
| restart |  |
|  | Runs a task or a script. |
| run |  |
|  | Sets the value of the identified attribute(s). |
| set |  |
|  | Sets the state of an alarm. |
| set-alarm-state |  |
|  | Sets the system time. |
| set-time |  |
|  | Launch Linux Bash shell. |
| shell |  |
|  | Shows information about the container or the attributes, pm, alarms, and logs. |
| show |  |
|  | Generates simulated events in the system such as alarm events. |
| simulate |  |
|  | Delay for a specified amount of time. |
| sleep |  |
|  | Generates an ssh private/public key for the ssh protocol. |
| ssh-keygen |  |
|  | Stores the current state of the Configuration database into one of the available backup slots. |
| take-snapshot |  |
|  | Stops a running operation. |
| terminate |  |
|  | Shows the system time. |
| time |  |
|  | Displays the routing path to reach the destination system. |
| traceroute |  |
|  | Unlocks a previously locked database. |
| unlock |  |
|  | Provides an overview of the upgrade status of all cards in the system. |
| upgrade-status |  |
|  | Uploads a file from the 1830 GX G30/1830 GX G40 to a designated destination. |
| upload |  |
|  | Shows system uptime. |
| uptime |  |
|  | Performs a dry-run of an "add", "delete" or "set" command. |
| validate |  |

<!-- page 89 -->

**Tip:** Refer to Operation Commands (p. 127) for more information.

### 2.2.10. User groups and access privilege

A group is an aggregation of users with identical authorization capabilities. Groups provide the base for authorization of user access to the network element by mapping to access rules. Multiple access privileges are defined to restrict user access to resources. Each access privilege allows a specific set of actions to be performed. One or more access privileges can be assigned to each user account. A user may be member of multiple groups, in which case the highest group permission level decides the user access permission. Table 32: Default User Groups (p. 89) lists the supported pre-defined user groups.

**Table 32: Default User Groups**

| User group name | Access privilege | Details |
| --- | --- | --- |
| Monitoring Access Group (MA) | Read- Only | • Allows the user to monitor the module, that is, the user has read-only access to equipment and traffic management model (for example, PM data, connectivity, provisioning status).<br>• The user cannot modify anything on the module (read-only privilege).<br>• The Monitoring Access is provided to all users by default. |
| Network Administrator Group (NA) | Create, Read, Update and Delete | Allows the user to monitor the module, manage equipment, turn-up module, provision services, and administer various network-related functions. That is,the user has read and write access to overall system configuration (for example, DCN / networking infrastructure / software and firmware configuration and upgrade schedules or management interface configurations). |
| Security Administrator Group (SA) | Create, Read, Update and Delete | Allows the user to perform module security management and administration related tasks. That is, the user has read and write access to all security related operations, full access to security management model, remote AAA server configurations. All operations can be implemented to manage the creation, enabling / disabling of new users and passwords, user session monitoring, configuration of external AAA servers. Only administrators with SA privileges will be able to administer security updates. |
| Provisioning Group (PR) | Create, Read, Update and Delete | Allows the user to monitor the module, configure facility endpoints, and provision services (for example, provisioning of equipment and facility end-points). |
| Network Engineer (NE) | Create, Read, Update and Delete | Allows the user to monitor the module and manage equipment. That is, the user has read and write access to traffic management related operations (for example, facility end-points and cross-connections). |
| Encryption Administrator (EA) | Create, Read, Update | Allows the user to configure the data and control plane encryption functions. Note that some of the operations might overlap with the SA role. |
| Test and Turn up Group (TT) | Create, Read, Update | Allows the user to monitor, turn-up, and troubleshoot the module fix network problems. |

<!-- page 90 -->

**Table 32: Default User Groups (continued)**

and Delete

**Note:** The TACACS+ protocol uses Privilege Level (integers 0-15) as an optional mechanism to provide an authorization scheme for a user account. For the mapping between TACACS+ Privilege Levels and user groups, refer to *1830 Converged OS Overview Guide*.

Each user group has access for configuring data model objects or applying CLI commands according with Table 33: Data Model Configuration Access (p. 90) and Table 34: CLI Command Execution Access (p. 92). Table 33: Data Model Configuration Access (p. 90) lists the access privileges for each object per user class.

**Table 33: Data Model Configuration Access**

| Object | Groups with write access | Groups with read access |
| --- | --- | --- |
| aaa-server | SA | all |
| ace | SA | all |
| acl | SA | all |
| alarm | NA | all |
| alarm-control | NA | all |
| asap | NA | all |
| card | NA,NE | all |
| certificates | SA | all |
| chassis | NA,NE | all |
| cli | SA,NA | all |
| clock | NA,NE,TT | all |
| command | Whatever user-group is able to do this command | all |
| community-string | SA,NA | all |
| connect | NA,NE | all |
| data-model | SA,NA | all |
| data-path-encryption | EA | all |
| dial-out-server | NA,NE | all |
| dns | NA,NE,TT | all |
| equipment | NA,NE | all |
| eth-zr | NA,PR,TT | all |
| ethernet | NA,PR,TT | all |
| facilities | NA,PR,TT | all |
| file-server | NA,NE,TT | all |
| flexo | NA,PR,TT | all |
| flexo-group | NA,PR,TT | all |
| grpc | SA,NA | all |
| interface | NA,NE,TT | all |
| leds | NA,NE | all |
| lldp | NA,PR,TT | all |
| log-file | NA,NE,TT | all |
| log-server | NA,NE,TT | all |
| log-console | NA,NE,TT | all |
| ne | NA,NE,TT | all |
| netconf | SA,NA | all |
| networking | NA,NE,TT | all |
| ntp | NA,NE,TT | all |
| ntp-key | SA | all |
| ntp-server | NA,NE,TT | all |
| odu | NA,PR,TT | all |
| optical-carrier | NA,PR,TT | all |
| ospf | NA,NE,TT | all |
| pm | NA,NE,TT | all |
| pm-control | NA,NE,TT | all |
| pm-profile | NA,NE,TT | all |
| protocols | SA,NA | all |
| routing | NA,NE,TT | all |
| services | NA,PR | all |
| secure-applications | SA | all |
| security | SA | all |
| security-policies | SA | all |
| session | SA | SA + other users can read themselves only |
| snmp | SA,NA | all |
| snmp-target | SA,NA | all |
| snmpv3-user | SA,NA | all |
| ssh | SA,NA | all |
| ssh-known-host | SA | all |
| ssh-authorized-keys | SA | all |
| static-route | NA,NE,TT | all |
| syslog | NA,NE,TT | all |
| system | NA,NE,TT | all |
| sw-services | NA | all |
| sw-control-rule | NA | all |
| tasks | NA,NE,TT | all |
| trib-ptp | NA,PR,TT | all |
| tom | NA,NE | all |
| topology | NA,PR,TT | all |
| user | SA | SA + other users can read their sessions only |
| user-group | SA | all |
| xcon | NA,PR | all |
| ztp | SA | all |

Table 34: CLI Command Execution Access (p. 92) lists the execution access for each CLI command per user class.

**Table 34: CLI Command Execution Access**

| Command |  | Conditions | Groups with access | Notes |
| --- | --- | --- | --- | --- |
| activate | activate-file | swimage/database | NA | - |
|  | eqpt-fw | - | NA | - |
|  | location-led | - | NA,NE | - |
|  | krp | - | SA | - |
| add |  | - | all | - |
| alias |  | - | all | - |
| call-home |  | - | NA,NE | - |
| cert-gen |  | - | SA | - |
| change-ztp-mode |  | - | SA | - |
| clear | app | - | NA | - |
|  | certificate | - | SA | - |
|  | database | - | NA | - |
|  | diagnostics | - | NA,NE | - |
|  | file | swimage | NA | - |
|  | isk | (other) - | NA,NE SA | - - |
|  | log | - | NA,NE | - |
|  | ospf | - | NA,PR,TT | - |
|  | pm | - | NA,NE | - |
|  | recover-mode | - | SA | - |
|  | system | - | NA | - |
|  | topology | - | NA,PR,TT | Clears LLDP neighbor info. |
| convert |  | - | all | - |
| default |  | &lt;same as target parameter&gt; |  |  |
| delete |  | - | all | - |
| download |  | swimage | NA | - |
|  |  | script | NA | - |
|  |  | database | NA | - |
|  |  | certificate | SA | - |
|  |  | krp | SA | - |
|  |  | (other) | NA,SA,NE | - |
| edit |  | - | all | - |
| exit |  | - | all | - |
| export |  | - | all | - |
| file |  | - | NA,NE,TT | - |
| gshell |  | - | NA,NE,TT | - |
| help |  | - | all | - |
| history |  | - | all | - |
| kill-session |  | - | SA | - |
| lock |  | - | SA,NA,EA,NE,TT,PR | - |
| password |  | - | all | (changes own password) |
| ping |  | - | NA,PR,TT | - |
| prepare-upgrade |  | - | NA | - |
| restart |  | - | NA,NE | Includes warm/cold/ shutdown for cards/ toms |
| run | script task | - - | all NA,NE,TT | (script content will be limited by current user credentials) - |
| set |  | - | all | - |
| set-alarm-state |  | - | NA,NE,PR,TT | - |
| set-time |  | - | NA,NE,TT | - |
| shell |  | - | NA,NE,TT | - |
| show |  | - | all | - |
| simulate |  | - | NA,NE | For equipment simulation |
| sleep |  | - | all | - |
| ssh-keygen |  | - | SA | - |
| swversion |  | - | all | - |
| take-snapshot |  | - | NA | - |
| terminate |  | - | all | - |
| time |  | - | all | - |
| top |  | - | all | - |
| traceroute |  | - | NA,PR,TT | - |
| tree |  | - | all | - |
| unalias |  | - | all | - |
| unlock |  | - | SA,NA,EA,NE,TT,PR | - |
| up |  | - | all | - |
| upgrade-status |  | - | all | - |
| upload |  | database | NA | - |
|  |  | debug-log | NA,NE | - |
|  |  | logs | NA,NE,TT | - |
|  |  | pm-logs | NA,NE,TT | - |
| uptime |  | - | all | - |
| validate |  | - | all | - |

<!-- page 95 -->
