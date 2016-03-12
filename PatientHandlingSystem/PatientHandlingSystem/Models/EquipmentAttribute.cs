using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class EquipmentAttribute
    {
        public int ID { get; set; }
        public int EquipmentID { get; set; }
        public string Name { get; set; }
        public int CurrentEquipmentValueID { get; set; }
    }
}