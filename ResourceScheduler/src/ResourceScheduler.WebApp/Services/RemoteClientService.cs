using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;

namespace ResourceScheduler.WebApp.Services;

/// <summary>
/// Phase 2 implementation of <see cref="IClientService"/> against the
/// Rust HTTP API. Not yet wired into DI; see Program.cs. The contract
/// surface mirrors the in-memory implementation; only transport differs.
/// </summary>
// TODO Phase 2: register this in Program.cs in place of InMemoryClientService once the Rust API ships. Configure the HttpClient base address from configuration (appsettings.json or build-time env).
internal sealed class RemoteClientService : IClientService
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _json = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public RemoteClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // ---- Buildings ----

    public async Task<IReadOnlyList<BuildingDto>> ListBuildingsAsync(CancellationToken ct = default)
    {
        var result = await GetJsonAsync<List<BuildingDto>>("/api/buildings", ct).ConfigureAwait(false);
        return result;
    }

    public Task<BuildingDto?> GetBuildingAsync(Guid id, CancellationToken ct = default)
    {
        return GetJsonOrNullAsync<BuildingDto>($"/api/buildings/{id}", ct);
    }

    public Task<BuildingDto> CreateBuildingAsync(BuildingCreate input, CancellationToken ct = default)
    {
        return PostJsonAsync<BuildingDto, BuildingCreate>("/api/buildings", input, ct);
    }

    public Task<BuildingDto> UpdateBuildingAsync(Guid id, BuildingUpdate input, int version, CancellationToken ct = default)
    {
        return PutJsonAsync<BuildingDto, BuildingUpdate>($"/api/buildings/{id}", input, version, ct);
    }

    public Task DeleteBuildingAsync(Guid id, int version, CancellationToken ct = default)
    {
        return DeleteAsync($"/api/buildings/{id}", version, ct);
    }

    // ---- Devices ----

    public async Task<IReadOnlyList<DeviceDto>> ListDevicesAsync(CancellationToken ct = default)
    {
        var result = await GetJsonAsync<List<DeviceDto>>("/api/devices", ct).ConfigureAwait(false);
        return result;
    }

    public Task<DeviceDto?> GetDeviceAsync(Guid id, CancellationToken ct = default)
    {
        return GetJsonOrNullAsync<DeviceDto>($"/api/devices/{id}", ct);
    }

    public Task<DeviceDto> CreateDeviceAsync(DeviceCreate input, CancellationToken ct = default)
    {
        return PostJsonAsync<DeviceDto, DeviceCreate>("/api/devices", input, ct);
    }

    public Task<DeviceDto> UpdateDeviceAsync(Guid id, DeviceUpdate input, int version, CancellationToken ct = default)
    {
        return PutJsonAsync<DeviceDto, DeviceUpdate>($"/api/devices/{id}", input, version, ct);
    }

    public Task DeleteDeviceAsync(Guid id, int version, CancellationToken ct = default)
    {
        return DeleteAsync($"/api/devices/{id}", version, ct);
    }

    // ---- Device-Groups ----

    public async Task<IReadOnlyList<DeviceGroupDto>> ListDeviceGroupsAsync(CancellationToken ct = default)
    {
        var result = await GetJsonAsync<List<DeviceGroupDto>>("/api/device-groups", ct).ConfigureAwait(false);
        return result;
    }

    public Task<DeviceGroupDto?> GetDeviceGroupAsync(Guid id, CancellationToken ct = default)
    {
        return GetJsonOrNullAsync<DeviceGroupDto>($"/api/device-groups/{id}", ct);
    }

    public Task<DeviceGroupDto> CreateDeviceGroupAsync(DeviceGroupCreate input, CancellationToken ct = default)
    {
        return PostJsonAsync<DeviceGroupDto, DeviceGroupCreate>("/api/device-groups", input, ct);
    }

    public Task<DeviceGroupDto> UpdateDeviceGroupAsync(Guid id, DeviceGroupUpdate input, int version, CancellationToken ct = default)
    {
        return PutJsonAsync<DeviceGroupDto, DeviceGroupUpdate>($"/api/device-groups/{id}", input, version, ct);
    }

    public Task<DeviceGroupDto> ActivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        return PostEmptyAsync<DeviceGroupDto>($"/api/device-groups/{id}/activate", version, ct);
    }

    public Task<DeviceGroupDto> DeactivateDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        return PostEmptyAsync<DeviceGroupDto>($"/api/device-groups/{id}/deactivate", version, ct);
    }

    public Task DeleteDeviceGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        return DeleteAsync($"/api/device-groups/{id}", version, ct);
    }

    // ---- People ----

    public async Task<IReadOnlyList<PersonDto>> ListPeopleAsync(CancellationToken ct = default)
    {
        var result = await GetJsonAsync<List<PersonDto>>("/api/people", ct).ConfigureAwait(false);
        return result;
    }

    public Task<PersonDto> CreatePersonAsync(PersonCreate input, CancellationToken ct = default)
    {
        return PostJsonAsync<PersonDto, PersonCreate>("/api/people", input, ct);
    }

    public Task<PersonDto> UpdatePersonAsync(Guid id, PersonUpdate input, CancellationToken ct = default)
    {
        return PutJsonAsync<PersonDto, PersonUpdate>($"/api/people/{id}", input, version: null, ct);
    }

    public Task DeletePersonAsync(Guid id, CancellationToken ct = default)
    {
        return DeleteAsync($"/api/people/{id}", version: null, ct);
    }

    // ---- Test-Groups ----

    public async Task<IReadOnlyList<TestGroupDto>> ListTestGroupsAsync(CancellationToken ct = default)
    {
        var result = await GetJsonAsync<List<TestGroupDto>>("/api/test-groups", ct).ConfigureAwait(false);
        return result;
    }

    public Task<TestGroupDto> CreateTestGroupAsync(TestGroupCreate input, CancellationToken ct = default)
    {
        return PostJsonAsync<TestGroupDto, TestGroupCreate>("/api/test-groups", input, ct);
    }

    public Task<TestGroupDto> UpdateTestGroupAsync(Guid id, TestGroupUpdate input, int version, CancellationToken ct = default)
    {
        return PutJsonAsync<TestGroupDto, TestGroupUpdate>($"/api/test-groups/{id}", input, version, ct);
    }

    public Task DeleteTestGroupAsync(Guid id, int version, CancellationToken ct = default)
    {
        return DeleteAsync($"/api/test-groups/{id}", version, ct);
    }

    // ---- Reservations ----

    public async Task<IReadOnlyList<ReservationDto>> ListReservationsAsync(ReservationFilter? filter = null, CancellationToken ct = default)
    {
        var url = "/api/reservations" + BuildReservationQuery(filter);
        var result = await GetJsonAsync<List<ReservationDto>>(url, ct).ConfigureAwait(false);
        return result;
    }

    public Task<ReservationDto> CreateReservationAsync(ReservationCreate input, CancellationToken ct = default)
    {
        return PostJsonAsync<ReservationDto, ReservationCreate>("/api/reservations", input, ct);
    }

    public Task<ReservationDto> ConfirmReservationAsync(Guid id, int version, CancellationToken ct = default)
    {
        return PostEmptyAsync<ReservationDto>($"/api/reservations/{id}/confirm", version, ct);
    }

    public Task<ReservationDto> CancelReservationAsync(Guid id, int version, CancellationToken ct = default)
    {
        return PostEmptyAsync<ReservationDto>($"/api/reservations/{id}/cancel", version, ct);
    }

    // ---- Helpers ----

    private static string BuildReservationQuery(ReservationFilter? filter)
    {
        if (filter is null)
        {
            return string.Empty;
        }

        var parts = new List<string>();
        if (filter.DeviceGroupId is { } dgid)
        {
            parts.Add($"deviceGroupId={Uri.EscapeDataString(dgid.ToString())}");
        }
        if (filter.TestGroupId is { } tgid)
        {
            parts.Add($"testGroupId={Uri.EscapeDataString(tgid.ToString())}");
        }
        if (filter.FromUtc is { } from)
        {
            parts.Add($"fromUtc={Uri.EscapeDataString(from.ToUniversalTime().ToString("o"))}");
        }
        if (filter.ToUtc is { } to)
        {
            parts.Add($"toUtc={Uri.EscapeDataString(to.ToUniversalTime().ToString("o"))}");
        }
        if (filter.StatusIn is { Count: > 0 } statuses)
        {
            foreach (var status in statuses)
            {
                parts.Add($"statusIn={Uri.EscapeDataString(status.ToString())}");
            }
        }

        return parts.Count == 0 ? string.Empty : "?" + string.Join("&", parts);
    }

    private async Task<T> GetJsonAsync<T>(string url, CancellationToken ct)
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<T>(url, _json, ct).ConfigureAwait(false);
            if (result is null)
            {
                throw new NotFoundException($"Empty response from {url}.");
            }
            return result;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            throw new NotFoundException($"Resource not found at {url}.");
        }
    }

    private async Task<T?> GetJsonOrNullAsync<T>(string url, CancellationToken ct) where T : class
    {
        try
        {
            using var response = await _httpClient.GetAsync(url, ct).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_json, ct).ConfigureAwait(false);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    private async Task<TResult> PostJsonAsync<TResult, TBody>(string url, TBody body, CancellationToken ct)
    {
        using var response = await _httpClient.PostAsJsonAsync(url, body, _json, ct).ConfigureAwait(false);
        await ThrowForKnownStatusAsync(response, url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TResult>(_json, ct).ConfigureAwait(false);
        if (result is null)
        {
            throw new NotFoundException($"Empty response from {url}.");
        }
        return result;
    }

    private async Task<TResult> PostEmptyAsync<TResult>(string url, int version, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(string.Empty)
        };
        request.Headers.TryAddWithoutValidation("If-Match", version.ToString());
        request.Content!.Headers.ContentType = null;

        using var response = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
        await ThrowForKnownStatusAsync(response, url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TResult>(_json, ct).ConfigureAwait(false);
        if (result is null)
        {
            throw new NotFoundException($"Empty response from {url}.");
        }
        return result;
    }

    private async Task<TResult> PutJsonAsync<TResult, TBody>(string url, TBody body, int? version, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(body, mediaType: null, _json)
        };
        if (version is { } v)
        {
            request.Headers.TryAddWithoutValidation("If-Match", v.ToString());
        }

        using var response = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
        await ThrowForKnownStatusAsync(response, url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TResult>(_json, ct).ConfigureAwait(false);
        if (result is null)
        {
            throw new NotFoundException($"Empty response from {url}.");
        }
        return result;
    }

    private async Task DeleteAsync(string url, int? version, CancellationToken ct)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        if (version is { } v)
        {
            request.Headers.TryAddWithoutValidation("If-Match", v.ToString());
        }

        using var response = await _httpClient.SendAsync(request, ct).ConfigureAwait(false);
        await ThrowForKnownStatusAsync(response, url, ct).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    private async Task ThrowForKnownStatusAsync(HttpResponseMessage response, string url, CancellationToken ct)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        switch (response.StatusCode)
        {
            case HttpStatusCode.NotFound:
                throw new NotFoundException($"Resource not found at {url}.");
            case HttpStatusCode.Conflict:
                throw new ConflictException($"Version conflict at {url}.");
            case HttpStatusCode.BadRequest:
                ApiError? error = null;
                try
                {
                    error = await response.Content.ReadFromJsonAsync<ApiError>(_json, ct).ConfigureAwait(false);
                }
                catch
                {
                    // Body was not a structured ApiError; fall through to generic.
                }
                if (error is not null)
                {
                    throw new ValidationException(error.RuleId, error.Message);
                }
                break;
        }
    }

    private sealed record ApiError(string RuleId, string Message);
}
