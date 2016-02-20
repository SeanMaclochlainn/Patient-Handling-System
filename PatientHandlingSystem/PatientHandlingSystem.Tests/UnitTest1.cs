using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatientHandlingSystem.Controllers;
using PatientHandlingSystem.Models;
using Moq;

namespace PatientHandlingSystem.Tests
{
    [TestClass]
    public class UnitTest1
    {
       // [TestMethod]
        //public void TestBiggerThanTreeBranch()
        //{
            //Mock<Node> nodeMock = new Mock<Node>();
            //nodeMock.Setup(m => m.c)
            //PatientsController patientsController = new PatientsController();
            //PrivateObject privatePatientsController = new PrivateObject(patientsController);
            //Patient patient = new Patient();
            //Node node = new Node();
            //Node childNode = new Node();
            //privatePatientsController.Invoke("checkBranch", patient, node, childNode);
            //patientsController.checkBranch()
            //PatientsController patientsController = new PatientsController();
          //  Patient p = new Patient
          //  {
           //     FirstName
            //}
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
