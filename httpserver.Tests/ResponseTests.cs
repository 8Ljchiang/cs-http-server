using System;
using Xunit;

namespace Server.UnitTests
{
    public class ResponseTests
    {
        [Theory]
        [InlineData("Host","localhost:5000")]
        public void AddHeader_AddingHostHeader_ShouldAddHeaderDataToDictionary(string key, string value)
        {
            Response response = new Response();

            response.AddHeader(key, value);

            Assert.True(response.Headers.ContainsKey(key));
            Assert.Equal(value, response.Headers[key]);
        }
    }
}
