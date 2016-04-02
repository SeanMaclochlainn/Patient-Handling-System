using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class PatientAttributeValue
    {
        public int ID { get; set; }
        public int PatientAttributeID { get; set; }

        [DisplayName("Attribute Value")]
        public string Value { get; set; }

        public virtual PatientAttribute PatientAttribute { get; set; }
    }
}