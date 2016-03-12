using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class EquipmentAttributeValue
    {
        public int ID { get; set; }
        public int EquipmentAttributeID { get; set; }
        public string Name { get; set; }
    }
}