using System;
using System.Collections.Generic;

namespace APIproject.Domain.Entities.Models;

public partial class AuctionDetail
{
    public short AuctionId { get; set; }

    public string Title { get; set; }

    public string AuctionDescription { get; set; }

    public string ItemImage { get; set; }

    public decimal StartingPrice { get; set; }

    public DateOnly StartDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public DateOnly EndDate { get; set; }

    public string AuctionType { get; set; }

    public string AuctionStatus { get; set; }

    public short? BuyerId { get; set; }

    public decimal? WinningBidAmount { get; set; }

    public short PublishedByUserId { get; set; }

    public string IsVerified { get; set; }

    public string AwardStatus { get; set; }

    public virtual ICollection<AuctionBid> AuctionBids { get; set; } = new List<AuctionBid>();

    public virtual UserList Buyer { get; set; }

    public virtual ICollection<ContractDetail> ContractDetails { get; set; } = new List<ContractDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual UserList PublishedByUser { get; set; }
}
