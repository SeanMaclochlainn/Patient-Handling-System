using PatientHandlingSystem.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{
    public class DataService
    {
        private PatientHandlingContext db;

        public DataService(PatientHandlingContext context)
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
            for (int i=0; i<db.PatientsAttributes.Count();i++)
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

        public void EnterSolutionNode(string parentNodeIdStr, int treeId, string solutionContent)
        {
            int parentNodeId = int.Parse(parentNodeIdStr);
            Solution solution = new Solution { Content = solutionContent, TreeID = treeId };
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
            foreach(var node in nodes)
            {
                if (node.ParentID == selectedNode.ID)
                    return false;
            }
            return true;
        }

        //this doesn't actually delete the node in question, it only deletes its child nodes, and changes its node value to 0
        public void DeleteNode(int treeId, string nodeIdStr)
        {
            int nodeId = int.Parse(nodeIdStr);
            var childNodes = db.Nodes.Where(i => i.ParentID == nodeId).ToList();
            db.Nodes.RemoveRange(childNodes);

            var node = db.Nodes.Single(i => i.ID == nodeId);
            node.NodeValue = 0;
            db.Entry(node).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}