using System;
using System.Collections.Generic;
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

            Assert.Throws<DuplicateRouteException>(() => router.Use(method1, path, controller));

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
        [InlineData("pst", "/")]
        [InlineData("del", "/")]
        public void Use_CalledWithInvalidMethod_NoRoutesAdded(string method, string path)
        {
            int expectedPathCount = 0;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };

            Router router = new Router();

            Assert.Throws<InvalidMethodException>(() => router.Use(method, path, controller));
            Assert.False(router.Routes.ContainsKey(path));
            Assert.Equal(expectedPathCount, router.Routes.Count);
        }

        [Theory]
        [InlineData("GET", "@invalid")]
        [InlineData("GET", "invalid")]
        [InlineData("GET", "#invalid")]
        public void Use_CalledWithInvalidPath_NoRoutesAdded(string method, string path)
        {
            int expectedPathCount = 0;
            Func<Request, Response, Response> controller = (Request req, Response res) =>
            {
                return res;
            };

            Router router = new Router();

            Assert.Throws<InvalidPathException>(() => router.Use(method, path, controller));
            Assert.False(router.Routes.ContainsKey(path));
            Assert.Equal(expectedPathCount, router.Routes.Count);
        }

        [Fact]
        public void HandleRequest_CalledWithMultipleRoutesRegistered_ReturnsAppropriateResponse()
        {
            // Arrange 
            // 1. Create multiple handlers.
            string homeRoute = "/";
            string usersRoute = "/users";
            string getMethod = "GET";
            string postMethod = "POST";
            string protocol = "HTTP/1.1";
            string successStatus = "200 OK";
            string getHomeHeaderValue = "GetHomeRouteHandler";
            string postHomeHeaderValue = "PostHomeRouteHandler";
            string getUsersHeaderValue = "GetUsersRouteHandler";
            string postUsersHeaderValue = "PostUsersRouteHandler";

            Router router = new Router();

            const string Key = "Handler-Name";

            string getHomeResponseBody = "{ \"data\": \"some-info\" }";
            string getUsersResponseBody = "{ \"data\": [{ \"username\": \"abc123\" }, { \"username\": \"abc124\" }] }";

            Response GetHomeRouteHandler(Request req, Response res)
            {
                res.AddHeader(Key, getHomeHeaderValue);
                res.Body = getHomeResponseBody;
                return res;
            }

            Response PostHomeRouteHandler(Request req, Response res)
            {
                res.AddHeader(Key, postHomeHeaderValue);
                res.Body = req.Body;
                return res;
            }

            Response GetUsersRouteHandler(Request req, Response res)
            {
                res.AddHeader(Key, getUsersHeaderValue);
                res.Body = getUsersResponseBody;
                return res;
            }

            Response PostUsersRouteHandler(Request req, Response res)
            {
                res.AddHeader(Key, postUsersHeaderValue);
                res.Body = req.Body;
                return res;
            }

            // 2. Register handlers to the router.
            router.Use(getMethod, homeRoute, GetHomeRouteHandler);
            router.Use(postMethod, homeRoute, PostHomeRouteHandler);
            router.Use(getMethod, usersRoute, GetUsersRouteHandler);
            router.Use(postMethod, usersRoute, PostUsersRouteHandler);

            // 3. Create Requests.
            Dictionary<string, string> headers = new Dictionary<string, string>();
            string postHomeBody = "{ \"message\": \"Hello World\" }";
            string postUsersBody = "{ \"username\": \"abc123\" }";
            Request getHomeRequest = new Request("", getMethod, homeRoute, protocol, headers, "");
            Request postHomeRequest = new Request("", postMethod, homeRoute, protocol, headers, postHomeBody);
            Request getUsersRequest = new Request("", getMethod, usersRoute, protocol, headers, "");
            Request postUsersRequest = new Request("", postMethod, usersRoute, protocol, headers, postUsersBody);

            // Act
            // 1. Process 3 requests that correspond to the three different handlers.
            Response actualGetHomeResponse = router.HandleRequest(getHomeRequest);
            Response actualPostHomeResponse = router.HandleRequest(postHomeRequest);
            Response actualGetUsersReponse = router.HandleRequest(getUsersRequest);
            Response actualPostUsersResponse = router.HandleRequest(postUsersRequest);

            // Assert
            // 1. Assert that each of the 3 requests return responses that they should.
            // 1a. Check that all responses are not null.
            Assert.NotNull(actualGetHomeResponse);
            Assert.NotNull(actualPostHomeResponse);
            Assert.NotNull(actualGetUsersReponse);
            Assert.NotNull(actualPostUsersResponse);

            // 1b. Check that all responses are of the type Response.
            Assert.IsType<Response>(actualGetHomeResponse);
            Assert.IsType<Response>(actualPostHomeResponse);
            Assert.IsType<Response>(actualGetUsersReponse);
            Assert.IsType<Response>(actualPostUsersResponse);

            // 1c. Check that the Get / response is correct.
            Assert.Equal(successStatus, actualGetHomeResponse.Status);
            Assert.Equal(protocol, actualGetHomeResponse.Protocol);
            Assert.Equal(getHomeResponseBody, actualGetHomeResponse.Body);
            Assert.Equal(getHomeHeaderValue, actualGetHomeResponse.Headers[Key]);

            // 1d. Check that the Post / response is correct.
            Assert.Equal(successStatus, actualPostHomeResponse.Status);
            Assert.Equal(protocol, actualPostHomeResponse.Protocol);
            Assert.Equal(postHomeBody, actualPostHomeResponse.Body);
            Assert.Equal(postHomeHeaderValue, actualPostHomeResponse.Headers[Key]);

            // 1e. Check that the Get /users response is correct.
            Assert.Equal(successStatus, actualGetUsersReponse.Status);
            Assert.Equal(protocol, actualGetUsersReponse.Protocol);
            Assert.Equal(getUsersResponseBody, actualGetUsersReponse.Body);
            Assert.Equal(getUsersHeaderValue, actualGetUsersReponse.Headers[Key]);

            // 1f. Check that the Post /users response is correct.
            Assert.Equal(successStatus, actualPostUsersResponse.Status);
            Assert.Equal(protocol, actualPostUsersResponse.Protocol);
            Assert.Equal(postUsersBody, actualPostUsersResponse.Body);
            Assert.Equal(postUsersHeaderValue, actualPostUsersResponse.Headers[Key]);
        }

        [Theory]
        [InlineData("GET", "/", "HTTP/1.1", "")]
        [InlineData("PUT", "/", "HTTP/1.1", "")]
        [InlineData("POST", "/", "HTTP/1.1", "")]
        [InlineData("DELETE", "/", "HTTP/1.1", "")]
        public void HandleRequest_CalledWithValidRequest_ReturnsValidResponse(string method, string path, string protocol, string body)
        {
            // Arrange
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;
            Dictionary<string, string> headerDict = new Dictionary<string, string>();
            headerDict.Add("Host", "localhost:5000");
            headerDict.Add("User-Agent", "xUnit/1.0");
            headerDict.Add("Accept", "*/*");

            Request request = new Request(requestString, method, path, protocol, headerDict, body);
            Response defaultResponse = new Response();

            Response controller(Request req, Response res)
            {
                return defaultResponse;
            }
            Router router = new Router();
            router.Use(method, path, controller);

            // Act
            Response actualResponse = router.HandleRequest(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<Response>(actualResponse);
            Assert.Equal(defaultResponse.Body, actualResponse.Body);
            Assert.Equal(defaultResponse.Status, actualResponse.Status);
            Assert.Equal(defaultResponse.Protocol, actualResponse.Protocol);
        }

        [Theory]
        [InlineData("INVALID", "/", "HTTP/1.1", "")]
        [InlineData("GET", "INVALID", "HTTP/1.1", "")]
        [InlineData("GET", "/", "INVALID/1.1", "")]
        public void HandleRequest_CalledWithInvalidRequest_ReturnsInvalidRequestResponse(string method, string path, string protocol, string body)
        {
            // Arrange
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;
            Dictionary<string, string> headerDict = new Dictionary<string, string>
            {
                { "Host", "localhost:5000" },
                { "User-Agent", "xUnit/1.0" },
                { "Accept", "*/*" }
            };

            Request request = new Request(requestString, method, path, protocol, headerDict, body);
            Response expectedResponse = new Response
            {
                Status = "400 Bad Request"
            };

            Response controller(Request req, Response res)
            {
                return new Response();
            }
            Router router = new Router();
            router.Use("GET", "/", controller);

            // Act
            Response actualResponse = router.HandleRequest(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<Response>(actualResponse);
            Assert.Equal(expectedResponse.Body, actualResponse.Body);
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.Protocol, actualResponse.Protocol);
        }

        [Fact]
        public void HandleRequest_CalledWithValidOptionsRequest_ReturnsResponseWithAllowedMethodsHeader()
        {
            // Arrange
            string method = "OPTIONS";
            string path = "/";
            string protocol = "HTTP/1.1";
            string body = "";
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;
            Dictionary<string, string> headerDict = new Dictionary<string, string>
            {
                { "Host", "localhost:5000" },
                { "User-Agent", "xUnit/1.0" },
                { "Accept", "*/*" }
            };

            Request request = new Request(requestString, method, path, protocol, headerDict, body);
            Response expectedResponse = new Response
            {
                Status = "200 OK",
            };

            Response controller(Request req, Response res)
            {
                return new Response();
            }
            Router router = new Router();
            router.Use("GET", path, controller);
            router.Use("PUT", path, controller);
            router.Use("POST", path, controller);
            router.Use("DELETE", path, controller);

            // Act
            Response actualResponse = router.HandleRequest(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<Response>(actualResponse);
            Assert.Equal(expectedResponse.Body, actualResponse.Body);
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.Protocol, actualResponse.Protocol);

            Assert.True(actualResponse.Headers.ContainsKey("Allow"));

            string[] methods = actualResponse.Headers["Allow"].Split(",");

            Assert.Equal(6, methods.Length);
            Assert.True(Array.Exists(methods, (m) => m.Equals("GET")));
            Assert.True(Array.Exists(methods, (m) => m.Equals("PUT")));
            Assert.True(Array.Exists(methods, (m) => m.Equals("POST")));
            Assert.True(Array.Exists(methods, (m) => m.Equals("DELETE")));
            Assert.True(Array.Exists(methods, (m) => m.Equals("HEAD")));
            Assert.True(Array.Exists(methods, (m) => m.Equals("OPTIONS")));
        }

        [Fact]
        public void HandleRequest_CalledWithValidHeadRequest_ReturnsResponseWithGetHeaders()
        {
            // Arrange
            string method = "HEAD";
            string path = "/";
            string protocol = "HTTP/1.1";
            string body = "";
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;
            Dictionary<string, string> headerDict = new Dictionary<string, string>
            {
                { "Host", "localhost:5000" },
                { "User-Agent", "xUnit/1.0" },
                { "Accept", "*/*" }
            };

            Request request = new Request(requestString, method, path, protocol, headerDict, body);
            Response expectedResponse = new Response
            {
                Status = "200 OK",
                Body = ""
            };

            Response controller(Request req, Response res)
            {
                return new Response();
            }
            Router router = new Router();
            router.Use("GET", path, controller);

            // Act
            Response actualResponse = router.HandleRequest(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<Response>(actualResponse);
            Assert.Equal(expectedResponse.Body, actualResponse.Body);
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.Protocol, actualResponse.Protocol);

            Assert.True(actualResponse.Headers.ContainsKey("Content-Type"));
            Assert.Equal("text/html", actualResponse.Headers["Content-Type"]);
        }

        [Fact]
        public void HandleRequest_CalledWithValidRequest_ReturnsMissingMethodResponse()
        {
            // Arrange
            string method = "POST";
            string path = "/";
            string protocol = "HTTP/1.1";
            string body = "";
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;
            Dictionary<string, string> headerDict = new Dictionary<string, string>
            {
                { "Host", "localhost:5000" },
                { "User-Agent", "xUnit/1.0" },
                { "Accept", "*/*" }
            };

            Request request = new Request(requestString, method, path, protocol, headerDict, body);
            Response expectedResponse = new Response
            {
                Status = "405 Method Not Allowed"
            };

            Response controller(Request req, Response res)
            {
                return new Response();
            }
            Router router = new Router();
            router.Use("GET", path, controller);

            // Act
            Response actualResponse = router.HandleRequest(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<Response>(actualResponse);
            Assert.Equal(expectedResponse.Body, actualResponse.Body);
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.Protocol, actualResponse.Protocol);
        }

        [Fact] 
        public void HandleRequest_CalledWithValidRequest_ReturnsMissingPathResponse()
        {
            // Arrange
            string method = "GET";
            string path = "/";
            string protocol = "HTTP/1.1";
            string body = "";
            string headers = "Host: localhost:5000\n" +
                "User-Agent: xUnit/1.0\n" +
                "Accept: */*\n";
            string requestString = method + " " + path + " " + protocol + "\n"
                + headers + "\n"
                + "\n" + body;
            Dictionary<string, string> headerDict = new Dictionary<string, string>
            {
                { "Host", "localhost:5000" },
                { "User-Agent", "xUnit/1.0" },
                { "Accept", "*/*" }
            };

            Request request = new Request(requestString, method, path, protocol, headerDict, body);
            Response expectedResponse = new Response
            {
                Status = "404 Not Found"
            };

            Response controller(Request req, Response res)
            {
                return new Response();
            }
            Router router = new Router();
            router.Use(method, "/someOtherPath", controller);

            // Act
            Response actualResponse = router.HandleRequest(request);

            // Assert
            Assert.NotNull(actualResponse);
            Assert.IsType<Response>(actualResponse);
            Assert.Equal(expectedResponse.Body, actualResponse.Body);
            Assert.Equal(expectedResponse.Status, actualResponse.Status);
            Assert.Equal(expectedResponse.Protocol, actualResponse.Protocol);
        }
    }
}
