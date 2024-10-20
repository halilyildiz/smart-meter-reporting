using ReportService.Models;

namespace ReportService.Services
{
  public class MeterServiceHttpClient
  {
    private readonly HttpClient _httpClient;

    public MeterServiceHttpClient(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<List<MeterDataDTO>> GetMeterDataAsync(string serialNumber)
    {
        var response = await _httpClient.GetAsync($"http://localhost:5251/api/Meters/{serialNumber}");
        response.EnsureSuccessStatusCode();

        var meterDatas = await response.Content.ReadFromJsonAsync<List<MeterDataDTO>>();

        return meterDatas != null ? meterDatas : new List<MeterDataDTO>();
    }
  }
}
