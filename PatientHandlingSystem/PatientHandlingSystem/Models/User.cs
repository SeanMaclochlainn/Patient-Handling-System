using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class User
    {
        public int ID { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("E-mail Address")]
        public string EmailAddress { get; set; }
    }
}