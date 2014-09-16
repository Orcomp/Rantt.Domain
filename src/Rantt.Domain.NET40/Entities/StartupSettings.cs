using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rantt.Domain.Entities
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

#if (!SILVERLIGHT)
    [Serializable]
    #endif
    [DataContract]
    public class StartupSettings
    {
        /// <summary>
        /// Start view interval type.
        /// </summary>
        [DataMember]
        public StartViewIntervalType StartViewIntervalType { get; set; }

        /// <summary>
        /// Start date.
        /// </summary>
        [DataMember]
        public DateTime Start { get; set; }

        /// <summary>
        /// Start date.
        /// </summary>
        [DataMember]
        public DateTime End { get; set; }

        /// <summary>
        /// Defines unit in which duration is defined
        /// </summary>
        [DataMember]
        public UnitOfTime DurationUnitOfTime { get; set; }

        /// <summary>
        /// Duration of interval starting with current day
        /// </summary>
        [DataMember]
        public int Duration { get; set; }

        #if (!SILVERLIGHT)
        [XmlIgnore]
        #endif
        public double DurationInSeconds
        {
            get
            {
                switch (DurationUnitOfTime)
                {
                    case UnitOfTime.Day:
                        return TimeSpan.FromDays(Duration).TotalSeconds;
                    case UnitOfTime.Hour:
                        return TimeSpan.FromHours(Duration).TotalSeconds;
                    case UnitOfTime.Minute:
                        return TimeSpan.FromMinutes(Duration).TotalSeconds;
                    case UnitOfTime.Week:
                        return TimeSpan.FromDays(Duration).TotalSeconds;
                    default:
                        return Duration;
                }
            }
        }
    }
}
