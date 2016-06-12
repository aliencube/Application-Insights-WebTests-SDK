using System;
using System.Configuration;

namespace Aliencube.Azure.Insights.WebTests.ConsoleApp.Settings
{
    /// <summary>
    /// This represents the base configuration element entity. This MUST be inherited.
    /// </summary>
    /// <typeparam name="T">Type of inheriting class.</typeparam>
    public abstract class BaseConfigElement<T> : ConfigurationElement where T : ConfigurationElement
    {
        /// <summary>
        /// Clones the current instance.
        /// </summary>
        /// <returns>Returns <see cref="T"/> instance cloned.</returns>
        public T Clone()
        {
            return (T)Activator.CreateInstance(typeof(T), this);
        }
    }
}