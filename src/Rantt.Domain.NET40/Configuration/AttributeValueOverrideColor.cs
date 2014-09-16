// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeValueOverrideColor.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2014 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   AttributeValueOverrideColor class definition
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    /// <summary>
    /// AttributeValueOverrideColor class
    /// </summary>
    public class AttributeValueOverrideColor
    {
        /// <summary>
        /// The name of attribute
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// The value of attribute
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        /// The color string
        /// </summary>
        public string Color { get; set; }
    }
}
