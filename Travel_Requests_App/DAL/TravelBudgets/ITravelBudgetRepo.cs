using Travel_Requests_App.Models;

namespace Travel_Requests_App.DAL.TravelBudgets
{
    public interface ITravelBudgetRepo
    {
        public void AddBudgetAllocation(TravelBudgetAllocation travelBudgetAllocation);
    }
}
