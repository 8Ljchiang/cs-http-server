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
            string status = "200 OK";
            Response response = new Response();
            response.AddHeader("Access", "*/*");
            response.Status = status;
            string expectedResponseString =
                "HTTP/1.1 " + status + "\n" +
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
            string status = "200 OK";
            Response response = new Response();
            response.AddHeader("Access", "*/*");
            response.Body = body;
            response.Status = status;
            string expectedResponseString =
                "HTTP/1.1 " + status + "\n" +
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
