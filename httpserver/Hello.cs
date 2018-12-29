using System;

namespace Server
{
    public class Hello
    {
        public Hello()
        {

        }

        public string SayHello()
        {
            return "Hello World";
        }

        public string SayHello(string name)
        {
            return $"Hello {name}";
        }
    }
}
