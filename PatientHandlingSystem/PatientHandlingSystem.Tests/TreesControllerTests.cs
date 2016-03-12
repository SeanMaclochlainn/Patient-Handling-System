using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientHandlingSystem.Controllers;
using PatientHandlingSystem.Models;
using Moq;
using System.Data.Entity;
using PatientHandlingSystem.DAL;
using System.Linq;
using System.Collections.Generic;
using PatientHandlingSystem.ViewModels;

namespace PatientHandlingSystem.Tests
{
    [TestClass]
    public class TreesControllerTests
    {
        Mock<PatientHandlingContext> patientHandlingContext;

        public TreesControllerTests()
        {
            patientHandlingContext = new Mock<PatientHandlingContext>();
        }

        //PatientHandlingContext patientHandlingContext = new PatientHandlingContext();

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t => sourceList.Remove(t));
            dbSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>> (ts =>
                  {
                      foreach(var t in ts) { sourceList.Remove(t); }
                  });

            return dbSet.Object;
        }

        //Ensures when deleting a tree that the solutions and nodes associated with the tree, and the tree itself, is deleted
        //[TestMethod]
        //public void DeleteConfirmedDeletesAllParts()
        //{
        //    patientHandlingContext = MockDatabase.GetSolutionMockDbSet(new List<Solution> {
        //        new Solution { ID = 1, Content = "Solution 1", TreeID = 1 },
        //        new Solution { ID = 2, Content = "Numeric solution 1", TreeID = 1 },
        //        new Solution { ID = 3, Content = "Numeric solution 2", TreeID = 1 }
        //    }, patientHandlingContext);


        //    patientHandlingContext = MockDatabase.GetTreeMockDbSet(new List<Tree> { new Tree { ID = 1, Name = "Test Tree" } }, patientHandlingContext);

        //    patientHandlingContext = MockDatabase.GetNodeMockDbSet(new List<Node> {
        //        new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
        //        new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = true },
        //        new Node { ID = 3, ParentID = 2, NodeValue = 2, EdgeValue = 200, EdgeOperator = "<=", TreeID = 1, SolutionNode = true, Numeric = false },
        //        new Node { ID = 4, ParentID = 2, NodeValue = 3, EdgeValue = 200, EdgeOperator = ">", TreeID = 1, SolutionNode = true, Numeric = false }
        //        }, patientHandlingContext);
            

        //    TreesController treesController = new TreesController(patientHandlingContext.Object);
            
        //    int treesCount = patientHandlingContext.Object.Trees.Count();
        //    int nodeCount = patientHandlingContext.Object.Nodes.Count();
        //    int solutionCount = patientHandlingContext.Object.Solutions.Count();
        //    treesController.DeleteConfirmed(1);
        //    Assert.AreEqual(treesCount - 1, patientHandlingContext.Object.Trees.Count(), "Trees deletion error");
        //    Assert.AreEqual(nodeCount - 4, patientHandlingContext.Object.Nodes.Count(), "Nodes deletion error");
        //    Assert.AreEqual(solutionCount - 3, patientHandlingContext.Object.Solutions.Count(), "Solutions deletion error");
        //}

        [TestMethod]
        public void UpadateTreeController()
        {
            //set up context and dataservice instance
            patientHandlingContext = MockDatabase.GetNodeMockDbSet(new List<Node> {
                new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 2, ParentID = 1, NodeValue = 0, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 3, ParentID = 1, NodeValue = 2, EdgeValue = 2, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 4, ParentID = 3, NodeValue = 1, EdgeValue = 3, EdgeOperator = "==", TreeID = 1, SolutionNode = true, Numeric = false }
                }, patientHandlingContext);
            var mock = new Mock<IDataService>();
            mock.Setup(i => i.DeleteRegularNode(It.IsAny<int>(), It.IsAny<string>()));
            mock.Setup(i => i.EnterSolutionNode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()));

            TreesController treesController = new TreesController(patientHandlingContext.Object, mock.Object);

            //tests when a parent node hasn't been selected
            //arrange
            TreeEditorViewModel treeEditorVM = new TreeEditorViewModel { ParentNodeID = null, Tree = new Tree { ID = 1 } };

            //act
            treesController.UpdateTree(treeEditorVM, "true");

            //assert
            Assert.AreEqual(treesController.ModelState["NoNodeSelected"].Errors[0].ErrorMessage, "Please select a parent node");

            //test when a user is trying to delete a node and a stub node is selected
            //arrange
            treeEditorVM.ParentNodeID = "2";

            //act
            treesController.UpdateTree(treeEditorVM, "true");

            //assert
            Assert.AreEqual(treesController.ModelState["StubNodeSelected"].Errors[0].ErrorMessage, "Cannot delete a stub node");

            //test when a user is trying to delete a solution node
            //arrange
            treeEditorVM.ParentNodeID = "4";

            //act
            treesController.UpdateTree(treeEditorVM, "true");

            //assert
            mock.Verify(i => i.DeleteSolutionNode(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

            //tests when the delete button has been pressed, with a node selected
            //Arrange
            treeEditorVM.ParentNodeID = "1";
            treeEditorVM.Tree = new Tree { ID = 1, Name = "" };
            

            //act
            treesController.UpdateTree(treeEditorVM, "true");

            //assert
            mock.Verify(i => i.DeleteRegularNode(It.IsAny<int>(), It.IsAny<string>()), Times.Once);

            //test for when a solution node is entered
            //arrange
            treeEditorVM.SolutionInput = true;

            //act
            treesController.UpdateTree(treeEditorVM, null);

            //assert
            mock.Verify(i => i.EnterSolutionNode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);

            //test for when an attribute node was entered
            //arrange
            treeEditorVM.SolutionInput = false;
            treeEditorVM.SelectedAttribute = new Models.Attribute();
            treeEditorVM.SelectedAttributeNumericValue = new AttributeValue();

            //act
            treesController.UpdateTree(treeEditorVM, null);

            //assert
            mock.Verify(i => i.EnterAttributeNode(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Boolean>(), It.IsAny<string>()), Times.Once);


        }

    }
}
