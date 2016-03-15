using PatientHandlingSystem.Models;
using PatientHandlingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PatientHandlingSystem.DAL
{
    public class EquipmentRepository
    {
        private PatientHandlingContext db;

        public EquipmentRepository(PatientHandlingContext context)
        {
            db = context;
        }

        public void AddEquipmentItem(EquipmentViewModel equipmentVM)
        {
            //add the equipment item
            var equipmentItem = equipmentVM.Equipment;
            db.Equipment.Add(equipmentVM.Equipment);
            db.SaveChanges();

            //add the attributes for the equipment item
            foreach(var completeEquipmentAttribute in equipmentVM.CompleteEquipmentAttributes)
            {
                var equipmentAttribute = completeEquipmentAttribute.EquipmentAttribute;
                equipmentAttribute.EquipmentID = equipmentItem.ID;
                db.EquipmentAttributes.Add(equipmentAttribute);
                db.SaveChanges();

                //add the equipment attribute values for this attribute
                foreach(var equipmentAttributeValue in completeEquipmentAttribute.EquipmentAttributeValues)
                {
                    equipmentAttributeValue.EquipmentAttributeID = equipmentAttribute.ID;
                    db.EquipmentAttributeValues.Add(equipmentAttributeValue);
                }
            }
            db.SaveChanges();
        }

        public void UpdateEquipmentItem(EquipmentViewModel equipmentVM)
        {
            var equipmentItem = equipmentVM.Equipment;
            db.Entry(equipmentItem).State = EntityState.Modified;

            foreach(var completeEquipmentAttribute in equipmentVM.CompleteEquipmentAttributes)
            {
                var equipmentAttribute = completeEquipmentAttribute.EquipmentAttribute;
                db.Entry(equipmentAttribute).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public void DeleteEquipmentItem(int equipmentId)
        {
            var equipmentItem = db.Equipment.Find(equipmentId);
            db.Equipment.Remove(equipmentItem);
            var equipmentAttributes = db.EquipmentAttributes.Where(i => i.EquipmentID == equipmentItem.ID).ToList();
            db.EquipmentAttributes.RemoveRange(equipmentAttributes);
            var equipmentAttributesAttributeValues = new List<EquipmentAttributeValue>();
            foreach(var equipmentAttribute in equipmentAttributes)
            {
                equipmentAttributesAttributeValues.AddRange(db.EquipmentAttributeValues.Where(i => i.EquipmentAttributeID == equipmentAttribute.ID).ToList());
            }
            db.EquipmentAttributeValues.RemoveRange(equipmentAttributesAttributeValues);
        }

        public EquipmentViewModel GetEquipmentViewModel(int equipmentId)
        {
            var completeEquipmentAttributes = new List<CompleteEquipmentAttribute>();
            
            foreach(var equipmentAttribute in db.EquipmentAttributes.Where(i => i.EquipmentID == equipmentId).ToList())
            {
                //generate selectlist of equipment attribute values for each equipment attribute
                var equipmentAttributeValuesSelectList = new List<SelectListItem>();
                if(equipmentAttribute.CurrentEquipmentAttributeValueID == 0)
                {
                    equipmentAttributeValuesSelectList.Add(new SelectListItem { Text = "Please select...", Selected = true, Value = "" });
                }
                Boolean selected = false;
                foreach (var equipmentAttributeValue in equipmentAttribute.EquipmentAttributeValues)
                {
                    if (equipmentAttributeValue.EquipmentAttribute.CurrentEquipmentAttributeValueID == equipmentAttributeValue.ID)
                        selected = true;
                    else
                        selected = false;
                    equipmentAttributeValuesSelectList.Add(new SelectListItem { Text = equipmentAttributeValue.Name, Value = equipmentAttributeValue.ID.ToString(), Selected = selected });
                }

                var completeEquipmentAttribute = new CompleteEquipmentAttribute { EquipmentAttribute = equipmentAttribute, EquipmentAttributeValues = equipmentAttribute.EquipmentAttributeValues, EquipmentAttributeValuesSelectList = equipmentAttributeValuesSelectList };
                completeEquipmentAttributes.Add(completeEquipmentAttribute);
            }
            var equipmentVM = new EquipmentViewModel { Equipment = db.Equipment.Find(equipmentId), CompleteEquipmentAttributes = completeEquipmentAttributes };
            return equipmentVM;
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}