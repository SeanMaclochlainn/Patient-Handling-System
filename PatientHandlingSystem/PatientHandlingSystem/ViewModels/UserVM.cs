using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class UserVM
    {
        public int ID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("E-mail Address")]
        public string EmailAddress { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<string> Roles { get; set; } //to store role drop down elements

        [DisplayName("Role")]
        public string SelectedRole { get; set; } // to store selected role from drop down
    }
}