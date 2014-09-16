//-----------------------------------------------------------------------
// <copyright file="IOperation.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain
{
    using System;
    using System.Collections.Generic;
    using CsvHelper;

    /// <summary>
    /// Represents an operation on a resource
    /// </summary>
    /// <typeparam name="T">The type of the operation</typeparam>
    public interface IOperation<T> : IDateTimePeriod
    {
        /// <summary>
        /// Gets resource name.
        /// </summary>
        string Resource { get; }

        /// <summary>
        /// Gets or sets the resource attribute name.
        /// </summary>
        string ResourceAttributeName { get; set; }

        /// <summary>
        /// Gets setup start time.
        /// </summary>
        T SetupStartTime { get; }

        /// <summary>
        /// Gets start time.
        /// </summary>
        T StartTime { get; }

        /// <summary>
        /// Gets tear down time.
        /// </summary>
        T TearDownStartTime { get; }

        /// <summary>
        /// Gets end time.
        /// </summary>
        T EndTime { get; }

        /// <summary>
        /// Gets Reference.
        /// </summary>
        string Reference { get; }

        /// <summary>
        /// Gets the unique id.
        /// </summary>
        string UniqueId { get; }

        /// <summary>
        /// Gets the attributes of the operation
        /// </summary>
        IAttributeCollection Attributes { get; }

        /// <summary>
        /// Gets or sets the intervals associated with an operation.
        /// </summary>
        IEnumerable<Interval> Intervals { get; set; }

        /// <summary>
        /// The save to csv.
        /// </summary>
        /// <param name="csvWriter">
        /// The csv writer.
        /// </param>
        void SaveToCsv(CsvWriter csvWriter);

        /// <summary>
        /// The write csv header.
        /// </summary>
        /// <param name="csvWriter">
        /// The csv writer.
        /// </param>
        void WriteCsvHeader(CsvWriter csvWriter);
    }
}