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
            Assert.True(dispatcher.Commands.ContainsKey(commandType));
            Assert.Equal(expectedCommandTypeCount, dispatcher.Commands.Count);
            Assert.Equal(expectedHandlerCount, dispatcher.Commands[commandType].Count);
        }

        [Fact]
        public void RegisterCommandHandler_Called2Times_AddsHandler()
        {
            // Arrange
            int expectedCommandTypeCount = 1;
            int expectedHandlerCount = 2;
            string commandType = "listen";
            Action<Dictionary<string, object>> handler = (Dictionary<string, object> payload) =>
            {

            };
            CommandDispatcher dispatcher = new CommandDispatcher();

            // Act
            dispatcher.RegisterCommandHandler(commandType, handler);
            dispatcher.RegisterCommandHandler(commandType, handler);

            // Assert
            Assert.True(dispatcher.Commands.ContainsKey(commandType));
            Assert.Equal(expectedCommandTypeCount, dispatcher.Commands.Count);
            Assert.Equal(expectedHandlerCount, dispatcher.Commands[commandType].Count);
        }

        [Fact]
        public void Process_CalledWithCommandType_HandlersAreCalled()
        {
            // Arrange
            int expectedCallCount = 2;
            int actualPort = 5000;
            string commandType = "listen";
            List<string> handlerCallHistory = new List<string>();

            Action<Dictionary<string, object>> handler1 = (Dictionary<string, object> payload) =>
            {
                string port = payload["port"].ToString();
                handlerCallHistory.Add($"handler1({port})");
            };

            void handler2(Dictionary<string, object> payload)
            {
                string port = payload["port"].ToString();
                handlerCallHistory.Add($"handler2({port})");
            }

            CommandDispatcher dispatcher = new CommandDispatcher();
            dispatcher.RegisterCommandHandler(commandType, handler1);
            dispatcher.RegisterCommandHandler(commandType, handler2);

            Dictionary<string, object> actualPayload = new Dictionary<string, object>
            {
                { "port", 5000 }
            };

            // Act
            dispatcher.Process(commandType, actualPayload);

            // Assert
            Assert.Equal(expectedCallCount, handlerCallHistory.Count);
            Assert.Equal($"handler1({actualPort})", handlerCallHistory[0]);
            Assert.Equal($"handler2({actualPort})", handlerCallHistory[1]);
        }
    }
}
