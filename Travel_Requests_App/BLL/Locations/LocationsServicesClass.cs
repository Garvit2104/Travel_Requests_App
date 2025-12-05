using Travel_Requests_App.DAL.Locations;
using Travel_Requests_App.DTOs.Locations;
using Travel_Requests_App.Models;
namespace Travel_Requests_App.BLL.Locations
{
    public class LocationsServicesClass : ILocationServices
    {
        private readonly ILocationRepo locationRepo;

        public LocationsServicesClass(ILocationRepo locationRepo)
        {
            this.locationRepo = locationRepo;
        }

        public List<LocationResponsesDTO> GetAllLocation()
        {
            var result = locationRepo.GetAllLocations();

            List<LocationResponsesDTO> ls = new List<LocationResponsesDTO>();

            foreach(var item in result)
            {
                LocationResponsesDTO locationDTO = new LocationResponsesDTO();
                locationDTO.id = item.Id;
                locationDTO.name = item.Name;

                ls.Add(locationDTO);
            }
            return ls;
        }
    }
}
