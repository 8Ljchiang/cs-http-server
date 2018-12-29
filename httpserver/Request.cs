using System;
using System.Collections.Generic;

namespace Server
{
    public class Request
    {
        private string _requestString;
        private string _method;
        private string _path;
        private string _type;
        private Dictionary<string, string> _headers;
        private string _body;

        public Request(string requestString, string method, string path, string protocol, Dictionary<string, string> headers, string body)
        {
            RequestString = requestString;
            Method = method;
            Path = path;
            Type = protocol;
            Headers = headers;
            Body = body;
        }

        public string RequestString { get => _requestString; set => _requestString = value; }
        public string Method { get => _method; set => _method = value; }
        public string Path { get => _path; set => _path = value; }
        public string Type { get => _type; set => _type = value; }
        public Dictionary<string, string> Headers { get => _headers; set => _headers = value; }
        public string Body { get => _body; set => _body = value; }
    }
}
