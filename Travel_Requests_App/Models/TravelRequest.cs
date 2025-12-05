using System;
using System.Collections.Generic;

namespace Travel_Requests_App.Models;

public partial class TravelRequest
{
    public int RequestId { get; set; }

    public int? RaisedByEmployeeId { get; set; }

    public int? ToBeApprovedByHrId { get; set; }

    public DateTime? RequestRaisedOn { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? PurposeOfTravel { get; set; }

    public int? LocationId { get; set; }

    public string? RequestStatus { get; set; }

    public DateTime? RequestApprovedOn { get; set; }

    public string? Priority { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<TravelBudgetAllocation> TravelBudgetAllocations { get; set; } = new List<TravelBudgetAllocation>();
}
