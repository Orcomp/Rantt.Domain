namespace Rantt.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Domain.Entities;

    /// <summary>
    /// Pure project data class.
    /// </summary>
    [DataContract]
    [KnownType(typeof(RawRanttDataSet))]
    [KnownType(typeof(CalendarPeriod))]
    [KnownType(typeof(Operation<DateTime>))]
    [KnownType(typeof(AttributeCollection))]
    [KnownType(typeof(Relationship))]
    public class RawRanttData
    {
        /// <summary>
        /// Gets or sets the list of datasets.
        /// </summary>
        [DataMember]
        public List<RawRanttDataSet> DataSets { get; set; }

        /// <summary>
        /// Gets or sets the default attribute.
        /// </summary>
        [DataMember]
        public string DefaultAttribute { get; set; }

        /// <summary>
        /// Gets or sets the reference which uniquely define rantt data.
        /// </summary>
        [DataMember]
        public string Reference { get; set; }
    }
}
