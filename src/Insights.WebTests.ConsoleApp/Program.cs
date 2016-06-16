using Aliencube.AdalWrapper;
using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.Azure.Insights.WebTests.Services;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

namespace Aliencube.Azure.Insights.WebTests.ConsoleApp
{
    /// <summary>
    /// This represents the main entity of the console application entry.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Executes the console application.
        /// </summary>
        /// <param name="args">List of arguments.</param>
        public static void Main(string[] args)
        {
            var options = CommandBuildOptions.Build(args);

            using (var settings = WebTestSettingsElement.CreateInstance())
            using (var context = new AuthenticationContextWrapper($"{settings.Authentication.AadInstanceUrl.TrimEnd('/')}/{settings.Authentication.TenantName}.onmicrosoft.com", false))
            using (var service = new WebTestService(settings, context))
            {
                var processed = service.ProcessAsync(options).Result;
            }
        }
    }
}