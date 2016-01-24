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
        public Models.Attribute Attribute { get; set; }
        public List<AttributeValue> AttributeValues { get; set; }
        public AttributeValue SelectedAttributeValue { get; set; }
    }
}