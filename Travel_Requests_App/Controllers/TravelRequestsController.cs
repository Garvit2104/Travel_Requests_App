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
        public async Task<TravelResponseDTO> CreatTravelRequest(TravelRequestsReqDTO tRequestDTO)
        {
            var requests = await travelRequestService.CreateTravelRequest(tRequestDTO);
            return requests;
        }

        [HttpGet("travelrequest/{HRid}/pending")]

        public async Task<IActionResult> GetAllPendingRequests(int HRid)
        {
            try
            {
                var result = await travelRequestService.GetAllPendingRequests(HRid);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Problem(title: "Error", detail: ex.Message, statusCode: 400);
            }
        }

        [HttpGet("travelrequests/{trid}")]
        public async Task<ActionResult<TravelResponseDTO>> getTravelRequestById(int trid)
        {
            try
            {
                var result = await travelRequestService.getTravelRequestById(trid);
                return Ok(result);

            }
            catch(Exception ex)
            {
                return Problem(title: "Error", detail: ex.Message, statusCode: 400);
            }
            
        }

        [HttpPut("travelrequests/{trid}/update")]

        public async Task<IActionResult> getUpdateRequestStatus(int trid, UpdateRequestDTO updateDTO)
        {
            try
            {
                var result = await travelRequestService.getUpdateRequestStatus(trid, updateDTO);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Problem(title: "Error", detail: ex.Message, statusCode: 400);
            }
        }

        [HttpPost("travelrequests/calculatebudget/{trid}")]
        public async Task<IActionResult> CalculateBudget(int trid)
        {
            try
            {
                var result = await travelRequestService.CalculateBudget(trid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem(title: "Error", detail: ex.Message, statusCode: 400);
            }
        }



    }
}
