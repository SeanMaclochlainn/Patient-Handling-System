using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class PatientAttribute
    {
        public int ID { get; set; }
        public int PatientID { get; set; }
        public int AttributeID { get; set; }
        public int AttributeValueID { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Attribute Attribute { get; set; }
        public virtual AttributeValue AttributeValue { get; set; }
    }
}