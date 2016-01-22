using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class AttributeValue
    {
        public int ID { get; set; }
        public int AttributeID { get; set; }
        public string Name { get; set; }

        public virtual Attribute Attribute { get; set; }
    }
}