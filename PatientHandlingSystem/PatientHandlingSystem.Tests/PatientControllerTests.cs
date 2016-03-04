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

        //Mock<PatientHandlingContext> patientHandlingContext;

        //public PatientControllerTests()
        //{
        //    patientHandlingContext = new Mock<PatientHandlingContext>();
        //}

        //private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        //{
        //    var queryable = sourceList.AsQueryable();

        //    var dbSet = new Mock<DbSet<T>>();
        //    dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        //    dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        //    dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        //    dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        //    dbSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(t => sourceList.Add(t));
        //    dbSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(sourceList.AddRange);
        //    dbSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t => sourceList.Remove(t));
        //    dbSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts =>
        //    {
        //        foreach (var t in ts) { sourceList.Remove(t); }
        //    });

        //    return dbSet.Object;
        //}

        //public static DbSet<Patient> MockUnitOfWork(List<Patient> students)
        //{
        //    var mockUnitOfWork = new Mock<PatientHandlingContext>();

        //    mockUnitOfWork.Setup(x => x.Patients()).Returns(students.AsQueryable());
        //    mockUnitOfWork.Setup(x => x.StudentsRepository.Add(It.IsAny<Student>())).Callback<Student>((s) => students.Add(s));

        //    return mockUnitOfWork.Object;
        //}
        //[TestMethod]
        //public void IterateThroughNominalTree()
        //{
        //    var dbset = new Mock<DbSet<Patient>>();
        //    var mockContext = new Mock<PatientHandlingContext>();
        //    mockContext.Setup(m => m.Patients).Returns(dbset.Object);

        //   // var service = new BlogService(mockContext.Object);
        //    //service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

        //    dbset.Verify(m => m.Add(It.IsAny<Patient>()), Times.Once());
        //    mockContext.Verify(m => m.SaveChanges(), Times.Once());
        //    patientHandlingContext.Attributes = GetQueryableMockDbSet(
        //        new List<Models.Attribute>
        //        {
        //            new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = false },
        //            new Models.Attribute { ID = 2, Name = "Injured", Numeric = false }
        //        }
        //    );

        //    patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
        //        new List<AttributeValue>
        //        {
        //            new AttributeValue { ID = 1, AttributeID = 1, Value = "Full" },
        //            new AttributeValue { ID = 2, AttributeID = 1, Value = "None" },
        //            new AttributeValue { ID = 3, AttributeID = 2, Value = "Yes" },
        //            new AttributeValue { ID = 4, AttributeID = 2, Value = "No" }
        //        }
        //    );

        //    patientHandlingContext.Patients = GetQueryableMockDbSet(new List<Patient> { new Patient { ID = 1, FirstName = "test", LastName = "testlast" } });

        //    patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
        //        new List<PatientAttribute>
        //        {
        //            new PatientAttribute { ID = 1, PatientID = 1, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
        //            new PatientAttribute { ID = 2, PatientID = 1, AttributeID = 2, AttributeValueID = 3, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 3) }
        //        }
        //    );

        //    patientHandlingContext.Solutions = GetQueryableMockDbSet(
        //        new List<Solution>
        //        {
        //            new Solution { ID = 1, Content = "Solution 1", TreeID = 1 }
        //        }
        //    );

        //    patientHandlingContext.Trees = GetQueryableMockDbSet(new List<Tree> { new Tree { ID = 1, Name = "Test Tree" } });

        //    patientHandlingContext.Nodes = GetQueryableMockDbSet(
        //        new List<Node>
        //        {
        //            new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
        //            new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = false },
        //            new Node { ID = 3, ParentID = 2, NodeValue = 1, EdgeValue = 3, EdgeOperator = "==", TreeID = 1, SolutionNode = true, Numeric = false }
        //        }
        //    );

        //    PatientsController patientsController = new PatientsController(patientHandlingContext);
        //    var result = ((ViewResult)patientsController.HandlingPlan(1, 1)).Model;
        //    Assert.AreEqual(((Solution)result).Content, "Solution 1");

        //}

        //[TestMethod]
        //public void IterateThroughNumericalAndNominalTree()
        //{
        //    patientHandlingContext.Attributes = GetQueryableMockDbSet(
        //        new List<Models.Attribute>
        //        {
        //            new Models.Attribute { ID = 1, Name = "Weight Bearing Capacity", Numeric = false },
        //            new Models.Attribute { ID = 2, Name = "Weight", Numeric = true }
        //        }
        //    );

        //    patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
        //        new List<AttributeValue>
        //        {
        //            new AttributeValue { ID = 1, AttributeID = 1, Value = "Full" },
        //            new AttributeValue { ID = 2, AttributeID = 1, Value = "None" },
        //            new AttributeValue { ID = 3, AttributeID = 2, Value = "100" },
        //            new AttributeValue { ID = 4, AttributeID = 2, Value = "201" }
        //        }
        //    );

        //    patientHandlingContext.Patients = GetQueryableMockDbSet(
        //        new List<Patient>
        //        {
        //            new Patient { ID = 1, FirstName = "test", LastName = "testlast" },
        //            new Patient { ID = 2, FirstName = "test2", LastName = "testlast2" }
        //        }
        //    );

        //    patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
        //        new List<PatientAttribute>
        //        {
        //            new PatientAttribute { ID = 1, PatientID = 1, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
        //            new PatientAttribute { ID = 2, PatientID = 1, AttributeID = 2, AttributeValueID = 3, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 3) },
        //            new PatientAttribute { ID = 3, PatientID = 2, AttributeID = 1, AttributeValueID = 1, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 1) },
        //            new PatientAttribute { ID = 4, PatientID = 2, AttributeID = 2, AttributeValueID = 4, AttributeValue = patientHandlingContext.AttributeValues.Single(i => i.ID == 4) }
        //        }
        //    );

        //    patientHandlingContext.Solutions = GetQueryableMockDbSet(
        //        new List<Solution>
        //        {
        //            new Solution { ID = 1, Content = "Solution 1", TreeID = 1 },
        //            new Solution { ID = 2, Content = "Numeric solution 1", TreeID = 1 },
        //            new Solution { ID = 3, Content = "Numeric solution 2", TreeID = 1 }
        //        }
        //    );

        //    patientHandlingContext.Trees = GetQueryableMockDbSet(new List<Tree> { new Tree { ID = 1, Name = "Test Tree" } });

        //    patientHandlingContext.Nodes = GetQueryableMockDbSet(
        //        new List<Node>
        //        {
        //            new Node { ID = 1, ParentID = 0, NodeValue = 1, EdgeValue = 0, EdgeOperator = null, TreeID = 1, SolutionNode = false, Numeric = false },
        //            new Node { ID = 2, ParentID = 1, NodeValue = 2, EdgeValue = 1, EdgeOperator = "==", TreeID = 1, SolutionNode = false, Numeric = true },
        //            new Node { ID = 3, ParentID = 2, NodeValue = 2, EdgeValue = 200, EdgeOperator = "<=", TreeID = 1, SolutionNode = true, Numeric = false },
        //            new Node { ID = 4, ParentID = 2, NodeValue = 3, EdgeValue = 200, EdgeOperator = ">", TreeID = 1, SolutionNode = true, Numeric = false }
        //        }
        //    );

        //    PatientsController patientsController = new PatientsController(patientHandlingContext);
        //    var numericTest1 = ((ViewResult)patientsController.HandlingPlan(1, 1)).Model;

        //    //this tests the <=200 part of the tree
        //    Assert.AreEqual(((Solution)numericTest1).Content, "Numeric solution 1");

        //    //this tests the >200 part of the tree
        //    var numericTest2 = ((ViewResult)patientsController.HandlingPlan(2, 1)).Model;
        //    Assert.AreEqual(((Solution)numericTest2).Content, "Numeric solution 2");

        //}

        //[TestMethod]
        //public void CreatePatient()
        //{

        //    patientHandlingContext.Attributes = GetQueryableMockDbSet(
        //        new List<Models.Attribute>
        //        {
        //            new Models.Attribute { ID = 1, Name = "attr1", Numeric = false },
        //            new Models.Attribute { ID = 2, Name = "attr2", Numeric = false }
        //        }
        //    );
        //    patientHandlingContext.AttributeValues = GetQueryableMockDbSet(
        //        new List<AttributeValue>
        //        {
        //            new AttributeValue { ID = 1, AttributeID = 1, Value = "attr1ValA", Attribute = patientHandlingContext.Attributes.Single(i=>i.ID ==1)},
        //            new AttributeValue { ID = 2, AttributeID = 1, Value = "attr1ValB", Attribute = patientHandlingContext.Attributes.Single(i=>i.ID ==1)},
        //            new AttributeValue { ID = 3, AttributeID = 2, Value = "attr2ValA", Attribute = patientHandlingContext.Attributes.Single(i=>i.ID ==2)},
        //            new AttributeValue { ID = 4, AttributeID = 2, Value = "attr2ValB", Attribute = patientHandlingContext.Attributes.Single(i=>i.ID ==2)}
        //        }
        //    );

        //    //this doesn't work unless there is an initial value present otherwise it doesn't work
        //    patientHandlingContext.PatientsAttributes = GetQueryableMockDbSet(
        //        new List<PatientAttribute>
        //        {
        //            new PatientAttribute { ID = 1, AttributeID = 1, PatientID = 1, AttributeValueID = 1 },
        //            new PatientAttribute { ID = 2, AttributeID = 2, PatientID = 1, AttributeValueID = 3 }
        //        }
        //    );

        //    patientHandlingContext.Patients = GetQueryableMockDbSet(
        //        new List<Patient>
        //        {
        //            new Patient { ID = 1, FirstName = "fnamep1", LastName = "fnamep2" }
        //        }
        //    );
        //    var completeAttributes = new List<CompleteAttribute>
        //    {
        //        new CompleteAttribute { Attribute = patientHandlingContext.Attributes.Single(i=>i.ID == 1), SelectedAttributeValue = patientHandlingContext.AttributeValues.Single(i=>i.ID == 1) },
        //        //new CompleteAttribute {Attribute = patientHandlingContext.Attributes.Single(i=>i.ID ==2), SelectedAttributeValue = new AttributeValue {ID = 0, AttributeID = 2, Value = "50"} }
        //    };

        //    PatientsController patientController = new PatientsController(patientHandlingContext);
        //    PatientViewModel patientvm = new PatientViewModel
        //    {
        //        Patient = new Patient { ID = 2, FirstName = "fname", LastName = "lname" },
        //        CompleteAttributes = completeAttributes
        //    };

        //    int prePatientCount = patientHandlingContext.Patients.Count();
        //    DataService ds = new DataService(patientHandlingContext);
        //    var prePatientsAttributes = ds.getAllRelevantPatientsAttributes(1);
        //    var results = patientController.Create(patientvm);

        //    //assert that a patient was added to the database
        //    Assert.AreEqual(prePatientCount + 1, patientHandlingContext.Patients.Count());

        //    var patient = patientHandlingContext.Patients.Find(patientHandlingContext.Patients.Max(i=>i.ID));

        //    //assert that the patient was assigned the correct attribute value to attr1
        //    var result2 = ds.getAllPatientAttributeValues(patient.ID).ToList();
        //    Assert.AreEqual("testval", ds.getAllPatientAttributeValues(patient.ID).First().Value);

        //    //assert that an instance of PatientsAttributes was added to the table for the patients first attribute
        //    Assert.AreEqual(patient.ID, patientHandlingContext.PatientsAttributes.Single(i => i.AttributeID == 1 && i.AttributeValueID == 1).PatientID);

        //    //assert that the patients second (numeric) attribute's value is entered into the AttributeValue field
        //    //Assert.AreEqual("50", ds.getAllPatientAttributeValues(patient.ID).Single(i => i.AttributeID == 2).Value);


        //}
        //public static Mock<PatientHandlingContext> GetPatientMockDbSet(List<Patient> data, Mock<PatientHandlingContext> mockContext) 
        //{
        //    var queryableData = data.AsQueryable();
        //    var mockSet = new Mock<DbSet<Patient>>();

        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        //    mockSet.Setup(i => i.Add(It.IsAny<Patient>())).Callback<Patient>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });

        //    mockContext.Setup(i => i.Patients).Returns(mockSet.Object);

        //    return mockContext;
        //}

        //[TestMethod]
        //public void CreatePatient()
        //{
        //    var attributes = new List<Models.Attribute>
        //        {
        //            new Models.Attribute { ID = 1, Name = "attr1", Numeric = false },
        //            new Models.Attribute { ID = 2, Name = "attr2", Numeric = false }
        //        };
        //    var attributeValues = new List<AttributeValue>
        //            {
        //                new AttributeValue { ID = 1, AttributeID = 1, Value = "attr1ValA", Attribute = attributes.Single(i=>i.ID ==1)},
        //                new AttributeValue { ID = 2, AttributeID = 1, Value = "attr1ValB", Attribute = attributes.Single(i=>i.ID ==1)},
        //                new AttributeValue { ID = 3, AttributeID = 2, Value = "attr2ValA", Attribute = attributes.Single(i=>i.ID ==2)},
        //                new AttributeValue { ID = 4, AttributeID = 2, Value = "attr2ValB", Attribute = attributes.Single(i=>i.ID ==2)}
        //            };
        //    var patientsAttributes = new List<PatientAttribute>
        //            {
        //                new PatientAttribute { ID = 1, AttributeID = 1, PatientID = 1, AttributeValueID = 1 },
        //                new PatientAttribute { ID = 2, AttributeID = 2, PatientID = 1, AttributeValueID = 3 }
        //            };
        //    var patients = new List<Patient>
        //            {
        //                new Patient { ID = 1, FirstName = "fnamep1", LastName = "fnamep2" }
        //            };
                
        //    var mockContext = new Mock<PatientHandlingContext>();
        //    mockContext = MockDatabase.GetAttributeMockDbSet(attributes, mockContext);
        //    mockContext = MockDatabase.GetAttributeValueMockDbSet(attributeValues, mockContext);
        //    mockContext = MockDatabase.GetPatientAttributeMockDbSet(patientsAttributes, mockContext);
        //    mockContext = MockDatabase.GetPatientMockDbSet(patients, mockContext);

        //    PatientsController patientController = new PatientsController(mockContext.Object);

        //    var completeAttributes = new List<CompleteAttribute>
        //    {
        //        new CompleteAttribute { Attribute = attributes.Single(i=>i.ID == 1), SelectedAttributeValue = attributeValues.Single(i=>i.ID == 1) },
        //        //new CompleteAttribute {Attribute = patientHandlingContext.Attributes.Single(i=>i.ID ==2), SelectedAttributeValue = new AttributeValue {ID = 0, AttributeID = 2, Value = "50"} }
        //    };

        //    PatientViewModel patientvm = new PatientViewModel
        //    {
        //        Patient = new Patient { ID = 2, FirstName = "fname", LastName = "lname" },
        //        CompleteAttributes = completeAttributes
        //    };

        //    int prePatientCount = mockContext.Object.Patients.Count();
        //    DataService ds = new DataService(mockContext.Object);
        //    var prePatientsAttributes = ds.getAllRelevantPatientsAttributes(1);
        //    var results = patientController.Create(patientvm);

        //    //assert that a patient was added to the database
        //    Assert.AreEqual(prePatientCount + 1, mockContext.Object.Patients.Count());

        //    var patient = mockContext.Object.Patients.Find(mockContext.Object.Patients.Max(i => i.ID));

        //    //assert that the patient was assigned the correct attribute value to attr1
        //    var result2 = ds.getAllPatientAttributeValues(patient.ID).ToList();
        //    Assert.AreEqual("testval", ds.getAllPatientAttributeValues(patient.ID).First().Value);

        //    //assert that an instance of PatientsAttributes was added to the table for the patients first attribute
        //    Assert.AreEqual(patient.ID, mockContext.Object.PatientsAttributes.Single(i => i.AttributeID == 1 && i.AttributeValueID == 1).PatientID);

        //    //assert that the patients second (numeric) attribute's value is entered into the AttributeValue field
        //    //Assert.AreEqual("50", ds.getAllPatientAttributeValues(patient.ID).Single(i => i.AttributeID == 2).Value);
        //}
        //implementation using GetPatientMockDbSet method of test "AddPatient" method
        //[TestMethod]
        //public void CreatePatient()
        //{
        //    var patients = new List<Patient>
        //    {
        //        new Patient { ID = 1, FirstName = "BBB", LastName = "test" },
        //        new Patient { ID = 2, FirstName = "ZZZ", LastName = "dfsd" },
        //        new Patient { ID = 3, FirstName = "AAA", LastName = "fds"}
        //    };

        //    var mockContext = GetPatientMockDbSet(patients, new Mock<PatientHandlingContext>());


        //    PatientsController pc = new PatientsController(mockContext.Object);
        //    pc.AddPatient(new Patient { FirstName = "test", LastName = "fds" });


        //}


        //perfect implementation, just not in a method
        //[TestMethod]
        //public void CreatePatient()
        //{
        //    var data = new List<Patient>
        //    {
        //        new Patient { ID = 1, FirstName = "BBB", LastName = "test" },
        //        new Patient { ID = 2, FirstName = "ZZZ", LastName = "dfsd" },
        //        new Patient { ID = 3, FirstName = "AAA", LastName = "fds"}
        //    };

        //    var queryableData = data.AsQueryable();
        //    var mockSet = new Mock<DbSet<Patient>>();

        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        //    mockSet.As<IQueryable<Patient>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        //    mockSet.Setup(i => i.Add(It.IsAny<Patient>())).Callback<Patient>(j=> { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });



        //    var mockContext = new Mock<PatientHandlingContext>();
        //    mockContext.Setup(c => c.Patients).Returns(mockSet.Object);


        //    PatientsController pc = new PatientsController(mockContext.Object);
        //    pc.AddPatient(new Patient { FirstName = "test", LastName = "fds" });


        //}
    }
}
