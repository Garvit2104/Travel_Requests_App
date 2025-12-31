using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Requests_App.BLL.ClientService;
using Travel_Requests_App.BLL.TravelRequests;
using Travel_Requests_App.DTOs.Grades;

namespace Travel_Requests_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesClientAPIController : ControllerBase
    {
        private readonly HrClientService _htrApiClient;
        private readonly ITravelRequestService travelReqService;
        public GradesClientAPIController(HrClientService _htrApiClient, ITravelRequestService travelReqService)
        {
            this._htrApiClient = _htrApiClient;
            this.travelReqService = travelReqService;
        }

        [HttpGet("employee/{id}")]

        public async Task<UserResponseDTO> GetEmployeeById(int id)
        {
            var result = await _htrApiClient.GetEmployeeById(id);
            return result;
        }

        //[HttpPut("empoyeed/{id}")]

        //public async Task<UserResponseDTO> updateEmployeeById(int id, UserRequestDTO userRequestDTO)
        //{
        //    var result = await _htrApiClient.UpdateEmployeeByIdAsync(id, userRequestDTO);
        //    return result;
        //}

       

        

    }
}
