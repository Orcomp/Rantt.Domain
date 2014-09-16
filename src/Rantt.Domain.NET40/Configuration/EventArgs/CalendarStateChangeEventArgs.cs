// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarStateChangeEventArgs.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   The Resource Configuration property changes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.EventArgs
{
    using System.ComponentModel;

    using Catel;

    /// <summary>
    /// The Resource Configuration property changes.
    /// </summary>
    public class CalendarStateConfigurationChangeEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarStateConfigurationChangeEventArgs" /> class.
        /// </summary>
        /// <param name="calendarStateConfiguration">The calendar state configuration model.</param>
        /// <param name="propertyName">Name of the property.</param>
        public CalendarStateConfigurationChangeEventArgs(CalendarStateConfiguration calendarStateConfiguration, string propertyName)
            : base(propertyName)
        {
            Argument.IsNotNull("calendarStateConfiguration", calendarStateConfiguration);

            Model = calendarStateConfiguration;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public CalendarStateConfiguration Model { get; private set; }
    }
}
