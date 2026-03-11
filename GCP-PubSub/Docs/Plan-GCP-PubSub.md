# Plan: GCP-PubSub .NET Library

## Goal

Encapsulate Google Cloud Pub/Sub publish and subscribe C# examples into a reusable .NET library (`GCP.PubSub`) that any .NET client can consume. Multi-targeted for .NET 8, 9, and 10. Includes three sample client projects (Console, WinForms, Blazor Server) demonstrating library consumption.

---

## References

- [Pub/Sub Quickstart - Publish & Receive (C#)](https://docs.cloud.google.com/pubsub/docs/publish-receive-messages-client-library)
- [Google.Cloud.PubSub.V1 NuGet (v3.30.0)](https://www.nuget.org/packages/Google.Cloud.PubSub.V1)
- [.NET Client Library Reference](https://docs.cloud.google.com/dotnet/docs/reference/Google.Cloud.PubSub.V1/latest)

---

## Solution Structure

```
E:\Archive\GitHub\dlandi\Scratch\GCP-PubSub\
├── GCP-PubSub.sln
├── Directory.Build.props              ← shared project properties (no TFM — each project sets its own)
├── Directory.Packages.props           ← central package management
├── Docs\
│   └── Plan-GCP-PubSub.md
├── src\
│   └── GCP.PubSub\
│       ├── GCP.PubSub.csproj          ← net8.0;net9.0;net10.0
│       ├── IPubSubPublisher.cs
│       ├── IPubSubSubscriber.cs
│       ├── PubSubPublisher.cs
│       ├── PubSubSubscriber.cs
│       ├── PubSubOptions.cs
│       ├── Log.cs                     ← LoggerMessage source-generated log methods
│       └── ServiceCollectionExtensions.cs
├── samples\
│   ├── GCP.PubSub.Console\           ← net8.0  — generic host console app
│   │   ├── GCP.PubSub.Console.csproj
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── GCP.PubSub.WinForms\          ← net8.0-windows  — WinForms desktop app
│   │   ├── GCP.PubSub.WinForms.csproj
│   │   ├── Program.cs
│   │   ├── MainForm.cs
│   │   ├── MainForm.Designer.cs
│   │   └── appsettings.json
│   └── GCP.PubSub.BlazorServer\      ← net8.0  — Blazor Server web app
│       ├── GCP.PubSub.BlazorServer.csproj
│       ├── Program.cs
│       ├── Components\
│       │   ├── App.razor
│       │   ├── Routes.razor
│       │   ├── _Imports.razor
│       │   ├── Layout\
│       │   │   ├── MainLayout.razor
│       │   │   └── NavMenu.razor
│       │   └── Pages\
│       │       ├── Home.razor
│       │       └── PubSub.razor       ← publish/subscribe demo page
│       ├── wwwroot\
│       │   └── app.css
│       └── appsettings.json
└── tests\
    └── GCP.PubSub.Tests\
        ├── GCP.PubSub.Tests.csproj    ← net8.0;net9.0;net10.0
        ├── PubSubPublisherTests.cs
        └── PubSubSubscriberTests.cs
```

---

## Step 1: Create Solution and Projects

### 1a. `Directory.Build.props` — shared properties

```xml
<Project>
  <PropertyGroup>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <LangVersion>latest</LangVersion>
    <IsTrimmable>true</IsTrimmable>
  </PropertyGroup>
</Project>
```

> `TargetFrameworks` is intentionally **not** set here — each project declares its own. The library and tests multi-target `net8.0;net9.0;net10.0`, while sample apps target `net8.0` (or `net8.0-windows` for WinForms).
>
> **Note on AOT**: `IsAotCompatible` is not set because `Google.Cloud.PubSub.V1` relies on gRPC/Protobuf reflection, which is not AOT-safe. `IsTrimmable=true` is set to enable trim analysis warnings in our code without requiring the dependency graph to be fully trim-safe.

### 1b. `Directory.Packages.props` — central package management

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="Google.Cloud.PubSub.V1" Version="3.30.0" />
    <PackageVersion Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="8.0.2" />
    <PackageVersion Include="Microsoft.Extensions.Options" Version="8.0.2" />
    <PackageVersion Include="Microsoft.Extensions.Options.DataAnnotations" Version="8.0.0" />
    <PackageVersion Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="8.0.0" />
    <PackageVersion Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.2" />
    <!-- Sample app packages -->
    <PackageVersion Include="Microsoft.Extensions.Hosting" Version="8.0.1" />
    <!-- Test packages -->
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
    <PackageVersion Include="xunit" Version="2.9.3" />
    <PackageVersion Include="xunit.runner.visualstudio" Version="2.8.2" />
    <PackageVersion Include="NSubstitute" Version="5.3.0" />
  </ItemGroup>
</Project>
```

### 1c. `GCP-PubSub.sln`

Solution file at the repo root containing all 5 projects (library, tests, 3 samples). Solution folders: `src`, `tests`, `samples`.

### 1d. `src/GCP.PubSub/GCP.PubSub.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
    <RootNamespace>GCP.PubSub</RootNamespace>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Google.Cloud.PubSub.V1" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" />
    <PackageReference Include="Microsoft.Extensions.Options" />
    <PackageReference Include="Microsoft.Extensions.Options.DataAnnotations" />
    <PackageReference Include="Microsoft.Extensions.Options.ConfigurationExtensions" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" />
  </ItemGroup>
</Project>
```

> Versions omitted from `PackageReference` — resolved by `Directory.Packages.props` (Central Package Management).

### 1e. `tests/GCP.PubSub.Tests/GCP.PubSub.Tests.csproj`

- **Multi-targeted**: `<TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>` (verifies compilation on all TFMs)
- References: `GCP.PubSub`, `xunit`, `NSubstitute`, `Microsoft.NET.Test.Sdk`

> **NSubstitute over Moq**: Moq v4.20+ bundled [SponsorLink](https://github.com/devlooped/moq/issues/1372), which scanned developer emails during builds. The .NET community [moved to NSubstitute](https://www.dimitrilaaraybi.com/blog/moqtonsubstitute/) as the preferred mocking framework.

### 1f. Sample project `.csproj` files — see Steps 8–10

---

## Step 2: Configuration — `PubSubOptions.cs`

Options class following the .NET Options pattern with **data annotation validation** (.NET 8+):

```csharp
using System.ComponentModel.DataAnnotations;

public class PubSubOptions
{
    [Required(AllowEmptyStrings = false)]
    public string ProjectId { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string TopicId { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string SubscriptionId { get; set; } = string.Empty;
}
```

Bindable from `appsettings.json` via `IOptions<PubSubOptions>`. Validated at startup via `ValidateDataAnnotations().ValidateOnStart()` (see Step 5).

---

## Step 3: Publish Interface & Implementation

### `IPubSubPublisher.cs`

```csharp
public interface IPubSubPublisher : IAsyncDisposable
{
    Task<string> PublishAsync(string message, CancellationToken cancellationToken = default);
    Task<string> PublishAsync(string message, IDictionary<string, string>? attributes,
                              CancellationToken cancellationToken = default);
    Task<int> PublishBatchAsync(IEnumerable<string> messages,
                                CancellationToken cancellationToken = default);
}
```

### `PubSubPublisher.cs`

- Wraps `PublisherClient` from `Google.Cloud.PubSub.V1`.
- Lazily creates `PublisherClient` via `PublisherClient.CreateAsync(TopicName)`.
- `PublishAsync(string)` — publishes a single message, returns the message ID.
- `PublishAsync(string, attributes)` — publishes with custom attributes on `PubsubMessage`.
- `PublishBatchAsync(IEnumerable<string>)` — parallel publish (from the quickstart example), returns count of successfully published messages. The `CancellationToken` controls the batch loop; Google's underlying `PublisherClient.PublishAsync` does not accept a token directly.
- `IAsyncDisposable.DisposeAsync()` — calls `publisher.ShutdownAsync(TimeSpan.FromSeconds(15))`.
- Uses **`[LoggerMessage]` source-generated** log methods (see `Log.cs`) for high-performance structured logging — avoids boxing, allocations, and message template parsing at runtime.
- All `await` calls use **`ConfigureAwait(false)`** (library best practice — avoids deadlocks when consumed from synchronous contexts).

Key design: the `PublisherClient` is held as a long-lived singleton internally (as recommended by Google's docs for high-throughput scenarios). Thread-safe lazy initialization via `SemaphoreSlim` + null-check (not `Lazy<Task<T>>`, which caches faulted tasks permanently and prevents retry on transient failures).

---

## Step 4: Subscribe Interface & Implementation

### `IPubSubSubscriber.cs`

```csharp
public interface IPubSubSubscriber : IAsyncDisposable
{
    Task<int> PullMessagesAsync(Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
                                CancellationToken cancellationToken = default);
    Task<int> PullMessagesAsync(Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
                                TimeSpan listenDuration,
                                CancellationToken cancellationToken = default);
}
```

### `PubSubSubscriber.cs`

- Wraps `SubscriberClient` from `Google.Cloud.PubSub.V1`.
- **Per-call lifecycle**: Each `PullMessagesAsync` invocation creates a new `SubscriberClient` via `SubscriberClient.CreateAsync(SubscriptionName)`, starts it, waits for the listen duration, then stops it. Google's `SubscriberClient` is single-use (create → start → stop; cannot be restarted), so a new instance is required per call.
- The `handler` callback receives: message text, attributes dictionary, and a cancellation token. Returns `true` to ACK, `false` to NACK.
- Default listen duration: 5 seconds (matching the quickstart). Overload accepts custom `TimeSpan`.
- Uses **`TimeProvider`** (.NET 8 BCL abstraction) for the listen-duration delay instead of raw `Task.Delay`. This enables deterministic testing with `FakeTimeProvider` from `Microsoft.Extensions.TimeProvider.Testing`.
- `IAsyncDisposable.DisposeAsync()` — safety net that stops any currently active subscriber if `PullMessagesAsync` is still running.
- Uses **`[LoggerMessage]` source-generated** log methods (see `Log.cs`).
- All `await` calls use **`ConfigureAwait(false)`**.
- The wrapper class itself is registered as a singleton (it's stateless except for tracking the active subscriber); the underlying `SubscriberClient` instances are ephemeral per pull call.

---

## Step 5: Dependency Injection — `ServiceCollectionExtensions.cs`

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGcpPubSub(
        this IServiceCollection services,
        Action<PubSubOptions> configure)
    {
        services.AddOptions<PubSubOptions>()
            .Configure(configure)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(TimeProvider.System);
        services.TryAddSingleton<IPubSubPublisher, PubSubPublisher>();
        services.TryAddSingleton<IPubSubSubscriber, PubSubSubscriber>();
        return services;
    }
}
```

Key improvements over naive `services.Configure()`:
- **`ValidateDataAnnotations()`** — validates `[Required]` attributes on `PubSubOptions` properties.
- **`ValidateOnStart()`** — fails fast at host startup if options are invalid (not on first use).
- **`TryAddSingleton`** — allows consumers to substitute their own implementations before calling `AddGcpPubSub`.
- **`TimeProvider.System`** — registers the system `TimeProvider`; tests can replace with `FakeTimeProvider`.

Second overload for `IConfiguration`-bound scenarios (used by sample apps):

```csharp
public static IServiceCollection AddGcpPubSub(
    this IServiceCollection services,
    string configSectionPath = "PubSub")
{
    services.AddOptions<PubSubOptions>()
        .BindConfiguration(configSectionPath)
        .ValidateDataAnnotations()
        .ValidateOnStart();

    services.AddSingleton(TimeProvider.System);
    services.TryAddSingleton<IPubSubPublisher, PubSubPublisher>();
    services.TryAddSingleton<IPubSubSubscriber, PubSubSubscriber>();
    return services;
}
```

Usage examples:

```csharp
// Lambda-based (explicit values):
builder.Services.AddGcpPubSub(opts =>
{
    opts.ProjectId = "my-project";
    opts.TopicId = "my-topic";
    opts.SubscriptionId = "my-subscription";
});

// Config-bound (from appsettings.json "PubSub" section):
builder.Services.AddGcpPubSub();
```

---

## Step 6: Source-Generated Logging — `Log.cs`

```csharp
internal static partial class Log
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Published message {MessageId}")]
    internal static partial void PublishedMessage(this ILogger logger, string messageId);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to publish message")]
    internal static partial void PublishFailed(this ILogger logger, Exception exception);

    [LoggerMessage(Level = LogLevel.Information, Message = "Received message {MessageId}: {Text}")]
    internal static partial void ReceivedMessage(this ILogger logger, string messageId, string text);
}
```

Uses `[LoggerMessage]` attribute ([source generator](https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator)) — zero-allocation logging at runtime; the compiler generates the `IsEnabled` guard and structured-log formatting.

---

## Step 7: Unit Tests

### `PubSubPublisherTests.cs`

- Verify options validation rejects empty `ProjectId` / `TopicId` at startup.
- Verify `PublishBatchAsync` with empty collection returns 0.

### `PubSubSubscriberTests.cs`

- Verify options validation rejects empty `ProjectId` / `SubscriptionId` at startup.
- Verify `PullMessagesAsync` respects cancellation.
- Verify `TimeProvider` is used for listen-duration delay (using `FakeTimeProvider`).

> Note: Full integration tests require a live GCP project. Unit tests focus on input validation and configuration. Integration tests can be added later with the `[Trait("Category", "Integration")]` convention.

---

## Step 8: Console Sample — `samples/GCP.PubSub.Console`

### `GCP.PubSub.Console.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\src\GCP.PubSub\GCP.PubSub.csproj" />
    <PackageReference Include="Microsoft.Extensions.Hosting" />
  </ItemGroup>
</Project>
```

### `appsettings.json`

```json
{
  "PubSub": {
    "ProjectId": "your-gcp-project-id",
    "TopicId": "your-topic-id",
    "SubscriptionId": "your-subscription-id"
  }
}
```

> All three sample apps share this same `appsettings.json` structure.

### `Program.cs`

- Uses `Host.CreateDefaultBuilder()` → `ConfigureServices` → `AddGcpPubSub()` (config-bound overload).
- Resolves `IPubSubPublisher` and `IPubSubSubscriber` from the host's `IServiceProvider`.
- Demonstrates:
  1. Publishing a single message via `PublishAsync(string)`.
  2. Publishing a batch via `PublishBatchAsync(IEnumerable<string>)`.
  3. Subscribing via `PullMessagesAsync` with a handler that prints each message to console.
- Calls `host.StopAsync()` after the demo completes.

```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddGcpPubSub(); // binds "PubSub" section from appsettings.json
    })
    .Build();

await host.StartAsync();

var publisher = host.Services.GetRequiredService<IPubSubPublisher>();
var subscriber = host.Services.GetRequiredService<IPubSubSubscriber>();

// Publish
var messageId = await publisher.PublishAsync("Hello from Console!");
Console.WriteLine($"Published: {messageId}");

// Subscribe
var count = await subscriber.PullMessagesAsync(
    async (text, attributes, ct) =>
    {
        Console.WriteLine($"Received: {text}");
        return true; // ACK
    },
    TimeSpan.FromSeconds(10));

Console.WriteLine($"Received {count} messages.");
await host.StopAsync();
```

---

## Step 9: WinForms Sample — `samples/GCP.PubSub.WinForms`

### `GCP.PubSub.WinForms.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net8.0-windows</TargetFramework>
    <UseWindowsForms>true</UseWindowsForms>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\src\GCP.PubSub\GCP.PubSub.csproj" />
    <PackageReference Include="Microsoft.Extensions.Hosting" />
  </ItemGroup>
</Project>
```

### `Program.cs`

- Builds a `Host` with `AddGcpPubSub()` (config-bound).
- Registers `MainForm` as a transient service.
- Starts the host, resolves `MainForm`, and runs it via `Application.Run()`.
- Disposes host on application exit.

```csharp
ApplicationConfiguration.Initialize();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((ctx, services) =>
    {
        services.AddGcpPubSub();
        services.AddTransient<MainForm>();
    })
    .Build();

await host.StartAsync();

var form = host.Services.GetRequiredService<MainForm>();
Application.Run(form);

await host.StopAsync();
```

### `MainForm.cs` / `MainForm.Designer.cs`

UI layout:
- **TextBox** (`txtMessage`) — message text input.
- **Button** (`btnPublish`) — publishes `txtMessage.Text` via `IPubSubPublisher.PublishAsync`.
- **Button** (`btnSubscribe`) — starts `IPubSubSubscriber.PullMessagesAsync` for a configurable duration.
- **ListBox** (`lstMessages`) — displays published message IDs and received messages.
- **StatusStrip** (`statusBar`) — shows operation status.

Key implementation details:
- Constructor receives `IPubSubPublisher` and `IPubSubSubscriber` via DI.
- Button click handlers use `async void` (standard for WinForms event handlers).
- Library's `ConfigureAwait(false)` ensures the UI thread is freed during Pub/Sub I/O; results are marshalled back to the UI via normal `await` continuation (no explicit `Invoke` needed because the top-level `await` captures `SynchronizationContext`).
- Error handling: `try/catch` around each operation, errors shown in `statusBar`.

---

## Step 10: Blazor Server Sample — `samples/GCP.PubSub.BlazorServer`

### `GCP.PubSub.BlazorServer.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\src\GCP.PubSub\GCP.PubSub.csproj" />
  </ItemGroup>
</Project>
```

> No `Microsoft.Extensions.Hosting` needed — the web SDK includes it. Blazor Server has built-in DI.

### `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddGcpPubSub(); // binds "PubSub" section from appsettings.json

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
```

### `Components/Pages/PubSub.razor`

- Route: `@page "/pubsub"`
- Render mode: `@rendermode InteractiveServer`
- Injects `IPubSubPublisher` and `IPubSubSubscriber`.
- UI elements:
  - **Input field** + **Publish button** — calls `PublishAsync`, displays returned message ID.
  - **Subscribe button** with duration selector — calls `PullMessagesAsync`, appends received messages to a list.
  - **Messages list** — shows published/received messages with timestamps.
  - **Status indicator** — shows "Publishing...", "Listening...", or idle state.
- Uses `StateHasChanged()` within the subscriber handler to update the UI as messages arrive.
- Error handling via `try/catch` with error message display.

```razor
@page "/pubsub"
@rendermode InteractiveServer
@inject IPubSubPublisher Publisher
@inject IPubSubSubscriber Subscriber

<h3>Pub/Sub Demo</h3>

<div>
    <input @bind="messageText" placeholder="Enter message..." />
    <button @onclick="PublishMessage" disabled="@isPublishing">Publish</button>
</div>

<div>
    <button @onclick="SubscribeMessages" disabled="@isSubscribing">
        Subscribe (@listenSeconds s)
    </button>
</div>

<h4>Messages</h4>
<ul>
    @foreach (var msg in messages)
    {
        <li>@msg</li>
    }
</ul>
```

### Blazor scaffolding files

Standard .NET 8 Blazor Server scaffolding:
- `App.razor` — root component with `<HeadOutlet>` and `<Routes>`.
- `Routes.razor` — `<Router>` component.
- `_Imports.razor` — global `@using` directives including `GCP.PubSub`.
- `Layout/MainLayout.razor` — layout with `@Body` and nav.
- `Layout/NavMenu.razor` — navigation with links to Home and PubSub pages.
- `Pages/Home.razor` — simple landing page.
- `wwwroot/app.css` — minimal styling.

---

## Step 11: Build Verification

- `dotnet build` the solution — verifies all 5 projects across their respective target frameworks.
- `dotnet test` the test project on all three TFMs.
- Verify each sample app compiles cleanly against `net8.0` (or `net8.0-windows` for WinForms).
- Ensure clean build with no warnings under `<TreatWarningsAsErrors>` (optional, recommend enabling).

---

## Implementation Order

| Step | Task | Files |
|------|------|-------|
| 1 | Create solution + build infrastructure | `.sln`, `Directory.Build.props`, `Directory.Packages.props`, `.csproj` files |
| 2 | `PubSubOptions` | `PubSubOptions.cs` |
| 3 | `IPubSubPublisher` + `PubSubPublisher` | 2 files |
| 4 | `IPubSubSubscriber` + `PubSubSubscriber` | 2 files |
| 5 | `ServiceCollectionExtensions` (both overloads) | 1 file |
| 6 | `Log` (source-generated logging) | `Log.cs` |
| 7 | Unit tests | 2 test files |
| 8 | Console sample | `.csproj`, `Program.cs`, `appsettings.json` |
| 9 | WinForms sample | `.csproj`, `Program.cs`, `MainForm.cs/.Designer.cs`, `appsettings.json` |
| 10 | Blazor Server sample | `.csproj`, `Program.cs`, Razor components, `appsettings.json` |
| 11 | Build & test verification | CLI commands |

---

## NuGet Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `Google.Cloud.PubSub.V1` | 3.30.0 | Core Pub/Sub client SDK |
| `Microsoft.Extensions.DependencyInjection.Abstractions` | 8.0.2 | `IServiceCollection` for DI registration |
| `Microsoft.Extensions.Options` | 8.0.2 | `IOptions<T>` pattern |
| `Microsoft.Extensions.Options.DataAnnotations` | 8.0.0 | `ValidateDataAnnotations()` + `ValidateOnStart()` |
| `Microsoft.Extensions.Options.ConfigurationExtensions` | 8.0.0 | `BindConfiguration()` for the config-bound `AddGcpPubSub` overload |
| `Microsoft.Extensions.Logging.Abstractions` | 8.0.2 | `ILogger<T>` + `[LoggerMessage]` source generator |

Sample apps additionally:

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.Extensions.Hosting` | 8.0.1 | Generic host for Console + WinForms (Blazor gets this from web SDK) |

Test project additionally:

| Package | Version | Purpose |
|---------|---------|---------|
| `xunit` | 2.9.3 | Test framework |
| `xunit.runner.visualstudio` | 2.8.2 | VS test adapter |
| `Microsoft.NET.Test.Sdk` | 17.12.0 | Test SDK |
| `NSubstitute` | 5.3.0 | Mocking (replaces Moq) |

---

## Key Design Decisions

1. **Interfaces first** — `IPubSubPublisher` / `IPubSubSubscriber` allow mocking in consumer tests.
2. **Options pattern with validation** — `[Required]` data annotations + `ValidateOnStart()` for fail-fast at host startup.
3. **IAsyncDisposable** — Proper cleanup of long-lived gRPC clients.
4. **Singleton registration** — Google recommends reusing `PublisherClient`/`SubscriberClient` across the app lifetime.
5. **Callback-based subscribe** — Consumer provides a `Func` handler instead of dealing with `SubscriberClient` internals; returns bool for ACK/NACK.
6. **Multi-target net8.0;net9.0;net10.0** — Broadest current LTS + current + preview support.
7. **M.E. package versions at 8.0.x** — Lowest common denominator for multi-targeting; higher TFMs resolve compatible versions automatically.
8. **`[LoggerMessage]` source generator** — Zero-allocation structured logging; compiler generates `IsEnabled` guards and formatting ([Microsoft guidance](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/library-guidance)).
9. **`ConfigureAwait(false)`** — All library awaits use `ConfigureAwait(false)` to prevent deadlocks in synchronous consumer contexts.
10. **`TimeProvider` (.NET 8)** — Subscriber delay uses `TimeProvider` instead of `Task.Delay` for [deterministic testability](https://andrewlock.net/exploring-the-dotnet-8-preview-avoiding-flaky-tests-with-timeprovider-and-itimer/).
11. **`SemaphoreSlim` lazy init** — Publisher client init uses `SemaphoreSlim` + null-check, not `Lazy<Task<T>>` (which caches faulted tasks permanently).
12. **Central Package Management** — `Directory.Packages.props` centralizes all NuGet versions; `Directory.Build.props` shares project properties ([Microsoft guidance](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)).
13. **NSubstitute over Moq** — Moq's [SponsorLink controversy](https://github.com/devlooped/moq/issues/1372) led the .NET community to adopt NSubstitute.
14. **`IsTrimmable=true`** — Enables trim analysis on our code. `IsAotCompatible` omitted because `Google.Cloud.PubSub.V1` uses reflection-heavy gRPC/Protobuf ([Microsoft guidance](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/prepare-libraries-for-trimming)).
15. **Two `AddGcpPubSub` overloads** — `Action<PubSubOptions>` for explicit values; `string configSectionPath` for `appsettings.json` binding via `BindConfiguration()`. Sample apps use the config-bound overload.
16. **Sample apps target `net8.0` only** — Apps are deployable artifacts, not libraries; no need to multi-target. WinForms uses `net8.0-windows` for WPF/WinForms SDK support.
17. **WinForms + Generic Host** — `Microsoft.Extensions.Hosting` provides DI integration without requiring manual `ServiceCollection` wiring. `MainForm` is registered as a transient and resolved from the host's `IServiceProvider`.
18. **Blazor `@rendermode InteractiveServer`** — .NET 8 Blazor Server rendering mode; enables `StateHasChanged()` for real-time UI updates as messages arrive from the subscriber callback.
