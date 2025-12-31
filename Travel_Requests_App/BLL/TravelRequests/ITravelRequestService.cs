using Travel_Requests_App.DTOs;
using Travel_Requests_App.DTOs.TravelRequestDTO;
using Travel_Requests_App.Models;

namespace Travel_Requests_App.BLL.TravelRequests
{
    public interface ITravelRequestService
    {
        public Task<TravelResponseDTO> CreateTravelRequest(TravelRequestsReqDTO travelRequestDTO);

        Task<IEnumerable<TravelResponseDTO>> GetAllPendingRequests(int HRid);

        Task<TravelResponseDTO> getTravelRequestById(int trid);

        Task<TravelResponseDTO> getUpdateRequestStatus(int trid, UpdateRequestDTO updateDTO);

        public Task<int> CalculateBudget(int travelRequestId);

        


    }
}
