// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeValueOverrideColorClassMap.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// <summary>
//   Csv helper configuration for attribute value override color
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Rantt.Domain.Configuration.CsvHelperConfiguration
{
    using CsvHelper.Configuration;

    public class AttributeValueOverrideColorClassMap : CsvClassMap<AttributeValueOverrideColor>
    {
        public AttributeValueOverrideColorClassMap()
        {
            Map(m => m.AttributeName);
            Map(m => m.AttributeValue);
            Map(m => m.Color);
        } 
    }
}
