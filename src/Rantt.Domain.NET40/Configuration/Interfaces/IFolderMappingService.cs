// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFolderMappingService.cs" company="Rantt development team">
//   Copyright (c) 2012 - 2013 Rantt development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Configuration
{
    public interface IFolderMappingService
    {
        /// <summary>
        /// Gets configuration folder path.
        /// </summary>
        /// <returns>The path.</returns>
        string GetConfigurationFolderPath();

        /// <summary>
        /// Gets folder to save state data between application sessions.
        /// </summary>
        /// <returns>The path.</returns>
        string GetStateFolderPath();
    }
}