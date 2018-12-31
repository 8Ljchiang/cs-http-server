using System;
using System.Collections.Generic;

namespace Server
{
    public class CommandDispatcher
    {

        Dictionary<string, List<Action<Dictionary<string, object>>>> _commands = new Dictionary<string, List<Action<Dictionary<string, object>>>>();
        public CommandDispatcher()
        {
        }

        public Dictionary<string, List<Action<Dictionary<string, object>>>> Commands { get => _commands; set => _commands = value; }

        public void Process(string commandType, Dictionary<string, object> payload)
        {
            if (ContainsCommand(commandType))
            {
                List<Action<Dictionary<string, object>>> handlers = Commands[commandType];

                foreach (var handler in handlers)
                {
                    handler(payload);
                }
            }
        }

        public void RegisterCommandHandler(string commandType, Action<Dictionary<string, object>> handler)
        {
            if (ContainsCommand(commandType))
            {
                Commands[commandType].Add(handler);
            }
            else
            {
                List<Action<Dictionary<string, object>>> handlerList = new List<Action<Dictionary<string, object>>>
                {
                    handler
                };
                Commands.Add(commandType, handlerList);
            }
        }

        private bool ContainsCommand(string commandType)
        {
            return Commands.ContainsKey(commandType);
        }
    }
}
