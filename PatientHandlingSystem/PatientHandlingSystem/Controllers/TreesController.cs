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
    public class TreesController : Controller
    {
        private PatientHandlingContext db;

        public TreesController()
        {
            db = new PatientHandlingContext();
        }

        public TreesController(PatientHandlingContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            return View(db.Trees.ToList());
        }

        public ActionResult ViewTree(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            ViewBag.TreeName = tree.Name;
            return View(db.Nodes.Where(i=>i.TreeID == tree.ID).ToList());
        }

        // GET: Trees/Create
        public ActionResult Create()
        {
            var tree = new Tree { Name = "" };
            db.Trees.Add(tree);
            db.SaveChanges();

            var treeCreator = new TreeEditorViewModel
            {
                Tree = tree,
                Attributes = db.Attributes.ToList(),
                Nodes = new List<Node>()
            };
            return View(treeCreator);
        }

        public PartialViewResult UpdateTree(TreeEditorViewModel treeCreatorVM, string deleteButton)
        {
            DataService dataService = new DataService(db);
            if(deleteButton == "true")
            {
                dataService.DeleteNode(treeCreatorVM.Tree.ID, treeCreatorVM.ParentNodeID);
            }
            else if(treeCreatorVM.SolutionInput == true)
            {
                dataService.EnterSolutionNode(treeCreatorVM.ParentNodeID, treeCreatorVM.Tree.ID, treeCreatorVM.Solution);
            }
            else
            {
                dataService.EnterAttributeNode(treeCreatorVM.ParentNodeID, treeCreatorVM.SelectedAttribute.ID, treeCreatorVM.Tree.ID, treeCreatorVM.SelectedAttribute.Numeric, treeCreatorVM.SelectedAttributeNumericValue.Value);
            }
            
            var nodes = db.Nodes.Where(i => i.TreeID == treeCreatorVM.Tree.ID).ToList();
            return PartialView("_Tree", nodes);
        }

        public PartialViewResult DeleteNode(int nodeId, int treeId)
        {
            var dataService = new DataService(db);
            dataService.IsLeafNode(nodeId, treeId);
            return PartialView("_Tree", db.Nodes.Where(i => i.TreeID == treeId).ToList());
        }

        // POST: Trees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken] this has to be removed because of the ajax call
        public ActionResult AddTreeName(string treeName)
        {
            var tree = db.Trees.Single(i=>i.ID == db.Trees.Max(j=>j.ID));
            tree.Name = treeName;
            db.Entry(tree).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public void EditTreeName(Tree postedTree)
        {
            var tree = db.Trees.Find(postedTree.ID);
            tree.Name = postedTree.Name;
            db.Entry(tree).State = EntityState.Modified;
            db.SaveChanges();
        }
        // GET: Trees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }

            var treeCreator = new TreeEditorViewModel
            {
                Tree = tree,
                Attributes = db.Attributes.ToList(),
                Nodes = db.Nodes.Where(i => i.TreeID == tree.ID).ToList()
            };
            return View(treeCreator);
        }

        // GET: Trees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tree tree = db.Trees.Find(id);
            if (tree == null)
            {
                return HttpNotFound();
            }
            return View(tree);
        }

        // POST: Trees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tree tree = db.Trees.Single(i=>i.ID ==id);
            var nodes = db.Nodes.Where(i => i.TreeID == tree.ID).ToList();
            var solutions = db.Solutions.Where(i => i.TreeID == tree.ID).ToList();

            db.Trees.Remove(tree);
            db.Nodes.RemoveRange(nodes);
            db.Solutions.RemoveRange(solutions);

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
