using System;
using Xunit;

namespace httpserver.Tests
{
    public class HelloTests
    {
        [Fact]
        public void SayHello_MethodCalled_ReturnsHelloString()
        {
            // Arrange
            Hello greeter = new Hello();
            string expected = "Hello World";

            // Act
            string actual = greeter.SayHello();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
