# Service account key for Pierre Mertz

Internal record of how and why we issued a Google Cloud service account JSON key to Pierre Mertz for the Simple-GCP demo, with operational instructions for the key's lifecycle.

## Context

On 2026-04-22, Pierre Mertz (`pierre.mertz@nokia.com`) needed access to the `Simple-GCP` console sample so he could clone the repo and run it against our live Pub/Sub project. The simplest access path, adding him as a user principal in project IAM, was not possible:

```
gcloud projects add-iam-policy-binding pubsub-demo-01-494119 \
  --member="user:pierre.mertz@nokia.com" \
  --role="roles/pubsub.editor"

ERROR: (gcloud.projects.add-iam-policy-binding) INVALID_ARGUMENT:
User pierre.mertz@nokia.com does not exist.
```

GCP rejects principal bindings that do not resolve to a real Google identity. `nokia.com` is a Microsoft 365 domain, so the email is not a Google account.

Rather than ask Pierre to create a personal Google account tied to his work email (Nokia IT policy risk, clicks on his side), we chose to issue him a service account JSON key. This is the path Google officially discourages for production but is acceptable for a time-boxed, trusted-collaborator demo with a strict billing budget on the project.

## What we created

| Item | Value |
|---|---|
| GCP project | `pubsub-demo-01-494119` |
| Service account id | `pubsub-demo-dev` |
| Service account email | `pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com` |
| Project role | `roles/pubsub.editor` |
| Key file (on Dennis's machine) | `C:\Users\dland\gcp-keys\pubsub-demo-dev.json` |
| Key id | `4fe1c26b01a8b6b7ccce1cc140c9384979307610` |
| Created under account | `dlandi2000@gmail.com` |
| Created on | 2026-04-22 |

## Commands that produced this state

```bash
# 1. Create the service account
gcloud iam service-accounts create pubsub-demo-dev \
    --display-name="Pub/Sub demo shared dev" \
    --description="Shared service account for Simple-GCP demo" \
    --project=pubsub-demo-01-494119

# 2. Grant Pub/Sub editor on the project
gcloud projects add-iam-policy-binding pubsub-demo-01-494119 \
    --member="serviceAccount:pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com" \
    --role="roles/pubsub.editor" \
    --condition=None

# 3. Create the JSON key
gcloud iam service-accounts keys create \
    "C:\Users\dland\gcp-keys\pubsub-demo-dev.json" \
    --iam-account=pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com \
    --project=pubsub-demo-01-494119
```

## Key handoff

The file at `C:\Users\dland\gcp-keys\pubsub-demo-dev.json` is a long-lived credential. It must reach Pierre without touching unsecured channels (email body, public chat, GitHub, screenshots, pastebins, AI chat logs).

Approved channels:

1. Shared password vault (1Password, Bitwarden) with the attachment scoped to Pierre only.
2. GPG-encrypted email using Pierre's public key.
3. Password-protected 7z archive (AES-256) sent over any channel, with the password delivered over a different channel (voice or SMS).

Do not:

- Commit to any repo, including private ones.
- Paste contents into issue trackers, chat, AI assistants, or notes services.
- Email unencrypted.
- Screenshot any portion of the file.

## Lifecycle and revocation

See `simple-gcp-setup-for-pierre.md` for the receiving-side instructions Pierre follows.

List the current keys on this service account:

```bash
gcloud iam service-accounts keys list \
  --iam-account=pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com
```

Revoke a specific key by id (Pierre's key id is `4fe1c26b01a8b6b7ccce1cc140c9384979307610`) when he is done or if the key is suspected leaked:

```bash
gcloud iam service-accounts keys delete 4fe1c26b01a8b6b7ccce1cc140c9384979307610 \
  --iam-account=pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com
```

Rotate the key (issue a new one, confirm Pierre's machine is using it, then delete the old one):

```bash
# New key
gcloud iam service-accounts keys create \
  "C:\Users\dland\gcp-keys\pubsub-demo-dev-new.json" \
  --iam-account=pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com

# Old key, after Pierre confirms new key works
gcloud iam service-accounts keys delete <OLD_KEY_ID> \
  --iam-account=pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com
```

Delete the entire service account when the demo ends:

```bash
gcloud iam service-accounts delete \
  pubsub-demo-dev@pubsub-demo-01-494119.iam.gserviceaccount.com
```

## Housekeeping reminders

- Calendar reminder for **2026-07-22** (90 days from issuance): either rotate or delete the key.
- Do not set `GOOGLE_APPLICATION_CREDENTIALS` to this key on the issuing machine. Local ADC user credentials are already sufficient there, and defining the env var would override them.
- Keep the key file out of any directory that is, or could become, a git working tree.
- If the budget alert on `pubsub-demo-01-494119` fires unexpectedly, consider the key leaked until proven otherwise and revoke immediately.

## Related docs

- `google-cloud-pubsub-dotnet8-local-setup.md` - original local setup for .NET 8 and Pub/Sub.
- `simple-gcp-session-2026-04-22.md` - session log for the Simple-GCP console project.
- `simple-gcp-setup-for-pierre.md` - receiving-side instructions for Pierre.
