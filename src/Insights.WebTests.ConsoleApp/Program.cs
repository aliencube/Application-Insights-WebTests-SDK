﻿using System;

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
            Console.WriteLine("Web Test Generator for Azure Application Insights");
            Console.WriteLine();

            CommandBuildOptions options;
            try
            {
                options = CommandBuildOptions.Build(args);
            }
            catch
            {
                Console.WriteLine(CommandBuildOptions.GetUsage());
#if DEBUG
                Console.WriteLine("Press any key to complete the process");
                Console.ReadLine();
#endif
                return;
            }

            Console.WriteLine("Processing started ...");

            using (var settings = WebTestSettingsElement.CreateInstance())
            using (var context = new AuthenticationContextWrapper($"{settings.Authentication.AadInstanceUrl.TrimEnd('/')}/{settings.Authentication.TenantName}.onmicrosoft.com", false))
            using (var service = new WebTestService(settings, context))
            {
                try
                {
                    var processed = service.ProcessAsync(options).Result;
                }
                catch (AggregateException ex)
                {
                    foreach (var e in ex.InnerExceptions)
                    {
                        Console.WriteLine($"--- Exception #{ex.InnerExceptions.IndexOf(e) + 1} ---");
                        LogErrorToConsole(e);
                    }
                }
                catch (Exception ex)
                {
                    LogErrorToConsole(ex);
                }
            }

            Console.WriteLine("Processing completed");
#if DEBUG
            Console.WriteLine("Press any key to complete the process");
            Console.ReadLine();
#endif
        }

        private static void LogErrorToConsole(Exception ex)
        {
            while (true)
            {
                Console.WriteLine(ex.Message);
#if DEBUG
                Console.WriteLine(ex.StackTrace);
#endif
                if (ex.InnerException == null)
                {
                    break;
                }

                ex = ex.InnerException;
            }
        }
    }
}