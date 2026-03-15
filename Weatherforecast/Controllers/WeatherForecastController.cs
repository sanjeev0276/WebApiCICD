using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Weatherforecast.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IConfiguration _config;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration)
    {
      _logger = logger;
      _config = configuration;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast
      {
        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
      // Hardcoded check for local testing (No SQL needed)
      if (request.Email == "admin@test.com" && request.Password == "password123")
      {
        var mToken = new ManualToken(_config);
        string token = mToken.GetToken(); // Your existing logic

        return Ok(new { Token = token });
      }

      // Return 401 if credentials don't match
      return Unauthorized(new { message = "Invalid email or password" });
    }



  }
}
