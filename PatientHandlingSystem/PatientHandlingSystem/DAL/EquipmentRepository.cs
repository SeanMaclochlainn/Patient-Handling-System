using PatientHandlingSystem.Models;
using PatientHandlingSystem.ViewModels;
using System;
using System.Collections.Generic;
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
            db.Equipment.Add(equipmentVM.Equipment);
        }

        public void DeleteEquipmentItem(int equipmentId)
        {
            var equipmentItem = db.Equipment.Find(equipmentId);
            db.Equipment.Remove(equipmentItem);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}