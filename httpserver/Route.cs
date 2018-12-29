using System;
namespace Server
{
    public class Route
    {
        private string _method;
        private string _path;
        private string _controllerId;

        public Route(string method, string path, string controllerId)
        {
            _method = method;
            _path = path;
            _controllerId = controllerId;
        }

        public string ControllerId { get => _controllerId; set => _controllerId = value; }
        public string Path { get => _path; set => _path = value; }
        public string Method { get => _method; set => _method = value; }
    }
}
