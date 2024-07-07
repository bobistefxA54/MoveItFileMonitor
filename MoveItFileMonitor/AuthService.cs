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
        private const string _homeFolderId = "homeFolderID"; // ID in the property is with capital D not 'Id'
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
            var response = await _httpClient.PostAsync("https://testserver.moveitcloud.com/api/v1/token", urlEncodedCredentials);
            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Login failed. Check credentials");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(responseString);
            if (jsonResponse[_accessToken] is null)
            {
                throw new NullReferenceException("Access token is null"); // this check might not be necessary if the response has a success status code
            }
            return jsonResponse[_accessToken].ToString();

        }

        public async Task<string> GetHomeFolderIdAsync(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var user = await _httpClient.GetStringAsync("https://testserver.moveitcloud.com/api/v1/users/self");

            var jsonResponse = JObject.Parse(user);
            if (jsonResponse[_homeFolderId] is null)
            {
                throw new NullReferenceException($"Home folder ID is null"); // unclear if this is necessary since if there is a user, there has to be a home folder
            }
            return jsonResponse[_homeFolderId].ToString();
        }

    }
}
