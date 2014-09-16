// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntervalEntityType.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The interval type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    /// <summary>
    /// The interval type.
    /// </summary>
    public enum IntervalEntityType
    {
        /// <summary>
        /// The operation.
        /// </summary>
        Operation, 

        /// <summary>
        /// The calendar period.
        /// </summary>
        CalendarPeriod,

        /// <summary>
        /// The operation relationship.
        /// </summary>
        Relationship
    }
}