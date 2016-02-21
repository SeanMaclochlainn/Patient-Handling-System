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
            PatientHandlingContext phc = new PatientHandlingContext();
            phc.Attributes = GetQueryableMockDbSet(new Models.Attribute { ID = 1, Name = "est", Numeric = true });
            var controller = new AttributesController(phc);

            var result = (List<Models.Attribute>)controller.Index().Model;

            Assert.IsInstanceOfType(result, typeof(List<Models.Attribute>));
        }

        [TestMethod]
        public void TestAttributesDetailsMethod()
        {
            PatientHandlingContext phc = new PatientHandlingContext();

            //phc.Attributes = GetQueryableMockDbSet(new Models.Attribute { ID = 1, Name = "test", Numeric = true });
            //phc.AttributeValues = GetQueryableMockDbSet(new AttributeValue { ID = 1, AttributeID = 1, Name = "test val" });

            List<Models.Attribute> attributeList = new List<Models.Attribute>
            {
                new Models.Attribute { ID = 1, Name = "test", Numeric = true }
            };

            IQueryable<Models.Attribute> attributeQueryableList = attributeList.AsQueryable();
            var attributeMockSet = new Mock<DbSet<Models.Attribute>>();

            attributeMockSet.Setup(m => m.Find(It.IsAny<int>())).Returns<object[]>(ids => attributeQueryableList.FirstOrDefault(u => u.ID == (int)ids[0]));
            attributeMockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.Provider).Returns(attributeQueryableList.Provider);
            attributeMockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.Expression).Returns(attributeQueryableList.Expression);
            attributeMockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.ElementType).Returns(attributeQueryableList.ElementType);
            attributeMockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.GetEnumerator()).Returns(attributeQueryableList.GetEnumerator());


            phc.Attributes = attributeMockSet.Object;

            List<AttributeValue> attributeValueList = new List<AttributeValue>
            {
                new AttributeValue { ID = 1, AttributeID = 1, Name = "test val" }
            };
            IQueryable<AttributeValue> attributeValueQueryableList = attributeValueList.AsQueryable();
            var attributeValueMockSet = new Mock<DbSet<AttributeValue>>();
            attributeValueMockSet.As<IQueryable<AttributeValue>>().Setup(m => m.Provider).Returns(attributeValueQueryableList.Provider);
            attributeValueMockSet.As<IQueryable<AttributeValue>>().Setup(m => m.Expression).Returns(attributeValueQueryableList.Expression);
            attributeValueMockSet.As<IQueryable<AttributeValue>>().Setup(m => m.ElementType).Returns(attributeValueQueryableList.ElementType);
            attributeValueMockSet.As<IQueryable<AttributeValue>>().Setup(m => m.GetEnumerator()).Returns(attributeValueQueryableList.GetEnumerator());
            phc.AttributeValues = attributeValueMockSet.Object;

            var controller = new AttributesController(phc); 
            var result = ((ViewResult)controller.Details(1)).Model;
            Assert.IsInstanceOfType(result, typeof(AttributeViewModel));

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
