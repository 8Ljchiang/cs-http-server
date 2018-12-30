using System;
using Xunit;

namespace Server.UnitTests
{
    public class ResponseBuilderTests
    {
        [Fact]
        public void CreateResponseString_CalledWithValidResponseNoBodyText_ReturnsString()
        {
            // Arrange
            Response response = new Response();
            response.AddHeader("Access", "*/*");
            string expectedResponseString =
                "HTTP/1.1 200 OK\n" +
                "Access: */*\n" +
                "Content-Type: text/html\n";

            // Act
            string actualResponseString = ResponseBuilder.CreateResponseString(response);

            // Assert
            Assert.Equal(expectedResponseString, actualResponseString);
        }

        [Fact]
        public void CreateResponseString_CalledWithValidResponseWithBodyText_ReturnsString()
        {
            // Arrange
            string body = "{ \"response\": \"hello world\" }";
            Response response = new Response();
            response.AddHeader("Access", "*/*");
            response.Body = body;
            string expectedResponseString =
                "HTTP/1.1 200 OK\n" +
                "Access: */*\n" +
                "Content-Type: text/html\n" +
                "\n" + body;

            // Act
            string actualResponseString = ResponseBuilder.CreateResponseString(response);

            // Assert
            Assert.Equal(expectedResponseString, actualResponseString);
        }
    }
}
