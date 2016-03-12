using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PatientHandlingSystem.DAL
{
    public interface IDataService
    {
        void DeleteRegularNode(int treeId, string nodeIdStr);
        void DeleteSolutionNode(int treeId, string nodeIdStr);
        void EnterAttributeNode(string parentNodeId, int selectedAttributeId, int treeId, Boolean selectedAttributeNumeric, string selectedAttributeNumericValue);
        void EnterSolutionNode(string parentNodeIdStr, int treeId, string solutionContent);
    }
}