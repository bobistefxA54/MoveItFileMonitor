using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace MoveItFileMonitor
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: MoveItFileMonitor <username> <password> <path>");
                return;
            }

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton<HttpClient>()
                .AddSingleton<AuthService>()
                .AddSingleton<FileUploader>()
                .AddSingleton<FileMonitor>()
                .AddLogging(builder => builder.AddConsole())
                .BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            using HttpClient client = new();
            
            try
            {
                string username = args[0];
                string password = args[1];
                string path = args[2];

                var authService = serviceProvider.GetRequiredService<AuthService>();
                var fileUploader = serviceProvider.GetRequiredService<FileUploader>();

                string token = await authService.GetAccessTokenAsync(username, password);
                string homeFolderId = await authService.GetHomeFolderIdAsync(token);

                var fileMonitor = new FileMonitor(path, token, homeFolderId, fileUploader, logger);
                fileMonitor.Start();


                Console.WriteLine("Monitoring folder. Press any key to exit...");
                Console.ReadKey();
            }
            catch (NullReferenceException ex)
            {
                logger.LogError(ex, "A null reference error occurred: {Message}", ex.Message);
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "An HTTP request error occurred: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
            }
        }

    }
}
