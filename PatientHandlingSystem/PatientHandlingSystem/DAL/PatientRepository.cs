using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PatientHandlingSystem.Models;

namespace PatientHandlingSystem.DAL
{
    public class PatientRepository : IPatientRepository
    {
        private PatientHandlingContext db;

        public PatientRepository(PatientHandlingContext context)
        {
            db = context;
        }

        public Patient GetPatient(int id)
        {
            return db.Patients.Find(id);
        }

    }
}