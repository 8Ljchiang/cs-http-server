using System;
namespace Server
{
    public class Route
    {
        private string _method;
        private string _path;
        private readonly Func<IRequest, IResponse, IResponse> _controller;

        public Route(string method, string path, Func<IRequest, IResponse, IResponse> controller)
        {
            _method = method;
            _path = path;
            _controller = controller;
        }

        public IResponse CreateResponse(IRequest req, IResponse res)
        {
            return _controller(req, res);
        }
    }
}
