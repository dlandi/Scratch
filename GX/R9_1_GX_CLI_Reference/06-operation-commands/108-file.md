---
source: R9_1_GX_CLI_Command_Reference_Guide_001P4.md
part: 06-operation-commands
section: '6.108. file'
source_lines: 11011-11123
---

## 6.108. file

#### Command Description

**file** This command is used to perform basic file and directory operations. This command performs file management operations, on both files and directories. Note that file system access is restricted to the current user's access (not all files/directories are editable). Supported operations include:

- rename : renames a file or directory.
- view : does listing for a file or directory.
- delete : deletes a file
- sha256sum : calculates SHA256 hash for a file.
- md5sum : calculates md5sum hash for a file.

**clear file** This command removes one particular file from the system. This command can be used to clear a downloaded software image (swimage) file. Both the \<filetype\> and an identifier \<target-file\> need to be provided. The supported file-types for clearing include:

- krp : removes a Key Replacement Package (note: only 1 krp may exist, so \<target-file\> is not needed)
- script : deletes a script from the filesystem; use 'show script' to visualize available scripts
- swimage : deletes a downloaded swimage which was not yet installed

The \<target-file\> will depend on the filetype; use \<tab\> when filling it in to get possible options.

#### Command Syntax

```
clear [-f] file [filetype=]<value> [target-file=]<value>
file [operation=]<value> [file-path=]<value> [new-file-path=]<value>
```

<!-- page 492 -->

#### Command Usage Details

**Table 302: file Command Usage**

| Section | Description |
| --- | --- |
| Access Mode | Operational mode |

#### Command Parameters

**Table 303: file Command Parameters**

| Parameter | Description | Values | Default | Used in |
| --- | --- | --- | --- | --- |
| operation | The operation to be performed | • rename: Renames a file or directory.<br>• delete: Deletes a file.<br>• view: Shows a listing for a file or directory.<br>• sha256sum: Generates SHA256 hash checksum of a file.<br>• md5sum: Generates md5 hash checksum of a file. | n/a | file |
| file-path | Current file path. | string | n/a | file |
| new-file-path | New file path when the operation is rename. | string pattern '[A-Za-z0-9 \-/\.]* _ | n/a | file |
| result | The file operation result. | string | n/a | file |
| filename | The name of the file to be displayed including the path to the file. | string | n/a | show |
| filetype | Predefined file type that supports clearing. Type of file to be cleared:<br>• krp : removes a Key Replacement Package (note: only 1 krp may exist, so &lt;targetfile&gt; is not needed)<br>• script : deletes a script from the filesystem; use 'show script' to visualize available scripts<br>• swimage : deletes a downloaded swimage which was not yet installed | swimage, script, krp | n/a | clear |
| target-file | Filepath of the file to be deleted. | &lt;string&gt; | n/a | clear |

#### Examples

This example shows how to view renaming a file or directory:

```
file rename /tmp/a.log /tmp/b.log
```

This example shows how to list a directory:

```
file view /tmp
```

This example shows how to view deleting a file:

```
file delete /tmp/a.log
```

This example shows how to generate a sha256 checksum:

```
file sha256sum /tmp/a.log
```

This example shows how to generate a md5 checksum:

<!-- page 494 -->

```
file md5sum /tmp/a.log
```

This example shows how to display the contents of a file:

```
show file transfer/text.txt
 hello there
[ ne ]
```

This example shows how to remove test.cli script:

```
clear file script test.cli
```

This example shows how to remove installed KRP:

```
clear file krp
```

This example shows how to clear a software image file:

```
clear file swimage Dogos-F1.0-Main-trunk-2020.10.19_09_11.manifest
```

<!-- page 495 -->
