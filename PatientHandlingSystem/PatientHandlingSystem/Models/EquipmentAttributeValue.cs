using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class EquipmentAttributeValue
    {
        public int ID { get; set; }
        public int EquipmentAttributeID { get; set; }

        [DisplayName("Attribute Value")]
        public string Name { get; set; }

        public virtual EquipmentAttribute EquipmentAttribute { get; set; }
    }
}