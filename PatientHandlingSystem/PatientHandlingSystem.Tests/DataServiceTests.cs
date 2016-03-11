using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientHandlingSystem.DAL;
using Moq;
using PatientHandlingSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace PatientHandlingSystem.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        Mock<PatientHandlingContext> patientHandlingContext;

        public DataServiceTests()
        {
            patientHandlingContext = new Mock<PatientHandlingContext>();
        }

        [TestMethod]
        public void TestDeleteNodeMethod()
        {
            //this is a tree with two levels
            //patientHandlingContext = MockDatabase.GetNodeMockDbSet(new List<Node> {
            //    new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 3, ParentID = 1, NodeValue = 0, EdgeValue = 2, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 4, ParentID = 1, NodeValue = 0, EdgeValue = 3, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 5, ParentID = 2, NodeValue = 0, EdgeValue = 4, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 6, ParentID = 2, NodeValue = 0, EdgeValue = 5, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 7, ParentID = 2, NodeValue = 0, EdgeValue = 6, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false }
            //    }, patientHandlingContext);

            //DataService dataService = new DataService(patientHandlingContext.Object);

            //dataService.DeleteNode(1, "2");
            //int nodesCount = patientHandlingContext.Object.Nodes.Count();
            //Assert.Equals(nodesCount, 4);
        }
    }
}
