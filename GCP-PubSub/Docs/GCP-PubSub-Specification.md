# GCP-PubSub Library Specification

## 1. Overview

**GCP.PubSub** is a .NET library that provides a simplified, dependency-injection-friendly wrapper around the Google Cloud Pub/Sub client SDK (`Google.Cloud.PubSub.V1`). It encapsulates the creation and lifecycle of `PublisherClient` and `SubscriberClient`, exposing clean interfaces (`IPubSubPublisher`, `IPubSubSubscriber`, `IPubSubAdmin`) that integrate with the Microsoft.Extensions ecosystem for configuration, logging, and DI.

**Namespace:** `GCP.PubSub`

**Target Frameworks:** `net8.0`, `net9.0`, `net10.0`

## 2. Solution Structure

```
GCP-PubSub/
  Directory.Build.props          # Shared build properties
  Directory.Packages.props       # Central Package Management
  GCP-PubSub.slnx               # Solution file
  src/
    GCP.PubSub/                  # Core library
      GCP.PubSub.csproj
      PubSubOptions.cs
      IPubSubPublisher.cs
      IPubSubSubscriber.cs
      PubSubPublisher.cs
      PubSubSubscriber.cs
      IPubSubAdmin.cs
      PubSubAdmin.cs
      ServiceCollectionExtensions.cs
      Log.cs
  tests/
    GCP.PubSub.Tests/            # Unit tests
      GCP.PubSub.Tests.csproj
      PubSubPublisherTests.cs
      PubSubSubscriberTests.cs
      PubSubAdminTests.cs
  samples/
    GCP.PubSub.Console/          # Console sample (net8.0)
    GCP.PubSub.WinForms/         # WinForms sample (net8.0-windows)
    GCP.PubSub.BlazorServer/     # Blazor Server sample (net8.0)
```

## 3. Build Infrastructure

### 3.1 Directory.Build.props

Shared properties applied to all projects in the repository:

| Property           | Value    | Purpose                                      |
|--------------------|----------|----------------------------------------------|
| `ImplicitUsings`   | `enable` | Auto-imports common System namespaces         |
| `Nullable`         | `enable` | Nullable reference types for all projects     |
| `LangVersion`      | `latest` | Latest C# language features                   |
| `IsTrimmable`      | `true`   | Marks assemblies as safe for IL trimming      |

Individual projects declare their own `TargetFramework(s)`.

### 3.2 Central Package Management

All NuGet package versions are managed centrally via `Directory.Packages.props`:

| Package                                                  | Version  | Used By       |
|----------------------------------------------------------|----------|---------------|
| `Google.Cloud.PubSub.V1`                                | 3.30.0   | Library       |
| `Microsoft.Extensions.DependencyInjection.Abstractions`  | 8.0.2    | Library       |
| `Microsoft.Extensions.Options`                           | 8.0.2    | Library       |
| `Microsoft.Extensions.Options.DataAnnotations`           | 8.0.0    | Library       |
| `Microsoft.Extensions.Options.ConfigurationExtensions`   | 8.0.0    | Library       |
| `Microsoft.Extensions.Logging.Abstractions`              | 8.0.2    | Library       |
| `Microsoft.Extensions.DependencyInjection`               | 8.0.1    | Tests         |
| `Microsoft.Extensions.Logging`                           | 8.0.1    | Tests         |
| `Microsoft.Extensions.Hosting`                           | 8.0.1    | Samples       |
| `Microsoft.NET.Test.Sdk`                                 | 17.12.0  | Tests         |
| `xunit`                                                  | 2.9.3    | Tests         |
| `xunit.runner.visualstudio`                              | 2.8.2    | Tests         |
| `NSubstitute`                                            | 5.3.0    | Tests         |

## 4. Configuration

### 4.1 PubSubOptions

**File:** `src/GCP.PubSub/PubSubOptions.cs`

A POCO validated via `System.ComponentModel.DataAnnotations`:

| Property         | Type     | Validation                             | Default          |
|------------------|----------|----------------------------------------|------------------|
| `ProjectId`      | `string` | `[Required(AllowEmptyStrings = false)]`| `string.Empty`   |
| `TopicId`        | `string` | `[Required(AllowEmptyStrings = false)]`| `string.Empty`   |
| `SubscriptionId` | `string` | `[Required(AllowEmptyStrings = false)]`| `string.Empty`   |

Validation is enforced at startup via `ValidateDataAnnotations()` and `ValidateOnStart()`. Empty or whitespace-only strings are rejected, throwing `OptionsValidationException` when the options are first resolved.

### 4.2 Registration Methods

**File:** `src/GCP.PubSub/ServiceCollectionExtensions.cs`

Two `IServiceCollection` extension methods:

#### Overload 1 -- Programmatic configuration

```csharp
public static IServiceCollection AddGcpPubSub(
    this IServiceCollection services,
    Action<PubSubOptions> configure)
```

Configures options via a delegate. Calls `ValidateDataAnnotations()` and `ValidateOnStart()`.

#### Overload 2 -- Configuration binding

```csharp
public static IServiceCollection AddGcpPubSub(
    this IServiceCollection services,
    string configSectionPath = "PubSub")
```

Binds options from `IConfiguration` using `BindConfiguration(configSectionPath)`. Default section path is `"PubSub"`.

**appsettings.json example:**

```json
{
  "PubSub": {
    "ProjectId": "your-gcp-project-id",
    "TopicId": "your-topic-id",
    "SubscriptionId": "your-subscription-id"
  }
}
```

#### Shared registrations

Both overloads call `RegisterCoreServices()`, which registers:

| Service                       | Implementation      | Lifetime  | Method            |
|-------------------------------|---------------------|-----------|-------------------|
| `TimeProvider`                | `TimeProvider.System`| Singleton | `TryAddSingleton` |
| `IPubSubPublisher`            | `PubSubPublisher`   | Singleton | `TryAddSingleton` |
| `IPubSubSubscriber`           | `PubSubSubscriber`  | Singleton | `TryAddSingleton` |
| `IPubSubAdmin`                | `PubSubAdmin`       | Singleton | `TryAddSingleton` |

`TryAddSingleton` is used so consumers can substitute their own implementations before calling `AddGcpPubSub`.

#### Trimming suppression

Both overloads are annotated with `[UnconditionalSuppressMessage("Trimming", "IL2026")]` because `ValidateDataAnnotations()` and `BindConfiguration()` use reflection internally. The justification is that `PubSubOptions` contains only simple string properties that the linker preserves.

## 5. Public API

### 5.1 IPubSubPublisher

**File:** `src/GCP.PubSub/IPubSubPublisher.cs`

```csharp
public interface IPubSubPublisher : IAsyncDisposable
{
    Task<string> PublishAsync(
        string message,
        CancellationToken cancellationToken = default);

    Task<string> PublishAsync(
        string message,
        IDictionary<string, string>? attributes,
        CancellationToken cancellationToken = default);

    Task<int> PublishBatchAsync(
        IEnumerable<string> messages,
        CancellationToken cancellationToken = default);
}
```

| Method             | Returns   | Description                                                    |
|--------------------|-----------|----------------------------------------------------------------|
| `PublishAsync`     | `string`  | Publishes a single message. Returns the server-assigned message ID. Optionally attaches key-value attributes. |
| `PublishBatchAsync`| `int`     | Publishes multiple messages concurrently via `Task.WhenAll`. Returns the count of successfully published messages. |

### 5.2 IPubSubSubscriber

**File:** `src/GCP.PubSub/IPubSubSubscriber.cs`

```csharp
public interface IPubSubSubscriber : IAsyncDisposable
{
    Task<int> PullMessagesAsync(
        Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
        CancellationToken cancellationToken = default);

    Task<int> PullMessagesAsync(
        Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
        TimeSpan listenDuration,
        CancellationToken cancellationToken = default);
}
```

| Method             | Returns | Description                                                          |
|--------------------|---------|----------------------------------------------------------------------|
| `PullMessagesAsync`| `int`   | Starts a streaming pull subscriber for the given duration. Invokes `handler` for each received message. The handler receives the UTF-8 message text, the attributes dictionary, and a `CancellationToken`. Returning `true` from the handler ACKs the message; `false` NACKs it. Returns the total count of messages received. |

**Default listen duration:** 5 seconds (when the `TimeSpan` overload is not used).

**Handler signature:**

```csharp
Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler
```

- Parameter 1: `string` -- the message body (UTF-8 decoded)
- Parameter 2: `IDictionary<string, string>` -- message attributes
- Parameter 3: `CancellationToken` -- cancellation signal
- Return: `Task<bool>` -- `true` = ACK, `false` = NACK

### 5.3 IPubSubAdmin

**File:** `src/GCP.PubSub/IPubSubAdmin.cs`

```csharp
public interface IPubSubAdmin
{
    Task<IReadOnlyList<string>> ListTopicsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> ListSubscriptionsAsync(
        CancellationToken cancellationToken = default);
}
```

| Method                  | Returns                 | Description                                                         |
|-------------------------|-------------------------|---------------------------------------------------------------------|
| `ListTopicsAsync`       | `IReadOnlyList<string>` | Lists all topic IDs in the configured GCP project.                  |
| `ListSubscriptionsAsync`| `IReadOnlyList<string>` | Lists all subscription IDs in the configured GCP project.           |

Both methods return simple string IDs (e.g., `"my-topic"`), not full resource names (e.g., `"projects/my-project/topics/my-topic"`). No `IAsyncDisposable` is needed — the underlying `PublisherServiceApiClient` and `SubscriberServiceApiClient` are lightweight admin API wrappers, not long-lived gRPC streaming clients.

## 6. Implementation Details

### 6.1 PubSubPublisher

**File:** `src/GCP.PubSub/PubSubPublisher.cs`

**Class:** `sealed class PubSubPublisher : IPubSubPublisher`

#### Client initialization

The `PublisherClient` is lazily created on first use via a thread-safe double-check locking pattern using `SemaphoreSlim`:

1. Fast path: if `_publisher` is not null, return immediately.
2. Slow path: acquire `_initLock`, check again, then call `PublisherClient.CreateAsync(topicName)`.

The `TopicName` is derived from `PubSubOptions.ProjectId` and `PubSubOptions.TopicId`.

#### Single publish

Constructs a `PubsubMessage` with `ByteString.CopyFromUtf8(message)` data and optional attributes. Calls `PublisherClient.PublishAsync(pubsubMessage)`. Logs the message ID on success or the exception on failure (then re-throws).

#### Batch publish

Materializes the input `IEnumerable<string>` to a `List<string>`. Returns 0 for empty input. Creates a `Task` per message via `Task.WhenAll`. Uses `Interlocked.Increment` for thread-safe counting. Individual message failures are logged but do not abort the batch.

#### Disposal

`DisposeAsync()` calls `PublisherClient.ShutdownAsync(TimeSpan.FromSeconds(15))` to flush pending messages, then disposes the `SemaphoreSlim`. Guarded by a `_disposed` flag.

### 6.2 PubSubSubscriber

**File:** `src/GCP.PubSub/PubSubSubscriber.cs`

**Class:** `sealed class PubSubSubscriber : IPubSubSubscriber`

#### Per-call lifecycle

Each call to `PullMessagesAsync` creates a fresh `SubscriberClient`. This is intentional -- the Google Cloud `SubscriberClient` is designed for a start-once/stop-once lifecycle:

1. Create `SubscriberClient.CreateAsync(subscriptionName)`.
2. Start streaming with `subscriber.StartAsync(messageHandler)`.
3. Wait for the listen duration to elapse.
4. Stop with `subscriber.StopAsync(CancellationToken.None)`.
5. Await the `startTask` to ensure clean shutdown.

#### TimeProvider-aware delay

The listen duration is implemented using the .NET 8 `TimeProvider`-aware `CancellationTokenSource` constructor:

```csharp
using var delayCts = new CancellationTokenSource(listenDuration, _timeProvider);
using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
    cancellationToken, delayCts.Token);
await Task.Delay(Timeout.InfiniteTimeSpan, linkedCts.Token);
```

This allows unit tests to inject a fake `TimeProvider` to control time without real delays. The `OperationCanceledException` is caught silently since it indicates either timer expiry (normal) or external cancellation (also expected).

#### Message handling

The `StartAsync` callback converts `PubsubMessage.Data` to UTF-8 text, casts `Attributes` to `IDictionary<string, string>`, logs the message, increments an `Interlocked` counter, and delegates to the user's handler. The handler's `bool` return is mapped to `SubscriberClient.Reply.Ack` or `Reply.Nack`.

#### Disposal

`DisposeAsync()` stops any active subscriber via `StopAsync(CancellationToken.None)`.

### 6.3 PubSubAdmin

**File:** `src/GCP.PubSub/PubSubAdmin.cs`

**Class:** `sealed class PubSubAdmin : IPubSubAdmin`

#### Per-call client creation

Each call to `ListTopicsAsync` or `ListSubscriptionsAsync` creates a fresh `PublisherServiceApiClient` or `SubscriberServiceApiClient` respectively. These are lightweight admin API clients (not streaming gRPC channels like `PublisherClient`/`SubscriberClient`), so per-call creation is appropriate.

#### Topic listing

1. Constructs a `ProjectName` from `PubSubOptions.ProjectId` (via `Google.Api.Gax.ResourceNames`).
2. Calls `PublisherServiceApiClient.CreateAsync()` to obtain the admin client.
3. Calls `client.ListTopicsAsync(projectName)` which returns a `PagedAsyncEnumerable`.
4. Iterates with `await foreach` using `ConfigureAwait(false)`.
5. Extracts `topic.TopicName.TopicId` from each `Topic` resource.

#### Subscription listing

Same pattern using `SubscriberServiceApiClient.CreateAsync()` and `client.ListSubscriptionsAsync(projectName)`, extracting `sub.SubscriptionName.SubscriptionId`.

### 6.4 Structured Logging

**File:** `src/GCP.PubSub/Log.cs`

Uses the `[LoggerMessage]` source generator for zero-allocation, high-performance structured logging:

| Method              | Level       | Message Template                                      |
|---------------------|-------------|-------------------------------------------------------|
| `PublishedMessage`  | Information | `"Published message {MessageId}"`                     |
| `PublishFailed`     | Error       | `"Failed to publish message"`                         |
| `ReceivedMessage`   | Information | `"Received message {MessageId}: {Text}"`              |
| `SubscriberStarted` | Information | `"Subscriber started, listening for {Duration}"`      |
| `SubscriberStopped` | Information | `"Subscriber stopped, received {Count} messages"`     |
| `ListedTopics`      | Information | `"Listed {Count} topics"`                             |
| `ListedSubscriptions`| Information | `"Listed {Count} subscriptions"`                     |

All methods are defined as `internal static partial` extension methods on `ILogger`.

### 6.5 Async best practices

All library code follows these conventions:

- **`ConfigureAwait(false)`** on every `await` to avoid capturing the synchronization context.
- **`CancellationToken`** propagation on all public async methods.
- **`ObjectDisposedException.ThrowIf(_disposed, this)`** at the start of public methods to guard against use-after-dispose.
- **`IAsyncDisposable`** on both interfaces for proper gRPC channel cleanup.

## 7. Test Suite

**Project:** `tests/GCP.PubSub.Tests/`

**Frameworks:** xunit 2.9.3 with NSubstitute 5.3.0

**Target Frameworks:** `net8.0`, `net9.0`, `net10.0` (tests run on all three)

### 7.1 PubSubPublisherTests

| Test                                            | Type   | Asserts                                               |
|-------------------------------------------------|--------|-------------------------------------------------------|
| `OptionsValidation_RejectsEmptyProjectIdOrTopicId` | Theory | Empty/whitespace `ProjectId` or `TopicId` throws `OptionsValidationException` |
| `OptionsValidation_AcceptsValidOptions`         | Fact   | Valid options resolve without exception; values match  |
| `AddGcpPubSub_RegistersPublisherAsSingleton`    | Fact   | Two `GetRequiredService<IPubSubPublisher>()` calls return same instance |

### 7.2 PubSubSubscriberTests

| Test                                                  | Type   | Asserts                                                |
|-------------------------------------------------------|--------|--------------------------------------------------------|
| `OptionsValidation_RejectsEmptyProjectIdOrSubscriptionId` | Theory | Empty/whitespace `ProjectId` or `SubscriptionId` throws `OptionsValidationException` |
| `AddGcpPubSub_RegistersSubscriberAsSingleton`         | Fact   | Two `GetRequiredService<IPubSubSubscriber>()` calls return same instance |
| `AddGcpPubSub_RegistersTimeProvider`                  | Fact   | `GetRequiredService<TimeProvider>()` returns `TimeProvider.System` |

### 7.3 PubSubAdminTests

| Test                                            | Type   | Asserts                                               |
|-------------------------------------------------|--------|-------------------------------------------------------|
| `AddGcpPubSub_RegistersAdminAsSingleton`        | Fact   | Two `GetRequiredService<IPubSubAdmin>()` calls return same instance |

### 7.4 Test pattern

Tests exercise the real DI pipeline by constructing a `ServiceCollection`, calling `AddLogging()` and `AddGcpPubSub(opts => ...)`, building the `ServiceProvider`, and resolving services. This validates the full registration chain including options validation.

## 8. Sample Applications

### 8.1 Console Application

**Project:** `samples/GCP.PubSub.Console/` (net8.0)

Uses `Host.CreateDefaultBuilder(args)` with `AddGcpPubSub()` (configuration binding from `appsettings.json`). Demonstrates:

- Listing available topics and subscriptions via `IPubSubAdmin` at startup
- Publishing a single message with `PublishAsync`
- Publishing a batch with `PublishBatchAsync`
- Subscribing for 10 seconds with `PullMessagesAsync`

### 8.2 WinForms Application

**Project:** `samples/GCP.PubSub.WinForms/` (net8.0-windows)

Uses Generic Host (`Host.CreateDefaultBuilder`) integrated with `Application.Run`. The `MainForm` receives `IPubSubPublisher`, `IPubSubSubscriber`, and `IPubSubAdmin` via constructor injection. Demonstrates:

- List Topics button: queries GCP and displays all topic IDs in the project
- List Subscriptions button: queries GCP and displays all subscription IDs in the project
- Publish button: sends a message and displays the returned ID
- Subscribe button: listens for 10 seconds, marshals received messages to the UI thread via `Invoke()`

### 8.3 Blazor Server Application

**Project:** `samples/GCP.PubSub.BlazorServer/` (net8.0)

Uses `WebApplication.CreateBuilder` with `AddGcpPubSub()` and Blazor Server components (`@rendermode InteractiveServer`). The `PubSub.razor` page injects `IPubSubPublisher`, `IPubSubSubscriber`, and `IPubSubAdmin` directly. Demonstrates:

- List Topics and List Subscriptions buttons for project discovery
- Text input and publish button
- Configurable listen duration with subscribe button
- Real-time message list updated via `InvokeAsync` + `StateHasChanged`

All three samples bind configuration from the `"PubSub"` section in `appsettings.json`.

## 9. Design Decisions

| #  | Decision                                     | Rationale                                                                                   |
|----|----------------------------------------------|---------------------------------------------------------------------------------------------|
| 1  | Interfaces extend `IAsyncDisposable`         | `PublisherClient` needs `ShutdownAsync` to flush; `SubscriberClient` needs `StopAsync`       |
| 2  | Singleton lifetime for publisher/subscriber  | gRPC channels are expensive to create; reuse across the application                          |
| 3  | `TryAddSingleton` for core services          | Allows consumers to register custom implementations before calling `AddGcpPubSub`            |
| 4  | `SemaphoreSlim` for lazy init (not `Lazy<Task<T>>`) | `Lazy<Task<T>>` caches faulted tasks permanently; semaphore allows retry on transient errors |
| 5  | Per-call `SubscriberClient` lifecycle        | Google's `SubscriberClient` is single-use (start once, stop once); reuse is not supported    |
| 6  | `TimeProvider` for testable delays           | .NET 8 BCL abstraction; enables unit tests to control time without real delays               |
| 7  | `[LoggerMessage]` source generator           | Zero-allocation structured logging at compile time; avoids boxing and string interpolation    |
| 8  | `ConfigureAwait(false)` everywhere           | Library code must not capture synchronization context to avoid deadlocks                      |
| 9  | `ValidateDataAnnotations` + `ValidateOnStart`| Fail-fast on startup with clear errors rather than runtime `NullReferenceException`s          |
| 10 | NSubstitute over Moq                         | Moq's SponsorLink controversy; NSubstitute has cleaner syntax and no telemetry concerns       |
| 11 | Central Package Management                   | Single source of truth for all NuGet versions across the solution                             |
| 12 | Per-project `TargetFramework(s)`             | Library multi-targets (net8.0/9.0/10.0); samples target net8.0 only                          |
| 13 | `IsTrimmable=true` in `Directory.Build.props`| Marks all assemblies as trim-compatible; IL2026 suppressions added where reflection is used    |
| 14 | Handler returns `bool` for ACK/NACK          | Simple, intuitive API; `true` = acknowledge, `false` = negative acknowledge                   |
| 15 | Batch publish swallows individual failures   | Partial success is more useful than all-or-nothing for batch operations                       |
| 16 | `IPubSubAdmin` has no `IAsyncDisposable`     | Admin API clients (`PublisherServiceApiClient`, `SubscriberServiceApiClient`) are lightweight wrappers, not long-lived gRPC streaming clients; per-call creation is appropriate |
| 17 | Admin methods return `IReadOnlyList<string>` of IDs | Callers typically need just the topic/subscription ID, not full GCP resource names; simplifies consumption |

## 10. Authentication

The library delegates authentication entirely to the underlying `Google.Cloud.PubSub.V1` SDK. No authentication configuration is exposed in `PubSubOptions`. The SDK uses Application Default Credentials (ADC), which resolves in this order:

1. `GOOGLE_APPLICATION_CREDENTIALS` environment variable (path to service account JSON)
2. gcloud CLI default credentials (`gcloud auth application-default login`)
3. GCE/GKE metadata server (when running on Google Cloud)

## 11. Error Handling

| Scenario                        | Behavior                                                                 |
|---------------------------------|--------------------------------------------------------------------------|
| Invalid options at startup      | `OptionsValidationException` thrown on first `IOptions<PubSubOptions>.Value` access |
| Publish failure (single)        | Exception logged at Error level, then re-thrown to the caller            |
| Publish failure (batch, single message) | Exception logged at Error level; other messages continue; count reflects successes only |
| Subscribe handler exception     | Propagated by the `SubscriberClient` infrastructure                      |
| Use after dispose               | `ObjectDisposedException` thrown immediately                             |
| Listen duration elapsed         | `OperationCanceledException` caught internally; subscriber stops gracefully |
| External cancellation           | `OperationCanceledException` caught internally; subscriber stops gracefully |
| List topics/subscriptions failure | Exception propagated to the caller (e.g., permission denied, network error) |

## 12. Threading Model

- **`PubSubPublisher`** is thread-safe. The `SemaphoreSlim`-guarded lazy initialization ensures exactly one `PublisherClient` is created. `PublisherClient.PublishAsync` is itself thread-safe.
- **`PubSubSubscriber`** stores a reference to `_activeSubscriber` for disposal purposes. Concurrent calls to `PullMessagesAsync` are not explicitly synchronized, but each call creates its own `SubscriberClient` instance. The `_activeSubscriber` field tracks only the most recent one for `DisposeAsync`.
- **`PubSubAdmin`** is thread-safe. Each method call creates its own admin API client; no shared mutable state.
- **Message counting** uses `Interlocked.Increment` for lock-free thread safety.

## 13. Constraints and Limitations

1. **Messages are UTF-8 strings only.** Binary payloads are not supported through the current API surface. Consumers needing binary should encode to Base64 before publishing.
2. **No dead-letter queue configuration.** Dead-letter topics must be configured at the GCP infrastructure level.
3. **No message ordering.** The library does not set ordering keys on published messages.
4. **No custom `PublisherClient`/`SubscriberClient` settings.** Connection pool size, flow control, and retry settings use SDK defaults.
5. **Subscriber is not reentrant for a single instance.** While the singleton can handle sequential calls to `PullMessagesAsync`, concurrent calls will overwrite the `_activeSubscriber` tracking field.
6. **Publisher shutdown timeout is fixed at 15 seconds.** Not configurable.
