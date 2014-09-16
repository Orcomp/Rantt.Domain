// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceConfigurationClassMap.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Csv helper configuration for resource configuration class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.CsvHelperConfiguration
{
    using CsvHelper.Configuration;
    using Configuration;

    /// <summary>
    /// Csv helper configuration for resource configuration class.
    /// </summary>
    public class ResourceConfigurationClassMap : CsvClassMap<ResourceConfiguration>
    {
        public ResourceConfigurationClassMap()
        {
            Map(m => m.Name);
            Map(m => m.IsVisible);
            Map(m => m.Position);
        }
    }
}
