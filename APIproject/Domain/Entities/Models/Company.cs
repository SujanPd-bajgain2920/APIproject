using System;
using System.Collections.Generic;

namespace APIproject.Domain.Entities.Models;

public partial class Company
{
    public short CompanyId { get; set; }

    public string CompanyName { get; set; }

    public string FullAddress { get; set; }

    public string OfficeEmail { get; set; }

    public string CompanyWebsiteUrl { get; set; }

    public string RegistrationNumber { get; set; }

    public string RegistrationDocument { get; set; }

    public string PanNumber { get; set; }

    public string PanDocument { get; set; }

    public string CompanyType { get; set; }

    public string Position { get; set; }

    public decimal? Rating { get; set; }

    public short? UserbidId { get; set; }

    public bool IsVerified { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<TenderApplication> TenderApplications { get; set; } = new List<TenderApplication>();

    public virtual ICollection<TenderDetail> TenderDetails { get; set; } = new List<TenderDetail>();

    public virtual UserList Userbid { get; set; }
}
