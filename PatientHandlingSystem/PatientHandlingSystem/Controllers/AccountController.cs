using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PatientHandlingSystem.DAL;
using PatientHandlingSystem.Models;
using WebMatrix.WebData;
using PatientHandlingSystem.ViewModels;
using System.Web.Security;

namespace PatientHandlingSystem.Controllers
{
    public class AccountController : Controller
    {
        private PatientHandlingContext db;
        private AccountRepository accountRepository;

        public AccountController()
        {
            db = new PatientHandlingContext();
            accountRepository = new AccountRepository(db);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Register()
        {
            var registerVM = accountRepository.GetRegisterViewModel();
            return View(registerVM);
        }

        [HttpPost]
        public ActionResult Register(UserVM user)
        {
            accountRepository.RegisterUser(user.EmailAddress, user.Password, user.FirstName, user.LastName, user.SelectedRole);
            return RedirectToAction("Users", "Users");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserVM user)
        {
            if(WebSecurity.Login(user.EmailAddress, user.Password))
            {
                return RedirectToAction("Index", "Patients");
            }
            else
            {
                return View();
            }
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Users()
        {
            var usersVM = accountRepository.GetUsersViewModel();
            return View(usersVM);
        }

        public ActionResult EditUser(int id)
        {
            var user = accountRepository.GetUser(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Users");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
