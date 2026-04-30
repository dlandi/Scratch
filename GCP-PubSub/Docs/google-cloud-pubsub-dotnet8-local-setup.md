# Google Cloud Pub/Sub from a Local .NET 8 Console App

## Objective

Create a Google Cloud account, create a Google Cloud project, enable Pub/Sub, provision a topic and pull subscription, and access Pub/Sub from a local .NET 8 console application running on your machine.

This guide uses:

- Google Cloud Console for account, billing, and project setup
- `gcloud` CLI for provisioning and local authentication
- Application Default Credentials (ADC) for local .NET authentication
- A .NET 8 console app using the `Google.Cloud.PubSub.V1` client library

---

## 1. What you will end up with

By the end of this guide you will have:

- A Google Cloud account
- A Google Cloud project
- Billing enabled for that project
- The Pub/Sub API enabled
- One topic
- One pull subscription
- A local .NET 8 console app that publishes a message and then receives it

---

## 2. Prerequisites

Before you start, make sure you have:

- A Google account
- .NET 8 SDK installed locally
- A terminal available on your machine
- Permission to create a Google Cloud billing account and project

Useful verification commands:

```bash
dotnet --version
gcloud --version
```

If `gcloud` is not installed yet, you will install it in Step 5.

---

## 3. Create your Google Cloud account

1. Go to Google Cloud and create an account.
2. If you are a new customer, start the free trial if you want to use the trial credits.
3. Sign in to the Google Cloud Console.

Notes:

- Google Cloud projects are the unit where APIs are enabled and resources are created.
- A project must be linked to an active Cloud Billing account in order to use Google Cloud services.

---

## 4. Create billing and a project

### Option A - Use the Google Cloud Console

1. Create or confirm your Cloud Billing account.
2. Create a new project.
3. Link the project to the billing account.
4. Copy the project ID. You will need it later.

Suggested example values:

- Project name: `PubSub Demo`
- Project ID: `your-pubsub-demo-123456`

Project ID rules you should remember:

- 6 to 30 characters
- lowercase letters, numbers, and hyphens only
- must start with a letter
- cannot end with a hyphen
- cannot be reused once used previously

### Option B - Create the project with `gcloud`

After `gcloud` is installed and initialized:

```bash
gcloud projects create PROJECT_ID
gcloud config set project PROJECT_ID
```

Replace `PROJECT_ID` with your real project ID.

Example:

```bash
gcloud projects create dennis-pubsub-demo-123456
gcloud config set project dennis-pubsub-demo-123456
```

Important:

- Creating the project alone does not automatically make it usable for Pub/Sub.
- Make sure billing is enabled for the project.

---

## 5. Install and initialize the Google Cloud CLI

Install the Google Cloud CLI from the official installer for your OS.

After installation, initialize it:

```bash
gcloud init
```

During initialization:

1. Sign in with your Google account.
2. Select the project you created.
3. Let `gcloud` store the default configuration.

You can verify the currently selected project with:

```bash
gcloud config get-value project
```

If needed, set it again:

```bash
gcloud config set project PROJECT_ID
```

---

## 6. Enable the Pub/Sub API

Run:

```bash
gcloud services enable pubsub.googleapis.com
```

This enables Pub/Sub for the current project.

---

## 7. Configure local authentication for your .NET app

For local development, you can use either user-based Application Default Credentials or a service account key file.

### Option A: user-based ADC through `gcloud`

Run:

```bash
gcloud auth application-default login
```

This creates the local credential file that Google Cloud client libraries use automatically.

### Recommended: set the quota project

This avoids a common local-development error where user credentials do not have a usable quota project.

```bash
gcloud auth application-default set-quota-project PROJECT_ID
```

Replace `PROJECT_ID` with your actual project ID.

Example:

```bash
gcloud auth application-default set-quota-project dennis-pubsub-demo-123456
```

### Option B: service account key file through `GOOGLE_APPLICATION_CREDENTIALS`

If you already have a service account JSON key, set `GOOGLE_APPLICATION_CREDENTIALS` to that file instead of using `gcloud auth application-default login`.

PowerShell example:

```powershell
$env:GOOGLE_APPLICATION_CREDENTIALS="C:\Users\landi\gcp-keys\ssu-pubsub-proj-1c74130096a1.json"
```

On the current machine in this repo, the sample is configured this way and targets project `ssu-pubsub-proj`.

### Optional: use service account impersonation instead of user credentials

If you want a more production-like setup without downloading a service account key, Google documents service account impersonation for local ADC and lists C# as supported.

Example pattern:

```bash
gcloud auth application-default login --impersonate-service-account SERVICE_ACCOUNT_EMAIL
```

For this guide, plain user-based ADC is sufficient.

---

## 8. Create the Pub/Sub topic and pull subscription

Create a topic:

```bash
gcloud pubsub topics create my-topic
```

Create a pull subscription:

```bash
gcloud pubsub subscriptions create my-sub --topic=my-topic
```

At this point your minimal Pub/Sub environment exists.

---

## 9. Smoke test the Pub/Sub resources with `gcloud`

Publish a test message:

```bash
gcloud pubsub topics publish my-topic --message="hello"
```

Pull the message:

```bash
gcloud pubsub subscriptions pull my-sub --auto-ack
```

If that works, your Google Cloud project, Pub/Sub API, topic, and subscription are all provisioned correctly.

---

## 10. Create the local .NET 8 console app

Create a new console project:

```bash
dotnet new console -n GcpPubSubDemo --framework net8.0
```

Move into the project folder:

```bash
cd GcpPubSubDemo
```

Add the Google Pub/Sub client library:

```bash
dotnet package add Google.Cloud.PubSub.V1
```

---

## 11. Replace `Program.cs`

Replace the contents of `Program.cs` with the following:

```csharp
using Google.Cloud.PubSub.V1;
using System;
using System.Threading;
using System.Threading.Tasks;

string? projectId = Environment.GetEnvironmentVariable("GCP_PROJECT_ID");
string? topicId = Environment.GetEnvironmentVariable("PUBSUB_TOPIC_ID");
string? subscriptionId = Environment.GetEnvironmentVariable("PUBSUB_SUBSCRIPTION_ID");

if (string.IsNullOrWhiteSpace(projectId))
{
    throw new InvalidOperationException("Set the GCP_PROJECT_ID environment variable.");
}

if (string.IsNullOrWhiteSpace(topicId))
{
    throw new InvalidOperationException("Set the PUBSUB_TOPIC_ID environment variable.");
}

if (string.IsNullOrWhiteSpace(subscriptionId))
{
    throw new InvalidOperationException("Set the PUBSUB_SUBSCRIPTION_ID environment variable.");
}

TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

Console.WriteLine($"Project: {projectId}");
Console.WriteLine($"Topic: {topicId}");
Console.WriteLine($"Subscription: {subscriptionId}");

PublisherClient publisher = await PublisherClient.CreateAsync(topicName);
string payload = $"Hello from .NET 8 at {DateTimeOffset.UtcNow:O}";
string messageId = await publisher.PublishAsync(payload);
Console.WriteLine($"Published message ID: {messageId}");
await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));

SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
var receivedSignal = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

Task subscriberTask = subscriber.StartAsync((message, cancellationToken) =>
{
    string text = message.Data.ToStringUtf8();
    Console.WriteLine($"Received message ID: {message.MessageId}");
    Console.WriteLine($"Received payload: {text}");

    receivedSignal.TrySetResult(true);
    return Task.FromResult(SubscriberClient.Reply.Ack);
});

using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
await using var registration = timeoutCts.Token.Register(() => receivedSignal.TrySetCanceled(timeoutCts.Token));

try
{
    await receivedSignal.Task;
}
catch (OperationCanceledException)
{
    Console.WriteLine("Timed out waiting for a message.");
}
finally
{
    await subscriber.StopAsync(CancellationToken.None);
    await subscriberTask;
}
```

What this code does:

1. Reads your project, topic, and subscription names from environment variables.
2. Uses ADC automatically. You do not put credentials in code.
3. Creates a publisher client and publishes a message.
4. Creates a subscriber client and listens for a message.
5. Acknowledges the message once received.

---

## 12. Set environment variables and run the app

### PowerShell

```powershell
$env:GCP_PROJECT_ID="ssu-pubsub-proj"
$env:PUBSUB_TOPIC_ID="SSU-1-PubSub-Topic"
$env:PUBSUB_SUBSCRIPTION_ID="SSU-1-PubSub-Topic-sub"
dotnet run
```

### Command Prompt

```cmd
set GCP_PROJECT_ID=ssu-pubsub-proj
set PUBSUB_TOPIC_ID=SSU-1-PubSub-Topic
set PUBSUB_SUBSCRIPTION_ID=SSU-1-PubSub-Topic-sub
dotnet run
```

### Bash

```bash
export GCP_PROJECT_ID="ssu-pubsub-proj"
export PUBSUB_TOPIC_ID="SSU-1-PubSub-Topic"
export PUBSUB_SUBSCRIPTION_ID="SSU-1-PubSub-Topic-sub"
dotnet run
```

Expected output will look roughly like this:

```text
Project: ssu-pubsub-proj
Topic: SSU-1-PubSub-Topic
Subscription: SSU-1-PubSub-Topic-sub
Published message ID: 1234567890123456
Received message ID: 1234567890123456
Received payload: Hello from .NET 8 at 2026-04-22T12:34:56.7890000+00:00
```

---

## 13. Troubleshooting

### Error: API not enabled

Symptom:

```text
PERMISSION_DENIED
```

Fix:

```bash
gcloud services enable pubsub.googleapis.com
```

### Error: no quota project or unknown project used for request

Fix these in order:

```bash
gcloud config set project PROJECT_ID
gcloud auth application-default login
gcloud auth application-default set-quota-project PROJECT_ID
```

### Error: topic or subscription not found

Check:

```bash
gcloud config get-value project
gcloud pubsub topics list
gcloud pubsub subscriptions list
```

Make sure the environment variables match the names you actually created.

### Error: authentication not found

If you are using user-based ADC, recreate it locally:

```bash
gcloud auth application-default login
```

If you are using a service account key file, verify that `GOOGLE_APPLICATION_CREDENTIALS` points to the right file and that the file still exists.

---

## 14. Cleanup

If you are finished and want to remove everything created for this walkthrough:

```bash
gcloud pubsub subscriptions delete my-sub
gcloud pubsub topics delete my-topic
```

If you also want to remove the locally created ADC credentials:

```bash
gcloud auth application-default revoke
```

If you created a throwaway project for this exercise, you can delete the project from the Google Cloud Console.

---

## 15. Recommended next step

Once this works, the next sensible step is to split the demo into:

- a dedicated publisher app
- a dedicated subscriber app
- configuration from `appsettings.json` instead of shell environment variables
- a service account or workload identity strategy for non-local environments

---

## Official references

1. Google Cloud getting started: https://docs.cloud.google.com/docs/get-started
2. Cloud Billing account creation: https://docs.cloud.google.com/billing/docs/how-to/create-billing-account
3. Creating projects: https://docs.cloud.google.com/resource-manager/docs/creating-managing-projects
4. Install Google Cloud CLI: https://docs.cloud.google.com/sdk/docs/install-sdk
5. Set up ADC for local development: https://docs.cloud.google.com/docs/authentication/set-up-adc-local-dev-environment
6. Quota project guidance: https://docs.cloud.google.com/docs/quotas/set-quota-project
7. Pub/Sub client library quickstart: https://docs.cloud.google.com/pubsub/docs/publish-receive-messages-client-library
8. Pub/Sub gcloud quickstart: https://docs.cloud.google.com/pubsub/docs/publish-receive-messages-gcloud
9. Pub/Sub .NET client library reference: https://docs.cloud.google.com/dotnet/docs/reference/Google.Cloud.PubSub.V1/latest
10. `dotnet package add` reference: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-package-add
