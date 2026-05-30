using System;
using System.Collections.Generic;

namespace APIproject.Models;

public partial class Term
{
    public short TermId { get; set; }

    public short ContractId { get; set; }

    public string TermDescription { get; set; }

    public DateTime CreatedDate { get; set; }

    public short CreatedBy { get; set; }

    public virtual ContractDetail Contract { get; set; }

    public virtual UserList CreatedByNavigation { get; set; }
}
