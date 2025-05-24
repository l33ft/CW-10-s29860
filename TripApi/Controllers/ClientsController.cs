using Microsoft.AspNetCore.Mvc;
using TripApi.Services;

namespace TripApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpDelete("{idClient}")]
        public async Task<ActionResult> DeleteClient(int idClient)
        {
            var result = await _clientService.DeleteClientAsync(idClient);
            
            if (result == "Klient został pomyślnie usunięty")
            {
                return Ok(new { message = result });
            }
            
            return BadRequest(new { message = result });
        }
    }
}