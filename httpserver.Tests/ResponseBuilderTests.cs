﻿using System;
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
                "Content-Type:text/html\n" +
                "Access:*/*\n";

            // Act
            string actualResponseString = ResponseBuilder.CreateResponseString(response);

            // Assert
            Assert.Equal(expectedResponseString, actualResponseString);
        }

        [Fact]
        public void CreateResponseString_CalledWithValidResponseWithBodyText_ReturnsString()
        {

        }
    }
}