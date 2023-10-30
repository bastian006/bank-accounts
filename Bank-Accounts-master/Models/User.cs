using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bank_accounts.Models
{
    public class User
    {
    [Key]
    public int UserId { get; set; }

    [Required]
    [MinLength(2)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("ConfirmPassword")]
    [MinLength(8)]
    public string Password { get; set; }

    [NotMapped]
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }

		public List<Transaction> UserTransaction { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}