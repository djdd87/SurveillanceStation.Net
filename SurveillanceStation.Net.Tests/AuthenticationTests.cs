using NSubstitute;
using RichardSzalay.MockHttp;

namespace SurveillanceStation.Net.Tests;

public class AuthenticationTests
{
    private const string BaseUrl = "http://test-url.com";
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly SurveillanceStationClient _client;

    public AuthenticationTests()
    {
        _mockHttp = new MockHttpMessageHandler();

        var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
        _client = new SurveillanceStationClient(BaseUrl, httpClient);
    }

    [Fact]
    public async Task LoginAsync_ShouldSetSessionId_WhenCredentialsAreValid()
    {
        // Arrange
        var expectedResponse = "{\"success\":true,\"data\":{\"sid\":\"test-session-id\"}}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Auth/Login/v1")
                 .WithQueryString("account=testuser&passwd=testpassword")
                 .Respond("application/json", expectedResponse);

        // Act
        await _client.LoginAsync("testuser", "testpassword");

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Equal("test-session-id", _client.Sid);
    }

    [Fact]
    public async Task LogoutAsync_ShouldClearSessionId()
    {
        // Arrange
        var expectedResponse = "{\"success\":true}";
        _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Auth/Logout/v1")
                 .Respond("application/json", expectedResponse);

        // Set a session ID
        _client.Login("test-session-id");

        // Act
        await _client.LogoutAsync();

        // Assert
        _mockHttp.VerifyNoOutstandingExpectation();
        Assert.Null(_client.Sid);
    }
}