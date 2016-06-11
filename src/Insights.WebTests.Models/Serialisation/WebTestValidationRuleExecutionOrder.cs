namespace Aliencube.Azure.Insights.WebTests.Models.Serialisation
{
    /// <summary>
    /// This specifies the validation rule execution order.
    /// </summary>
    public class WebTestValidationRuleExecutionOrder
    {
        /// <summary>
        /// Identifies the rule executed BEFORE dependents.
        /// </summary>
        public const string BeforeDependents = "BeforeDependents";

        /// <summary>
        /// Identifies the rule executed AFTER dependents.
        /// </summary>
        public const string AfterDependents = "AfterDependents";

        /// <summary>
        /// Identifies the rule executed BEFORE and AFTER dependents.
        /// </summary>
        public const string BeforeAndAfterDependents = "BeforeAndAfterDependents";
    }
}