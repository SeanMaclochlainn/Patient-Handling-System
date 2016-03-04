using Moq;
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

        public static Mock<PatientHandlingContext> GetAttributeMockDbSet(List<Models.Attribute> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<Models.Attribute>>();

            mockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<Models.Attribute>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<Models.Attribute>())).Callback<Models.Attribute>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<Models.Attribute>>())).Callback<IEnumerable<Models.Attribute>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<Models.Attribute>())).Callback<Models.Attribute>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<Models.Attribute>>())).Callback<IEnumerable<Models.Attribute>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.Attributes).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetAttributeValueMockDbSet(List<AttributeValue> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<AttributeValue>>();

            mockSet.As<IQueryable<AttributeValue>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<AttributeValue>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<AttributeValue>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<AttributeValue>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<AttributeValue>())).Callback<AttributeValue>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<AttributeValue>>())).Callback<IEnumerable<AttributeValue>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<AttributeValue>())).Callback<AttributeValue>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<AttributeValue>>())).Callback<IEnumerable<AttributeValue>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.AttributeValues).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<PatientHandlingContext> GetPatientAttributeMockDbSet(List<PatientAttribute> data, Mock<PatientHandlingContext> mockContext)
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<PatientAttribute>>();

            mockSet.As<IQueryable<PatientAttribute>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<PatientAttribute>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<PatientAttribute>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<PatientAttribute>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(i => i.Add(It.IsAny<PatientAttribute>())).Callback<PatientAttribute>(j => { data.Add(j); j.ID = data.Max(i => i.ID) + 1; });
            mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<PatientAttribute>>())).Callback<IEnumerable<PatientAttribute>>(data.AddRange);
            mockSet.Setup(m => m.Remove(It.IsAny<PatientAttribute>())).Callback<PatientAttribute>(t => data.Remove(t));
            mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<PatientAttribute>>())).Callback<IEnumerable<PatientAttribute>>(ts =>
            {
                foreach (var t in ts) { data.Remove(t); }
            });

            mockContext.Setup(i => i.PatientsAttributes).Returns(mockSet.Object);

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
