using System;
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

            if (IsValidMethod(routeMethod) && IsValidPath(routePath))
            {
                Route newRoute = new Route(routeMethod, routePath, controller);

                if (Routes.ContainsKey(routePath))
                {
                    AddRouteToRoutes(newRoute);
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
            return (path.Substring(0,1).Equals("/"));
        }
    }
}
