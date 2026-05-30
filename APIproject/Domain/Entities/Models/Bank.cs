using System;
using System.Collections.Generic;

namespace APIproject.Domain.Entities.Models;

public partial class Bank
{
    public short BankId { get; set; }

    public string BankName { get; set; }

    public string AccountNumber { get; set; }

    public string AccountType { get; set; }

    public string AccountHolderName { get; set; }

    public short? UserbankId { get; set; }

    public bool IsVerified { get; set; }

    public virtual UserList Userbank { get; set; }
}
