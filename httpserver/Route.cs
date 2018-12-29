using System;
namespace Server
{
    public class Route
    {
        private string _method;
        private string _path;
        private readonly Func<Request, Response, Response> _controller;

        public Route(string method, string path, Func<Response, Response, Response> controller)
        {
            _method = method;
            _path = path;
            _controller = controller;
        }

        public Response CreateResponse(Request req, Response res)
        {
            return _controller(req, res);
        }
    }
}
