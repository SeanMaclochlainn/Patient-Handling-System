using PatientHandlingSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.Models
{

    public class Node
    {
        private PatientHandlingContext db = new PatientHandlingContext();

        public int ID { get; set; }
        public int ParentID { get; set; }
        public int NodeValue { get; set; } //contains either the AttributeID or the SolutionID
        public int EdgeValue { get; set; } //contains the AttributeValueID or the numeric value of the edge pointing to this node, if the current node is numeric
        public string EdgeOperator { get; set; }
        public int TreeID { get; set; }
        public Boolean SolutionNode { get; set; }
        public Boolean Numeric { get; set; }

        public string NodeText()
        {
            if (SolutionNode == false)
                return db.Attributes.Any(i => i.ID == NodeValue) ? db.Attributes.Find(NodeValue).Name : "";
            else
                return db.Solutions.Find(NodeValue).Title;//.Substring(0,10)+"...";
        }
        public string SolutionContent()
        {
            return db.Solutions.Find(NodeValue).Content.Replace("\r\n", string.Empty); ;
        }
        public string solutionNodeIds()
        {
            return "test";// db.Nodes.Where(i => i.TreeID == TreeID && i.SolutionNode == true).Select(i => i.ID).ToArray();
        }
        //public List<int> solutionNodeIds()
        //{
        //    return db.Nodes.Where(i => i.TreeID == TreeID && i.SolutionNode == true).Select(i => i.ID).ToList();
        //}
        public string edgeText()
        {
            return db.AttributeValues.Find(EdgeValue).Value;
        }

        public virtual Tree Tree { get; set; }
    }
}