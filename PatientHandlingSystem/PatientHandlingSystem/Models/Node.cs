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
        public int? SecondaryNodeValue { get; set; } //this is only used for equipment attribute ids, so when there is an equipment node
        public int EdgeValue { get; set; } //contains the AttributeValueID or the numeric value of the edge pointing to this node, if the current node is numeric
        public string EdgeOperator { get; set; }
        public int TreeID { get; set; }
        public Boolean SolutionNode { get; set; }
        public Boolean Numeric { get; set; }
        public Boolean PatientAttributeNode { get; set; } 
        public Boolean EquipmentNode { get; set; }

        public string NodeText()
        {//TODO take out the conditionals from all of these
            if (PatientAttributeNode)
                return db.PatientAttributes.Any(i => i.ID == NodeValue) ? db.PatientAttributes.Find(NodeValue).Name : "";
            else if (EquipmentNode)
                return db.Equipment.Any(i => i.ID == NodeValue) ? db.Equipment.Find(NodeValue).Name + ": " + db.EquipmentAttributes.Find(SecondaryNodeValue).Name : "";
            else
                return db.Solutions.Find(NodeValue).Title;//.Substring(0,10)+"...";
        }
        public string SolutionContent()
        {
            return db.Solutions.Find(NodeValue).Content.Replace("\r\n", string.Empty); ;
        }
        public string edgeText()
        {
            if (PatientAttributeNode) 
                return db.PatientAttributeValues.Find(EdgeValue).Value;
            else //node is an equipment attribute 
                return db.EquipmentAttributeValues.Find(EdgeValue).Name;
        }

        public virtual Tree Tree { get; set; }
    }
}