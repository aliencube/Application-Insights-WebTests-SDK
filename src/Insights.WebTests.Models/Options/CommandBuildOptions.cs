﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;
using Aliencube.Azure.Insights.WebTests.Models.Extensions;

using CommandLine;

namespace Aliencube.Azure.Insights.WebTests.Models.Options
{
    /// <summary>
    /// This represents the build options entity for command line arguments.
    /// </summary>
    public class CommandBuildOptions
    {
        /// <summary>
        /// Gets or sets the web test name.
        /// </summary>
        [Option('n', "name", Required = true, HelpText = "Web test name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the web test URL.
        /// </summary>
        [Option('u', "url", Required = true, HelpText = "Web test URL")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Options.TestType"/> value. Default is <c>TestType.UrlPingTest</c>.
        /// </summary>
        [Option('t', "type", Required = false, DefaultValue = TestType.UrlPingTest, HelpText = "Web test type")]
        public TestType TestType { get; set; }

        /// <summary>
        /// Gets or sets the authentication type. Default is <c>AuthType.None</c>.
        /// </summary>
        [Option('a', "authtype", Required = false, DefaultValue = AuthType.None, HelpText = "Authentication type")]
        public AuthType AuthType { get; set; }

        /// <summary>
        /// Gets or sets the access token value. Default is <c>String.Empty</c>.
        /// </summary>
        [Option("token", Required = false, DefaultValue = "", HelpText = "Access token")]
        public string AccessToken { get; set; }

        [HelpOption]
        public static string GetUsage()
        {
            var sb = new StringBuilder();
            sb.AppendLine("-n|--name\tNameof web test.");
            sb.AppendLine("-u|--url\tURL for web test.");
            sb.AppendLine("-t|--type\tType of web test. Default is UrlPingTest.");
            sb.AppendLine("-a|--authtype\tType of authentication. Default is None.");
            sb.AppendLine("--token\t\tAccess token value. Default is empty.");

            return sb.ToString();
        }

        /// <summary>
        /// Builds the arguments to the <see cref="CommandBuildOptions"/> object.
        /// </summary>
        /// <param name="args">List of arguments to build.</param>
        /// <returns>Returns the <see cref="CommandBuildOptions"/> object built.</returns>
        public static CommandBuildOptions Build(IEnumerable<string> args)
        {
            if (args.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(args));
            }

            var options = new CommandBuildOptions();
            if (!Parser.Default.ParseArguments(args.ToArray(), options))
            {
                throw new InvalidArgumentsException();
            }

            return options;
        }
    }
}