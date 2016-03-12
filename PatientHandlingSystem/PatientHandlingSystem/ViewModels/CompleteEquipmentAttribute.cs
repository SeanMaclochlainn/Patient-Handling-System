using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class CompleteEquipmentAttribute
    {
        public EquipmentAttribute EquipmentAttribute { get; set; }
        public List<EquipmentAttributeValue> EquipmentAttributeValues { get; set; }
    }
}