using Moq;
using Moq.Protected;
using System.Net;

namespace MoveItFileMonitor.test
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<HttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;
        private AuthService authService;

        [TestInitialize]
        public void Initialize()
        {
            mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
            authService = new AuthService(httpClient);
        }

        [TestMethod]
        public void GetAccessTokenAsync_WithValidCredentials_ReturnsValidToken()
        {
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"access_token\":\"valid_token\"}")
                });

            var token = authService.GetAccessTokenAsync("username", "password");

            Assert.AreEqual("valid_token", token.Result);
        }

        [TestMethod]
        public void GetAccessTokenAsync_WithFailedSuccessStatusCode_ThrowsHttpRequestException()
        {
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Unauthorized
                });

            Assert.ThrowsExceptionAsync<HttpRequestException>(() => authService.GetAccessTokenAsync("invalid_username", "invalid_password"));
        }

        [TestMethod]
        public void GetAccessTokenAsync_WithNullJsonResponse_ThrowsNullReferenceException()
        {
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    Content = null
                });

            Assert.ThrowsExceptionAsync<NullReferenceException>(() => authService.GetAccessTokenAsync("invalid_username", "invalid_password"));
        }

        [TestMethod]
        public void GetHomeFolderIdAsync_WithNullJsonResponse_ThrowsNullReferenceException()
        {
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    Content = null
                });

            Assert.ThrowsExceptionAsync<NullReferenceException>(() => authService.GetHomeFolderIdAsync("invalid_access_token"));
        }
    }
}