// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeType.cs" company="Orcamp">
//   Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
// <summary>
//   Presents attribute type
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    /// <summary>
    /// Describes .net type
    /// </summary>
    public class AttributeType
    {
        /// <summary>
        /// Gets or sets the class name.
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the system type.
        /// </summary>
        public string SystemType { get; set; }
    }
}
