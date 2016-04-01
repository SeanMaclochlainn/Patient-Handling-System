using PatientHandlingSystem.DAL;
using PatientHandlingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class TreeRepository : ITreeRepository
    {
        private PatientHandlingContext db;

        public TreeRepository(PatientHandlingContext context)
        {
            db = context;
        }

        public List<PatientAttribute> getAllRelevantPatientsAttributes(int patientId)//gets all the patientattributes for this patient, for every attribute possible
        {
            return db.PatientsAttributes.Where(i => i.PatientID == patientId).ToList();
        }

        public List<AttributeValue> getAllPatientAttributeValues(int patientId)
        {
            var attributeValues = new List<AttributeValue>();
            var patientattrs = db.PatientsAttributes;
            //some weird bug in the database mocking is forcing me to us a for loop instead of a foreach loop here
            for (int i = 0; i < db.PatientsAttributes.Count(); i++)
            {
                var attrvals = db.AttributeValues.ToList();
                var attributeValue = db.AttributeValues.Single(j => j.ID == db.PatientsAttributes.ElementAt(i).AttributeValueID);
                attributeValues.Add(attributeValue);
            }
            return attributeValues;
        }

        //Tree Methods
        public void EnterAttributeNode(string parentNodeId, int selectedAttributeId, int treeId, Boolean selectedAttributeNumeric, string selectedAttributeNumericValue)
        {
            //ParentNodeID is the ID of the node that was selected by the user. It is named ParentNodeID, as it is about to become a parent node
            int parentID = 0;
            Boolean parentNodeExists = int.TryParse(parentNodeId, out parentID);
            var parentNode = new Node();

            //this if statement only runs when the first node is inserted
            if (!parentNodeExists)
            {
                parentID = 0;
                parentNode = new Node
                {
                    NodeValue = selectedAttributeId,
                    ParentID = parentID,
                    TreeID = treeId
                };
                db.Nodes.Add(parentNode);
                db.SaveChanges();
            }
            else
            {
                parentNode = db.Nodes.Find(parentID);
                parentNode.NodeValue = selectedAttributeId;
                db.Entry(parentNode).State = EntityState.Modified;
                db.SaveChanges();
            }

            var nodesToAdd = new List<Node>();
            if (selectedAttributeNumeric)
            {
                var childNodeA = new Node
                {
                    ParentID = parentNode.ID,
                    TreeID = treeId,
                    EdgeOperator = "<=",
                    EdgeValue = int.Parse(selectedAttributeNumericValue),
                    Numeric = true
                };
                var childNodeB = new Node
                {
                    ParentID = parentNode.ID,
                    TreeID = treeId,
                    EdgeOperator = ">",
                    EdgeValue = int.Parse(selectedAttributeNumericValue),
                    Numeric = true
                };
                nodesToAdd.Add(childNodeA);
                nodesToAdd.Add(childNodeB);
            }
            else
            {
                var attributeValues = db.Attributes.Find(selectedAttributeId).AttributeValues;
                foreach (var av in attributeValues)
                {
                    var childNode = new Node
                    {
                        ParentID = parentNode.ID,
                        TreeID = treeId,
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

        public void EnterSolutionNode(string parentNodeIdStr, int treeId, string solutionContent, string solutionTitle)
        {
            int parentNodeId = int.Parse(parentNodeIdStr);
            Solution solution = new Solution { Content = solutionContent, TreeID = treeId, Title = solutionTitle };
            db.Solutions.Add(solution);
            db.SaveChanges();

            var parentNode = db.Nodes.Find(parentNodeId);
            parentNode.NodeValue = solution.ID;
            parentNode.SolutionNode = true;
            db.Entry(parentNode).State = EntityState.Modified;
            db.SaveChanges();
        }

        public Boolean IsLeafNode(int nodeId, int treeId)
        {
            var selectedNode = db.Nodes.Find(nodeId);
            var nodes = db.Nodes.Where(i => i.TreeID == treeId).ToList();
            foreach (var node in nodes)
            {
                if (node.ParentID == selectedNode.ID)
                    return false;
            }
            return true;
        }

        public void DeleteRegularNode(int treeId, string nodeIdStr)
        {
            //get parent and child nodes
            int nodeId = int.Parse(nodeIdStr);
            var parentNode = db.Nodes.Single(i => i.ID == nodeId);
            var childNodes = db.Nodes.Where(i => i.ParentID == nodeId).ToList();

            //if there is only one parent node in the tree, delete all nodes, otherwise, delete the child nodes and make the parent node into a stub node
            if (parentNode.ParentID == 0)
            {
                db.Nodes.Remove(parentNode);
                db.Nodes.RemoveRange(childNodes);
            }
            else
            {
                db.Nodes.RemoveRange(childNodes);
                var node = db.Nodes.Single(i => i.ID == nodeId);
                node.NodeValue = 0;
                db.Entry(node).State = EntityState.Modified;
            }
            db.SaveChanges();

        }

        public void DeleteSolutionNode(int treeId, string nodeIdStr)
        {
            int nodeId = int.Parse(nodeIdStr);
            var node = db.Nodes.Single(i => i.ID == nodeId);
            node.SolutionNode = false;
            node.NodeValue = 0;
            db.Entry(node).State = EntityState.Modified;
        }

        public TreeEditorViewModel GetTreeEditorViewModel(int treeId)
        {
            var treeCreator = new TreeEditorViewModel
            {
                Tree = db.Trees.Find(treeId),
                Attributes = db.Attributes.ToList(),
                Nodes = db.Nodes.Where(i => i.TreeID == treeId).OrderBy(j => j.ID).ToList(),
                EquipmentAttributes = db.EquipmentAttributes.ToList(),
                Equipment = db.Equipment.ToList()
            };
            return treeCreator;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public Solution GetHandlingPlan(int patientId, int treeId)
        {
            var patient = db.Patients.Single(i => i.ID == patientId);
            var tree = db.Trees.Single(i => i.ID == treeId);

            Node selectedNode = db.Nodes.Single(i => i.ParentID < 1 && i.TreeID == tree.ID); //the node with parentID of zero is the root node

            while (db.Nodes.Where(i => i.ParentID == selectedNode.ID).Count() > 0) //basically while the selected node has children
            {
                Boolean result = false;
                List<Node> childNodes = db.Nodes.Where(i => i.ParentID == selectedNode.ID).ToList();
                int j = 0;
                while (result != true)
                {
                    if (j >= childNodes.Count)
                        return new Solution { Content = "Error" }; // this is the case where the instance cannot be classified, this can happen occasionally with nominal data
                    Node childNode = childNodes.ElementAt(j);
                    result = checkBranch(patient, selectedNode, childNode);
                    if (result == true)
                    {
                        selectedNode = childNodes.ElementAt(j);
                    }
                    j++;
                }
            }
            return db.Solutions.Single(i => i.ID == selectedNode.NodeValue);
        }

        private Boolean checkBranch(Patient patient, Node parentNode, Node childNode)
        {
            var attribute = db.Attributes.Single(i => i.ID == parentNode.NodeValue);
            var patientAttributeValue = db.PatientsAttributes.Single(i => i.PatientID == patient.ID && i.AttributeID == attribute.ID).AttributeValue;
            int number1;
            int number2;
            switch (childNode.EdgeOperator)
            {
                case "==":
                    var edgeAttributeValue = db.AttributeValues.Single(i => i.ID == childNode.EdgeValue);
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

    }
    
}