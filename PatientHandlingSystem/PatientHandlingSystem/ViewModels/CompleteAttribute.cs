using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientHandlingSystem.ViewModels
{
    public class CompleteAttribute
    {
        public PatientAttribute PatientAttribute { get; set; }
        public List<PatientAttributeValue> PatientAttributeValues { get; set; }
        public PatientAttributeValue SelectedPatientAttributeValue { get; set; }
    }
}