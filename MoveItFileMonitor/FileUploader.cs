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
        public FileUploader(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task UploadFileAsync(string filePath, string token, string homeFolderID)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            using var formData = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(File.ReadAllBytes(filePath));
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            formData.Add(fileContent, "file", Path.GetFileName(filePath));

            string uri = $"https://testserver.moveitcloud.com/api/v1/folders/{homeFolderID}/files";

            var response = await _httpClient.PostAsync(uri, formData);
            response.EnsureSuccessStatusCode();
        }
    }
}
