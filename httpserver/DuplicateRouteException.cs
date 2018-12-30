using System;
namespace Server
{
    public class DuplicateRouteException : Exception
    {
        public DuplicateRouteException()
        {
        }

        public DuplicateRouteException(string message)
            : base(message)
        {
        }

        public DuplicateRouteException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
