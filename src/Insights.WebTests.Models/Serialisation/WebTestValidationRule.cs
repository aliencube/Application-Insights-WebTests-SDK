using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Aliencube.Azure.Insights.WebTests.Models.Serialisation
{
    /// <summary>
    /// This represents the web test validation rule entity to be serialised in the <see cref="WebTestConfiguration"/> class.
    /// </summary>
    public class WebTestValidationRule
    {
        private const string ValidationRuleFindText = "Microsoft.VisualStudio.TestTools.WebTesting.Rules.ValidationRuleFindText, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a";
        private const string FindText = "Find Text";
        private const string FindTextDescription = "Verifies the existence of the specified text in the response.";

        private const string FindTextKey = "FindText";
        private const string IgnoreCaseKey = "IgnoreCase";
        private const string UseRegularExpressionKey = "UseRegularExpression";
        private const string PassIfTextFoundKey = "PassIfTextFound";

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestValidationRule"/> class.
        /// </summary>
        public WebTestValidationRule()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WebTestValidationRule"/> class.
        /// </summary>
        /// <param name="text">Text to find in the validation rule.</param>
        public WebTestValidationRule(string text)
        {
            this.Classname = ValidationRuleFindText;
            this.DisplayName = FindText;
            this.Description = FindTextDescription;
            this.Level = WebTestValidationRuleValidationLevel.High;
            this.ExectuionOrder = WebTestValidationRuleExecutionOrder.BeforeDependents;
            this.RuleParameters = new List<WebTestValidationRuleParameter>()
                                      {
                                          new WebTestValidationRuleParameter(FindTextKey, text),
                                          new WebTestValidationRuleParameter(IgnoreCaseKey, Convert.ToString(false)),
                                          new WebTestValidationRuleParameter(UseRegularExpressionKey, Convert.ToString(false)),
                                          new WebTestValidationRuleParameter(PassIfTextFoundKey, Convert.ToString(true)),
                                      };
        }

        /// <summary>
        /// Gets the class name. This is always <c>Microsoft.VisualStudio.TestTools.WebTesting.Rules.ValidationRuleFindText, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</c>.
        /// </summary>
        [XmlAttribute()]
        public string Classname { get; }

        /// <summary>
        /// Gets the display name. This is always <c>Find Text</c>.
        /// </summary>
        [XmlAttribute()]
        public string DisplayName { get; }

        /// <summary>
        /// Gets the description. This is always <c>Verifies the existence of the specified text in the response.</c>.
        /// </summary>
        [XmlAttribute()]
        public string Description { get; }

        /// <summary>
        /// Gets the validation level. This is always <c>High</c>.
        /// </summary>
        [XmlAttribute()]
        public string Level { get; }

        /// <summary>
        /// Gets the execution order. This is always <c>BeforeDependents</c>.
        /// </summary>
        [XmlAttribute()]
        public string ExectuionOrder { get; }

        /// <summary>
        /// Gets the list of the <see cref="WebTestValidationRuleParameter"/> objects.
        /// </summary>
        [XmlArray("RuleParameters", IsNullable = false)]
        [XmlArrayItem("RuleParameter", IsNullable = false)]
        public List<WebTestValidationRuleParameter> RuleParameters { get; }
    }
}