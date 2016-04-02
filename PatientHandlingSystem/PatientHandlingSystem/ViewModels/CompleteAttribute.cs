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
        public Models.PatientAttribute Attribute { get; set; }
        public List<PatientAttributeValue> AttributeValues { get; set; }
        public PatientAttributeValue SelectedAttributeValue { get; set; }
    }
}