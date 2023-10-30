using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bank_accounts.Models
{
  public class Transaction
  {
    [Key]
    public int TransactionId { get; set; }
		[Required]
		[Display (Name = "Deposit/Withdraw")]
    [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:0.###}")]
    public decimal Amount { get; set; }

		public int UserId { get; set; }
		public User Owner { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
  }
}