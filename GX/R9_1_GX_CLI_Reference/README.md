# 1830 GX Release 9.1 CLI Reference Guide - split by chapter

Generated from `../R9_1_GX_CLI_Command_Reference_Guide_001P4.md` (1900-003486 Revision 001, July 2026),
28634 lines. Each file below is a byte-exact slice of the source with a
YAML frontmatter header prepended; concatenating every slice in the order
listed reproduces the original document exactly.

**This whole directory is generated. Do not edit these files by hand:**
the next build overwrites them. The toolchain that produces them lives in
`../tools/` - see [tools/README.md](../tools/README.md). Rebuild with
`python ../tools/build_all.py`.

Internal anchor links (`](#...)`) are left exactly as they appear in the
source and therefore do not resolve across files. Image references point at
`images/figure-p*.png`, which does not exist alongside the source either.

## Start here

| File | Use it for |
| --- | --- |
| [INDEX.md](INDEX.md) | Master command index: 395 commands in 16 functional domains, with summaries and file paths |
| [index/README.md](index/README.md) | Retrieval guide: which index file answers which kind of question |
| [NAVIGATION.md](NAVIGATION.md) | The document's own table of contents, with links rewritten to the split files |

## Parts

| Part | File | Section | Source lines |
| --- | --- | --- | --- |
| `00-front-matter` | [00-cover-and-legal.md](00-front-matter/00-cover-and-legal.md) | Cover and legal notices | 1-36 |
| `00-front-matter` | [01-contents.md](00-front-matter/01-contents.md) | Contents | 37-498 |
| `00-front-matter` | [02-list-of-figures.md](00-front-matter/02-list-of-figures.md) | List of Figures | 499-510 |
| `00-front-matter` | [03-list-of-tables.md](00-front-matter/03-list-of-tables.md) | List of Tables | 511-1390 |
| `00-front-matter` | [04-about-this-document.md](00-front-matter/04-about-this-document.md) | About this document | 1391-1455 |
| `01-introduction` | [01-introduction.md](01-introduction/01-introduction.md) | 1. Introduction | 1456-1825 |
| `02-using-the-cli` | [02-using-the-cli.md](02-using-the-cli/02-using-the-cli.md) | 2. Using the Command Line Interface (CLI) | 1826-2990 |
| `03-auxiliary-and-help-commands` | [03-auxiliary-and-help-commands.md](03-auxiliary-and-help-commands/03-auxiliary-and-help-commands.md) | 3. Auxiliary and Help Commands | 2991-3037 |
| `04-navigation-and-display-commands` | [04-navigation-and-display-commands.md](04-navigation-and-display-commands/04-navigation-and-display-commands.md) | 4. Navigation and Display Commands | 3038-3539 |
| `05-piped-commands` | [05-piped-commands.md](05-piped-commands/05-piped-commands.md) | 5. Piped Commands | 3540-4012 |
| `06-operation-commands` | [000-overview.md](06-operation-commands/000-overview.md) | 6. Operation Commands (chapter opening) | 4013-4018 |
| `99-acronyms` | [99-acronyms.md](99-acronyms/99-acronyms.md) | Acronyms | 27832-28635 |

Chapter 6 continues with one file per command; see
[06-operation-commands/README.md](06-operation-commands/README.md).
