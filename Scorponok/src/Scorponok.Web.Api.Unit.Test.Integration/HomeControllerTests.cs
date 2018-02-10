using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using FluentAssertions;
using System.Net;

namespace Scorponok.Web.Api.Unit.Test.Integration
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void HelloWorld()
        {
            //Arrange's
            var client = new RestClient("http://localhost:53290/Home/Index");
            var request = new RestRequest() { Method = Method.GET };

            //Act's
            var response = client.Execute(request);

            //Assert's
            response.IsSuccessful.Should().Be(true);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().Be("Hello World!!!");
        }
    }
}
