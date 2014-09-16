namespace Rantt.Domain.Configuration
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The pure attribute extended configuration.
    /// </summary>
    [DataContract]
    public class RawAttributeExtendedConfiguration
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        [DataMember]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether visibility.
        /// </summary>
        [DataMember]
        public bool Visibility { get; set; }
    }
}
