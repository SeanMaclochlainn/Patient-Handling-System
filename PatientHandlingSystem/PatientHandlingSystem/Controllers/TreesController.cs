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

            var treeCreator = new TreeCreatorViewModel
            {
                Tree = tree,
                Attributes = db.Attributes.ToList()
            };
            return View(treeCreator);
        }

        public PartialViewResult UpdateTree(TreeCreatorViewModel treeCreatorVM, string undoButton)
        {
            if (undoButton == "undo")
            {
                var previousParentNodeId = db.Nodes.ToList().Last().ParentID;
                var previousParentNode = db.Nodes.Find(previousParentNodeId);
                var previousChildNodes = db.Nodes.Where(i => i.ParentID == previousParentNode.ID).ToList();
                db.Nodes.RemoveRange(previousChildNodes);
                previousParentNode.NodeValue = 0;
                db.Entry(previousParentNode).State = EntityState.Modified;
                db.SaveChanges();
            }
            else if(treeCreatorVM.SolutionInput == true)
            {
                Solution solution = new Solution { Content = treeCreatorVM.Solution, TreeID = treeCreatorVM.Tree.ID };
                db.Solutions.Add(solution);
                db.SaveChanges();

                int parentNodeID = int.Parse(treeCreatorVM.ParentNodeID);
                var parentNode = db.Nodes.Find(parentNodeID);
                parentNode.NodeValue = solution.ID;
                parentNode.SolutionNode = true;
                db.Entry(parentNode).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                treeCreatorVM.SelectedAttribute.AttributeValues = db.AttributeValues.Where(i => i.AttributeID == treeCreatorVM.SelectedAttribute.ID).ToList();

                //ParentNodeID is the ID of the node that was selected by the user. It is named ParentNodeID, as it is about to become a parent node
                int parentID = 0;
                Boolean parentNodeExists = int.TryParse(treeCreatorVM.ParentNodeID, out parentID);
                var parentNode = new Node();

                //this if statement only runs when the first node is inserted
                if (!parentNodeExists)
                {
                    parentID = 0;
                    parentNode = new Node
                    {
                        NodeValue = treeCreatorVM.SelectedAttribute.ID,
                        ParentID = parentID,
                        TreeID = treeCreatorVM.Tree.ID
                    };
                    db.Nodes.Add(parentNode);
                    db.SaveChanges();
                    Debug.WriteLine("Added Node. Attribute Name: " + parentNode.NodeText() + " This is the first node in the tree. ");
                }
                else
                {
                    parentNode = db.Nodes.Find(parentID);
                    parentNode.NodeValue = treeCreatorVM.SelectedAttribute.ID;
                    db.Entry(parentNode).State = EntityState.Modified;
                    db.SaveChanges();
                }

                var nodesToAdd = new List<Node>();
                if (treeCreatorVM.SelectedAttribute.Numeric)
                {
                    var childNodeA = new Node
                    {
                        ParentID = parentNode.ID,
                        TreeID = treeCreatorVM.Tree.ID,
                        EdgeOperator = "<=",
                        EdgeValue = int.Parse(treeCreatorVM.SelectedAttributeValue.Value), 
                        Numeric = true
                    };
                    var childNodeB = new Node
                    {
                        ParentID = parentNode.ID,
                        TreeID = treeCreatorVM.Tree.ID,
                        EdgeOperator = ">",
                        EdgeValue = int.Parse(treeCreatorVM.SelectedAttributeValue.Value), 
                        Numeric = true
                    };
                    nodesToAdd.Add(childNodeA);
                    nodesToAdd.Add(childNodeB);
                }
                else
                {
                    var attributeValues = db.Attributes.Find(treeCreatorVM.SelectedAttribute.ID).AttributeValues;
                    foreach (var av in attributeValues)
                    {
                        if (av.ID == 0)
                            Debug.WriteLine("test");
                        var childNode = new Node
                        {
                            ParentID = parentNode.ID,
                            TreeID = treeCreatorVM.Tree.ID,
                            EdgeOperator = "==",
                            EdgeValue = av.ID, 
                            Numeric = false
                        };
                        nodesToAdd.Add(childNode);
                    }
                }
                db.Nodes.AddRange(nodesToAdd);
                db.SaveChanges();
            }
            
            var nodes = db.Nodes.Where(i => i.TreeID == treeCreatorVM.Tree.ID).ToList();
            return PartialView("_Tree", nodes);
        }
        // POST: Trees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TreeCreatorViewModel treeVM)
        {
            var tree = db.Trees.Find(treeVM.Tree.ID);
            tree.Name = treeVM.Tree.Name;
            db.Entry(tree).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
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

            var treeCreator = new TreeCreatorViewModel
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
