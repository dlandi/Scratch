#Requires -Version 5.1
<#
.SYNOPSIS
    Grant, revoke, or list Pub/Sub IAM access on the shared GCP project.

.DESCRIPTION
    Wraps `gcloud projects *-iam-policy-binding` so new developers can be
    onboarded to the Simple-GCP sample without clicking through the Cloud
    Console. Requires the gcloud CLI on PATH and an authenticated account
    with `resourcemanager.projects.setIamPolicy` on the target project
    (Owner or Project IAM Admin).

.PARAMETER Action
    grant  = add a role binding for the user
    revoke = remove a role binding for the user
    list   = show current role bindings (all human users by default, or
             just the specified user if -User is given)

.PARAMETER User
    The developer's Google account email. Required for grant and revoke.

.PARAMETER Role
    The IAM role to grant or revoke. Defaults to roles/pubsub.editor,
    which covers publish, pull, and topic/subscription management. For a
    narrower grant, use roles/pubsub.publisher, roles/pubsub.subscriber,
    or roles/pubsub.viewer.

.PARAMETER Project
    The GCP project id. Defaults to the Simple-GCP demo project.

.EXAMPLE
    .\manage-pubsub-access.ps1 grant -User teammate@example.com

.EXAMPLE
    .\manage-pubsub-access.ps1 grant -User teammate@example.com -Role roles/pubsub.subscriber

.EXAMPLE
    .\manage-pubsub-access.ps1 list

.EXAMPLE
    .\manage-pubsub-access.ps1 revoke -User teammate@example.com
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet('grant', 'revoke', 'list')]
    [string]$Action,

    [Parameter(Mandatory = $false, Position = 1)]
    [string]$User,

    [ValidateSet(
        'roles/pubsub.admin',
        'roles/pubsub.editor',
        'roles/pubsub.publisher',
        'roles/pubsub.subscriber',
        'roles/pubsub.viewer'
    )]
    [string]$Role = 'roles/pubsub.editor',

    [string]$Project = 'pubsub-demo-01-494119'
)

$ErrorActionPreference = 'Stop'

if (-not (Get-Command gcloud -ErrorAction SilentlyContinue)) {
    throw "gcloud CLI not found on PATH. Install from https://cloud.google.com/sdk/docs/install-sdk and re-run."
}

if ($Action -in @('grant', 'revoke') -and [string]::IsNullOrWhiteSpace($User)) {
    throw "-User is required when Action is '$Action'."
}

switch ($Action) {
    'grant' {
        Write-Host "Granting $Role to user:$User on project $Project..."
        gcloud projects add-iam-policy-binding $Project `
            --member="user:$User" `
            --role=$Role
        if ($LASTEXITCODE -ne 0) { throw "gcloud returned exit code $LASTEXITCODE." }
        Write-Host "Done. Ask the developer to run:" -ForegroundColor Green
        Write-Host "  gcloud auth login"
        Write-Host "  gcloud config set project $Project"
        Write-Host "  gcloud auth application-default login"
        Write-Host "  gcloud auth application-default set-quota-project $Project"
    }

    'revoke' {
        Write-Host "Revoking $Role from user:$User on project $Project..."
        gcloud projects remove-iam-policy-binding $Project `
            --member="user:$User" `
            --role=$Role
        if ($LASTEXITCODE -ne 0) { throw "gcloud returned exit code $LASTEXITCODE." }
        Write-Host "Done." -ForegroundColor Green
    }

    'list' {
        $filter = if ([string]::IsNullOrWhiteSpace($User)) {
            "bindings.members:user:*"
        }
        else {
            "bindings.members:user:$User"
        }

        gcloud projects get-iam-policy $Project `
            --flatten="bindings[].members" `
            --format="table(bindings.role, bindings.members)" `
            --filter="$filter"
        if ($LASTEXITCODE -ne 0) { throw "gcloud returned exit code $LASTEXITCODE." }
    }
}
