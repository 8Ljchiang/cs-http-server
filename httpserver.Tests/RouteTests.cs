using System;
using System.Collections.Generic;
using Xunit;

namespace Server.UnitTests
{
    public class RouteTests
    {
        [Fact]
        public void CreateResponse_WhenCalled_ShouldCallFuncDelegate()
        {
            // Arrange
            int expectedCallCount = 1;
            string expectedCallString = "controller()";
            List<string> methodCallHistory = new List<string>();

            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                methodCallHistory.Add(expectedCallString);
                return res;
            };

            Route route = new Route("GET", "/home", controller);

            // Act
            Request mockRequest = new Request();
            Response mockResponse = new Response();
            route.CreateResponse(mockRequest, mockResponse);

            // Assert
            Assert.Equal(expectedCallCount, methodCallHistory.Count);
            Assert.Equal(expectedCallString, methodCallHistory[0]);
        }
    }
}
