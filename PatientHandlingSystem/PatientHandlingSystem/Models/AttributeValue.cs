using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class AttributeValue
    {
        public int ID { get; set; }
        public int AttributeID { get; set; }

        [DisplayName("Attribute Value")]
        public string Name { get; set; }

        public virtual Attribute Attribute { get; set; }
    }
}