using PatientHandlingSystem.Models;
using PatientHandlingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

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

                foreach (var equipmentAttributeValue in completeEquipmentAttribute.EquipmentAttributeValues)
                {
                    var thisEquipmentAttributeValue = equipmentAttributeValue;
                    db.Entry(thisEquipmentAttributeValue).State = EntityState.Modified;
                }
            }
            db.SaveChanges();
        }

        public void DeleteEquipmentItem(int equipmentId)
        {
            var equipmentItem = db.Equipment.Find(equipmentId);
            db.Equipment.Remove(equipmentItem);
        }

        public EquipmentViewModel GetEquipmentViewModel(int equipmentId)
        {
            var completeEquipmentAttributes = new List<CompleteEquipmentAttribute>();
            foreach(var equipmentAttribute in db.EquipmentAttributes.Where(i => i.EquipmentID == equipmentId).ToList())
            {
                var completeEquipmentAttribute = new CompleteEquipmentAttribute { EquipmentAttribute = equipmentAttribute, EquipmentAttributeValues = equipmentAttribute.EquipmentAttributeValues };
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