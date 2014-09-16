// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataSourceType.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The data source type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    /// <summary>
    /// The data source type.
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// The csv.
        /// </summary>
        Csv, 

        /// <summary>
        /// The xml.
        /// </summary>
        Xml, 

        /// <summary>
        /// The database.
        /// </summary>
        Db, 

        /// <summary>
        /// The json.
        /// </summary>
        Json,

        /// <summary>
        /// The excel.
        /// </summary>
        Excel,

        /// <summary>
        /// The memory.
        /// </summary>
        Memory
    }
}