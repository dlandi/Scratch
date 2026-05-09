# CI workflows

## build-test.yml

Triggers on push to `main` or `master` and on every pull request.

Runs on `ubuntu-latest`. The pipeline gates on both stacks:

### Rust (`ResourceScheduler/src/Rust`)

1. `cargo fmt --all --check`
2. `cargo clippy --all-targets -- -D warnings`
3. `cargo test --all-targets` (router-level integration tests against
   in-memory SQLite, ~40 tests)
4. `cargo build --release` (produces `target/release/resource-scheduler-api`
   so the .NET integration tests can spawn it without rebuilding)

The cargo registry, git index, and `target/` are cached on
`Cargo.lock`. First-cold runs take a few minutes; warm runs are
seconds.

### .NET (`ResourceScheduler/src/DotNet`)

Uses the SDK pinned in `global.json`.

1. `ResourceScheduler.Tests` (xUnit): rule and concurrency tests for
   `InMemoryClientService`, plus the 23 wire-level contract tests for
   `RemoteClientService`.
2. `ResourceScheduler.UI.Tests` (bUnit + xUnit): component-level tests
   for `Pill`, `StatusDot`, `RuleViolationBanner`, `MiniTopology`,
   `Avatar`, `Header`, `Modal`, and the editor components.
3. `ResourceScheduler.IntegrationTests` (xUnit): cross-stack tests
   that boot the Rust binary on a random port with a temp SQLite file
   and drive it through the production `RemoteClientService`.

Test results from all three projects are uploaded as a `test-results`
artifact in TRX format, retained for 14 days.

## Failure triage

- **Rust step failure** (`cargo fmt`, `cargo clippy`, `cargo test`,
  `cargo build`): the failing crate, file, and line appear in the step
  log. Reproduce locally by `cd ResourceScheduler/src/Rust && cargo
  <step>`.
- **.NET build failure**: read the `Build` step log; the failing
  project and file appear in the standard `dotnet build` output.
- **Test failure**: download the `test-results` artifact and open the
  TRX file in Visual Studio or Rider, or read the relevant test step
  log directly for the failing test name and stack.
- **Integration test failure with no obvious test name**: the fixture
  may have failed to start the Rust binary. Check the `Integration
  tests` step log for `cargo build failed` or `Rust server did not
  respond on .../healthz` lines.
