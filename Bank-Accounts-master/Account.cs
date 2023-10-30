using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bank_accounts.Models
{
  public class Account
  {
    public Transaction userTrans { get; set; }
    public List<Transaction> allTrans { get; set; }
  }
}