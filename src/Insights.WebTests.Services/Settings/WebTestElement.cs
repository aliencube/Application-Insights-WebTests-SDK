using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;
using Aliencube.Azure.Insights.WebTests.Models.Options;
using Aliencube.ConfigurationConverters;

namespace Aliencube.Azure.Insights.WebTests.Services.Settings
{
    /// <summary>
    /// This represents the configuration element entity for web test.
    /// </summary>
    public class WebTestElement : BaseConfigElement<WebTestElement>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestElement"/> class.
        /// </summary>
        public WebTestElement()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestElement"/> class.
        /// </summary>
        /// <param name="element"><see cref="WebTestElement"/> instance.</param>
        public WebTestElement(WebTestElement element)
        {
            this.TestType = element.TestType;
            this.Status = element.Status;
            this.WebTestFrequency = (int)element.Frequency;
            this.ParseDependentRequests = element.ParseDependentRequests;
            this.RetriesForWebTestFailure = element.RetriesForWebTestFailure;
            this.TestLocations = element.TestLocations;
            this.SuccessCriteria = element.SuccessCriteria.Clone();
            this.Alerts = element.Alerts.Clone();
        }

        /// <summary>
        /// Gets or sets the test type.
        /// </summary>
        [ConfigurationProperty("testType", IsRequired = true, DefaultValue = TestType.UrlPingTest)]
        [TypeConverter(typeof(CaseInsensitiveEnumConverter<TestType>))]
        public virtual TestType TestType
        {
            get { return (TestType)this["testType"]; }
            set { this["testType"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the the web test status is enabled or not.
        /// </summary>
        [ConfigurationProperty("status", IsRequired = true, DefaultValue = TestStatus.Enabled)]
        [TypeConverter(typeof(CaseInsensitiveEnumConverter<TestStatus>))]
        public virtual TestStatus Status
        {
            get { return (TestStatus)this["status"]; }
            set { this["status"] = value; }
        }

        /// <summary>
        /// Sets the test frequency in minutes.
        /// </summary>
        [ConfigurationProperty("frequency", IsRequired = true, DefaultValue = 5)]
        public virtual int WebTestFrequency
        {
            private get { return (int)this["frequency"]; }
            set { this["frequency"] = value; }
        }

        /// <summary>
        /// Gets the test frequency in minutes converted from Web/App.config.
        /// </summary>
        public virtual TestFrequency Frequency
        {
            get
            {
                if (!Enum.IsDefined(typeof(TestFrequency), this.WebTestFrequency))
                {
                    throw new InvalidEnumValueException();
                }

                return (TestFrequency)Enum.ToObject(typeof(TestFrequency), this.WebTestFrequency);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to parse dependent requrests or not.
        /// </summary>
        [ConfigurationProperty("parseDependentRequests", IsRequired = true, DefaultValue = true)]
        public virtual bool ParseDependentRequests
        {
            get { return (bool)this["parseDependentRequests"]; }
            set { this["parseDependentRequests"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to enable retries when failure.
        /// </summary>
        [ConfigurationProperty("retriesForWebTestFailure", IsRequired = true, DefaultValue = RetriesForWebTestFailure.Enable)]
        [TypeConverter(typeof(CaseInsensitiveEnumConverter<RetriesForWebTestFailure>))]
        public virtual RetriesForWebTestFailure RetriesForWebTestFailure
        {
            get { return (RetriesForWebTestFailure)this["retriesForWebTestFailure"]; }
            set { this["retriesForWebTestFailure"] = value; }
        }

        /// <summary>
        /// Gets or sets the list of test locations delimited by commas.
        /// </summary>
        [ConfigurationProperty("testLocations", IsRequired = true)]
        [TypeConverter(typeof(CommaDelimitedListConverter<TestLocations>))]
        public virtual List<TestLocations> TestLocations
        {
            get { return (List<TestLocations>)this["testLocations"]; }
            set { this["testLocations"] = value; }
        }

        /// <summary>
        /// Gets or sets the success criteria.
        /// </summary>
        [ConfigurationProperty("successCriteria", IsRequired = true)]
        public virtual SucessCriteriaElement SuccessCriteria
        {
            get { return (SucessCriteriaElement)this["successCriteria"]; }
            set { this["successCriteria"] = value; }
        }

        /// <summary>
        /// Gets or sets the success criteria.
        /// </summary>
        [ConfigurationProperty("alerts", IsRequired = true)]
        public virtual AlertsElement Alerts
        {
            get { return (AlertsElement)this["alerts"]; }
            set { this["alerts"] = value; }
        }
    }
}