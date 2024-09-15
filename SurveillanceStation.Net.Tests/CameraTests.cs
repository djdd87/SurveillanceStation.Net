using RichardSzalay.MockHttp;

namespace SurveillanceStation.Net.Tests;

public class CameraTests
{
    private const string BaseUrl = "http://test-url.com";
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SurveillanceStationClient _client;

    public CameraTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
        _client = new SurveillanceStationClient(BaseUrl, httpClient);
    }

    [Fact]
    public async Task ListCamerasAsync_ShouldReturnCameraList_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"total\":1,\"cameras\":[{\"camId\":1,\"name\":\"Test Camera\",\"ip\":\"192.168.1.100\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Camera/List/v1")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.ListCamerasAsync();

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(1, result.Total);
        Assert.Single(result.Cameras);
        Assert.Equal(1, result.Cameras[0].CamId);
        Assert.Equal("Test Camera", result.Cameras[0].Name);
        Assert.Equal("192.168.1.100", result.Cameras[0].Ip);
    }

    [Fact]
    public async Task EditCameraAsync_ShouldReturnUpdatedCameraInfo_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"camera\":{\"camId\":1,\"name\":\"Updated Camera\",\"ip\":\"192.168.1.100\"}}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Camera/Edit/v1")
                 .WithQueryString("camId=1&newName=Updated Camera")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.EditCameraAsync("1", newName: "Updated Camera");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(1, result.CamId);
        Assert.Equal("Updated Camera", result.Name);
        Assert.Equal("192.168.1.100", result.Ip);
    }

    [Fact]
    public async Task GetCameraInfoAsync_ShouldReturnCameraInfo_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"cameras\":[{\"camId\":1,\"name\":\"Test Camera\",\"ip\":\"192.168.1.100\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Camera/Get/v1")
                 .WithQueryString("camIds=1")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.GetCameraInfoAsync("1");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Single(result);
        Assert.Equal(1, result[0].CamId);
        Assert.Equal("Test Camera", result[0].Name);
        Assert.Equal("192.168.1.100", result[0].Ip);
    }
}