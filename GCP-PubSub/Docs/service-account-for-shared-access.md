# Service account key for shared Simple-GCP access

Internal record of the Google Cloud service account JSON key currently used on this machine for the Simple-GCP demo, with operational instructions for the key's lifecycle.

## Context

The current machine now uses a Google Cloud service account JSON key instead of the original machine's user-based Application Default Credentials. The active key belongs to project `ssu-pubsub-proj` and is intended for local Simple-GCP development only.

This keeps credentials out of code while allowing the app to authenticate through `GOOGLE_APPLICATION_CREDENTIALS`.

## What we created

| Item | Value |
|---|---|
| GCP project | `ssu-pubsub-proj` |
| Service account id | `ssu-pubsub-serviceaccount-01` |
| Service account email | `ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com` |
| Project role | `roles/pubsub.editor` |
| Topic | `SSU-1-PubSub-Topic` |
| Key file (on this machine) | `C:\Users\landi\gcp-keys\ssu-pubsub-serviceaccount-01.json` |
| Key id | `1c74130096a141b5cbfda6b51cf151f5ae358c7e` |
| Windows user profile | `C:\Users\landi` |

## Commands that produced this state

```bash
# 1. Create the service account
gcloud iam service-accounts create ssu-pubsub-serviceaccount-01 \
	--display-name="Pub/Sub demo shared dev" \
	--description="Shared service account for Simple-GCP demo" \
	--project=ssu-pubsub-proj

# 2. Grant Pub/Sub editor on the project
gcloud projects add-iam-policy-binding ssu-pubsub-proj \
	--member="serviceAccount:ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com" \
	--role="roles/pubsub.editor" \
	--condition=None

# 3. Create the JSON key
gcloud iam service-accounts keys create \
	"C:\Users\landi\gcp-keys\ssu-pubsub-serviceaccount-01.json" \
	--iam-account=ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com \
	--project=ssu-pubsub-proj
```

## Key handling

The file under `C:\Users\landi\gcp-keys\` is a long-lived credential. It must stay out of unsecured channels (email body, public chat, GitHub, screenshots, pastebins, AI chat logs).

Approved channels:

1. Shared password vault (1Password, Bitwarden) with the attachment scoped only to the intended user.
2. GPG-encrypted email using the recipient's public key.
3. Password-protected 7z archive (AES-256) sent over any channel, with the password delivered over a different channel (voice or SMS).

Do not:

- Commit to any repo, including private ones.
- Paste contents into issue trackers, chat, AI assistants, or notes services.
- Email unencrypted.
- Screenshot any portion of the file.

## Lifecycle and revocation

See `simple-gcp-setup-for-shared-access.md` for the receiving-side setup flow.

List the current keys on this service account:

```bash
gcloud iam service-accounts keys list \
  --iam-account=ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com
```

Revoke a specific key by id (`1c74130096a141b5cbfda6b51cf151f5ae358c7e`) when the machine is retired from the demo or if the key is suspected leaked:

```bash
gcloud iam service-accounts keys delete 1c74130096a141b5cbfda6b51cf151f5ae358c7e \
  --iam-account=ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com
```

Rotate the key (issue a new one, confirm the machine is using it, then delete the old one):

```bash
# New key
gcloud iam service-accounts keys create \
	"C:\Users\landi\gcp-keys\ssu-pubsub-proj-<new-key-id>.json" \
  --iam-account=ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com

# Old key, after the replacement key works
gcloud iam service-accounts keys delete <OLD_KEY_ID> \
  --iam-account=ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com
```

Delete the entire service account when the demo ends:

```bash
gcloud iam service-accounts delete \
  ssu-pubsub-serviceaccount-01@ssu-pubsub-proj.iam.gserviceaccount.com
```

## Housekeeping reminders

- Set `GOOGLE_APPLICATION_CREDENTIALS` only on machines that should use this key, and clear it when the demo is finished.
- Keep the key file out of any directory that is, or could become, a git working tree.
- If the budget alert on `ssu-pubsub-proj` fires unexpectedly, consider the key leaked until proven otherwise and revoke immediately.

## Related docs

- `google-cloud-pubsub-dotnet8-local-setup.md` - original local setup for .NET 8 and Pub/Sub.
- `simple-gcp-session-2026-04-22.md` - session log and current machine notes for the Simple-GCP console project.
- `simple-gcp-setup-for-shared-access.md` - receiving-side setup instructions.
