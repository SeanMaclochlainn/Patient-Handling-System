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
        private ITreeRepository treeRepository;

        public TreesController()
        {
            db = new PatientHandlingContext();
            treeRepository = new TreeRepository(db);
        }

        public TreesController(PatientHandlingContext context, ITreeRepository dataService)
        {
            db = context;
            this.treeRepository = dataService;
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
            return View("Create", "_TreeEditor", treeCreator);
        }

        public PartialViewResult UpdateTree(TreeEditorViewModel treeCreatorVM, string deleteButton)
        {
            List<Node> originalNodes = db.Nodes.Where(i => i.TreeID == treeCreatorVM.Tree.ID).ToList(); ;

            //check if a node is selected
            if(treeCreatorVM.ParentNodeID == null && deleteButton == "true")
            {
                ModelState.AddModelError("NoNodeSelected", "Please select a parent node");
                return PartialView("_Tree", originalNodes);
            }
            //check if users is trying to delete a node and a stub node is selected
            else if (deleteButton == "true" && originalNodes.SingleOrDefault(i => i.ID == int.Parse(treeCreatorVM.ParentNodeID)).NodeValue == 0)
            {
                ModelState.AddModelError("StubNodeSelected", "Cannot delete a stub node");
                return PartialView("_Tree", originalNodes);
            }


            if (deleteButton == "true")//when delete button is pressed
            {
                var node = originalNodes.Single(i => i.ID == int.Parse(treeCreatorVM.ParentNodeID));
                if(node.SolutionNode)
                {
                    treeRepository.DeleteSolutionNode(treeCreatorVM.Tree.ID, treeCreatorVM.ParentNodeID);
                }
                else
                {
                    treeRepository.DeleteRegularNode(treeCreatorVM.Tree.ID, treeCreatorVM.ParentNodeID);
                }
            }
            else if(treeCreatorVM.NodeType == "Solution")//when user is trying to input a solution node
            {
                treeRepository.EnterSolutionNode(treeCreatorVM.ParentNodeID, treeCreatorVM.Tree.ID, treeCreatorVM.Solution);
            }
            else //when user is trying to input a regular node, numeric or otherwise
            {
                treeRepository.EnterAttributeNode(treeCreatorVM.ParentNodeID, treeCreatorVM.SelectedAttribute.ID, treeCreatorVM.Tree.ID, treeCreatorVM.SelectedAttribute.Numeric, treeCreatorVM.SelectedAttributeNumericValue.Value);
            }

            List<Node> nodes = db.Nodes.Where(i => i.TreeID == treeCreatorVM.Tree.ID).ToList(); ;

            return PartialView("_Tree", nodes);
        }

        public PartialViewResult DeleteNode(int nodeId, int treeId)
        {
            var dataService = new TreeRepository(db);
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
            return View("Edit", "_TreeEditor", treeCreator);
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
