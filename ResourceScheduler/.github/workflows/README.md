# CI workflows

## build-test.yml

Triggers on push to `main` or `master` and on every pull request.

Runs on `ubuntu-latest` using the .NET SDK pinned in `src/global.json`.
The job restores, builds in Release, and runs both test projects:

- `ResourceScheduler.Tests` (xUnit): domain rule and concurrency tests
  for `InMemoryClientService`.
- `ResourceScheduler.UI.Tests` (bUnit + xUnit): component-level tests
  for `Pill`, `StatusDot`, `RuleViolationBanner`, `MiniTopology`,
  `Avatar`, `Header`, `Modal`, and the editor components.

Test results are uploaded as a `test-results` artifact in TRX format,
retained for 14 days.

## Failure triage

- Build failure: read the `Build` step log; the failing project and
  file appear in the standard `dotnet build` output.
- Test failure: download the `test-results` artifact and open the TRX
  file in Visual Studio or Rider, or read the `Test` step log
  directly for the failing test name and stack.
