using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class PatientViewModel
    {
        public Patient Patient { get; set; }
        public List<CompleteAttribute> CompleteAttributes { get; set; }
    }
}