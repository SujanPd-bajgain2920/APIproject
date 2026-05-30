using System;
using System.Collections.Generic;

namespace APIproject.Domain.Entities.Models;

public partial class Bid
{
    public short BiddingId { get; set; }

    public short AucBidId { get; set; }

    public decimal BiddingAmount { get; set; }

    public virtual AuctionBid AucBid { get; set; }
}
