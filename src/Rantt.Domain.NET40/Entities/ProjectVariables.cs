// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectVariables.cs" company="Orcomp">
//   Copyright Orcomp
// </copyright>
// <summary>
//   The project.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Entities
{
    using System;

    using Catel.Data;

    [Serializable]
    public class ProjectVariables : SavableModelBase<ProjectVariables>
    {
        /// <summary>
        /// The name of workspace
        /// </summary>
        public string LastWorkspace { get; set; }
    }
}
