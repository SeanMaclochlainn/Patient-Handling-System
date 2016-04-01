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

        private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            dbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(t => sourceList.Add(t));
            dbSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(sourceList.AddRange);
            dbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t => sourceList.Remove(t));
            dbSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts =>
            {
                foreach (var t in ts) { sourceList.Remove(t); }
            });
            return dbSet.Object;
        }

        [TestMethod]
        public void TestAttributesIndex()
        {
            patientHandlingContext.PatientAttributes = GetQueryableMockDbSet(new List<Models.PatientAttribute> { new Models.PatientAttribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true } });

            var controller = new AttributesController(patientHandlingContext);

            var result = (List<Models.PatientAttribute>)controller.Index().Model;

            Assert.IsInstanceOfType(result, typeof(List<Models.PatientAttribute>));
        }

        [TestMethod]
        public void TestAttributesDetailsMethod()
        {
            patientHandlingContext.PatientAttributes = GetQueryableMockDbSet(new List<Models.PatientAttribute> { new Models.PatientAttribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true } });
            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(new List<AttributeValue> { new AttributeValue { ID = 1, AttributeID = 1, Value = "test val" } });

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
        public void NumericAttributeCreation() //tests to see that when creating a numeric attribute, no additional attribute values are created
        {
            AttributesController attributeController = new AttributesController(patientHandlingContext);

            patientHandlingContext.PatientAttributes = GetQueryableMockDbSet(new List<Models.PatientAttribute> { new Models.PatientAttribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = true } });

            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(new List<AttributeValue> { new AttributeValue { ID = 1, AttributeID = 1, Value = "test val" } });

            AttributeViewModel attributevm = new AttributeViewModel()
            {
                AttributeName = "test name",
                Numeric = true,
                AttributeValues = new List<string>() { "test val", "test2 val" }
            };
            int preAttributeCount = patientHandlingContext.PatientAttributes.Count();
            int preAttributeValuesCount = patientHandlingContext.AttributeValues.Count(); //there should not be any additional attributevalues added when creating a numeric attribute
            var result = attributeController.Create(attributevm);

            //checks that an attribute was added to the attributes table
            Assert.AreEqual(preAttributeCount + 1, patientHandlingContext.PatientAttributes.Count());

            //checks that there are no additional attribute values added upon creating a numeric attribute
            Assert.AreEqual(preAttributeValuesCount, patientHandlingContext.AttributeValues.Count());

            Assert.AreEqual(true, patientHandlingContext.PatientAttributes.Single(i => i.ID == 1).Numeric, "Attribute added not numeric");
        }

        [TestMethod]
        public void NominalAttributeCreation()
        {
            AttributesController attributeController = new AttributesController(patientHandlingContext);

            patientHandlingContext.PatientAttributes = GetQueryableMockDbSet(new List<Models.PatientAttribute> { new Models.PatientAttribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = false } });

            patientHandlingContext.AttributeValues = GetQueryableMockDbSet(new List<AttributeValue> { new AttributeValue { ID = 1, AttributeID = 1, Value = "test val" } });

            AttributeViewModel attributevm = new AttributeViewModel()
            {
                AttributeName = "test name",
                Numeric = false,
                AttributeValues = new List<string>() { "test val", "test2 val" }
            };
            int preAttributeCount = patientHandlingContext.PatientAttributes.Count();
            int preAttributeValuesCount = patientHandlingContext.AttributeValues.Count();
            var result = attributeController.Create(attributevm);

            //asserts an attribute was created
            Assert.AreEqual(preAttributeCount + 1, patientHandlingContext.PatientAttributes.Count());

            //asserts two attribute vales were added to the AttributeValues table
            Assert.AreEqual(preAttributeValuesCount+2, patientHandlingContext.AttributeValues.Count());
        }
    }
}
