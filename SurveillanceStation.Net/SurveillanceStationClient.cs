using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

namespace SurveillanceStation.Net;

public partial class SurveillanceStationClient : ISurveillanceStationClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public string Sid { get => _sid; }
    private string _sid;

    public SurveillanceStationClient(string baseUrl, HttpClient httpClient)
    {
        _baseUrl = baseUrl;
        _httpClient = httpClient;
    }

    private async Task<T> SendRequestAsync<T>(string endpoint, HttpMethod method, object data = null)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        if (data != null)
        {
            foreach (var prop in data.GetType().GetProperties())
            {
                object value = prop.GetValue(data);
                if (value is bool)
                {
                    value = value.ToString().ToLower();
                }

                if (value != null)
                {
                    query[prop.Name] = value?.ToString();
                }
            }
        }

        var uriBuilder = new UriBuilder($"{_baseUrl}{endpoint}");
        uriBuilder.Query = query.ToString();

        var request = new HttpRequestMessage(method, uriBuilder.Uri);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        if (typeof(T) == typeof(byte[]))
        {
            return (T)(object)await response.Content.ReadAsByteArrayAsync();
        }
        else if (typeof(T) == typeof(HttpResponseMessage))
        {
            return (T)(object)response;
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<T>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (!result.Success)
            {
                throw new ApiException(result.Error?.Code ?? 0, result.Error?.Message ?? "Unknown error");
            }

            return result.Data;
        }
    }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public ErrorInfo Error { get; set; }
}

public class ErrorInfo
{
    public int Code { get; set; }
    public string Message { get; set; }
}

public class ApiException : Exception
{
    public int ErrorCode { get; }

    public ApiException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}