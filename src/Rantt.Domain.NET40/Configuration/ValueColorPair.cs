//-----------------------------------------------------------------------
// <copyright file="ValueColorPair.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System.Windows.Media;

    /// <summary>
    /// Class storing value and color pair
    /// </summary>
    public class ValueColorPair
    {
        /// <summary>
        /// Gets or sets named pair category
        /// </summary>
        public string PairCategory { get; set; }

        /// <summary>
        /// Gets or sets value in the pair
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets color in the pair
        /// </summary>
        public string Color { get; set; }
    }
}
