// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceConfigurationChangeEventArgs.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   The property changes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.EventArgs
{
    using System.ComponentModel;

    using Catel;

    

    /// <summary>
    /// The Resource Configuration property changes.
    /// </summary>
    public class ResourceConfigurationChangeEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfigurationChangeEventArgs" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The resource configuration model.</param>
        /// <param name="propertyName">Name of the property.</param>
        public ResourceConfigurationChangeEventArgs(ResourceConfiguration resourceConfiguration, string propertyName)
            : base(propertyName)
        {
            // If resource configuration is not provided it means every resource has been changed.
            // Argument.IsNotNull("resourceConfiguration", resourceConfiguration);

            Model = resourceConfiguration;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public ResourceConfiguration Model { get; private set; }
    }
}
