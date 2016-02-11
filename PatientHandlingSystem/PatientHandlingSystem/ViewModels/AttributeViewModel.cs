using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class AttributeViewModel
    {
        [DisplayName("Attribute Name")]
        public string AttributeName { get; set; }

        [DisplayName("Numeric Attribute")]
        public Boolean Numeric { get; set; }

        [DisplayName("Attribute Value")]
        public List<string> AttributeValues { get; set; }
    }
}