namespace SurveillanceStation.Net;

public partial class SurveillanceStationClient
{
    public async Task<LicensePlateInfo> CreateLicensePlateAsync(string plateNumber, int type = 1, string description = null)
    {
        var data = new { plateNumber, type, description };
        var result = await SendRequestAsync<LicensePlateCreateResponse>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Database/Create/v1", HttpMethod.Get, data);
        return result.LicensePlate[0];
    }

    public async Task<LicensePlateListResponse> ListLicensePlatesAsync(string keyword = null)
    {
        var data = new { keyword };
        return await SendRequestAsync<LicensePlateListResponse>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Database/List/v1", HttpMethod.Get, data);
    }

    public async Task<LicensePlateInfo> EditLicensePlateAsync(string plateNumber, string newPlateNumber = null, int? type = null, string description = null)
    {
        var data = new { plateNumber, newPlateNumber, type, description };
        var result = await SendRequestAsync<LicensePlateEditResponse>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Database/Edit/v1", HttpMethod.Get, data);
        return result.LicensePlate[0];
    }

    public async Task DeleteLicensePlatesAsync(string plateNumbers)
    {
        var data = new { plateNumbers };
        await SendRequestAsync<object>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Database/Delete/v1", HttpMethod.Get, data);
    }

    public async Task<LicensePlateEventListResponse> ListLicensePlateEventsAsync(string camIds = null, string plateNumbers = null, string startTime = null, string endTime = null, bool withThumbnail = false, int limit = 100)
    {
        var data = new { camIds, plateNumbers, startTime, endTime, withThumbnail, limit };
        return await SendRequestAsync<LicensePlateEventListResponse>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Event/ListEvent/v1", HttpMethod.Get, data);
    }

    public async Task<byte[]> DownloadLicensePlateEventRecordingAsync(int eventId, string dsId = "0")
    {
        var data = new { eventId, dsId };
        return await SendRequestAsync<byte[]>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Event/DownloadRecording/v1", HttpMethod.Get, data);
    }

    public async Task DownloadLicensePlateReportAsync(string camIds = null, string startTime = null, string endTime = null, int downloadFormat = 0)
    {
        var data = new { camIds, startTime, endTime, downloadFormat };
        await SendRequestAsync<object>("/webapi/SurveillanceStation/ThirdParty/LicensePlate/Event/DownloadReport/v1", HttpMethod.Get, data);
    }
}

public class LicensePlateCreateResponse
{
    public LicensePlateInfo[] LicensePlate { get; set; }
}

public class LicensePlateListResponse
{
    public int Total { get; set; }
    public LicensePlateInfo[] LicensePlates { get; set; }
}

public class LicensePlateEditResponse
{
    public LicensePlateInfo[] LicensePlate { get; set; }
}

public class LicensePlateEventListResponse
{
    public int Total { get; set; }
    public LicensePlateEventInfo[] Events { get; set; }
}

public class LicensePlateInfo
{
    public string PlateNumber { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
}

public class LicensePlateEventInfo
{
    public string PlateNumber { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
    public string Comment { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public int EventId { get; set; }
    public bool Locked { get; set; }
    public string LicensePlateThumbnail { get; set; }
    public string VehicleThumbnail { get; set; }
    public int CamId { get; set; }
    public int DsId { get; set; }
    public string CamName { get; set; }
}