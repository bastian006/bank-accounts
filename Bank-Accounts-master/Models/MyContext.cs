using bank_accounts.Models;
using Microsoft.EntityFrameworkCore;
namespace bank_accounts
{
    public class MyContext : DbContext

    {
    // base() calls the parent class' constructor passing the "options" parameter along
    public MyContext(DbContextOptions options) : base(options) { }

    // "users" table is represented by this DbSet "Users"
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    }
}