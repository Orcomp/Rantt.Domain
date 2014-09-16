// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawResourceConfiguration.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Pure resource configuration data
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Pure resource configuration data
    /// </summary>
    [DataContract]
    public struct RawResourceConfiguration
    {
        /// <summary>
        /// The resource name
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The resource position
        /// </summary>
        [DataMember]
        public int Position { get; set; }

        /// <summary>
        /// The resource visibility
        /// </summary>
        [DataMember]
        public bool IsVisible { get; set; }
    }
}
