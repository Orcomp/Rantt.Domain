// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeConfigurationChangeEventArgs.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   The Resource Configuration property changes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.EventArgs
{
    using System.Windows.Media;

    using Catel;
    using Catel.Data;

    /// <summary>
    /// The Attribute Configuration property changes.
    /// </summary>
    public class AttributeConfigurationChangeEventArgs : AdvancedPropertyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConfigurationChangeEventArgs" /> class.
        /// </summary>
        /// <param name="attributeConfiguration">The attribute configuration model.</param>
        /// <param name="propertyName">Name of the property.</param>
        public AttributeConfigurationChangeEventArgs(AttributeConfiguration attributeConfiguration, string propertyName)
            : base(attributeConfiguration, propertyName)
        {
            Argument.IsNotNull("attributeConfiguration", attributeConfiguration);

            Model = attributeConfiguration;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public AttributeConfiguration Model { get; private set; }

        /// <summary>
        /// Gets or sets attribute value for which change is specific.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets visibility of value.
        /// </summary>
        public bool Visibility { get; set; }

        /// <summary>
        /// Gets or sets color of value.
        /// </summary>
        public Color Color { get; set; }
    }
}
