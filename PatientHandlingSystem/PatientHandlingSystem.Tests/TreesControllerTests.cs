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
        PatientHandlingContext patientHandlingContext = new PatientHandlingContext();

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


        [TestMethod]
        public void DeleteConfirmedDeletesAllParts()
        {
            patientHandlingContext.Solutions = GetQueryableMockDbSet(new List<Solution> {
                new Solution { ID = 1, Content = "Solution 1", TreeID = 1 },
                new Solution { ID = 2, Content = "Numeric solution 1", TreeID = 1 },
                new Solution { ID = 3, Content = "Numeric solution 2", TreeID = 1 }
            });

            patientHandlingContext.Trees = GetQueryableMockDbSet(new List<Tree> { new Tree { ID = 1, Name = "Test Tree" } });

            patientHandlingContext.Nodes = GetQueryableMockDbSet( new List<Node> {
                new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = true },
                new Node { ID = 3, ParentID = 2, NodeValue = 2, EdgeValue = 200, EdgeOperator = "<=", TreeID = 1, SolutionNode = true, Numeric = false },
                new Node { ID = 4, ParentID = 2, NodeValue = 3, EdgeValue = 200, EdgeOperator = ">", TreeID = 1, SolutionNode = true, Numeric = false }
                });

            TreesController treesController = new TreesController(patientHandlingContext);
            int treesCount = patientHandlingContext.Trees.Count();
            int nodeCount = patientHandlingContext.Nodes.Count();
            int solutionCount = patientHandlingContext.Solutions.Count();
            treesController.DeleteConfirmed(1);
            Assert.AreEqual(treesCount - 1, patientHandlingContext.Trees.Count(), "Trees deletion error");
            Assert.AreEqual(nodeCount - 4, patientHandlingContext.Nodes.Count(), "Nodes deletion error");
            Assert.AreEqual(solutionCount - 3, patientHandlingContext.Solutions.Count(), "Solutions deletion error");
        }
    }
}
