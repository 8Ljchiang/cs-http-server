using System;
using Xunit;

namespace Server.UnitTests
{
    public class RouterTests
    {
        [Fact]
        public void Use_CalledWith2DifferentMethods_Add2RoutesToRoutesDictionary()
        {
            int expectedRouteCount = 2;
            int expectedPathCount = 1;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };
            string method1 = "GET";
            string method2 = "POST";
            string path = "/home";
            Router router = new Router();

            router.Use(method1, path, controller);
            router.Use(method2, path, controller);

            Assert.True(router.Routes.ContainsKey(path));
            Assert.Equal(expectedPathCount, router.Routes.Count);
            Assert.Equal(expectedRouteCount, router.Routes[path].Count);
        }

        [Fact]
        public void Use_Called2TimesWithSameMethodAndPath_Add1RouteToRoutesDictionary()
        {
            int expectedRouteCount = 1;
            int expectedPathCount = 1;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };
            string method1 = "GET";
            string path = "/home";
            Router router = new Router();

            router.Use(method1, path, controller);
            router.Use(method1, path, controller);

            Assert.True(router.Routes.ContainsKey(path));
            Assert.Equal(expectedPathCount, router.Routes.Count);
            Assert.Equal(expectedRouteCount, router.Routes[path].Count);
        }

        [Fact]
        public void Use_CalledWith2DifferentPaths_Add1RouteForEachPathToRoutesDictionary()
        {
            int expectedRouteCount = 1;
            int expectedPathCount = 2;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };
            string method1 = "GET";
            string path1 = "/home";
            string method2 = "POST";
            string path2 = "/users";

            Router router = new Router();

            router.Use(method1, path1, controller);
            router.Use(method2, path2, controller);

            Assert.True(router.Routes.ContainsKey(path1));
            Assert.True(router.Routes.ContainsKey(path2));
            Assert.Equal(expectedPathCount, router.Routes.Count);

            Assert.Equal(expectedRouteCount, router.Routes[path1].Count);
            Assert.Equal(expectedRouteCount, router.Routes[path2].Count);
        }

        [Theory]
        [InlineData("get", "/")]
        [InlineData("put", "/")]
        [InlineData("post", "/")]
        [InlineData("delete", "/")]
        [InlineData("GET", "/")]
        [InlineData("PUT", "/")]
        [InlineData("POST", "/")]
        [InlineData("DELETE", "/")]
        public void Use_CalledWithValidMethods_Add1RouteToRoutesDictionary(string method, string path)
        {
            int expectedRouteCount = 1;
            int expectedPathCount = 1;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };

            Router router = new Router();

            router.Use(method, path, controller);

            Assert.True(router.Routes.ContainsKey(path));
            Assert.Equal(expectedPathCount, router.Routes.Count);
            Assert.Equal(expectedRouteCount, router.Routes[path].Count);
        }

        [Theory]
        [InlineData("gt", "/")]
        [InlineData("pt", "/")]
        [InlineData("post", "/")]
        [InlineData("del", "/")]
        public void Use_CalledWithInvalidMethods_NoRoutesAdded(string method, string path)
        {
            int expectedPathCount = 0;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };

            Router router = new Router();

            router.Use(method, path, controller);

            Assert.False(router.Routes.ContainsKey(path));
            Assert.Equal(expectedPathCount, router.Routes.Count);
        }
    }

}
