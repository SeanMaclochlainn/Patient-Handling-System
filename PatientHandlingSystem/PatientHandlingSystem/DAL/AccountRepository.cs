using PatientHandlingSystem.Models;
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

        public RegisterVM GetRegisterViewModel()
        {
            var roleProvider = new SimpleRoleProvider(Roles.Provider);
            var userVM = new RegisterVM();
            userVM.Roles = roleProvider.GetAllRoles().ToList();
            return userVM;
        }

        public void RegisterUser(string emailAddress, string password, string firstName, string lastName, string userType)
        {
            WebSecurity.CreateUserAndAccount(emailAddress, password, new { FirstName = firstName, LastName = lastName });
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
                var userVM = new UserVM { FirstName = user.FirstName, LastName = user.LastName, EmailAddress = user.EmailAddress, SelectedRole = roles[0], ID = user.ID };
                userVMs.Add(userVM);
            }
            return userVMs;
        }

        public User GetUser(int userId)
        {
            return db.Users.Find(userId);
        }

        public void DeletUser(int userId)
        {
            var user = db.Users.Find(userId);
            
            var roleProvider = new SimpleRoleProvider(Roles.Provider);
            roleProvider.RemoveUsersFromRoles(new[] { user.EmailAddress }, roleProvider.GetRolesForUser(user.EmailAddress));
            ((SimpleMembershipProvider)Membership.Provider).DeleteAccount(user.EmailAddress); // deletes record from webpages_Membership table

            Membership.DeleteUser(user.EmailAddress, true);
            
        }
    }
}