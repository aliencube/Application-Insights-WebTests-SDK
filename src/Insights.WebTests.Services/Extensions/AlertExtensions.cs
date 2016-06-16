using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Aliencube.Azure.Insights.WebTests.Models;
using Aliencube.Azure.Insights.WebTests.Models.Serialisation;
using Aliencube.Azure.Insights.WebTests.Services.Settings;

using Microsoft.Azure;
using Microsoft.Azure.Management.Insights.Models;

namespace Aliencube.Azure.Insights.WebTests.Services.Extensions
{
    /// <summary>
    /// This represents the extensions entity for the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    public static class AlertExtensions
    {
        private const string AlertMetricName = "GSMT_AvRaW";

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