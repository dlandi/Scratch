# Index - retrieval guide

Machine-oriented index over the split GX CLI Reference Guide. Everything here is
generated from `../R9_1_GX_CLI_Command_Reference_Guide_001P4.md`; the content files
themselves are unmodified slices of that document.

## Which file answers which question

| The query looks like | Start here |
| --- | --- |
| "how do I configure X", command name known | [../INDEX.md](../INDEX.md) |
| Domain vocabulary, no command name ("wavelength", "upgrade", "loopback", "MACsec") | [topics.md](topics.md) |
| An attribute or parameter name ("admin-state", "tx-power") | [parameters.md](parameters.md) |
| An AID or entity string ("card-1-1", "port-1-1-DCN") | [entities.md](entities.md) |
| "who is allowed to run X", user groups, privileges | [access-control.md](access-control.md) |
| A page citation from the text, "(p. 934)" | [pages.tsv](pages.tsv) |
| "Table 93", "Figure 5" | [tables.tsv](tables.tsv) |
| Anything programmatic: filtering, joins, bulk analysis | [commands.jsonl](commands.jsonl) |
| Browsing the document in reading order | [../NAVIGATION.md](../NAVIGATION.md) |

## Retrieval notes

- Operation command files are self-contained and short (median 53 lines).
  Prefer reading the whole file over grepping fragments out of it.
- `INDEX.md` lists every command exactly once, under one primary domain.
  `topics.md` is where cross-cutting membership lives, so a command can appear in
  several topics.
- Access mode matters for answering "why did my command fail": a `cand`-only
  command must be run after `configure`, and needs `commit` to take effect.
- The guide covers four chassis variants (G31, G32, G34c, G42). Parameter ranges
  and supported cards differ per variant; check the parameter table rather than
  assuming.

## commands.jsonl fields

One JSON object per line, 395 lines.

| Field | Meaning |
| --- | --- |
| `name`, `category`, `section` | Command name; operation/navigation/piped/auxiliary; source section number |
| `file`, `anchor`, `source_lines`, `pages` | Where to read it, and where it came from in the source |
| `summary` | First substantive sentence of the description |
| `domain`, `domain_title`, `topics` | Curated classification |
| `kind`, `verbs`, `entity_ids`, `aid_prefix` | `entity` vs `action`; verbs the syntax supports; AID pattern the command addresses |
| `sub_keywords` | Literal keywords the command takes that are not addressable entities (`activate-file`, `location-led`) |
| `access_mode` | `operational` / `candidate_config` booleans, the source's own label, `qualified` when the source narrows it, plus the raw string |
| `usage_details` | Every row of the Command Usage Details table: pre-conditions, post-conditions, related commands, AID notes |
| `parameters`, `param_count` | Name, description, values, default and `used_in` verbs |
| `tables`, `page_refs` | Table numbers defined in the section; pages it cites |
| `user_groups` | Execution access (Table 34) and object access (Table 33) where defined |
| `has_examples`, `line_count` | Whether the section has worked examples; section size |

## Coverage

- 395 commands, 3800 parameter rows, 1787 distinct parameter names
- 294 commands address a named entity; 101 are action commands
- 47 commands carry explicit execution-access rules; 70 objects carry model access rules
- 57 topics, 487 topic-to-command associations
- `tables.tsv` covers 857 tables and 7 figures. The source gives Figure 7 no
  caption line (it is listed in the List of Figures and its image reference
  exists), so it has no row.
