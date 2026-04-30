# Simple-GCP

Minimal .NET 8 console app for connecting to Google Cloud Pub/Sub.

Current behavior:
- authenticates with a Google service account JSON key
- reads configuration from `appsettings.json`
- uses environment variables as optional overrides
- subscribes to one Pub/Sub subscription and prints the first message it receives
- exits after receiving one message, or after a 30-second timeout

## Prerequisites

- .NET 8 SDK installed
- a Google service account JSON key file with Pub/Sub access

## 1. Put the service account key outside the repo

Do not place the JSON key anywhere under this git working tree.

Recommended folder:
- `C:\Users\<you>\gcp-keys\`

Example:
- `C:\Users\landi\gcp-keys\ssu-pubsub-serviceaccount-01.json`

## 2. Update `appsettings.json`

This project reads its runtime settings from:
- `Simple-GCP\appsettings.json`

Current shape:

```json
{
  "GoogleCloud": {
	"ProjectId": "ssu-pubsub-proj",
	"GoogleApplicationCredentials": "C:\\Users\\landi\\gcp-keys\\ssu-pubsub-serviceaccount-01.json"
  },
  "PubSub": {
	"TopicId": "SSU-1-PubSub-Topic",
	"SubscriptionId": "SSU-1-PubSub-Topic-sub"
  }
}
```

Set these values correctly for your machine:
- `GoogleCloud:ProjectId`
- `GoogleCloud:GoogleApplicationCredentials`
- `PubSub:TopicId`
- `PubSub:SubscriptionId`

Notes:
- `GoogleApplicationCredentials` must point to the local JSON key file
- the app will set `GOOGLE_APPLICATION_CREDENTIALS` automatically from this value if that env var is not already defined
- environment variables still override the values in `appsettings.json`

## 3. Run the app

From the repo root:

```powershell
dotnet run --project Simple-GCP\Simple-GCP.csproj
```

Or in Visual Studio:
- open `GCP-PubSub.slnx`
- set `Simple-GCP` as the startup project
- press `F5`

## Expected output

```text
Project: ssu-pubsub-proj
Topic: SSU-1-PubSub-Topic
Subscription: SSU-1-PubSub-Topic-sub
Received message ID: <message-id>
Received payload: <message-text>
```

If no message is available for the subscription, the app prints:

```text
Timed out waiting for a message.
```

## Troubleshooting

### Credentials file not found

Check:
- the file exists at the path in `appsettings.json`
- the path is correct for the current Windows user
- the file is outside the repo

### Authentication error

Make sure the service account key is valid and has Pub/Sub permissions.

### Topic or subscription not found

Make sure these values in `appsettings.json` match the real GCP resource ids exactly:
- `ProjectId`
- `TopicId`
- `SubscriptionId`

## Security reminders

- never commit the JSON key file
- never paste its contents into repo files
- keep the key outside any git working tree
