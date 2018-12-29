using System;
namespace Server
{
    public class Route
    {
        private string _method;
        private string _path;
        private Func<Request, Response, Response> _controller;

        public Route(string method, string path, Func<Request, Response, Response> controller)
        {
            Method = method;
            Path = path;
            _controller = controller;
        }

        public string Method { get => _method; set => _method = value; }
        public string Path { get => _path; set => _path = value; }

        public Response CreateResponse(Request req, Response res)
        {
            return _controller(req, res);
        }
    }
}
