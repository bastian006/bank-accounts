using Microsoft.EntityFrameworkCore;
using bank_accounts.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace bank_accounts.Controllers {
  public class BankAccountController : Controller
  {
    private MyContext dbContext;
    // here we can "inject" our context service into the constructor
    public BankAccountController(MyContext context)
    {
      dbContext = context;
    }

    [HttpGet("")]
    public IActionResult RegisterView()
    {
      return View();
    }

    [HttpPost("Register")]
    public IActionResult Register(UserRegistration newUser)
    {
      if (ModelState.IsValid)
      {
        if (dbContext.Users.Any(u => u.Email == newUser.Email))
        {
          ModelState.AddModelError("Email", "Email already in use!");
          return View("RegisterView");
        }
        else
        {
          PasswordHasher<UserRegistration> Hasher = new PasswordHasher<UserRegistration>();
          newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
          User NewUser = new User
          {
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Password = newUser.Password,
          };
          dbContext.Users.Add(NewUser);
          dbContext.SaveChanges();

          int uid = NewUser.UserId;
          HttpContext.Session.SetInt32("uid", uid);

          return RedirectToAction("Account");
        }
      }
      else
      {
        return View("RegisterView");
      }
    }

    [HttpGet("login")]
    public IActionResult LoginView()
    {
      return View();
    }
    [HttpPost("loginuser")]
    public IActionResult Login(UserLogin currentUser)
    {
      if (ModelState.IsValid) {
      User userInDb = dbContext.Users.FirstOrDefault(u => u.Email == currentUser.LoginEmail);
      if (userInDb == null)
      {
        // Add an error to ModelState and return to View!
        ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
        return View("LoginView");
      }
      // Initialize hasher object
      var hasher = new PasswordHasher<UserLogin>();

      // verify provided password against hash stored in db
      var result = hasher.VerifyHashedPassword(currentUser, userInDb.Password, currentUser.LoginPassword);

      // result can be compared to 0 for failure
      if (result == 0)
      {
        // handle failure (this should be similar to how "existing email" is handled)
        ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
        return View("LoginView");
      }
      int uid = userInDb.UserId;
      HttpContext.Session.SetInt32("uid", uid);

      return RedirectToAction("Account");

      }
      else
      {
        return View("LoginView");
      }
    }

    [HttpGet("account")]
    public IActionResult Account()
    {
      int? uid = HttpContext.Session.GetInt32("uid");
      if (uid == null)
      {
        return RedirectToAction("RegisterView");
      }
      else
      {
        User retrivedUser = dbContext.Users.FirstOrDefault(u => u.UserId == uid);
        ViewBag.retrivedUser = retrivedUser;
        List<Transaction> transactionsWithUser = dbContext.Transactions
        .Where(transaction => transaction.UserId == uid)
        .OrderByDescending(t => t.CreatedAt)
        .ToList();

        decimal sum = dbContext.Transactions
        .Where(transaction => transaction.UserId == uid)
        .Sum(t => t.Amount);
        ViewBag.Total = sum;
        return View(new Account {allTrans = transactionsWithUser});
      }
    }
    [HttpPost("account/new")]
    public IActionResult NewTransaction(Account NewTransaction)
    {
      if (ModelState.IsValid)
      {
      int? uid = HttpContext.Session.GetInt32("uid");
      User retrivedUser = dbContext.Users.FirstOrDefault(u => u.UserId == uid);

      NewTransaction.userTrans.UserId = (int)uid;
      NewTransaction.userTrans.Owner = retrivedUser;

      dbContext.Transactions.Add(NewTransaction.userTrans);
      dbContext.SaveChanges();
      return RedirectToAction("Account");
      }
      else {
        return View("Account");
      }
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
      HttpContext.Session.Clear();
      return RedirectToAction("RegisterView");
    }
  }
}
