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
using System.Web.Mvc;

namespace PatientHandlingSystem.Tests
{
    [TestClass]
    public class PatientControllerTests
    {
        PatientHandlingContext patientHandlingContext = new PatientHandlingContext();

        private static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet.Object;
        }

        [TestMethod]
        public void IterateThroughNominalTree()
        {
            patientHandlingContext.Attributes = GetQueryableMockDbSet(
                new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = false },
                new Models.Attribute { ID = 2, Name = "Injured", Numeric = false }
            );

            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
                new AttributeValue { ID = 1, AttributeID = 1, Value = "Full" },
                new AttributeValue { ID = 2, AttributeID = 1, Value = "None" },
                new AttributeValue { ID = 3, AttributeID = 2, Value = "Yes" },
                new AttributeValue { ID = 4, AttributeID = 2, Value = "No" }
                );

            patientHandlingContext.Patients = GetQueryableMockDbSet(new Patient { ID = 1, FirstName = "test", LastName = "testlast" });

            patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
                new PatientAttribute { ID = 1, PatientID = 1, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
                new PatientAttribute { ID = 2, PatientID = 1, AttributeID = 2, AttributeValueID = 3, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 3) }
                );

            patientHandlingContext.Solutions = GetQueryableMockDbSet(
                new Solution { ID = 1, Content = "Solution 1", TreeID = 1 }
                );

            patientHandlingContext.Trees = GetQueryableMockDbSet(new Tree { ID = 1, Name = "Test Tree" });

            patientHandlingContext.Nodes = GetQueryableMockDbSet(
                new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 3, ParentID = 2, NodeValue = 1, EdgeValue = 3, EdgeOperator = "==", TreeID = 1, SolutionNode = true, Numeric = false }

                );

            PatientsController patientsController = new PatientsController(patientHandlingContext);
            var result = ((ViewResult)patientsController.HandlingPlan(1, 1)).Model;
            Assert.AreEqual(((Solution)result).Content, "Solution 1");

        }

        [TestMethod]
        public void IterateThroughNumericalAndNominalTree()
        {
            patientHandlingContext.Attributes = GetQueryableMockDbSet(
                new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = false },
                new Models.Attribute { ID = 2, Name = "Weight", Numeric = true }
            );

            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
                new AttributeValue { ID = 1, AttributeID = 1, Value = "Full" },
                new AttributeValue { ID = 2, AttributeID = 1, Value = "None" },
                new AttributeValue { ID = 3, AttributeID = 2, Value = "100" },
                new AttributeValue { ID = 4, AttributeID = 2, Value = "201" }
                );

            patientHandlingContext.Patients = GetQueryableMockDbSet(
                new Patient { ID = 1, FirstName = "test", LastName = "testlast" },
                new Patient { ID = 2, FirstName = "test2", LastName = "testlast2" }
                );

            patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
                new PatientAttribute { ID = 1, PatientID = 1, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
                new PatientAttribute { ID = 2, PatientID = 1, AttributeID = 2, AttributeValueID = 3, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 3) },
                new PatientAttribute { ID = 3, PatientID = 2, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
                new PatientAttribute { ID = 4, PatientID = 2, AttributeID = 2, AttributeValueID = 4, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 4) }
                );

            patientHandlingContext.Solutions = GetQueryableMockDbSet(
                new Solution { ID = 1, Content = "Solution 1", TreeID = 1 },
                new Solution { ID = 2, Content = "Numeric solution 1", TreeID = 1 },
                new Solution { ID = 3, Content = "Numeric solution 2", TreeID = 1 }
                );

            patientHandlingContext.Trees = GetQueryableMockDbSet(new Tree { ID = 1, Name = "Test Tree" });

            patientHandlingContext.Nodes = GetQueryableMockDbSet(
                new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = true },
                new Node { ID = 3, ParentID = 2, NodeValue = 2, EdgeValue = 200, EdgeOperator = "<=", TreeID = 1, SolutionNode = true, Numeric = false },
                new Node { ID = 4, ParentID = 2, NodeValue = 3, EdgeValue = 200, EdgeOperator = ">", TreeID = 1, SolutionNode = true, Numeric = false }
                );

            PatientsController patientsController = new PatientsController(patientHandlingContext);
            var numericTest1 = ((ViewResult)patientsController.HandlingPlan(1, 1)).Model;

            //this tests the <=200 part of the tree
            Assert.AreEqual(((Solution)numericTest1).Content, "Numeric solution 1");

            //this tests the >200 part of the tree
            var numericTest2 = ((ViewResult)patientsController.HandlingPlan(2, 1)).Model;
            Assert.AreEqual(((Solution)numericTest2).Content, "Numeric solution 2");

        }
    }
}
