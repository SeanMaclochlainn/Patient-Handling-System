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
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Models.Attribute> Attributes { get; set; }
        public DbSet<PatientAttribute> PatientAttributes { get; set; }
        public DbSet<AttributeValue> AttributeValues { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}