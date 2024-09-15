using RichardSzalay.MockHttp;
using System.Net;

namespace SurveillanceStation.Net.Tests;

public class SurveillanceStationClientSnapshotTests
{
    private const string BaseUrl = "http://test-url.com";
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SurveillanceStationClient _client;

    public SurveillanceStationClientSnapshotTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
        _client = new SurveillanceStationClient(BaseUrl, httpClient);
    }

    [Fact]
    public async Task TakeAndDownloadSnapshotAsync_ShouldReturnJpegByteArray_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedContent = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }; // JPEG file signature
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/SnapShot/Take/v1")
                 .WithQueryString("camId=1&profileType=0&download=true&save=false")
                 .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
                 {
                     Content = new ByteArrayContent(expectedContent)
                 });

        // Act
        var result = await _client.TakeAndDownloadSnapshotAsync("1");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(expectedContent, result);
    }

    [Fact]
    public async Task TakeAndSaveSnapshotAsync_ShouldReturnSnapshotInfo_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"dsId\":1,\"snapshotId\":100,\"camName\":\"Test Camera\"}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/SnapShot/Take/v1")
                 .WithQueryString("camId=1&profileType=0&download=false&save=true")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.TakeAndSaveSnapshotAsync("1");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(1, result.DsId);
        Assert.Equal(100, result.SnapshotId);
        Assert.Equal("Test Camera", result.CamName);
    }
}