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
using System.Diagnostics;

namespace PatientHandlingSystem.Controllers
{
    public class AttributesController : Controller
    {
        private PatientHandlingContext db;

        public AttributesController()
        {
            db = new PatientHandlingContext();
        }

        public AttributesController(PatientHandlingContext context)
        {
            db = context;
        }
        // GET: Attributes
        public ViewResult Index()
        {
            return View(db.PatientAttributes.ToList());
        }

        // GET: Attributes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var patientAttribute = db.PatientAttributes.Single(i=>i.ID == id);
            if (patientAttribute == null)
            {
                return HttpNotFound();
            }

            var attributeValues = db.PatientAttributeValues.Where(i => i.PatientAttributeID == patientAttribute.ID).Select(i=>i.Value).ToList();

            AttributeViewModel attributeVM = new AttributeViewModel
            {
                AttributeName = patientAttribute.Name,
                AttributeValues = attributeValues,
                Numeric = patientAttribute.Numeric
            };
            return View(attributeVM);
        }

        public ActionResult Create()
        {
            var attributeList = new List<string>();
            attributeList.Add("");
            var attribute = new AttributeViewModel
            {
                AttributeValues = attributeList
            };
            return View(attribute);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AttributeViewModel attributevm)
        {
            PatientAttribute patientAttribute = new PatientAttribute
            {
                Name = attributevm.AttributeName, 
                Numeric = attributevm.Numeric
            };

            if (ModelState.IsValid)
            {
                db.PatientAttributes.Add(patientAttribute);
                db.SaveChanges();
            }

            //no attribute values are added if the attribute is numeric
            if (!patientAttribute.Numeric)
            {
                var attributeValues = new List<PatientAttributeValue>();

                foreach (var i in attributevm.AttributeValues)
                {
                    attributeValues.Add(new PatientAttributeValue
                    {
                        PatientAttributeID = patientAttribute.ID,
                        Value = i
                    });
                }

                if (ModelState.IsValid)
                {
                    db.PatientAttributeValues.AddRange(attributeValues);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            else
            {
                return RedirectToAction("Index");
            }
                
            

            return View(patientAttribute);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientAttribute attribute = db.PatientAttributes.Find(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }

            var attributeValues = db.PatientAttributeValues.Where(i => i.PatientAttributeID == attribute.ID).ToList();
            var completeAttribute = new CompleteAttribute
            {
                Attribute = attribute,
                AttributeValues = attributeValues
            };
            return View(completeAttribute);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompleteAttribute completeAttribute)
        {
            var attribute = completeAttribute.Attribute;

            foreach(var av in completeAttribute.AttributeValues)
            {
                db.Entry(av).State = EntityState.Modified;
            }
            db.SaveChanges();
            if (ModelState.IsValid)
            {
                db.Entry(attribute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attribute);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientAttribute patientAttribute = db.PatientAttributes.Find(id);
            if (patientAttribute == null)
            {
                return HttpNotFound();
            }
            return View(patientAttribute);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PatientAttribute attribute = db.PatientAttributes.Find(id);
            var attributeValues = new List<PatientAttributeValue>();
            attributeValues.AddRange(db.PatientAttributeValues.Where(i => i.PatientAttributeID == attribute.ID).ToList());

            db.PatientAttributes.Remove(attribute);
            db.PatientAttributeValues.RemoveRange(attributeValues);
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
