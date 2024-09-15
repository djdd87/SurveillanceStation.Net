
using RichardSzalay.MockHttp;

namespace SurveillanceStation.Net.Tests;

    public class SurveillanceStationClientMapTests
    {
        private const string BaseUrl = "http://test-url.com";
        private readonly MockHttpMessageHandler _mockHttp;
        private readonly SurveillanceStationClient _client;

        public SurveillanceStationClientMapTests()
        {
            _mockHttp = new MockHttpMessageHandler();
            var httpClient = new HttpClient(_mockHttp) { BaseAddress = new Uri(BaseUrl) };
            _client = new SurveillanceStationClient(BaseUrl, httpClient);
        }

        [Fact]
        public async Task UpdateDeviceLocationAsync_ShouldReturnUpdatedGeoMapItem_WhenApiCallIsSuccessful()
        {
            // Arrange
            var expectedResponse = "{\"success\":true,\"data\":{\"geoMapItem\":[{\"deviceId\":\"1\",\"latitude\":40.7128,\"longitude\":-74.0060,\"direction\":90,\"viewAngle\":120,\"radius\":100}]}}";
            _mockHttp.Expect(HttpMethod.Get, $"{BaseUrl}/webapi/SurveillanceStation/ThirdParty/Emap/ChangeLocation/v1")
                     .WithQueryString("deviceId=1&latitude=40.7128&longitude=-74.006&radius=100&viewAngle=120&direction=90")
                     .Respond("application/json", expectedResponse);

            // Act
            var result = await _client.UpdateDeviceLocationAsync("1", 40.7128, -74.0060, 100, 120, 90);

            // Assert
            _mockHttp.VerifyNoOutstandingExpectation();
            Assert.Equal("1", result.DeviceId);
            Assert.Equal(40.7128, result.Latitude);
            Assert.Equal(-74.0060, result.Longitude);
            Assert.Equal(90, result.Direction);
            Assert.Equal(120, result.ViewAngle);
            Assert.Equal(100, result.Radius);
        }
    }