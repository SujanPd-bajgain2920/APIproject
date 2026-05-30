using System;
using System.Collections.Generic;

namespace APIproject.Domain.Entities.Models;

public partial class TenderApplication
{
    public short ApplicationId { get; set; }

    public short TenderAppllyId { get; set; }

    public short CompanyApplyId { get; set; }

    public decimal ProposedBudget { get; set; }

    public string ProposedDuration { get; set; }

    public string ApplicationDocument { get; set; }

    public string ApplicationStatus { get; set; }

    public virtual Company CompanyApply { get; set; }

    public virtual TenderDetail TenderApplly { get; set; }
}
