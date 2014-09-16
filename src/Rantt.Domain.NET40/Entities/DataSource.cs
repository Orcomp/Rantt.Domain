// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSource.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The data source.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// The data source.
    /// </summary>
    #if (!SILVERLIGHT)
    [Serializable]
    #endif
    public class DataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class.
        /// </summary>
        public DataSource()
        {
            this.FieldMappings = new List<FieldMapping>();
        }

        #region Public Properties

        /// <summary>
        /// Gets or sets the culture info.
        /// </summary>
        public string CultureInfo { get; set; }

        /// <summary>
        /// Gets or sets the data source type.
        /// </summary>
        public DataSourceType DataSourceType { get; set; }

        /// <summary>
        /// Gets or sets the date representation.
        /// </summary>
        public DateRepresentation DateRepresentation { get; set; }

        /// <summary>
        /// Gets or sets the start date for relative date presentation
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the where start.
        /// </summary>
        public DateTime WhereStart { get; set; }

        /// <summary>
        /// Gets or sets the where end.
        /// </summary>
        public DateTime WhereEnd { get; set; }

        /// <summary>
        /// Gets or sets the unit of time.
        /// </summary>
        public UnitOfTime UnitOfTime { get; set; }

        /// <summary>
        /// Gets or sets the field mappings.
        /// </summary>
        public List<FieldMapping> FieldMappings { get; set; }

        /// <summary>
        /// Gets or sets the interval type.
        /// </summary>
        public IntervalEntityType IntervalEntityType { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        // Fields...
        private string _FilePath;

        /// <summary>
        /// The file path
        /// </summary>
        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the worksheet name.
        /// </summary>
        public string WorksheetName { get; set; }

        /// <summary>
        /// Gets or sets cell range in the excel wotksheet.
        /// </summary>
        /// <remarks> 
        /// If null or empty means that start from A1 and go right and down until empty
        /// If only top left cell defined (e.g. "C7") start from tope left cell go right and down until empty
        /// If range is fully defined (e.g. "C7:FC987") we read the full range as if it was csv. 
        /// First line in a defined range is a header line
        /// </remarks>
        public string CellRange { get; set; }

        /// <summary>
        /// Gets or sets the table name.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the default attribute.
        /// </summary>
        public string DefaultAttribute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether we should skip that dataset or not.
        /// </summary>
        public bool IsEmpty { get; set; }

        /// <summary>
        /// Gets or sets data object. 
        /// </summary>
        private object Data { get; set; }
        
        #endregion

        /// <summary>
        /// The get data source column name.
        /// </summary>
        /// <param name="ranttColumnName">
        /// The rantt column name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public string GetDataSourceColumnName(string ranttColumnName)
        {
            if (this.FieldMappings != null)
            {
                FieldMapping fieldMapping =
                    this.FieldMappings.FirstOrDefault(fm => fm.RanttColumnName == ranttColumnName);

                if (fieldMapping != null)
                {
                    return fieldMapping.DataSourceColumnName;
                }
            }

            return ranttColumnName;
        }

        /// <summary>
        /// The get rantt column name.
        /// </summary>
        /// <param name="dataSourceColumnName">
        /// The data source column name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public string GetRanttColumnName(string dataSourceColumnName)
        {
            if (this.FieldMappings != null)
            {
                FieldMapping fieldMapping =
                    this.FieldMappings.FirstOrDefault(fm => fm.RanttColumnName == dataSourceColumnName);

                if (fieldMapping != null)
                {
                    return fieldMapping.RanttColumnName;
                }
            }

            return dataSourceColumnName;
        }

        /// <summary>
        /// Clears data fields which is not needed
        /// </summary>
        public void RemoveNotNeededData()
        {
            if (this.DataSourceType != DataSourceType.Db)
            {
                this.ConnectionString = string.Empty;
                this.TableName = string.Empty;
            }
            else
            {
                this.FilePath = string.Empty;
            }
        }
    }
}