using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveItFileMonitor
{
    public class AuthService
    {
        private const string _accessToken = "access_token";
        private const string _grantType = "password";
        private const string _homeFolderId = "homeFolderID";
        private readonly string? _baseUrl;
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["MoveItCloud:BaseUrl"];
        }

        public async Task<string> GetAccessTokenAsync(string username, string password)
        {
            var credentials = new Dictionary<string, string>
                {
                    { "grant_type", _grantType },
                    { "username", username },
                    { "password", password }
                };

            var urlEncodedCredentials = new FormUrlEncodedContent(credentials);
            var response = await _httpClient.PostAsync($"{_baseUrl}/token", urlEncodedCredentials);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Login failed. Check credentials");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            var accessToken = (jsonResponse[_accessToken]?.ToString()) ?? throw new NullReferenceException("Access token is null");
            return accessToken;
        }

        public async Task<string> GetHomeFolderIdAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var user = await _httpClient.GetStringAsync($"{_baseUrl}/users/self");

            var jsonResponse = JObject.Parse(user);
            var homeFolderId = jsonResponse[_homeFolderId]?.ToString() ?? throw new NullReferenceException("Home folder ID is null");
            return homeFolderId;
        }

    }
}
