using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Xunit.Sdk
{
    /// <summary>
    /// Exception thrown when the value is unexpectedly not of the exact given type.
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
    [Serializable]
    public class IsTypeException : AssertActualExpectedException
    {
        /// <summary>
        /// Creates a new instance of the <see cref="IsTypeException"/> class.
        /// </summary>
        /// <param name="expectedTypeName">The expected type name</param>
        /// <param name="actualTypeName">The actual type name</param>
        public IsTypeException(string expectedTypeName, string actualTypeName)
            : base(expectedTypeName, actualTypeName, "Assert.IsType() Failure") { }

        /// <inheritdoc/>
        protected IsTypeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}