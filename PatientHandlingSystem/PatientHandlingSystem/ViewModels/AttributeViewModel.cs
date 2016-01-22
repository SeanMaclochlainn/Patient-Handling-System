using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class AttributeViewModel
    {
        public string AttributeName { get; set; }
        public List<string> AttributeValues { get; set; }
    }
}