using RichardSzalay.MockHttp;
using System.Net;

namespace SurveillanceStation.Net.Tests;

public class RecordingTests
{
    private const string BaseUrl = "http://test-url.com";
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SurveillanceStationClient _client;

    public RecordingTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
        _client = new SurveillanceStationClient(BaseUrl, httpClient);
    }

    [Fact]
    public async Task DownloadRecordingAsync_ShouldReturnByteArray_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedContent = new byte[] { 1, 2, 3, 4, 5 };
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Recording/Download/v1")
                 .WithQueryString("camId=1&startTime=2023-09-15 10:00:00&endTime=2023-09-15 11:00:00")
                 .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
                 {
                     Content = new ByteArrayContent(expectedContent)
                 });

        // Act
        var result = await _client.DownloadRecordingAsync("1", "2023-09-15 10:00:00", "2023-09-15 11:00:00");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(expectedContent, result);
    }

    [Fact]
    public async Task DownloadRecordingSnapshotsAsync_ShouldReturnByteArray_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedContent = new byte[] { 5, 4, 3, 2, 1 };
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Recording/DownloadSnapshot/v1")
                 .WithQueryString("startTime=2023-09-15 10:00:00&endTime=2023-09-15 11:00:00&camId=1&interval=60")
                 .Respond(req => new HttpResponseMessage(HttpStatusCode.OK)
                 {
                     Content = new ByteArrayContent(expectedContent)
                 });

        // Act
        var result = await _client.DownloadRecordingSnapshotsAsync("2023-09-15 10:00:00", "2023-09-15 11:00:00", "1", 60);

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(expectedContent, result);
    }
}