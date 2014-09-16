// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawConfigurationData.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Pure configuration data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Rantt.Domain.Configuration.DataContracts;
    using Rantt.Domain.Entities;

    /// <summary>
    /// Pure configuration data.
    /// </summary>
    [DataContract]
    [KnownType(typeof(RawResourceConfiguration))]
    [KnownType(typeof(StartupSettings))]
    public class RawConfigurationData
    {
        /// <summary>
        /// Gets or sets resource configurations.
        /// </summary>
        [DataMember]
        public List<RawResourceConfiguration> ResourceConfigurations { get; set; }

        /// <summary>
        /// Gets or sets base attribute configurations.
        /// </summary>
        [DataMember]
        public List<RawBaseAttributeConfiguration> BaseAttributeConfigurations { get; set; }

        /// <summary>
        /// Gets or sets extended attribute configurations.
        /// </summary>
        [DataMember]
        public Dictionary<string, List<RawAttributeExtendedConfiguration>> ExtendedAttributeConfigurations { get; set; }

        /// <summary>
        /// Gets or sets calendar state configurations.
        /// </summary>
        [DataMember]
        public List<RawCalendarStateConfiguration> CalendarStateConfigurations { get; set; }

        /// <summary>
        /// Gets or sets list of available workspaces.
        /// </summary>
        [DataMember]
        public List<string> AvailableWorkspaces { get; set; }

        [DataMember]
        public RawWorkspaceConfiguration CurrentWorkspace { get; set; }

        [DataMember]
        public StartupSettings StartupSettings { get; set; }

        [DataMember]
        public List<string[]> DefaultColorsOverridings { get; set; }
    }
}
