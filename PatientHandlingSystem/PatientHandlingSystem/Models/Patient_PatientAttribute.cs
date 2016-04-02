using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class Patient_PatientAttribute
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int PatientAttributeID { get; set; }
        public int PatientAttributeValueID { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual PatientAttribute PatientAttribute { get; set; }
        public virtual PatientAttributeValue PatientAttributeValue { get; set; }
    }
}