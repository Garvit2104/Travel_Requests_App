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
        public TravelRequest CreateTravelRequest(TravelRequest request)
        {
            var savedResult = context.TravelRequests.Add(request).Entity;
            context.SaveChanges(); 
            return savedResult;
        }

        public List<TravelRequest> GetAllPendingRequests(int HRid)
        {
            var pendingUsers = context.TravelRequests.Where(u => u.ToBeApprovedByHrId == HRid && u.RequestStatus == "New").ToList();
            return pendingUsers;
        }

        public TravelRequest getTravelRequestById(int trid)
        {
            var result = context.TravelRequests.Find(trid);
            return result;
        }

        public TravelRequest getUpdateRequestStatus( TravelRequest request)
        {
            context.Entry(request).State = EntityState.Modified;
            context.SaveChanges();

            return request;
        }

        
    }
}
