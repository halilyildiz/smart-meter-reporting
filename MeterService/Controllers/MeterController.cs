using Microsoft.AspNetCore.Mvc;

namespace MeterService.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MeterController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetMeters()
    {
      return Ok("Meter data");
    }
  }
}
