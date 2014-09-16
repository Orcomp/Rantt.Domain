// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSetDescriptor.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The data set descriptor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The data set descriptor.
    /// </summary>
    #if (!SILVERLIGHT)
    [Serializable]
    #endif
    public class DataSetDescriptor
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the calendar period data source.
        /// </summary>
        public DataSource CalendarPeriodDataSource { get; set; }

        /// <summary>
        /// Gets or sets the operation data source.
        /// </summary>
        public DataSource OperationDataSource { get; set; }

        /// <summary>
        /// Gets or sets the operation relationship data source.
        /// </summary>
        public DataSource OperationRelationshipDataSource { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        public string Reference { get; set; }

        #endregion

        /// <summary>
        /// Clears data fields which is not needed
        /// </summary>
        public void RemoveNotNeededData()
        {
            if (CalendarPeriodDataSource != null)
            {
                CalendarPeriodDataSource.RemoveNotNeededData();
            }

            if (OperationDataSource != null)
            {
                OperationDataSource.RemoveNotNeededData();
            }

            if (OperationRelationshipDataSource != null)
            {
                OperationRelationshipDataSource.RemoveNotNeededData();
            }
        }
    }
}