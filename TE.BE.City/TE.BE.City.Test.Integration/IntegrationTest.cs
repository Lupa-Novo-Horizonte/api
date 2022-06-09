using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace TE.BE.City.Test.Integration
{
    /// <summary>
    /// This test are using MSTest
    /// </summary>
    [TestClass]
    public class IntegrationTest
    {
        /// <summary>
        /// Ping all the APIs used and check if it's alive.
        /// </summary>
        [TestMethod]
        public void ApiTest()
        {
            //WebRequest request = WebRequest.Create("http://localhost:63869/api/Token");

            WebRequest request = WebRequest.Create("http://www.google.com");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response == null || response.StatusCode != HttpStatusCode.OK)
                Assert.Fail();
        }
    }
}
