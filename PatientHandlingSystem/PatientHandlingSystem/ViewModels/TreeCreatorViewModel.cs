using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.ViewModels
{
    public class TreeCreatorViewModel
    {
        public Tree Tree { get; set; }
        public List<Models.Attribute> Attributes { get; set; }
        public Models.Attribute SelectedAttribute { get; set; }
        public string ParentNodeID { get; set; }
    }
}