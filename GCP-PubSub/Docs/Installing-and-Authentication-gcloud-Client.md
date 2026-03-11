# Installing and Authenticating the `gcloud` Client

This document describes how to install the Google Cloud SDK (`gcloud`) on Windows and configure authentication so the samples in this repository can access Google Cloud Pub/Sub.

## Install `gcloud`

If `winget` is available, install the Google Cloud SDK from PowerShell:

`winget install --id Google.CloudSDK --accept-source-agreements --accept-package-agreements --silent`

After installation, the executable may be available at:

`C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd`

Verify the installation:

`gcloud --version`

If `gcloud` is not yet on `PATH` in the current shell, use the full path:

`& 'C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd' --version`

## Add `gcloud` to the Environment `PATH`

If the `gcloud` command is not recognized, add the Google Cloud SDK `bin` folder to your Windows Environment `PATH`.

Typical install location:

`C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin`

### Temporary for the current PowerShell session

`$env:PATH += ';C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin'`

### Persist for the current user

`[System.Environment]::SetEnvironmentVariable('PATH', $env:PATH + ';C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin', 'User')`

After updating `PATH`, open a new terminal window and verify:

`gcloud --version`

## Set the Default Google Cloud Project

Set the active project used by the SDK:

`gcloud config set project gcp-subs-prj-prod-ipm-8a77`

If `gcloud` is not on `PATH`, use:

`& 'C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd' config set project gcp-subs-prj-prod-ipm-8a77`

## Authenticate the CLI

Sign in with your Google account:

`gcloud auth login`

Or, with the full path:

`& 'C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd' auth login`

This step authenticates the CLI itself.

## Configure Application Default Credentials

Create Application Default Credentials for local development:

`gcloud auth application-default login`

Or, with the full path:

`& 'C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd' auth application-default login`

This step is required for the .NET samples in this repository because the Google client libraries use Application Default Credentials.

## Alternative: Use a Service Account Key

Instead of `gcloud auth application-default login`, you can point the SDK and application to a service account JSON key file.

In PowerShell:

`$env:GOOGLE_APPLICATION_CREDENTIALS="C:\path\to\service-account.json"`

## Run the Console Sample

After authentication is configured, run the console sample:

`dotnet run --project samples\GCP.PubSub.Console\GCP.PubSub.Console.csproj`

Expected behavior:

- the sample starts successfully
- available topics are listed
- available subscriptions are listed
- publish and subscribe operations can proceed

## Troubleshooting

### `Your default credentials were not found`

Run:

`gcloud auth application-default login`

### `gcloud` is not recognized

Open a new terminal window so the updated `PATH` is loaded, or use the full executable path:

`& 'C:\Program Files (x86)\Google\Cloud SDK\google-cloud-sdk\bin\gcloud.cmd' --version`

### Project is not set correctly

Verify the active project:

`gcloud config get-value project`
