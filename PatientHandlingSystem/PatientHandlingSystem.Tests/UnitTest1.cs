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
    public class UnitTest1
    {
        PatientHandlingContext patientHandlingContext = new PatientHandlingContext();
        
        public UnitTest1()
        {
            //patientHandlingContext.Attributes = GetQueryableMockDbSet(new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true });
            //patientHandlingContext.AttributeValues = GetQueryableMockDbSet(new AttributeValue { ID = 1, AttributeID = 1, Name = "test val" });
            //patientHandlingContext.Patients = GetQueryableMockDbSet(new Patient { ID = 1, FirstName = "test", LastName = "testlast" });
            //patientHandlingContext.Trees = GetQueryableMockDbSet(new Tree { ID = 1, Name = "Test Tree" });
            //var nodes = new Node[]
            //{
            //    new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
            //    new Node { ID = 2, ParentID = 1, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false }
            //};
            //patientHandlingContext.Nodes = GetQueryableMockDbSet(nodes);
        }

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
        public void TestAttributesIndex()
        {
            patientHandlingContext.Attributes = GetQueryableMockDbSet(new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true });

            var controller = new AttributesController(patientHandlingContext);

            var result = (List<Models.Attribute>)controller.Index().Model;

            Assert.IsInstanceOfType(result, typeof(List<Models.Attribute>));
        }

        [TestMethod]
        public void TestAttributesDetailsMethod()
        {
            patientHandlingContext.Attributes = GetQueryableMockDbSet(new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true });
            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(new AttributeValue { ID = 1, AttributeID = 1, Value = "test val" });

            var controller = new AttributesController(patientHandlingContext); 
            var result = ((ViewResult)controller.Details(1)).Model;
            Assert.IsInstanceOfType(result, typeof(AttributeViewModel));

        }

        [TestMethod]
        public void TestAttributeCreateVM() //tests to see if it has created the correct viewmodel
        {
            AttributesController attributesController = new AttributesController();
            var result = ((ViewResult)attributesController.Create()).Model;
            Assert.IsInstanceOfType(result, typeof(AttributeViewModel));
            var attributeList = new List<string> { "" };
            var attribute = new AttributeViewModel
            {
                AttributeValues = attributeList
            };

            Assert.AreEqual(((AttributeViewModel)result).AttributeValues.Count, attribute.AttributeValues.Count);
        }

        [TestMethod]
        public void IterateThroughNominalTree()
        {
            patientHandlingContext.Attributes = GetQueryableMockDbSet(
                new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true },
                new Models.Attribute { ID = 2, Name = "Injured", Numeric = true }
            );

            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
                new AttributeValue { ID = 1, AttributeID = 1, Value = "Full" },
                new AttributeValue { ID = 2, AttributeID = 1, Value = "None" },
                new AttributeValue { ID = 3, AttributeID = 2, Value = "Yes" },
                new AttributeValue { ID = 4, AttributeID = 2, Value = "No" }
                );

            patientHandlingContext.Patients = GetQueryableMockDbSet(new Patient { ID = 1, FirstName = "test", LastName = "testlast" });

            patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
                new PatientAttribute { ID = 1, PatientID = 1, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i=>i.ID == 1) },
                new PatientAttribute { ID = 2, PatientID = 1, AttributeID = 2, AttributeValueID = 3, AttributeValue = patientHandlingContext.AttributeValues.Single(i=>i.ID == 3) }
                );

            patientHandlingContext.Solutions = GetQueryableMockDbSet(
                new Solution { ID = 1, Content = "Solution 1" }
                );

            patientHandlingContext.Trees = GetQueryableMockDbSet(new Tree { ID = 1, Name = "Test Tree" });

            patientHandlingContext.Nodes = GetQueryableMockDbSet(
                new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
                new Node { ID = 3, ParentID = 2, NodeValue = 1, EdgeValue = 3, EdgeOperator = "==", TreeID = 1, SolutionNode = true, Numeric = false}

                );

            PatientsController patientsController = new PatientsController(patientHandlingContext);
            var result = ((ViewResult)patientsController.HandlingPlan(1, 1)).Model;
            Assert.AreEqual(((Solution)result).Content, "Solution 1");
            
        }

        //[TestMethod]
        //public void IterateThroughNumericalTree()
        //{
        //    patientHandlingContext.Attributes = GetQueryableMockDbSet(
        //        new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true },
        //        new Models.Attribute { ID = 2, Name = "Co-operation", Numeric = true }
        //    );

        //    patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
        //        new AttributeValue { ID = 1, AttributeID = 1, Name = "Full" },
        //        new AttributeValue { ID = 2, AttributeID = 1, Name = "None" },
        //        new AttributeValue { ID = 3, AttributeID = 2, Name = "Fully" },
        //        new AttributeValue { ID = 4, AttributeID = 2, Name = "Partial" }
        //        );

        //    patientHandlingContext.Patients = GetQueryableMockDbSet(new Patient { ID = 1, FirstName = "test", LastName = "testlast" });

        //    patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
        //        new PatientAttribute { ID = 1, PatientID = 1, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
        //        new PatientAttribute { ID = 2, PatientID = 1, AttributeID = 2, AttributeValueID = 3 }
        //        );

        //    patientHandlingContext.Solutions = GetQueryableMockDbSet(
        //        new Solution { ID = 1, Content = "Solution 1" }
        //        );

        //    patientHandlingContext.Trees = GetQueryableMockDbSet(new Tree { ID = 1, Name = "Test Tree" });

        //    patientHandlingContext.Nodes = GetQueryableMockDbSet(
        //        new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
        //        new Node { ID = 2, ParentID = 1, NodeValue = 1, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = true, Numeric = false }
        //        );

        //    PatientsController patientsController = new PatientsController(patientHandlingContext);
        //    var result = ((ViewResult)patientsController.HandlingPlan(1, 1)).Model;
        //    Assert.AreEqual(((Solution)result).Content, "Solution 1");

        //}
        [TestMethod]
        public void simpleTest()
        {
            PatientsController patientsController = new PatientsController();
            var result = patientsController.Add(1, 2);
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void simpleTest2()
        {
            PatientsController patientsController = new PatientsController();

            Node n = new Node
            {
                ID = 1
            };
            var result = patientsController.testMethod(n);
            Assert.AreEqual(true, result);
        }
    }
}
