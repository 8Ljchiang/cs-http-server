using System;
using System.Collections.Generic;
using Xunit;

namespace Server.UnitTests
{
    public class CommandDispatcherTests
    {
        [Fact]
        public void RegisterCommandHandler_CalledOnce_AddsHandler()
        {
            // Arrange
            int expectedCommandTypeCount = 1;
            int expectedHandlerCount = 1;
            string commandType = "listen";
            Action<Dictionary<string, object>> handler = (Dictionary<string, object> payload) =>
            { 

            };
            CommandDispatcher dispatcher = new CommandDispatcher();

            // Act
            dispatcher.RegisterCommandHandler(commandType, handler);

            // Assert
            Assert.Equal(expectedCommandTypeCount, dispatcher.Commands.Count);
            Assert.Equal(expectedHandlerCount, dispatcher.Commands[commandType].Count);
        }
    }
}
