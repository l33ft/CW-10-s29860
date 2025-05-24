using Microsoft.AspNetCore.Mvc;
using TripApi.DTOs;
using TripApi.Services;

namespace TripApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        [HttpGet]
        public async Task<ActionResult<TripResponseDto>> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page i pageSize muszą być większe od 0");
            }

            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<ActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientToTripDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tripService.AssignClientToTripAsync(idTrip, dto);
            
            if (result == "Klient został pomyślnie przypisany do wycieczki")
            {
                return Ok(new { message = result });
            }
            
            return BadRequest(new { message = result });
        }
    }
}