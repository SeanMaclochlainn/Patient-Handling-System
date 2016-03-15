using PatientHandlingSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class EquipmentAttribute
    {
        public int ID { get; set; }
        public int EquipmentID { get; set; }

        [DisplayName("Attribute Name")]
        public string Name { get; set; }
        public int CurrentEquipmentAttributeValueID { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual List<EquipmentAttributeValue> EquipmentAttributeValues { get; set; }

        PatientHandlingContext db = new PatientHandlingContext();

        public EquipmentAttributeValue CurrentEquipmentAttribute
        {
            get { return db.EquipmentAttributeValues.Single(i => i.ID == CurrentEquipmentAttributeValueID); }
        }

    }

}