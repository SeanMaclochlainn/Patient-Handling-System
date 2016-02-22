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
    public class AttributeControllerTests
    {
        PatientHandlingContext patientHandlingContext = new PatientHandlingContext();
        
        public AttributeControllerTests()
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
