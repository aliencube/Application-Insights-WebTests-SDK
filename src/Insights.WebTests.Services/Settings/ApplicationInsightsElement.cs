using System.Configuration;

namespace Aliencube.Azure.Insights.WebTests.Services.Settings
{
    /// <summary>
    /// This represents the configuration element entity for Application Insights details.
    /// </summary>
    public class ApplicationInsightsElement : BaseConfigElement<ApplicationInsightsElement>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationInsightsElement"/> class.
        /// </summary>
        public ApplicationInsightsElement()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationInsightsElement"/> class.
        /// </summary>
        /// <param name="element"><see cref="ApplicationInsightsElement"/> instance.</param>
        public ApplicationInsightsElement(ApplicationInsightsElement element)
        {
            this.Name = element.Name;
            this.ResourceGroup = element.ResourceGroup;
            this.SubscriptionId = element.SubscriptionId;
        }

        /// <summary>
        /// Gets or sets the name of the Application Insights resource.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public virtual string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// Gets or sets the resource group.
        /// </summary>
        [ConfigurationProperty("resourceGroup", IsRequired = true)]
        public virtual string ResourceGroup
        {
            get { return (string)this["resourceGroup"]; }
            set { this["resourceGroup"] = value; }
        }

        /// <summary>
        /// Gets or sets the subscription Id.
        /// </summary>
        [ConfigurationProperty("subscriptionId", IsRequired = true)]
        public virtual string SubscriptionId
        {
            get { return (string)this["subscriptionId"]; }
            set { this["subscriptionId"] = value; }
        }
    }
}