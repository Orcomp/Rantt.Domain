namespace Rantt.Domain.Configuration.DataContracts
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The pure workspace configuration.
    /// </summary>
    [DataContract]
    public class RawWorkspaceConfiguration
    {
        /// <summary>
        /// The name of workspace
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The name of attribute rantt rows are grouped by. 
        /// </summary>
        [DataMember]
        public string GroupByAttributeName { get; set; }

        /// <summary>
        /// The view start date.
        /// </summary>
        [DataMember]
        public DateTime ViewStartDate { get; set; }

        /// <summary>
        /// The view end date.
        /// </summary>
        [DataMember]
        public DateTime ViewEndDate { get; set; }

        /// <summary>
        /// The resource scale.
        /// </summary>
        [DataMember]
        public double ScaleY { get; set; }

        /// <summary>
        /// The y view position.
        /// </summary>
        [DataMember]
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the past should be highlighted.
        /// </summary>
        [DataMember]
        public bool HighlightPast { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the past should be highlighted.
        /// </summary>
        [DataMember]
        public bool AreTooltipsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is data row enabled.
        /// </summary>
        [DataMember]
        public bool IsDataRowEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is plot row enabled.
        /// </summary>
        [DataMember]
        public bool IsPlotRowEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating plot type enabled.
        /// </summary>
        [DataMember]
        public int PlotType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which intrval will be taken for plot draw.
        /// </summary>
        [DataMember]
        public int PlotInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show current time.
        /// </summary>
        [DataMember]
        public bool ShowCurrentTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether horizontal grid lines are shown.
        /// </summary>
        [DataMember]
        public bool ShowHorizontalGridLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether vertical grid lines are shown.
        /// </summary>
        [DataMember]
        public bool ShowVerticalGridLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use relative time.
        /// </summary>
        [DataMember]
        public bool UseRelativeTime { get; set; }

        /// <summary>
        /// Gets or sets the relative start time value.
        /// </summary>
        [DataMember]
        public DateTime RelativeStartTime { get; set; }

        /// <summary>
        /// Gets or sets the operation color attribute.
        /// </summary>
        [DataMember]
        public string OperationColorAttribute { get; set; }

        /// <summary>
        /// Gets or sets the list of higlighted attributtes.
        /// </summary>
        [DataMember]
        public List<string> SelectedAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show count row on data row
        /// </summary>
        [DataMember]
        public bool ShowCountData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show duration row on data row
        /// </summary>
        [DataMember]
        public bool ShowDurationData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether how to show duration (Millisecond, Second, Minute, Hour, Day)
        /// </summary>
        [DataMember]
        public RelativeDurationEnum RelativeDuration { get; set; }
    }
}
