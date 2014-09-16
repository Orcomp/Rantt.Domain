// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawCalendarStateConfiguration.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Pure base calendar state configuration
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Pure base calendar state configuration
    /// </summary>
    public class RawCalendarStateConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether The visibility is on.
        /// </summary>
        [DataMember]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        [DataMember]
        public string Color { get; set; }
    
    }
}
