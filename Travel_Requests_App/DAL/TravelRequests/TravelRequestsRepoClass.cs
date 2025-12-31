using Microsoft.EntityFrameworkCore;
using Travel_Requests_App.Data;
using Travel_Requests_App.DTOs.TravelRequestDTO;
using Travel_Requests_App.Models;

namespace Travel_Requests_App.DAL.TravelRequests
{
    public class TravelRequestsRepoClass : ITravelRequestsRepo
    {
        private readonly TravelPlannerDbContext context;

        public TravelRequestsRepoClass(TravelPlannerDbContext context)
        {
            this.context = context;
        }
        public async Task<TravelRequest> CreateTravelRequest(TravelRequest request)
        {
            var savedResult = await context.TravelRequests.AddAsync(request);
            await context.SaveChangesAsync(); 
            return savedResult.Entity;
        }

        public async Task<IEnumerable<TravelRequest>> GetAllPendingRequests(int HRid)
        {
            var pendingUsers =  context.TravelRequests.AsNoTracking().Where(u => u.ToBeApprovedByHrId == HRid && u.RequestStatus == "New").AsEnumerable();
            return await Task.FromResult(pendingUsers);
        }

        public async Task<TravelRequest> getTravelRequestById(int trid)
        {
            var result = await context.TravelRequests.FindAsync(trid);
            return result;
        }

        public async Task<TravelRequest> getUpdateRequestStatus( TravelRequest request)
        {
            context.Entry(request).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return request;
        }

        
    }
}
