using System;
using System.Collections.Generic;

namespace Rantt.Domain
{
    using System.Runtime.Serialization;
    using Entities;

    /// <summary>
    /// Pure rantt dataset class
    /// </summary>
    [DataContract]
    [KnownType(typeof(CalendarPeriod))]
    [KnownType(typeof(Operation<DateTime>))]
    [KnownType(typeof(AttributeCollection))]
    [KnownType(typeof(Relationship))]
    public class RawRanttDataSet
    {
        /// <summary>
        /// Gets or sets dataset name 
        /// </summary>
        [DataMember]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the calendar periods.
        /// </summary>
        [DataMember]
        public List<ICalendarPeriod> CalendarPeriods { get; set; }

        /// <summary>
        /// Gets or sets the operations.
        /// </summary>
        [DataMember]
        public List<IOperation<DateTime>> Operations { get; set; }

        /// <summary>
        /// Gets or sets the relationships.
        /// </summary>
        [DataMember]
        public List<IRelationship> Relationships { get; set; }
    }
}
