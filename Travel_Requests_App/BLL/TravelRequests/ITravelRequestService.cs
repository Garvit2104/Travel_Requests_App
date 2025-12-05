using Travel_Requests_App.DTOs;
using Travel_Requests_App.DTOs.TravelRequestDTO;
using Travel_Requests_App.Models;

namespace Travel_Requests_App.BLL.TravelRequests
{
    public interface ITravelRequestService
    {
        TravelResponseDTO CreateTravelRequest(TravelRequestsReqDTO travelRequestDTO);

        List<TravelResponseDTO> GetAllPendingRequests(int HRid);

        TravelResponseDTO getTravelRequestById(int trid);

        TravelResponseDTO getUpdateRequestStatus(int trid, UpdateRequestDTO updateDTO);

        public int CalculateBudget(int travelRequestId);

        


    }
}
