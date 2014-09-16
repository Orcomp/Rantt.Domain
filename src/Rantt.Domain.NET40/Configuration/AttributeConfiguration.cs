// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeConfiguration.cs" company="Cherry development team">
//   Copyright (c) 2008 - 2013 Cherry development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rantt.Domain.Configuration
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Media;
    
    using Catel.Data;
    using EventArgs;
    using Helpers;

    /// <summary>
    /// The attribute configuration.
    /// </summary>
    public class AttributeConfiguration : ModelBase
    {
        private static readonly Dictionary<string, Type> TypeCache = new Dictionary<string, Type>();

        private static readonly object SyncObject = new object();
        
        #region Constants
        /// <summary>
        /// List allowed to be selected for attribute.
        /// </summary>
        public static readonly List<AttributeType> SupportedTypes = new List<AttributeType>
                                                                        {
                                                                            new AttributeType { ClassName = "System.String", SystemType = "System.String", DisplayName = "String" }, 
                                                                            new AttributeType { ClassName = "Link", SystemType = "System.String", DisplayName = "Link" }, 
                                                                            new AttributeType { ClassName = "System.DateTime", SystemType = "System.DateTime", DisplayName = "DateTime" }, 
                                                                            new AttributeType { ClassName = "System.Boolean", SystemType = "System.Boolean", DisplayName = "Boolean" }, 
                                                                            new AttributeType { ClassName = "System.Double", SystemType = "System.Double", DisplayName = "Double" }, 
                                                                            new AttributeType { ClassName = "System.Int32", SystemType = "System.Int32", DisplayName = "Integer" }
                                                                        };

        #endregion

        #region Fields        
        /// <summary>
        /// The _values.
        /// </summary>
        private readonly HashSet<string> _values = new HashSet<string>();                         

        /// <summary>
        /// The name.
        /// </summary>
        private string _name;
        
        /// <summary>
        /// The _type
        /// </summary>
        private string _type;                  
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConfiguration" /> class.
        /// </summary>        
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="category">The category.</param>
        /// <param name="description">The description.</param>
        /// <param name="isInTooltip">The is in tooltip.</param>
        /// <param name="isInEditor">The is in editor.</param>
        /// <param name="isInDetailsWindow">The is in details window.</param>
        /// <param name="isDisabled">The is disabled.</param>
        /// <param name="isInLabel">The is on rectangle.</param>
        /// <param name="tooltipPosition">The tooltip position.</param>
        /// <param name="detailWindowPosition">The detail window position.</param>
        /// <param name="editorPosition">The editor position.</param>
        /// <param name="labelPosition">The rectangle position.</param>
        /// <param name="isColorAttribute">The is color attribute.</param>        
        public AttributeConfiguration(            
            string name, 
            string type,
            string category,
            string description,
            bool isInTooltip, 
            bool isInEditor, 
            bool isInDetailsWindow, 
            bool isDisabled,
            bool isInLabel,
            int tooltipPosition, 
            int detailWindowPosition, 
            int editorPosition,
            int labelPosition,
            bool isColorAttribute)
        {                        
            Name = name;
            Type = type;
            Category = category;
            Description = description;
            Colors = new Dictionary<string, string>();
            Visibility = new Dictionary<string, bool>();
            IsInTooltip = isInTooltip;
            IsInEditor = isInEditor;
            IsInDetailsWindow = isInDetailsWindow;
            IsDisabled = isDisabled;
            IsInLabel = isInLabel;
            TooltipPosition = tooltipPosition;
            DetailWindowPosition = detailWindowPosition;
            EditorPosition = editorPosition;
            LabelPosition = labelPosition;
            IsColorAttribute = isColorAttribute;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConfiguration"/> class.
        /// </summary>
        public AttributeConfiguration()
        {
            Colors = new Dictionary<string, string>();
            Visibility = new Dictionary<string, bool>();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the colors.
        /// </summary>
        /// <value>
        /// The colors.
        /// </value>
        public Dictionary<string, string> Colors { get; private set; }

        /// <summary>
        /// Visibility dictionary - for each attribute value whether it visible or not
        /// </summary>
        public Dictionary<string, bool> Visibility { get; private set; }

        /// <summary>
        /// Gets or sets the detail window position.
        /// </summary>
        public int DetailWindowPosition { get; set; }        

        /// <summary>
        /// Gets or sets the editor position.
        /// </summary>
        public int EditorPosition { get; set; }       
        
        /// <value>
        /// <c>true</c> if this instance is disabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisabled { get; set; }        

        /// <value>
        /// <c>true</c> if this instance is in details window; otherwise, <c>false</c>.
        /// </value>
        public bool IsInDetailsWindow { get; set; }       

        /// <value>
        /// <c>true</c> if this instance is in editor; otherwise, <c>false</c>.
        /// </value>
        public bool IsInEditor { get; set; }        

        /// <summary>
        /// Gets or sets a value indicating whether is in tooltip.
        /// </summary>
        public bool IsInTooltip { get; set; }        

        /// <summary>
        /// Gets or sets a value indicating whether is in label.
        /// </summary>
        public bool IsInLabel { get; set; }         

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is in label.
        /// </summary>
        public int TooltipPosition { get; set; }
        
        /// <summary>
        /// Gets or sets the label position.
        /// </summary>
        public int LabelPosition { get; set; }          

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }

            set
            {
                if (_type != value)
                {
                    if (CanChangeType(value))
                    {
                        _type = value;
                        RaisePropertyChanged(() => Type);
                    }
                    else
                    {
                        throw new Exception("Unable to convert type of attribute");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets attribute category.
        /// </summary>
        public string Category { get; set; }         

        /// <summary>
        /// Gets or sets attribute description.
        /// </summary>
        public string Description { get; set; } 
       
        /// <summary>
        /// Gets or sets a value indicating whether is color attribute.
        /// </summary>
        public bool IsColorAttribute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show attribute value in data row.
        /// </summary>
        public bool IsInDataRow { get; set; }

        /// <summary>
        /// Gets or sets the editor position.
        /// </summary>
        public int DataRowPosition { get; set; }

        /// <summary>
        /// Gets or sets a format for numbers in the data row.
        /// </summary>
        public string DataRowNumericFormat { get; set; }

        /// <summary>
        /// Gets all possible _values for attribute
        /// </summary>
        public IEnumerable<object> Values
        {
            get
            {
                return _values;
            }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Sets color for attribute's value.
        /// </summary>
        /// <param name="value">
        /// Attribute's value.
        /// </param>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        public void SetAttributeValueColor(object value, Color newColor)
        {
            string strValue = value == null ? string.Empty : value.ToString();
            Color defaultColor = GetDefaultColor(value);

            AttributeConfigurationChangeEventArgs args = new AttributeConfigurationChangeEventArgs(this, "Colors") { Color = newColor, Value = strValue };
            if (!Colors.ContainsKey(strValue))
            {
                if (newColor != defaultColor)
                {
                    Colors.Add(strValue, CustomColorConverter.ColorToString(newColor));
                    RaisePropertyChanged(this, "Colors", args);
                }
            }
            else
            {
                if (newColor != defaultColor)
                {
                    Colors[strValue] = CustomColorConverter.ColorToString(newColor);
                }
                else
                {
                    Colors.Remove(strValue);
                }

                RaisePropertyChanged(this, "Colors", args);
            }
        }

        /// <summary>
        /// Gets color for attribute's value.
        /// </summary>
        /// <param name="value">
        /// Attribute's value.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        public Color GetAttributeValueColor(object value)
        {
            string strValue = value == null ? string.Empty : value.ToString();
            if (Colors.ContainsKey(strValue))
            {
                return CustomColorConverter.StringToColor(Colors[strValue]);
            }

            return GetDefaultColor(value);
        }

        public bool IsDefaultColor(object value)
        {
            return this.GetAttributeValueColor(value) == this.GetDefaultColor(value);
        }

        /// <summary>
        /// Sets color for attribute's value.
        /// </summary>
        /// <param name="value">attribute's value.</param>
        /// <param name="isVisible">The _visibility.</param>
        public void SetAttributeValueVisibility(object value, bool isVisible)
        {
            // default value for visibility is true so there is no need to store true value
            string strValue = value == null ? string.Empty : value.ToString();
            if (!Visibility.ContainsKey(strValue))
            {
                if (!isVisible)
                {
                    Visibility.Add(strValue, isVisible);
                }
            }
            else
            {
                if (isVisible)
                {
                    Visibility.Remove(strValue);
                }
                else
                {
                    Visibility[strValue] = false;
                }
            }

            // HACK
            AdvancedPropertyChangedEventArgs eventArgs = new AdvancedPropertyChangedEventArgs(this, "Visibility", new AttributeConfigurationChangeEventArgs(this, "Visibility") { Value = strValue, Visibility = isVisible });
            RaisePropertyChanged(this, eventArgs);
        }

        public void SetAttributeAllValuesVisibility(bool isVisible)
        {
            if (isVisible)
            {
                Visibility.Clear();
            }
            else
            {
                foreach (string strValue in _values)
                {
                    Visibility[strValue] = false;
                }
            }
            
            AdvancedPropertyChangedEventArgs eventArgs = new AdvancedPropertyChangedEventArgs(
                this, 
                "AllVisibility", 
                new AttributeConfigurationChangeEventArgs(this, "AllVisibility") { Value = null, Visibility = isVisible });

            RaisePropertyChanged(this, eventArgs);
        }

        /// <summary>
        /// Gets color for attribute's value.
        /// </summary>
        /// <param name="value">
        /// Attribute's value.
        /// </param>
        /// <returns>
        /// The color.
        /// </returns>
        public bool GetAttributeValueVisibility(object value)
        {
            string strValue = value == null ? string.Empty : value.ToString();
            if (Visibility.ContainsKey(strValue))
            {
                return Visibility[strValue];
            }

            return true;
        }

        /// <summary>
        /// The initialize attribute value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void InitializeAttributeValue(string value)
        {
            if (!_values.Contains(value))
            {
                _values.Add(value);
            }
        }

        /// <summary>
        /// Returns type for given type name.
        /// </summary>
        /// <param name="typeName">
        /// Fully qualified class name.
        /// </param>
        /// <returns>
        /// The type.
        /// </returns>
        public Type GetTypeByName(string typeName)
        {
            AttributeType attributeType = SupportedTypes.FirstOrDefault(t => t.ClassName == typeName);
            if (attributeType == null)
            {
                return null;
            }

            if (!TypeCache.ContainsKey(typeName))
            {
                lock (SyncObject)
                {
                    if (!TypeCache.ContainsKey(typeName))
                    {
                        Type result = null;
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        foreach (Assembly assembly in assemblies)
                        {
                            Type foundType = assembly.GetType(attributeType.SystemType);
                            if (foundType != null)
                            {
                                result = foundType;
                            }
                        }
                        TypeCache.Add(typeName, result);
                    }
                }
            }

            return TypeCache[typeName];
        }

        /// <summary>
        /// Returns .net type for attribute.
        /// </summary>
        /// <returns>
        /// .net type instance.
        /// </returns>
        public Type GetAttributeType()
        {
            return GetTypeByName(_type);
        }

        /// <summary>
        /// The get extended configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="List{AttributeExtendedConfiguration}"/>.
        /// </returns>
        public List<AttributeExtendedConfiguration> GetExtendedConfiguration()
        {
            List<AttributeExtendedConfiguration> result = new List<AttributeExtendedConfiguration>();
            foreach (object value in Values)
            {
                string strValue = value == null ? string.Empty : value.ToString();
                if (Visibility.ContainsKey(strValue) || Colors.ContainsKey(strValue))
                {
                    result.Add(new AttributeExtendedConfiguration
                                   {
                                       Value = strValue,
                                       Color = Colors.ContainsKey(strValue) ? Colors[strValue] : CustomColorConverter.ColorToString(GetDefaultColor(value)),
                                       Visibility = !Visibility.ContainsKey(strValue)
                                   });
                }
            }

            return result;
        }

        /// <summary>
        /// The turn to default.
        /// </summary>
        public void TurnToDefault()
        {
            Colors.Clear();
            Visibility.Clear();
        }

        /// <summary>
        /// Order a list of _values by their underlying type.
        /// i.e. DateTime, Double, Integer, String
        /// </summary>
        /// <param name="descending">
        /// The descending.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable{string}</cref>
        ///     </see>
        ///     .
        /// </returns>
        public List<string> OrderByType(bool descending = false)
        {
            Type foundType = GetTypeByName(Type);

            IEnumerable<object> objectValues = _values.Select(this.GetTypedValue);

            IEnumerable<object> sortedValues = descending ? objectValues.OrderByDescending(obj => obj) : objectValues.OrderBy(obj => obj);

            return sortedValues.Select(obj => obj.ToString()).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetTypedValueStringMap()
        {
            return _values.ToDictionary(s => GetTypedValue(s).ToString(), s => s);
        }

        /// <summary>
        /// Formats given value based on current attribute type.
        /// </summary>
        /// <param name="value">value to be formated</param>
        /// <returns>formated value</returns>
        public string GetFormattedByTypeValue(string value) 
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            
            return this.GetTypedValue(value).ToString();
        }

        #endregion
        
        private object GetTypedValue(string value)
        {
            Type foundType = GetTypeByName(Type);
            try
            {
                return Convert.ChangeType(value, foundType, null);
            }
            catch (Exception)
            {
                return value;
            }
        }

        /// <summary>
        /// Checks if changing type is possible.
        /// </summary>
        /// <param name="newTypeName">
        /// Fully qualified type name.
        /// </param>
        /// <returns>
        /// True or false.
        /// </returns>
        private bool CanChangeType(string newTypeName)
        {
            try
            {
                Type foundType = GetTypeByName(newTypeName);
                foreach (string value in _values)
                {
                    // ReSharper disable ReturnValueOfPureMethodIsNotUsed
#if (SILVERLIGHT)
                    Convert.ChangeType(value, foundType, null);
#else
                    Convert.ChangeType(value, foundType);
#endif

                    // ReSharper restore ReturnValueOfPureMethodIsNotUsed
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The get default color.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="Color"/>.
        /// </returns>
        private Color GetDefaultColor(object value)
        {
            string strValue = (value == null) ? "null" : value.ToString();

            if (strValue.ToLower() == "true")
            {
                return System.Windows.Media.Colors.Green;
            }

            if (strValue.ToLower() == "false")
            {
                return System.Windows.Media.Colors.Red;
            }

            UTF8Encoding encoding = new UTF8Encoding();
            Crc32 crc32 = new Crc32();

            byte[] bytes = encoding.GetBytes(_name + strValue);
            byte[] hash = crc32.ComputeHash(bytes);

            return Color.FromArgb(255, hash[1], hash[2], hash[3]);
        }        
    }
}