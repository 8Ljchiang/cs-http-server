using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<Request, Response, Response> getDefaultController = (Request req, Response res) =>
            {
                res.Body = "Hello World";
                return res;
            };

            Router router = new Router();
            router.Use("GET", "/", getDefaultController);

            Server server = new Server(router);

            server.ListenWithTCPListener(5000);
        }
    }
}
