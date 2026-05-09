// Tests for the Phase 2 HTTP-backed implementation of IClientService.
//
// We don't have a real Rust API yet, so the strategy is to swap in a
// recording HttpMessageHandler that captures every request the service
// makes and returns canned responses. The tests then assert:
//
//   * the URL is correct (path, plural form, sub-resource verbs, query)
//   * the HTTP method is correct
//   * If-Match carries the version when the operation requires one
//   * 404 -> NotFoundException, 409 -> ConflictException,
//     400 with structured body -> ValidationException(RuleId, Message)
//   * JSON bodies serialise as camelCase with string-typed enums

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using Xunit;

namespace ResourceScheduler.Tests;

public sealed class RemoteClientServiceTests
{
    /// <summary>
    /// Records every HttpRequestMessage and replies with a queued
    /// response. Tests configure the queue, exercise the service, then
    /// inspect the captured requests.
    /// </summary>
    private sealed class RecordingHandler : HttpMessageHandler
    {
        public List<HttpRequestMessage> Requests { get; } = new();
        public List<string> RequestBodies { get; } = new();
        public Queue<HttpResponseMessage> Responses { get; } = new();

        public void EnqueueJson<T>(HttpStatusCode status, T body)
        {
            var json = JsonSerializer.Serialize(body, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
            });
            Responses.Enqueue(new HttpResponseMessage(status)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }

        public void EnqueueRaw(HttpStatusCode status, string? body = null, string? contentType = "application/json")
        {
            var msg = new HttpResponseMessage(status);
            if (body is not null)
            {
                msg.Content = new StringContent(body, Encoding.UTF8, contentType ?? "application/json");
            }
            Responses.Enqueue(msg);
        }

        public void EnqueueEmpty(HttpStatusCode status) =>
            Responses.Enqueue(new HttpResponseMessage(status));

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(request);
            string body = string.Empty;
            if (request.Content is not null)
            {
                body = await request.Content.ReadAsStringAsync(cancellationToken);
            }
            RequestBodies.Add(body);

            if (Responses.Count == 0)
            {
                throw new InvalidOperationException("No queued response for " + request.RequestUri);
            }
            return Responses.Dequeue();
        }
    }

    private static (RemoteClientService svc, RecordingHandler handler) NewService()
    {
        var handler = new RecordingHandler();
        var http = new HttpClient(handler) { BaseAddress = new Uri("https://api.example/") };
        return (new RemoteClientService(http), handler);
    }

    // ============================================================
    // URL + method + If-Match header
    // ============================================================

    [Fact]
    public async Task ListBuildings_uses_GET_on_plural_resource()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new List<BuildingDto>());

        await svc.ListBuildingsAsync();

        Assert.Single(h.Requests);
        Assert.Equal(HttpMethod.Get, h.Requests[0].Method);
        Assert.Equal("/api/buildings", h.Requests[0].RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task GetBuilding_returns_null_on_404_without_throwing()
    {
        var (svc, h) = NewService();
        h.EnqueueEmpty(HttpStatusCode.NotFound);

        var result = await svc.GetBuildingAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateBuilding_sends_PUT_with_If_Match_version_header()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = "B",
            Address = "A",
            Version = 8,
        });

        await svc.UpdateBuildingAsync(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            new BuildingUpdate("New", "Addr"),
            version: 7);

        Assert.Equal(HttpMethod.Put, h.Requests[0].Method);
        Assert.Equal("/api/buildings/11111111-1111-1111-1111-111111111111",
            h.Requests[0].RequestUri!.AbsolutePath);
        Assert.True(h.Requests[0].Headers.TryGetValues("If-Match", out var values));
        Assert.Equal("7", values!.Single());
    }

    [Fact]
    public async Task DeleteBuilding_sends_DELETE_with_If_Match()
    {
        var (svc, h) = NewService();
        h.EnqueueEmpty(HttpStatusCode.NoContent);

        await svc.DeleteBuildingAsync(Guid.NewGuid(), version: 3);

        Assert.Equal(HttpMethod.Delete, h.Requests[0].Method);
        Assert.True(h.Requests[0].Headers.TryGetValues("If-Match", out var values));
        Assert.Equal("3", values!.Single());
    }

    [Fact]
    public async Task DeletePerson_omits_If_Match_because_PersonDto_has_no_Version()
    {
        var (svc, h) = NewService();
        h.EnqueueEmpty(HttpStatusCode.NoContent);

        await svc.DeletePersonAsync(Guid.NewGuid());

        Assert.Equal(HttpMethod.Delete, h.Requests[0].Method);
        Assert.False(h.Requests[0].Headers.Contains("If-Match"));
    }

    [Fact]
    public async Task UpdatePerson_omits_If_Match_because_PersonDto_has_no_Version()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new PersonDto
        {
            PersonId = Guid.NewGuid(),
            Name = "P",
        });

        await svc.UpdatePersonAsync(Guid.NewGuid(), new PersonUpdate("Q", null));

        Assert.Equal(HttpMethod.Put, h.Requests[0].Method);
        Assert.False(h.Requests[0].Headers.Contains("If-Match"));
    }

    [Fact]
    public async Task ActivateDeviceGroup_uses_POST_on_activate_subresource_with_If_Match()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "G",
            Status = DeviceGroupStatus.Active,
            Version = 4,
        });

        await svc.ActivateDeviceGroupAsync(
            Guid.Parse("22222222-2222-2222-2222-222222222222"), version: 3);

        Assert.Equal(HttpMethod.Post, h.Requests[0].Method);
        Assert.Equal("/api/device-groups/22222222-2222-2222-2222-222222222222/activate",
            h.Requests[0].RequestUri!.AbsolutePath);
        Assert.True(h.Requests[0].Headers.TryGetValues("If-Match", out var values));
        Assert.Equal("3", values!.Single());
    }

    [Fact]
    public async Task ConfirmReservation_uses_POST_on_confirm_subresource()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new ReservationDto
        {
            ReservationId = Guid.NewGuid(),
            DeviceGroupId = Guid.NewGuid(),
            TestGroupId = Guid.NewGuid(),
            StartUtc = DateTime.UtcNow,
            EndUtc = DateTime.UtcNow.AddHours(1),
            Status = ReservationStatus.Confirmed,
            Version = 2,
        });

        var id = Guid.Parse("33333333-3333-3333-3333-333333333333");
        await svc.ConfirmReservationAsync(id, version: 1);

        Assert.Equal(HttpMethod.Post, h.Requests[0].Method);
        Assert.Equal($"/api/reservations/{id}/confirm",
            h.Requests[0].RequestUri!.AbsolutePath);
    }

    [Fact]
    public async Task CancelReservation_uses_POST_on_cancel_subresource()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new ReservationDto
        {
            ReservationId = Guid.NewGuid(),
            DeviceGroupId = Guid.NewGuid(),
            TestGroupId = Guid.NewGuid(),
            StartUtc = DateTime.UtcNow,
            EndUtc = DateTime.UtcNow.AddHours(1),
            Status = ReservationStatus.Cancelled,
            Version = 2,
        });

        var id = Guid.Parse("44444444-4444-4444-4444-444444444444");
        await svc.CancelReservationAsync(id, version: 5);

        Assert.Equal(HttpMethod.Post, h.Requests[0].Method);
        Assert.Equal($"/api/reservations/{id}/cancel",
            h.Requests[0].RequestUri!.AbsolutePath);
    }

    // ============================================================
    // Plural resource paths
    // ============================================================

    [Theory]
    [InlineData("/api/buildings")]
    [InlineData("/api/devices")]
    [InlineData("/api/device-groups")]
    [InlineData("/api/people")]
    [InlineData("/api/test-groups")]
    [InlineData("/api/reservations")]
    public async Task List_endpoints_use_kebab_case_plural_paths(string expected)
    {
        var (svc, h) = NewService();
        // Queue an empty array for whichever list method the test uses.
        h.EnqueueRaw(HttpStatusCode.OK, "[]");

        switch (expected)
        {
            case "/api/buildings":      await svc.ListBuildingsAsync();      break;
            case "/api/devices":        await svc.ListDevicesAsync();        break;
            case "/api/device-groups":  await svc.ListDeviceGroupsAsync();   break;
            case "/api/people":         await svc.ListPeopleAsync();         break;
            case "/api/test-groups":    await svc.ListTestGroupsAsync();     break;
            case "/api/reservations":   await svc.ListReservationsAsync();   break;
        }

        Assert.Equal(expected, h.Requests[0].RequestUri!.AbsolutePath);
    }

    // ============================================================
    // Reservation filter query string
    // ============================================================

    [Fact]
    public async Task ListReservations_with_filter_emits_repeated_statusIn_and_iso_dates()
    {
        var (svc, h) = NewService();
        h.EnqueueRaw(HttpStatusCode.OK, "[]");

        var deviceGroupId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        var testGroupId   = Guid.Parse("66666666-6666-6666-6666-666666666666");
        var fromUtc = new DateTime(2026, 5, 9, 12, 0, 0, DateTimeKind.Utc);
        var toUtc   = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc);

        await svc.ListReservationsAsync(new ReservationFilter(
            DeviceGroupId: deviceGroupId,
            TestGroupId: testGroupId,
            FromUtc: fromUtc,
            ToUtc: toUtc,
            StatusIn: new[] { ReservationStatus.Pending, ReservationStatus.Confirmed }));

        var query = h.Requests[0].RequestUri!.Query;
        Assert.Contains($"deviceGroupId={deviceGroupId}".ToLowerInvariant(), query.ToLowerInvariant());
        Assert.Contains($"testGroupId={testGroupId}".ToLowerInvariant(), query.ToLowerInvariant());
        Assert.Contains("fromUtc=", query);
        Assert.Contains("toUtc=", query);
        // Repeated query parameter, one per status.
        Assert.Contains("statusIn=Pending", query);
        Assert.Contains("statusIn=Confirmed", query);
    }

    [Fact]
    public async Task ListReservations_with_null_filter_omits_query_string()
    {
        var (svc, h) = NewService();
        h.EnqueueRaw(HttpStatusCode.OK, "[]");

        await svc.ListReservationsAsync(filter: null);

        Assert.Equal(string.Empty, h.Requests[0].RequestUri!.Query);
    }

    // ============================================================
    // Status-code translation
    // ============================================================

    [Fact]
    public async Task NotFound_response_throws_NotFoundException()
    {
        var (svc, h) = NewService();
        h.EnqueueEmpty(HttpStatusCode.NotFound);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            svc.UpdateBuildingAsync(Guid.NewGuid(), new BuildingUpdate("x", "y"), version: 1));
    }

    [Fact]
    public async Task Conflict_response_throws_ConflictException()
    {
        var (svc, h) = NewService();
        h.EnqueueEmpty(HttpStatusCode.Conflict);

        await Assert.ThrowsAsync<ConflictException>(() =>
            svc.DeleteBuildingAsync(Guid.NewGuid(), version: 1));
    }

    [Fact]
    public async Task BadRequest_with_ApiError_body_throws_ValidationException_with_RuleId()
    {
        var (svc, h) = NewService();
        h.EnqueueRaw(HttpStatusCode.BadRequest,
            "{\"ruleId\":\"R10\",\"message\":\"Group already booked.\"}");

        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            svc.ConfirmReservationAsync(Guid.NewGuid(), version: 1));

        Assert.Equal("R10", ex.RuleId);
        Assert.Contains("Group already booked", ex.Message);
    }

    [Fact]
    public async Task BadRequest_with_unstructured_body_falls_through_to_HttpRequestException()
    {
        var (svc, h) = NewService();
        h.EnqueueRaw(HttpStatusCode.BadRequest, "not-json", "text/plain");

        // ThrowForKnownStatusAsync doesn't synthesise a ValidationException
        // when the body cannot be parsed; EnsureSuccessStatusCode then
        // throws the standard HttpRequestException.
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            svc.CreateBuildingAsync(new BuildingCreate("x", "y")));
    }

    // ============================================================
    // JSON serialisation
    // ============================================================

    [Fact]
    public async Task CreateDeviceGroup_serialises_DeviceLayoutEntry_in_camelCase()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new DeviceGroupDto
        {
            DeviceGroupId = Guid.NewGuid(),
            Name = "G",
            Status = DeviceGroupStatus.Inactive,
            Version = 1,
        });

        var d1 = Guid.NewGuid();
        await svc.CreateDeviceGroupAsync(new DeviceGroupCreate(
            "Bench A",
            new[] { d1 },
            Array.Empty<DeviceConnectionDto>(),
            new[] { new DeviceLayoutEntry(d1, 0.25, 0.75) }));

        var body = h.RequestBodies[0];
        Assert.Contains("\"name\":\"Bench A\"", body);
        // Layout round-trips with camelCase property names.
        Assert.Contains("\"layout\":[", body);
        Assert.Contains("\"deviceId\":", body);
        Assert.Contains("\"x\":0.25", body);
        Assert.Contains("\"y\":0.75", body);
    }

    [Fact]
    public async Task CreateDevice_serialises_DeviceStatus_as_string()
    {
        var (svc, h) = NewService();
        h.EnqueueJson(HttpStatusCode.OK, new DeviceDto
        {
            DeviceId = Guid.NewGuid(),
            Name = "D",
            Status = DeviceStatus.Available,
            BuildingId = Guid.NewGuid(),
            Version = 1,
        });

        await svc.CreateDeviceAsync(new DeviceCreate("D", DeviceStatus.Maintenance, Guid.NewGuid()));

        var body = h.RequestBodies[0];
        // Enum serialised as the string name, not the underlying integer.
        Assert.Contains("\"status\":\"Maintenance\"", body);
        Assert.DoesNotContain("\"status\":1", body);
    }
}
