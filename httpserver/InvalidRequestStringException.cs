using System;
namespace Server
{
    public class InvalidRequestStringException : Exception
    {
        public InvalidRequestStringException()
        {
        }

        public InvalidRequestStringException(string message)
            : base(message)
        {
        }

        public InvalidRequestStringException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
