using Travel_Requests_App.Data;
using Travel_Requests_App.Models;
namespace Travel_Requests_App.DAL.Locations
{
    public class LocationsRepoClass : ILocationRepo
    {
        private readonly TravelPlannerDbContext context;

        public LocationsRepoClass(TravelPlannerDbContext context)
        {
            this.context = context;
        }

        public List<Location> GetAllLocations()
        {
            var result = context.Locations.ToList();
            return result;
        }
    }
}
