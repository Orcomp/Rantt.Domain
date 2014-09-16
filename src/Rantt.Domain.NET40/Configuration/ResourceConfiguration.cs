//-----------------------------------------------------------------------
// <copyright file="ResourceConfiguration.cs" company="Orcomp">
//     Copyright (c) 2013 Orcomp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Rantt.Domain.Configuration
{
    using Catel.Data;

    /// <summary>
    /// The resource configuration.
    /// </summary>
    public class ResourceConfiguration : ModelBase
    {
        #region Fields                
        /// <summary>
        /// The name.
        /// </summary>
        private string _name;      
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfiguration" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="position">The position.</param>
        /// <param name="isVisible">The is visible.</param>
        public ResourceConfiguration(string name, int position, bool isVisible)
        {            
            _name = name;
            Position = position;
            IsVisible = isVisible;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConfiguration"/> class.
        /// </summary>
        public ResourceConfiguration()
        {
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether is visible.
        /// </summary>
        public bool IsVisible { get; set; }
       
        private void IsVisibleChanged()
        {
            
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets display name of resource.
        /// </summary>
        public string DisplayName 
        {
            get
            {
                return !string.IsNullOrEmpty(_name) ? _name : @"N/A";
            }
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public int Position { get; set; }                     
        #endregion 
       
        public override bool Equals(object obj)
        {
            // Do not want to be dependent from Fody here
            if ((obj as ResourceConfiguration) == null)
            {
                return false;
            }

            return this.Name.Equals((obj as ResourceConfiguration).Name);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}