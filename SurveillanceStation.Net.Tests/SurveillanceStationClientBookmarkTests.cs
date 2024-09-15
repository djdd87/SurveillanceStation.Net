using RichardSzalay.MockHttp;

namespace SurveillanceStation.Net.Tests;

public class SurveillanceStationClientBookmarkTests
{
    private const string BaseUrl = "http://test-url.com";
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SurveillanceStationClient _client;

    public SurveillanceStationClientBookmarkTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
        _client = new SurveillanceStationClient(BaseUrl, httpClient);
    }

    [Fact]
    public async Task CreateBookmarkAsync_ShouldReturnBookmarkInfo_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"bookmark\":[{\"bookmarkId\":1,\"name\":\"Test Bookmark\",\"camId\":1,\"startTime\":\"2023-09-15 10:00:00\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Bookmark/Create/v1")
                 .WithQueryString("camId=1&name=Test Bookmark&startTime=2023-09-15 10:00:00")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.CreateBookmarkAsync("1", "Test Bookmark", "2023-09-15 10:00:00");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal(1, result.BookmarkId);
        Assert.Equal("Test Bookmark", result.Name);
        Assert.Equal(1, result.CamId);
        Assert.Equal("2023-09-15 10:00:00", result.StartTime);
    }

    [Fact]
    public async Task ListBookmarksAsync_ShouldReturnBookmarkList_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"bookmarks\":[{\"bookmarkId\":1,\"name\":\"Test Bookmark\",\"camId\":1,\"startTime\":\"2023-09-15 10:00:00\"}]}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Bookmark/List/v1")
                 .WithQueryString("camIds=1")
                 .Respond("application/json", expectedResponse);

        // Act
        var result = await _client.ListBookmarksAsync("1");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Single(result.Bookmarks);
        Assert.Equal(1, result.Bookmarks[0].BookmarkId);
        Assert.Equal("Test Bookmark", result.Bookmarks[0].Name);
        Assert.Equal(1, result.Bookmarks[0].CamId);
        Assert.Equal("2023-09-15 10:00:00", result.Bookmarks[0].StartTime);
    }
}