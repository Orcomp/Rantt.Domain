//-----------------------------------------------------------------------
// <copyright file="Interval.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    /// Interval class.
    /// </summary>
    public class Interval
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interval"/> class.
        /// </summary>
        public Interval()
        {
        }

        /// <summary>
        /// Gets or sets Reference IDs
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets font color.
        /// </summary>
        public FontColor FontColor { get; set; }

        /// <summary>
        /// Gets or sets color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the rectangle is disabled.
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets the row.
        /// </summary>
        public IntervalGridRow Row { get; set; }

        /// <summary>
        /// Gets or sets the interval type.
        /// </summary>
        public IntervalType IntervalType { get; set; }

        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        [TypeConverter(typeof(CrossPlatform.DateTimeConverter))]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets end date.
        /// </summary>
        [TypeConverter(typeof(CrossPlatform.DateTimeConverter))]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets attribute
        /// </summary>
        public IAttributeCollection Attributes { get; set; }
    }
}
