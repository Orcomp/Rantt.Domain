// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeConfigurationClassMap.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Csv helper configuration for attribute configuration class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.CsvHelperConfiguration
{
    using CsvHelper.Configuration;
    using Configuration;

    /// <summary>
    /// Csv helper configuration for attribute configuration class.
    /// </summary>
    public class AttributeConfigurationClassMap : CsvClassMap<AttributeConfiguration>
    {
        public AttributeConfigurationClassMap()
        {
            Map(m => m.Name);
            Map(m => m.Type);
            Map(m => m.IsColorAttribute);
            Map(m => m.IsInDetailsWindow);
            Map(m => m.DetailWindowPosition);
            Map(m => m.IsInLabel);
            Map(m => m.LabelPosition);
            Map(m => m.IsInTooltip);
            Map(m => m.TooltipPosition);
            Map(m => m.Category);
            Map(m => m.Description);
            Map(m => m.DataRowPosition);
            Map(m => m.IsInDataRow);
            Map(m => m.DataRowNumericFormat);
            Map(m => m.Values).Ignore();
        }
    }
}
