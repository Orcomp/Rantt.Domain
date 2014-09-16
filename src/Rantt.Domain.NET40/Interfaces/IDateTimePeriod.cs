// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDateTimePeriod.cs" company="Orcomp">
//   Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
// <summary>
//   The DateTimePeriod interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain
{
    using System;

    /// <summary>
    /// The DateTimePeriod interface.
    /// </summary>
    public interface IDateTimePeriod
    {
        /// <summary>
        /// Gets the start time.
        /// </summary>
        DateTime PeriodStartTime { get; }

        /// <summary>
        /// Gets the end time.
        /// </summary>
        DateTime PeriodEndTime { get; }
    }
}