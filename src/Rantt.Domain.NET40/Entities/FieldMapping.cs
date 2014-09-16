// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldMapping.cs" company="Orcomp">
//   Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Entities
{
    /// <summary>
    /// The field mapping.
    /// </summary>
    public class FieldMapping
    {
        /// <summary>
        /// Gets or sets the data source column name.
        /// </summary>
        public string DataSourceColumnName { get; set; }

        /// <summary>
        /// Gets or sets the rant column name.
        /// </summary>
        public string RanttColumnName { get; set; }
    }
}
