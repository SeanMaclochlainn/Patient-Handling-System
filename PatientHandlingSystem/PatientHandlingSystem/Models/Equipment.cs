using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Boolean Available { get; set; }
    }
}