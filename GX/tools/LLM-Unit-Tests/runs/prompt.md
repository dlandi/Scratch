# The run prompt

The exact prompt run 01 used, one agent per shard, with `NN` the shard number.
**Use it verbatim for later runs.** A second run is only a variance measurement
if the method is held constant; change the wording and you have measured the
prompt instead of the corpus.

`prepare_run.py` writes the shards and prints this with the paths filled in.

---

You are answering operator questions about the Nokia 1830 GX R9.1 CLI using only the generated documentation corpus at `E:\Archive\GitHub\dlandi\Scratch\GX\R9_1_GX_CLI_Reference\`.

Read your questions from:
`<SHARD PATH>`
(JSONL, one `{"id":..., "question":...}` per line.)

HARD RULE: you must NOT open, read, grep or list anything under `E:\Archive\GitHub\dlandi\Scratch\GX\tools\`. That directory holds the expected answers and reading it invalidates this run. Work only from `R9_1_GX_CLI_Reference\`.

How to find each answer (this is the retrieval path the corpus is designed for):
- `R9_1_GX_CLI_Reference\INDEX.md` lists every command under a domain.
- `R9_1_GX_CLI_Reference\index\topics.md` maps subject vocabulary to commands.
- `R9_1_GX_CLI_Reference\index\parameters.md` maps a parameter name to the commands that take it.
- `R9_1_GX_CLI_Reference\index\entities.md` maps an AID prefix to its command.
- Command files live in `R9_1_GX_CLI_Reference\06-operation-commands\`. They are short; read the whole file rather than grepping fragments out of it.

For each question, write a self-contained technical answer of roughly 400-800 characters, as you would answer an operator. Include the concrete specifics the guide states: exact command and attribute names, the permitted values, the defaults, the numeric ranges and limits, and any stated pre-condition or restriction. Use the guide's own spelling for identifiers and values. Do not hedge, do not describe your search, do not add caveats about the documentation. If the guide genuinely does not say something, say what it does say.

Write your output to:
`<OUTPUT PATH>`
as JSONL, one object per line, exactly `{"id": "<the id verbatim>", "answer": "<your answer>"}`. One line per question, ids unchanged. Nothing else in the file.

Your final message should just report how many answers you wrote and any question you could not find a source for.

---

## What run 01 did with it

18 shards of 25 questions, dispatched as background agents. Answers averaged 810
characters, above the 400-800 asked for, because several questions want three or
four objects and their permitted values. That is fine and worth expecting again.

No agent reported an unanswerable question. Several reported defects in the
source document as they read it, which is where seven of the eight now listed in
`../../README.md` came from. Worth collecting again.
