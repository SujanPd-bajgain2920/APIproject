using System;
using System.Collections.Generic;

namespace APIproject.Models;

public partial class Payment
{
    public short PaymentId { get; set; }

    public short? PayTenderId { get; set; }

    public short? PayCompanyId { get; set; }

    public short? PayAuctionId { get; set; }

    public short PayToUser { get; set; }

    public short PayByUser { get; set; }

    public decimal PaymentAmount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentMethod { get; set; }

    public string SlipUpload { get; set; }

    public string PaymentStatus { get; set; }

    public virtual AuctionDetail PayAuction { get; set; }

    public virtual UserList PayByUserNavigation { get; set; }

    public virtual Company PayCompany { get; set; }

    public virtual TenderDetail PayTender { get; set; }

    public virtual UserList PayToUserNavigation { get; set; }
}
