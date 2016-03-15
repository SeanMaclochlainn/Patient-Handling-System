using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientHandlingSystem.ViewModels
{
    public class CompleteEquipmentAttribute
    {
        [DisplayName("Attribute Name")]
        public EquipmentAttribute EquipmentAttribute { get; set; }
        public List<EquipmentAttributeValue> EquipmentAttributeValues { get; set; }
        public List<SelectListItem> EquipmentAttributeValuesSelectList { get; set; } //this is only used in the Equipment/Edit view
    }
}