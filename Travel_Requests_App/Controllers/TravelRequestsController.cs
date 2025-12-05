using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Requests_App.BLL.TravelRequests;
using Travel_Requests_App.DTOs.TravelRequestDTO;
using Travel_Requests_App.DTOs;

namespace Travel_Requests_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelRequestsController : ControllerBase
    {
        private readonly ITravelRequestService travelRequestService;

        public TravelRequestsController(ITravelRequestService travelRequestService)
        {
            this.travelRequestService = travelRequestService;
        }

        [HttpPost("travelrequests/new")]
        public TravelResponseDTO CreatTravelRequest(TravelRequestsReqDTO tRequestDTO)
        {
            var requests = travelRequestService.CreateTravelRequest(tRequestDTO);
            return requests;
        }

        [HttpGet("travelrequest/{HRid}/pending")]

        public List<TravelResponseDTO> GetAllPendingRequests(int HRid)
        {
            var result = travelRequestService.GetAllPendingRequests(HRid);
            return result;
        }

        [HttpGet("travelrequests/{trid}")]
        public TravelResponseDTO getTravelRequestById(int trid)
        {
            var result = travelRequestService.getTravelRequestById(trid);
            return result;
        }

        [HttpPut("travelrequests/{trid}/update")]

        public TravelResponseDTO getUpdateRequestStatus(int trid, UpdateRequestDTO updateDTO)
        {
            var result = travelRequestService.getUpdateRequestStatus(trid, updateDTO);
            return result;
        }

        

    }
}
