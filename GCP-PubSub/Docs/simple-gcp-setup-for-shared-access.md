# Simple-GCP setup for shared access

This document walks you through getting the `Simple-GCP` console app running on your machine against our shared Google Cloud Pub/Sub project. You will receive one file from me, and you will do a small amount of local configuration on your side.

## What you will have at the end

A .NET 8 console app that publishes a message to a Google Pub/Sub topic and then receives the same message back through a pull subscription. The project, topic, and subscription already exist on my side; you do not need to create or manage them.

## What I will send you

A single file containing a Google Cloud service account key. Treat it like a password:

- Do not commit it to any repo.
- Do not paste its contents anywhere.
- Do not forward it to anyone without asking me first.

If the file is ever lost, stolen, or accidentally pushed, tell me immediately so I can revoke the key on the Google side. The key will stay valid until I do.

## What you need installed

1. Git for Windows.
2. .NET 8 SDK. Verify with `dotnet --version`. Expect `8.x.x`; anything higher also works.
3. Visual Studio 2022 (17.8 or newer) or Rider 2024.x. Optional, but convenient.
4. The Google Cloud CLI is NOT required to run the app. You only need it if you want to troubleshoot authentication from the command line.

## Step 1: Save the key file outside any git repo

Create a folder for cloud credentials, well away from any folder git watches:

```powershell
New-Item -ItemType Directory -Path "C:\Users\<you>\gcp-keys" -Force
```

Move the key file into that folder so the full path is:

```
C:\Users\<you>\gcp-keys\ssu-pubsub-proj-1c74130096a1.json
```

Replace `<you>` with your Windows username throughout this doc.

## Step 2: Set the GOOGLE_APPLICATION_CREDENTIALS environment variable

The Google Cloud .NET client library looks for this env var and, if set, uses the key file it points at.

In PowerShell, run:

```powershell
[Environment]::SetEnvironmentVariable(
  "GOOGLE_APPLICATION_CREDENTIALS",
	"C:\Users\<you>\gcp-keys\ssu-pubsub-proj-1c74130096a1.json",
  "User"
)
```

This writes the variable to your Windows user profile. It persists across reboots and across shells.

Close and reopen any terminal, IDE, or Visual Studio windows after running this so they pick up the new environment. New processes started from a pre-existing shell will not see the variable.

Confirm it stuck:

```powershell
$env:GOOGLE_APPLICATION_CREDENTIALS
```

You should see the full path to the key file. If you see nothing, your current shell was opened before Step 2 ran; open a fresh PowerShell window.

## Step 3: Clone the repo

```powershell
cd C:\source   # or wherever you keep source
git clone <repo-url>
cd GCP-PubSub
```

Replace `<repo-url>` with the URL I send you alongside this document.

## Step 4: Open the solution

Open `GCP-PubSub.slnx` in Visual Studio (or Rider). The solution contains several projects. The one you want is under the `Simple-GCP` solution folder.

## Step 5: Run Simple-GCP

In Visual Studio: right-click the `Simple-GCP` project in Solution Explorer, choose "Set as Startup Project", then press F5.

From the command line, this is equivalent:

```powershell
dotnet run --project Simple-GCP\Simple-GCP.csproj
```

The app reads three environment variables for the project id, topic, and subscription. Those values are already baked into `Simple-GCP\Properties\launchSettings.json`, so you do not need to set them yourself.

## Expected output

```
Project: ssu-pubsub-proj
Topic: SSU-1-PubSub-Topic
Subscription: SSU-1-PubSub-Topic-sub
Published message ID: <some 16-digit number>
Received message ID: <the same number>
Received payload: Hello from .NET 8 at 2026-04-22T...
```

The app exits on its own within about five seconds.

## If something goes wrong

### `Unhandled exception. System.InvalidOperationException: Set the GCP_PROJECT_ID environment variable.`

You are launching the app with an environment that does not include the `launchSettings.json` profile. In Visual Studio, make sure the green run button says "Simple-GCP", not "Simple-GCP (No launch profile)". From the command line, use the `--project` argument as shown above.

### `Grpc.Core.RpcException: Status(StatusCode="Unauthenticated", ...)`

The key file is not being found.

- Confirm the env var is set: `echo $env:GOOGLE_APPLICATION_CREDENTIALS`.
- Confirm the file at that path exists: `Test-Path $env:GOOGLE_APPLICATION_CREDENTIALS` (should return `True`).
- Confirm you restarted the terminal or IDE after setting the env var.
- Confirm the app is targeting project `ssu-pubsub-proj` through `Simple-GCP\Properties\launchSettings.json`.

### `Grpc.Core.RpcException: Status(StatusCode="PermissionDenied", ...)`

The key file is valid but the service account does not have the right role. Send me the full error; it may mean I need to re-grant the role on my side.

### `Grpc.Core.RpcException: Status(StatusCode="NotFound", "Resource not found (resource=SSU-1-PubSub-Topic).")`

The topic or subscription was deleted or renamed on my side. Ping me.

### The app hangs for 30 seconds then prints "Timed out waiting for a message."

Publish succeeded but subscribe did not complete in time. Rerun the app. If it keeps happening, ping me; your key may have `pubsub.publisher` but not `pubsub.subscriber`.

## Do not

- Do not set `GOOGLE_APPLICATION_CREDENTIALS` at the "Machine" scope. Keep it in your user profile.
- Do not commit the key file, ever. Check `git status` before every commit and verify nothing in `C:\Users\<you>\gcp-keys\` is referenced.
- Do not copy the key into a CI pipeline, container image, or publish output. It is valid until I manually revoke it.
- Do not share the key with anyone else on the team without asking me. I need to know who has it.

## Cleanup when you are done with the demo

Delete the key file:

```powershell
Remove-Item "C:\Users\<you>\gcp-keys\ssu-pubsub-proj-1c74130096a1.json"
```

Unset the env var:

```powershell
[Environment]::SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", $null, "User")
```

Let me know you are done so I can revoke the key on my side as well.
