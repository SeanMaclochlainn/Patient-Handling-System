using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class EquipmentViewModel
    {
        public Equipment Equipment { get; set; }
        public List<CompleteEquipmentAttribute> CompleteEquipmentAttributes { get; set; }
    }
}