using System.Configuration;

namespace Aliencube.Azure.Insights.WebTests.ConsoleApp.Settings
{
    /// <summary>
    /// This represents the configuration element entity for authentication.
    /// </summary>
    public sealed class AuthenticationElement : BaseConfigElement<AuthenticationElement>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AuthenticationElement"/> class.
        /// </summary>
        public AuthenticationElement()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AuthenticationElement"/> class.
        /// </summary>
        /// <param name="element"><see cref="AuthenticationElement"/> instance.</param>
        public AuthenticationElement(AuthenticationElement element)
        {
            this.ClientId = element.ClientId;
            this.ClientSecret = element.ClientSecret;
            this.TenantName = element.TenantName;
            this.AadInstanceUrl = element.AadInstanceUrl;
            this.ManagementInstanceUrl = element.ManagementInstanceUrl;
        }

        /// <summary>
        /// Gets or sets the client Id.
        /// </summary>
        [ConfigurationProperty("clientId", IsRequired = true)]
        public string ClientId
        {
            get { return (string)this["clientId"]; }
            set { this["clientId"] = value; } 
        }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        [ConfigurationProperty("clientSecret", IsRequired = true)]
        public string ClientSecret
        {
            get { return (string)this["clientSecret"]; }
            set { this["clientSecret"] = value; }
        }

        /// <summary>
        /// Gets or sets the tenant name.
        /// </summary>
        [ConfigurationProperty("tenantName", IsRequired = true)]
        public string TenantName
        {
            get { return (string)this["tenantName"]; }
            set { this["tenantName"] = value; }
        }

        /// <summary>
        /// Gets or sets the Azure AD instance URL.
        /// </summary>
        [ConfigurationProperty("aadInstance", IsRequired = true)]
        public string AadInstanceUrl
        {
            get { return (string)this["aadInstance"]; }
            set { this["aadInstance"] = value; }
        }

        /// <summary>
        /// Gets or sets the Azure management instance URL.
        /// </summary>
        [ConfigurationProperty("managementInstance", IsRequired = true)]
        public string ManagementInstanceUrl
        {
            get { return (string)this["managementInstance"]; }
            set { this["managementInstance"] = value; }
        }
    }
}