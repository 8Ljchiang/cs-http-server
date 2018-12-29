using System;
using Xunit;

namespace Server.Tests
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

        [Theory]
        [InlineData("Mike", "Hello Mike")]
        [InlineData("Bill", "Hello Bill")]
        public void SayHello_MethodCalledWithName_ReturnsGreetingWithName(string name, string expected) 
        {
            // Arrange
            Hello greeter = new Hello();

            // Act
            string actual = greeter.SayHello(name);

            // Assert    
            Assert.Equal(expected, actual);
        }
    }
}
