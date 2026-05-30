using System;
using System.Collections.Generic;

namespace APIproject.Models;

public partial class TenderDetail
{
    public short TenderId { get; set; }

    public string Title { get; set; }

    public string TenderDescription { get; set; }

    public decimal BudgetEstimation { get; set; }

    public string IssuedBy { get; set; }

    public DateOnly IssuedDate { get; set; }

    public DateOnly OpeningDate { get; set; }

    public DateOnly ClosingDate { get; set; }

    public string TenderType { get; set; }

    public string ProjectDuration { get; set; }

    public string TenderDocument { get; set; }

    public short? AwardCompanyId { get; set; }

    public DateOnly? AwardDate { get; set; }

    public short PublishedByUserId { get; set; }

    public string IsVerified { get; set; }

    public string TenderStatus { get; set; }

    public string AwardStatus { get; set; }

    public virtual Company AwardCompany { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual UserList PublishedByUser { get; set; }

    public virtual ICollection<TenderApplication> TenderApplications { get; set; } = new List<TenderApplication>();
}
