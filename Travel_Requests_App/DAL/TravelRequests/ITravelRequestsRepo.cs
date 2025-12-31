using Travel_Requests_App.Models;

namespace Travel_Requests_App.DAL.TravelRequests
{
    public interface ITravelRequestsRepo
    {
        public Task<TravelRequest> CreateTravelRequest(TravelRequest request);

        public Task<IEnumerable<TravelRequest>> GetAllPendingRequests(int HRid);

        public Task<TravelRequest> getTravelRequestById(int trid);

        public Task<TravelRequest> getUpdateRequestStatus(TravelRequest request);
    }
}
