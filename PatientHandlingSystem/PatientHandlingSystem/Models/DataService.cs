using PatientHandlingSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class DataService
    {
        private PatientHandlingContext db;

        public DataService(PatientHandlingContext context)
        {
            db = context;
        }

        public List<PatientAttribute> getAllRelevantPatientsAttributes(int patientId)//gets all the patientattributes for this patient, for every attribute possible
        {
            return db.PatientsAttributes.Where(i => i.PatientID == patientId).ToList();
        }

        public List<AttributeValue> getAllPatientAttributeValues(int patientId)
        {
            var attributeValues = new List<AttributeValue>();
            var patientattrs = db.PatientsAttributes;
            //some weird bug in the database mocking is forcing me to us a for loop instead of a foreach loop here
            for (int i=0; i<db.PatientsAttributes.Count();i++)
            {
                var attrvals = db.AttributeValues.ToList();
                var attributeValue = db.AttributeValues.Single(j => j.ID == db.PatientsAttributes.ElementAt(i).AttributeValueID);
                attributeValues.Add(attributeValue);
            }
            return attributeValues;
        }
    }
}