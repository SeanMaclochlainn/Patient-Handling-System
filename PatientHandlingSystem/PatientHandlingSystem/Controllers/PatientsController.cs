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
            foreach (var i in db.Attributes.ToList())
            {
                var patientAttribute = db.PatientsAttributes.SingleOrDefault(j => j.AttributeID == i.ID && j.PatientID == patient.ID);
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

            var patientsAttributesList = db.PatientsAttributes.Where(i => i.PatientID == patient.ID).ToList();
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
            foreach(var a in db.Attributes)
            {
                var attributeValues = a.AttributeValues;

                if(!a.Numeric)
                    attributeValues.Add(new AttributeValue { ID = 0, Value = "Select Attribute" });

                var completeAttribute = new CompleteAttribute
                {
                    Attribute = a,
                    AttributeValues = attributeValues,
                    SelectedAttributeValue = new AttributeValue()
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
                        var patientsAttributesList = new List<PatientAttribute>();
                        foreach (var ca in patientVM.CompleteAttributes)
                        {
                            PatientAttribute patientAttribute = null;

                            if (ca.Attribute.Numeric)
                            {
                                //for numeric attributes, the attribute value assigned to the patient is contained in the AttributeValue table, and then also referenced
                                //in the PatientsAttributes table
                                var attributeValue = new AttributeValue { AttributeID = ca.Attribute.ID, Value = ca.SelectedAttributeValue.Value };
                                db.AttributeValues.Add(attributeValue);
                                db.SaveChanges();

                                patientAttribute = new PatientAttribute
                                {
                                    AttributeID = ca.Attribute.ID,
                                    AttributeValueID = attributeValue.ID,
                                    PatientID = patient.ID
                                };
                            }
                            else
                            {
                                patientAttribute = new PatientAttribute
                                {
                                    AttributeID = ca.Attribute.ID,
                                    AttributeValueID = ca.SelectedAttributeValue.ID,
                                    PatientID = patient.ID
                                };
                            }

                            patientsAttributesList.Add(patientAttribute);
                        }

                        if (ModelState.IsValid)
                        {
                            foreach (var pa in patientsAttributesList)
                            {
                                db.PatientsAttributes.Add(pa);
                            }
                            //db.PatientsAttributes.AddRange(patientsAttributesList);
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
            var patientsAttributes = db.PatientsAttributes.Where(i => i.PatientID == patient.ID).ToList();
            var attributes = db.Attributes.ToList();
            foreach(var a in attributes)
            {
                var attributeValues = a.AttributeValues;
                var selectedAttribute = db.PatientsAttributes.SingleOrDefault(i => i.AttributeID == a.ID && i.PatientID == patient.ID);
                AttributeValue selectedAttributeValue = null;
                if (selectedAttribute != null)
                {
                    selectedAttributeValue = selectedAttribute.AttributeValue;
                }
                else
                {
                    selectedAttributeValue = new AttributeValue { ID = 0, Value = "Please Select Attribute", AttributeID = a.ID };
                    attributeValues.Add(new AttributeValue { Value = "Please Select Attribute", ID = 0, AttributeID = a.ID });
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
                var patientAttribute = db.PatientsAttributes.Include(j=>j.Attribute).SingleOrDefault(i => i.PatientID == patient.ID && i.AttributeID == ca.SelectedAttributeValue.AttributeID);

                //this is null if a new attribute has been added recently, and the patient hasn't been assigned an attribute value for this attribute
                if (patientAttribute == null)
                {
                    patientAttribute = new PatientAttribute { AttributeID = ca.SelectedAttributeValue.AttributeID, AttributeValueID = ca.SelectedAttributeValue.ID, PatientID = patient.ID };
                    db.PatientsAttributes.Add(patientAttribute);
                    db.SaveChanges();
                }
                else
                {
                    if(patientAttribute.Attribute.Numeric)
                    {
                        patientAttribute.AttributeValue.Value = ca.SelectedAttributeValue.Value;
                        db.Entry(patientAttribute).State = EntityState.Modified;
                    }
                    else
                    {
                        patientAttribute.AttributeValueID = ca.SelectedAttributeValue.ID;
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
