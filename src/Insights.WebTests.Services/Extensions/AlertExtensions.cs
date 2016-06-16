using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Aliencube.Azure.Insights.WebTests.Models;
using Aliencube.Azure.Insights.WebTests.Models.Extensions;
using Aliencube.Azure.Insights.WebTests.Models.Serialisation;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

using Microsoft.Azure;
using Microsoft.Azure.Management.Insights.Models;

namespace Aliencube.Azure.Insights.WebTests.Services.Extensions
{
    /// <summary>
    /// This represents the extensions entity for the <see cref="RuleCreateOrUpdateParameters"/> class.
    /// </summary>
    public static class AlertExtensions
    {
        private const string AlertMetricName = "GSMT_AvRaW";

        /// <summary>
        /// Adds tags for web test alert.
        /// </summary>
        /// <param name="parameters"><see cref="RuleCreateOrUpdateParameters"/> instance.</param>
        /// <param name="webTest"><see cref="ResourceBaseExtended"/> instance representing web test resource.</param>
        /// <param name="insights"><see cref="ResourceBaseExtended"/> instance representing Application Insights resource.</param>
        /// <returns>Returns the <see cref="RuleCreateOrUpdateParameters"/> instance with tags added.</returns>
        public static RuleCreateOrUpdateParameters AddTags(this RuleCreateOrUpdateParameters parameters, ResourceBaseExtended webTest, ResourceBaseExtended insights)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (webTest == null)
            {
                throw new ArgumentNullException(nameof(webTest));
            }

            if (insights == null)
            {
                throw new ArgumentNullException(nameof(insights));
            }

            parameters.Tags.Add($"hidden-link:{insights.Id}", "Resource");
            parameters.Tags.Add($"hidden-link:{webTest.Id}", "Resource");

            return parameters;
        }

        /// <summary>
        /// Adds properties for web test alert.
        /// </summary>
        /// <param name="parameters"><see cref="RuleCreateOrUpdateParameters"/> instance.</param>
        /// <param name="name">Name of web test.</param>
        /// <param name="element"><see cref="WebTestElement"/> instance from App.config/Web.config.</param>
        /// <param name="webTest"><see cref="ResourceBaseExtended"/> instance representing web test resource.</param>
        /// <param name="insights"><see cref="ResourceBaseExtended"/> instance representing Application Insights resource.</param>
        /// <returns>Returns the <see cref="RuleCreateOrUpdateParameters"/> instance with properties added.</returns>
        public static RuleCreateOrUpdateParameters AddProperties(this RuleCreateOrUpdateParameters parameters, string name, WebTestElement element, ResourceBaseExtended webTest, ResourceBaseExtended insights)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (webTest == null)
            {
                throw new ArgumentNullException(nameof(webTest));
            }

            if (insights == null)
            {
                throw new ArgumentNullException(nameof(insights));
            }

            var alertName = $"{name}-{insights.Name}-alert";

            var action = new RuleEmailAction() { SendToServiceOwners = element.Alerts.SendAlertToAdmin };
            if (!element.Alerts.Recipients.IsNullOrEmpty())
            {
                action.CustomEmails = element.Alerts.Recipients;
            }

            var source = new RuleMetricDataSource() { MetricName = AlertMetricName, ResourceUri = webTest.Id };
            var condition = new LocationThresholdRuleCondition()
                                {
                                    DataSource = source,
                                    FailedLocationCount = element.Alerts.AlertLocationThreshold,
                                    WindowSize = TimeSpan.FromMinutes((int)element.Alerts.TestAlertFailureTimeWindow),
                                };
            var rule = new Rule()
                           {
                               Name = alertName,
                               Description = string.Empty,
                               IsEnabled = element.Alerts.IsEnabled,
                               LastUpdatedTime = DateTime.UtcNow,
                               Actions = { action },
                               Condition = condition,
                           };
            parameters.Properties = rule;

            return parameters;
        }
    }
}