using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;
using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.ConfigurationValueConverter;

namespace Aliencube.Azure.Insights.WebTests.Services.Settings
{
    /// <summary>
    /// This represents the configuration element for the alerts.
    /// </summary>
    public class AlertsElement : BaseConfigElement<AlertsElement>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AlertsElement"/> class.
        /// </summary>
        public AlertsElement()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AlertsElement"/> class.
        /// </summary>
        /// <param name="element"><see cref="AlertsElement"/> instance.</param>
        public AlertsElement(AlertsElement element)
        {
            this.Status = element.Status;
            this.AlertLocationThreshold = element.AlertLocationThreshold;
            this.WebTestAlertFailureTimeWindow = (int)element.TestAlertFailureTimeWindow;
            this.SendAlertToAdmin = element.SendAlertToAdmin;
            this.Recipients = element.Recipients;
            this.WebhookUrl = element.WebhookUrl;
        }

        /// <summary>
        /// Gets or sets the value indicating whether the the alert status is enabled or not.
        /// </summary>
        [ConfigurationProperty("status", IsRequired = true, DefaultValue = TestStatus.Enabled)]
        [TypeConverter(typeof(CaseInsensitiveEnumConverter<TestStatus>))]
        public virtual TestStatus Status
        {
            get { return (TestStatus)this["status"]; }
            set { this["status"] = value; }
        }

        /// <summary>
        /// Gets the value indicating whether the the alert status is enabled or not.
        /// </summary>
        public virtual bool IsEnabled => this.Status == TestStatus.Enabled;

        /// <summary>
        /// Gets or sets the thershold value for alter locations.
        /// </summary>
        [ConfigurationProperty("alertLocationThreshold", IsRequired = true, DefaultValue = 1)]
        public virtual int AlertLocationThreshold
        {
            get { return (int)this["alertLocationThreshold"]; }
            set { this["alertLocationThreshold"] = value; }
        }

        /// <summary>
        /// Sets the time period value in minute, when the failure keeps happening longer than the value specified.
        /// </summary>
        [ConfigurationProperty("alertFailureTimeWindow", IsRequired = true, DefaultValue = 5)]
        public virtual int WebTestAlertFailureTimeWindow
        {
            private get { return (int)this["alertFailureTimeWindow"]; }
            set { this["alertFailureTimeWindow"] = value; }
        }

        /// <summary>
        /// Gets the time period value in minute, when the failure keeps happening longer than the value specified.
        /// </summary>
        public virtual TestAlertFailureTimeWindow TestAlertFailureTimeWindow
        {
            get
            {
                if (!Enum.IsDefined(typeof(TestAlertFailureTimeWindow), this.WebTestAlertFailureTimeWindow))
                {
                    throw new InvalidEnumValueException();
                }

                return (TestAlertFailureTimeWindow)Enum.ToObject(typeof(TestAlertFailureTimeWindow), this.WebTestAlertFailureTimeWindow);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to sent alert emails to admin or not.
        /// </summary>
        [ConfigurationProperty("sendAlertToAdmin", IsRequired = true, DefaultValue = true)]
        public virtual bool SendAlertToAdmin
        {
            get { return (bool)this["sendAlertToAdmin"]; }
            set { this["sendAlertToAdmin"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of emails to get alerts, delimited by commas.
        /// </summary>
        [ConfigurationProperty("recipients", IsRequired = true)]
        [TypeConverter(typeof(CommaDelimitedListConverter<string>))]
        public virtual List<string> Recipients
        {
            get { return (List<string>)this["recipients"]; }
            set { this["recipients"] = value; }
        }

        /// <summary>
        /// Gets or sets the webhook URL.
        /// </summary>
        [ConfigurationProperty("webhookUrl", IsRequired = false)]
        public virtual string WebhookUrl
        {
            get { return (string)this["webhookUrl"]; }
            set { this["webhookUrl"] = value; }
        }
    }
}