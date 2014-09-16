//-----------------------------------------------------------------------
// <copyright file="FixedFieldNames.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Centralized place to organize names of fixed fields in CSV file
    /// </summary>
    public static class FixedFieldNames
    {
        /// <summary>
        /// A reference ID for the operation
        /// </summary>
        public const string Reference = "Reference";

        /// <summary>
        /// Operation's start time
        /// </summary>
        public const string StartTime = "StartTime";

        /// <summary>
        /// Operation's end time
        /// </summary>
        public const string EndTime = "EndTime";

        /// <summary>
        /// Operation's setup start time
        /// </summary>
        public const string SetupStartTime = "SetupStartTime";

        /// <summary>
        /// Operation's tear down start time
        /// </summary>
        public const string TearDownStartTime = "TearDownStartTime";

        /// <summary>
        /// Operation's default resource
        /// </summary>
        public const string Resource = "Resource";

        /// <summary>
        /// The all field names as a set
        /// </summary>
        public static readonly HashSet<string> AllNames = new HashSet<string>(new[] { StartTime, EndTime, SetupStartTime, TearDownStartTime, Resource });
    } 
}
