using Travel_Requests_App.DTOs.TravelRequestDTO;
using Travel_Requests_App.DAL.TravelRequests;
using Travel_Requests_App.Models;
using Travel_Requests_App.DTOs;
using Travel_Requests_App.BLL.ClientService;
using Travel_Requests_App.DTOs.Grades;
using Travel_Requests_App.DAL.TravelBudgets;
namespace Travel_Requests_App.BLL.TravelRequests
{
    public class TravelRequestService : ITravelRequestService
    {
        private readonly ITravelRequestsRepo travelRequestRepo;
        private readonly ITravelBudgetRepo travelBudgetRepo;
        private readonly HrClientService hrClient;

        public TravelRequestService(ITravelRequestsRepo travelRequestRepo, ITravelBudgetRepo travelBudgetRepo, HrClientService hrClient)
        {
            this.travelRequestRepo = travelRequestRepo;
            this.travelBudgetRepo = travelBudgetRepo;
            this.hrClient = hrClient;
        }

        public static TravelRequest RequestDtoToEntity(TravelRequestsReqDTO travelRequestDTO)
        {
            TravelRequest trEntity = new TravelRequest();
            trEntity.RaisedByEmployeeId = travelRequestDTO.raised_by_employee_id;
            trEntity.ToBeApprovedByHrId = travelRequestDTO.to_be_approved_by_hr_id;
            trEntity.RequestRaisedOn = DateTime.Now;
            trEntity.FromDate = travelRequestDTO.from_date;
            trEntity.ToDate = travelRequestDTO.to_date;
            trEntity.PurposeOfTravel = travelRequestDTO.purpose_of_travel;
            trEntity.LocationId = travelRequestDTO.location_id;
            trEntity.RequestStatus = travelRequestDTO.request_status;
            trEntity.RequestApprovedOn = DateTime.Now;
            trEntity.Priority = travelRequestDTO.Priority;

            if (trEntity.FromDate <= DateTime.Now.Date) 
                throw new ArgumentException("FromDate must be greater than today's date.");

            if (trEntity.ToDate <= trEntity.FromDate) 
                throw new ArgumentException("ToDate must be greater than FromDate.");

            return trEntity;
        }

        public static TravelResponseDTO EntityToResponseDto(TravelRequest entity)
        {
            TravelResponseDTO response = new TravelResponseDTO
            {
                request_id = entity.RequestId,
                raised_by_employee_id = entity.RaisedByEmployeeId,
                to_be_approved_by_hr_id = entity.ToBeApprovedByHrId,
                request_raised_on = entity.RequestRaisedOn,
                from_date = entity.FromDate,
                to_date = entity.ToDate,
                purpose_of_travel = entity.PurposeOfTravel,
                location_id = entity.LocationId,
                request_status = entity.RequestStatus,
                RequestApprovedOn = entity.RequestApprovedOn,
                Priority = entity.Priority
            };
            return response;
        }
        public async Task<TravelResponseDTO> CreateTravelRequest(TravelRequestsReqDTO travelRequestDTO)
        {
            if (await hrClient.GetEmployeeById(travelRequestDTO.raised_by_employee_id.GetValueOrDefault()) == null)
                throw new Exception("This EmployeeId doesn't exist");
            var user = await hrClient.GetEmployeeById(travelRequestDTO.to_be_approved_by_hr_id.GetValueOrDefault());
            if (user.role != "HR")
                throw new Exception("No HR exist with this Id");
       
            var entity = RequestDtoToEntity(travelRequestDTO);

            // save entities in Db and used the same result in reponse back
            var result = await travelRequestRepo.CreateTravelRequest(entity);

            return EntityToResponseDto(result);
            
        }

        public async Task<IEnumerable<TravelResponseDTO>> GetAllPendingRequests(int HRid)
        {
            var user = await hrClient.GetEmployeeById(HRid);
            if (user == null)
                throw new Exception("Employee with this Id does not exist");

            if (user.role != "HR")
                throw new Exception("HR with this Id does not exisit");

            var result = await travelRequestRepo.GetAllPendingRequests(HRid);

            if (result.Count() == 0)
                throw new Exception("No Pending Request for this HR");


            List<TravelResponseDTO> pendingReqList = new List<TravelResponseDTO>();

            foreach(var item in result)
            { 
                // calling reusable response dto
                TravelResponseDTO response = EntityToResponseDto(item);
                pendingReqList.Add(response);
            }
            return pendingReqList;    
            
        }

        public async Task<TravelResponseDTO> getTravelRequestById(int trid)
        {
            var result = await travelRequestRepo.getTravelRequestById(trid);
            if(result == null)
                throw new Exception("No Travel Request with this Travel Request Id");
            TravelResponseDTO response = EntityToResponseDto(result);

            return response;

        }

        public async Task<TravelResponseDTO> getUpdateRequestStatus(int trid, UpdateRequestDTO updateDTO)
        {
            var result = await travelRequestRepo.getTravelRequestById(trid);

            if (result == null)
            {
                throw new KeyNotFoundException($"Travel request with ID {trid} was not found.");
            }

            // Only allow update if current status is NEW
            if (!string.Equals(result.RequestStatus, "New", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Request status must be NEW to proceed with an update.");
            }
            // updating
            result.RequestStatus = updateDTO.request_status;

            var getResult = await travelRequestRepo.getUpdateRequestStatus(result);
            // response back to user

            // Map via reusable DTO mapper
            var response = EntityToResponseDto(getResult);


            if (getResult.RequestStatus=="Approved")
            {
                int days = (int)(response.to_date - response.from_date).GetValueOrDefault().TotalDays;
                var travelBudgetAllocation = new TravelBudgetAllocation
                {
                    TravelRequestId = trid,
                    ApprovedBudget = await CalculateApprovedBudget(getResult.RaisedByEmployeeId.GetValueOrDefault(), getResult.Priority, (int)(getResult.ToDate - getResult.FromDate).Value.TotalDays),
                    ApprovedModeOfTravel = await CalculateModeOfTravelBudget(),
                    ApprovedHotelStarRating = await CalculateHotelBudget((int)getResult.RaisedByEmployeeId.GetValueOrDefault())
                };
            travelBudgetRepo.AddBudgetAllocation(travelBudgetAllocation);
            }
            
            return response;

        }

        public async Task<int> CalculateBudget(int travelRequestId)
        {
            TravelRequest travelRequest = await travelRequestRepo.getTravelRequestById(travelRequestId);

            if (travelRequest == null)
                throw new Exception("Travel Request Id not exist");

            if (travelRequest.RequestStatus != "Approved")
                throw new Exception("Travel Request is not approved so can not calculate the budget for it");
            int budget = await CalculateApprovedBudget(travelRequest.RaisedByEmployeeId.GetValueOrDefault(), travelRequest.Priority, (int)(travelRequest.ToDate - travelRequest.FromDate).Value.TotalDays);

            return budget;

        }

        public async Task<string> CalculateHotelBudget(int requestRaisedById)
        {
            UserResponseDTO user = await hrClient.GetEmployeeById(requestRaisedById);
            string[] HrHotel = { "7-Star", "5-Star" };
            string[] OthersHotel = { "5-Star", "3-Star" };

            Random rand = new();
            string selectedHotel = "";
            if(user.role == "HR")
            {
                selectedHotel = HrHotel[rand.Next(HrHotel.Length)];
            }
            else
            {
                selectedHotel = OthersHotel[rand.Next(OthersHotel.Length)];
            }
            return selectedHotel;
        }

        public async Task<string> CalculateModeOfTravelBudget()
        {
            Random rand = new();
            string[] ModeOfTravel = { "Air", "Train", "Bus" };
            string mode = ModeOfTravel[rand.Next(ModeOfTravel.Length)];
            return await Task.FromResult(mode);
        }

        public async Task<int> CalculateApprovedBudget(int id, string priority, int days)
        {
            UserResponseDTO user = await hrClient.GetEmployeeById(id);
            string userGradeId = user.current_grade_id;
            int finalBudget;
            int maxBudgetByGrade = 0;
            int maxDaysByPriority = 0;

            if(userGradeId == "Grade-1")
            {
                maxBudgetByGrade = 15000;
            }
            else if(userGradeId == "Grade-2")
            {
                maxBudgetByGrade = 12500;
            }
            else if(userGradeId == "Grade-3")
            {
                maxBudgetByGrade = 10000;
            }
            if(priority == "One")
            {
                maxDaysByPriority = 30;
            }
            else if(priority == "Two")
            {
                maxDaysByPriority = 20;
            }
            else if(priority == "Three")
            {
                maxDaysByPriority = 10;
            }
            else
            {
                throw new Exception("Invalid priority");
            }
            if (days > maxDaysByPriority)
                throw new Exception("Can't go for this long");
            finalBudget = days * maxBudgetByGrade;
            return finalBudget;
        }
    }
}
