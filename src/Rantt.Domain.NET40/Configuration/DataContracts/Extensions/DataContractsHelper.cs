// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContractsHelper.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Extensions method for make life with contracts data processing easier
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.DataContracts.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;

    /// <summary>
    /// Extensions method for make life with contracts data processing easier.
    /// </summary>
    public static class DataContractsHelper
    {
        /// <summary>
        /// Gets raw configuration data for given rantt configuration.
        /// </summary>
        /// <param name="configuration">The rantt configuration.</param>
        /// <returns>The raw configuration data.</returns>
        public static RawConfigurationData GetRawConfigurationData(this RanttConfiguration configuration)
        {
            RawConfigurationData result = new RawConfigurationData
                                              {
                                                  ResourceConfigurations = new List<RawResourceConfiguration>(), 
                                                  BaseAttributeConfigurations = new List<RawBaseAttributeConfiguration>(),
                                                  ExtendedAttributeConfigurations = new Dictionary<string, List<RawAttributeExtendedConfiguration>>(),
                                                  CalendarStateConfigurations = new List<RawCalendarStateConfiguration>(),
                                                  AvailableWorkspaces = configuration.AvailableWorkspaces.Keys.ToList()
                                              };

            foreach (var resourceConfiguration in configuration.ResourceConfigurations.Values)
            {
                result.ResourceConfigurations.Add(new RawResourceConfiguration { IsVisible = resourceConfiguration.IsVisible, Name = resourceConfiguration.Name, Position = resourceConfiguration.Position });
            }

            foreach (AttributeConfiguration attributeConfiguration in configuration.AttributeConfigurations.Values)
            {
                result.BaseAttributeConfigurations.Add(attributeConfiguration.GetRawBaseAttributeConfiguration());
                List<RawAttributeExtendedConfiguration> extendedConfigurations = attributeConfiguration.GetRawExtendedAttributeConfiguration();
                if (extendedConfigurations.Count > 0)
                {
                    result.ExtendedAttributeConfigurations.Add(attributeConfiguration.Name, extendedConfigurations);
                }
            }

            foreach (CalendarStateConfiguration calendarStateConfiguration in  configuration.CalendarStateConfigurations.Values)
            {
                result.CalendarStateConfigurations.Add(new RawCalendarStateConfiguration { Color = calendarStateConfiguration.CalendarStateColor, IsVisible = calendarStateConfiguration.IsVisible, Name = calendarStateConfiguration.Name });
            }

            if (configuration.CurrentWorkspace != null)
            {
                result.CurrentWorkspace = new RawWorkspaceConfiguration
                                              {
                                                  Name = configuration.CurrentWorkspace.Name,
                                                  GroupByAttributeName = configuration.CurrentWorkspace.GroupByAttributeName,
                                                  HighlightPast = configuration.CurrentWorkspace.HighlightPast,
                                                  AreTooltipsEnabled = configuration.CurrentWorkspace.AreTooltipsEnabled,
                                                  IsDataRowEnabled = configuration.CurrentWorkspace.IsDataRowEnabled,
                                                  IsPlotRowEnabled = configuration.CurrentWorkspace.IsPlotRowEnabled,
                                                  OperationColorAttribute = configuration.CurrentWorkspace.OperationColorAttribute,
                                                  PlotType = configuration.CurrentWorkspace.PlotType,
                                                  PlotInterval = configuration.CurrentWorkspace.PlotInterval,
                                                  RelativeStartTime = configuration.CurrentWorkspace.RelativeStartTime,
                                                  SelectedAttributes = configuration.CurrentWorkspace.SelectedAttributes,
                                                  ShowCurrentTime = configuration.CurrentWorkspace.ShowCurrentTime,
                                                  ShowHorizontalGridLines = configuration.CurrentWorkspace.ShowHorizontalGridLines,
                                                  ShowVerticalGridLines = configuration.CurrentWorkspace.ShowVerticalGridLines,
                                                  UseRelativeTime = configuration.CurrentWorkspace.UseRelativeTime,
                                                  RelativeDuration = configuration.CurrentWorkspace.RelativeDuration,
                                                  ShowCountData = configuration.CurrentWorkspace.ShowCountData,
                                                  ShowDurationData = configuration.CurrentWorkspace.ShowDurationData
                                              };
            }

            return result;
        }
        
        /// <summary>
        /// Loads raw configuration data.
        /// </summary>
        /// <param name="configuration">
        /// The configuration object in context of which operation is executed.
        /// </param>
        /// <param name="rawConfigurationData">
        /// The raw configuration data.
        /// </param>
        public static void LoadRawConfiguration(this RanttConfiguration configuration, RawConfigurationData rawConfigurationData)
        {
            configuration.IsInitializing = true;
            configuration.AvailableWorkspaces.Clear();
            configuration.ResourceConfigurations.Clear();
            configuration.AttributeConfigurations.Clear();
            configuration.CalendarStateConfigurations.Clear();

            foreach (RawResourceConfiguration rawResource in rawConfigurationData.ResourceConfigurations)
            {
                ResourceConfiguration resourceConfiguration = new ResourceConfiguration(rawResource.Name, rawResource.Position, rawResource.IsVisible);
                configuration.ResourceConfigurations.Add(resourceConfiguration.Name, resourceConfiguration);
                resourceConfiguration.PropertyChanged += (sender, args) => configuration.RaiseConfigurationChangeEvent(sender, args.PropertyName);
            }

            foreach (RawBaseAttributeConfiguration rawBaseAttributeConfiguration in rawConfigurationData.BaseAttributeConfigurations)
            {
                AttributeConfiguration attributeConfiguration = rawBaseAttributeConfiguration.GetAttributeConfiguration();
                configuration.AttributeConfigurations.Add(attributeConfiguration.Name, attributeConfiguration);
                

                if (rawConfigurationData.ExtendedAttributeConfigurations.ContainsKey(attributeConfiguration.Name))
                {
                    foreach (RawAttributeExtendedConfiguration extendedConfiguration in rawConfigurationData.ExtendedAttributeConfigurations[attributeConfiguration.Name])
                    {
                        attributeConfiguration.InitializeAttributeValue(extendedConfiguration.Value);
                        attributeConfiguration.SetAttributeValueColor(extendedConfiguration.Value, CustomColorConverter.StringToColor(extendedConfiguration.Color));
                        attributeConfiguration.SetAttributeValueVisibility(extendedConfiguration.Value, extendedConfiguration.Visibility);
                    }
                }

                attributeConfiguration.PropertyChanged += (sender, args) => configuration.RaiseConfigurationChangeEvent(sender, args.PropertyName);
            }

            foreach (RawCalendarStateConfiguration rawCalendarStateConfiguration in rawConfigurationData.CalendarStateConfigurations)
            {
                CalendarStateConfiguration calendarStateConfiguration = new CalendarStateConfiguration (rawCalendarStateConfiguration.Name, CustomColorConverter.StringToColor(rawCalendarStateConfiguration.Color)) { IsVisible = rawCalendarStateConfiguration.IsVisible };
                calendarStateConfiguration.PropertyChanged += (sender, args) => configuration.RaiseConfigurationChangeEvent(sender, args.PropertyName);
            }

            RawWorkspaceConfiguration rawWorkspace = rawConfigurationData.CurrentWorkspace;
            configuration.CurrentWorkspace = rawWorkspace == null ? null :
                                                 new WorkspaceConfiguration
                                                 {
                                                     Name = rawWorkspace.Name,
                                                     GroupByAttributeName = rawWorkspace.GroupByAttributeName, 
                                                     HighlightPast = rawWorkspace.HighlightPast,
                                                     AreTooltipsEnabled = rawWorkspace.AreTooltipsEnabled,
                                                     IsDataRowEnabled = rawWorkspace.IsDataRowEnabled,
                                                     IsPlotRowEnabled = rawWorkspace.IsPlotRowEnabled,
                                                     OperationColorAttribute = rawWorkspace.OperationColorAttribute,
                                                     PlotType = rawWorkspace.PlotType,
                                                     PlotInterval = rawWorkspace.PlotInterval,
                                                     RelativeStartTime = rawWorkspace.RelativeStartTime,
                                                     SelectedAttributes = rawWorkspace.SelectedAttributes,
                                                     ShowCurrentTime = rawWorkspace.ShowCurrentTime,
                                                     ShowHorizontalGridLines = rawWorkspace.ShowHorizontalGridLines,
                                                     ShowVerticalGridLines = rawWorkspace.ShowVerticalGridLines,
                                                     UseRelativeTime = rawWorkspace.UseRelativeTime,
                                                     RelativeDuration = rawWorkspace.RelativeDuration, 
                                                     ShowCountData = rawWorkspace.ShowCountData, 
                                                     ShowDurationData = rawWorkspace.ShowDurationData
                                                 };

            foreach (string availableWorkspace in rawConfigurationData.AvailableWorkspaces)
            {
                if ((configuration.CurrentWorkspace != null) &&
                    (configuration.CurrentWorkspace.Name == availableWorkspace))
                {
                    configuration.AvailableWorkspaces.Add(availableWorkspace, configuration.CurrentWorkspace);
                }
                else
                {
                    configuration.AvailableWorkspaces.Add(availableWorkspace, null);
                }
                
            }

            configuration.IsInitializing = false;
        }

        /// <summary>
        /// Gets raw attribute base configuration for the given attribute.
        /// </summary>
        /// <param name="attributeConfiguration">
        /// Given attribute.
        /// </param>
        /// <returns>
        /// The raw base attribute's configuration.
        /// </returns>
        public static RawBaseAttributeConfiguration GetRawBaseAttributeConfiguration(this AttributeConfiguration attributeConfiguration)
        {
            return new RawBaseAttributeConfiguration
                       {
                           Category = attributeConfiguration.Category, 
                           Description = attributeConfiguration.Description, 
                           DetailWindowPosition = attributeConfiguration.DetailWindowPosition, 
                           EditorPosition = attributeConfiguration.EditorPosition, 
                           IsColorAttribute = attributeConfiguration.IsColorAttribute, 
                           IsDisabled = attributeConfiguration.IsDisabled, 
                           IsInDetailsWindow = attributeConfiguration.IsInDetailsWindow, 
                           IsInEditor = attributeConfiguration.IsInEditor, 
                           IsInLabel = attributeConfiguration.IsInLabel, 
                           IsInTooltip = attributeConfiguration.IsInTooltip, 
                           LabelPosition = attributeConfiguration.LabelPosition, 
                           Name = attributeConfiguration.Name, 
                           TooltipPosition = attributeConfiguration.TooltipPosition, 
                           Type = attributeConfiguration.Type, 
                           IsInDataRow = attributeConfiguration.IsInDataRow, 
                           DataRowPosition = attributeConfiguration.DataRowPosition,
                           DataRowNumericFormat = attributeConfiguration.DataRowNumericFormat
                       };
        }

        /// <summary>
        /// Gets attribute configuration from the given raw base attribute.
        /// </summary>
        /// <param name="rawBaseAttributeConfiguration">The given raw base attribute.</param>
        /// <returns>Attribute configuration.</returns>
        public static AttributeConfiguration GetAttributeConfiguration(this RawBaseAttributeConfiguration rawBaseAttributeConfiguration)
        {
            return new AttributeConfiguration(rawBaseAttributeConfiguration.Name, rawBaseAttributeConfiguration.Type, rawBaseAttributeConfiguration.Category,
                rawBaseAttributeConfiguration.Description, rawBaseAttributeConfiguration.IsInTooltip, rawBaseAttributeConfiguration.IsInEditor, rawBaseAttributeConfiguration.IsInDetailsWindow,
                rawBaseAttributeConfiguration.IsDisabled, rawBaseAttributeConfiguration.IsInLabel, rawBaseAttributeConfiguration.TooltipPosition, rawBaseAttributeConfiguration.DetailWindowPosition,
                rawBaseAttributeConfiguration.EditorPosition, rawBaseAttributeConfiguration.LabelPosition, rawBaseAttributeConfiguration.IsColorAttribute)
                       {
                           IsInDataRow = rawBaseAttributeConfiguration.IsInDataRow, 
                           DataRowPosition = rawBaseAttributeConfiguration.DataRowPosition, 
                           DataRowNumericFormat = rawBaseAttributeConfiguration.DataRowNumericFormat
                       };
        }


        /// <summary>
        /// Gets raw attribute extended configuration for the given attribute.
        /// </summary>
        /// <param name="attributeConfiguration">Given attribute.</param>
        /// <returns>The raw base attribute's configuration.</returns>
        public static List<RawAttributeExtendedConfiguration> GetRawExtendedAttributeConfiguration(this AttributeConfiguration attributeConfiguration)
        {
            foreach (var colorKey in attributeConfiguration.Colors.Keys)
            {
                attributeConfiguration.InitializeAttributeValue(colorKey);
            }

            foreach (var visibilityKey in attributeConfiguration.Visibility.Keys)
            {
                attributeConfiguration.InitializeAttributeValue(visibilityKey);
            }
            
            return attributeConfiguration.GetExtendedConfiguration().Select(attributeExtendedConfiguration => new RawAttributeExtendedConfiguration { Color = attributeExtendedConfiguration.Color, Value = attributeExtendedConfiguration.Value, Visibility = attributeExtendedConfiguration.Visibility }).ToList();
        }
    }
}
