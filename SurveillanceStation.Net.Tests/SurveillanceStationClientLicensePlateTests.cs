using RichardSzalay.MockHttp;

namespace SurveillanceStation.Net.Tests;

public class SurveillanceStationClientLicensePlateTests
{
    private const string BaseUrl = "http://test-url.com";
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SurveillanceStationClient _client;

    public SurveillanceStationClientLicensePlateTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
        _client = new SurveillanceStationClient(BaseUrl, httpClient);
    }

    [Fact]
    public async Task CreateLicensePlateAsync_ShouldReturnLicensePlateInfo_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"licensePlate\":[{\"plateNumber\":\"ABC123\",\"type\":1,\"description\":\"Test plate\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/LicensePlate/Database/Create/v1")
                 .WithQueryString("plateNumber=ABC123&type=1&description=Test plate")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.CreateLicensePlateAsync("ABC123", 1, "Test plate");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal("ABC123", result.PlateNumber);
        Assert.Equal(1, result.Type);
        Assert.Equal("Test plate", result.Description);
    }

    [Fact]
    public async Task ListLicensePlatesAsync_ShouldReturnLicensePlateList_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"total\":1,\"licensePlates\":[{\"plateNumber\":\"ABC123\",\"type\":1,\"description\":\"Test plate\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/LicensePlate/Database/List/v1")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.ListLicensePlatesAsync();

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(1, result.Total);
        Assert.Single(result.LicensePlates);
        Assert.Equal("ABC123", result.LicensePlates[0].PlateNumber);
        Assert.Equal(1, result.LicensePlates[0].Type);
        Assert.Equal("Test plate", result.LicensePlates[0].Description);
    }

    [Fact]
    public async Task ListLicensePlateEventsAsync_ShouldReturnLicensePlateEventList_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"total\":1,\"events\":[{\"plateNumber\":\"ABC123\",\"camId\":1,\"startTime\":\"2023-09-15 10:00:00\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/LicensePlate/Event/ListEvent/v1")
                 .WithQueryString("camIds=1&startTime=2023-09-15 10:00:00&endTime=2023-09-15 11:00:00")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.ListLicensePlateEventsAsync("1", startTime: "2023-09-15 10:00:00", endTime: "2023-09-15 11:00:00");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(1, result.Total);
        Assert.Single(result.Events);
        Assert.Equal("ABC123", result.Events[0].PlateNumber);
        Assert.Equal(1, result.Events[0].CamId);
        Assert.Equal("2023-09-15 10:00:00", result.Events[0].StartTime);
    }
}