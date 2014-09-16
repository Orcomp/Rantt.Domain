//-----------------------------------------------------------------------
// <copyright file="IntervalGridRow.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Rantt.Domain
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The interval grid row.
    /// </summary>
    public class IntervalGridRow
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public IEnumerable<IntervalGridRow> Children { get; set; }

        /// <summary>
        /// Gets or sets the gantt index.
        /// </summary>
        public int GanttIndex { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the specific name.
        /// </summary>
        public string SpecificName { get; set; }

        /// <summary>
        /// Gets or sets the operations.
        /// </summary>
        public List<IOperation<DateTime>> Operations { get; set; }

        /// <summary>
        /// Gets or sets the calendar periods.
        /// </summary>
        public List<ICalendarPeriod> CalendarPeriods { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public IntervalGridRow Parent { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        public int Start { get; set; }

        #endregion
    }
}