namespace SurveillanceStation.Net;

public partial class SurveillanceStationClient
{
    public async Task<GeoMapItem> UpdateDeviceLocationAsync(string deviceId, double? longitude = null, double? latitude = null, int? radius = null, int? viewAngle = null, int? direction = null)
    {
        var data = new { deviceId, longitude, latitude, radius, viewAngle, direction };
        var result = await SendRequestAsync<MapUpdateResponse>("/webapi/SurveillanceStation/ThirdParty/Emap/ChangeLocation/v1", HttpMethod.Get, data);
        return result.GeoMapItem[0];
    }
}

public class MapUpdateResponse
{
    public GeoMapItem[] GeoMapItem { get; set; }
}

public class GeoMapItem
{
    public string DeviceId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Direction { get; set; }
    public int ViewAngle { get; set; }
    public int Radius { get; set; }
}