//-----------------------------------------------------------------------
// <copyright file="CrossPlatform.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain
{
    /// <summary>
    /// CrossPlatform class.
    /// </summary>
    public class CrossPlatform
    {               
        /// <summary>
        /// DateTimeConverter class.
        /// </summary>
        public class DateTimeConverter :
#if SILVERLIGHT
                System.Windows.Controls.DateTimeTypeConverter
#else
                System.ComponentModel.DateTimeConverter
#endif
        {
        }
    }
}
