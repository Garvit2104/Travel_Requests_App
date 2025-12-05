using Travel_Requests_App.Models;

namespace Travel_Requests_App.DAL.TravelRequests
{
    public interface ITravelRequestsRepo
    {
        public TravelRequest CreateTravelRequest(TravelRequest request);

        public List<TravelRequest> GetAllPendingRequests(int HRid);

        TravelRequest getTravelRequestById(int trid);

        TravelRequest getUpdateRequestStatus(TravelRequest request);
    }
}
