﻿using PatientHandlingSystem.Models;
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
        public virtual DbSet<Models.Attribute> Attributes { get; set; }
        public virtual DbSet<PatientAttribute> PatientsAttributes { get; set; }
        public virtual DbSet<AttributeValue> AttributeValues { get; set; }
        public virtual DbSet<Node> Nodes { get; set; }
        public virtual DbSet<Tree> Trees { get; set; }
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<Equipment> Equipment { get; set; }

        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<PatientHandlingContext>(null);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}