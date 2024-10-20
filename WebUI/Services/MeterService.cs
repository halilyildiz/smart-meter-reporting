using WebUI.Models;

namespace WebUI.Services
{
  public class MeterService
  {
    private readonly HttpClient _httpClient;

    public MeterService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<MeterResponse> GetLastMeterDataAsync(string serialNumber)
    {
      var response = await _httpClient.GetAsync($"http://localhost:5251/api/meters/GetLastMeterData/{serialNumber}");
      if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        return null;
      }

      response.EnsureSuccessStatusCode();
      return await response.Content.ReadFromJsonAsync<MeterResponse>();
    }

    public async Task<MeterResponse> SaveMeterDataAsync(MeterRequest meterRequest)
    {
      if (meterRequest == null)
      {
        throw new ArgumentNullException(nameof(meterRequest), "MeterMeasurementRequest boş olamaz.");
      }

      var response = await _httpClient.PostAsJsonAsync("http://localhost:5251/api/meters", meterRequest);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadFromJsonAsync<MeterResponse>();
    }
  }
}
