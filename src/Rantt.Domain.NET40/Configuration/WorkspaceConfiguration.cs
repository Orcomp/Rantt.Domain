namespace Rantt.Domain.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Catel.Data;

    /// <summary>
    /// Class to hold workspace configuration other than holded in Rantt Configuration
    /// </summary>
    /// <para>
    /// it is quick and dirty implementation of comparison two workspace configuration
    /// I guess it is ok for class which is not going to be changed.
    /// If class structure become not stable by some reason you need carefully rewrite Equals function
    /// </para>
#if (!SILVERLIGHT)
    [Serializable]
#endif
    public class WorkspaceConfiguration : SavableModelBase<WorkspaceConfiguration>, IEquatable<WorkspaceConfiguration>
    {
        /// <summary>
        /// The name of workspace
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The name of attribute rantt rows are grouped by. 
        /// </summary>
        public string GroupByAttributeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the past should be highlighted.
        /// </summary>
        public bool HighlightPast { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tooltips should be shown.
        /// </summary>
        public bool AreTooltipsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is data row enabled.
        /// </summary>
        public bool IsDataRowEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is plot row enabled.
        /// </summary>
        public bool IsPlotRowEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating plot type enabled.
        /// </summary>
        public int PlotType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating which intrval will be taken for plot draw.
        /// </summary>
        public int PlotInterval { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show current time.
        /// </summary>
        public bool ShowCurrentTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether horizontal grid lines are shown.
        /// </summary>
        public bool ShowHorizontalGridLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether vertical grid lines are shown.
        /// </summary>
        public bool ShowVerticalGridLines { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use relative time.
        /// </summary>
        public bool UseRelativeTime { get; set; }

        /// <summary>
        /// Gets or sets the relative start time value.
        /// </summary>
        public DateTime RelativeStartTime { get; set; }

        /// <summary>
        /// Gets or sets the operation color attribute.
        /// </summary>
        public string OperationColorAttribute { get; set; }

        /// <summary>
        /// Gets or sets the list of higlighted attributtes.
        /// </summary>
        public List<string> SelectedAttributes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show count row on data row
        /// </summary>
        public bool ShowCountData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show duration row on data row
        /// </summary>
        public bool ShowDurationData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether how to show duration (Millisecond, Second, Minute, Hour, Day)
        /// </summary>
        public RelativeDurationEnum RelativeDuration { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as WorkspaceConfiguration;
            if (other == null)
            {
                return false;
            }

            return Equals(other);
        }

        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(Name) ? base.GetHashCode() : Name.GetHashCode();
        }

        public bool Equals(WorkspaceConfiguration other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool allProperiesEqual = Name == other.Name && 
                                     GroupByAttributeName == other.GroupByAttributeName &&
                                     HighlightPast == other.HighlightPast &&
                                     AreTooltipsEnabled == other.AreTooltipsEnabled  && 
                                     IsDataRowEnabled == other.IsDataRowEnabled  &&
                                     IsPlotRowEnabled == other.IsPlotRowEnabled  &&
                                     ShowCountData == other.ShowCountData &&
                                     ShowDurationData == other.ShowDurationData  &&
                                     RelativeDuration == other.RelativeDuration  &&
                                     PlotType == other.PlotType  &&
                                     PlotInterval == other.PlotInterval  &&
                                     RelativeStartTime == other.RelativeStartTime  &&
                                     ShowCurrentTime == other.ShowCurrentTime  &&
                                     ShowHorizontalGridLines == other.ShowHorizontalGridLines &&
                                     ShowVerticalGridLines == other.ShowVerticalGridLines &&
                                     UseRelativeTime == other.UseRelativeTime &&
                                     OperationColorAttribute == other.OperationColorAttribute;

            if (!allProperiesEqual)
            {
                return false;
            }

            if ((this.SelectedAttributes == null) && (other.SelectedAttributes == null))
            {
                return true;
            }

            if ((this.SelectedAttributes == null) || (other.SelectedAttributes == null))
            {
                return false;
            }

            if (this.SelectedAttributes.Count != other.SelectedAttributes.Count)
            {
                return false;
            }

            return this.SelectedAttributes.Count == 0 || SelectedAttributes.Any(s => other.SelectedAttributes.Contains(s));
        }
    }
}
