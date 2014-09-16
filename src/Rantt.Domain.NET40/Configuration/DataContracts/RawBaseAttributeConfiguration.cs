// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawBaseAttributeConfiguration.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Pure base attribute configuration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Pure base attribute configuration.
    /// </summary>
    [DataContract]
    public struct RawBaseAttributeConfiguration
    {
        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute is used in the tooltip.
        /// </summary>
        [DataMember]
        public bool IsInTooltip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute is used in the editor.
        /// </summary>
        [DataMember]
        public bool IsInEditor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute is used in the details window.
        /// </summary>
        [DataMember]
        public bool IsInDetailsWindow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute is disabled.
        /// </summary>
        [DataMember]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute is mentioned in the operation label.
        /// </summary>
        [DataMember]
        public bool IsInLabel { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public int TooltipPosition { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public int DetailWindowPosition { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public int EditorPosition { get; set; }

        /// <summary>
        /// Gets or sets the attribute's name.
        /// </summary>
        [DataMember]
        public int LabelPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute can be used for coloring operation.
        /// </summary>
        [DataMember]
        public bool IsColorAttribute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show attribute value in data row.
        /// </summary>
        [DataMember]
        public bool IsInDataRow { get; set; }

        /// <summary>
        /// Gets or sets the editor position.
        /// </summary>
        [DataMember]
        public int DataRowPosition { get; set; }

        /// <summary>
        /// Gets or sets a format for numbers in the data row.
        /// </summary>
        [DataMember]
        public string DataRowNumericFormat { get; set; }
    }
}
