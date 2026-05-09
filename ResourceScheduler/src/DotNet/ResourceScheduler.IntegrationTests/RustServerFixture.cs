using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace ResourceScheduler.IntegrationTests;

/// <summary>
/// Boots the Rust resource-scheduler-api binary on a random port with a
/// dedicated SQLite file, waits for /healthz to respond, and tears
/// everything down on dispose. One instance per test class via
/// IClassFixture; xUnit may run several classes in parallel, each with
/// its own port, DB file, and process, so tests stay isolated.
/// </summary>
public sealed class RustServerFixture : IAsyncLifetime
{
    public string BaseUrl { get; private set; } = string.Empty;

    private Process? _process;
    private string? _dbPath;

    public async Task InitializeAsync()
    {
        var manifest = LocateCargoToml();
        var crateRoot = Path.GetDirectoryName(manifest)!;
        var binaryName = OperatingSystem.IsWindows()
            ? "resource-scheduler-api.exe"
            : "resource-scheduler-api";
        var binaryPath = Path.Combine(crateRoot, "target", "release", binaryName);

        // Build (no-op if up to date). Stream output so a build failure
        // surfaces the actual cargo error in the test log.
        var build = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "cargo",
                Arguments = $"build --release --manifest-path \"{manifest}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        build.Start();
        var buildOut = await build.StandardOutput.ReadToEndAsync();
        var buildErr = await build.StandardError.ReadToEndAsync();
        await build.WaitForExitAsync();
        if (build.ExitCode != 0)
        {
            throw new InvalidOperationException(
                $"cargo build failed (exit {build.ExitCode}):\nstdout:\n{buildOut}\nstderr:\n{buildErr}");
        }
        if (!File.Exists(binaryPath))
        {
            throw new FileNotFoundException(
                $"Rust binary not found after build: {binaryPath}");
        }

        var port = GetFreePort();
        _dbPath = Path.Combine(
            Path.GetTempPath(),
            $"rs-int-{Guid.NewGuid():N}.db");

        var psi = new ProcessStartInfo
        {
            FileName = binaryPath,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };
        psi.Environment["BIND_ADDR"] = $"127.0.0.1:{port}";
        psi.Environment["DATABASE_URL"] = $"sqlite://{_dbPath.Replace('\\', '/')}";
        // Tone down tracing so test output stays readable.
        psi.Environment["RUST_LOG"] = "warn";

        _process = Process.Start(psi)
            ?? throw new InvalidOperationException("Failed to start resource-scheduler-api");

        BaseUrl = $"http://127.0.0.1:{port}";
        await WaitForHealthzAsync(BaseUrl, TimeSpan.FromSeconds(20));
    }

    public Task DisposeAsync()
    {
        try
        {
            if (_process is { HasExited: false })
            {
                _process.Kill(entireProcessTree: true);
                _process.WaitForExit(2000);
            }
        }
        catch
        {
            // best effort
        }
        if (_dbPath is not null && File.Exists(_dbPath))
        {
            try { File.Delete(_dbPath); } catch { /* best effort */ }
        }
        return Task.CompletedTask;
    }

    private static int GetFreePort()
    {
        var listener = new TcpListener(IPAddress.Loopback, 0);
        listener.Start();
        var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }

    private static async Task WaitForHealthzAsync(string baseUrl, TimeSpan timeout)
    {
        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
        var deadline = DateTime.UtcNow + timeout;
        Exception? last = null;
        while (DateTime.UtcNow < deadline)
        {
            try
            {
                var resp = await client.GetAsync($"{baseUrl}/healthz");
                if (resp.IsSuccessStatusCode) return;
            }
            catch (Exception ex)
            {
                last = ex;
            }
            await Task.Delay(100);
        }
        throw new TimeoutException(
            $"Rust server did not respond on {baseUrl}/healthz within {timeout}",
            last);
    }

    private static string LocateCargoToml()
    {
        // Walk up from the test assembly's working directory looking
        // for the Rust crate. Layout: ResourceScheduler/src/Rust/Cargo.toml,
        // and the test bin lives at
        // ResourceScheduler/src/DotNet/ResourceScheduler.IntegrationTests/bin/<conf>/<tfm>/.
        var dir = AppContext.BaseDirectory;
        while (dir is not null)
        {
            var candidate = Path.Combine(dir, "src", "Rust", "Cargo.toml");
            if (File.Exists(candidate)) return Path.GetFullPath(candidate);

            var sibling = Path.Combine(dir, "..", "Rust", "Cargo.toml");
            if (File.Exists(sibling)) return Path.GetFullPath(sibling);

            dir = Directory.GetParent(dir)?.FullName;
        }
        throw new FileNotFoundException(
            "Could not locate Rust Cargo.toml relative to test assembly");
    }
}
