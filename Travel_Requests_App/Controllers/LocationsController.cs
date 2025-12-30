using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Requests_App.BLL.Locations;
using Travel_Requests_App.DTOs.Locations;

namespace Travel_Requests_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationServices locationService;

        public LocationsController(ILocationServices locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet("locations")]

        public async Task<IEnumerable<LocationResponsesDTO>> GetAllLocation()
        {
            var locations = await locationService.GetAllLocation();
            return locations;
        }
    }
}
