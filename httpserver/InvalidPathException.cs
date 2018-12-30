using System;
namespace Server
{
    public class InvalidPathException : Exception
    {
        public InvalidPathException()
        {
        }

        public InvalidPathException(string message)
            : base(message)
        {
        }

        public InvalidPathException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
