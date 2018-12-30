using System;
using Xunit;

namespace Server.UnitTests
{
    public class RequestBuilderTests
    {
        [Fact]
        public void CreateRequest_CalledWithValidData_ReturnRequest()
        {
            // Arrange
            int expectedHeaderCount = 3;
            string method = "GET";
            string path = "/";
            string protocol = "HTTP/1.1";
            string body = "";
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;

            // Act
            Request actualRequest = RequestBuilder.CreateRequest(requestString);

            // Assert
            Assert.NotNull(actualRequest);
            Assert.IsType<Request>(actualRequest);

            Assert.Equal(method, actualRequest.Method);
            Assert.Equal(path, actualRequest.Path);
            Assert.Equal(protocol, actualRequest.Protocol);
            Assert.Equal(body, actualRequest.Body);

            Assert.Equal(expectedHeaderCount, actualRequest.Headers.Count);
            Assert.True(actualRequest.Headers.ContainsKey("Host"));
            Assert.Equal("localhost:5000", actualRequest.Headers["Host"]);
            Assert.True(actualRequest.Headers.ContainsKey("User-Agent"));
            Assert.Equal("xUnit/1.0", actualRequest.Headers["User-Agent"]);
            Assert.True(actualRequest.Headers.ContainsKey("Accept"));
            Assert.Equal("*/*", actualRequest.Headers["Accept"]);
        }

        [Theory]
        [InlineData("INVALID", "/", "HTTP/1.1")]
        [InlineData("", "/", "HTTP/1.1")]
        [InlineData("GET", "", "HTTP/1.1")]
        [InlineData("GET", "/", "INVALID/1.1")]
        [InlineData("", "", "")]
        public void CreateRequest_CalledWithInvalidData_ThrowsInvalidRequestStringException(string method, string path, string protocol)
        {
            // Arrange   
            string body = "";
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;

            // Act & Assert
            Assert.Throws<InvalidRequestStringException>(() => RequestBuilder.CreateRequest(requestString));
        }
    }
}
