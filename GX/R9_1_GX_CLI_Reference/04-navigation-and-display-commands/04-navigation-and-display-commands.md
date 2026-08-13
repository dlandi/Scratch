---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 04-navigation-and-display-commands
section: '4. Navigation and Display Commands'
source_lines: 3038-3539
---

# 4. Navigation and Display Commands

This section describes how to use the navigation and display commands while operating the 1830 GX G30/1830 GX G40.

**Tip:** Unless stated otherwise, always assume the root command prompt [ne] for all the examples in this section.

## 4.1. alias

#### Command Description

The `alias` command is used to define a more user-friendly alphanumeric string for one or more commands, container, attribute, or values. When using `alias` command, take into consideration the following information:

- Alias with no arguments prints the current list of aliases in the form 'name=value'.
- When parameters are provided, an alias is defined for \<name\> to the given \<value\>.
- For multi word values, quotes must be used.
- Aliases can reference other aliases.
- Aliases can be removed using the command unalias.
- Aliases are persistent and shared among all users and CLI sessions.
- The \<name\> of an alias can be used without resolving it by using quotes.

#### Command Syntax

```
alias [-h]
alias [<name>=<value>]
```

<!-- page 98 -->

#### Command Usage Details

**Table 37: alias Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 38: alias command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;name&gt; | Name of the alias to add. |
| &lt;value&gt; | Value to replace the alias name with. |

#### Examples

This example shows how to display the list of existing aliases:

```
alias
```

This example shows how to create a single word alias:

```
alias s=show
```

This example shows how to create a multi word alias:

```
alias sa='show alarm'
```

<!-- page 99 -->

## 4.2. edit

#### Command Description

The edit command is used to navigate the managed entity hierarchy. The provided entity IDs can either be absolute (starting from the root of the system) or relative to the current entity shown in the path banner. The current level in the hierarchy is shown in the path banner. The attempt to edit an nonexistent entity instance will fail. The effect of the edit command is visible on the CLI banner:

```
[ NE ]
user@host> edit chassis-1
[ chassis-1 ]
user@host>
```

Related commands:

- `up` - will navigate to the parent level.
- `top` - will navigate to the top of the hierarchy root.

#### Command Syntax

```
edit [-h]
edit <entity-id> [<entity-id> ...]
```

#### Command Usage Details

**Table 39: edit Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Related Commands | up (p. 110) - navigates to the parent level, top (p. 102) - navigates to the top of the hierarchy root |

<!-- page 100 -->

#### Command Parameters

**Table 40: edit command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| &lt;entity id&gt; | Instance ID of the entity to be addressed. |

#### Examples

This example shows how to navigate to a 1830 GX G40 port entity using absolute addressing:

```
edit port-1-5-T1
```

This example shows how to navigate to this entity using absolute addressing:

```
edit card-1-1
```

This example shows how to navigate to the same entity using relative addressing:

```
edit chassis-1 slot-2
```

<!-- page 101 -->

## 4.3. history

#### Command Description

The `history` command is used to display the current session's command history as a numbered list. Each command can be repeated using '!\<n\>', where n is the number in the history list. The '!!' can also be used to repeat the previous command.

**Tip:** Search for particular history entries by filtering with piped commands, for example: `history | include 'show'`

#### Command Syntax

```
history [-h]
```

#### Command Usage Details

**Table 41: history Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode, Candidate Configuration Mode |

#### Command Parameters

**Table 42: history command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Examples

This example shows how to display the list of the commands run by the user in the current session:

```
history
```

<!-- page 102 -->

## 4.4. top

#### Command Description

The `top` command is used to bring the current path to the top of the managed entity hierarchy [ne]. The command changes the currently selected managed entity to the top level. The effect of the top command is visible on the CLI banner.

#### Command Syntax

```
top [-h]
```

#### Command Usage Details

**Table 43: top Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Related Commands | edit (p. 99) navigates to any entity in the hierarchy. up (p. 110) navigates to the parent level. |

#### Command Parameters

**Table 44: top command Flags**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Examples

This example shows how to navigates to the [chassis-1] level on the hierarchy:

```
edit chassis-1
[ chassis-1 ]
```

This example shows how to returns to the top level of the hierarchy ([ne]).

<!-- page 103 -->

```
top
[ ne ]
user@host>
```

<!-- page 104 -->

## 4.5. tree

#### Command Description

The `tree` command is used to display the managed entity hierarchy in a tree-like format. This allows a better understanding and view of the system hierarchy. The tree syntax starts on the current path but can be triggered at any point of the hierarchy. The `tree` is a recursive managed entity listing command that produces a depth indented listing of nodes outputted to the CLI console. When using `tree` command, take into consideration the following information:

- Only managed entities are shown, including all the existing instances.
- No attribute name or values are shown.
- The base of the tree is the current working entity or the provided entity id, and unless defined otherwise, the complete structure is shown up to max depth.
- Using the -a flag displays only the target entity, along with all its ancestors.
- Using the -o flag will display the YANG tree of the provided top node.
- The output is displayed using UTF-8 character set.

#### Command Syntax

```
tree [-h]
tree [-a|-o|-d=depth] ([<entity id> ...])
```

#### Command Usage Details

**Table 45: tree Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |

<!-- page 105 -->

#### Command Parameters

**Table 46: tree command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -a | Displays entity ancestors only. |
| -d=depth | Maximum display depth of the directory tree. The depth specifies the number of levels to be displayed in the tree syntax. If not specified all levels are displayed by default. |
| -o | Displays a tree based on a YANG top node. |
| &lt;entity id&gt; | Instance ID of the entity to be displayed in the tree. |

#### Examples

This example shows how to display a tree list starting from the currently scoped entity:

```
tree
```

This example shows how to display a tree list up depth 2, from the currently scoped entity:

```
tree -d=2
```

This example shows how to display a tree list starting from card-1-1 entity:

```
tree card-1-1
```

This example shows how to display a tree list including all ancestors or this interface:

```
tree -a interface-DCN
```

This example shows how to display the tree list starting from the currently scoped entity:

```
tree | more
```

The following output is displayed:

<!-- page 106 -->

```
tree |more
ne
├----equipment
│   ├----card-1-1
│   │   ├----console-1-1
│   │   ├----controller-card-1-1
│   │   ├----port-1-1-AUX-1
│   │   │   └----comm-eth-1-1-AUX-1
│   │   ├----port-1-1-AUX-2
│   │   │   └----comm-eth-1-1-AUX-2
│   │   ├----port-1-1-CRAFT
│   │   │   └----comm-eth-1-1-CRAFT
│   │   ├----port-1-1-DCN
│   │   │   └----comm-eth-1-1-DCN
│   │   └----port-1-1-U1
│   │       └----usb-1-1-U1
│   ├----card-1-2
│   │   ├----port-1-2-U1
│   │   │   └----usb-1-2-U1
│   │   └----port-1-2-U2
│   │       └----usb-1-2-U2
│   ├----card-1-5
│   │   ├----capabilities-1-5
│   │   │   ├----supported-carrier-mode-1-5/250E.72S
│   │   │   ├----supported-carrier-mode-1-5/300E.72S
│   │   │   ├----supported-carrier-mode-1-5/350E.72S
│   │   │   ├----supported-carrier-mode-1-5/400E.72S
│   │   │   ├----supported-carrier-mode-1-5/400E.84P
│   │   │   ├----supported-carrier-mode-1-5/400E.84S
│   │   │   ├----supported-carrier-mode-1-5/400E.91P
│   │   │   ├----supported-carrier-mode-1-5/400E.91S
│   │   │   ├----supported-carrier-mode-1-5/400E.96P
│   │   │   ├----supported-carrier-mode-1-5/400E.96S
--more--
```

This example shows how to display a tree list including all ancestors or this usb entity:

<!-- page 107 -->

```
tree -a port usb
```

The following output is displayed:

```
tree -a port usb
ne
└----equipment
    └----card-1-1
        └----port-1-1-U1
            └----usb-1-1-U1
ne
└----equipment
    └----card-1-2
        └----port-1-2-U1
            └───usb-1-2-U1
ne
└----equipment
    └----card-1-2
        └----port-1-2-U2
            └----usb-1-2-U2
```

<!-- page 108 -->

## 4.6. unalias

#### Command Description

The `unalias` command is used to remove an alias previously defined.. When using `unalias` command, take into consideration the following rules:

- If -a is supplied, all alias definitions are removed.
- The command is always successful unless a supplied \<name\> is not a defined alias.
- Aliases can be added and visualized using the 'alias' command.
- Removing an alias will have impact on all sessions and users.

#### Command Syntax

```
unalias -h
unalias -a
unalias [<name> ...]
```

#### Command Usage Details

**Table 47: unalias Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational Mode |

#### Command Parameters

**Table 48: unalias command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |
| -a | Remove all defined aliases. |
| &lt;name&gt; | Name of the alias to remove. |

#### Examples

This example shows how to remove an alias named 'foo':

```
unalias foo
```

This example shows how to remove multiple aliases 'bar', 'bar' and 'qux':

```
unalias bar baz qux
```

This example shows how to remove all existing aliases:

```
unalias -a
```

<!-- page 110 -->

## 4.7. up

#### Command Description

The `up` command is used to bring the current path up by one path level in the managed entity hierarchy. The command changes the currently selected managed entity to its parent level. When using `up` command, take into consideration the following rules:

- At the hierarchy top level, the `up` command has no impact.
- The effect of the `up` command is visible on the CLI banner.
- This command does not support any parameters.

Related commands:

- `edit` - will navigate to any entity in the hierarchy.
- `top` - will navigate to the top of the hierarchy root.

#### Command Syntax

```
up [-h]
```

#### Command Usage Details

**Table 49: up Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode, Candidate Configuration mode |
| Related Commands | edit (p. 99) - navigates to any entity in the hierarchy, top (p. 102) - navigates to the top of the hierarchy root |

<!-- page 111 -->

#### Command Parameters

**Table 50: up command parameters**

| Parameter | Description |
| --- | --- |
| -h | Displays help for this command. |

#### Examples

This example shows how to go to the card level and use up to return to the ne level: Initial hierarchy top level [ne]:

```
[ ne ]
```

Navigate to [card] level using edit command:

```
edit card-1-5
[ card-1-5 ]
```

Navigate to [card] parent level [equipment] using up command:

```
up
[ equipment ]
```

Navigate to the hierarchy top level [ne] using up command:

```
up
[ ne ]
```

<!-- page 112 -->
