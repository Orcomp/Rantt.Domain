// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RanttDataSet.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Media;
    using Configuration;
    using CsvHelper;

    /// <summary>
    /// The container of all data read from one dataset.
    /// </summary>
    public class RanttDataSet
    {
        #region Constants
        #endregion

        #region Fields
        /// <summary>
        /// The operation reference map.
        /// </summary>
        private readonly Dictionary<string, string> refmap = new Dictionary<string, string>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RanttDataSet" /> class.
        /// </summary>
        /// <param name="operations">The operations.</param>
        /// <param name="calendarPeriods">The calendar periods.</param>
        /// <param name="relationships">The relationships.</param>
        public RanttDataSet(List<IOperation<DateTime>> operations, List<ICalendarPeriod> calendarPeriods, List<IRelationship> relationships)
        {
            Operations = operations;
            CalendarPeriods = calendarPeriods;
            Relationships = relationships;

            var dictionary = new Dictionary<string, IAttributeCollection>();
            foreach (var operation in Operations)
            {
                dictionary.Add(operation.UniqueId, operation.Attributes);
            }

            OperationDetails = dictionary;
            OperationAttributeNames = Operations.Count > 0
                                          ? new HashSet<string>(Operations.First().Attributes.Names)
                                          : new HashSet<string>();

            ResourceCalendarPeriods = CalendarPeriods
                .GroupBy(x => x.Resource)
                .ToDictionary(x => x.Key, x => x.ToList());

            IsCalendarInformationAvailable = CalendarPeriods.Any();
            IsOperationAttributesAvailable = OperationAttributeNames.Count > 0;
            IsRelationshipsAvailable = Relationships != null && Relationships.Any();

            // TODO we should decide something later this translation does not seem good architecturally
            TranslateRelationships();
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets the operations.
        /// </summary>
        public List<IOperation<DateTime>> Operations { get; private set; }

        /// <summary>
        /// Gets the operation details.
        /// </summary>
        public Dictionary<string, IAttributeCollection> OperationDetails { get; private set; }

        /// <summary>
        /// Gets the operation attribute names.
        /// </summary>
        public HashSet<string> OperationAttributeNames { get; private set; }

        /// <summary>
        /// Gets the relationships.
        /// </summary>
        public List<IRelationship> Relationships { get; private set; }

        /// <summary>
        /// Gets the calendar periods.
        /// </summary>
        public List<ICalendarPeriod> CalendarPeriods { get; private set; }

        /// <summary>
        /// Gets the resource calendar periods.
        /// </summary>
        public Dictionary<string, List<ICalendarPeriod>> ResourceCalendarPeriods { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is calendar information available.
        /// </summary>
        public bool IsCalendarInformationAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is relationships available.
        /// </summary>
        public bool IsRelationshipsAvailable { get; private set; }

        /// <summary>
        /// Gets a value indicating whether is operation attributes available.
        /// </summary>
        public bool IsOperationAttributesAvailable { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// The initialization gantt configuration.
        /// </summary>
        /// <param name="ranttConfiguration">
        /// The gantt configuration.
        /// </param>
        /// <param name="currentResourceConfigurationSource">
        /// The current resource configuration source.
        /// </param>
        /// <param name="visibleByDefaultAttributes">
        /// The visible by default attributes.
        /// </param>
        public void InitRanttConfiguration(
            RanttConfiguration ranttConfiguration, 
            Dictionary<string, ResourceConfiguration> currentResourceConfigurationSource, 
            HashSet<string> visibleByDefaultAttributes = null)
        {
            ranttConfiguration.IsInitializing = true;

            RepopulateResourceConfiguration(ranttConfiguration, currentResourceConfigurationSource);

            // add calendar period configuration if it doesn't exist yet
            CalendarPeriods.Select(x => x.CalendarState)
                           .Distinct()
                           .Where(x => !ranttConfiguration.CalendarStateConfigurations.ContainsKey(x))
                           .ToList()
                           .ForEach(x => ranttConfiguration.CalendarStateConfigurations.Add(x, new CalendarStateConfiguration(x, Color.FromArgb(0xFF, 0xF5, 0xF5, 0xF5))));

            // add attribute configuration if it doesn't exist
            foreach (var operation in Operations)
            {
                foreach (string attributeName in operation.Attributes.Names)
                {
                    if (!ranttConfiguration.AttributeConfigurations.ContainsKey(attributeName))
                    {
                        int pos = ranttConfiguration.AttributeConfigurations.Count + 1;

                        ranttConfiguration.AttributeConfigurations.Add(
                            attributeName, 
                            new AttributeConfiguration(attributeName, typeof(string).ToString(), string.Empty, string.Empty, 
                                true, 
                                true, 
                                true, 
                                false, 
                                visibleByDefaultAttributes != null && visibleByDefaultAttributes.Contains(attributeName), 
                                pos, 
                                pos, 
                                pos, 
                                pos,
                                true)
                                {
                                    IsInDataRow = (attributeName == "Quantity"), 
                                    DataRowPosition = pos, 
                                    DataRowNumericFormat = "g5"
                                });
                    }

                    string attrValue = operation.Attributes[attributeName];
                    ranttConfiguration.AttributeConfigurations[attributeName].InitializeAttributeValue(attrValue);
                }
            }

            ranttConfiguration.IsInitializing = false;
        }

        /// <summary>
        /// The repopulate resource configuration.
        /// </summary>
        /// <param name="ranttConfiguration">
        /// The gantt configuration.
        /// </param>
        /// <param name="currentResourceConfigurationSource">
        /// The current resource configuration source.
        /// </param>
        public void RepopulateResourceConfiguration(
            RanttConfiguration ranttConfiguration, 
            Dictionary<string, ResourceConfiguration> currentResourceConfigurationSource)
        {
            // Group allOperations by resource
            var operationsGroupedByResource =
                Operations
                    .GroupBy(x => x.Resource)
                    .ToDictionary(
                        x => x.Key, 
                        x => x.OrderBy(y => y.StartTime).ToList());

            foreach (var resourceName in operationsGroupedByResource.Keys)
            {
                if (!operationsGroupedByResource.ContainsKey(resourceName))
                {
                    continue;
                }

                if (!currentResourceConfigurationSource.ContainsKey(resourceName))
                {
                    ResourceConfiguration resourceConfiguration = new ResourceConfiguration(resourceName, currentResourceConfigurationSource.Count + 1, true);
                    currentResourceConfigurationSource.Add(
                        resourceName,
                        resourceConfiguration);

                    if (currentResourceConfigurationSource == ranttConfiguration.ResourceConfigurations)
                    {
                        resourceConfiguration.PropertyChanged += (sender, args) => ranttConfiguration.RaiseConfigurationChangeEvent(sender, args.PropertyName);
                    }
                }
            }
        }

        #if (!SILVERLIGHT)
        /// <summary>
        /// The save operations to csv file.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void SaveOperationsToCsvFile(string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                using (var csv = new CsvWriter(writer))
                {
                    IOperation<DateTime> first = Operations.FirstOrDefault();
                    if (first != null)
                    {
                        first.WriteCsvHeader(csv);
                    }

                    foreach (IOperation<DateTime> operation in Operations)
                    {
                        operation.SaveToCsv(csv);
                    }
                }
            }
        }

        /// <summary>
        /// The save operation relationships to csv file.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void SaveOperationRelationshipsToCsvFile(string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                using (var csv = new CsvWriter(writer))
                {
                    IEnumerable rels = Relationships;
                    csv.WriteRecords(rels);
                }
            }
        }

        /// <summary>
        /// The save calendar periods to csv file.
        /// </summary>
        /// <param name="filename">
        /// The filename.
        /// </param>
        public void SaveCalendarPeriodsToCsvFile(string filename)
        {
            using (var writer = new StreamWriter(filename))
            {
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(CalendarPeriods as IEnumerable);
                }
            }
        }
        #endif

        /// <summary>
        /// The translate operation reference.
        /// </summary>
        /// <param name="reference">
        /// The reference.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string TranslateOperationReference(string reference)
        {
            if (refmap.ContainsKey(reference))
            {
                return refmap[reference];
            }

            var operation = Operations.FirstOrDefault(op => op.Reference == reference);
            if (operation != null)
            {
                refmap.Add(reference, operation.UniqueId);
                return operation.UniqueId;
            }

            // throw new Exception(string.Format("Error during relationship interpretation. Operation with reference '{0}' doesn't exist.", reference));
            return "-1";
        }

        /// <summary>
        /// The translate relationships.
        /// </summary>
        private void TranslateRelationships()
        {
            if (Relationships == null)
            {
                return;
            }

            foreach (IRelationship relationship in Relationships)
            {
                relationship.From = TranslateOperationReference(relationship.From);
                relationship.To = TranslateOperationReference(relationship.To);
            }

            Relationships.RemoveAll(rel => rel.From == "-1" || rel.To == "-1");
        }
        #endregion
    }
}