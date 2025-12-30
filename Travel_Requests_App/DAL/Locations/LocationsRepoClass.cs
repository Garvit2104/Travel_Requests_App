using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Location>> GetAllLocations()
        {
            var result =  context.Locations.AsNoTracking().AsEnumerable();
            return await Task.FromResult(result);
        }
    }
}
