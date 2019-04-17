using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using webapi.Controllers;
using System.Net.Http;
using System.Net;

namespace webapi.Tests
{
    [TestClass]
    public class TempControllerTest
    {
            TempController controller = new TempController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        [TestMethod]
        public void GetAllTest()
        {

            var result = controller.Get();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }

        [TestMethod]
        [Priority(1)]
        public void PostTest()
        {
            

            var result = controller.Post("Gramado");

            //Assert.IsNotNull(controller.Get("Gramado"));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }

        [TestMethod]
        [Priority(2)]
        public void PostConflictTest()
        {
            

            var result = controller.Post("Gramado");

            //Assert.IsNotNull(controller.Get("Gramado"));
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);

        }

        [TestMethod]
        [Priority(2)]
        public void GetOneTest()
        {
           

            var result = controller.Get("Porto Alegre");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }

        [TestMethod]
        [Priority(2)]
        public void GetOneNotFoundTest()
        {
            
            
            var result = controller.Get("asdasd");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        }

        [TestMethod]
        [Priority(2)]
        public void GetOneNullTest()
        {
            

            var result = controller.Get("");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        }

        [TestMethod]
        public void PostCepTest()
        {
            

            var result = controller.PostCep("11010010");

            controller.Delete("Santos");
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void PostCepInvalidTest()
        {
            

            var result = controller.PostCep("00000");

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        }

        [TestMethod]
        public void PostCepExistTest()
        {
            

            var result = controller.PostCep("26030160");

            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);


        }

        [TestMethod]
        [Priority(3)]
        public void DeleteTempTest()
        {
            

            var result = controller.DeleteTemps("Gramado");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }

        [TestMethod]
        [Priority(3)]
        public void DeleteTempNotFoundTest()
        {
           

            var result = controller.DeleteTemps("asda");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        }

        [TestMethod]
        [Priority(4)]
        public void DeleteCityTest()
        {
            

            var result = controller.Delete("Gramado");

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

        }

        [TestMethod]
        [Priority(5)]
        public void DeleteCityNotFoundTest()
        {
            

            var result = controller.Delete("Gramado");

            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        }

        //[TestMethod]
        //[Priority(5)]
        //public void RefreshTempTest()
        //{
        //    APIs core = new APIs();


        //    var result = core

        //    Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);

        //}


    }
}
