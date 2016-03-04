﻿using System;
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

        public PatientsController()
        {
            db = new PatientHandlingContext();
        }

        public PatientsController(PatientHandlingContext context)
        {
            db = context;
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
                if (patientAttribute == null && !patientAttribute.Attribute.Numeric)
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

        public ActionResult HandlingPlan(int patientId, int treeId)
        {
            var patient = db.Patients.Single(i=>i.ID == patientId);
            var tree = db.Trees.Single(i=>i.ID == treeId);

            Node selectedNode = db.Nodes.Single(i => i.ParentID < 1 && i.TreeID == tree.ID); //the node with parentID of zero is the root node

            while (db.Nodes.Where(i => i.ParentID == selectedNode.ID).Count() > 0) //basically while the selected node has children
            {
                Boolean result = false;
                List<Node> childNodes = db.Nodes.Where(i => i.ParentID == selectedNode.ID).ToList();
                int j = 0;
                while (result != true)
                {
                    if (j >= childNodes.Count)
                        return View(new Solution { Content = "Error"}); // this is the case where the instance cannot be classified, this can happen occasionally with nominal data
                    Node childNode = childNodes.ElementAt(j);
                    result = checkBranch(patient, selectedNode, childNode);
                    if (result == true)
                    {
                        selectedNode = childNodes.ElementAt(j);
                    }
                    j++;
                }
            }
            return View("Solution", db.Solutions.Single(i=>i.ID == selectedNode.NodeValue));
        }

        private Boolean checkBranch(Patient patient, Node parentNode, Node childNode)
        {
            var attribute = db.Attributes.Single(i=>i.ID ==parentNode.NodeValue);
            var patientAttributeValue = db.PatientsAttributes.Single(i => i.PatientID == patient.ID && i.AttributeID == attribute.ID).AttributeValue;
            int number1;
            int number2;
            switch (childNode.EdgeOperator)
            {
                case "==":
                    var edgeAttributeValue = db.AttributeValues.Single(i=>i.ID == childNode.EdgeValue);
                    if (edgeAttributeValue.ID == patientAttributeValue.ID)
                        return true;
                    else
                        return false;
                case "<=":
                    number1 = int.Parse(patientAttributeValue.Value);
                    number2 = childNode.EdgeValue;

                    if (number1 <= number2)
                        return true;
                    else
                        return false;
                case ">":
                    number1 = int.Parse(patientAttributeValue.Value);
                    number2 = childNode.EdgeValue;

                    if (number1 > number2)
                        return true;
                    else
                        return false;
            }
            return true;
        }

        public int AddPatient(Patient p)
        {
            db.Patients.Add(p);
            db.SaveChanges();
            var patient = db.Patients.Single(i => i.ID == p.ID);
            return 1;
        }

        public Boolean testMethod(Node n)
        {
            if (n.ID == 1)
            {
                return true;
            }
            else
                return false;
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
