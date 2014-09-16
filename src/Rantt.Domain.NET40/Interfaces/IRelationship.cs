// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRelationship.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain
{
    /// <summary>
    /// Represents an operations relationship.
    /// </summary>
    public interface IRelationship
    {
        #region Properties
        /// <summary>
        /// Gets or sets <c>from</c> operation.
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// Gets or sets <c>to</c> operation.
        /// </summary>
        string To { get; set; }
        #endregion
    }
}