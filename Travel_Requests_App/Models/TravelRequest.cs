using System;
using System.Collections.Generic;

namespace Travel_Requests_App.Models;

public partial class TravelRequest
{
    public int RequestId { get; set; }

    public int? RaisedByEmployeeId { get; set; }

    public int? ToBeApprovedByHrId { get; set; }

    public DateOnly? RequestRaisedOn { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? PurposeOfTravel { get; set; }

    public int? LocationId { get; set; }

    public string? RequestStatus { get; set; }

    public DateOnly RequestApprovedOn { get; set; }

    public string? Priority { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<TravelBudgetAllocation> TravelBudgetAllocations { get; set; } = new List<TravelBudgetAllocation>();
}
