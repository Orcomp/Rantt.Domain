// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumDescription.cs" company="Rantt development team">
//   Copyright (c) 2012 - 2013 Rantt development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Reflection;

    public static class EnumDescription
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}