# Simple-GCP session recap, 2026-04-22

This is a record of the session where the `Simple-GCP` console project was created, wired into the solution, and verified end-to-end against a live Google Cloud Pub/Sub project.

## Outcome

A `.NET 8` console app at `Simple-GCP/Simple-GCP.csproj` publishes a message to a Pub/Sub topic and receives it through a pull subscription, with no credentials in code.

The original 2026-04-22 verification used user-based Application Default Credentials on a different machine. The current machine now uses a service account key via `GOOGLE_APPLICATION_CREDENTIALS`.

Verified run output:

```
Project: pubsub-demo-01-494119
Topic: SSU-1-PubSub-Topic
Subscription: SSU-1-PubSub-Topic-sub
Published message ID: 18770939703703971
Received message ID: 18770939703703971
Received payload: Hello from .NET 8 at 2026-04-22T20:06:00.1523561+00:00
```

## What was created or changed in the repo

- `Simple-GCP/Simple-GCP.csproj` - new `net8.0` console project, `OutputType=Exe`, with `<PackageReference Include="Google.Cloud.PubSub.V1" />`. No `Version=` attribute because this repo uses central package management via `Directory.Packages.props`.
- `Simple-GCP/Program.cs` - top-level statements pulled verbatim from step 11 of `Docs/google-cloud-pubsub-dotnet8-local-setup.md`. Reads three env vars, publishes a timestamped message, then starts a subscriber with a 30-second timeout and a clean `StopAsync` in `finally`.
- `GCP-PubSub.slnx` - added a `/Simple-GCP/` solution folder pointing at the new csproj.

Nothing else in the repo was modified.

## Google Cloud resources used

The current machine is configured to run against GCP project `ssu-pubsub-proj`.

| Resource | Value |
|---|---|
| Project id | `ssu-pubsub-proj` |
| Topic | `SSU-1-PubSub-Topic` |
| Subscription | `SSU-1-PubSub-Topic-sub` (pull, bound to `SSU-1-PubSub-Topic`) |
| Auth mode on current machine | Service account key via `GOOGLE_APPLICATION_CREDENTIALS` |
| Windows user profile | `C:\Users\landi` |
| Credential file location | `C:\Users\landi\gcp-keys\ssu-pubsub-proj-1c74130096a1.json` |

Note: the original setup doc (`Docs/google-cloud-pubsub-dotnet8-local-setup.md`) still shows generic examples in some sections. The current topic is `SSU-1-PubSub-Topic`.

## Environment variables for the app

```powershell
$env:GCP_PROJECT_ID="ssu-pubsub-proj"
$env:PUBSUB_TOPIC_ID="SSU-1-PubSub-Topic"
$env:PUBSUB_SUBSCRIPTION_ID="SSU-1-PubSub-Topic-sub"
```

Authentication on this machine comes from `GOOGLE_APPLICATION_CREDENTIALS` pointing at the service account key file, not from `gcloud auth application-default login`.

## Run command

From the repo root:

```powershell
dotnet run --project "Simple-GCP\Simple-GCP.csproj"
```

The app exits on its own within a few seconds of receiving the message. No Ctrl+C required.

## Evaluation of Claude Web's milestone-10 follow-up

During the session, Claude Web produced a "milestone 10" recap with suggestions. Items the user should be aware of:

- Keep: env var names, PowerShell one-liner, ADC file location, budget alert recommendation, unused-project cleanup suggestions.
- Watch: Claude Web suggested `dotnet add package Google.Cloud.PubSub.V1`. Do not run this in this repo. It injects a `Version=` attribute that breaks central package management (NU1008). The package is already referenced.
- Watch: Claude Web's "minimal smoke test code" awaits `subscriber.StartAsync(...)` directly and tells the user to Ctrl+C. This blocks forever and skips `StopAsync`. The `Program.cs` actually installed in the repo uses a `TaskCompletionSource` with a 30-second timeout and a `finally`-block `StopAsync`, which is cleaner.
- Personal choice: Claude Web offered persistent `[Environment]::SetEnvironmentVariable(..., "User")` variants. For a scratch repo the session-scoped one-liner is preferred to avoid leaking the project id into every future PowerShell session.

## Open follow-ups (not executed in this session)

- Set a monthly billing budget alert on `pubsub-demo-01-494119` (recommended: $10 with 50/90/100% email thresholds).
- Delete unused sibling GCP projects if still around (`gcp-pubsub-01`, `gen-lang-client-0107425200`, `jukeboxpostcard`, `psintegration01`). Destructive, so do manually from the GCP Console.
- If concerned about ADC token exposure in prior transcripts, run `gcloud auth application-default revoke` followed by `gcloud auth application-default login` and re-set the quota project.
- Decide whether to rename topic `demo-topic-01` to `my-topic` (to match the original setup doc) or to update the doc to match reality. Picking one eliminates the drift.

## Related docs

- service-account-for-shared-access.md - service-account record for the current shared-access setup.
- simple-gcp-setup-for-shared-access.md - receiving-side setup instructions.

