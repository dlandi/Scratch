## Prompt engineering workflow for agent-driven feature implementation

A feature (“row expansion” for QuickGrid/Blazor) was driven end-to-end through a **spec → plan → tasks → agent prompt → execution reports** pipeline.

* **Specification / Plan / Tasks** were produced via a **collaboration/discussion with Copilot in Visual Studio 2026 using ChatGPT 5.2**.   
* **Spec** defines the target architecture, invariants, parity requirements vs legacy (`RowColumn`), namespace/CSS rules, and the intended state machine + contracts for `RowExpandFeature<TGridItem>`. 
* **Implementation plan** translates that spec into an executable roadmap: concrete types to add, rendering/dispatch rules, event ordering, spacer-row semantics, and failure/guard behavior. 
* **Task breakdown** turns the plan into granular deliverables with explicit checklists and “interface alignment” gates (plumbing prerequisites → contracts → state/data → feature → component → CSS → demo page). 
* **Agent execution prompt** operationalizes all of the above into a repeatable “agent work session” that enforces build correctness *and* time tracking (because the IDE agent couldn’t reliably infer local time). 
* **Execution report** records start/end/duration per task + files changed + checklist evidence, with sessions grouped by milestone/phase. 
* **Summary** rolls up totals: ~**35m 24s** of agent-executed work across sessions, plus a separately called-out **15 minutes** of manual testing/debugging afterward. 

Net effect: the “prep docs” (spec/plan/tasks) become **structured context**, and the prompt becomes the **execution contract** that yields auditable task-level telemetry (timestamps, diffs, artifacts).

---

## Anatomy of the prompt (the techniques worth reusing)

1. **Runbook header with explicit variables**
   A small config table pins the report paths, spec/task sources, and current phase—reducing ambiguity and keeping output consistently routed. 

2. **Mode gate (“abort if not in agent mode”)**
   Prevents accidental execution in the wrong interaction mode, avoiding half-runs and missing logs. 

3. **Non-negotiable build requirement**
   “Work isn’t complete until the solution builds cleanly,” plus a required build step and a no-false-completion rule. 

4. **Defined “stuck” behavior (pause + ask with evidence)**
   If build errors can’t be resolved locally, the prompt forces a stop, asks for guidance, and requires exact errors + file paths (prevents hallucinated fixes). 

5. **Pre-mortem for a known workspace failure mode**
   The “accidental extra root folder” section is a targeted safeguard against a common repo-layout mistake, with required checks and remediation steps. 

6. **Time tracking as a first-class protocol, not “best effort”**

   * Mandatory timestamp tool call before session start and before/after **each** task (`Get-Date ...`). 
   * Hard abort if timestamps can’t be obtained. 

7. **A strict per-task logging schema (portable + auditable)**
   The report format forces: TaskId/description, start/end, computed duration, files changed, required checklists, then implementation notes. 

8. **Explicit allowed context sources + parity anchors**
   The prompt whitelists where truth comes from (spec, plan, legacy reference, existing architecture) to keep the agent grounded. 

9. **Sequencing constraint (“execute tasks in order”)**
   The task list is bound to a phase and executed sequentially, keeping the execution report aligned with the planned milestones. 

Key framing: treat the prompt like a **CI policy + lab notebook template**—it defines *how work is allowed to complete*, *what evidence must be produced*, and *how time and correctness are proven*, not merely “what to build.”
