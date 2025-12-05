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

        public TravelResponseDTO CreateTravelRequest(TravelRequestsReqDTO travelRequestDTO)
        {
            TravelRequest entity = new TravelRequest();
            entity.RaisedByEmployeeId = travelRequestDTO.raised_by_employee_id;
            entity.ToBeApprovedByHrId = travelRequestDTO.to_be_approved_by_hr_id;
            entity.RequestRaisedOn = travelRequestDTO.request_raised_on;
            entity.FromDate = travelRequestDTO.from_date;
            entity.ToDate = travelRequestDTO.to_date;
            entity.PurposeOfTravel = travelRequestDTO.purpose_of_travel;
            entity.LocationId = travelRequestDTO.location_id;
            entity.RequestStatus = travelRequestDTO.request_status;
            entity.RequestApprovedOn = travelRequestDTO.RequestApprovedOn ?? DateTime.Now;
            entity.Priority = travelRequestDTO.Priority;

            var result = travelRequestRepo.CreateTravelRequest(entity);

            List<TravelResponseDTO> ls = new List<TravelResponseDTO>();


            TravelResponseDTO response = new TravelResponseDTO
            {
                request_id = result.RequestId,
                raised_by_employee_id = result.RaisedByEmployeeId,
                to_be_approved_by_hr_id = result.ToBeApprovedByHrId,
                request_raised_on = result.RequestRaisedOn,
                from_date = result.FromDate,
                to_date = result.ToDate,
                purpose_of_travel = result.PurposeOfTravel,
                location_id = result.LocationId,
                request_status = result.RequestStatus,
                RequestApprovedOn = result.RequestApprovedOn,
                Priority = result.Priority
            };
            return response;
        }

        public List<TravelResponseDTO> GetAllPendingRequests(int HRid)
        {
            var result = travelRequestRepo.GetAllPendingRequests(HRid);

            List<TravelResponseDTO> ls = new List<TravelResponseDTO>();

            foreach(var item in result)
            {
                TravelResponseDTO response = new TravelResponseDTO();

                response.request_id = item.RequestId;
                response.raised_by_employee_id = item.RaisedByEmployeeId;
                response.to_be_approved_by_hr_id = item.ToBeApprovedByHrId;
                response.request_raised_on = item.RequestRaisedOn;
                response.from_date = item.FromDate;
                response.to_date = item.ToDate;
                response.purpose_of_travel = item.PurposeOfTravel;
                response.location_id = item.LocationId;
                response.request_status = item.RequestStatus;
                response.RequestApprovedOn = item.RequestApprovedOn;
                response.Priority = item.Priority;

                ls.Add(response);

            }
            return ls;    
            
        }

        public TravelResponseDTO getTravelRequestById(int trid)
        {
            var result = travelRequestRepo.getTravelRequestById(trid);
            TravelResponseDTO response = new TravelResponseDTO();

            response.request_id = result.RequestId;
            response.raised_by_employee_id = result.RaisedByEmployeeId;
            response.to_be_approved_by_hr_id = result.ToBeApprovedByHrId;
            response.request_raised_on = result.RequestRaisedOn;
            response.from_date = result.FromDate;
            response.to_date = result.ToDate;
            response.purpose_of_travel = result.PurposeOfTravel;
            response.location_id = result.LocationId;
            response.request_status = result.RequestStatus;
            response.RequestApprovedOn = result.RequestApprovedOn;
            response.Priority = result.Priority;

            return response;

        }

        public TravelResponseDTO getUpdateRequestStatus(int trid, UpdateRequestDTO updateDTO)
        {
            var result = travelRequestRepo.getTravelRequestById(trid);

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

            var getResult = travelRequestRepo.getUpdateRequestStatus(result);
            // response back to user
            TravelResponseDTO response = new TravelResponseDTO();
           
            response.request_id = getResult.RequestId;
            response.raised_by_employee_id = getResult.RaisedByEmployeeId;
            response.to_be_approved_by_hr_id = getResult.ToBeApprovedByHrId;
            response.request_raised_on = getResult.RequestRaisedOn;
            response.from_date = getResult.FromDate;
            response.to_date = result.ToDate;
            response.purpose_of_travel = getResult.PurposeOfTravel;
            response.location_id = getResult.LocationId;
            response.request_status = getResult.RequestStatus;
            response.Priority = getResult.Priority;
            response.RequestApprovedOn = getResult.RequestApprovedOn;

            if(getResult.RequestStatus=="Approved")
            {
                int days = (int)(response.to_date - response.from_date).GetValueOrDefault().TotalDays;
                var travelBudgetAllocation = new TravelBudgetAllocation
                {
                    TravelRequestId = trid,
                    ApprovedBudget = CalculateApprovedBudget(getResult.RaisedByEmployeeId.GetValueOrDefault(), getResult.Priority, (int)(getResult.ToDate - getResult.FromDate).Value.TotalDays),
                    ApprovedModeOfTravel = CalculateModeOfTravelBudget(),
                    ApprovedHotelStarRating = CalculateHotelBudget((int)getResult.RaisedByEmployeeId.GetValueOrDefault())
                };
            travelBudgetRepo.AddBudgetAllocation(travelBudgetAllocation);
            }
            
            return response;

        }

        public int CalculateBudget(int travelRequestId)
        {
            TravelRequest travelRequest = travelRequestRepo.getTravelRequestById(travelRequestId);

            if (travelRequest == null)
                throw new Exception("Travel Request Id not exist");

            if (travelRequest.RequestStatus != "Approved")
                throw new Exception("Travel Request is not approved so can not calculate the budget for it");
            int budget = CalculateApprovedBudget(travelRequest.RaisedByEmployeeId.GetValueOrDefault(), travelRequest.Priority, (int)(travelRequest.ToDate - travelRequest.FromDate).Value.TotalDays);

            return budget;

        }

        public string CalculateHotelBudget(int requestRaisedById)
        {
            UserResponseDTO user = hrClient.GetEmployeeById(requestRaisedById).Result;
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

        public string CalculateModeOfTravelBudget()
        {
            Random rand = new();
            string[] ModeOfTravel = { "Air", "Train", "Bus" };
            string mode = ModeOfTravel[rand.Next(ModeOfTravel.Length)];
            return mode;
        }

        public int CalculateApprovedBudget(int id, string priority, int days)
        {
            UserResponseDTO user = hrClient.GetEmployeeById(id).Result;
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
