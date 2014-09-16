using System;

namespace Rantt.Domain.Exceptions
{
    using System.Runtime.Serialization;
    using Entities;
    
    /// <summary>
    /// The exception for cases when type conversion is failed during reading
    /// </summary>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class TypeConversionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class.
        /// </summary>
        public TypeConversionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public TypeConversionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="inner"></param>
        public TypeConversionException(string message, Exception inner)
            : base(message, inner)
        {
        }

#if (!SILVERLIGHT)
        /// <summary>
        /// Initializes a new instance of the <see cref="Operation{T}"/> class.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        protected TypeConversionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif

        /// <summary>
        /// The field name for reading of which exception happened.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The field value caused exception.
        /// </summary>
        public string FieldValue { get; set; }
    }
}
