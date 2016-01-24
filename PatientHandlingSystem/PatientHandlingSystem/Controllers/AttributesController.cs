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
    public class AttributesController : Controller
    {
        private PatientHandlingContext db = new PatientHandlingContext();

        // GET: Attributes
        public ActionResult Index()
        {
            return View(db.Attributes.ToList());
        }

        // GET: Attributes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Models.Attribute attribute = db.Attributes.Find(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }

            var attributeValues = db.AttributeValues.Where(i => i.AttributeID == attribute.ID).Select(i=>i.Name).ToList();

            AttributeViewModel attributeVM = new AttributeViewModel
            {
                AttributeName = attribute.Name,
                AttributeValues = attributeValues
            };
            return View(attributeVM);
        }

        // GET: Attributes/Create
        public ActionResult Create()
        {
            var attributeList = new List<string>();
            attributeList.Add("");
            attributeList.Add("");
            attributeList.Add("");
            var attribute = new AttributeViewModel
            {
                AttributeValues = attributeList
            };
            return View(attribute);
        }

        // POST: Attributes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AttributeViewModel attributevm)
        {
            Models.Attribute attribute = new Models.Attribute
            {
                Name = attributevm.AttributeName
            };

            if (ModelState.IsValid)
            {
                db.Attributes.Add(attribute);
                db.SaveChanges();
            }

            var attributeValues = new List<AttributeValue>();

            foreach(var i in attributevm.AttributeValues)
            {
                attributeValues.Add(new AttributeValue
                {
                    AttributeID = attribute.ID,
                    Name = i
                });
            }
            
            if(ModelState.IsValid)
            {
                db.AttributeValues.AddRange(attributeValues);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attribute);
        }

        // GET: Attributes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Attribute attribute = db.Attributes.Find(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }

            var attributeValues = db.AttributeValues.Where(i => i.AttributeID == attribute.ID).ToList();
            var completeAttribute = new CompleteAttribute
            {
                Attribute = attribute,
                AttributeValues = attributeValues
            };
            return View(completeAttribute);
        }

        // POST: Attributes/Edit/5
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

        // POST: Attributes/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Attribute attribute = db.Attributes.Find(id);
            var attributeValues = new List<AttributeValue>();
            attributeValues.AddRange(db.AttributeValues.Where(i => i.AttributeID == attribute.ID).ToList());

            db.Attributes.Remove(attribute);
            db.AttributeValues.RemoveRange(attributeValues);
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
