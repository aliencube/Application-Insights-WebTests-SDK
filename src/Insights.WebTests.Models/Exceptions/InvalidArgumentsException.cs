using System;
using System.Runtime.Serialization;

namespace Aliencube.Azure.Insights.WebTests.Models.Exceptions
{
    /// <summary>
    /// This represents the exception entity for invalid HTTP status code.
    /// </summary>
    public class InvalidArgumentsException : ApplicationException
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidArgumentsException"/> class.
        /// </summary>
        public InvalidArgumentsException()
            : this("Invalid arguments")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidArgumentsException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the error. </param>
        public InvalidArgumentsException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidArgumentsException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception. </param>
        public InvalidArgumentsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InvalidArgumentsException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data. </param>
        /// <param name="context">The contextual information about the source or destination. </param>
        protected InvalidArgumentsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}