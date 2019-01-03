﻿using System;
using System.Collections.Generic;

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

            Func<Request, Response, Response> postController = (Request req, Response res) =>
            {
                res.Body = "{ response: success }";
                return res;
            };

            Router router = new Router();
            router.Use("GET", "/", getDefaultController);
            router.Use("POST", "/", postController);

            int port = 5000;

            Server server = new Server(router, false);
            void ListenFinisher(Dictionary<string, object> payload)
            {
                Console.WriteLine($"Listening on port {port}...");
            }

            server.On("listen", ListenFinisher);

            server.ListenWithTCPListener(port);
        }
    }
}
