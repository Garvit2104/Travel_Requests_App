using Travel_Requests_App.Models;

namespace Travel_Requests_App.DAL.Locations
{
    public interface ILocationRepo
    {
        public Task<IEnumerable<Location>> GetAllLocations();
    }
}
