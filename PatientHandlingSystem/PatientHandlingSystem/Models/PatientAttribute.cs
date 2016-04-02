using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class PatientAttribute
    {
        public int ID { get; set; }

        [DisplayName("Attribute Name")]
        public string Name { get; set; }
        public Boolean Numeric { get; set; }

        public virtual List<PatientAttributeValue> PatientAttributeValues { get; set; }
    }
}