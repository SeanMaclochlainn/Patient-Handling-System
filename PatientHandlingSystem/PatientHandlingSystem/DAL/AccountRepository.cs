using PatientHandlingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace PatientHandlingSystem.DAL
{
    public class AccountRepository
    {
        private PatientHandlingContext db;

        public AccountRepository(PatientHandlingContext context)
        {
            db = context;
        }

        public UserVM GetRegisterViewModel()
        {
            var roleProvider = new SimpleRoleProvider(Roles.Provider);
            return new UserVM { Roles = roleProvider.GetAllRoles().ToList() };
        }

        public void RegisterUser(string emailAddress, string password, string firstName, string lastName, string userType)
        {
            WebSecurity.CreateUserAndAccount(emailAddress, emailAddress, new { FirstName = emailAddress, LastName = lastName });
            var roleProvider = new SimpleRoleProvider(Roles.Provider);
            roleProvider.AddUsersToRoles(new string[] { emailAddress }, new string[] { userType });
        }

        public List<UserVM> GetUsersViewModel()
        {
            var userVMs = new List<UserVM>();
            var roleProvider = new SimpleRoleProvider(Roles.Provider);
            foreach (var user in db.Users.ToList())
            {
                string[] roles = roleProvider.GetRolesForUser(user.EmailAddress);
                var userVM = new UserVM { FirstName = user.FirstName, LastName = user.LastName, EmailAddress = user.EmailAddress, SelectedRole = roles[0] };
                userVMs.Add(userVM);
            }
            return userVMs;
        }
    }
}