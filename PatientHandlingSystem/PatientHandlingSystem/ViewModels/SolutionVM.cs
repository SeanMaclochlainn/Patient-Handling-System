using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class SolutionVM
    {
        public Patient Patient { get; set; }
        public Solution Solution { get; set; }
    }
}