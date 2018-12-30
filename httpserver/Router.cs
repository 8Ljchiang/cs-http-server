﻿using System;
using System.Collections.Generic;

namespace Server
{
    public class Router
    {
        private Dictionary<string, List<Route>> _routes = new Dictionary<string, List<Route>>();

        public Router() { }

        public Dictionary<string, List<Route>> Routes { get => _routes; set => _routes = value; }

        public void Use(string method, string path, Func<Request, Response, Response> controller)
        {
            string routePath = path.ToLower();
            string routeMethod = method.ToUpper();

            if (IsValidMethod(routeMethod))
            {
                if (IsValidPath(routePath))
                {
                    Route newRoute = new Route(routeMethod, routePath, controller);

                    if (Routes.ContainsKey(routePath))
                    {
                        if (GetMatchingRouteWithFind(routeMethod, routePath) == null)
                        {
                            AddRouteToRoutes(newRoute);
                        }
                        else
                        {
                            throw new DuplicateRouteException();
                        }
                    }
                    else
                    {
                        List<Route> routeList = new List<Route>
                        {
                            newRoute
                        };
                        Routes.Add(routePath, routeList);
                    }
                }
                else
                {
                    throw new InvalidPathException();
                }
            }
            else
            {
                throw new InvalidMethodException();
            }
        }

        public Response HandleRequest(Request clientRequest)
        {
            string requestProtocol = clientRequest.Protocol.Split("/")[0];
            string requestMethod = clientRequest.Method;
            string requestPath = clientRequest.Path;

            if (IsValidRequest(clientRequest) && requestProtocol.Equals("HTTP"))
            {
                if (ContainsPath(requestPath))
                {
                    switch (requestMethod)
                    {
                        case "HEAD":
                            return HandleHeadRequest(clientRequest);
                        case "OPTIONS":
                            return HandleOptionsRequest(clientRequest);
                        default:
                            if (ContainsRoute(requestMethod, requestPath))
                            {
                                Response defaultResponse = new Response();
                                Route matchedRoute = GetMatchingRouteWithFind(requestMethod, requestPath);
                                return matchedRoute.CreateResponse(clientRequest, defaultResponse);
                            }
                            else
                            {
                                return HandleMissingMethod(clientRequest);
                            }
                    }
                }
                else
                {
                    return HandleMissingPath(clientRequest);
                }
            }
            else
            {
                return HandleInvalidRequest(clientRequest);
            }
        }

        private Response HandleHeadRequest(Request clientRequest)
        {
            Response response = new Response();
            Route getRoute = GetMatchingRouteWithFind("GET", clientRequest.Path);
            getRoute.CreateResponse(clientRequest, response);
            response.Body = "";
            return response;
        }

        private Response HandleOptionsRequest(Request clientRequest)
        {
            List<string> allowedMethods = new List<string>();
            allowedMethods.Add("OPTIONS");
            allowedMethods.Add("HEAD");

            List<Route> routes = GetRoutes(clientRequest.Path);
            foreach (var route in routes)
            {
                allowedMethods.Add(route.Method);
            }

            Response response = new Response();
            string methods = String.Join(",", allowedMethods);
            response.AddHeader("Allow", methods);
            return response;
        }

        private Response HandleMissingMethod(Request clientRequest)
        {
            Response response = new Response
            {
                Status = "405 Method Not Allowed"
            };

            return response; ;
        }

        private Response HandleMissingPath(Request clientRequest)
        {
            Response response = new Response
            {
                Status = "404 Not Found"
            };

            return response;
        }

        private Response HandleInvalidRequest(Request clientRequest)
        {
            Response response = new Response
            {
                Status = "400 Bad Request"
            };

            return response;
        }

        private Route GetMatchingRoute(string method, string path)
        {
            List<Route> routes = GetRoutes(path);
            foreach (var route in routes)
            {
                if (route.Method.Equals(method))
                {
                    return route;
                }
            }
            return null;
        }

        private List<Route> GetRoutes(string path)
        {
            return Routes[path];
        }

        private Route GetMatchingRouteWithFind(string method, string path)
        {
            return GetRoutes(path).Find(route => route.Method.Equals(method));
        }

        private void AddRouteToRoutes(Route route)
        {
            if (GetMatchingRoute(route.Method, route.Path) == null)
            {
                Routes[route.Path].Add(route);
            }
        }

        private bool IsValidMethod(string method)
        {
            string[] validMethods = new string[] { "GET", "PUT", "POST", "DELETE" };

            return Array.Exists(validMethods, element => element.Equals(method));
        }

        private bool IsValidPath(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.Substring(0, 1).Equals("/"));
        }

        private bool IsValidRequest(Request request)
        {
            string requestMethod = request.Method;
            string requestPath = request.Path;

            string[] acceptedMethods = { "GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS" };

            bool hasAcceptedMethod = Array.Exists(acceptedMethods, element => element.Equals(requestMethod));
            bool hasValidPath = IsValidPath(requestPath);

            return (hasAcceptedMethod && hasValidPath);
        }

        private bool ContainsPath(string path)
        {
            return (Routes.ContainsKey(path));
        }

        private bool ContainsRoute(string method, string path)
        {
            Route route = GetMatchingRouteWithFind(method, path);
            if (route != null)
            {
                return true;
            }
            return false;
        }
    }
}