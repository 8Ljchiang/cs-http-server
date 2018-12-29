using System.Collections.Generic;

namespace Server
{
    public class Response
    {
        private string _status = "200 OK";
        private string _protocol = "HTTP/1.1";
        private string _type = "text/html";
        private string _body = "";
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        public Response()
        {
            this._headers.Add("Content-Type", Type);
        }

        public void AddHeader(string key, string value)
        {
            this._headers.Add(key, value);
        }

        public string Status { get => _status; set => _status = value; }
        public string Protocol { get => _protocol; set => _protocol = value; }
        public string Type { get => _type; set => _type = value; }
        public string Body { get => _body; set => _body = value; }
    }
}