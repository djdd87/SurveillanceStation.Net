
namespace SurveillanceStation.Net;

public partial class SurveillanceStationClient
{
    public async Task LoginAsync(string account, string password)
    {
        var data = new { account, passwd = password };
        var result = await SendRequestAsync<LoginResponse>("/webapi/SurveillanceStation/ThirdParty/Auth/Login/v1", HttpMethod.Get, data);
        _sid = result.Sid;
    }

    public async Task LogoutAsync()
    {
        await SendRequestAsync<object>("/webapi/SurveillanceStation/ThirdParty/Auth/Logout/v1", HttpMethod.Get);
        _sid = null;
    }
}

public class LoginResponse
{
    public string Sid { get; set; }
}