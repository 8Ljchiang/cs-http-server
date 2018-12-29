using System;
using System.Collections.Generic;

namespace Server
{
    public class Router
    {
        private Dictionary<string, List<Route>> routes = new Dictionary<string, List<Route>>();

        public Router() {}

        public void use(string method, string path, Func<IResponse, IRequest, IReponse> contoller)
        {
            Route newRoute = new Route(path, method, controller);

            if (routes.ContainsKey(path))
            {
                List<Route> routeList = new List<Route>
                {
                    newRoute
                };
            }
            else
            {
                List<Route> routeList = routes[path];
                routeList.Add(newRoute);
            }
        }
    }
}
