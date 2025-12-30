using Travel_Requests_App.DTOs.Locations;
using Travel_Requests_App.DTOs.TravelRequestDTO;

namespace Travel_Requests_App.BLL.Locations
{
    public interface ILocationServices
    {
        public Task<IEnumerable<LocationResponsesDTO>> GetAllLocation();

        
    }
}
