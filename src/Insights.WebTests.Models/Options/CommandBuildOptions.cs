using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Gets or sets the <see cref="TestType"/> value. Default is <c>TestType.UrlPingTest</c>.
        /// </summary>
        [Option('t', "type", Required = false, DefaultValue = TestType.UrlPingTest, HelpText = "Web test type")]
        public TestType Type { get; set; }

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