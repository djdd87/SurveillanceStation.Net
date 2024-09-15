namespace SurveillanceStation.Net;

public partial class SurveillanceStationClient
{
    public async Task<byte[]> TakeAndDownloadSnapshotAsync(string camId, int profileType = 0, bool save = false, string time = null)
    {
        var data = new
        {
            camId,
            profileType,
            download = true,
            save,
            time
        };

        var response = await SendRequestAsync<HttpResponseMessage>("/webapi/SurveillanceStation/ThirdParty/SnapShot/Take/v1", HttpMethod.Get, data);
        return await response.Content.ReadAsByteArrayAsync();
    }

    public async Task<SnapshotInfo> TakeAndSaveSnapshotAsync(string camId, int profileType = 0, string time = null)
    {
        var data = new
        {
            camId,
            profileType,
            download = false,
            save = true,
            time
        };

        return await SendRequestAsync<SnapshotInfo>("/webapi/SurveillanceStation/ThirdParty/SnapShot/Take/v1", HttpMethod.Get, data);
    }
}

public class SnapshotInfo
{
    public int DsId { get; set; }
    public int SnapshotId { get; set; }
    public required string CamName { get; set; }
}