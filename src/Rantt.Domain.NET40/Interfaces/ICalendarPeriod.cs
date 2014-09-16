//-----------------------------------------------------------------------
// <copyright file="ICalendarPeriod.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain
{
    using System;

    /// <summary>
    /// Interface representing a period in the calendar of a resource
    /// </summary>
    public interface ICalendarPeriod : IDateTimePeriod
    {
        /// <summary>
        /// Gets or sets resource name.
        /// </summary>
        string Resource { get; set; }

        /// <summary>
        /// Gets or sets the calendar state
        /// </summary>
        string CalendarState { get; set; }

        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        DateTime EndTime { get; set; }
    }
}
