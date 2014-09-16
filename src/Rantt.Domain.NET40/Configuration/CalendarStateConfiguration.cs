//-----------------------------------------------------------------------
// <copyright file="CalendarStateConfiguration.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System.Windows.Media;
    using System.Xml.Serialization;
    using Catel.Data;

    /// <summary>
    /// Class handling calendar state configuration and its change
    /// </summary>
    public class CalendarStateConfiguration : ModelBase
    {
        #region Fields
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarStateConfiguration" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="color">The color.</param>
        public CalendarStateConfiguration(string name, Color color)
        {
            IsVisible = true;
            Name = name;
            CalendarStateColor = CustomColorConverter.ColorToString(color);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarStateConfiguration"/> class.
        /// </summary>
        public CalendarStateConfiguration()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether The visibility is on
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the _name.
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets or sets the calendar state color.
        /// </summary>
        public string CalendarStateColor { get; set; }

        /// <summary>
        /// Gets color of calendar state
        /// </summary>
        /// <returns>the color</returns>
        public Color GetColor()
        {
            return CustomColorConverter.StringToColor(CalendarStateColor);
        }

        /// <summary>
        /// sets color of calendar state
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetColor(Color color)
        {
            CalendarStateColor = CustomColorConverter.ColorToString(color);
            RaisePropertyChanged("Color");
        }
        #endregion
    }
}