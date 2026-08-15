using System.Net;
using System.Net.Http.Json;
using TrainingCenter.Api.DTOs;

namespace TrainingCenter.Api.Tests;

public class BusinessRulesIntegrationTests : IClassFixture<TrainingCenterWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly TrainingCenterWebApplicationFactory _factory;

    public BusinessRulesIntegrationTests(TrainingCenterWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateStudent_DuplicateEmail_Returns409()
    {
        await _factory.ResetDatabaseAsync();

        var request = new CreateStudentRequest("Duplicate Test", "mohamed@example.com", "01000000000");
        var response = await _client.PostAsJsonAsync("/api/students", request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task CreateEnrollment_DuplicateActiveEnrollment_Returns400()
    {
        await _factory.ResetDatabaseAsync();

        var request = new CreateEnrollmentRequest(1, 1);
        var response = await _client.PostAsJsonAsync("/api/enrollments", request);
        var body = await response.Content.ReadAsStringAsync();

        Assert.True(response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.Conflict,
            $"Expected 400/409 but got {(int)response.StatusCode}: {body}");
    }

    [Fact]
    public async Task CreatePayment_Overpayment_Returns400()
    {
        await _factory.ResetDatabaseAsync();

        var payment = new CreatePaymentRequest(3, 5000m, "Online", "PAY-OVER", null);
        var response = await _client.PostAsJsonAsync("/api/payments", payment);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreatePayment_OnCancelledEnrollment_Returns400()
    {
        await _factory.ResetDatabaseAsync();

        await _client.PutAsJsonAsync("/api/enrollments/2/status",
            new UpdateEnrollmentStatusRequest("Cancelled"));

        var payment = new CreatePaymentRequest(2, 100m, "Online", "PAY-CANCEL", null);
        var response = await _client.PostAsJsonAsync("/api/payments", payment);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetTracks_InvalidStatusFilter_Returns400()
    {
        await _factory.ResetDatabaseAsync();

        var response = await _client.GetAsync("/api/tracks?status=NotARealStatus");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTrack_WithActiveEnrollments_Returns409()
    {
        await _factory.ResetDatabaseAsync();

        var response = await _client.DeleteAsync("/api/tracks/1");

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task DeleteTrack_NotFound_Returns404()
    {
        await _factory.ResetDatabaseAsync();

        var response = await _client.DeleteAsync("/api/tracks/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateTrack_InvalidFee_Returns400()
    {
        await _factory.ResetDatabaseAsync();

        var request = new CreateTrackRequest(
            "Invalid Fee Track", "BAD-FEE", null, "Beginner",
            10, 0m, DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddMonths(1), 1);

        var response = await _client.PostAsJsonAsync("/api/tracks", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SoftDeleteStudent_SetsInactive()
    {
        await _factory.ResetDatabaseAsync();

        var deleteResponse = await _client.DeleteAsync("/api/students/2");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var enrollResponse = await _client.PostAsJsonAsync("/api/enrollments",
            new CreateEnrollmentRequest(2, 2));

        Assert.Equal(HttpStatusCode.BadRequest, enrollResponse.StatusCode);
    }
}
