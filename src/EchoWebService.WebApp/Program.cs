using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace EchoWebService.WebApp
{
    /// <summary>
    /// Entry point definition for the web app.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments, if any.</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creates the host builder for the web app.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>The host builder to be used to create a hosting instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
