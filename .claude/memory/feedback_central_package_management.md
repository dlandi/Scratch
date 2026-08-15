---
name: Central package management in GCP-PubSub repo
description: The GCP-PubSub scratch repo uses central package management, so do not run `dotnet add package`
type: feedback
originSessionId: 8672e7cb-1f08-48c2-ae1e-c9a134a9b4e1
---
In `E:\Archive\GitHub\dlandi\Scratch\GCP-PubSub`, package versions are centrally managed via `Directory.Packages.props` (`ManagePackageVersionsCentrally=true`). Project csproj files use bare `<PackageReference Include="..." />` without a `Version=` attribute.

**Why:** Running `dotnet add package <name>` injects a `Version=` attribute into the csproj, which breaks central package management with NU1008. The user noticed this pattern when evaluating a Claude Web suggestion that said to run `dotnet add package Google.Cloud.PubSub.V1` against a project where the package was already referenced.

**How to apply:** To add a new dependency to a project in this repo, (1) add a `<PackageVersion Include="..." Version="..." />` line to `Directory.Packages.props`, (2) add a `<PackageReference Include="..." />` line (no version) to the csproj. Never suggest `dotnet add package` for this repo.
