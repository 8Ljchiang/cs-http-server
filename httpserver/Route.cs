using System;
namespace Server
{
    public class Route
    {
        private string _method;
        private string _path;
        private readonly Func<in IRequest, in IResponse, out IResponse> _controller;

        public Route(string method, string path, Func<in IRequest, in IResponse, out IResponse> controller)
        {
            _method = method;
            _path = path;
            _controller = controller;
        }

        public IResponse GetResponse(IRequest req, IResponse res)
        {
            return _controller(req, res);
        }
    }
}
