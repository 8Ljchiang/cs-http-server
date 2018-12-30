using System;
namespace Server
{
    public class InvalidMethodException : Exception
    {
        public InvalidMethodException()
        {
        }

        public InvalidMethodException(string message)
            : base(message)
        {
        }

        public InvalidMethodException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
