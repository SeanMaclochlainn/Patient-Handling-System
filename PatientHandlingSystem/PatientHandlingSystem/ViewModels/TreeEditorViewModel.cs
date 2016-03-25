﻿using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class TreeEditorViewModel
    {
        public Tree Tree { get; set; }
        public List<Models.Attribute> Attributes { get; set; }
        public Models.Attribute SelectedAttribute { get; set; }
        public AttributeValue SelectedAttributeNumericValue { get; set; }
        public List<Equipment> Equipment { get; set; }
        public Equipment SelectedEquipment { get; set; }
        public List<EquipmentAttribute> EquipmentAttributes { get; set; }
        public EquipmentAttribute SelectedEquipmentAttribute { get; set; }
        public string ParentNodeID { get; set; }
        public string SolutionTitle { get; set; }
        public string Solution { get; set; }
        public string NodeType { get; set; }
        public List<Node> Nodes { get; set; }
    }
}