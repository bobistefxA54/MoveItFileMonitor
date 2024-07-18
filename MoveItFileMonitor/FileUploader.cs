using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveItFileMonitor
{
    public class FileUploader
    {
        private readonly HttpClient _httpClient;
        private readonly string? _baseUrl;
        public FileUploader(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["MoveItCloud:BaseUrl"];
        }

        public async Task UploadFileAsync(string filePath, string token, string homeFolderID)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            using var formData = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            formData.Add(fileContent, "file", Path.GetFileName(filePath));

            string uri = $"{_baseUrl}/folders/{homeFolderID}/files";

            var response = await _httpClient.PostAsync(uri, formData);
            response.EnsureSuccessStatusCode();
        }
    }
}
