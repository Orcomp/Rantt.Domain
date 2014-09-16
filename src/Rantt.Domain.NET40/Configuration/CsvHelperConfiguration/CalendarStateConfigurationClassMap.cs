// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarStateConfigurationClassMap.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Csv helper configuration for calendar state configuration class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.CsvHelperConfiguration
{
    using CsvHelper.Configuration;

    /// <summary>
    /// Csv helper configuration for calendar state configuration class.
    /// </summary>
    public class CalendarStateConfigurationClassMap : CsvClassMap<CalendarStateConfiguration>
    {
        public CalendarStateConfigurationClassMap()
        {
            Map(m => m.Name);
            Map(m => m.IsVisible);
            Map(m => m.CalendarStateColor);
        }
    }
}
