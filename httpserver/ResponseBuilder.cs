using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public static class ResponseBuilder
    {
        public static string CreateResponseString(Response response)
        {
            StringBuilder sb = new StringBuilder();

            string protocol = response.Protocol;
            string status = response.Status;
            string body = response.Body;
            Dictionary<string, string> headers = response.Headers;

            List<string> keys = new List<string>(headers.Keys);
            keys.Sort();

            sb.AppendLine(protocol + " " + status);

            foreach (var key in keys)
            {
                string headerField = key;
                string headerValue = headers[key];
                sb.AppendLine(headerField + ": " + headerValue);
            }

            if (!string.IsNullOrEmpty(body))
            {
                sb.Append("\n");
                sb.Append(body);
            }

            string result = sb.ToString();
            return result;
        }
    }
}
