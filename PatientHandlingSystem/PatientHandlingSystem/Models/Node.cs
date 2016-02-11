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
        public int NodeValue { get; set; }
        public int EdgeValue { get; set; }
        public string EdgeOperator { get; set; }
        public int TreeID { get; set; }
        public Boolean SolutionNode { get; set; }

        public string NodeText()
        {
            if (SolutionNode == false)
                return db.Attributes.Any(i=>i.ID == NodeValue) ? db.Attributes.Find(NodeValue).Name : "";
            else
                return "solution";
        }

        public string edgeText()
        {
            return db.AttributeValues.Find(EdgeValue).Name;
        }

        public virtual Tree Tree { get; set; }
    }
}