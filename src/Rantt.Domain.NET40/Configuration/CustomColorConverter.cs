// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomColorConverter.cs" company="Orcamp">
//   Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
// <summary>
//   Custom color converter introduced because ColorConverter class is absent in Silverlight
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// Custom color converter introduced because ColorConverter class is absent in Silverlight
    /// </summary>
    public static class CustomColorConverter
    {
        /// <summary>
        /// The color to string.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ColorToString(Color color)
        {
            return string.Format("{0};{1};{2};{3}", color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// The string to color.
        /// </summary>
        /// <param name="strColor">
        /// The s color.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// Unexpected color format in the configuration database
        /// </exception>
        public static Color StringToColor(string strColor)
        {
            string[] clrParts = strColor.Split(';');
            try
            {
                return Color.FromArgb(Convert.ToByte(clrParts[0]), Convert.ToByte(clrParts[1]), Convert.ToByte(clrParts[2]), Convert.ToByte(clrParts[3]));
            }
            catch (Exception)
            {
                throw new Exception("Unexpected color format in the configuration database");
            }
        }
    }
}
