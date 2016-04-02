using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.DAL
{
    public class PatientHandlingContext : DbContext
    {
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<PatientAttribute> PatientAttributes { get; set; }
        public virtual DbSet<Patient_PatientAttribute> Patient_PatientAttributes { get; set; }
        public virtual DbSet<PatientAttributeValue> PatientAttributeValues { get; set; }
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Tree> Trees { get; set; }
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentAttribute> EquipmentAttributes { get; set; }
        public virtual DbSet<EquipmentAttributeValue> EquipmentAttributeValues { get; set; }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PatientHandlingContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}