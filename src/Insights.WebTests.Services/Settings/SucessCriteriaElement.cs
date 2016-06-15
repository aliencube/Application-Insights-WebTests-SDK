using System;
using System.Configuration;

using Aliencube.Azure.Insights.WebTests.Models.Exceptions;
using Aliencube.Azure.Insights.WebTests.Models.Options;

namespace Aliencube.Azure.Insights.WebTests.Services.Settings
{
    /// <summary>
    /// This represents the configuration element for the success criteria.
    /// </summary>
    public class SucessCriteriaElement : BaseConfigElement<SucessCriteriaElement>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SucessCriteriaElement"/> class.
        /// </summary>
        public SucessCriteriaElement()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SucessCriteriaElement"/> class.
        /// </summary>
        /// <param name="element"><see cref="SucessCriteriaElement"/> instance.</param>
        public SucessCriteriaElement(SucessCriteriaElement element)
        {
            this.WebTestTimeout = (int)element.Timeout;
            this.RequireHttpResponse = element.RequireHttpResponse;
            this.StatusCodeMustEqualTo = element.StatusCodeMustEqualTo;
            this.RequireContentMatch = element.RequireContentMatch;
            this.ContentMustContain = element.ContentMustContain;
        }

        /// <summary>
        /// Sets the test timeout in secondes.
        /// </summary>
        [ConfigurationProperty("timeout", IsRequired = true, DefaultValue = 120)]
        public virtual int WebTestTimeout
        {
            private get { return (int)this["timeout"]; }
            set { this["timeout"] = value; }
        }

        /// <summary>
        /// Gets the test timeout in seconds.
        /// </summary>
        public virtual TestTimeout Timeout
        {
            get
            {
                if (!Enum.IsDefined(typeof(TestTimeout), this.WebTestTimeout))
                {
                    throw new InvalidEnumValueException();
                }

                return (TestTimeout)Enum.ToObject(typeof(TestTimeout), this.WebTestTimeout);
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to require HTTP response or not.
        /// </summary>
        [ConfigurationProperty("requireHttpResponse", IsRequired = true, DefaultValue = true)]
        public virtual bool RequireHttpResponse
        {
            get { return (bool)this["requireHttpResponse"]; }
            set { this["requireHttpResponse"] = value; }
        }

        /// <summary>
        /// Gets or sets the status code expected.
        /// </summary>
        [ConfigurationProperty("statusCodeMustEqual", IsRequired = true, DefaultValue = 200)]
        public virtual int StatusCodeMustEqualTo
        {
            get { return (int)this["statusCodeMustEqual"]; }
            set { this["statusCodeMustEqual"] = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether to require response body content to match.
        /// </summary>
        [ConfigurationProperty("requireContentMatch", IsRequired = true, DefaultValue = false)]
        public virtual bool RequireContentMatch
        {
            get { return (bool)this["requireContentMatch"]; }
            set { this["requireContentMatch"] = value; }
        }

        /// <summary>
        /// Gets or sets the content to match.
        /// </summary>
        [ConfigurationProperty("contentMustContain", IsRequired = false)]
        public virtual string ContentMustContain
        {
            get { return (string)this["contentMustContain"]; }
            set { this["contentMustContain"] = value; }
        }
    }
}