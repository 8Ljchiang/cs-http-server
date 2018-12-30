using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public static class RequestBuilder
    {
        public static Request CreateRequest(string data)
        {
            if (IsValidRequestString(data.Trim()))
            {
                string[] requestLines = ConvertRequestStringToArray(data.Trim());
                string[] firstLineParts = requestLines[0].Split(" ");

                string method = firstLineParts[0];
                string path = firstLineParts[1];
                string protocol = firstLineParts[2];
                string body = GetBody(requestLines);
                Dictionary<string, string> headersDict = GetHeaderDictionary(requestLines);

                Request request = new Request(data, method, path, protocol, headersDict, body);
                return request;
            }
            else
            {
                 throw new InvalidRequestStringException();
            }
        }

        private static string[] ConvertRequestStringToArray(string requestString)
        {
            if (!string.IsNullOrEmpty(requestString))
            {
                return requestString.Split("\n");
            }
            return null;
        }

        private static bool IsValidRequestString(string requestString)
        {
            string[] requestLines = ConvertRequestStringToArray(requestString);
            if (requestLines != null && requestLines.Length > 1)
            {
                string[] firstLineParts = requestLines[0].Split(" ");

                if (firstLineParts.Length == 3)
                {
                    string method = firstLineParts[0];
                    string path = firstLineParts[1];
                    string protocol = firstLineParts[2].Split("/")[0];

                    string[] validMethods = { "GET", "PUT", "POST", "DELETE", "HEAD", "OPTIONS" };

                    if (Array.Exists(validMethods, (m) => m.Equals(method)) && !string.IsNullOrEmpty(path) && path.Substring(0, 1).Equals("/") && protocol.Equals("HTTP"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsDividerLine(string line)
        {
            if (line.Equals("\n") || line.Length < 2 || line.Equals(" \n"))
            {
                return true;
            }
            return false;
        }

        private static string GetBody(string[] requestStringLines)
        {
            StringBuilder sb = new StringBuilder();

            if (requestStringLines.Length > 0)
            {
                bool isBodyStarted = false;

                for (int i = 1; i < requestStringLines.Length; i++)
                {
                    string line = requestStringLines[i];

                    if (isBodyStarted)
                    {
                        sb.Append(line + "\n");
                    }

                    if (IsDividerLine(line))
                    {
                        isBodyStarted = true;
                    }
                }

            }
            return sb.ToString();
        }

        private static Dictionary<string, string> GetHeaderDictionary(string[] requestStringLines)
        {
            Dictionary<string, string> headerDictionary = new Dictionary<string, string>();
            int lineIndex = 1;

            do
            {
                string[] lineParts = requestStringLines[lineIndex].Split(":");
                string[] subsetLineParts = new string[lineParts.Length - 1];

                Array.Copy(lineParts, 1, subsetLineParts, 0, subsetLineParts.Length);
                string headerKey = lineParts[0].Trim();
                string headerValue = string.Join(":", subsetLineParts).Trim();

                headerDictionary.Add(headerKey, headerValue);

                lineIndex++;
            } while (lineIndex < requestStringLines.Length && !IsDividerLine(requestStringLines[lineIndex]));

            return headerDictionary;
        }
    }
}
