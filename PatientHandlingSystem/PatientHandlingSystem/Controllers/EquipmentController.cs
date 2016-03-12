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
    public class EquipmentController : Controller
    {
        private PatientHandlingContext db;
        private EquipmentRepository equipmentRepository;

        public EquipmentController()
        {
            db = new PatientHandlingContext();
            equipmentRepository = new EquipmentRepository(db);
        }
        // GET: Equipment
        public ActionResult Index()
        {
            return View(db.Equipment.ToList());
        }

        // GET: Equipment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipment.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // GET: Equipment/Create
        public ActionResult Create()
        {
            var equipmentVM = new EquipmentViewModel { Equipment = new Equipment(), CompleteEquipmentAttributes = new List<CompleteEquipmentAttribute>() };
            return View(equipmentVM);
        }

        // POST: Equipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EquipmentViewModel equipmentVM)
        {
            if (ModelState.IsValid)
            {
                equipmentRepository.AddEquipmentItem(equipmentVM);
                equipmentRepository.Save();
                return RedirectToAction("Index");
            }

            return View(equipmentVM);
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipment.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipment);
        }

        // GET: Equipment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Equipment.Find(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            equipmentRepository.DeleteEquipmentItem(id);
            equipmentRepository.Save();
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
