using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class TreeEditorViewModel
    {
        public Tree Tree { get; set; } 
        public List<PatientAttribute> PatientAttributes { get; set; } //patient attributes for the Patient Attribute drop down list (for GET)
        public PatientAttribute SelectedPatientAttribute { get; set; } //Patient attribute selected from the drop down (for POST)
        public PatientAttributeValue SelectedPatientAttributeNumericValue { get; set; } //patient attribute numeric value (for POST). Is null unless there is a numeric value entered
        public List<Equipment> Equipment { get; set; } 
        public Equipment SelectedEquipment { get; set; }
        public List<EquipmentAttribute> EquipmentAttributes { get; set; }
        public EquipmentAttribute SelectedEquipmentAttribute { get; set; }
        public string ParentNodeID { get; set; } //the ID of the node that is selected in the tree - i.e. it will be the parent node if the insertion  is successful
        public string SolutionTitle { get; set; }
        public string Solution { get; set; }
        public string NodeType { get; set; } //type of node being inserted (on POST)
        public List<Node> Nodes { get; set; } //nodes in the tree (for GET)
    }
}