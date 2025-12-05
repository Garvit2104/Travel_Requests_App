using Microsoft.AspNetCore.Components.Forms;
using Travel_Requests_App.Data;
using Travel_Requests_App.Models;

namespace Travel_Requests_App.DAL.TravelBudgets
{
    public class TravelBudgetRepository : ITravelBudgetRepo
    {
        private readonly TravelPlannerDbContext context;

        public TravelBudgetRepository(TravelPlannerDbContext context)
        {
            this.context = context;
        }

        public void AddBudgetAllocation(TravelBudgetAllocation travelBudgetAllocation)
        {
            context.TravelBudgetAllocations.Add(travelBudgetAllocation);
            context.SaveChanges();
        }

    }
}
