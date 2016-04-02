﻿using Moq;
using Moq.Protected;
using PatientHandlingSystem.DAL;
using PatientHandlingSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientHandlingSystem.Tests
{
    class MockDatabase
    {
        public static Mock<PatientHandlingContext> GetPatientMockDbSet(List<Patient> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Patient>>();

            mockSet.As<IQueryable<Patient>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Patient>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Patient>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Patient>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Patient>())).Callback<Patient>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Patient>>())).Callback<IEnumerable<Patient>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Patient>())).Callback<Patient>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Patient>>())).Callback<IEnumerable<Patient>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });


            mockContext.Setup(i => i.Patients).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetAttributeMockDbSet(List<Models.PatientAttribute> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Models.PatientAttribute>>();

            mockSet.As<IQueryable<Models.PatientAttribute>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Models.PatientAttribute>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Models.PatientAttribute>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Models.PatientAttribute>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Models.PatientAttribute>())).Callback<Models.PatientAttribute>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Models.PatientAttribute>>())).Callback<IEnumerable<Models.PatientAttribute>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Models.PatientAttribute>())).Callback<Models.PatientAttribute>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Models.PatientAttribute>>())).Callback<IEnumerable<Models.PatientAttribute>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.PatientAttributes).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetAttributeValueMockDbSet(List<PatientAttributeValue> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<PatientAttributeValue>>();

            mockSet.As<IQueryable<PatientAttributeValue>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<PatientAttributeValue>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<PatientAttributeValue>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<PatientAttributeValue>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<PatientAttributeValue>())).Callback<PatientAttributeValue>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<PatientAttributeValue>>())).Callback<IEnumerable<PatientAttributeValue>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<PatientAttributeValue>())).Callback<PatientAttributeValue>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<PatientAttributeValue>>())).Callback<IEnumerable<PatientAttributeValue>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.PatientAttributeValues).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetPatientAttributeMockDbSet(List<Patient_PatientAttribute> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Patient_PatientAttribute>>();

            mockSet.As<IQueryable<Patient_PatientAttribute>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Patient_PatientAttribute>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Patient_PatientAttribute>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Patient_PatientAttribute>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Patient_PatientAttribute>())).Callback<Patient_PatientAttribute>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Patient_PatientAttribute>>())).Callback<IEnumerable<Patient_PatientAttribute>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Patient_PatientAttribute>())).Callback<Patient_PatientAttribute>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Patient_PatientAttribute>>())).Callback<IEnumerable<Patient_PatientAttribute>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.Patient_PatientAttributes).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetTreeMockDbSet(List<Tree> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Tree>>();

            mockSet.As<IQueryable<Tree>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Tree>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Tree>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Tree>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Tree>())).Callback<Tree>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Tree>>())).Callback<IEnumerable<Tree>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Tree>())).Callback<Tree>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Tree>>())).Callback<IEnumerable<Tree>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.Trees).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetSolutionMockDbSet(List<Solution> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Solution>>();

            mockSet.As<IQueryable<Solution>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Solution>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Solution>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Solution>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Solution>())).Callback<Solution>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Solution>>())).Callback<IEnumerable<Solution>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Solution>())).Callback<Solution>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Solution>>())).Callback<IEnumerable<Solution>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.Solutions).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetNodeMockDbSet(List<Node> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Node>>();

            mockSet.As<IQueryable<Node>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Node>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Node>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Node>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Node>())).Callback<Node>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Node>>())).Callback<IEnumerable<Node>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Node>())).Callback<Node>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Node>>())).Callback<IEnumerable<Node>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.Nodes).Returns(mockSet.Object);

            return mockContext;
        }

    }
}
