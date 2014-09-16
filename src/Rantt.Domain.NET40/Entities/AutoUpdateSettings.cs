using System;

namespace Rantt.Domain.Entities
{
    using System.Xml.Serialization;

#if (!SILVERLIGHT)
        [Serializable]
    #endif
    public class AutoUpdateSettings
    {
        public AutoUpdateSettings()
        {
            Enabled = false;
            DurationUnitOfTime = UnitOfTime.Hour;
            Duration = 1;
        }

        /// <summary>
        /// Indicates whether auto update is enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Defines unit in which duration is defined
        /// </summary>
        public UnitOfTime DurationUnitOfTime { get; set; }

        /// <summary>
        /// Duration of update interval 
        /// </summary>
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
