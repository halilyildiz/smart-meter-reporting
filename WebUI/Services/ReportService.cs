using System.ComponentModel;
using WebUI.Models;

namespace WebUI.Services
{
  public class ReportService
  {
    private readonly HttpClient _httpClient;

    public ReportService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<ReportResponse[]> GetReportsAsync()
    {
      var response = await _httpClient.GetAsync("http://localhost:5116/api/reports");
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadFromJsonAsync<ReportResponse[]>();
    }

    public async Task<Stream> GetReportAsync(Guid reportId)
    {
      var response = await _httpClient.GetAsync($"http://localhost:5116/api/reports/{reportId}");

      if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        return null; 
      }

      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStreamAsync();
    }

    public async Task<bool> RequestReportAsync(string serialNumber)
    {
      if (string.IsNullOrEmpty(serialNumber))
      {
        throw new ArgumentException("Seri numarası boş olamaz.", nameof(serialNumber));
      }

      var response = await _httpClient.PostAsJsonAsync($"http://localhost:5116/api/reports/{serialNumber}", serialNumber);
      response.EnsureSuccessStatusCode();

      return response.IsSuccessStatusCode;
    }
  }

}
