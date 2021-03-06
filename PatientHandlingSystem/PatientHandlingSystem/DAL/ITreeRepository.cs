﻿using PatientHandlingSystem.Models;
using PatientHandlingSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.DAL
{
    public interface ITreeRepository
    {
        void DeleteRegularNode(int treeId, string nodeIdStr);
        void DeleteSolutionNode(int treeId, string nodeIdStr);
        void EnterAttributeNode(string parentNodeId, int selectedAttributeId, int treeId, Boolean selectedAttributeNumeric, string selectedAttributeNumericValue);
        void EnterSolutionNode(string parentNodeIdStr, int treeId, string solutionContent, string solutionTitle);
        void EnterEquipmentNode(string parentNodeId, int selectedEquipmentId, int selectedEquipmentAttributeId, int treeId);
        TreeEditorViewModel GetTreeEditorViewModel(int treeId);
        void Save();
        Boolean IsLeafNode(int nodeId, int treeId);
        Solution GetHandlingPlan(int patientId, int treeId);
        TreeEditorViewModel CreateTreeEditorViewModel();
    }
}