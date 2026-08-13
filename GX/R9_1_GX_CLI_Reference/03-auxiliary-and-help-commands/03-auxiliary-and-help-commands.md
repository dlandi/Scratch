---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 03-auxiliary-and-help-commands
section: '3. Auxiliary and Help Commands'
source_lines: 2991-3037
---

# 3. Auxiliary and Help Commands

Auxiliary commands are add-ons to the main commands and provide an aid in very specific use cases, such as evaluating the system response to a command or script. Table 35: Help additional commands (p. 95) summarizes all the available auxiliary commands.

**Table 35: Help additional commands**

| Commands | Description | Example |
| --- | --- | --- |
|  | Starts a timer for the typed command. | Single command: tic;show log;toc List of commands (script): tic ;show log ;traceroute 10.128.1.1 ;toc |
| tic |  |  |
|  | Displays the elapsed time since the timer was started and the command was executed by the system. |  |
| toc |  |  |

The help commands provide information regarding the available commands, containers, or attributes. Two main types of help are available:

- Normal help information, using the “help” command.
- Contextual help, using the “?” character symbol.

The “?” character symbol can be used at any time to display what can be typed by the user at the current prompt and can be combined with other symbols to obtain specific help information. Table 36: Help additional commands (p. 95) summarizes all the available help commands/command combinations.

**Table 36: Help additional commands**

| Commands/ Command combinations | Description | Example |
| --- | --- | --- |
|  | Displays the list of all available commands |  |
| [?] |  | ? |
|  | Displays information on how the command/container/attribute can be used. |  |
| help &lt;command/container/attri bute&gt; |  | help show |
|  | Searches the provided keyword(s) in all commands help text, and displays the matches, in order of relevance. Partial keywords can be provided using a wild card. |  |
| help -s &lt;keyword&gt; [&lt;keyword&gt; ...] |  | help -s inventory help -s data* |
|  | Displays the possible addressable parameters for the given command. |  |
| &lt;command&gt; [?] |  | show ? |
|  | Displays the possible options for the given command. |  |
| &lt;command&gt; [-][?] |  | show -? |
|  | Displays the complete help for the given command. |  |
| &lt;command&gt; [-][h] |  | show -h |
|  | Displays the hierarchy sub-containers and the attribute list definition of the container. |  |
| &lt;command&gt; &lt;container&gt; [?] |  | show &lt;container&gt; ? |
|  | Displays the list of attributes of the given container. |  |
| &lt;command&gt; &lt;attribute&gt; [?] |  | show &lt;attribute&gt; ? |

<!-- page 96 -->

**Tip:** For a more detailed information about the “help” command and the complete CLI help system refer to CLI Help (p. 77).

<!-- page 97 -->
