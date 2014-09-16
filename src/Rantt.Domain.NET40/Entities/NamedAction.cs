// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamedAction.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Entities
{
    using System;

    /// <summary>
    /// The action with given name.
    /// </summary>
    public class NamedAction
    {
        /// <summary>
        /// Unique action name to be referenced.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// The reference to the action.
        /// </summary>
        public Action Action { get; set; } 

    }
}
