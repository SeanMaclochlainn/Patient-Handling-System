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
        private PatientHandlingContext db;
        private TreeRepository treeRepository;
        private PatientRepository patientRepository;

        public PatientsController()
        {
            db = new PatientHandlingContext();
            treeRepository = new TreeRepository(db);
            patientRepository = new PatientRepository(db);
        }

        public PatientsController(PatientHandlingContext context)
        {
            db = context;
            treeRepository = new TreeRepository(db);
            patientRepository = new PatientRepository(db);
        }
        // GET: Patients
        public ActionResult Index()
        {
            List<PatientAndTreesViewModel> patientAndTreesVMs = new List<PatientAndTreesViewModel>();
            foreach(var patient in db.Patients.ToList())
            {
                var patientAndTreesVM = new PatientAndTreesViewModel
                {
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    ID = patient.ID,
                    Trees = db.Trees.ToList()
                };
                patientAndTreesVMs.Add(patientAndTreesVM);
            }
            return View(patientAndTreesVMs);
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
            foreach (var i in db.PatientAttributes.ToList())
            {
                var patientAttribute = db.Patient_PatientAttributes.SingleOrDefault(j => j.PatientAttributeID == i.ID && j.PatientID == patient.ID);
                PatientAttributeValue selectedAttributeValue = null;
                if (patientAttribute != null)
                    selectedAttributeValue = db.PatientAttributeValues.Find(patientAttribute.PatientAttributeValueID);
                else
                    selectedAttributeValue = new PatientAttributeValue();
                var completeAttribute = new CompleteAttribute
                {
                    PatientAttribute = i,
                    PatientAttributeValues = i.PatientAttributeValues,
                    SelectedPatientAttributeValue = selectedAttributeValue
                };

                completeAttributes.Add(completeAttribute);
            }

            var patient_PatientAttributesList = db.Patient_PatientAttributes.Where(i => i.PatientID == patient.ID).ToList();
            var patientVM = new PatientViewModel()
            {
                Patient = patient,
                CompleteAttributes = completeAttributes
            };
            return View(patientVM);
        }

        public ActionResult Create()
        {
            var completeAttributes = new List<CompleteAttribute>();
            foreach(var a in db.PatientAttributes)
            {
                var attributeValues = a.PatientAttributeValues;

                if(!a.Numeric)
                    attributeValues.Add(new PatientAttributeValue { ID = 0, Value = "Select Attribute" });

                var completeAttribute = new CompleteAttribute
                {
                    PatientAttribute = a,
                    PatientAttributeValues = attributeValues,
                    SelectedPatientAttributeValue = new PatientAttributeValue()
                };
                completeAttributes.Add(completeAttribute);
            }

            var patientVM = new PatientViewModel()
            {
                Patient = new Patient(),
                CompleteAttributes = completeAttributes
            };
            return View(patientVM);
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //this method is not unit tested because of the db.begintransaction part
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
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

            using (var db = this.db)
            {
                using (var transaction = db.Database.BeginTransaction()) //this allows you to have two savechanges() in one function
                {
                    try
                    {
                        var patient_PatientAttributesList = new List<Patient_PatientAttribute>();
                        foreach (var ca in patientVM.CompleteAttributes)
                        {
                            Patient_PatientAttribute patientAttribute = null;

                            if (ca.PatientAttribute.Numeric)
                            {
                                //for numeric attributes, the attribute value assigned to the patient is contained in the AttributeValue table, and then also referenced
                                //in the Patient_PatientAttributes table
                                var attributeValue = new PatientAttributeValue { PatientAttributeID = ca.PatientAttribute.ID, Value = ca.SelectedPatientAttributeValue.Value };
                                db.PatientAttributeValues.Add(attributeValue);
                                db.SaveChanges();

                                patientAttribute = new Patient_PatientAttribute
                                {
                                    PatientAttributeID = ca.PatientAttribute.ID,
                                    PatientAttributeValueID = attributeValue.ID,
                                    PatientID = patient.ID
                                };
                            }
                            else
                            {
                                patientAttribute = new Patient_PatientAttribute
                                {
                                    PatientAttributeID = ca.PatientAttribute.ID,
                                    PatientAttributeValueID = ca.SelectedPatientAttributeValue.ID,
                                    PatientID = patient.ID
                                };
                            }

                            patient_PatientAttributesList.Add(patientAttribute);
                        }

                        if (ModelState.IsValid)
                        {
                            foreach (var pa in patient_PatientAttributesList)
                            {
                                db.Patient_PatientAttributes.Add(pa);
                            }
                            //db.Patient_PatientAttributes.AddRange(patientsAttributesList);
                            db.SaveChanges();
                            transaction.Commit();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
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
            var patientsAttributes = db.Patient_PatientAttributes.Where(i => i.PatientID == patient.ID).ToList();
            var patientAttributes = db.PatientAttributes.ToList();
            foreach(var a in patientAttributes)
            {
                var attributeValues = a.PatientAttributeValues;
                var selectedAttribute = db.Patient_PatientAttributes.SingleOrDefault(i => i.PatientAttributeID == a.ID && i.PatientID == patient.ID);
                PatientAttributeValue selectedAttributeValue = null;
                if (selectedAttribute != null)
                {
                    selectedAttributeValue = selectedAttribute.PatientAttributeValue;
                }
                else
                {
                    selectedAttributeValue = new PatientAttributeValue { ID = 0, Value = "Please Select Attribute", PatientAttributeID = a.ID };
                    attributeValues.Add(new PatientAttributeValue { Value = "Please Select Attribute", ID = 0, PatientAttributeID = a.ID });
                }
                var completeAttribute = new CompleteAttribute
                {
                    PatientAttribute = a,
                    PatientAttributeValues = attributeValues,
                    SelectedPatientAttributeValue = selectedAttributeValue
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
                var patientAttribute = db.Patient_PatientAttributes.Include(j=>j.PatientAttribute).SingleOrDefault(i => i.PatientID == patient.ID && i.PatientAttributeID == ca.SelectedPatientAttributeValue.PatientAttributeID);

                //this is null if a new attribute has been added recently, and the patient hasn't been assigned an attribute value for this attribute
                if (patientAttribute == null)
                {
                    patientAttribute = new Patient_PatientAttribute { PatientAttributeID = ca.SelectedPatientAttributeValue.PatientAttributeID, PatientAttributeValueID = ca.SelectedPatientAttributeValue.ID, PatientID = patient.ID };
                    db.Patient_PatientAttributes.Add(patientAttribute);
                    db.SaveChanges();
                }
                else
                {
                    if(patientAttribute.PatientAttribute.Numeric)
                    {
                        patientAttribute.PatientAttributeValue.Value = ca.SelectedPatientAttributeValue.Value;
                        db.Entry(patientAttribute).State = EntityState.Modified;
                    }
                    else
                    {
                        patientAttribute.PatientAttributeValueID = ca.SelectedPatientAttributeValue.ID;
                        db.Entry(patientAttribute).State = EntityState.Modified;
                    }
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

        public ActionResult HandlingPlanSet(int patientId)
        {
            var patient = patientRepository.GetPatient(patientId);

            SolutionsVM solutionsVM = new SolutionsVM
            {
                Patient = patient,
                Solutions = new List<Solution>()
            };

            foreach(var tree in db.Trees.ToList())
            {
                var solution = treeRepository.GetHandlingPlan(patientId, tree.ID);
                solutionsVM.Solutions.Add(solution);
            }
            return View("SolutionSet",solutionsVM);
        }

        public ActionResult HandlingPlan(int patientId, int treeId)
        {
            var patient = patientRepository.GetPatient(patientId);

            List<Solution> solutions = new List<Solution>();
            solutions.Add(treeRepository.GetHandlingPlan(patientId, treeId));

            SolutionsVM solutionsVM = new SolutionsVM
            {
                Patient = patient,
                Solutions = solutions
            };
            return View("SolutionSet", solutionsVM);
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
