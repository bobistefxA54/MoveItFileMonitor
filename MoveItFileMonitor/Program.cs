using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace MoveItFileMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger logger = factory.CreateLogger("MainProgram");

            using HttpClient client = new HttpClient();
            
            try
            {
                string username = args[0];
                string password = args[1];
                string path = args[2];

                var authService = new AuthService(client);
                var fileUploader = new FileUploader(client);

                string token = await authService.GetAccessTokenAsync(username, password); // returns token successfully
                string homeFolderId = await authService.GetHomeFolderIdAsync(token); // returns homeFolderID successfully

                var fileMonitor = new FileMonitor(path, token, homeFolderId, fileUploader);
                fileMonitor.Start();


                Console.WriteLine("Monitoring folder. Press any key to exit...");
                Console.ReadKey();
            }
            catch (NullReferenceException ex)
            {
                logger.LogInformation(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
            }
        }

    }
}
