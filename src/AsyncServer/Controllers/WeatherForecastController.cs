using AsyncServer.Services;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private WeatherForecastService _service;

        public WeatherForecastController(WeatherForecastService service)
        {
            _service = service;
        }

        [HttpGet(Name = "GetWeatherForecast/{date}")]
        public async Task<IActionResult> Get(DateOnly date)
        {
            Guid requestId = Guid.NewGuid();
            _ = _service.GetForecast(requestId, date);

            return await Task.FromResult(Accepted($"/WeatherForecast/Status/{requestId}"));
        }

        [HttpPost("GetAsyncWeatherForecast/{date}")]
        public async Task<IActionResult> GetByQueue(DateOnly date)
        {
            Guid requestId = Guid.NewGuid();
            await _service.SetForecastRequestToQueue(requestId, date);

            return Accepted($"/AsyncWeatherForecast/Status/{requestId}");
        }

        [HttpGet("Status/{requestId}")]
        public async Task<IActionResult> GetStatus(Guid requestId)
        {
            var status = await _service.GetRequestStatus(requestId);
            if(status == OperationStatus.Finished)
            {
                Response.Headers.Add("Location", $"/WeatherForecast/{requestId}");
                return StatusCode(StatusCodes.Status303SeeOther);
            } else
            {
                var data = new
                {
                    status = status
                };
                return Ok(data);
            }
        }

        [HttpGet("{requestId}")]
        public async Task<IActionResult> GetReadyData(Guid requestId)
        {
            var result = await _service.GetReadyForecast(requestId);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
    }
}