using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PatientHandlingSystem.DAL;
using PatientHandlingSystem.Models;
using PatientHandlingSystem.ViewModels;

namespace PatientHandlingSystem.Controllers
{
    public class PatientsController : Controller
    {
        private PatientHandlingContext db = new PatientHandlingContext();

        // GET: Patients
        public ActionResult Index()
        {
            return View(db.Patients.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            var completeAttributes = new List<CompleteAttribute>();
            foreach (var i in db.Attributes.ToList())
            {
                var patientAttribute = db.PatientAttributes.SingleOrDefault(j => j.AttributeID == i.ID && j.PatientID == patient.ID);
                AttributeValue selectedAttributeValue = null;
                if (patientAttribute != null)
                    selectedAttributeValue = db.AttributeValues.Find(patientAttribute.AttributeValueID);
                else
                    selectedAttributeValue = new AttributeValue();
                var completeAttribute = new CompleteAttribute
                {
                    Attribute = i,
                    AttributeValues = i.AttributeValues,
                    SelectedAttributeValue = selectedAttributeValue
                };

                completeAttributes.Add(completeAttribute);
            }

            var patientAttributes = db.PatientAttributes.Where(i => i.PatientID == patient.ID).ToList();
            var patientVM = new PatientViewModel
            {
                Patient = patient,
                CompleteAttributes = completeAttributes
            };
            return View(patientVM);
        }

        public ActionResult Create()
        {
            var completeAttributes = new List<CompleteAttribute>();
            foreach(var a in db.Attributes)
            {
                var attributeValues = a.AttributeValues;
                attributeValues.Add(new AttributeValue { ID = 0, Name = "Select Attribute" });
                var completeAttribute = new CompleteAttribute
                {
                    Attribute = a,
                    AttributeValues = attributeValues,
                    SelectedAttributeValue = new AttributeValue()
                };
                completeAttributes.Add(completeAttribute);
            }

            var patientVM = new PatientViewModel
            {
                Patient = new Patient(),
                CompleteAttributes = completeAttributes
            };
            return View(patientVM);
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PatientViewModel patientVM)
        {
            var patient = new Patient
            {
                FirstName = patientVM.Patient.FirstName,
                LastName = patientVM.Patient.LastName
            };

            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
            }

            var patientAttributes = new List<PatientAttribute>();
            foreach(var a in patientVM.CompleteAttributes)
            {
                var patientAttribute = new PatientAttribute
                {
                    AttributeID = a.Attribute.ID,
                    AttributeValueID = a.SelectedAttributeValue.ID,
                    PatientID = patient.ID
                };
                patientAttributes.Add(patientAttribute);
            }
            if (ModelState.IsValid)
            {
                db.PatientAttributes.AddRange(patientAttributes);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            var completeAttributes = new List<CompleteAttribute>();
            var patientAttributes = db.PatientAttributes.Where(i => i.PatientID == patient.ID).ToList();
            var attributes = db.Attributes.ToList();
            foreach(var a in attributes)
            {
                var attributeValues = a.AttributeValues;
                var selectedAttribute = db.PatientAttributes.SingleOrDefault(i => i.AttributeID == a.ID && i.PatientID == patient.ID);
                AttributeValue selectedAttributeValue = null;
                if (selectedAttribute != null)
                {
                    selectedAttributeValue = selectedAttribute.AttributeValue;
                }
                else
                {
                    selectedAttributeValue = new AttributeValue { ID = 0, Name = "Please Select Attribute", AttributeID = a.ID };
                    attributeValues.Add(new AttributeValue { Name = "Please Select Attribute", ID = 0, AttributeID = a.ID });
                }
                var completeAttribute = new CompleteAttribute
                {
                    Attribute = a,
                    AttributeValues = attributeValues,
                    SelectedAttributeValue = selectedAttributeValue
                };
                completeAttributes.Add(completeAttribute);
            }

            var patientVM = new PatientViewModel
            {
                Patient = patient,
                CompleteAttributes = completeAttributes
            };
            return View(patientVM);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PatientViewModel patientVM)
        {
            var patient = db.Patients.Find(patientVM.Patient.ID);
            patient.FirstName = patientVM.Patient.FirstName;
            patient.LastName = patientVM.Patient.LastName;
            foreach(var ca in patientVM.CompleteAttributes)
            {
                var patientAttribute = db.PatientAttributes.SingleOrDefault(i => i.PatientID == patient.ID && i.AttributeID == ca.SelectedAttributeValue.AttributeID);

                //this is null if a new attribute has been added recently
                if (patientAttribute == null)
                {
                    patientAttribute = new PatientAttribute { AttributeID = ca.SelectedAttributeValue.AttributeID, AttributeValueID = ca.SelectedAttributeValue.ID, PatientID = patient.ID };
                    db.PatientAttributes.Add(patientAttribute);
                    db.SaveChanges();
                }
                else
                {
                    patientAttribute.AttributeValueID = ca.SelectedAttributeValue.ID;
                    db.Entry(patientAttribute).State = EntityState.Modified;
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patient patient = db.Patients.Find(id);
            db.Patients.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
