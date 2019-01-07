using System;
using System.Collections.Generic;
using Xunit;

namespace Server.UnitTests
{
    public class RouteTests
    {
        [Fact]
        public void CreateResponse_WhenCalled_ShouldCallFuncDelegateAndReturnResponse()
        {
            // Arrange
            int expectedCallCount = 1;
            string expectedCallString = "controller()";
            List<string> methodCallHistory = new List<string>();

            Func<Request, Response, string, Response> controller = (Request req, Response res, string contextData) =>
            {
                methodCallHistory.Add(expectedCallString);
                return res;
            };
            Route route = new Route("GET", "/home", controller);

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Content-Type", "application/json" }
            };
            Request mockRequest = new Request("requestString", "GET", "/home", "HTTP/1.1", headers, "");
            Response mockResponse = new Response();

            // Act
            Response actual = route.CreateResponse(mockRequest, mockResponse, " ");

            // Assert
            Assert.Equal(expectedCallCount, methodCallHistory.Count);
            Assert.Equal(expectedCallString, methodCallHistory[0]);

            Assert.NotNull(actual);
            Assert.IsType<Response>(actual);
        }
    }
}
