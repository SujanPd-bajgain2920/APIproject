using System;
using System.Collections.Generic;

namespace APIproject.Domain.Entities.Models;

public partial class AuctionBid
{
    public short BidId { get; set; }

    public short AuctionBidId { get; set; }

    public short BidderId { get; set; }

    public decimal BidAmount { get; set; }

    public DateOnly BidDate { get; set; }

    public TimeOnly BidTime { get; set; }

    public string BidStatus { get; set; }

    public virtual AuctionDetail AuctionBidNavigation { get; set; }

    public virtual UserList Bidder { get; set; }

    public virtual ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
